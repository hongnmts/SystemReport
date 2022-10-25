using System.Threading.Tasks;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.Services;

namespace SystemReport.WebAPI.Interfaces
{
    public interface ILoggingService
    {
        Task<bool> SaveChanges();
        LoggingService WithCollectionName(string collectionName);
        LoggingService WithDatabaseName(string databaseName);
        LoggingService WithAction(string action);
        LoggingService WithUserName(string userName);
        LoggingService WithContentLog(string contentLog);
        LoggingService WithActionResult(string actionResult);
        
        Task<Logging> GetById(string id);
        Task<PagingModel<Logging>> GetPaging(LoggingParam param);
    }
}