using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using SystemReport.WebAPI.Authorization;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Interfaces.BaseInterfaces;
using SystemReport.WebAPI.Interfaces.Identity;
using SystemReport.WebAPI.Models;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRoleService _roleService;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IJwtUtils _jwtUtils;
        private readonly ILoggingService _logger;
        private readonly IDbSettings _settings;
        private DataContext _context;

        public IdentityService(
            ILoggingService logger,
            IDbSettings settings,
            DataContext context,
            IUserService userService,
            IRoleService roleService,
            IRefreshTokenService refreshTokenService,
            JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters,
            IJwtUtils jwtUtils,
            IHttpContextAccessor httpContext
        ) : base(context, httpContext)
        {
            _context = context;
            _userService = userService;
            _roleService = roleService;
            _refreshTokenService = refreshTokenService;
            _jwtSettings = jwtSettings;
            _jwtUtils = jwtUtils;
            _tokenValidationParameters = tokenValidationParameters;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.UserCollectionName)
                .WithDatabaseName(_settings.DatabaseName);
        }

        public async Task<bool> CheckUser(string userName)
        {
            var existingUser = await _userService.FindUserWithUserNameOrPhoneNumber(userName);

            if (existingUser != default)
            {
                return true;
            }

            return false;
        }

        public async Task<AuthResponse> RegisterAsync(User userModel)
        {
            var createdUser = await _userService.Create(userModel);

            return await GenerateAuthenticationResultForUserAsync(createdUser);
        }

        public async Task<AuthResponse> LoginAsync(AuthRequest model)
        {
            var user = await Authenticate(model);
            if (!user.IsActived)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Tài khoản hiện tại đang bị khóa.");
            }
            else if (user.IsRequireVerify)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Tài khoản chưa được xác minh.");
            }

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthResponse> TokenAsync(AuthRequest model)
        {
            var usertemp = await _userService.FindUserWithUserNameOrPhoneNumber(model.UserName);
            if (usertemp == null)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy người dùng.");
            }

            var temp = await _refreshTokenService.GetByUserId(usertemp.Id);
            if (temp != null && !String.IsNullOrEmpty(temp.Token))
            {
                var validatedToken = GetPrincipalFromToken(temp.Token);

                if (validatedToken != null)
                {
                    var expiryDateUnix =
                        long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                    var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                        .AddSeconds(expiryDateUnix);

                    if (expiryDateTimeUtc > DateTime.UtcNow)
                    {
                        var userLogin = new UserLogin(usertemp);
                        return new AuthResponse(
                            userLogin,
                            temp.Token,
                            temp.JwtId,
                            temp.ExpiryDate
                        );
                    }
                }
            }

            var user = await Authenticate(model);

            if (user == null)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Tài khoản hoặc mật khẩu không chính xác!");
            }

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthResponse> RefreshTokenAsync(AuthRequest model)
        {
            var validatedToken = GetPrincipalFromToken(model.Token);

            if (validatedToken == null)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Invalid Token");
            }

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("This token hasn't expired yet");
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _refreshTokenService.GetByJwtId(model.RefreshToken);

            if (storedRefreshToken == null)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("This refresh token does not exist");
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("This refresh token has expired");
            }

            if (storedRefreshToken.Invalidated)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("This refresh token has been invalidated");
            }

            if (storedRefreshToken.Used)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("This refresh token has been used");
            }

            if (storedRefreshToken.JwtId != jti)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("This refresh token does not match this JWT");
            }

            storedRefreshToken.Used = true;
            await _refreshTokenService.Update(storedRefreshToken);

            var user = await _userService.GetById(validatedToken.Claims.Single(x => x.Type == "id").Value);
            var authtoken = await GenerateAuthenticationResultForUserAsync(user);
            storedRefreshToken.Token = authtoken.Token;
            await _refreshTokenService.Update(storedRefreshToken);
            return authtoken;
        }

        public async Task<User> Authenticate(AuthRequest model)
        {
            if (string.IsNullOrEmpty(model.UserName))
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Tài khoản không được trống!");
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Mật khẩu không được để trống");
            }
            
            var user = _context.Users.AsQueryable().Where(x =>
                (x.UserName.ToLower() == model.UserName.Trim().ToLower()) &&
                !x.IsAppAuthentication).FirstOrDefault();
            if (user == null)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy tài khoản.");

            if (!PasswordExtensions.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Mật khẩu không chính xác.");
            // var jwtToken = _jwtUtils.GenerateJwtToken(user);
            var permissions = await _roleService.GetPermissionForCurrentUer(user.UserName);
            var menu = await _roleService.GetMenuForUser(user.UserName);
            user.Permissions = permissions;
            user.Menu = menu;
            // var userModel = new UserLogin(user.Id, user.UserName, user.FullName, user.PhoneNumber, user.Email,
            //     permissions, user.Roles, menu, user.Avatar);
            return user;
        }

        private async Task<AuthResponse> GenerateAuthenticationResultForUserAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim("id", user.Id)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                Token = tokenHandler.WriteToken(token),
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.Add(_jwtSettings.TokenRefreshStore)
            };

            var createdRefreshToken = await _refreshTokenService.Create(refreshToken);

            var userLogin = new UserLogin(user);

            return new AuthResponse(userLogin,
                tokenHandler.WriteToken(token),
                refreshToken.JwtId,
                tokenDescriptor.Expires.HasValue ? tokenDescriptor.Expires.Value : refreshToken.ExpiryDate
            );
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }


        public async Task<AuthResponsePaygov> GetTokenOthers(AuthRequest model)
        {

            var usertemp = await _userService.FindUserWithUserNameOrPhoneNumber(model.UserName);
            if (usertemp == null)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy người dùng.");
            }

            var temp = await _refreshTokenService.GetByUserId(usertemp.Id);
            if (temp != null && !String.IsNullOrEmpty(temp.Token))
            {
                var validatedToken = GetPrincipalFromToken(temp.Token);

                if (validatedToken != null)
                {
                    var expiryDateUnix =
                        long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                    var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                        .AddSeconds(expiryDateUnix);

                    if (expiryDateTimeUtc > DateTime.UtcNow)
                    {
                        var userLogin = new UserLogin(usertemp);
                        return new AuthResponsePaygov(userLogin, temp.Token, temp.JwtId, temp.ExpiryDate)
                        {
                            expires_in = (int) (temp.ExpiryDate - DateTime.UtcNow).TotalSeconds
                        };
                    }
                }
            }
            var user = await Authenticate(model);

            return await GenerateAuthenticationResultForUserAsyncOrthers(user);
        }
        
        public async Task<AuthResponsePaygov> GetTokenPaygov(AuthRequest model)
        {

            var usertemp = await _userService.FindUserWithUserNameOrPhoneNumber(model.UserName);
            if (usertemp == null)
            {
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy người dùng.");
            }

            var temp = await _refreshTokenService.GetByUserId(usertemp.Id);
            if (temp != null && !String.IsNullOrEmpty(temp.Token))
            {
                var validatedToken = GetPrincipalFromToken(temp.Token);

                if (validatedToken != null)
                {
                    var expiryDateUnix =
                        long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                    var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                        .AddSeconds(expiryDateUnix);

                    if (expiryDateTimeUtc > DateTime.UtcNow)
                    {
                        var userLogin = new UserLogin(usertemp);
                        return new AuthResponsePaygov(userLogin, temp.Token, temp.JwtId, temp.ExpiryDate)
                        {
                            expires_in = (int) (temp.ExpiryDate - DateTime.UtcNow).TotalSeconds
                        };
                    }
                }
            }
            var user = await Authenticate(model);

            return await GenerateAuthenticationResultForUserAsyncOrthers(user);
        }
         
        private async Task<AuthResponsePaygov> GenerateAuthenticationResultForUserAsyncOrthers(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim("id", user.Id)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.OthersTokenLife),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                Token = tokenHandler.WriteToken(token),
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.Add(_jwtSettings.OthersTokenLife)
            };

            var createdRefreshToken = await _refreshTokenService.Create(refreshToken);
            // string strPageRole = _userManager.GetSringTotalPageRole(user);
            var userLogin = new UserLogin(user);
            return new AuthResponsePaygov(userLogin, tokenHandler.WriteToken(token), createdRefreshToken.JwtId,  tokenDescriptor.Expires.HasValue ? tokenDescriptor.Expires.Value : refreshToken.ExpiryDate)
            {
                expires_in = (int)_jwtSettings.PaygovTokenLife.TotalSeconds
            };
        }
    }
}