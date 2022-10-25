using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces.Identity
{
    public interface IMenuService
    {
        Task<Menu> Create(Menu model);
        Task<Menu> Update(Menu model);
        Task Delete(string id);
        Task<IEnumerable<Menu>> Get();
        Task<Menu> GetById(string id);
        Task<PagingModel<Menu>> GetPaging(PagingParam param);
        Task<List<MenuTreeVM>> GetTree();
        Task<List<MenuTreeVM>> GetTreeList();
        Task<List<NavMenuVM>> GetTreeListMenuForCurrentUser(List<Menu> listMenu);
    }
}