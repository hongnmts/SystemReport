using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IVanBanDenService
    {
        Task<VanBanDen> Create(VanBanDen model);
        Task<VanBanDen> Update(VanBanDen model);
        Task<VanBanDen> ButPhe(ButPhe model);
        Task<VanBanDen> PhanCong(List<PhanCong> model);
        Task Delete(string id);
        Task<List<VanBanDen>> Get();
        Task<VanBanDen> GetById(string id);
        Task<PagingModel<VanBanDen>> GetPaging(VanBanDenParam param);
        Task<PagingModel<VanBanDen>> GetPagingXuLy(VanBanDenParam param);
        Task ChuyenTrangThaiVanBan(TrangThaiParam model);
    }
}