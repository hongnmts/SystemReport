using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Services;
using SystemReport.WebAPI.ViewModels;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private IThongKeService _thongKeService;
        public ThongKeController(IThongKeService thongKeService)
        {
            _thongKeService = thongKeService;
        }

        [HttpGet]
        [Route("get-tieu-chi/{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var response =  _thongKeService.GetTieuChiTongKe(id);

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

        [HttpPost]
        [Route("get-bangbieu")]
        public async Task<IActionResult> GetBangBieu([FromBody] List<TieuChiVM> models)
        {
            try
            {
                var response = await _thongKeService.FilterBangBieu(models, "");

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
    }
}
