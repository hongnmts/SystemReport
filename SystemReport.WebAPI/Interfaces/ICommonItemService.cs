using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface ICommonItemService
    {
        Task<CommonItem> Create(CommonItem model);
        Task<CommonItem> Update(CommonItem model);
        Task Delete(string id);
        Task<List<CommonItem>> Get();
        Task<CommonItem> GetById(string id);
        Task<List<CommonItem>> GetByType(string id);
        Task<PagingModel<CommonItem>> GetPaging(PagingParam param);
    }
}
