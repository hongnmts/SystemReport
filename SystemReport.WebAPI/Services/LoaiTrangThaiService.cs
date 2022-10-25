using System;
using System.Collections.Generic;
using System.Linq;
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

    public class LoaiTrangThaiService : BaseService, ILoaiTrangThaiService
    {
        private DataContext _context;
        private BaseMongoDb<LoaiTrangThai, string> BaseMongoDb;
        private IMongoCollection<LoaiTrangThai> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public LoaiTrangThaiService (ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<LoaiTrangThai, string>(_context.LoaiTrangThai);
            _collection = context.LoaiTrangThai;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.ChucVuCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<LoaiTrangThai> Create(LoaiTrangThai model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var checkName = _context.LoaiTrangThai.Find(x => x.Ten == model.Ten && x.IsDeleted != true).FirstOrDefault();

            if (checkName != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);
            }

            var entity = new LoaiTrangThai
            {
                Ten = model.Ten,
                Code = model.Code,
                MoTa = model.MoTa,
                ThuTu = model.ThuTu,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName
            };

            if (model.ListTrangThai != default)
            {
                var trangthaiId = model.ListTrangThai.Select(x => x.Id).ToList();
                    var trangThai = _context.TrangThai.Find(x => trangthaiId.Contains(x.Id)).ToList().Select(x => new TrangThaiShort()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Ten = x.Ten,
                        BgColor = x.BgColor,
                        Color = x.Color
                    }).ToList();
                    entity.ListTrangThai = trangThai;
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

        public async Task<LoaiTrangThai> Update(LoaiTrangThai model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.LoaiTrangThai.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var checkName = _context.LoaiTrangThai.Find(x => x.Id != model.Id
                                                             && x.Code.ToLower() == model.Code.ToLower()
                                                             && x.IsDeleted != true
            ).FirstOrDefault();
            if (checkName != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);
            }

            entity.Ten = model.Ten;
            entity.Code = model.Code;
            entity.MoTa = model.MoTa;
            entity.ThuTu = model.ThuTu;
            
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserName;
            
            if (model.ListTrangThai != default)
            {
                var trangthaiId = model.ListTrangThai.Select(x => x.Id).ToList();
                var trangThai = _context.TrangThai.Find(x => trangthaiId.Contains(x.Id)).ToList().Select(x => new TrangThaiShort()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Ten = x.Ten,
                    BgColor = x.BgColor,
                    Color = x.Color
                }).ToList();
                entity.ListTrangThai = trangThai;
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
            
            var entity = _context.LoaiTrangThai.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public async Task<List<LoaiTrangThai>> Get()
        {
            return await _context.LoaiTrangThai.Find(x => x.IsDeleted != true).SortByDescending(x => x.ThuTu)
                .ToListAsync();
        }

        public async Task<LoaiTrangThai> GetById(string id)
        {
            return await _context.LoaiTrangThai.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<LoaiTrangThai>> GetPaging(LinhVucParam param)
        {
            PagingModel<LoaiTrangThai> result = new PagingModel<LoaiTrangThai>();
            var builder = Builders<LoaiTrangThai>.Filter;
            var filter = builder.Empty;

            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            string sortBy = nameof(LoaiTrangThai.ThuTu);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<LoaiTrangThai>
                        .Sort.Descending(sortBy)
                    : Builders<LoaiTrangThai>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }
    }
}