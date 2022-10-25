using System.Threading.Tasks;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IExportExcelService
    {
        Task ExportBangBieu(string bangBieuId);
    }
}
