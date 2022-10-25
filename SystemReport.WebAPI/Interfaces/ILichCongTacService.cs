using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface ILichCongTacService
    {
        Task<LichCongTac> Create(LichCongTac model);
        Task<LichCongTac> Update(LichCongTac model);
        Task Delete(string id);
        Task<LichCongTac> GetById(string id);
        Task<PagingModel<LichCongTac>> GetPagingCaNhan(LichCongTacParam param);

        #region CongViec
        Task<LichCongTac> CreateCongViec(CongViec model);
        Task<LichCongTac> UpdateCongViec(CongViec model);
        Task DeleteCongViec(CongViec model);
        Task<CongViec> GetByIdCongViec(CongViec model);
        #endregion

        Task<dynamic> GetAll();
        Task<dynamic> GetPaging(LichCongTacParam param);
    }
}