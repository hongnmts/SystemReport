using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;
using System.Runtime.CompilerServices;
using SystemReport.WebAPI.ViewModels;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SystemReport.WebAPI.Services
{
    public class HoTroDoanhNghiepService : BaseService, IHoTroDoanhNghiepService
    {
        private DataContext _context;
        private BaseMongoDb<HoTroDN, string> BaseMongoDb;
        private IMongoCollection<HoTroDN> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public HoTroDoanhNghiepService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<HoTroDN, string>(_context.HoTroDN);
            _collection = context.HoTroDN;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.LinhVucCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<HoTroDN> Create(HoTroDN model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            if(model.ToChuc != default)
            {
                if (!string.IsNullOrEmpty(model.ToChuc.Name))
                    model.ToChuc.Name=  model.ToChuc.Name.ToUpper();
            }
            var entity = new HoTroDN
            {
                ToChuc = model.ToChuc,
                LoaiHinh = model.LoaiHinh,
                DiaChi = model.DiaChi,
                DonViHanhChinh = model.DonViHanhChinh,
                NoiDungHoTro = model.NoiDungHoTro,
                SoTien = model.SoTien,
                QuyetDinh = model.QuyetDinh,
                NgayKy = model.NgayKy,
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
        public async Task<HoTroDN> Update(HoTroDN model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.HoTroDN.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.ToChuc = model.ToChuc;
            entity.LoaiHinh = model.LoaiHinh;
            entity.DiaChi = model.DiaChi;
            entity.DonViHanhChinh = model.DonViHanhChinh;
            entity.NoiDungHoTro = model.NoiDungHoTro;
            entity.SoTien = model.SoTien;
            entity.QuyetDinh = model.QuyetDinh;
            entity.NgayKy = model.NgayKy;
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


            var entity = _context.HoTroDN.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public async Task<List<HoTroDN>> Get()
        {
            return await _context.HoTroDN.Find(x => x.IsDeleted != true).SortByDescending(x => x.NgayKy)
                .ToListAsync();
        }

        public async Task<HoTroDN> GetById(string id)
        {
            return await _context.HoTroDN.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<HoTroDN>> GetPaging(PagingParam param)
        {
            var result = new PagingModel<HoTroDN>();
            var builder = Builders<HoTroDN>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            //if (!String.IsNullOrEmpty(param.Content))
            //{
            //    filter = builder.And(filter,
            //        builder.Where(x => x.Name.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            //}
            string sortBy = nameof(HoTroDN.NgayKy);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<PagingModel<HoTroDN>> GetPaging(HoTroDNVM param)
        {
            var result = new PagingModel<HoTroDN>();
            var builder = Builders<HoTroDN>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if(param.ToChuc != default && param.ToChuc.Count > 0) {
                var ids = param.ToChuc.Select(x => x.Id).ToList();
                filter &= builder.Where(x => x.ToChuc != default && ids.Contains(x.ToChuc.Id));
            }
            if (param.LoaiHinh != default && param.LoaiHinh.Count > 0)
            {
                var ids = param.LoaiHinh.Select(x => x.Id).ToList();
                filter &= builder.Where(x => x.LoaiHinh != default && ids.Contains(x.LoaiHinh.Id));
            }
            if (param.DonViHanhChinh != default && param.DonViHanhChinh.Count > 0)
            {
                var ids = param.DonViHanhChinh.Select(x => x.Id).ToList();
                filter &= builder.Where(x => x.DonViHanhChinh != default && ids.Contains(x.DonViHanhChinh.Id));
            }
            if (param.NoiDungHoTro != default && param.NoiDungHoTro.Count > 0)
            {
                var ids = param.NoiDungHoTro.Select(x => x.Id).ToList();
                filter &= builder.Where(x => x.NoiDungHoTro != default && x.NoiDungHoTro.Any(b => ids.Contains(b.Id)));
            }
            if (param.QuyetDinh != default && param.QuyetDinh.Count > 0)
            {
                var ids = param.QuyetDinh.Select(x => x.Id).ToList();
                filter &= builder.Where(x => x.QuyetDinh != default && ids.Contains(x.QuyetDinh.Id));
            }
            if (!string.IsNullOrEmpty(param.DiaChi))
            {
                filter &= builder.Where(x => x.DiaChi != default && x.DiaChi.ToLower().Contains(param.DiaChi.ToLower()));
            }
            if(param.NgayKyStart != default && param.NgayKyEnd != default)
            {
                var start = DateTime.SpecifyKind((DateTime)param.NgayKyStart, DateTimeKind.Utc);
                var end = DateTime.SpecifyKind((DateTime)param.NgayKyEnd, DateTimeKind.Utc);

                filter &= builder.Where(x => x.NgayKy >= start && x.NgayKy <= end);
            }
            else
            {
                if (param.NgayKyStart != default)
                {
                    var start = DateTime.SpecifyKind((DateTime)param.NgayKyStart, DateTimeKind.Utc);

                    filter &= builder.Where(x => x.NgayKy >= start );
                }
                if (param.NgayKyEnd != default)
                {
                    var end = DateTime.SpecifyKind((DateTime)param.NgayKyEnd, DateTimeKind.Utc);

                    filter &= builder.Where(x => x.NgayKy <= end);
                }
            }

            if (param.NgayNhapEnd != default && param.NgayNhapStart != default)
            {
                var start = DateTime.SpecifyKind((DateTime)param.NgayNhapStart, DateTimeKind.Utc);
                var end = DateTime.SpecifyKind((DateTime)param.NgayNhapEnd, DateTimeKind.Utc);

                filter &= builder.Where(x => x.ModifiedAt >= start && x.ModifiedAt <= end);
            }
            else
            {
                if (param.NgayNhapStart != default)
                {
                    var start = DateTime.SpecifyKind((DateTime)param.NgayNhapStart, DateTimeKind.Utc);

                    filter &= builder.Where(x => x.ModifiedAt >= start);
                }
                if (param.NgayNhapEnd != default)
                {
                    var end = DateTime.SpecifyKind((DateTime)param.NgayNhapEnd, DateTimeKind.Utc);

                    filter &= builder.Where(x => x.ModifiedAt <= end);
                }
            }


            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            var data = await _collection.Find(filter).SortByDescending(x => x.ModifiedAt)
                .ToListAsync();
            result.Data = await _collection.Find(filter).SortByDescending(x => x.ModifiedAt).Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            result.SoHoSo = data.Count();
            result.Money = data.Sum(x => x.SoTien);

            result.ThongKeHTDNVMs = new List<ThongKeHTDNVM>();
            result.ThongKeHTDNVMs.Add(GetThongKe(HoTroDoanhNghiep.TOCHUC, "Tổ chức", data));
            result.ThongKeHTDNVMs.Add(GetThongKe(HoTroDoanhNghiep.LOAIHINH, "Loại hình", data));
            result.ThongKeHTDNVMs.Add(GetThongKe(HoTroDoanhNghiep.HUYEN, "Huyện/TP", data));
            result.ThongKeHTDNVMs.Add(GetThongKe(HoTroDoanhNghiep.NOIDUNGHOTRO, "Nội dung hỗ trợ", data));
            result.ThongKeHTDNVMs.Add(GetThongKe(HoTroDoanhNghiep.QUYETDINH, "Quyết định", data));

            return result;
        }

        private ThongKeHTDNVM GetThongKe(string type, string name, IEnumerable<HoTroDN> data)
        {
            var commonItems = _context.CommonItem.Find(x => x.Type == type && x.IsDeleted != true).ToList();

            var result = new ThongKeHTDNVM();

            result.Id = Guid.NewGuid().ToString();
            result.Name = name;
            result.Items = new List<ThongKeHTDItem>();

            foreach (var item in commonItems)
            {
                var cItem = new ThongKeHTDItem();

                cItem.Id = item.Id;
                cItem.Name = item.Name;
                if (type == HoTroDoanhNghiep.LOAIHINH)
                {
                    cItem.Count = data.Where(x => x.LoaiHinh != default && x.LoaiHinh.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }
                    
                if (type == HoTroDoanhNghiep.TOCHUC)
                {
                    cItem.Count = data.Where(x => x.ToChuc != default && x.ToChuc.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }
   
                if (type == HoTroDoanhNghiep.QUYETDINH)
                {
                    cItem.Count = data.Where(x => x.QuyetDinh != default && x.QuyetDinh.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }

                if (type == HoTroDoanhNghiep.HUYEN)
                {
                    cItem.Count = data.Where(x => x.DonViHanhChinh != default && x.DonViHanhChinh.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }

                if (type == HoTroDoanhNghiep.NOIDUNGHOTRO)
                {
                    cItem.Count = data.Where(x => x.NoiDungHoTro != default && x.NoiDungHoTro.Any(a => a.Id == item.Id)).Count();
                    result.Count += cItem.Count;
                }
                   
                result.Items.Add(cItem);
            }

            // Khác
            var cItemKhac = new ThongKeHTDItem();

            cItemKhac.Id = "";
            cItemKhac.Name = "Khác";
            if (type == HoTroDoanhNghiep.LOAIHINH)
            {
                cItemKhac.Count = data.Where(x => x.LoaiHinh == default).Count();
                result.Count += cItemKhac.Count;
            }

            if (type == HoTroDoanhNghiep.TOCHUC)
            {
                cItemKhac.Count = data.Where(x => x.ToChuc == default).Count();
                result.Count += cItemKhac.Count;
            }

            if (type == HoTroDoanhNghiep.QUYETDINH)
            {
                cItemKhac.Count = data.Where(x => x.QuyetDinh == default).Count();
                result.Count += cItemKhac.Count;
            }

            if (type == HoTroDoanhNghiep.HUYEN)
            {
                cItemKhac.Count = data.Where(x => x.DonViHanhChinh == default).Count();
                result.Count += cItemKhac.Count;
            }
                
            if (type == HoTroDoanhNghiep.NOIDUNGHOTRO)
            {
                cItemKhac.Count = data.Where(x => x.NoiDungHoTro == default).Count();
                result.Count += cItemKhac.Count;
            }

            result.Items.Add(cItemKhac);

            return result;

        }
    }
}
