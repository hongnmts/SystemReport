using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.APIs
{

    [Route("api/v1/[controller]")]
    // [Authorize]
    public class KhoiCoQuanController : ControllerBase
    {
        private IKhoiCoQuanService _khoiCoQuanService;

        public KhoiCoQuanController(IKhoiCoQuanService khoiCoQuanService)
        {
            _khoiCoQuanService = khoiCoQuanService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] KhoiCoQuan model)
        {
            try
            {
                var response = await _khoiCoQuanService.Create(model);
                return Ok(
                    new ResultResponse<KhoiCoQuan>()
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
        public async Task<IActionResult> Update([FromBody] KhoiCoQuan model)
        {
            try
            {
                var response = await _khoiCoQuanService.Update(model);

                return Ok(
                    new ResultResponse<KhoiCoQuan>()
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
                await _khoiCoQuanService.Delete(id);

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
                var response = await _khoiCoQuanService.GetById(id);

                return Ok(
                    new ResultResponse<KhoiCoQuan>()
                        .WithData(response)
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
        [Route("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _khoiCoQuanService.Get();

                return Ok(
                    new ResultResponse<List<KhoiCoQuan>>()
                        .WithData(response)
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
                var response = await _khoiCoQuanService.GetPaging(param);

                return Ok(
                    new ResultResponse<PagingModel<KhoiCoQuan>>()
                        .WithData(response)
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
        [Route("import-KhoiCoQuan")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportKhoiCoQuan()
        {
            try
            {
                await _khoiCoQuanService.ReadDataKhoiCoQuan(@"OldData/KhoiCoQuan.xlsx");
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

        [HttpPost]
        [Route("import-CoQuan")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportCoQuan()
        {
            try
            {
                await _khoiCoQuanService.ReadDataCoQuan(@"OldData/CoQuan.xlsx");
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

        [HttpPost]
        [Route("import-chucvu")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportChucVu()
        {
            try
            {
                await _khoiCoQuanService.ReadDataChucVu(@"OldData/ChucVu.xlsx");
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
    }
}