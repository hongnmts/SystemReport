using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface ILoaiVanBanService
    {
        Task<LoaiVanBan> Create(LoaiVanBan model);
        Task<LoaiVanBan> Update(LoaiVanBan model);
        Task Delete(string id);
        Task<IEnumerable<LoaiVanBan>> Get();
        Task<LoaiVanBan> GetById(string id);
        Task<PagingModel<LoaiVanBan>> GetPaging(PagingParam param);
        Task<List<LoaiVanBanTreeVM>> GetTree();
    }
}