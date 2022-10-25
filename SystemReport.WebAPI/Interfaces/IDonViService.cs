using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IDonViService
    {
        Task<DonVi> Create(DonVi model);
        Task<DonVi> Update(DonVi model);
        Task Delete(string id);
        Task<IEnumerable<DonVi>> Get();
        Task<DonVi> GetById(string id);
        Task<PagingModel<DonVi>> GetPaging(DonViParam param);
        Task<List<DonViTreeVM>> GetTree();
        List<string> GetListDonViId(string donViId);
        Task<IEnumerable<DonVi>> GetDonViCha();
    }
}