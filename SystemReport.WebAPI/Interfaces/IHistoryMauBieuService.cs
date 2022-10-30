using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.Services;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IHistoryMauBieuService
    {
        Task<PagingModel<HistoryMauBieu>> GetHistoryByMauBieuId(HistoryParam param);
        Task<bool> SaveChangeHistory();
        HistoryMauBieuService WithTitle(string title);
        HistoryMauBieuService WithContent(string content);
        HistoryMauBieuService WithStatus(TrangThai trangThai);
        HistoryMauBieuService WithUserName(User currentUser);
        HistoryMauBieuService WithFormKey(string formKey);
        HistoryMauBieuService WithCollection(string collection, string collectionId, string collectionName);
        HistoryMauBieuService WithAction(string action);
        HistoryMauBieuService WithOldValue(dynamic oldValue);
        HistoryMauBieuService WithBangBieu(BangBieu bangBieu);
        Task<DemoMauBieuVM> RenderTableBangBieu(string historyId);
    }
}
