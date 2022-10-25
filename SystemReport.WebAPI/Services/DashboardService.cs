using System.Threading.Tasks;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Interfaces.Identity;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

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
            var data = new DashboardVM();
            data.SoCVDen = (int)_context.VanBanDen.Find(x =>(
                x.CreatedBy == CurrentUserName || (x.ListOwerId != default && x.ListOwerId.Contains(CurrentUserName)) || (x.TrangThai != default && x.TrangThai.Code == DefaultRoleCode.BAN_HANH_VAN_BAN_DEN)) &&
                x.IsDeleted != true).CountDocuments();
            data.SoCVDi = (int)_context.VanBanDi.Find(x =>(
                x.CreatedBy == CurrentUserName || (x.ListOwerId != default && x.ListOwerId.Contains(CurrentUserName) || (x.TrangThai != default && x.TrangThai.Code == DefaultRoleCode.BAN_HANH))) &&
                x.IsDeleted != true).CountDocuments();
            data.SoThongBao = (int)_context.Notify.Find(x => x.RecipientId == CurrentUser.Id).CountDocuments();
            data.SoThu = (int)_context.HopThu.Find(x => x.NguoiNhan != default && x.NguoiNhan.UserName == CurrentUserName).CountDocuments();

            return data;
        }
    }
}