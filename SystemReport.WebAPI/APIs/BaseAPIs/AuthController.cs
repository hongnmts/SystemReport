using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces.BaseInterfaces;
using SystemReport.WebAPI.Models;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.APIs.Identity
{
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly AppApiKey _appApiKey;
        public AuthController(IIdentityService identityService, AppApiKey appApiKey)
        {
            _identityService = identityService;
            _appApiKey = appApiKey;
        }

        // [HttpPost]
        // [Route("login")]
        // [AllowAnonymous]
        // public async Task<IActionResult> Login([FromHeader] string clientid, [FromHeader] string secretkey, [FromBody] AuthRequest user)
        // {
        //     try
        //     {
        //         if (!_appApiKey.SecretKey.Equals(secretkey) || !_appApiKey.ClientId.Equals(clientid))
        //         {
        //             return Unauthorized();
        //         }
        //         
        //         var response = await _identityService.LoginAsync(user);
        //
        //         return Ok(
        //             new ResultResponse<AuthResponse>()
        //                 .WithData(response)
        //                 .WithCode(EResultResponse.SUCCESS.ToString())
        //                 .WithMessage("Đăng nhập thành công")
        //         );
        //     }
        //     catch (ResponseMessageException ex)
        //     {
        //         return Ok(
        //             new ResultMessageResponse().WithCode(ex.ResultCode)
        //                 .WithMessage(ex.ResultString)
        //         );
        //     }
        // }



        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthRequest user)
        {
            try
            {
                var response = await _identityService.LoginAsync(user);

                return Ok(
                    new ResultResponse<AuthResponse>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage("Đăng nhập thành công")
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }
        }










        [HttpPost("register")]
        public async Task<IActionResult> Register([FromHeader] string clientid, [FromHeader] string secretkey, [FromBody] User model)
        {
            try
            {
                if (!_appApiKey.SecretKey.Equals(secretkey) || !_appApiKey.ClientId.Equals(clientid))
                {
                    return Unauthorized();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResultMessageResponse().WithCode(EResultResponse.FAIL.ToString())
                        .WithErrors(ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)))
                    );
                }

                var response = await _identityService.RegisterAsync(model);

                return Ok(
                    new ResultResponse<AuthResponse>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage("Đăng ký thành công")
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> Refresh([FromHeader] string clientid, [FromHeader] string secretkey, [FromBody] AuthRequest request)
        {
            try
            {
                if (!_appApiKey.SecretKey.Equals(secretkey) || !_appApiKey.ClientId.Equals(clientid))
                {
                    return Unauthorized();
                }
                var response = await _identityService.RefreshTokenAsync(request);

                return Ok(
                    new ResultResponse<AuthResponse>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage("Refresh token success")
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }
        }
    }
}