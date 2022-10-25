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
using SystemReport.WebAPI.ViewModels;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class DonViService : BaseService, IDonViService
    {
        private DataContext _context;
        private BaseMongoDb<DonVi, string> BaseMongoDb;
        private IMongoCollection<DonVi> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public DonViService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<DonVi, string>(_context.DonVis);
            _collection = context.DonVis;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.DonViCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        private string GetTenDonViCha(string donViChaId)
        {
            return _context.DonVis.Find(x => x.Id == donViChaId).FirstOrDefault()?.Ten;
        }
        public async Task<DonVi> Create(DonVi model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = new DonVi()
            {
                Id = model.Id,
                Ten = model.Ten,
                MaDonVi = model.MaDonVi,
                TenDonViCha = GetTenDonViCha(model.DonViCha),
                // MaCapCoQuan = model.MaCapCoQuan,
                // TenCapCoQuan= model.TenCapCoQuan,
                MaCapDonVi = model.MaCapDonVi,
                TenCapDonVi = model.TenCapDonVi,
                CapDV = model.CapDV,
                DonViCha = model.DonViCha,
                // MaCoQuan = model.MaCoQuan,
                // MaLoaiHinhCoQuan= model.MaLoaiHinhCoQuan,
                // TenLoaiHinhCoQuan = model.TenLoaiHinhCoQuan,
                MaLoaiHinhDonVi = model.MaLoaiHinhDonVi,
                TenLoaiHinhDonVi = model.MaLoaiHinhDonVi,
                ModifiedBy = CurrentUserName,
                CreatedBy = CurrentUserName
            };

            var result = await BaseMongoDb.CreateAsync(entity);

            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.CREATE_FAILURE);
            }

            await _logger.WithAction(nameof(this.Create)).WithActionResult(EResultResponse.SUCCESS.ToString())
                .WithContentLog(DefaultMessage.CREATE_SUCCESS).SaveChanges();
            return entity;
        }
        
        public async Task<DonVi> Update(DonVi model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Dữ liệu không được trống.");
            }

            var entity = _collection.Find(x => x.Id == model.Id).FirstOrDefault();

            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.NOT_EXIST.ToString())
                    .WithMessage("Không tìm thấy dữ liệu");
            }

            entity.Ten = model.Ten;
            // entity.MaCapCoQuan = model.MaCapCoQuan;
            // entity.TenCapCoQuan = model.TenCapCoQuan;
            entity.MaDonVi = model.MaDonVi;
            entity.MaCapDonVi = model.MaCapDonVi;
            entity.TenDonViCha = GetTenDonViCha(model.DonViCha);
            entity.TenCapDonVi = model.TenCapDonVi;
            entity.CapDV = model.CapDV;
            entity.DonViCha = model.DonViCha;
            // entity.MaCoQuan = model.MaCoQuan;
            // entity.MaLoaiHinhCoQuan = model.MaLoaiHinhCoQuan;
            // entity.TenLoaiHinhCoQuan = model.TenLoaiHinhCoQuan;
            entity.MaLoaiHinhDonVi = model.MaLoaiHinhDonVi;
            entity.TenLoaiHinhDonVi = model.MaLoaiHinhDonVi;
            entity.ModifiedBy = CurrentUserName;
            entity.ModifiedAt = DateTime.Now;

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            await _logger.WithAction(nameof(this.Update)).WithActionResult(EResultResponse.SUCCESS.ToString())
                .WithContentLog(DefaultMessage.UPDATE_SUCCESS).SaveChanges();
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

            var entity = _collection.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.ModifiedAt = DateTime.Now;
            entity.IsDeleted = true;

            var result = await BaseMongoDb.DeleteAsync(entity);

            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }

            await _logger.WithAction(nameof(this.Delete))
                .WithActionResult(EResultResponse.SUCCESS.ToString())
                .WithContentLog(DefaultMessage.DELETE_SUCCESS)
                .SaveChanges();
        }

        public async Task<IEnumerable<DonVi>> Get()
        {
            return await _collection.Find(x => x.IsDeleted != true).ToListAsync();
        }
        public async Task<IEnumerable<DonVi>> GetDonViCha()
        {
            return await _collection.Find(x => x.DonViCha == null && x.IsDeleted != true).ToListAsync();
        }
        public async Task<DonVi> GetById(string id)
        {
            return await _collection.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();
        }

        public async Task<PagingModel<DonVi>> GetPaging(DonViParam param)
        {
            PagingModel<DonVi> result = new PagingModel<DonVi>();
            var builder = Builders<DonVi>.Filter;
            var filter = builder.Empty;

            filter = builder.And(filter, builder.Eq(x => x.IsDeleted, false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }
            // if (!String.IsNullOrEmpty(param.idLinhVuc))
            // {
            //     filter = builder.And(filter,
            //         builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            // }
            string sortBy = nameof(DonVi.CreatedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<DonVi>.Sort.Ascending(sortBy)
                    : Builders<DonVi>.Sort.Descending(sortBy))
                .Skip(param.Skip).Limit(param.Limit)
                .ToListAsync();
            return result;
        }


        public async Task<List<DonViTreeVM>> GetTree()
        {
            var listDonVi = await _context.DonVis.Find(x  => x.IsDeleted ==false).SortBy(donVi => donVi.CapDV).ToListAsync();
            var parents = listDonVi.Where(x => x.DonViCha == null).ToList();
            List<DonViTreeVM> list = new List<DonViTreeVM>();
            foreach (var item in parents)
            {
                DonViTreeVM donVi = new DonViTreeVM(item);
                list.Add(donVi);
                GetLoopItem(ref list, listDonVi, donVi);
            }
            return list;
        }

        private List<DonViTreeVM> GetLoopItem(ref List<DonViTreeVM> list, List<DonVi> items, DonViTreeVM target)
        {
            try
            {
                var coquan = items.FindAll((item) => item.DonViCha == target.Id).ToList();
                if (coquan.Count > 0)
                {
                    target.Children = new List<DonViTreeVM>();
                    foreach (var item in coquan)
                    {
                        DonViTreeVM itemDV = new DonViTreeVM(item);
                        target.Children.Add(itemDV);
                        GetLoopItem(ref list, items, itemDV);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

            return null;
        }

        public List<string> GetListDonViId(string coQuanId)
        {
            var listDonVi = _context.DonVis.AsQueryable().Where(_ => true).OrderBy(donVi => donVi.CapDV).ToList();
            var parents = listDonVi.Where(x => x.Id == coQuanId).ToList();
            List<string> list = new List<string>();
            foreach (var item in parents)
            {
                list.Add(item.Id);
                GetLoopItemCoQuan(ref list, listDonVi, item.Id);
            }

            return list;
        }

        private List<string> GetLoopItemCoQuan(ref List<string> list, List<DonVi> items, string target)
        {
            try
            {
                var coquan = items.FindAll((item) => item.DonViCha == target).ToList();
                if (coquan.Count > 0)
                {
                    foreach (var item in coquan)
                    {
                        list.Add(item.Id);
                        GetLoopItemCoQuan(ref list, items, item.Id);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

            return null;
        }
    }
}