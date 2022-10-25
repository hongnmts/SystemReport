using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class RowValueController : ControllerBase
    {
        private IRowValueService _rowValueService;

        public RowValueController(IRowValueService rowValueService)
        {
            _rowValueService = rowValueService;
        }

        [HttpPost]
        [Route("create-sub")]
        public async Task<IActionResult> CreateSub([FromBody] List<RowValue> model)
        {
            try
            {
                var response = await _rowValueService.CreateSub(model);
                return Ok(
                    new ResultResponse<dynamic>()
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
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] List<RowValue> model)
        {
            try
            {
                var response = await _rowValueService.Create(model);
                return Ok(
                    new ResultResponse<dynamic>()
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
            catch (Exception ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage(ex.Message)
                );
            }
        }

        [HttpPost]
        [Route("delete-rowvalue")]
        public async Task<IActionResult> DeleteRowValue([FromBody] List<RowValue> model)
        {
            try
            {
                var response = await _rowValueService.DeleteRowValue(model);
                return Ok(
                    new ResultResponse<dynamic>()
                        .WithData(response)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage("Xóa thành công!")
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
        public async Task<IActionResult> Update([FromBody] RowValue model)
        {
            try
            {
                var response = await _rowValueService.Update(model);

                return Ok(
                    new ResultResponse<dynamic>()
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
                await _rowValueService.Delete(id);

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
                var response = await _rowValueService.GetById(id);

                return Ok(
                    new ResultResponse<dynamic>()
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
        [Route("get-tree-row-value-by-bang-bieu-id/{id}")]
        public async Task<IActionResult> GetTreeThuocTinhByBangBieuId(string id)
        {
            try
            {
                var response = await _rowValueService.GetTreeByBangBieuId(id);

                return Ok(
                    new ResultResponse<dynamic>()
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
        [Route("render-body-by-bang-bieu-id/{id}")]
        public async Task<IActionResult> RenderBodyByBangBieuId(string id)
        {
            try
            {
                var response = await _rowValueService.RenderBodyByBangBieuId(id);

                return Ok(
                    new ResultResponse<dynamic>()
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
        [Route("render-body-main-by-bang-bieu-id/{id}")]
        public async Task<IActionResult> RenderBodyMainByBangBieuId(string id)
        {
            try
            {
                var response = await _rowValueService.RenderBodyMainByBangBieuId(id);

                return Ok(
                    new ResultResponse<dynamic>()
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
        [Route("get-parent-row-value-by-bang-bieu-id/{id}")]
        public async Task<IActionResult> GetParentRowValueByBangBieuId(string id)
        {
            try
            {
                var response = await _rowValueService.GetTreeParentRowValue(id);

                return Ok(
                    new ResultResponse<dynamic>()
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
        [Route("get-row-value-by-key-row/{id}")]
        public async Task<IActionResult> GetRowValueByKeyRow(string id)
        {
            try
            {
                var response = _rowValueService.GetRowValueByKeyRow(id);

                return Ok(
                    new ResultResponse<dynamic>()
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
        [Route("add-row-tong/{id}")]
        public async Task<IActionResult> AddRowTong(string id)
        {
            try
            {
                await _rowValueService.AddRowTong(id); 

                return Ok(
                    new ResultResponse<dynamic>()
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage("Thêm tính tổng thành công!")
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