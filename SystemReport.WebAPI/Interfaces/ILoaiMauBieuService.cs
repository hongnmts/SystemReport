using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface ILoaiMauBieuService
    {
        Task<LoaiMauBieu> Create(LoaiMauBieu model);
        Task<LoaiMauBieu> Update(LoaiMauBieu model);
        Task Delete(string id);
        Task<IEnumerable<LoaiMauBieu>> Get();
        Task<LoaiMauBieu> GetById(string id);
        Task<PagingModel<LoaiMauBieu>> GetPaging(PagingParam param);
        Task<List<LoaiMauBieuTreeVM>> GetTree();
        Task<IEnumerable<LoaiMauBieu>> GetCountLoaiBaoCao();
    }
}