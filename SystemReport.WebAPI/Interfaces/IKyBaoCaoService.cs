using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IKyBaoCaoService
    {
        Task<KyBaoCao> Create(KyBaoCao model);
        Task<KyBaoCao> Update(KyBaoCao model);
        Task Delete(string id);
        Task<List<KyBaoCao>> Get();
        Task<KyBaoCao> GetById(string id);
        Task<PagingModel<KyBaoCao>> GetPaging(PagingParam param);
    }
}