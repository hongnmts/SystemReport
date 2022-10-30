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
    public class ModuleTrangThaiService : BaseService, IModuleTrangThaiService
    {
        private readonly DataContext _context;
        private readonly BaseMongoDb<ModuleTrangThai, string> BaseMongoDb;
        private readonly IMongoCollection<ModuleTrangThai> _collection;
        private readonly IDbSettings _settings;
        private ILoggingService _logger;

        public ModuleTrangThaiService(ILoggingService logger, IDbSettings settings,
            DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<ModuleTrangThai, string>(_context.ModuleTrangThai);
            _collection = context.ModuleTrangThai;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.WarningCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<ModuleTrangThai> Create(ModuleTrangThai model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = new ModuleTrangThai()
            {
                ModuleId = model.ModuleId,
                ModuleName = model.ModuleName,
                StartTrangThai = model.StartTrangThai,
                StartTrangThaiId = model.StartTrangThaiId,
                EndTrangThai = model.EndTrangThai,
                EndTrangThaiIds = model.EndTrangThaiIds,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            var result = await BaseMongoDb.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }

            return entity;
        }

        public async Task<ModuleTrangThai> Update(ModuleTrangThai model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.ModuleTrangThai.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }


            entity.ModuleId = model.ModuleId;
            entity.ModuleName = model.ModuleName;
            entity.StartTrangThai = model.StartTrangThai;
            entity.StartTrangThaiId = model.StartTrangThaiId;
            entity.EndTrangThai = model.EndTrangThai;
            entity.EndTrangThaiIds = model.EndTrangThaiIds;
            entity.ModifiedBy = CurrentUserName;
            entity.ModifiedAt = DateTime.Now;

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


            var entity = _context.ModuleTrangThai.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserName;
            var result = await BaseMongoDb.DeleteAsync(entity);

            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }
        }


        public async Task<ModuleTrangThai> GetById(string id)
        {
            return await _context.ModuleTrangThai.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<ModuleTrangThai>> GetPaging(ModuleTrangThaiParam param)
        {
            var result = new PagingModel<ModuleTrangThai>();
            var builder = Builders<ModuleTrangThai>.Filter;
            var filter = builder.Empty;

            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.ModuleName.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            string sortBy = nameof(ModuleTrangThai.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<ModuleTrangThai>
                        .Sort.Ascending(sortBy)
                    : Builders<ModuleTrangThai>
                        .Sort.Descending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<ModuleTrangThai> GetModuleTrangThaiByIdModule(string moduleId)
        {
            return await _collection.Find(x => x.ModuleId == moduleId).FirstOrDefaultAsync();
        }

        public async Task<List<ModuleTrangThai>> GetAll()
        {
            return await _collection.Find(x => true).ToListAsync();
        }
    }
}