using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text;
using SystemReport.WebAPI.Authorization;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Interfaces.BaseInterfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Services;


namespace SystemReport.WebAPI.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);

            // services.Configure<SignatureDigitalApiKey>(configuration.GetSection("SignatureDigitalApiKey"));
            // var signatureDigitalApiKey = new SignatureDigitalApiKey();
            // configuration.Bind(nameof(signatureDigitalApiKey), signatureDigitalApiKey);


            services.AddSingleton(jwtSettings);
            // services.AddSingleton(signatureDigitalApiKey);
            services.AddSingleton<IFileProvider>(
             new PhysicalFileProvider(
                 Path.Combine(Directory.GetCurrentDirectory(), "Files")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSingleton<IJwtUtils, JwtUtils>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var appApiKey = new AppApiKey();
            configuration.Bind(nameof(appApiKey), appApiKey);
            services.AddSingleton(appApiKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async (context) =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        //var data = context.HttpContext.RequestServices.ge<IHttpContextAccessor>();
                        var userId = context.Principal.GetLoggedInUserId();
                        var user = await userService.GetById(userId);

                        //data.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(context.Principal);
                        if (user == null)
                        {

                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");

                        }
                        else
                        {
                            context.HttpContext.Items["User"] = user;
                        }
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            });
        }
    }
}