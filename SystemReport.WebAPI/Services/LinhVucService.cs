using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class LinhVucService : BaseService, ILinhVucService
    {
        private DataContext _context;
        private BaseMongoDb<LinhVuc, string> BaseMongoDb;
        private IMongoCollection<LinhVuc> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public LinhVucService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<LinhVuc, string>(_context.LinhVuc);
            _collection = context.LinhVuc;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.LinhVucCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<LinhVuc> Create(LinhVuc model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var checkName = _context.LinhVuc.Find(x => x.Ten == model.Ten && x.IsDeleted != true).FirstOrDefault();

            if (checkName != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);
            }
            var entity = new LinhVuc
            {
                Ten = model.Ten,
                MoTa = model.MoTa,
                ThuTu = model.ThuTu,
                // DonVis = model.DonVis,
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
        public async Task<LinhVuc> Update(LinhVuc model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.LinhVuc.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var checkName = _context.LinhVuc.Find(x => x.Id != model.Id
                                                       && x.Ten.ToLower() == model.Ten.ToLower()
                                                       && x.IsDeleted != true
            ).FirstOrDefault();
            if (checkName != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);
            }

            entity.Ten = model.Ten;
            entity.MoTa = model.MoTa;
            entity.ThuTu = model.ThuTu;
            entity.DonVis = model.DonVis;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserName;

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


            var entity = _context.LinhVuc.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserName;
            entity.IsDeleted = true;
            var result = await BaseMongoDb.DeleteAsync(entity);

            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }
        }

        public async Task<List<LinhVuc>> Get()
        {
            return await _context.LinhVuc.Find(x => x.IsDeleted != true).SortByDescending(x => x.ThuTu)
                .ToListAsync();
        }

        public async Task<LinhVuc> GetById(string id)
        {
            return await _context.LinhVuc.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<LinhVuc>> GetPaging(LinhVucParam param)
        {
            PagingModel<LinhVuc> result = new PagingModel<LinhVuc>();
            var builder = Builders<LinhVuc>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }
            string sortBy = nameof(LinhVuc.ThuTu);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<LinhVuc>
                        .Sort.Descending(sortBy)
                    : Builders<LinhVuc>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<PagingModel<DonViShort>> GetByIdByFields(ChildPaging childPaging)
        {
            PagingModel<DonViShort> result = new PagingModel<DonViShort>();
            var builder = Builders<LinhVuc>.Filter;
            var filter = builder.Empty;
            try
            {
                filter = Builders<LinhVuc>.Filter.Where(x => x.Id == childPaging.ParentId);
                var model = await _collection.Find(filter).FirstOrDefaultAsync();
                result.TotalRows = model.DonVis.Count();
                if (result != null)
                {
                    result.Data =  model.DonVis.Skip(childPaging.Skip).Take(childPaging.Limit).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.TotalRows = 0;
                result.Data = null;
            }
            return result;
        }
        public async  Task  AddUnit(List<DonViShort> list)
        {
            var entity = _collection.Find(x => x.Id == list[0].linhVucId).FirstOrDefault();
                if (entity == default)
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
                }
                var filter = Builders<LinhVuc>.Filter.Where(x => x.Id == entity.Id);
                foreach (var item in list)
                {
                    var model = entity.DonVis.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (model != null)
                    {
                        throw new ResponseMessageException()
                            .WithCode(EResultResponse.FAIL.ToString())
                            .WithMessage(DefaultMessage.NAME_EXISTED);
                    }
                    var update = Builders<LinhVuc>.Update.Push(y => y.DonVis, item);
                    var result = _collection.UpdateOneAsync(filter, update);
                    if (result== default)
                    {
                        throw new ResponseMessageException()
                            .WithCode(EResultResponse.FAIL.ToString())
                            .WithMessage(DefaultMessage.CREATE_FAILURE);
                    }
                }
            
        }

        public async  Task DeleteUnit(DonViShort model)
        {
            var entity = _collection.Find(x => x.Id == model.linhVucId).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var filter = Builders<LinhVuc>.Filter.Where(x => x.Id == model.linhVucId);
            var update = Builders<LinhVuc>.Update.PullFilter(y => y.DonVis,
                Builders<DonViShort>.Filter.Where(n => n.Id == model.Id));
            var result = _collection.UpdateOneAsync(filter, update);
            if (result== default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }
        }
    }
}