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
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class HinhThucGuiService : BaseService, IHinhThucGuiService
    {
        private DataContext _context;
        private BaseMongoDb<HinhThucGui, string> BaseMongoDb;
        private IMongoCollection<HinhThucGui> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public HinhThucGuiService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<HinhThucGui, string>(_context.HinhThucGui);
            _collection = context.HinhThucGui;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.HinhThucGuiCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<HinhThucGui> Create(HinhThucGui model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var checkName = _context.HinhThucGui.Find(x => x.Ten == model.Ten && x.IsDeleted != true).FirstOrDefault();

            if (checkName != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);
            }
            var entity = new HinhThucGui
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
        public async Task<HinhThucGui> Update(HinhThucGui model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.HinhThucGui.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var checkName = _context.HinhThucGui.Find(x => x.Id != model.Id
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


            var entity = _context.HinhThucGui.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public async Task<List<HinhThucGui>> Get()
        {
            return await _context.HinhThucGui.Find(x => x.IsDeleted != true).SortByDescending(x => x.ThuTu)
                .ToListAsync();
        }

        public async Task<HinhThucGui> GetById(string id)
        {
            return await _context.HinhThucGui.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<HinhThucGui>> GetPaging(PagingParam param)
        {
            PagingModel<HinhThucGui> result = new PagingModel<HinhThucGui>();
            var builder = Builders<HinhThucGui>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }
            string sortBy = nameof(HinhThucGui.ThuTu);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<HinhThucGui>
                        .Sort.Descending(sortBy)
                    : Builders<HinhThucGui>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }
    }
}