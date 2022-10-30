using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IHomeService
    {
        HomeValue GetHomeValue();
        Task<PagingModel<MauBieu>> GetDanhSachMauBieu(MauBieuParam param);
    }
}
