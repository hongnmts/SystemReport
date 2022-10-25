using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces.BaseInterfaces;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.APIs.Identity
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class TokenController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public TokenController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> GetToken([FromHeader] string Authorization, [FromForm] string grant_type)
        {
            if (String.IsNullOrEmpty(Authorization))
            {
                return Unauthorized();
            }

            if (String.IsNullOrEmpty(grant_type))
            {
                return Unauthorized(new TokenResultFail
                {
                    error = "invalid_grant",
                    error_description = "Missing grant_type!"
                });
            }

            if (grant_type != "client_credentials")
            {
                return Unauthorized(new TokenResultFail
                {
                    error = "invalid_grant",
                    error_description = "Wrong grant_type!"
                });
            }

            try
            {
                string[] temp = Authorization.Split(" ");
                if (temp[0] == "Basic" && !String.IsNullOrEmpty(temp[1]))
                {
                    string[] tempdecode = StringExtensions.Base64Decode(temp[1]).Split(":");
                    if (tempdecode != null && tempdecode.Length > 0)
                    {
                        string user = StringExtensions.Base64Decode(tempdecode[0]);
                        string pass = StringExtensions.Base64Decode(tempdecode[1]);
                        var model = new AuthRequest() {UserName = user, Password = pass};
                        var response = await _identityService.GetTokenOthers(model);

                        return Ok(
                            new ResultResponse<AuthResponse>()
                                .WithData(response)
                                .WithCode(EResultResponse.SUCCESS.ToString())
                                .WithMessage("Get token success")
                        );
                    }
                }
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }

            return Unauthorized();
        }


        [HttpPost]
        [Route("tokenl")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> GetTokenL([FromHeader] string Authorization, [FromForm] string grant_type)
        {
            if (String.IsNullOrEmpty(Authorization))
            {
                return Unauthorized();
            }

            if (String.IsNullOrEmpty(grant_type))
            {
                return Unauthorized(new TokenResultFail
                {
                    error = "invalid_grant",
                    error_description = "Missing grant_type!"
                });
            }

            if (grant_type != "client_credentials")
            {
                return Unauthorized(new TokenResultFail
                {
                    error = "invalid_grant",
                    error_description = "Wrong grant_type!"
                });
            }

            try
            {
                string[] temp = Authorization.Split(" ");
                if (temp[0] == "Basic" && !String.IsNullOrEmpty(temp[1]))
                {
                    string[] tempdecode = StringExtensions.Base64Decode(temp[1]).Split(":");
                    if (tempdecode != null && tempdecode.Length > 0)
                    {
                        string user = StringExtensions.Base64Decode(tempdecode[0]);
                        string pass = StringExtensions.Base64Decode(tempdecode[1]);
                        var model = new AuthRequest() {UserName = user, Password = pass};
                        var response = await _identityService.GetTokenOthers(model);

                        return Ok(
                            new ResultResponse<AuthResponse>()
                                .WithData(response)
                                .WithCode(EResultResponse.SUCCESS.ToString())
                                .WithMessage("Get token success")
                        );
                    }
                }
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }

            return Unauthorized();
        }
    }
}