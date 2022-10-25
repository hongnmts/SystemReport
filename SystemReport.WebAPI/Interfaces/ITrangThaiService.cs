using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface ITrangThaiService
    {
        Task<TrangThai> Create(TrangThai model);
        Task<TrangThai> Update(TrangThai model);
        Task Delete(string id);
        Task<TrangThai> GetById(string id);
        Task<PagingModel<TrangThai>> GetPaging(TrangThaiParam param);
        Task<List<TrangThai>> GetAll();
        Task<List<TrangThaiTreeVM>> GetTree();
        Task<List<TrangThaiShort>> GetNextTrangThai(TrangThaiParam param);
    }
}