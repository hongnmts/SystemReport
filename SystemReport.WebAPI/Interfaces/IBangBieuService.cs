using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IBangBieuService
    {
        Task<BangBieu> Create(BangBieu model);
        Task<BangBieu> Update(BangBieu model);
        Task Delete(string id);
        Task<List<BangBieu>> Get();
        Task<BangBieu> GetById(string id);
        Task<PagingModel<BangBieu>> GetPaging(BangBieuParam param);
        Task<List<BangBieu>> GetBangBieuByMauBieuId(string mauBieuId);
        Task<List<HeaderTableVM>> RenderHeader(string bangBieuId);
        Task<List<HeaderTableVM>> RenderHeader(List<ThuocTinh> thuocTinhs);
        Task<RenderTableNhapLieu> RenderNhapLieuBangBieu(string bangBieuId);
        Task SaveDataBangBieu(List<BodyTableVM> data);
    }
}