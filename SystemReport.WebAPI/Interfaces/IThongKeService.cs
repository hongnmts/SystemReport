using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IThongKeService
    {
        TieuChiThongKeVM GetTieuChiTongKe(string bangBieuId);
        Task<DemoMauBieuVM> FilterBangBieu(List<TieuChiVM> models, string bangBieuId);
    }
}
