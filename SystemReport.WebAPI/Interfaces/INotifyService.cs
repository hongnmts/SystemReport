using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.Services;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface INotifyService
    {
        NotifyService WithNotify(Notify notify);
        NotifyService WithRecipients(List<string> recipientIds);
        NotifyService WithRecipient(string recipientId);
        Task PushNotify();
        Task<PagingModel<Notify>> GetPaging(NotifyParam param);
        Task<Notify> GetById(string id);
        Task<ResultResponse<NotifyVM>> GetListNotify();
        Task<ResultResponse<Notify>> ChangeStatus(string id);
        Task LuuCVNoiBo(string idNotify);
    }
}