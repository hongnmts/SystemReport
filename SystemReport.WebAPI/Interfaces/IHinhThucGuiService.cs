using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IHinhThucGuiService
    {
        Task<HinhThucGui> Create(HinhThucGui model);
        Task<HinhThucGui> Update(HinhThucGui model);
        Task Delete(string id);
        Task<List<HinhThucGui>> Get();
        Task<HinhThucGui> GetById(string id);
        Task<PagingModel<HinhThucGui>> GetPaging(PagingParam param);
    }
}