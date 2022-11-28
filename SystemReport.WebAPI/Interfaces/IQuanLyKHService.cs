using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IQuanLyKHService
    {
        Task<QuanLyKH> Create(QuanLyKH model);
        Task<QuanLyKH> Update(QuanLyKH model);
        Task Delete(string id);
        Task<List<QuanLyKH>> Get();
        Task<QuanLyKH> GetById(string id);
        Task<PagingModel<QuanLyKH>> GetPaging(QuanLyKHParam param);
        Task<PagingModel<QuanLyKH>> GetPagingThongKe(QuanLyKHParam param);
    }
}
