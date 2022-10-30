using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Services
{
    public class EnumService : BaseService, IEnumService
    {
        private DataContext _context;
        private BaseMongoDb<LinhVuc, string> BaseMongoDb;
        private IMongoCollection<LinhVuc> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;
        public EnumService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {

        }

        public async Task<List<EnumModel>> GetMucDo()
        {
            List<EnumModel> result = new List<EnumModel>();
            result.Add(new EnumModel
            {
                Code = EMucDo.THAP.ToString(),
                Name = "Thấp"
            });
            result.Add(new EnumModel
            {
                Code = EMucDo.TRUNGBINH.ToString(),
                Name = "Trung bình"
            });
            result.Add(new EnumModel
            {
                Code = EMucDo.CAO.ToString(),
                Name = "Cao"
            });
            return await Task.FromResult(result);
        }

        public async Task<List<EnumModel>> GetLoaiLichCongTac()
        {
            List<EnumModel> result = new List<EnumModel>();
            result.Add(new EnumModel
            {
                Code = LoaiLichCongTac.LICH_CONG_TAC_TRUONG,
                Name = "Lịch công tác trường"
            });
            result.Add(new EnumModel
            {
                Code = EMucDo.TRUNGBINH.ToString(),
                Name = "Trung bình"
            });
            result.Add(new EnumModel
            {
                Code = EMucDo.CAO.ToString(),
                Name = "Cao"
            });
            return await Task.FromResult(result);
        }
    }
}