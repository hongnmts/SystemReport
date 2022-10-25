using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;
using ClosedXML.Excel;
using SystemReport.WebAPI.Services;

namespace SystemReport.WebAPI.APIs
{
    [Route("api/v1/[controller]")]
    public class ExportController : ControllerBase
    {
        private IExportExcelService _exportExcelService;
        private IRowValueService _rowValueService;
        private IBangBieuService _bangBieuService;
        private IDonViService _donViService;

        public ExportController(IExportExcelService exportExcelService, IRowValueService rowValueService,
            IBangBieuService bangBieuService,
            IDonViService donViService)
        {
            _exportExcelService = exportExcelService;

            _rowValueService = rowValueService;
            _bangBieuService = bangBieuService;
            _donViService = donViService;
        }

        [HttpGet]
        [Route("excel/bangbieu/{id}")]
        public IActionResult ExportExcelBangBieu(string id)
        {
            var header = _bangBieuService.RenderHeader(id).Result;

            MemoryStream stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sample Sheet");
                int row = 1;
                foreach (var hd in header)
                {
                    int col = 1;
                    foreach (var td in hd.THeaderVms)
                    {
                        worksheet.Cell(row, col).Value = td.TenThuocTinh;
                        if(td.RowSpan > 1)
                        {
                            worksheet.Range(worksheet.Cell(row , col), worksheet.Cell(row + td.RowSpan, col)).Merge();
                        }
                        if (td.ColSpan > 1)
                        {
                            worksheet.Range(worksheet.Cell(row, col), worksheet.Cell(row , col + td.ColSpan)).Merge();
                        }
                        col++;
                    }
                    break;
                    row++;
                }
                workbook.SaveAs(stream);

                stream.Seek(0, SeekOrigin.Begin);

                //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                //result.Content = new ByteArrayContent(stream.ToArray());
                //result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                //result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                //result.Content.Headers.ContentDisposition.FileName = "ERSheet.xlsx";
                //return result;

                stream.Position = 0;

                return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = "example.xlsx" };
            }


        }
    }
}
