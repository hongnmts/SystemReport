using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IThuocTinhService
    {
        Task<ThuocTinh> Create(ThuocTinh model);
        Task<ThuocTinh> Update(ThuocTinh model);
        Task Delete(string id);
        Task<List<ThuocTinh>> Get();
        Task<ThuocTinh> GetById(string id);
        Task<PagingModel<ThuocTinh>> GetPaging(PagingParam param);
        Task<List<ThuocTinh>> GetThuocTinhByBangBieuId(string bangBieuId);
        Task<List<ThuocTinhTreeVM>> GetTreeByBangBieuId(string bangBieuId);
        Task<List<ThuocTinhListTreeVM>> GetListTreeByBangBieuId(string bangBieuId);
        List<ThuocTinh> GetThuocTinhChiTieu(string bangBieuId);
        List<ThuocTinh> GetNodeLeftChiTieu(string bangBieuId);
    }
}