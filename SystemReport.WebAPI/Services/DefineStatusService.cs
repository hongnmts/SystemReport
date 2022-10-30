using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Services
{
    public class DefineStatusService : BaseService, IDefineStatusService
    {
        private DataContext _context;
        private BaseMongoDb<LinhVuc, string> BaseMongoDb;
        private IMongoCollection<LinhVuc> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;
        public DefineStatusService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {

        }

        public async Task<List<StatusQuestion>> GetStatusQuestion()
        {
            List<StatusQuestion> result = new List<StatusQuestion>();
            result.Add(new StatusQuestion
            {
                StatusCode = DefineStatus.KHONGTIEPNHAN.ToString(),
                StatusName = "Không tiếp nhận"
            });
            result.Add(new StatusQuestion
            {
                StatusCode = DefineStatus.CHODUYET.ToString(),
                StatusName = "Chờ duyệt"
            });
            result.Add(new StatusQuestion
            {
                StatusCode = DefineStatus.VUATIEPNHAN.ToString(),
                StatusName = "Vừa tiếp nhận"
            });
            result.Add(new StatusQuestion
            {
                StatusCode = DefineStatus.DANGXULY.ToString(),
                StatusName = "Đang xử lý"
            });
            result.Add(new StatusQuestion
            {
                StatusCode = DefineStatus.DAXULYXONG.ToString(),
                StatusName = "Đã xử lý xong"
            });
            result.Add(new StatusQuestion
            {
                StatusCode = DefineStatus.DATRALOINGUOIDAN.ToString(),
                StatusName = "Đã trả lời người dân"
            });
            return await Task.FromResult(result);
        }
    }
}