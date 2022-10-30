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
using SystemReport.WebAPI.ViewModels;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class LoaiMauBieuService : BaseService, ILoaiMauBieuService
    {
        private DataContext _context;
        private BaseMongoDb<LoaiMauBieu, string> BaseMongoDb;
        private ILoggingService _logger;
        private IDbSettings _settings;

        public LoaiMauBieuService(
            ILoggingService logger,
            IDbSettings settings,
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<LoaiMauBieu, string>(_context.LoaiMauBieu);
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.LoaiMauBieuCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<LoaiMauBieu> Create(LoaiMauBieu model)
        {
            if (model == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            var checkName = _context.LoaiMauBieu.Find(x => x.Ten == model.Ten && x.IsDeleted != true).FirstOrDefault();

            if (checkName != default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);

            var entity = new LoaiMauBieu
            {
                Ten = model.Ten,
                Code = model.Code,
                ThuTu = model.ThuTu,
                TenNgan = model.TenNgan,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
                // ModifiedBy = CurrentUser.Id,
                // CreatedBy = CurrentUser.Id
            };

            var result = await BaseMongoDb.CreateAsync(entity);

            if (result.Entity.Id == default || !result.Success)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);

            return entity;
        }

        public async Task<LoaiMauBieu> Update(LoaiMauBieu model)
        {
            if (model == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            var entity = _context.LoaiMauBieu.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);

            var checkName = _context.LoaiMauBieu.Find(x => x.Id != model.Id && x.Ten == model.Ten && x.IsDeleted != true)
                .FirstOrDefault();

            if (checkName != default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);

            entity.Ten = model.Ten;
            entity.Code = model.Code;
            entity.ThuTu = model.ThuTu;
            entity.TenNgan = model.TenNgan;
            entity.ModifiedBy = CurrentUserName;
            entity.ModifiedAt = DateTime.Now;
            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);

            return entity;
        }

        public async Task Delete(string id)
        {
            if (id == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            var entity = _context.LoaiMauBieu.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);

            entity.ModifiedAt = DateTime.Now;
            entity.IsDeleted = true;

            var result = await BaseMongoDb.DeleteAsync(entity);

            if (!result.Success)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
        }

        public async Task<IEnumerable<LoaiMauBieu>> Get()
        {
            return await _context.LoaiMauBieu.Find(x => x.IsDeleted != true).ToListAsync();
        }

        public async Task<IEnumerable<LoaiMauBieu>> GetCountLoaiBaoCao()
        {
            var data = await _context.LoaiMauBieu.Find(x => x.IsDeleted != true).ToListAsync();
            var result = new List<LoaiMauBieu>();

            foreach (var item in data)
            {
                item.CountMauBieu = _context.MauBieu.Find(x => x.IsTemplate != true && x.LoaiMauBieu != default && x.LoaiMauBieu.Id == item.Id).ToList().Count;
                result.Add(item);
            }

            return result;
        }

        public async Task<LoaiMauBieu> GetById(string id)
        {
            return await _context.LoaiMauBieu.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();
        }

        public async Task<PagingModel<LoaiMauBieu>> GetPaging(PagingParam param)
        {
            PagingModel<LoaiMauBieu> result = new PagingModel<LoaiMauBieu>();
            var builder = Builders<LoaiMauBieu>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            string sortBy = nameof(LoaiMauBieu.ThuTu);
            result.TotalRows = await _context.LoaiMauBieu.CountDocumentsAsync(filter);
            result.Data = await _context.LoaiMauBieu.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<LoaiMauBieu>
                        .Sort.Ascending(sortBy)
                    : Builders<LoaiMauBieu>
                        .Sort.Descending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<List<LoaiMauBieuTreeVM>> GetTree()
        {
            var listLoaiMauBieu = await _context.LoaiMauBieu.Find(x => x.IsDeleted == false).SortBy(x => x.Ten).ToListAsync();
            List<LoaiMauBieuTreeVM> list = new List<LoaiMauBieuTreeVM>();
            foreach (var item in listLoaiMauBieu)
            {
                LoaiMauBieuTreeVM LoaiMauBieu = new LoaiMauBieuTreeVM(item);
                list.Add(LoaiMauBieu);
            }
            return list;
        }
    }
}