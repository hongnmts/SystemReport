using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Interfaces;

namespace SystemReport.WebAPI.Services
{
    public class ExportExcelService : BaseService, IExportExcelService
    {
        private DataContext _context;
        private IDbSettings _settings;
        private ILoggingService _logger;
        private IRowValueService _rowValueService;
        private IBangBieuService _bangBieuService;
        private IDonViService _donViService;

        public ExportExcelService(ILoggingService logger, IDbSettings settings, DataContext context,
            IRowValueService rowValueService,
            IBangBieuService bangBieuService,
            IDonViService donViService,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.MauBieuCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);

            _rowValueService = rowValueService;
            _bangBieuService = bangBieuService;
            _donViService = donViService;
        }

        public async Task ExportBangBieu(string bangBieuId)
        {
            var header = await _bangBieuService.RenderHeader(bangBieuId);

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

                        col++;
                    }
                    row++;
                }
                workbook.SaveAs("HelloWorld.xlsx");
            }
        }
    }
}
