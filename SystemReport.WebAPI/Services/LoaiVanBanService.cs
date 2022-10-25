using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Interfaces.Identity;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class LoaiVanBanService : BaseService, ILoaiVanBanService
    {
        private DataContext _context;
        private BaseMongoDb<LoaiVanBan, string> BaseMongoDb;
        private ILoggingService _logger;
        private IDbSettings _settings;

        public LoaiVanBanService(
            ILoggingService logger,
            IDbSettings settings,
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<LoaiVanBan, string>(_context.LoaiVanBan);
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.LoaiVanBanCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<LoaiVanBan> Create(LoaiVanBan model)
        {
            if (model == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            var checkName = _context.LoaiVanBan.Find(x => x.Ten == model.Ten && x.IsDeleted != true).FirstOrDefault();

            if (checkName != default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);

            var entity = new LoaiVanBan
            {
                Ten = model.Ten,
                Code = model.Code,
                ThuTu = model.ThuTu,
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

        public async Task<LoaiVanBan> Update(LoaiVanBan model)
        {
            if (model == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            var entity = _context.LoaiVanBan.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);

            var checkName = _context.LoaiVanBan.Find(x => x.Id != model.Id && x.Ten == model.Ten && x.IsDeleted != true)
                .FirstOrDefault();

            if (checkName != default)
                throw new ResponseMessageException().WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);

            entity.Ten = model.Ten;
            entity.Code = model.Code;
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
            var entity = _context.LoaiVanBan.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public async Task<IEnumerable<LoaiVanBan>> Get()
        {
            return await _context.LoaiVanBan.Find(x => x.IsDeleted != true).ToListAsync();
        }

        public async Task<LoaiVanBan> GetById(string id)
        {
            return await _context.LoaiVanBan.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();
        }

        public async Task<PagingModel<LoaiVanBan>> GetPaging(PagingParam param)
        {
            PagingModel<LoaiVanBan> result = new PagingModel<LoaiVanBan>();
            var builder = Builders<LoaiVanBan>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            string sortBy = "ThuTu";
            result.TotalRows = await _context.LoaiVanBan.CountDocumentsAsync(filter);
            result.Data = await _context.LoaiVanBan.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<LoaiVanBan>
                        .Sort.Ascending(sortBy)
                    : Builders<LoaiVanBan>
                        .Sort.Descending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<List<LoaiVanBanTreeVM>> GetTree()
        {
            var listLoaiVanBan = await _context.LoaiVanBan.Find(x  => x.IsDeleted ==false).SortBy(x=> x.Ten).ToListAsync();
            List<LoaiVanBanTreeVM> list = new List<LoaiVanBanTreeVM>();
            foreach (var item in listLoaiVanBan)
            {
                LoaiVanBanTreeVM loaiVanBan = new LoaiVanBanTreeVM(item);
                list.Add(loaiVanBan);
            }
            return list;
        }
    }
}