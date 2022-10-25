using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces.Identity
{
    public interface IRoleService
    {
        Task<Role> Create(Role model);
        Task<Role> Update(Role model);
        Task Delete(string id);
        Task<IEnumerable<Role>> Get();
        Task<Role> GetById(string id);
        Task<PagingModel<Role>> GetPaging(RoleParam param);
        Task<List<NavMenuVM>> GetMenuForUser(string userName);
        Task<List<string>> GetPermissionForCurrentUer(string userName);
    }
}