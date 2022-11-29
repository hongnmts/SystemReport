using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IMauBieuService
    {
        Task<MauBieu> Create(MauBieu model);
        Task<MauBieu> Update(MauBieu model);
        Task Delete(string id);
        Task<List<MauBieu>> Get();
        Task<MauBieu> GetById(string id);
        Task<PagingModel<MauBieu>> GetPaging(MauBieuParam param);
        Task<DemoMauBieuVM> RenderTable(string bangBieuId);
        Task<List<DemoMauBieuVM>> RenderTableMauBieu(string mauBieuId);
        Task<PagingModel<MauBieu>> GetPagingCaNhan(MauBieuParam param);
        Task<PagingModel<MauBieu>> GetPagingNhapLieu(MauBieuParam param);
        Task<PagingModel<MauBieu>> GetPagingKiemTra(MauBieuParam param);
        Task<PagingModel<MauBieu>> GetPagingTongHop(MauBieuParam param);
        Task<PagingModel<MauBieu>> GetPagingXuatBan(MauBieuParam param);

        Task<PagingModel<MauBieu>> GetPagingThongTinXuatBan(MauBieuParam param);

        // Tính toán biểu mẫu mới
        Task GenerateMauBieu(InputMauBieuModel model);

        Task DeleteMauBieu(string mauBieuId);

        Task ChangeStatus(TrangThaiParam model);

        List<ListNamMauBieuVM> ListNamMauBieu();
        Task<PagingModel<MauBieuItemVM>> GetMauBieuPaging(MauBieuParam param);
        Task GenerateMauBieuTongHop(InputMauBieuModel model);
    }
}