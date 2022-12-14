using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IHoatDongKhoaHocService
    {
        Task<HoatDongKhoaHoc> Create(HoatDongKhoaHoc model);
        Task<HoatDongKhoaHoc> Update(HoatDongKhoaHoc model);
        Task Delete(string id);
        Task<List<HoatDongKhoaHoc>> Get();
        Task<HoatDongKhoaHoc> GetById(string id);
        Task<PagingModel<HoatDongKhoaHoc>> GetPaging(HoatDongKhoaHocParam param);
        Task<PagingModel<HoatDongKhoaHoc>> GetPagingThongKe(HoatDongKhoaHocParam param);
    }
}
