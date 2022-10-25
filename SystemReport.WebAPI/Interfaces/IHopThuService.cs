using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IHopThuService
    {
        Task<HopThu> Create(HopThu model);
        Task<HopThu> Update(HopThu model);
        Task Delete(string id);
        Task DeleteR(string id);
        Task<HopThu> GetById(string id);
        Task<PagingModel<HopThu>> GetPaging(HopThuParam param);
        Task<PagingModel<HopThu>> GetPagingDaGui(HopThuParam param);
        Task<PagingModel<HopThu>> GetPagingRac(HopThuParam param);
        Task<HopThu> CreateSend(HopThu model);
    }
}