using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Interfaces
{
    public interface IDefineStatusService
    {
        Task<List<StatusQuestion>> GetStatusQuestion();
        
    }
}