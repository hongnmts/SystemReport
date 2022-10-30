using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class WarningService : BaseService, IWarningService
    {
        private readonly DataContext _context;
        private readonly BaseMongoDb<Warning, string> BaseMongoDb;
        private readonly IMongoCollection<Warning> _collection;
        private readonly IDbSettings _settings;
        private ILoggingService _logger;

        public WarningService(ILoggingService logger, IDbSettings settings,
            DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<Warning, string>(_context.Warning);
            _collection = context.Warning;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.WarningCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<Warning> Create(Warning model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = new Warning()
            {
                Title = model.Title,
                Content = model.Content,
                WarningDate = model.WarningDate,
                Priority = model.Priority,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
            if (model.UploadFiles != default && model.UploadFiles.Count > 0)
            {
                foreach (var file in model.UploadFiles)
                {
                    var newFile = new FileShort();
                    newFile.FileId = file.FileId;
                    newFile.FileName = file.FileName;
                    if (entity.FileManagers == default)
                    {
                        entity.FileManagers = new List<FileShort>();
                    }

                    entity.FileManagers.Add(newFile);
                }
            }

            var result = await BaseMongoDb.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }

            return entity;
        }

        public async Task<Warning> Update(Warning model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.Warning.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.WarningDate = model.WarningDate;
            entity.Priority = model.Priority;
            entity.ModifiedBy = CurrentUserName;
            entity.ModifiedAt = DateTime.Now;
            if (model.UploadFiles != default && model.UploadFiles.Count > 0)
            {
                foreach (var file in model.UploadFiles)
                {
                    var newFile = new FileShort();
                    newFile.FileId = file.FileId;
                    newFile.FileName = file.FileName;
                    if (entity.FileManagers == default)
                    {
                        entity.FileManagers = new List<FileShort>();
                    }

                    entity.FileManagers.Add(newFile);
                }
            }

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            return entity;
        }

        public async Task Delete(string id)
        {
            if (id == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }


            var entity = _context.Warning.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var result = await BaseMongoDb.DeleteByIdsync(id);

            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }
        }

        public async Task<List<Warning>> Get()
        {
            return await _context.Warning.Find(x => x.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<Warning> GetById(string id)
        {
            return await _context.Warning.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<Warning>> GetPaging(WarningParam param)
        {
            var result = new PagingModel<Warning>();
            var builder = Builders<Warning>.Filter;
            var filter = builder.Empty;

            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Title.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            string sortBy = nameof(Warning.WarningDate);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<Warning>
                        .Sort.Ascending(sortBy)
                    : Builders<Warning>
                        .Sort.Descending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }
    }
}