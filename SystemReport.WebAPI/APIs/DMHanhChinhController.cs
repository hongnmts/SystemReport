using System.Threading.Tasks;
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
    public class DMHanhChinhController : ControllerBase
    {
        private IDMHanhChinhService _dmHanhChinhService;

        public DMHanhChinhController(IDMHanhChinhService dmHanhChinhService)
        {
            _dmHanhChinhService = dmHanhChinhService;
        }

        #region Huyen

        [HttpPost]
        [Route("create-huyen")]
        public async Task<IActionResult> CreateHuyen([FromBody] Huyen model)
        {
            try
            {
                var response = await _dmHanhChinhService.CreateHuyen(model);

                return Ok(
                    new ResultResponse<Huyen>()
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
        [Route("update-huyen")]
        public async Task<IActionResult> UpdateHuyen([FromBody] Huyen model)
        {
            try
            {
                var response = await _dmHanhChinhService.UpdateHuyen(model);

                return Ok(
                    new ResultResponse<Huyen>()
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
        [Route("delete-huyen/{id}")]
        public async Task<IActionResult> DeleteHuyen(string id)
        {
            try
            {
                await _dmHanhChinhService.DeleteHuyen(id);

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
        [Route("get-by-id-huyen/{id}")]
        public async Task<IActionResult> GetByIdHuyen(string id)
        {
            try
            {
                var response = await _dmHanhChinhService.GetByIdHuyen(id);

                return Ok(
                    new ResultResponse<Huyen>()
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
        [Route("get-paging-params-huyen")]
        public async Task<IActionResult> GetPagingParamHuyen([FromBody] PagingParam param)
        {
            try
            {
                var response = await _dmHanhChinhService.GetPagingHuyen(param);

                return Ok(
                    new ResultResponse<PagingModel<Huyen>>()
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

        #endregion

        #region Xa

        [HttpPost]
        [Route("create-xa")]
        public async Task<IActionResult> CreateXa([FromBody] Xa model)
        {
            try
            {
                var response = await _dmHanhChinhService.CreateXa(model);

                return Ok(
                    new ResultResponse<Xa>()
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
        [Route("update-xa")]
        public async Task<IActionResult> UpdateXa([FromBody] Xa model)
        {
            try
            {
                var response = await _dmHanhChinhService.UpdateXa(model);

                return Ok(
                    new ResultResponse<Xa>()
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
        [Route("delete-xa/{id}")]
        public async Task<IActionResult> DeleteXa(string id)
        {
            try
            {
                await _dmHanhChinhService.DeleteXa(id);

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
        [Route("get-by-id-xa/{id}")]
        public async Task<IActionResult> GetByIdXa(string id)
        {
            try
            {
                var response = await _dmHanhChinhService.GetByIdXa(id);
    
                return Ok(
                    new ResultResponse<Xa>()
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
        [Route("get-paging-params-xa")]
        public async Task<IActionResult> GetPagingParamXa([FromBody]ChildPaging param)
        {
            try
            {
                var response = await _dmHanhChinhService.GetPagingXa(param);
                return Ok(
                    new ResultResponse<PagingModel<Xa>>()
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

        #endregion
    }
}