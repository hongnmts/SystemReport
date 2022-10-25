using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using Microsoft.AspNetCore.Mvc;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    // [Authorize]
    public class LoaiTrangThaiController : ControllerBase
    {
        private ILoaiTrangThaiService _loaiTrangThaiService;

        public LoaiTrangThaiController(ILoaiTrangThaiService loaiTrangThaiService)
        {
            _loaiTrangThaiService = loaiTrangThaiService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] LoaiTrangThai model)
        {
            try
            {
                var response = await _loaiTrangThaiService.Create(model);

                return Ok(
                    new ResultResponse<LoaiTrangThai>()
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
        public async Task<IActionResult> Update([FromBody] LoaiTrangThai model)
        {
            try
            {
                var response = await _loaiTrangThaiService.Update(model);

                return Ok(
                    new ResultResponse<LoaiTrangThai>()
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
                await _loaiTrangThaiService.Delete(id);

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
                var response = await _loaiTrangThaiService.GetById(id);

                return Ok(
                    new ResultResponse<LoaiTrangThai>()
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
                var response = await _loaiTrangThaiService.Get();

                return Ok(
                    new ResultResponse<List<LoaiTrangThai>>()
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
        public async Task<IActionResult> GetPagingParam([FromBody] LinhVucParam param)
        {
            try
            {
                var response = await _loaiTrangThaiService.GetPaging(param);

                return Ok(
                    new ResultResponse<PagingModel<LoaiTrangThai>>()
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
    }
}