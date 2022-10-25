using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IEnumService
    {
        Task<List<EnumModel>> GetMucDo();
    }
}