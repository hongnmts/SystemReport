using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IHoSoDonViService
    {
        Task<HoSoDonVi> Create(HoSoDonVi model);
        Task<HoSoDonVi> Update(HoSoDonVi model);
        Task Delete(string id);
        Task<List<HoSoDonVi>> Get();
        Task<HoSoDonVi> GetById(string id);
        Task<PagingModel<HoSoDonVi>> GetPaging(PagingParam param);
    }
}