using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Extensions;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Services
{
    public class HistoryMauBieuService : BaseService, IHistoryMauBieuService
    {
        private DataContext _context;
        private BaseMongoDb<HistoryMauBieu, string> BaseMongoDb;
        private IMongoCollection<HistoryMauBieu> _collection;
        #region Properties

        private HistoryMauBieu history;

        #endregion
        public HistoryMauBieuService(IDbSettings settings, DataContext context,
        IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<HistoryMauBieu, string>(_context.HistoryMauBieu);
            _collection = context.HistoryMauBieu;
            history = new HistoryMauBieu();
            history.CreatedBy = CurrentUserName;
        }

        public async Task<PagingModel<HistoryMauBieu>> GetHistoryByMauBieuId(HistoryParam param)
        {
            PagingModel<HistoryMauBieu> result = new PagingModel<HistoryMauBieu>();
            var builder = Builders<HistoryMauBieu>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter,
                               builder.Where(x => x.FormKey == param.FormKey));
            string sortBy = nameof(MauBieu.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            var data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();

            result.Data = data;
            return result;
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
        public async Task<List<BodyTableVM>> RenderBodyMainByBangBieuId(List<RowValue> rowValues)
        {
            var parents = rowValues.Where(x => x.ParentId == null).ToList();
            List<RowValue> list = new List<RowValue>();
            foreach (var item in parents)
            {
                var check = list.Any(x => x.Id == item.Id);
                if (check)
                    continue;
                list.Add(item);
                RenderGetLoopItem(ref list, rowValues, item);
            }

            var data = list.GroupBy(x => x.KeyRow).Select(x => new BodyTableVM()
            {
                KeyRow = x.Key,
                RowValues = x.OrderBy(x => x.Order).ToList()
            }).ToList();
            return data;
        }
        private List<dynamic> RenderGetLoopItem(ref List<RowValue> list, List<RowValue> items, RowValue target)
        {
            try
            {
                var coquan = items.FindAll((item) => item.ParentId == target.KeyRow).ToList();
                if (coquan.Count > 0)
                {
                    foreach (var item in coquan)
                    {
                        var check = list.Any(x => x.Id == item.Id);
                        if (check)
                            continue;
                        list.Add(item);
                        RenderGetLoopItem(ref list, items, item);
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

        public async Task<DemoMauBieuVM> RenderTableBangBieu(string historyId)
        {
            var historyValue = await _context.HistoryMauBieu.Find(x => x.Id == historyId).FirstOrDefaultAsync();
            var result = new DemoMauBieuVM();
            if (historyValue != default)
            {
                if (historyValue.JsonData != default)
                {
                    var jsonData = JsonConvert.DeserializeObject<HistoryMauBieuVM>(historyValue.JsonData);

                    if (jsonData != default)
                    {
                        var headers = await RenderHeader(jsonData.ThuocTinhs);
                        var body = await RenderBodyMainByBangBieuId(jsonData.RowValues);

                        var table = new DemoMauBieuVM();
                        table.TenBangBieu = jsonData.BangBieu.Ten;
                        table.IsHienThiTen = jsonData.BangBieu.HienThiTen;
                        table.ThuTu = jsonData.BangBieu.ThuTu;
                        table.Headers = headers;
                        table.Body = body;

                        result = table;
                    }
                }
            }
            return result;
        }
        public async Task<bool> SaveChangeHistory()
        {
            if (history == default)
                return false;
            Hash();
            history.Id = BsonObjectId.GenerateNewId().ToString();
            var result = await BaseMongoDb.CreateAsync(history);
            if (result.Entity.Id == default || !result.Success)
                return false;
            return true;
        }
        public HistoryMauBieuService WithBangBieu(BangBieu bangBieu)
        {
            if (bangBieu != default)
            {
                var newHistoryMauBieuVM = new HistoryMauBieuVM();

                if (bangBieu != default)
                {
                    var thuocTinhs = _context.ThuocTinh.Find(x => x.BangBieuId == bangBieu.Id).ToList();
                    var rowValues = _context.RowValue.Find(x => x.BangBieuId == bangBieu.Id).ToList();

                    newHistoryMauBieuVM.BangBieu = bangBieu;
                    newHistoryMauBieuVM.ThuocTinhs = thuocTinhs;
                    newHistoryMauBieuVM.RowValues = rowValues;

                    var jsonData = JsonConvert.SerializeObject(newHistoryMauBieuVM);

                    history.JsonData = jsonData;
                }

            }

            return this;
        }
        public HistoryMauBieuService WithTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                history.Title = title;
            }

            return this;
        }
        public HistoryMauBieuService WithContent(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                history.Content = content;
            }

            return this;
        }
        public HistoryMauBieuService WithStatus(TrangThai trangThai)
        {
            if (trangThai != default)
            {

                history.Status = new TrangThaiShort()
                {
                    Id = trangThai.Id,
                    Ten = trangThai.Ten,
                    Code = trangThai.Code,
                    BgColor = trangThai.BgColor,
                    Color = trangThai.Color
                };
            }

            return this;
        }
        public HistoryMauBieuService WithUserName(User currentUser)
        {
            if (currentUser != default)
            {
                history.UserName = currentUser.UserName;
                history.FullName = currentUser.FullName;
            }

            return this;
        }


        public HistoryMauBieuService WithFormKey(string formKey)
        {
            if (!string.IsNullOrEmpty(formKey))
            {
                history.FormKey = formKey;
            }

            return this;
        }

        public HistoryMauBieuService WithCollection(string collection, string collectionId, string collectionName)
        {
            if (!string.IsNullOrEmpty(collection)
                && !string.IsNullOrEmpty(collectionId)
                && !string.IsNullOrEmpty(collectionName))
            {
                history.Collection = collection;
                history.CollectionId = collectionId;
                history.CollectionName = collectionName;
            }

            return this;
        }

        public HistoryMauBieuService WithAction(string action)
        {
            if (!string.IsNullOrEmpty(action))
            {
                history.Action = action;
            }

            return this;
        }

        private void Hash()
        {
            var hash = StringExtensions.SHA256(history.CreatedAt.ToString() + history.CreatedBy + history.Id);
            history.Hash = hash;
            var pre = _collection.Find(x => x.FormKey == history.FormKey)
                .SortByDescending(x => x.CreatedAt).FirstOrDefault();
            if (pre != default)
                history.ReferenceHash = pre.Hash;
        }

        public HistoryMauBieuService WithOldValue(dynamic oldValue)
        {
            if (oldValue != null)
            {
                var value = JsonConvert.SerializeObject(oldValue);
                history.OldValue = value;
            }
            return this;
        }
    }
}
