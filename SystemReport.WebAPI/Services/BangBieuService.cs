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
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{

    public class BangBieuService : BaseService, IBangBieuService
    {
        private DataContext _context;
        private BaseMongoDb<BangBieu, string> BaseMongoDb;
        private BaseMongoDb<RowValue, string> RowValueDb;
        private IMongoCollection<BangBieu> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;
        private IRowValueService _rowValueService;
        private IHistoryMauBieuService _historyMauBieuService;

        public BangBieuService(ILoggingService logger, IDbSettings settings, DataContext context,
            IRowValueService rowValueService,
            IHistoryMauBieuService historyMauBieuService,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<BangBieu, string>(_context.BangBieu);
            RowValueDb = new BaseMongoDb<RowValue, string>(_context.RowValue);
            _collection = context.BangBieu;
            _rowValueService = rowValueService;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.BangBieuCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
            _historyMauBieuService = historyMauBieuService;
        }

        public async Task<BangBieu> Create(BangBieu model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            if (model.MauBieuId == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy mẫu biểu!");
            }

            var code = CommonExtensions.GenerateNewRandomDigit();
            var entity = new BangBieu
            {
                Ten = model.Ten,
                ThuTu = model.ThuTu,
                Code = code,
                HienThiTen = model.HienThiTen,
                DonViTinh = model.DonViTinh,
                MauBieuId = model.MauBieuId,
                CreatedBy = CurrentUserName,
                ModifiedBy = CurrentUserName,
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

        public async Task<BangBieu> Update(BangBieu model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.BangBieu.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.Ten = model.Ten;
            entity.ThuTu = model.ThuTu;
            entity.HienThiTen = model.HienThiTen;
            entity.DonViTinh = model.DonViTinh;
            entity.MauBieuId = model.MauBieuId;
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


            var entity = _context.BangBieu.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public async Task<List<BangBieu>> Get()
        {
            return await _context.BangBieu.Find(x => x.IsDeleted != true).SortByDescending(x => x.ThuTu)
                .ToListAsync();
        }

        public async Task<BangBieu> GetById(string id)
        {
            return await _context.BangBieu.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<BangBieu>> GetPaging(BangBieuParam param)
        {
            PagingModel<BangBieu> result = new PagingModel<BangBieu>();
            var builder = Builders<BangBieu>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.MauBieuId == param.MauBieuId && x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            string sortBy = nameof(MauBieu.ThuTu);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<BangBieu>
                        .Sort.Descending(sortBy)
                    : Builders<BangBieu>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<List<BangBieu>> GetBangBieuByMauBieuId(string mauBieuId)
        {
            return await _context.BangBieu.Find(x => x.MauBieuId == mauBieuId && x.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<List<HeaderTableVM>> RenderHeader(string bangBieuId)
        {
            var thuocTinhs = _context.ThuocTinh.AsQueryable()
                .Where(x => x.BangBieuId == bangBieuId && x.IsDeleted != true).OrderBy(x => x.Order).ToList();
            List<THeaderVM> theader = new List<THeaderVM>();

            var MaxRow = thuocTinhs.Max(x => x.Level);

            foreach (var thuocTinh in thuocTinhs)
            {
                var th = new THeaderVM();
                th.TenThuocTinh = thuocTinh.Ten;
                th.Level = thuocTinh.Level;
                th.RowSpan = FindRow(thuocTinh.Level, MaxRow, thuocTinh.Id, thuocTinhs);
                var findCol = FindDeepThuocTinh(thuocTinh.Id, thuocTinhs);
                th.ColSpan = findCol == 0 ? 1 : findCol;

                th.ThuTu = thuocTinh.ThuTu;
                // Config
                th.Width = thuocTinh.Width;
                th.FontStyle = thuocTinh.FontStyle;
                th.FontWeight = thuocTinh.FontWeight;
                th.FontHorizontalAlign = thuocTinh.FontHorizontalAlign;
                th.FontVerticalAlign = thuocTinh.FontVerticalAlign;

                th.DonViTinh = thuocTinh.DonViTinh;
                th.GhiChu = thuocTinh.GhiChu;
                th.Formula = thuocTinh.Formula;
                th.StyleInput = thuocTinh.StyleInput;
                th.StringCongThuc = thuocTinh.StringCongThuc;

                th.IsChiTieu = thuocTinh.IsChiTieu;
                th.HasChildren = HasChildren(thuocTinhs, thuocTinh.Id);
                th.Order = thuocTinh.Order;
                theader.Add(th);
            }

            List<HeaderTableVM> data = theader.GroupBy(x => x.Level)
                .Select(x => new HeaderTableVM() { Level = x.Key, THeaderVms = x.ToList() }).ToList();
            return data;
        }

        public async Task<List<HeaderTableVM>> RenderHeader(List<ThuocTinh> thuocTinhs)
        {
            List<THeaderVM> theader = new List<THeaderVM>();

            var MaxRow = thuocTinhs.Max(x => x.Level);

            foreach (var thuocTinh in thuocTinhs)
            {
                var th = new THeaderVM();
                th.TenThuocTinh = thuocTinh.Ten;
                th.Level = thuocTinh.Level;
                th.RowSpan = FindRow(thuocTinh.Level, MaxRow, thuocTinh.Id, thuocTinhs);
                var findCol = FindDeepThuocTinh(thuocTinh.Id, thuocTinhs);
                th.ColSpan = findCol == 0 ? 1 : findCol;

                th.ThuTu = thuocTinh.ThuTu;
                // Config
                th.Width = thuocTinh.Width;
                th.FontStyle = thuocTinh.FontStyle;
                th.FontWeight = thuocTinh.FontWeight;
                th.FontHorizontalAlign = thuocTinh.FontHorizontalAlign;
                th.FontVerticalAlign = thuocTinh.FontVerticalAlign;

                th.DonViTinh = thuocTinh.DonViTinh;
                th.GhiChu = thuocTinh.GhiChu;
                th.Formula = thuocTinh.Formula;
                th.StyleInput = thuocTinh.StyleInput;
                th.StringCongThuc = thuocTinh.StringCongThuc;

                th.IsChiTieu = thuocTinh.IsChiTieu;
                th.HasChildren = HasChildren(thuocTinhs, thuocTinh.Id);
                th.Order = thuocTinh.Order;
                theader.Add(th);
            }

            List<HeaderTableVM> data = theader.GroupBy(x => x.Level)
                .Select(x => new HeaderTableVM() { Level = x.Key, THeaderVms = x.ToList() }).ToList();
            return data;
        }

        private int FindRow(int level, int maxRow, string id, List<ThuocTinh> thuocTinhs)
        {
            var hasChildren = HasChildren(thuocTinhs, id);

            if (level == 1 && !hasChildren)
                return maxRow;
            if (level < maxRow && !hasChildren)
                return maxRow - (maxRow - level);
            return 1;
        }

        private bool HasChildren(List<ThuocTinh> thuocTinhs, string id)
        {
            var check = thuocTinhs.Any(x => x.Id != id && x.ParentId == id);
            return check;
        }

        public int FindDeepThuocTinh(string thuocTinhId, List<ThuocTinh> thuocTinhs)
        {
            var listDonVi = thuocTinhs;
            var parents = listDonVi.Where(x => x.ParentId == thuocTinhId).ToList();
            List<ThuocTinhListTreeVM> list = new List<ThuocTinhListTreeVM>();
            int Deep = 0;
            foreach (var item in parents)
            {
                GetLoopItemTree(ref Deep, listDonVi, item);
            }
            return Deep;
        }
        private List<ThuocTinhListTreeVM> GetLoopItemTree(ref int deep, List<ThuocTinh> items, ThuocTinh target)
        {

            var coquan = items.Where((item) => item.ParentId == target.Id).OrderByDescending(x => x.Level).ToList();
            if (coquan.Count > 0)
            {

                foreach (var item in coquan)
                {
                    GetLoopItemTree(ref deep, items, item);
                }
            }
            else
            {
                deep++;
            }

            return null;
        }

        public async Task<RenderTableNhapLieu> RenderNhapLieuBangBieu(string bangBieuId)
        {
            var bangBieu = _context.BangBieu.Find(x => x.Id == bangBieuId && x.IsDeleted != true).FirstOrDefault();
            if (bangBieu == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var headers = await RenderHeader(bangBieu.Id);
            var body = await _rowValueService.RenderBodyMainByBangBieuId(bangBieu.Id);
            var values = await _context.RowValue.Find(x => x.BangBieuId == bangBieu.Id).ToListAsync();

            var data = new RenderTableNhapLieu();

            data.BangBieu = bangBieu;
            data.Headers = headers;
            data.Body = body;
            data.Values = values;


            return data;
        }

        public async Task SaveDataBangBieu(List<BodyTableVM> data)
        {
            if (data == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var bangBieuId = "";
            if (data.Count > 0)
            {
                foreach (BodyTableVM item in data)
                {
                    var rowValues = _context.RowValue.Find(x => x.KeyRow == item.KeyRow && x.IsDeleted != true).ToList();

                    if (string.IsNullOrEmpty(bangBieuId))
                    {
                        bangBieuId = rowValues.FirstOrDefault()?.BangBieuId;
                    }
                    if (item.IsDeleted)
                    {
                        foreach (var rv in rowValues)
                        {
                            await RowValueDb.DeletedAsync(rv);
                        }
                        continue;
                    }
                    if (item.RowValues != default && item.RowValues.Count > 0)
                    {
                        var tempRowValues = item.RowValues;
                        foreach (var rv in tempRowValues)
                        {
                            var findRV = rowValues.Where(x => x.Id == rv.Id && x.IsDeleted != true).FirstOrDefault();
                            if (findRV != default)
                            {
                                rv.ModifiedAt = DateTime.Now;
                                rv.ModifiedBy = CurrentUserName;
                                await RowValueDb.UpdateAsync(rv);
                            }
                            else
                            {
                                await RowValueDb.CreateAsync(rv);
                            }
                        }

                    }

                }


                // history mẫu biểu
                var bangBieu = _context.BangBieu.Find(x => x.Id == bangBieuId).FirstOrDefault();
                var mauBieu = _context.MauBieu.Find(x => x.Id == bangBieu.MauBieuId).FirstOrDefault();

                await _historyMauBieuService.WithFormKey(mauBieu.Id)
                      .WithCollection(_settings.BangBieuCollectionName, bangBieu.Id, "Bảng biểu")
                      .WithTitle("Cập nhật dữ liệu bảng biểu")
                      .WithStatus(mauBieu.LastStatus)
                      .WithAction(nameof(this.SaveDataBangBieu))
                      .WithUserName(CurrentUser)
                      .WithBangBieu(bangBieu)
                      .SaveChangeHistory();
            }



        }


    }
}