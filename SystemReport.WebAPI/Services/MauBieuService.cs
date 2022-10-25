using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
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
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class MauBieuService : BaseService, IMauBieuService
    {
        private DataContext _context;
        private BaseMongoDb<MauBieu, string> BaseMongoDb;
        private BaseMongoDb<BangBieu, string> BangBieuDb;
        private BaseMongoDb<RowValue, string> RowValueDb;
        private BaseMongoDb<ThuocTinh, string> ThuocTinhDb;
        private IMongoCollection<MauBieu> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;
        private IRowValueService _rowValueService;
        private IBangBieuService _bangBieuService;
        private IDonViService _donViService;
        private IHistoryMauBieuService _historyMauBieu;

        public MauBieuService(ILoggingService logger, IDbSettings settings, DataContext context,
            IRowValueService rowValueService,
            IBangBieuService bangBieuService,
            IDonViService donViService,
            IHistoryMauBieuService historyMauBieu,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<MauBieu, string>(_context.MauBieu);
            BangBieuDb = new BaseMongoDb<BangBieu, string>(_context.BangBieu);
            RowValueDb = new BaseMongoDb<RowValue, string>(_context.RowValue);
            ThuocTinhDb = new BaseMongoDb<ThuocTinh, string>(_context.ThuocTinh);
            _collection = context.MauBieu;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.MauBieuCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);

            _rowValueService = rowValueService;
            _bangBieuService = bangBieuService;
            _donViService = donViService;
            _historyMauBieu = historyMauBieu;
        }

        public async Task<MauBieu> Create(MauBieu model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var code = CommonExtensions.GenerateNewRandomDigit();
            var entity = new MauBieu
            {
                Ten = model.Ten,
                TenNgan = model.TenNgan,
                Code = code,
                KyHieu = model.KyHieu,
                DonViNhan = model.DonViNhan,
                DonViBaoCao = model.DonViBaoCao,
                LoaiMauBieu = model.LoaiMauBieu,
                PhanQuyenDonVi = model.PhanQuyenDonVi,
                TuNgay = model.TuNgay,
                DenNgay = model.DenNgay,
                IsTemplate = model.IsTemplate,
                ThuTu = model.ThuTu,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
            };

            if (entity.PhanQuyenDonVi != default)
            {
                entity.PhanQuyenDonViIds = new List<string>();
                foreach (var item in model.PhanQuyenDonVi)
                {
                    entity.PhanQuyenDonViIds.AddRange(_donViService.GetListDonViId(item.Id));
                }
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

        public async Task<MauBieu> Update(MauBieu model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.MauBieu.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.Ten = model.Ten;
            entity.TenNgan = model.TenNgan;
            entity.KyHieu = model.KyHieu;
            entity.DonViNhan = model.DonViNhan;
            entity.DonViBaoCao = model.DonViBaoCao;
            entity.LoaiMauBieu = model.LoaiMauBieu;
            entity.PhanQuyenDonVi = model.PhanQuyenDonVi;
            entity.TuNgay = model.TuNgay;
            entity.DenNgay = model.DenNgay;
            entity.ThuTu = model.ThuTu;
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserName;

            if (entity.PhanQuyenDonVi != default)
            {
                if (entity.PhanQuyenDonViIds == default)
                {
                    entity.PhanQuyenDonViIds = new List<string>();
                }
                foreach (var item in entity.PhanQuyenDonVi)
                {
                    entity.PhanQuyenDonViIds.AddRange(_donViService.GetListDonViId(item.Id));
                }
            }
            else
            {
                entity.PhanQuyenDonViIds.Clear();
            }

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            // history mẫu biểu
            await _historyMauBieu.WithFormKey(entity.Id)
                  .WithCollection(_settings.MauBieuCollectionName, entity.Id, "Mẫu biểu")
                  .WithTitle("Cập nhật mẫu biểu")
                  .WithStatus(entity.LastStatus)
                  .WithAction(nameof(this.Update))
                  .WithUserName(CurrentUser)
                  .WithOldValue(model)
                  .SaveChangeHistory();
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


            var entity = _context.MauBieu.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public async Task<List<MauBieu>> Get()
        {
            return await _context.MauBieu.Find(x => x.IsDeleted != true).SortByDescending(x => x.ThuTu)
                .ToListAsync();
        }

        public async Task<MauBieu> GetById(string id)
        {
            return await _context.MauBieu.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<MauBieu>> GetPaging(MauBieuParam param)
        {
            PagingModel<MauBieu> result = new PagingModel<MauBieu>();
            var builder = Builders<MauBieu>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsTemplate == true && x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (param.LoaiMauBieu != default)
            {
                filter = builder.And(filter,
                    builder.Where(x => x.LoaiMauBieu != default && x.LoaiMauBieu.Id == param.LoaiMauBieu.Id));
            }

            string sortBy = nameof(MauBieu.ThuTu);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<MauBieu>
                        .Sort.Descending(sortBy)
                    : Builders<MauBieu>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<DemoMauBieuVM> RenderTable(string bangBieuId)
        {
            var bangBieu = _context.BangBieu.Find(x => x.Id == bangBieuId && x.IsDeleted != true).FirstOrDefault();
            var headers = await _bangBieuService.RenderHeader(bangBieuId);
            var body = await _rowValueService.RenderBodyMainByBangBieuId(bangBieuId);

            var table = new DemoMauBieuVM();
            table.TenBangBieu = bangBieu.Ten;
            table.IsHienThiTen = bangBieu.HienThiTen;
            table.ThuTu = bangBieu.ThuTu;
            table.Headers = headers;
            table.Body = body;
            return table;
        }

        public async Task<List<DemoMauBieuVM>> RenderTableMauBieu(string mauBieuId)
        {
            var data = new List<DemoMauBieuVM>();
            var mauBieu = _context.MauBieu.Find(x => x.Id == mauBieuId && x.IsDeleted != true).FirstOrDefault();
            var bangBieus = _context.BangBieu.Find(x => x.MauBieuId == mauBieu.Id && x.IsDeleted != true)
                .SortBy(x => x.ThuTu).ToList();
            foreach (var bb in bangBieus)
            {
                var headers = await _bangBieuService.RenderHeader(bb.Id);
                var body = await _rowValueService.RenderBodyMainByBangBieuId(bb.Id);

                var table = new DemoMauBieuVM();
                table.TenBangBieu = bb.Ten;
                table.IsHienThiTen = bb.HienThiTen;
                table.ThuTu = bb.ThuTu;
                table.Headers = headers;
                table.Body = body;

                data.Add(table);
            }

            return data;
        }

        #region Bao cao

        public async Task<PagingModel<MauBieu>> GetPagingCaNhan(MauBieuParam param)
        {
            PagingModel<MauBieu> result = new PagingModel<MauBieu>();
            var builder = Builders<MauBieu>.Filter;
            var filter = builder.Empty;
            if (CurrentUser.DonVi != default)
            {
            }

            filter = builder.And(filter,
                builder.Where(x => x.IsTemplate == true &&
                                   x.PhanQuyenDonVi != default &&
                                   x.PhanQuyenDonVi.Any(x => x.Id == CurrentUser.DonVi.Id) &&
                                   x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (param.LoaiMauBieu != default)
            {
                filter = builder.And(filter,
                    builder.Where(x => x.LoaiMauBieu != default && x.LoaiMauBieu.Id == param.LoaiMauBieu.Id));
            }

            string sortBy = nameof(MauBieu.ThuTu);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<MauBieu>
                        .Sort.Descending(sortBy)
                    : Builders<MauBieu>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<PagingModel<MauBieu>> GetPagingNhapLieu(MauBieuParam param)
        {
            PagingModel<MauBieu> result = new PagingModel<MauBieu>();
            var builder = Builders<MauBieu>.Filter;
            var filter = builder.Empty;

            if (!CurrentUser.Roles.Any(x => x.Code == DefaultRoleCode.BAO_CAO_VIEN))
            {
                result.TotalRows = 0;
                result.Data = new List<MauBieu>();
                return result;
            }

            filter = builder.And(filter,
                builder.Where(x => x.IsTemplate == false
                                   && x.PhanQuyenDonVi != default
                                   && x.PhanQuyenDonViIds != default
                                   && x.PhanQuyenDonViIds.Count > 0
                                   && x.PhanQuyenDonViIds.Contains(CurrentUser.DonVi.Id)
                                   && x.IsDeleted == false));

            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (param.LoaiMauBieu != default)
            {

                filter = builder.And(filter,
                    builder.Where(x => x.LoaiMauBieu != default && x.LoaiMauBieu.Id == param.LoaiMauBieu.Id));
            }


            if (param.ParamMauBieu != default && param.ParamMauBieu.Nam != default)
            {
                DateTime firstDay = DateTime.Now;
                DateTime lastDay = DateTime.Now;
                if (param.ParamMauBieu.Nam.HasValue)
                {
                    firstDay = new DateTime(param.ParamMauBieu.Nam.Value.ToLocalTime().Year, 1, 1);
                    lastDay = firstDay.AddYears(1).AddTicks(-1);

                    filter = builder.And(filter,
                        builder.Where(x => x.DenNgay != default && x.DenNgay <= lastDay && x.DenNgay >= firstDay));
                }

            }
            if (param.ParamMauBieu != default && param.ParamMauBieu.KyBaoCao != default)
            {
                // Tính từ ngày đến ngày
                if (param.ParamMauBieu.KyBaoCao != default)
                {
                    bool checkDay = false;

                    DateTime firstDay = DateTime.Now;
                    DateTime lastDay = DateTime.Now;

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.NAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.THANG && param.ParamMauBieu.Thang != default)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, int.Parse(param.ParamMauBieu.Thang.Id), 1);
                        lastDay = firstDay.AddMonths(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao._6THANGDAUNAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddMonths(6).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao._6THANGCUOINAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 7, 1);
                        lastDay = new DateTime(year, 1, 1).AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYI)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYII)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 4, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYIII)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 8, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYIV)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 10, 1);
                        lastDay = new DateTime(year, 1, 1).AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    //if (checkDay)
                    //{
                    //    filter = filter & builder.Gte(x => x.TuNgay, firstDay)
                    //    & builder.Lt(x => x.DenNgay, lastDay);
                    //}
                }
            }

            string sortBy = nameof(MauBieu.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            var data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();

            var rl = new List<MauBieu>();
            foreach (var item in data)
            {
                item.BangBieus = _context.BangBieu.Find(x => x.MauBieuId == item.Id && x.IsDeleted != true).ToList();

                rl.Add(item);
            }

            result.Data = rl;
            return result;
        }

        public async Task<PagingModel<MauBieu>> GetPagingKiemTra(MauBieuParam param)
        {
            PagingModel<MauBieu> result = new PagingModel<MauBieu>();
            var builder = Builders<MauBieu>.Filter;
            var filter = builder.Empty;


            if (!CurrentUser.Roles.Any(x => x.Code == DefaultRoleCode.LANH_DAO_DON_VI))
            {
                result.TotalRows = 0;
                result.Data = new List<MauBieu>();
                return result;
            }

            filter = builder.And(filter,
                builder.Where(x => x.IsTemplate == false
                                        //&& x.ListStatus.Any(x => x.Code == "KT")
                                        && x.PhanQuyenDonVi != default
                                        && x.PhanQuyenDonVi.Any(x => x.Id == CurrentUser.DonVi.Id)
                                        && x.IsDeleted == false));

            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (param.LoaiMauBieu != default)
            {
                filter = builder.And(filter,
                    builder.Where(x => x.LoaiMauBieu != default && x.LoaiMauBieu.Id == param.LoaiMauBieu.Id));
            }
            if (param.ParamMauBieu != default && param.ParamMauBieu.Nam != default)
            {
                DateTime firstDay = DateTime.Now;
                DateTime lastDay = DateTime.Now;
                if (param.ParamMauBieu.Nam.HasValue)
                {
                    firstDay = new DateTime(param.ParamMauBieu.Nam.Value.ToLocalTime().Year, 1, 1);
                    lastDay = firstDay.AddYears(1).AddTicks(-1);

                    filter = builder.And(filter,
                        builder.Where(x => x.DenNgay != default && x.DenNgay <= lastDay && x.DenNgay >= firstDay));
                }

            }

            if (param.ParamMauBieu != default && param.ParamMauBieu.KyBaoCao != default)
            {
                // Tính từ ngày đến ngày
                if (param.ParamMauBieu.KyBaoCao != default)
                {
                    bool checkDay = false;

                    DateTime firstDay = DateTime.Now;
                    DateTime lastDay = DateTime.Now;

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.NAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.THANG && param.ParamMauBieu.Thang != default)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, int.Parse(param.ParamMauBieu.Thang.Id), 1);
                        lastDay = firstDay.AddMonths(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao._6THANGDAUNAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddMonths(6).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao._6THANGCUOINAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 7, 1);
                        lastDay = new DateTime(year, 1, 1).AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYI)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYII)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 4, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYIII)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 8, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYIV)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 10, 1);
                        lastDay = new DateTime(year, 1, 1).AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    //if (checkDay)
                    //{
                    //    filter = filter & builder.Gte(x => x.TuNgay, firstDay)
                    //    & builder.Lt(x => x.DenNgay, lastDay);
                    //}
                }
            }

            string sortBy = nameof(MauBieu.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            var data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            var rl = new List<MauBieu>();
            foreach (var item in data)
            {
                item.BangBieus = _context.BangBieu.Find(x => x.MauBieuId == item.Id && x.IsDeleted != true).ToList();

                rl.Add(item);
            }

            result.Data = rl;
            return result;
        }

        public async Task<PagingModel<MauBieu>> GetPagingTongHop(MauBieuParam param)
        {
            PagingModel<MauBieu> result = new PagingModel<MauBieu>();
            var builder = Builders<MauBieu>.Filter;
            var filter = builder.Empty;


            if (!CurrentUser.Roles.Any(x => x.Code == DefaultRoleCode.CAN_BO_TONG_HOP))
            {
                result.TotalRows = 0;
                result.Data = new List<MauBieu>();
                return result;
            }

            if (CurrentUser.DonVi.MaDonVi == "VPS")
            {
                filter = builder.And(filter,
                    builder.Where(x => x.IsTemplate == false
                                       && x.IsDeleted == false));
            }
            else
            {
                filter = builder.And(filter,
                    builder.Where(x => x.IsTemplate == false
                                       && x.PhanQuyenDonVi != default
                                       && x.PhanQuyenDonViIds != default
                                       && x.PhanQuyenDonViIds.Count > 0
                                       && x.PhanQuyenDonViIds.Contains(CurrentUser.DonVi.Id)
                                       && x.IsDeleted == false));
            }


            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (param.LoaiMauBieu != default)
            {
                filter = builder.And(filter,
                    builder.Where(x => x.LoaiMauBieu != default && x.LoaiMauBieu.Id == param.LoaiMauBieu.Id));
            }

            if (param.ParamMauBieu != default && param.ParamMauBieu.Nam != default)
            {
                DateTime firstDay = DateTime.Now;
                DateTime lastDay = DateTime.Now;
                if (param.ParamMauBieu.Nam.HasValue)
                {
                    firstDay = new DateTime(param.ParamMauBieu.Nam.Value.ToLocalTime().Year, 1, 1);
                    lastDay = firstDay.AddYears(1).AddTicks(-1);

                    filter = builder.And(filter,
                        builder.Where(x => x.DenNgay != default && x.DenNgay <= lastDay && x.DenNgay >= firstDay));
                }
            }

            if (param.ParamMauBieu != default && param.ParamMauBieu.KyBaoCao != default)
            {
                // Tính từ ngày đến ngày
                if (param.ParamMauBieu.KyBaoCao != default)
                {
                    bool checkDay = false;

                    DateTime firstDay = DateTime.Now;
                    DateTime lastDay = DateTime.Now;

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.NAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.THANG && param.ParamMauBieu.Thang != default)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, int.Parse(param.ParamMauBieu.Thang.Id), 1);
                        lastDay = firstDay.AddMonths(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao._6THANGDAUNAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddMonths(6).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao._6THANGCUOINAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 7, 1);
                        lastDay = new DateTime(year, 1, 1).AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYI)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYII)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 4, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYIII)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 8, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYIV)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 10, 1);
                        lastDay = new DateTime(year, 1, 1).AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    //if (checkDay)
                    //{
                    //    filter = filter & builder.Gte(x => x.TuNgay, firstDay)
                    //    & builder.Lt(x => x.DenNgay, lastDay);
                    //}
                }
            }

            string sortBy = nameof(MauBieu.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            var data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();

            var rl = new List<MauBieu>();
            foreach (var item in data)
            {
                item.BangBieus = _context.BangBieu.Find(x => x.MauBieuId == item.Id && x.IsDeleted != true).ToList();

                rl.Add(item);
            }

            result.Data = rl;
            return result;
        }

        public async Task<PagingModel<MauBieu>> GetPagingXuatBan(MauBieuParam param)
        {

            PagingModel<MauBieu> result = new PagingModel<MauBieu>();
            var builder = Builders<MauBieu>.Filter;
            var filter = builder.Empty;

            if (!CurrentUser.Roles.Any(x => x.Code == DefaultRoleCode.LANH_DAO_SO))
            {
                result.TotalRows = 0;
                result.Data = new List<MauBieu>();
                return result;
            }

            if (CurrentUser.DonVi.MaDonVi == "LDS")
            {
                filter = builder.And(filter,
                    builder.Where(x => x.IsTemplate == false
                                       //&& x.ListStatus.Any(x => x.Code == "XB")
                                       && x.IsDeleted == false));
            }
            else
            {
                filter = builder.And(filter,
                    builder.Where(x => x.IsTemplate == false
                                       //&& x.LastStatus != default && x.LastStatus.Code == "XB"
                                       && x.IsDeleted == false));
            }


            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (param.LoaiMauBieu != default)
            {
                filter = builder.And(filter,
                    builder.Where(x => x.LoaiMauBieu != default && x.LoaiMauBieu.Id == param.LoaiMauBieu.Id));
            }


            if (param.ParamMauBieu != default && param.ParamMauBieu.Nam != default)
            {
                DateTime firstDay = DateTime.Now;
                DateTime lastDay = DateTime.Now;
                if (param.ParamMauBieu.Nam.HasValue)
                {
                    firstDay = new DateTime(param.ParamMauBieu.Nam.Value.ToLocalTime().Year, 1, 1);
                    lastDay = firstDay.AddYears(1).AddTicks(-1);

                    filter = builder.And(filter,
                        builder.Where(x => x.DenNgay != default && x.DenNgay <= lastDay && x.DenNgay >= firstDay));
                }

            }
            if (param.ParamMauBieu != default && param.ParamMauBieu.KyBaoCao != default)
            {
                // Tính từ ngày đến ngày
                if (param.ParamMauBieu.KyBaoCao != default)
                {
                    bool checkDay = false;

                    DateTime firstDay = DateTime.Now;
                    DateTime lastDay = DateTime.Now;

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.NAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.THANG && param.ParamMauBieu.Thang != default)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, int.Parse(param.ParamMauBieu.Thang.Id), 1);
                        lastDay = firstDay.AddMonths(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao._6THANGDAUNAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddMonths(6).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao._6THANGCUOINAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 7, 1);
                        lastDay = new DateTime(year, 1, 1).AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYI)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYII)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 4, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYIII)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 8, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYIV)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 10, 1);
                        lastDay = new DateTime(year, 1, 1).AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    //if (checkDay)
                    //{
                    //    filter = filter & builder.Gte(x => x.TuNgay, firstDay)
                    //    & builder.Lt(x => x.DenNgay, lastDay);
                    //}
                }
            }

            string sortBy = nameof(MauBieu.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            var data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();

            var rl = new List<MauBieu>();
            foreach (var item in data)
            {
                item.BangBieus = _context.BangBieu.Find(x => x.MauBieuId == item.Id && x.IsDeleted != true).ToList();

                rl.Add(item);
            }

            result.Data = rl;
            return result;
        }

        public async Task<PagingModel<MauBieu>> GetPagingThongTinXuatBan(MauBieuParam param)
        {

            PagingModel<MauBieu> result = new PagingModel<MauBieu>();
            var builder = Builders<MauBieu>.Filter;
            var filter = builder.Empty;

            filter = builder.And(filter,
                  builder.Where(x => x.IsTemplate == false
                                    && x.LastStatus != default
                                     && x.LastStatus.Code == "XB"
                                     && x.IsDeleted == false));


            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            if (param.LoaiMauBieu != default)
            {
                filter = builder.And(filter,
                    builder.Where(x => x.LoaiMauBieu != default && x.LoaiMauBieu.Id == param.LoaiMauBieu.Id));
            }


            if (param.ParamMauBieu != default && param.ParamMauBieu.Nam != default)
            {
                DateTime firstDay = DateTime.Now;
                DateTime lastDay = DateTime.Now;
                if (param.ParamMauBieu.Nam.HasValue)
                {
                    firstDay = new DateTime(param.ParamMauBieu.Nam.Value.ToLocalTime().Year, 1, 1);
                    lastDay = firstDay.AddYears(1).AddTicks(-1);

                    filter = builder.And(filter,
                        builder.Where(x => x.DenNgay != default && x.DenNgay <= lastDay && x.DenNgay >= firstDay));
                }

            }
            if (param.ParamMauBieu != default && param.ParamMauBieu.KyBaoCao != default)
            {
                // Tính từ ngày đến ngày
                if (param.ParamMauBieu.KyBaoCao != default)
                {
                    bool checkDay = false;

                    DateTime firstDay = DateTime.Now;
                    DateTime lastDay = DateTime.Now;

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.NAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.THANG && param.ParamMauBieu.Thang != default)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, int.Parse(param.ParamMauBieu.Thang.Id), 1);
                        lastDay = firstDay.AddMonths(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao._6THANGDAUNAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddMonths(6).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao._6THANGCUOINAM)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 7, 1);
                        lastDay = new DateTime(year, 1, 1).AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYI)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 1, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYII)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 4, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYIII)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 8, 1);
                        lastDay = firstDay.AddMonths(3).AddTicks(-1);
                        checkDay = true;
                    }

                    if (param.ParamMauBieu.KyBaoCao.Code == CodeKyBaoCao.QUYIV)
                    {
                        int year = param.ParamMauBieu.NamLocal;
                        firstDay = new DateTime(year, 10, 1);
                        lastDay = new DateTime(year, 1, 1).AddYears(1).AddTicks(-1);
                        checkDay = true;
                    }

                    //if (checkDay)
                    //{
                    //    filter = filter & builder.Gte(x => x.TuNgay, firstDay)
                    //    & builder.Lt(x => x.DenNgay, lastDay);
                    //}
                }
            }

            string sortBy = nameof(MauBieu.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            var data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();

            var rl = new List<MauBieu>();
            foreach (var item in data)
            {
                item.BangBieus = _context.BangBieu.Find(x => x.MauBieuId == item.Id && x.IsDeleted != true).ToList();

                rl.Add(item);
            }

            result.Data = rl;
            return result;
        }
        #endregion

        [Obsolete]
        public async Task GenerateMauBieu(InputMauBieuModel model)
        {
            try
            {
                if (model == default)
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage(DefaultMessage.DATA_NOT_FOUND);
                }

                var mauBieu = _context.MauBieu.Find(x => x.Id == model.MauBieuId && x.IsDeleted != true)
                    .FirstOrDefault();
                var code = CommonExtensions.GenerateNewRandomDigit();
                if (mauBieu != default)
                {
                    // Tạo mẫu biểu mới
                    var newMauBieu = new MauBieu();
                    newMauBieu = (MauBieu)mauBieu.Clone();

                    newMauBieu.CloneId = mauBieu.Id;
                    newMauBieu.Code = code;
                    newMauBieu.IsTemplate = false;
                    newMauBieu.Id = BsonObjectId.GenerateNewId().ToString();

                    newMauBieu.CreatedBy = CurrentUserName;
                    newMauBieu.ModifiedBy = CurrentUserName;
                    newMauBieu.ModifiedAt = DateTime.Now;
                    newMauBieu.CreatedAt = DateTime.Now;
                    // Người sở hữu mẫu biểu
                    newMauBieu.OwerId = CurrentUserName;
                    newMauBieu.OwerIds = new List<string>() { CurrentUserName };

                    // Trạng thái của mẫu biểu
                    var trangThai = _context.TrangThai.Find(x => x.Code.ToUpper() == StatusForm.NHAP_LIEU).FirstOrDefault();
                    if (trangThai == default)
                    {
                        throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Không tìm thấy trạng thái!");
                    }
                    trangThai.CurrentUser = CurrentUser;
                    newMauBieu.LastStatus = trangThai;
                    newMauBieu.ListStatus = new List<TrangThai>();
                    newMauBieu.ListStatus.Add(trangThai);

                    // Tính từ ngày đến ngày
                    if (model.KyBaoCao != default)
                    {
                        newMauBieu.KyBaoCao = model.KyBaoCao;

                        if (model.KyBaoCao.Code == CodeKyBaoCao.NAM)
                        {
                            int year = model.NamLocal;
                            DateTime firstDay = new DateTime(year, 1, 1);
                            DateTime lastDay = firstDay.AddYears(1).AddTicks(-1);

                            newMauBieu.TuNgay = firstDay;
                            newMauBieu.DenNgay = lastDay;
                        }

                        if (model.KyBaoCao.Code == CodeKyBaoCao.THANG)
                        {
                            int year = model.NamLocal;
                            DateTime firstDay = new DateTime(year, int.Parse(model.Thang.Id), 1);
                            DateTime lastDay = firstDay.AddMonths(1).AddTicks(-1);

                            newMauBieu.TuNgay = firstDay;
                            newMauBieu.DenNgay = lastDay;
                        }

                        if (model.KyBaoCao.Code == CodeKyBaoCao._6THANGDAUNAM)
                        {
                            int year = model.NamLocal;
                            DateTime firstDay = new DateTime(year, 1, 1);
                            DateTime lastDay = firstDay.AddMonths(6).AddTicks(-1);

                            newMauBieu.TuNgay = firstDay;
                            newMauBieu.DenNgay = lastDay;
                        }

                        if (model.KyBaoCao.Code == CodeKyBaoCao._6THANGCUOINAM)
                        {
                            int year = model.NamLocal;
                            DateTime firstDay = new DateTime(year, 7, 1);
                            DateTime lastDay = new DateTime(year, 1, 1).AddYears(1).AddTicks(-1);

                            newMauBieu.TuNgay = firstDay;
                            newMauBieu.DenNgay = lastDay;
                        }

                        if (model.KyBaoCao.Code == CodeKyBaoCao.QUYI)
                        {
                            int year = model.NamLocal;
                            DateTime firstDay = new DateTime(year, 1, 1);
                            DateTime lastDay = firstDay.AddMonths(3).AddTicks(-1);

                            newMauBieu.TuNgay = firstDay;
                            newMauBieu.DenNgay = lastDay;
                        }

                        if (model.KyBaoCao.Code == CodeKyBaoCao.QUYII)
                        {
                            int year = model.NamLocal;
                            DateTime firstDay = new DateTime(year, 4, 1);
                            DateTime lastDay = firstDay.AddMonths(3).AddTicks(-1);

                            newMauBieu.TuNgay = firstDay;
                            newMauBieu.DenNgay = lastDay;
                        }

                        if (model.KyBaoCao.Code == CodeKyBaoCao.QUYIII)
                        {
                            int year = model.NamLocal;
                            DateTime firstDay = new DateTime(year, 8, 1);
                            DateTime lastDay = firstDay.AddMonths(3).AddTicks(-1);

                            newMauBieu.TuNgay = firstDay;
                            newMauBieu.DenNgay = lastDay;
                        }

                        if (model.KyBaoCao.Code == CodeKyBaoCao.QUYIV)
                        {
                            int year = model.NamLocal;
                            DateTime firstDay = new DateTime(year, 10, 1);
                            DateTime lastDay = new DateTime(year, 1, 1).AddYears(1).AddTicks(-1);

                            newMauBieu.TuNgay = firstDay;
                            newMauBieu.DenNgay = lastDay;
                        }

                        if (model.KyBaoCao.Code == CodeKyBaoCao.GIAIDOAN)
                        {
                            newMauBieu.TuNgay = model.TuNgay;
                            newMauBieu.DenNgay = model.DenNgay;
                        }
                    }

                    await BaseMongoDb.CreateAsync(newMauBieu);
                    var oldBangBieu = _context.BangBieu
                        .Find(x => x.MauBieuId == newMauBieu.CloneId && x.IsDeleted != true)
                        .ToList();

                    if (oldBangBieu.Count <= 0)
                    {
                        throw new ResponseMessageException()
                            .WithCode(EResultResponse.FAIL.ToString())
                            .WithMessage("Không tìm thấy bảng biểu");
                    }

                    foreach (var bb in oldBangBieu)
                    {
                        var newBangBieu = new BangBieu();
                        newBangBieu = (BangBieu)bb.Clone();
                        newBangBieu.Id = BsonObjectId.GenerateNewId().ToString();
                        newBangBieu.MauBieuId = newMauBieu.Id;
                        newBangBieu.CloneId = bb.Id;
                        newBangBieu.CreatedBy = CurrentUserName;
                        newBangBieu.ModifiedBy = CurrentUserName;
                        newBangBieu.ModifiedAt = DateTime.Now;
                        newBangBieu.CreatedAt = DateTime.Now;
                        await BangBieuDb.CreateAsync(newBangBieu);

                        #region TinhToanThuocTinh

                        // Tạo thuộc tính

                        var thuocTinhs = _context.ThuocTinh.Find(x => x.BangBieuId == bb.Id && x.IsDeleted != true)
                            .ToList();
                        if (thuocTinhs.Count <= 0)
                        {
                            throw new ResponseMessageException()
                                .WithCode(EResultResponse.FAIL.ToString())
                                .WithMessage("Không tìm thấy thuộc tính");
                        }
                        var keyRowThuocTinh = BsonObjectId.GenerateNewId().ToString();
                        var compareThuocTinh = new List<CompareThuocTinh>();
                        foreach (var tt in thuocTinhs)
                        {
                            var cpThuocTinh = new CompareThuocTinh();


                            var newThuocTinh = new ThuocTinh();
                            newThuocTinh = (ThuocTinh)tt.Clone();
                            newThuocTinh.BangBieuId = newBangBieu.Id;
                            newThuocTinh.MauBieuId = newBangBieu.MauBieuId;
                            newThuocTinh.CloneId = tt.Id;
                            newThuocTinh.Id = BsonObjectId.GenerateNewId().ToString();
                            newThuocTinh.CreatedBy = CurrentUserName;
                            newThuocTinh.ModifiedBy = CurrentUserName;
                            newThuocTinh.Order = tt.Order;
                            newThuocTinh.ModifiedAt = DateTime.Now;
                            newThuocTinh.CreatedAt = DateTime.Now;
                            cpThuocTinh.NewValue = newThuocTinh.Id;
                            cpThuocTinh.OldValue = tt.Id;

                            compareThuocTinh.Add(cpThuocTinh);
                            await ThuocTinhDb.CreateAsync(newThuocTinh);
                        }

                        // sync thuoc tinh parent
                        var newThuocTinhs = _context.ThuocTinh
                            .Find(x => x.BangBieuId == newBangBieu.Id && x.IsDeleted != true).ToList();

                        foreach (var tt in newThuocTinhs)
                        {
                            var findTT = compareThuocTinh.Where(x => x.OldValue == tt.ParentId).FirstOrDefault();
                            if (findTT != default)
                            {
                                tt.ParentId = findTT.NewValue;
                                await ThuocTinhDb.UpdateAsync(tt);
                            }
                        }

                        // Kết thúc tính toán thuộc tính

                        #endregion

                        #region TinhToanChiTieu

                        // Tạo thuộc tính

                        var _listNewThuocTinh = _context.ThuocTinh
                            .Find(x => x.BangBieuId == newBangBieu.Id && x.IsDeleted != true).ToList();
                        var rowValues = _context.RowValue.Find(x => x.BangBieuId == bb.Id && x.IsDeleted != true)
                            .ToList();
                        var compareRowValue = new List<CompareThuocTinh>();


                        var groupByRowValues = rowValues.GroupBy(x => x.KeyRow).Select(x => new BodyTableVM() { KeyRow = x.Key, RowValues = x.ToList() }).ToList();
                        foreach (var gbValue in groupByRowValues)
                        {
                            var keyRowRowValue = BsonObjectId.GenerateNewId().ToString();
                            if (gbValue.RowValues.Count > 0)
                            {
                                var rows = gbValue.RowValues;
                                foreach (var tt in rows)
                                {
                                    var cpRowValue = new CompareThuocTinh();

                                    var newRowValue = new RowValue();

                                    newRowValue = (RowValue)tt.Clone();
                                    newRowValue.KeyRow = keyRowRowValue;
                                    newRowValue.BangBieuId = newBangBieu.Id;
                                    newRowValue.MauBieuId = newBangBieu.MauBieuId;
                                    newRowValue.CloneId = tt.Id;
                                    newRowValue.ThuocTinhId =
                                        _listNewThuocTinh.FirstOrDefault(x => x.CloneId == tt.ThuocTinhId)?.Id;

                                    newRowValue.Id = BsonObjectId.GenerateNewId().ToString();
                                    newRowValue.CreatedBy = CurrentUserName;
                                    newRowValue.ModifiedBy = CurrentUserName;
                                    newRowValue.ModifiedAt = DateTime.Now;
                                    newRowValue.CreatedAt = DateTime.Now;
                                    newRowValue.Order = tt.Order;
                                    cpRowValue.NewValue = newRowValue.KeyRow;
                                    cpRowValue.OldValue = tt.KeyRow;
                                    compareRowValue.Add(cpRowValue);
                                    await RowValueDb.CreateAsync(newRowValue);
                                }
                            }
                        }

                        // sync thuoc tinh parent
                        var newRowValues = _context.RowValue
                            .Find(x => x.BangBieuId == newBangBieu.Id && x.ParentId != null && x.IsDeleted != true).ToList();

                        foreach (var tt in newRowValues)
                        {
                            var findTT = compareRowValue.Where(x => x.OldValue == tt.ParentId).FirstOrDefault();
                            if (findTT != default)
                            {
                                tt.ParentId = findTT.NewValue;
                                await RowValueDb.UpdateAsync(tt);
                            }
                        }

                        // Kết thúc tính toán thuộc tính

                        #endregion
                    }

                    await _historyMauBieu.WithFormKey(newMauBieu.Id)
                            .WithCollection(_settings.MauBieuCollectionName, newMauBieu.Id, "Mẫu biểu")
                            .WithTitle("Tạo mới mẫu biểu")
                            .WithStatus(newMauBieu.LastStatus)
                            .WithAction(nameof(this.GenerateMauBieu))
                            .WithUserName(CurrentUser)
                            .SaveChangeHistory();
                }
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }
            catch (Exception e)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(e.Message);
            }
        }

        public async Task DeleteMauBieu(string mauBieuId)
        {
            var mauBieu = _context.MauBieu.Find(x => x.Id == mauBieuId).FirstOrDefault();
            if (mauBieu == default)
            {
                throw new ResponseMessageException()
           .WithCode(EResultResponse.FAIL.ToString())
           .WithMessage("Không tìm thấy mẫu biểu!");
            }

            var bangBieus = _context.BangBieu.Find(x => x.MauBieuId == mauBieu.Id).ToList();
            if (bangBieus.Count < 0)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy bảng biểu!");
            }

            foreach (var bb in bangBieus)
            {
                var thuocTinh = _context.ThuocTinh.Find(x => x.BangBieuId == bb.Id).ToList();
                var values = _context.RowValue.Find(x => x.BangBieuId == bb.Id).ToList();

                if (thuocTinh.Count < 0 || values.Count < 0)
                {
                    throw new ResponseMessageException()
                        .WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Không tìm thấy dữ liệu!");
                }
                var idsFilterThuocTinh = Builders<ThuocTinh>.Filter.In(d => d.Id, thuocTinh.Select(x => x.Id).ToList());
                var deleteResultThuocTinh = await _context.ThuocTinh.DeleteManyAsync(idsFilterThuocTinh);

                var idsFilterValues = Builders<RowValue>.Filter.In(d => d.Id, values.Select(x => x.Id).ToList());
                var deleteResultValues = await _context.RowValue.DeleteManyAsync(idsFilterValues);

                await BangBieuDb.DeletedAsync(bb);
            }

            await BaseMongoDb.DeletedAsync(mauBieu);
        }

        public async Task ChangeStatus(TrangThaiParam model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy dữ liệu!");
            }

            var trangThai = _context.TrangThai.Find(x => x.Code.ToUpper() == model.NewTrangThai.Code.ToUpper() && x.IsDeleted != true).FirstOrDefault();
            if (trangThai == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy trạng thái!");
            }
            var mauBieu = _context.MauBieu.Find(x => x.Id == model.MauBieuId && x.IsDeleted != true).FirstOrDefault();

            if (mauBieu == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy mẫu biểu!");
            }

            
            
            trangThai.Content = model.NoiDung;
            trangThai.CurrentUser = CurrentUser;

            mauBieu.LastStatus = trangThai;
            mauBieu.ListStatus.Add(trangThai);
            mauBieu.OwerId = CurrentUserName;
            mauBieu.OwerIds.Add(CurrentUserName);

            await BaseMongoDb.UpdateAsync(mauBieu);

            // history mẫu biểu
            await _historyMauBieu.WithFormKey(mauBieu.Id)
                  .WithCollection(_settings.MauBieuCollectionName, mauBieu.Id, "Mẫu biểu")
                  .WithTitle("Chuyển trạng thái xử lý")
                  .WithContent(model.NoiDung)
                  .WithStatus(mauBieu.LastStatus)
                  .WithAction(nameof(this.ChangeStatus))
                  .WithUserName(CurrentUser)
                  .WithOldValue(mauBieu)
                  .SaveChangeHistory();
        }

        public List<ListNamMauBieuVM> ListNamMauBieu()
        {
            var result = _context.MauBieu.Aggregate()
             .Match(k => k.DenNgay.HasValue)
             .Group(k => k.DenNgay.Value.Year,
                g => new ListNamMauBieuVM() { Nam = g.Key, Count = g.Count() }
              )
              .SortBy(d => d.Nam)
              .ToList();

            return result;
        }
    }
}