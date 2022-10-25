using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IChucVuService
    {
        Task<ChucVu> Create(ChucVu model);
        Task<ChucVu> Update(ChucVu model);
        Task Delete(string id);
        Task<List<ChucVu>> Get();
        Task<List<ChucVu>> GetAll();
        Task<ChucVu> GetById(string id);
        Task<PagingModel<ChucVu>> GetPaging(LinhVucParam param);
    }
}