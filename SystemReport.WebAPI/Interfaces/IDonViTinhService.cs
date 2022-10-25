using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IDonViTinhService
    {
        Task<DonViTinh> Create(DonViTinh model);
        Task<DonViTinh> Update(DonViTinh model);
        Task Delete(string id);
        Task<List<DonViTinh>> Get();
        Task<DonViTinh> GetById(string id);
        Task<PagingModel<DonViTinh>> GetPaging(PagingParam param);
    }
}