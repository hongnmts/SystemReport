using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.APIs.Identity
{
    [Route("api/v1/[controller]")]
    // [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] User model)
        {
            try
            {
                var response = await _userService.Create(model);

                return Ok(
                    new ResultResponse<User>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.CREATE_SUCCESS)
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
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] User model)
        {
            try
            {
                var response = await _userService.Update(model);

                return Ok(
                    new ResultResponse<User>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.UPDATE_SUCCESS)
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
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userService.Delete(id);

                return Ok(
                    new ResultMessageResponse()
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.DELETE_SUCCESS)
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

        [HttpGet]
        [Route("get-by-id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var data = await _userService.GetById(id);

                return Ok(
                    new ResultResponse<User>()
                        .WithData(data)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
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
        
        [HttpGet]
        [Route("get-user-by-id-don-vi/{id}")]
        public async Task<IActionResult> GetUserByIdDonVi(string id)
        {
            try
            {
                var data = await _userService.GetUserByIdDonVi(id);

                return Ok(
                    new ResultResponse<User>()
                        .WithData(data)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
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

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _userService.Get();

                return Ok(
                    new ResultResponse<IEnumerable<User>>()
                        .WithData(data)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
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
        [Route("get-paging-params")]
        public async Task<IActionResult> GetPagingParam([FromBody] PagingParam param)
        {
            try
            {
                var data = await _userService.GetPaging(param);

                return Ok(
                    new ResultResponse<PagingModel<User>>()
                        .WithData(data)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
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
        [Route("change-profile")]
        public async Task<IActionResult> ChangeProfile([FromBody] User model)
        {
            try
            {
                var data = await _userService.ChangeProfile(model);

                return Ok(
                    new ResultResponse<User>()
                        .WithData(data)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage("Cập nhật thông tin thành công.")
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
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserVM model)
        {
            try
            {
                var data = await _userService.ChangePassword(model);

                return Ok(
                    new ResultResponse<User>()
                        .WithData(data)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage("Đổi mật khẩu thành công.")
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
        [Route("import-user")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportCoQuan()
        {
            try
            {
                await _userService.ReadDataUser(@"OldData/ds-vien-chuc-SystemReport_new.xlsx");
                return Ok(
                    new { }

                );
            }
            catch (Exception ex)
            {
                return new ObjectResult(
                    new { }
                );
            }
        }
        
        [HttpGet]
        [Route("get-user-tree-for-donvi")]
        public async Task<IActionResult> UserTreeForDonVi()
        {
            try
            {
                var data = await _userService.UserTreeForDonVi();

                return Ok(
                    new ResultResponse<dynamic>()
                        .WithData(data)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
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
        [Route("update-signature")]
        public async Task<IActionResult> UpdateSignature([FromBody] SignatureSaveVM model)
        {
            try
            {
                await _userService.UpdateSignature(model);

                if (model.Action == "DELETE")
                {
                    return Ok(
                        new ResultMessageResponse()
                            .WithCode(EResultResponse.SUCCESS.ToString())
                            .WithMessage("Xóa chữ ký thành công!")
                    );
                }
                return Ok(
                    new ResultMessageResponse()
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage("Thêm chữ ký mới thành công!")
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
        [HttpGet]
        [Route("get-signature/{id}")]
        public async Task<IActionResult> GetSignature(string id)
        {
            try
            {
              var data =  await _userService.GetSignature(id);

              
              return Ok(
                  new ResultResponse<List<SignatureSave>>().WithData(data)
                      .WithCode(EResultResponse.SUCCESS.ToString())
                      .WithMessage("Lấy dữ liệu thành công")
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