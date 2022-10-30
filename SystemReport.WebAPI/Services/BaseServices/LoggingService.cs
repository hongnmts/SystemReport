using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.Services
{
    public class LoggingService : BaseService, ILoggingService
    {
        private readonly IMongoCollection<Logging> _loggingCollection;
        private BaseMongoDb<Logging, string> BaseMongoDb;

        public LoggingService(DataContext context, IHttpContextAccessor httpContext) : base(context, httpContext)
        {
            _loggingCollection = context.Loggings;
            BaseMongoDb = new BaseMongoDb<Logging, string>(context.Loggings);
        }

        #region Methods

        public async Task<bool> SaveChanges()
        {
            if (CheckEmptyProperties())
            {
                return false;
            }

            var logger = new Logging()
            {
                CollectionName = this.CollectionName,
                DatabaseName = this.DatabaseName,
                Action = this.Action,
                CreatedBy = this.UserName ?? CurrentUserName,
                ModifiedBy = this.UserName ?? CurrentUserName,
                ContentLog = this.ContentLog,
                ActionResult = this.ActionResult
            };

            var result = await BaseMongoDb.CreateAsync(logger);
            return result.Success;
        }

        #endregion

        #region Actions

        private string CollectionName { get; set; }
        private string DatabaseName { get; set; }
        private string Action { get; set; } // EActionLog
        private string UserName { get; set; }
        private string ContentLog { get; set; }
        private string ActionResult { get; set; } // EActionResultLog

        public LoggingService WithCollectionName(string collectionName)
        {
            if (!string.IsNullOrEmpty(collectionName))
            {
                CollectionName = collectionName;
            }

            return this;
        }

        public LoggingService WithDatabaseName(string databaseName)
        {
            if (!string.IsNullOrEmpty(databaseName))
            {
                DatabaseName = databaseName;
            }

            return this;
        }

        public LoggingService WithAction(string action)
        {
            if (!string.IsNullOrEmpty(action))
            {
                Action = action;
            }

            return this;
        }

        public LoggingService WithUserName(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                UserName = userName;
            }

            return this;
        }

        public LoggingService WithContentLog(string contentLog)
        {
            if (!string.IsNullOrEmpty(contentLog))
            {
                ContentLog = contentLog;
            }

            return this;
        }

        public LoggingService WithActionResult(string actionResult)
        {
            if (!string.IsNullOrEmpty(actionResult))
            {
                ActionResult = actionResult;
            }

            return this;
        }

        public async Task<Logging> GetById(string id)
        {
            return await _loggingCollection.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();
        }

        public async Task<PagingModel<Logging>> GetPaging(LoggingParam param)
        {
            PagingModel<Logging> result = new PagingModel<Logging>();
            var builder = Builders<Logging>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x =>
                     x.CreatedBy.Trim().ToLower().Contains(param.Content.Trim().ToLower())
                     || x.Action.Trim().ToLower().Contains(param.Content.Trim().ToLower())
                            || x.ContentLog.Trim().ToLower().Contains(param.Content.Trim().ToLower())
                            || x.CollectionName.Trim().ToLower().Contains(param.Content.Trim().ToLower())

                    ));
            }

            if (param.TuNgay != default && param.DenNgay != default)
            {
                DateTime firstDay = param.TuNgay.Value;
                DateTime lastDay = param.DenNgay.Value;

                filter = builder.And(filter,
                    builder.Where(x => x.CreatedAt != default && x.CreatedAt <= lastDay && x.CreatedAt >= firstDay));
            }
            else
            {
                if (param.TuNgay != default)
                {
                    DateTime firstDay = param.TuNgay.Value;

                    filter = builder.And(filter,
                        builder.Where(x => x.CreatedAt != default && x.CreatedAt >= firstDay));
                }
                if (param.DenNgay != default)
                {
                    DateTime lastDay = param.DenNgay.Value;

                    filter = builder.And(filter,
                        builder.Where(x => x.CreatedAt != default && x.CreatedAt >= lastDay));
                }
            }

            result.TotalRows = await _loggingCollection.CountDocumentsAsync(filter);
            result.Data = await _loggingCollection.Find(filter)
                .SortByDescending(x => x.CreatedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        // private void EmptyProperties()
        // {
        //     CollectionName = null;
        //     DatabaseName = null;
        //     Action = null;
        //     UserName = null;
        //     ContentLog = null;
        //     ActionResult = null;
        // }

        private bool CheckEmptyProperties()
        {
            if (string.IsNullOrEmpty(Action))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}