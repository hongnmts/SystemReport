using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IHoTroDoanhNghiepService
    {
        Task<HoTroDN> Create(HoTroDN model);
        Task<HoTroDN> Update(HoTroDN model);
        Task Delete(string id);
        Task<List<HoTroDN>> Get();
        Task<HoTroDN> GetById(string id);
        Task<PagingModel<HoTroDN>> GetPaging(PagingParam param);
        Task<PagingModel<HoTroDN>> GetPaging(HoTroDNVM param);
    }
}
