using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Services;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    public class HistoryVanBanController : ControllerBase
    {
        private HistoryVanBanDiService _historyVanBanDi;
        public HistoryVanBanController(HistoryVanBanDiService historyVanBanDi)
        {
            _historyVanBanDi = historyVanBanDi;
        }
        
        [HttpGet]
        [Route("get-van-ban-di-id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var response = await _historyVanBanDi.GetHistoryByQuestionId(id);

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