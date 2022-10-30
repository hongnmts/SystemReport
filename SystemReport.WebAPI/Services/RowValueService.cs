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
using SystemReport.WebAPI.ViewModels;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class RowValueService : BaseService, IRowValueService
    {
        private DataContext _context;
        private BaseMongoDb<RowValue, string> BaseMongoDb;
        private IMongoCollection<RowValue> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;
        private IThuocTinhService _thuocTinhService;

        public RowValueService(ILoggingService logger, IDbSettings settings, DataContext context,
            IThuocTinhService thuocTinhService,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<RowValue, string>(_context.RowValue);
            _collection = context.RowValue;
            _settings = settings;
            _thuocTinhService = thuocTinhService;
            _logger = logger.WithCollectionName(_settings.RowValueCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<List<RowValue>> CreateSub(List<RowValue> model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            // Tạo key
            var keyRow = BsonObjectId.GenerateNewId().ToString();
            var firstModel = model.FirstOrDefault() ?? new RowValue();
            foreach (var item in model)
            {
                var code = CommonExtensions.GenerateNewRandomDigit();

                var entity = new RowValue
                {
                    KeyRow = keyRow,
                    Code = code,
                    ThuTu = item.ThuTu,
                    Level = item.Level,
                    ParentId = item.ParentId,
                    ThuocTinhId = item.ThuocTinhId,
                    BangBieuId = item.BangBieuId,
                    StyleInput = item.StyleInput,
                    TinhTongChiTieuCon = item.TinhTongChiTieuCon,
                    Value = item.Value,
                    StyleValue = item.StyleValue,
                    GhiChu = item.GhiChu,
                    FontStyle = item.FontStyle,
                    FontWeight = item.FontWeight,
                    FontHorizontalAlign = item.FontHorizontalAlign,
                    CreatedBy = CurrentUserName,
                    ModifiedBy = CurrentUserName,
                    Order = item.Order
                };

                var result = await BaseMongoDb.CreateAsync(entity);
            }
            await StringCongThuc(keyRow);
            // Tính tổng chi tiêu con
            var dsThuocTinh = _context.ThuocTinh
                .Find(x => x.BangBieuId == firstModel.BangBieuId && x.TinhChiTieuCon == true).ToList();
            var chiTieus = _context.RowValue.Find(x => x.KeyRow == keyRow).ToList();
            var rowValues = _context.RowValue.Find(x => x.BangBieuId == firstModel.BangBieuId && x.IsDeleted != true)
                .ToList();
            foreach (var ct in chiTieus)
            {
                var tt = dsThuocTinh.Where(x => x.Id == ct.ThuocTinhId).FirstOrDefault();
                if (tt != default)
                {
                    ct.TinhTongChiTieuCon = false;
                    ct.StyleInput = new StyleInput()
                    {
                        Id = "int",
                        Name = "Kiểu số nguyên"
                    };
                    var result = await BaseMongoDb.UpdateAsync(ct);
                    await LoopTinhToanChiTieu(ct, rowValues);
                }
            }
            return model;
        }

        public async Task<List<RowValue>> Create(List<RowValue> model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var firstModel = model.FirstOrDefault() ?? new RowValue();
            var keyRow = BsonObjectId.GenerateNewId().ToString();
            int Level = LevelThuocTinh(model.FirstOrDefault()?.ParentId);
            var ids = model.Select(x => x.ThuocTinhId).ToList();
            var thuocTinhs = _thuocTinhService.GetNodeLeftChiTieu(model.FirstOrDefault()?.BangBieuId);
            if (thuocTinhs != default)
            {
                thuocTinhs = thuocTinhs.Where(x => !ids.Contains(x.Id)).ToList();
            }
            bool isCreate = false;
            foreach (var item in model)
            {
                if (item.Id != default)
                {
                    var entity = _context.RowValue.Find(x => x.Id == item.Id).FirstOrDefault();
                    if (entity == default)
                    {
                        throw new ResponseMessageException()
                            .WithCode(EResultResponse.FAIL.ToString())
                            .WithMessage(DefaultMessage.DATA_NOT_FOUND);
                    }

                    entity.ThuTu = item.ThuTu;
                    entity.KeyRow = item.KeyRow;
                    entity.Level = Level;
                    entity.ParentId = item.ParentId;
                    entity.BangBieuId = item.BangBieuId;
                    entity.StyleInput = item.StyleInput;
                    entity.TinhTongChiTieuCon = item.TinhTongChiTieuCon;
                    entity.Value = item.Value;
                    entity.StyleValue = item.StyleValue;
                    entity.GhiChu = item.GhiChu;
                    entity.Order = item.Order;
                    entity.FontStyle = item.FontStyle;
                    entity.FontWeight = item.FontWeight;
                    entity.FontHorizontalAlign = item.FontHorizontalAlign;

                    var result = await BaseMongoDb.UpdateAsync(entity);

                    keyRow = item.KeyRow;
                }
                else
                {
                    isCreate = true;

                    var code = CommonExtensions.GenerateNewRandomDigit();

                    var entity = new RowValue
                    {
                        KeyRow = keyRow,
                        Code = code,
                        ThuTu = item.ThuTu,
                        Level = Level,
                        ParentId = item.ParentId,
                        ThuocTinhId = item.ThuocTinhId,
                        BangBieuId = item.BangBieuId,
                        StyleInput = item.StyleInput,
                        Value = item.Value,
                        TinhTongChiTieuCon = item.TinhTongChiTieuCon,
                        StyleValue = item.StyleValue,
                        GhiChu = item.GhiChu,
                        FontStyle = item.FontStyle,
                        FontWeight = item.FontWeight,
                        Order = item.Order,
                        FontHorizontalAlign = item.FontHorizontalAlign,
                        CreatedBy = CurrentUserName,
                        ModifiedBy = CurrentUserName,
                    };

                    var result = await BaseMongoDb.CreateAsync(entity);
                }


            }

            if (isCreate)
            {
                foreach (var item in thuocTinhs)
                {
                    var code = CommonExtensions.GenerateNewRandomDigit();
                    var entity = new RowValue
                    {
                        KeyRow = keyRow,
                        Code = code,
                        ThuTu = firstModel.ThuTu,
                        Level = Level,
                        ParentId = firstModel.ParentId,
                        ThuocTinhId = item.Id,
                        BangBieuId = firstModel.BangBieuId,
                        TinhTongChiTieuCon = item.TinhChiTieuCon,
                        StyleInput = item.StyleInput,
                        Value = null,
                        StyleValue = null,
                        GhiChu = null,
                        Order = item.Order,
                        CreatedBy = CurrentUserName,
                        ModifiedBy = CurrentUserName,
                    };

                    var result = await BaseMongoDb.CreateAsync(entity);
                }

                await StringCongThuc(keyRow);
            }
            // Tính tổng chi tiêu con
            var dsThuocTinh = _context.ThuocTinh
                .Find(x => x.BangBieuId == firstModel.BangBieuId && x.TinhChiTieuCon == true).ToList();
            var chiTieus = _context.RowValue.Find(x => x.KeyRow == keyRow).ToList();
            var rowValues = _context.RowValue.Find(x => x.BangBieuId == firstModel.BangBieuId && x.IsDeleted != true)
                .ToList();
            foreach (var ct in chiTieus)
            {
                var tt = dsThuocTinh.Where(x => x.Id == ct.ThuocTinhId).FirstOrDefault();
                if (tt != default)
                {
                    ct.TinhTongChiTieuCon = true;
                    await LoopTinhToanChiTieu(ct, rowValues);
                }
            }
            return model;
        }

        public async Task<RowValue> Update(RowValue model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.RowValue.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.ThuTu = model.ThuTu;
            entity.Level = model.Level;
            entity.ParentId = model.ParentId;
            entity.BangBieuId = model.BangBieuId;
            entity.StyleInput = model.StyleInput;
            entity.Value = model.Value;
            entity.StyleValue = model.StyleValue;
            entity.GhiChu = model.GhiChu;
            entity.FontStyle = model.FontStyle;
            entity.FontWeight = model.FontWeight;
            entity.FontHorizontalAlign = model.FontHorizontalAlign;
            entity.TinhTongChiTieuCon = model.TinhTongChiTieuCon;

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

        public async Task<List<RowValue>> DeleteRowValue(List<RowValue> model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            List<string> parents = new List<string>();
            var keyRow = model.FirstOrDefault()?.KeyRow;
            var bangBieuId = model.FirstOrDefault()?.BangBieuId;
            if (keyRow == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy dòng");
            }

            /// Tìm và xóa những chỉ tiêu tính công thức ở cha
            await RemoveChiTieuCodeInFormula(bangBieuId, keyRow);

            var rowValues = _context.RowValue.Find(x => x.KeyRow == keyRow && x.IsDeleted != true).ToList();
            var children = GetNodeLeftRowValue(bangBieuId, keyRow);
            foreach (var item in rowValues)
            {
                var result = await BaseMongoDb.DeletedAsync(item);
            }

            var selectParents = children.Select(x => x.KeyRow).ToList();
            var listValueFormKyRow = _context.RowValue.Find(x => selectParents.Contains(x.KeyRow)).ToList();
            if (listValueFormKyRow.Count > 0)
            {
                foreach (var cd in listValueFormKyRow)
                {
                    await BaseMongoDb.DeletedAsync(cd);
                }
            }



            return model;
        }

        public async Task RemoveChiTieuCodeInFormula(string bangBieuId, string keyRow)
        {
            var dsThuocTinh = _context.ThuocTinh
                .Find(x => x.BangBieuId == bangBieuId && x.TinhChiTieuCon == true).ToList();

            var rowValues = _context.RowValue.Find(x => x.BangBieuId == bangBieuId && x.IsDeleted != true)
                .ToList();

            var chiTieus = rowValues.Where(x => x.KeyRow == keyRow).ToList();
            foreach (var ct in chiTieus)
            {
                var tt = dsThuocTinh.Where(x => x.Id == ct.ThuocTinhId).FirstOrDefault();
                if (tt != default)
                {
                    if (!string.IsNullOrEmpty(ct.ParentId))
                    {
                        var parent = rowValues.FirstOrDefault(x => x.KeyRow == ct.ParentId && x.ThuocTinhId == tt.Id);

                        if (parent != default)
                        {
                            if (parent.StyleInput != default && parent.StyleInput.Id == "formula")
                            {
                                var findFormulaCode = parent.StringCongThuc.Contains($"<{ct.Code}>");
                                if (findFormulaCode)
                                {
                                    parent.StringCongThuc = parent.StringCongThuc.Replace($"<{ct.Code}>", "0");

                                    var checkChildren = rowValues.Where(x => x.ParentId == parent.KeyRow && x.KeyRow != keyRow).ToList();
                                    if (checkChildren.Count <= 0)
                                    {
                                        parent.StyleInput = new StyleInput()
                                        {
                                            Id = "int",
                                            Name = "Kiểu số nguyên"
                                        };
                                    }

                                    await BaseMongoDb.UpdateAsync(parent);
                                }
                            }
                        }
                    }
                }
            }
        }
        public async Task Delete(string id)
        {
            if (id == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }


            var entity = _context.RowValue.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public async Task<RowValue> GetById(string id)
        {
            return await _context.RowValue.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<List<RowValueTreeVM>> GetTreeByBangBieuId(string bangBieuId)
        {
            // var listChiTiec = _context.RowValue.AsQueryable().Where(x  => x.BangBieuId == bangBieuId && x.IsDeleted ==false)
            var listDonVi = await _context.RowValue.Find(x => x.BangBieuId == bangBieuId && x.IsDeleted == false).SortBy(donVi => donVi.Level).ToListAsync();
            var parents = listDonVi.Where(x => x.ParentId == null).ToList();
            List<RowValueTreeVM> list = new List<RowValueTreeVM>();
            foreach (var item in parents)
            {
                RowValueTreeVM donVi = new RowValueTreeVM(item);
                list.Add(donVi);
                GetLoopItem(ref list, listDonVi, donVi);
            }
            return list;
        }

        private List<DonViTreeVM> GetLoopItem(ref List<RowValueTreeVM> list, List<RowValue> items, RowValueTreeVM target)
        {
            try
            {
                var coquan = items.FindAll((item) => item.ParentId == target.Id).ToList();
                if (coquan.Count > 0)
                {
                    target.Children = new List<RowValueTreeVM>();
                    foreach (var item in coquan)
                    {
                        RowValueTreeVM itemDV = new RowValueTreeVM(item);
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
        private int LevelThuocTinh(string parentId)
        {
            if (parentId == default)
                return 1;
            var thuocTinh = _context.RowValue.Find(x => x.Id == parentId && x.IsDeleted != true).FirstOrDefault();

            if (thuocTinh == default)
                return 1;
            return thuocTinh.Level + 1;
        }
        public async Task<List<BodyTableVM>> RenderBodyMainByBangBieuId(string bangBieuId)
        {
            var rowValues = await _context.RowValue.Find(x => x.BangBieuId == bangBieuId && x.IsDeleted == false).SortBy(donVi => donVi.Level).ToListAsync();
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
        public async Task<List<BodyTableVM>> RenderBodyByBangBieuId(string bangBieuId)
        {
            // Chỉ lấy những cái là is chi tiêu
            var thuocTinhs = _context.ThuocTinh.AsQueryable()
                .Where(x => x.BangBieuId == bangBieuId && x.IsChiTieu == true && x.IsDeleted != true).Select(x => x.Id).ToList();
            var rowValues = await _context.RowValue.Find(x => x.BangBieuId == bangBieuId && thuocTinhs.Contains(x.ThuocTinhId) && x.IsDeleted == false).SortBy(donVi => donVi.Level).ToListAsync();
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
                RowValues = x.ToList()
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

        public async Task<List<RowValueTreeVM>> GetTreeParentRowValue(string bangBieuId)
        {
            // var listChiTiec = _context.RowValue.AsQueryable().Where(x  => x.BangBieuId == bangBieuId && x.IsDeleted ==false)
            var listDonVi = await _context.RowValue.Find(x => x.BangBieuId == bangBieuId && x.IsDeleted == false).SortBy(donVi => donVi.Level).ToListAsync();
            var groupBy = listDonVi.GroupBy(x => x.KeyRow).Select(x => new RowValueTemp() { KeyRow = x.Key, RowValues = x.ToList() }).ToList();

            var tempRowValue = new List<RowValue>();
            foreach (var item in groupBy)
            {
                var itemRowValue = new RowValue();
                itemRowValue.Id = item.KeyRow;
                itemRowValue.ParentId = item.RowValues.Select(x => x.ParentId).FirstOrDefault();
                var temp = item.RowValues.OrderBy(x => x.ThuTu).Select(x => x.Value).ToList();
                string combinedString = string.Join("-", temp.ToArray());
                itemRowValue.Value = combinedString;

                tempRowValue.Add(itemRowValue);
            }
            var parents = tempRowValue.Where(x => x.ParentId == null).ToList();
            List<RowValueTreeVM> list = new List<RowValueTreeVM>();
            foreach (var item in parents)
            {
                RowValueTreeVM donVi = new RowValueTreeVM(item);
                list.Add(donVi);
                GetLoopItemTreeParent(ref list, tempRowValue, donVi);
            }
            return list;
        }

        private List<DonViTreeVM> GetLoopItemTreeParent(ref List<RowValueTreeVM> list, List<RowValue> items, RowValueTreeVM target)
        {
            try
            {
                var coquan = items.FindAll((item) => item.ParentId == target.Id).ToList();
                if (coquan.Count > 0)
                {
                    target.Children = new List<RowValueTreeVM>();
                    foreach (var item in coquan)
                    {
                        RowValueTreeVM itemDV = new RowValueTreeVM(item);
                        target.Children.Add(itemDV);
                        GetLoopItemTreeParent(ref list, items, itemDV);
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

        public List<RowValue> GetRowValueByKeyRow(string keyRow)
        {
            return _context.RowValue.Find(x => x.KeyRow == keyRow).ToList();
        }

        public List<RowValue> GetNodeLeftRowValue(string bangBieuId, string parentId)
        {
            var thuocTinhs = _context.RowValue.Find(x => x.BangBieuId == bangBieuId && x.IsDeleted != true).ToList();
            var parents = thuocTinhs.Where(x => x.ParentId == parentId).ToList();
            parents = parents.DistinctBy(x => x.KeyRow).ToList();
            List<RowValue> list = new List<RowValue>();
            foreach (var item in parents)
            {
                list.Add(item);
                LoopGetRowValueConsLeft(ref list, thuocTinhs, item);
            }

            return list;
        }

        private List<ThuocTinhListTreeVM> LoopGetRowValueConsLeft(ref List<RowValue> list, List<RowValue> items, RowValue target)
        {
            var coquan = items.Where((item) => item.ParentId == target.KeyRow).OrderByDescending(x => x.Level).ToList();
            coquan = coquan.DistinctBy(x => x.KeyRow).ToList();
            if (coquan.Count > 0)
            {
                foreach (var item in coquan)
                {
                    list.Add(item);
                    LoopGetRowValueConsLeft(ref list, items, item);
                }
            }
            return null;
        }

        public async Task<List<RowValue>> StringCongThuc(string keyRow)
        {
            var values = _context.RowValue.Find(x => x.KeyRow == keyRow).ToList();
            var thuocTinhs = values.Select(x => x.ThuocTinhId).ToList();
            var rawThuocTinh = _context.ThuocTinh.Find(x => thuocTinhs.Contains(x.Id) && x.IsDeleted != true).ToList();

            // lấy thuộc tính có công thức
            var thuocTinhCoCongThuc =
                rawThuocTinh.Where(x => x.StyleInput != default && x.StyleInput.Id == "formula").ToList();

            foreach (var rv in values)
            {
                var findTTCC = thuocTinhCoCongThuc.FirstOrDefault(x => x.Id == rv.ThuocTinhId);

                if (findTTCC != default)
                {
                    string strCongThuc = findTTCC.StringCongThuc;

                    rv.StyleInput = findTTCC.StyleInput;

                    List<string> arrays = new List<string>();
                    var getStringFormBetween = new GetStringFormBetween();
                    arrays = getStringFormBetween.Get(strCongThuc, "<", ">");

                    var thuocThinhArray = rawThuocTinh.Where(x => arrays.Contains(x.Code)).ToList();
                    foreach (var ar in thuocThinhArray)
                    {
                        var findRowValueThuocTInh = values.FirstOrDefault(x => x.ThuocTinhId == ar.Id);
                        strCongThuc = strCongThuc.Replace($"{ar.Code}", findRowValueThuocTInh.Code);
                    }

                    rv.StringCongThuc = strCongThuc;
                    await BaseMongoDb.UpdateAsync(rv);
                }
            }

            return values;
        }

        private void UpdateTinhTongChiTieu(RowValue rowValue)
        {
            var rowValues = _context.RowValue.Find(x => x.BangBieuId == rowValue.BangBieuId && x.IsDeleted != true)
                .ToList();

        }

        public async Task LoopTinhToanChiTieu(RowValue rowValue, List<RowValue> rowValues)
        {
            var value = rowValues.Where(x => x.ThuocTinhId == rowValue.ThuocTinhId && x.KeyRow == rowValue.KeyRow)
                .FirstOrDefault();
            if (value != default)
            {
                var checkParentId = string.IsNullOrEmpty(value.ParentId);
                if (!checkParentId)
                {
                    var parent = rowValues
                        .FirstOrDefault(x => x.ThuocTinhId == rowValue.ThuocTinhId && x.KeyRow == value.ParentId);

                    if (parent != default)
                    {
                        // if (parent.TinhTongChiTieuCon)
                        // {
                        //     
                        // }
                        parent.TinhTongChiTieuCon = true;
                        if (parent.StyleInput != default && parent.StyleInput.Id != "formula")
                        {
                            parent.StyleInput = new StyleInput() { Id = "formula", Name = "Công thức" };
                        }

                        if (string.IsNullOrEmpty(parent.StringCongThuc))
                        {
                            parent.StringCongThuc += $"<{rowValue.Code}>";
                        }
                        else
                        {
                            var check = parent.StringCongThuc.Contains(rowValue.Code);
                            if (!check)
                            {
                                parent.StringCongThuc += $"+ <{rowValue.Code}>";
                            }
                        }

                        await BaseMongoDb.UpdateAsync(parent);

                        await LoopTinhToanChiTieu(parent, rowValues);
                    }
                }
            }
        }

        // public async Task LoopRemoveTinhToanChiTieu(RowValue rowValue, List<RowValue> rowValues)
        // {
        //     var value = rowValues.Where(x => x.ThuocTinhId == rowValue.ThuocTinhId && x.KeyRow == rowValue.KeyRow)
        //         .FirstOrDefault();
        //     if (value != default)
        //     {
        //         var checkParentId = string.IsNullOrEmpty(value.ParentId);
        //         if (!checkParentId)
        //         {
        //             var parent = rowValues
        //                 .FirstOrDefault(x => x.ThuocTinhId == rowValue.ThuocTinhId && x.KeyRow == value.ParentId);
        //
        //             if (parent != default)
        //             {
        //                 // if (parent.TinhTongChiTieuCon)
        //                 // {
        //                 //     
        //                 // }
        //                 parent.TinhTongChiTieuCon = true;
        //                 if (parent.StyleInput != default && parent.StyleInput.Id != "formula")
        //                 {
        //                     parent.StyleInput = new StyleInput() {Id = "formula", Name = "Công thức"};
        //                 }
        //
        //                 if (string.IsNullOrEmpty(parent.StringCongThuc))
        //                 {
        //                     parent.StringCongThuc += $"<{rowValue.Code}>";
        //                 }
        //                 else
        //                 {
        //                     var check = parent.StringCongThuc.Contains(rowValue.Code);
        //                     if (!check)
        //                     {
        //                         parent.StringCongThuc += $"+ <{rowValue.Code}>"; 
        //                     }
        //                 }
        //              
        //                 await BaseMongoDb.UpdateAsync(parent);
        //
        //                 await LoopRemoveTinhToanChiTieu(parent, rowValues);
        //             }
        //         }
        //     }
        // }
        public async Task AddRowTong(string bangBieuId)
        {
            var thuocTinhs = _thuocTinhService.GetNodeLeftChiTieu(bangBieuId);

            if (thuocTinhs.Count > 0)
            {
                var keyRow = BsonObjectId.GenerateNewId().ToString();

                bool checkFirstTime = true;
                foreach (var item in thuocTinhs)
                {

                    var code = CommonExtensions.GenerateNewRandomDigit();
                    var entity = new RowValue
                    {
                        KeyRow = keyRow,
                        Code = code,
                        ThuTu = 1,
                        Level = 1,
                        ParentId = null,
                        ThuocTinhId = item.Id,
                        BangBieuId = item.BangBieuId,
                        StyleInput = new StyleInput() { Id = "formula", Name = "Công thức" },
                        Value = 0,
                        GhiChu = item.GhiChu,
                        FontStyle = item.FontStyle,
                        FontWeight = item.FontWeight,
                        FontHorizontalAlign = item.FontHorizontalAlign,
                        CreatedBy = CurrentUserName,
                        ModifiedBy = CurrentUserName,
                        IsTong = true,
                        RowParent = null,
                        StringCongThuc = FormulaWithRootRow(item.Id)
                    };
                    if (checkFirstTime)
                    {
                        entity.Value = "Tổng số";
                        entity.StyleInput = new StyleInput() { Id = "text", Name = "Kiểu chuỗi" };
                    }
                    var result = await BaseMongoDb.CreateAsync(entity);
                    checkFirstTime = false;
                }
            }
        }

        public string FormulaWithRootRow(string thuocTinh)
        {
            var values = _context.RowValue.Find(x => x.ThuocTinhId == thuocTinh && x.ParentId == null).ToList();

            var result = "";
            foreach (var item in values)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += $"+ <{item.Code}>";
                }
                else
                {
                    result += $"<{item.Code}>";
                }
            }

            return result;
        }
    }
}