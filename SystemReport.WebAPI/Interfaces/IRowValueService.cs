using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IRowValueService
    {
        Task<List<RowValue>> Create(List<RowValue> model);
        Task<List<RowValue>> CreateSub(List<RowValue> model);
        Task<RowValue> Update(RowValue model);
        Task Delete(string id);
        Task<RowValue> GetById(string id);
        Task<List<RowValueTreeVM>> GetTreeByBangBieuId(string bangBieuId);
        Task<List<BodyTableVM>> RenderBodyByBangBieuId(string bangBieuId);
        Task<List<RowValueTreeVM>> GetTreeParentRowValue(string bangBieuId);
        List<RowValue> GetRowValueByKeyRow(string keyRow);
        Task<List<RowValue>> DeleteRowValue(List<RowValue> model);
        Task<List<BodyTableVM>> RenderBodyMainByBangBieuId(string bangBieuId);
        Task<List<BodyTableVM>> RenderBodyMainByBangBieuId(List<RowValue> rowValues);
        Task<List<RowValue>> StringCongThuc(string keyRow);
        Task AddRowTong(string bangBieuId);
    }
}