using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface ICoQuanService
    {
        Task<CoQuan> Create(CoQuan model);
        Task<CoQuan> Update(CoQuan model);
        Task Delete(string id);
        Task<List<CoQuan>> Get();
        Task<CoQuan> GetById(string id);
        Task<PagingModel<CoQuan>> GetPaging(PagingParam param);
    }
}