using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface ICongVanService
    {
        Task<CongVan> Create(CongVan model);  
        Task<CongVan> Update(CongVan model);
        Task Delete(string id);
        Task<List<CongVan>> Get();
        Task<CongVan> GetById(string id);
        Task<PagingModel<CongVan>> GetPaging(CongVanParam param);
        Task<PagingModel<LuuCVDi>> GetPagingLuuCVDi(CongVanParam param);
        Task<PagingModel<LuuCVDen>> GetPagingLuuCVDen(CongVanParam param);
        Task<LuuCVDen> GetByIdLuuCVDen(string id);        
        Task<LuuCVDi> GetByIdLuuCVDi(string id);
    }
}