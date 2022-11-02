using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Services
{
    public class DashboardService : BaseService, IDashboardService
    {
        private DataContext _context;

        private IDbSettings _settings;
        private ILoggingService _logger;

        public DashboardService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.LinhVucCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public DashboardVM GetDashboard()
        {
            throw new System.NotImplementedException();
        }
    }
}