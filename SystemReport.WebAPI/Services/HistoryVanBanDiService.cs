using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Enums;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using StringExtensions = SystemReport.WebAPI.Extensions.StringExtensions;

namespace SystemReport.WebAPI.Services
{
    public class HistoryVanBanDiService : BaseService
    {
        private DataContext _context;
        private BaseMongoDb<HistoryVanBanDi, string> BaseMongoDb;
        private IMongoCollection<HistoryVanBanDi> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public HistoryVanBanDiService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<HistoryVanBanDi, string>(_context.HistoryVanBanDi);
            _collection = context.HistoryVanBanDi;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.HistoryVanBanDiCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
            history = new HistoryVanBanDi();
            history.CreatedBy = CurrentUserName;
        }

        public async Task<List<HistoryVanBanDi>> GetHistoryByQuestionId(string vanBanId)
        {
            return await _collection.Find(x => x.VanBanId == vanBanId).SortByDescending(x => x.ModifiedAt).ToListAsync();
        }
        public async Task<bool> SaveChangeHistory()
        {
            if (history == default)
                return false;
            Hash();
            history.Id = BsonObjectId.GenerateNewId().ToString();
            var result = await BaseMongoDb.CreateAsync(history);
            if (result.Entity.Id == default || !result.Success)
                return false;
            return true;
        }

        public HistoryVanBanDiService WithTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                history.Title = title;
            }

            return this;
        }
        public HistoryVanBanDiService WithContent(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                history.Content = content;
            }

            return this;
        }
        public HistoryVanBanDiService WithStatus(TrangThaiShort trangThai)
        {
            if (trangThai != default)
            {
                history.TrangThai = trangThai;
            }

            return this;
        }
        public HistoryVanBanDiService WithUserName(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var user = _context.Users.Find(x => x.UserName == userName).FirstOrDefault();
                if (user == default)
                    return this;

                history.UserName = user.UserName;
                history.FullName = user.FullName;
            }

            return this;
        }


        public HistoryVanBanDiService WithVanBanId(string vanBanId)
        {
            if (!string.IsNullOrEmpty(vanBanId))
            {
                history.VanBanId = vanBanId;
            }

            return this;
        }
        public HistoryVanBanDiService WithType(dynamic oldValue)
        {
            if (oldValue != null)
            {
                history.OldValue = oldValue;
            }

            return this;
        }
        public HistoryVanBanDiService WithAction(EAction action)
        {
            history.Action = action.GetDisplayName();

            return this;
        }
        public HistoryVanBanDiService WithAction(string action)
        {
            history.Action = action;

            return this;
        }

        private void Hash()
        {
            var hash = StringExtensions.SHA256(history.CreatedAt.ToString() + history.CreatedBy + history.Id);
            history.Hash = hash;
            var pre = _collection.Find(x => x.VanBanId == history.VanBanId)
                .SortByDescending(x => x.CreatedAt).FirstOrDefault();
            if (pre != default)
                history.ReferenceHash = pre.Hash;
        }
        #region Properties

        private HistoryVanBanDi history;

        #endregion
    }
}