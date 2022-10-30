using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
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
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class DMHanhChinhService : BaseService, IDMHanhChinhService
    {
        private DataContext _context;
        private ILoggingService _logger;
        private IDbSettings _settings;
        private IMenuService _menuService;
        private BaseMongoDb<Xa, string> BaseMongoDbXa;
        private BaseMongoDb<Huyen, string> BaseMongoDbHuyen;

        public DMHanhChinhService(
            ILoggingService logger,
            IDbSettings settings,
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            _settings = settings;
            _logger = logger
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
            BaseMongoDbXa = new BaseMongoDb<Xa, string>(_context.Xa);
            BaseMongoDbHuyen = new BaseMongoDb<Huyen, string>(_context.Huyen);
        }

        public async Task CreateMultiHuyen(List<Huyen> list)
        {
            await _context.Huyen.InsertManyAsync(list);
        }

        public async Task CreateMultiXa(List<Xa> list)
        {
            await _context.Xa.InsertManyAsync(list);
        }

        #region Huyen

        public async Task<Huyen> CreateHuyen(Huyen model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var checkName = _context.Huyen.Find(x => x.TenHuyen == model.TenHuyen).FirstOrDefault();

            if (checkName != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);
            }

            var entity = new Huyen()
            {
                TenHuyen = model.TenHuyen,
                MaHuyen = model.MaHuyen,
                CapDonVi = model.CapDonVi,
                ModifiedAt = DateTime.Now,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                CreatedBy = CurrentUserName,
            };

            var result = await BaseMongoDbHuyen.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }

            return entity;
        }

        public async Task<Huyen> UpdateHuyen(Huyen model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.Huyen.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var checkName = _context.Huyen.Find(x => x.Id != model.Id
                                                     && x.TenHuyen.ToLower() == model.TenHuyen.ToLower()
            ).FirstOrDefault();
            if (checkName != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);
            }

            entity.TenHuyen = model.TenHuyen;
            entity.MaHuyen = model.MaHuyen;
            entity.CapDonVi = model.CapDonVi;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserName;
            var result = await BaseMongoDbHuyen.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            return entity;
        }

        public async Task DeleteHuyen(string id)
        {
            if (id == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }


            var entity = _context.Huyen.Find(x => x.Id == id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var result = await BaseMongoDbHuyen.DeleteByIdsync(entity.Id);

            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }
        }

        public async Task<List<Huyen>> GetHuyen()
        {
            return await _context.Huyen.Find(x => true).SortByDescending(x => x.CapDonVi)
                .ToListAsync();
        }

        public async Task<Huyen> GetByIdHuyen(string id)
        {
            return await _context.Huyen.Find(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<Huyen>> GetPagingHuyen(PagingParam param)
        {
            PagingModel<Huyen> result = new PagingModel<Huyen>();
            var builder = Builders<Huyen>.Filter;
            var filter = builder.Empty;
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.TenHuyen.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }
            string sortBy = nameof(LinhVuc.ThuTu);
            result.TotalRows = await _context.Huyen.CountDocumentsAsync(filter);
            var a = await _context.Huyen.Find(filter)
               .Sort(param.SortDesc
                   ? Builders<Huyen>
                       .Sort.Descending(sortBy)
                   : Builders<Huyen>
                       .Sort.Ascending(sortBy))
               .Skip(param.Skip)
               .Limit(param.Limit)
               .ToListAsync();
            result.Data = getCountXa(a).Result;
            return result;
        }

        public async Task<List<Huyen>> getCountXa(List<Huyen> list)
        {
            List<Huyen> result = new List<Huyen>();
            foreach (var item in list)
            {
                item.SoLuongXa = (int)await _context.Xa.CountDocumentsAsync(x => x.HuyenId == item.Id);
                result.Add(item);
            }
            return list;
        }





        #endregion

        #region Xa

        public async Task<Xa> CreateXa(Xa model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var checkName = _context.Xa.Find(x => x.TenXa == model.TenXa).FirstOrDefault();

            if (checkName != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);
            }

            var entity = new Xa()
            {
                TenXa = model.TenXa,
                MaXa = model.MaXa,
                HuyenId = model.HuyenId,
                CapDonVi = model.CapDonVi,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
            var result = await BaseMongoDbXa.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }
            return entity;
        }
        public async Task<Xa> UpdateXa(Xa model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.Xa.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var checkName = _context.Xa.Find(x => x.Id != model.Id
                                                  && x.TenXa.ToLower() == model.TenXa.ToLower()
            ).FirstOrDefault();
            if (checkName != default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.NAME_EXISTED);
            }

            entity.TenXa = model.TenXa;
            entity.MaXa = model.MaXa;
            entity.HuyenId = model.HuyenId;
            entity.CapDonVi = model.CapDonVi;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserName;
            var result = await BaseMongoDbXa.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            return entity;
        }
        public async Task DeleteXa(string id)
        {
            if (id == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var entity = _context.Xa.Find(x => x.Id == id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var result = await BaseMongoDbXa.DeleteByIdsync(entity.Id);

            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }
        }

        public async Task<List<Xa>> GetXa()
        {
            return await _context.Xa.Find(x => true).SortByDescending(x => x.CapDonVi)
                .ToListAsync();
        }

        public async Task<Xa> GetByIdXa(string id)
        {
            return await _context.Xa.Find(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<Xa>> GetPagingXa(ChildPaging param)
        {
            PagingModel<Xa> result = new PagingModel<Xa>();
            var builder = Builders<Xa>.Filter;
            var filter = builder.Empty;
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.TenXa.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (!String.IsNullOrEmpty(param.ParentId))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.HuyenId == param.ParentId));
            }

            string sortBy = nameof(Xa.TenXa);
            result.TotalRows = await _context.Xa.CountDocumentsAsync(filter);
            result.Data = await _context.Xa.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<Xa>
                        .Sort.Descending(sortBy)
                    : Builders<Xa>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        #endregion
    }
}