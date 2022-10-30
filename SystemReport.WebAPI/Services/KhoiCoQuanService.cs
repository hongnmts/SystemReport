using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public class KhoiCoQuanService : BaseService, IKhoiCoQuanService
    {
        private DataContext _context;
        private BaseMongoDb<KhoiCoQuan, string> BaseMongoDb;
        private IMongoCollection<KhoiCoQuan> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public KhoiCoQuanService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<KhoiCoQuan, string>(_context.KhoiCoQuan);
            _collection = context.KhoiCoQuan;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.KhoiCoQuanCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<KhoiCoQuan> Create(KhoiCoQuan model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var checkName = _context.CoQuan.Find(x => x.Ten == model.Ten && x.IsDeleted != true).FirstOrDefault();

            if (checkName != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);
            }
            var entity = new KhoiCoQuan
            {
                Ten = model.Ten,
                MoTa = model.MoTa,
                ThuTu = model.ThuTu,
                Code = model.Code,
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
        public async Task<KhoiCoQuan> Update(KhoiCoQuan model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.KhoiCoQuan.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var checkName = _context.KhoiCoQuan.Find(x => x.Id != model.Id
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
            entity.Code = model.Code;
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


            var entity = _context.KhoiCoQuan.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public async Task<List<KhoiCoQuan>> Get()
        {
            return await _context.KhoiCoQuan.Find(x => x.IsDeleted != true).SortByDescending(x => x.ThuTu)
                .ToListAsync();
        }

        public async Task<KhoiCoQuan> GetById(string id)
        {
            return await _context.KhoiCoQuan.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<KhoiCoQuan>> GetPaging(PagingParam param)
        {
            PagingModel<KhoiCoQuan> result = new PagingModel<KhoiCoQuan>();
            var builder = Builders<KhoiCoQuan>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }
            string sortBy = nameof(KhoiCoQuan.ThuTu);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<KhoiCoQuan>
                        .Sort.Descending(sortBy)
                    : Builders<KhoiCoQuan>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task ReadDataKhoiCoQuan(string filePath)
        {
            List<KhoiCoQuan> KhoiCoQuan = new List<KhoiCoQuan>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        int khoiCoQuanId = 0;
                        var check = int.TryParse(reader.GetValue(0)?.ToString(), out khoiCoQuanId);
                        if (check && khoiCoQuanId != 0)
                        {
                            var unitGroup = new KhoiCoQuan()
                            {
                                Id = BsonObjectId.GenerateNewId().ToString(),
                                KhoiCoQuanId = khoiCoQuanId.ToString(),
                                Ten = reader.GetValue(1)?.ToString(),
                                MoTa = reader.GetValue(2)?.ToString(),
                                ThuTu = khoiCoQuanId
                            };
                            KhoiCoQuan.Add(unitGroup);
                        }
                    }
                }
            }
            await _collection.InsertManyAsync(KhoiCoQuan);
        }
        public async Task ReadDataChucVu(string filePath)
        {
            var KhoiCoQuan = new List<ChucVu>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int index = 1;
                    while (reader.Read())
                    {
                        var unitGroup = new ChucVu()
                        {
                            Id = BsonObjectId.GenerateNewId().ToString(),
                            Ten = reader.GetValue(0)?.ToString(),
                            ThuTu = index++
                        };
                        KhoiCoQuan.Add(unitGroup);
                    }
                }
            }
            await _context.ChucVu.InsertManyAsync(KhoiCoQuan);
        }
        public async Task ReadDataCoQuan(string filePath)
        {
            var khoiCoQuan = _context.KhoiCoQuan.AsQueryable().Where(x => x.IsDeleted != true)
                .Select(x => new KhoiCoQuanShort()
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    MoTa = x.MoTa,
                    KhoiCoQuanId = x.KhoiCoQuanId
                }).ToList();
            List<CoQuan> CoQuan = new List<CoQuan>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        int coquanId = 0;
                        var check = int.TryParse(reader.GetValue(0)?.ToString(), out coquanId);
                        if (check && coquanId != 0)
                        {
                            var khoiCoQuanId = 0;
                            var checkKhoiCoQuan = int.TryParse(reader.GetValue(2)?.ToString(), out khoiCoQuanId);
                            var khoiCoQuanEntity = new KhoiCoQuanShort();
                            if (checkKhoiCoQuan && khoiCoQuanId != 0)
                            {
                                khoiCoQuanEntity = khoiCoQuan.Where(x => x.KhoiCoQuanId == khoiCoQuanId.ToString()).FirstOrDefault();
                                if (khoiCoQuanEntity == default)
                                    continue;
                            }
                            var unitGroup = new CoQuan()
                            {
                                Id = BsonObjectId.GenerateNewId().ToString(),
                                KhoiCoQuan = khoiCoQuanEntity,
                                Ten = reader.GetValue(1)?.ToString(),
                                ThuTu = coquanId
                            };
                            await _context.CoQuan.InsertOneAsync(unitGroup);
                        }
                    }
                }
            }

        }
    }
}