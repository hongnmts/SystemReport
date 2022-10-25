using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IDMHanhChinhService
    {
        Task CreateMultiHuyen(List<Huyen> list);
        Task CreateMultiXa(List<Xa> list);

        Task<Huyen> CreateHuyen(Huyen model);
        Task<Huyen> UpdateHuyen(Huyen model);
        Task DeleteHuyen(string id);
        Task<List<Huyen>> GetHuyen();
        Task<Huyen> GetByIdHuyen(string id);
        Task<PagingModel<Huyen>> GetPagingHuyen(PagingParam param);
        Task<Xa> CreateXa(Xa model);
        Task<Xa> UpdateXa(Xa model);
        Task DeleteXa(string id);
        Task<List<Xa>> GetXa();
        Task<Xa> GetByIdXa(string id);
        Task<PagingModel<Xa>> GetPagingXa(ChildPaging param);
    }
}