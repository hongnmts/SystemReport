using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IModuleTrangThaiService
    {
        Task<ModuleTrangThai> Create(ModuleTrangThai model);
        Task<ModuleTrangThai> Update(ModuleTrangThai model);
        Task Delete(string id);
        Task<ModuleTrangThai> GetById(string id);
        Task<PagingModel<ModuleTrangThai>> GetPaging(ModuleTrangThaiParam param);
        Task<ModuleTrangThai> GetModuleTrangThaiByIdModule(string moduleId);
        Task<List<ModuleTrangThai>> GetAll();
    }
}