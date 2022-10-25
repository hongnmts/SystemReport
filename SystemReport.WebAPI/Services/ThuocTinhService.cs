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
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;

namespace SystemReport.WebAPI.Services
{
    public class ThuocTinhService : BaseService, IThuocTinhService
    {
        private DataContext _context;
        private BaseMongoDb<ThuocTinh, string> BaseMongoDb;
        private BaseMongoDb<RowValue, string> RowValueDb;
        private IMongoCollection<ThuocTinh> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public ThuocTinhService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<ThuocTinh, string>(_context.ThuocTinh);
            RowValueDb = new BaseMongoDb<RowValue, string>(_context.RowValue);
            _collection = context.ThuocTinh;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.ThuocTinhCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<ThuocTinh> Create(ThuocTinh model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }
            var code = CommonExtensions.GenerateNewRandomDigit();
            var entity = new ThuocTinh
            {
                Ten = model.Ten,
                Code = code,
                TenKhongDau = model.Ten.convertToUnSign3(),
                ThuTu = model.ThuTu,
                TinhChiTieuCon = model.TinhChiTieuCon,
                Level = LevelThuocTinh(model.ParentId),
                ParentId =  model.ParentId,
                BangBieuId = model.BangBieuId,
                StringCongThuc = model.StringCongThuc,
                FontStyle = model.FontStyle,
                FontWeight = model.FontWeight,
                FontHorizontalAlign = model.FontHorizontalAlign,
                FontVerticalAlign = model.FontVerticalAlign,
                Width = model.Width,
                
                DonViTinh = model.DonViTinh,
                GhiChu = model.GhiChu,
                Formula = model.Formula,
                StyleInput = model.StyleInput,
                
                IsChiTieu = model.IsChiTieu,
                
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

            
            // Cập nhật lại vị trí của thuộc tính
          await  UpdateOrderThuocTinh(entity.BangBieuId);
            return entity;
        }

        private int LevelThuocTinh(string parentId)
        {
            if (parentId == default)
                return 1;
            var thuocTinh = _context.ThuocTinh.Find(x => x.Id == parentId && x.IsDeleted != true).FirstOrDefault();

            if (thuocTinh == default)
                return 1;
            return thuocTinh.Level + 1;
        }
        public async Task<ThuocTinh> Update(ThuocTinh model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.ThuocTinh.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            var code = CommonExtensions.GenerateNewRandomDigit();
            if(entity.Code == default)
            {
                entity.Code = code;
            }
            entity.Ten = model.Ten;
            entity.TenKhongDau = model.Ten.convertToUnSign3();
            entity.ThuTu = model.ThuTu;
            entity.Level =  LevelThuocTinh(model.ParentId);
            entity.ParentId = model.ParentId;
            entity.BangBieuId = model.BangBieuId;
            entity.StringCongThuc = model.StringCongThuc;
            entity.TinhChiTieuCon = model.TinhChiTieuCon;
            entity.FontStyle = model.FontStyle;
            entity.FontWeight = model.FontWeight;
            entity.FontHorizontalAlign = model.FontHorizontalAlign;
            entity.FontVerticalAlign = model.FontVerticalAlign;
            entity.Width = model.Width;

            entity.GhiChu = model.GhiChu;
            entity.DonViTinh = model.DonViTinh;
            entity.Formula = model.Formula;
            entity.StyleInput = model.StyleInput;
            
            entity.IsChiTieu = model.IsChiTieu;
            
            entity.ModifiedAt = DateTime.Now;
            entity.ModifiedBy = CurrentUserName;

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.UPDATE_FAILURE);
            }

            // Update Row value when thuoctinh update
            await UpdateRowValue(entity);

            await UpdateOrderThuocTinh(entity.BangBieuId);
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


            var entity = _context.ThuocTinh.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

            // Kiểm tra có con thì deleted
            var listCons = await GetThuocTinhCon(entity.Id, entity.BangBieuId);
            var thuocTinhThuocBangBieu = _context.ThuocTinh
                .Find(x => x.BangBieuId == entity.BangBieuId && x.IsDeleted != true).ToList();
            
            foreach (var item in listCons)
            {
                var tt = thuocTinhThuocBangBieu.Where(x => x.Id == item).FirstOrDefault();
                if (tt != default)
                {
                    tt.ModifiedAt = DateTime.Now;
                    tt.ModifiedBy = CurrentUserName;
                    tt.IsDeleted = true;
                    await BaseMongoDb.DeleteAsync(tt);
                }
            }
            if (!result.Success)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DELETE_FAILURE);
            }
        }

        public async Task<List<ThuocTinh>> Get()
        {
            return await _context.ThuocTinh.Find(x => x.IsDeleted != true).SortByDescending(x => x.ThuTu)
                .ToListAsync();
        }

        public async Task<ThuocTinh> GetById(string id)
        {
            return await _context.ThuocTinh.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateRowValue(ThuocTinh thuocTinh)
        {
            var values = _context.RowValue.Find(x => x.ThuocTinhId == thuocTinh.Id && x.IsDeleted != true).ToList();
            if(values.Count > 0)
            {
                foreach (var item in values)
                {
                 //   item.StyleInput = thuocTinh.StyleInput;
                 //   item.FontHorizontalAlign = thuocTinh.FontHorizontalAlign;
                 //   item.FontVerticalAlign = thuocTinh.FontVerticalAlign;
                    item.Width = thuocTinh.Width;
                    item.StringCongThuc = thuocTinh.StringCongThuc;
                    await RowValueDb.UpdateAsync(item);
                }
            }

            // Cập nhật tất cả row value nếu đó là thuộc tính công thức
            await UpdateFormulaRowValue(thuocTinh);

        }

        private async Task UpdateFormulaRowValue(ThuocTinh thuocTinh)
        {
            if(thuocTinh != default)
            {
                if (!string.IsNullOrEmpty(thuocTinh.StringCongThuc))
                {
                    var rowValues = _context.RowValue.Find(x => x.ThuocTinhId == thuocTinh.Id).ToList();

                    List<string> arrays = new List<string>();
                    var getStringFormBetween = new GetStringFormBetween();
                    arrays = getStringFormBetween.Get(thuocTinh.StringCongThuc, "<", ">");

                    
                    var thuocTinhByCode = _context.ThuocTinh.Find(x => arrays.Contains(x.Code) && x.BangBieuId == thuocTinh.BangBieuId).SortBy(x => x.Order).ToList();

                    var thuocTinhByCodeIds = thuocTinhByCode.Select(x => x.Id).ToList();

                    var rowValuesTemp = _context.RowValue.Find(x => thuocTinhByCodeIds.Contains(x.ThuocTinhId) || x.ThuocTinhId == thuocTinh.Id).ToList();
              

                    foreach (var item in rowValues)
                    {
                        item.StyleInput = thuocTinh.StyleInput;
                    

                        var strCongThuc = thuocTinh.StringCongThuc;

                        foreach (var ttId in thuocTinhByCode)
                        {
                            var tt = rowValuesTemp.Where(x => x.KeyRow == item.KeyRow && x.ThuocTinhId == ttId.Id).FirstOrDefault();
                            if(tt != default)
                            {
                                strCongThuc = strCongThuc.Replace($"{ttId.Code}", tt.Code);
                            }
                        }

                        item.StringCongThuc = strCongThuc;

                        await RowValueDb.UpdateAsync(item);
                    }

                }
            }
        }
        public async Task<PagingModel<ThuocTinh>> GetPaging(PagingParam param)
        {
            PagingModel<ThuocTinh> result = new PagingModel<ThuocTinh>();
            var builder = Builders<ThuocTinh>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                filter = builder.And(filter,
                    builder.Where(x => x.Ten.Trim().ToLower().Contains(param.Content.Trim().ToLower())));
            }

            string sortBy = nameof(ThuocTinh.ThuTu);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .Sort(param.SortDesc
                    ? Builders<ThuocTinh>
                        .Sort.Descending(sortBy)
                    : Builders<ThuocTinh>
                        .Sort.Ascending(sortBy))
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<List<ThuocTinh>> GetThuocTinhByBangBieuId(string bangBieuId)
        {
            if (bangBieuId == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Không tìm thấy bảng biểu");
            }
            return await _context.ThuocTinh.Find(x => x.BangBieuId == bangBieuId && x.IsDeleted != true).SortBy(x => x.ThuTu)
                .ToListAsync();
        }
        
        public async Task<List<ThuocTinhTreeVM>> GetTreeByBangBieuId(string bangBieuId)
        {
            var listDonVi = await _context.ThuocTinh.Find(x  => x.BangBieuId == bangBieuId && x.IsDeleted ==false).SortBy(donVi => donVi.Level).ToListAsync();
            var parents = listDonVi.Where(x => x.ParentId == null).ToList();
            List<ThuocTinhTreeVM> list = new List<ThuocTinhTreeVM>();
            foreach (var item in parents)
            {
                ThuocTinhTreeVM donVi = new ThuocTinhTreeVM(item);
                list.Add(donVi);
                GetLoopItem(ref list, listDonVi, donVi);
            }
            return list;
        }

        private List<DonViTreeVM> GetLoopItem(ref List<ThuocTinhTreeVM> list, List<ThuocTinh> items, ThuocTinhTreeVM target)
        {
            try
            {
                var coquan = items.FindAll((item) => item.ParentId == target.Id).ToList();
                if (coquan.Count > 0)
                {
                    target.Children = new List<ThuocTinhTreeVM>();
                    foreach (var item in coquan)
                    {
                        ThuocTinhTreeVM itemDV = new ThuocTinhTreeVM(item);
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
        
        
        public async Task<List<ThuocTinhListTreeVM>> GetListTreeByBangBieuId(string bangBieuId)
        {
            var listDonVi = await _context.ThuocTinh.Find(_ => _.BangBieuId == bangBieuId && _.IsDeleted != true).SortByDescending(donVi => donVi.Level).ThenBy(x => x.ThuTu).ToListAsync();
            var parents = listDonVi.Where(x => x.ParentId == null).ToList();
            List<ThuocTinhListTreeVM> list = new List<ThuocTinhListTreeVM>();
            foreach (var item in parents)
            {
                ThuocTinhListTreeVM donVi = new ThuocTinhListTreeVM(item);
                list.Add(donVi);
                GetLoopItemTree(ref list, listDonVi, donVi);
            }
            return list;
        }
        private List<ThuocTinhListTreeVM> GetLoopItemTree(ref List<ThuocTinhListTreeVM> list, List<ThuocTinh> items, ThuocTinhListTreeVM target)
        {
            var coquan = items.Where((item) => item.ParentId == target.Id).OrderByDescending(x => x.Level).ThenBy(x => x.ThuTu).ToList();
            if (coquan.Count > 0)
            {
                target.Children = new List<ThuocTinhListTreeVM>();
                foreach (var item in coquan)
                {
                    ThuocTinhListTreeVM itemDV = new ThuocTinhListTreeVM(item);
                    target.Children.Add(itemDV);
                    // target.SubItems.Add(itemDV);
                    GetLoopItemTree(ref list, items, itemDV);
                }
            }
            return null;
        }

        public async Task< List<string>> GetThuocTinhCon(string id, string bangBieuId)
        {
            var listDonVi = await _context.ThuocTinh.Find(_ => _.BangBieuId == bangBieuId && _.IsDeleted != true).SortByDescending(thuocTinh => thuocTinh.Level).ToListAsync();
            var parents = listDonVi.Where(x => x.ParentId == id).ToList();
            List<ThuocTinh> list = new List<ThuocTinh>();
            foreach (var item in parents)
            {
                ThuocTinh donVi = item;
                list.Add(donVi);
                LoopGetThuocTinhCons(ref list, listDonVi, donVi);
            }
            return list != default? list.Select(x => x.Id).ToList(): new List<string>();
        }
        
        private List<ThuocTinhListTreeVM> LoopGetThuocTinhCons(ref List<ThuocTinh> list, List<ThuocTinh> items, ThuocTinh target)
        {
            var coquan = items.Where((item) => item.ParentId == target.Id).OrderByDescending(x => x.Level).ToList();
            if (coquan.Count > 0)
            {
                foreach (var item in coquan)
                {
                    // target.SubItems.Add(itemDV);
                    LoopGetThuocTinhCons(ref list, items, item);
                }
            }
            return null;
        }

        public List<ThuocTinh> GetThuocTinhChiTieu(string bangBieuId)
        {
            return _context.ThuocTinh.Find(x => x.BangBieuId == bangBieuId && x.IsChiTieu && x.IsDeleted != true)
                .ToList();
        }

        public List<ThuocTinh> GetNodeLeftChiTieu(string bangBieuId)
        {
            var thuocTinhs = _context.ThuocTinh.Find(x => x.BangBieuId == bangBieuId && x.IsDeleted != true).ToList();
            var parents = thuocTinhs.Where(x => x.ParentId == null).OrderBy(x => x.ThuTu).ToList();
            List<ThuocTinh> list = new List<ThuocTinh>();
            foreach (var item in parents)
            {
                LoopGetThuocTinhConsLeft(ref list, thuocTinhs, item);
            }

            return list;
        }
        
        private List<ThuocTinhListTreeVM> LoopGetThuocTinhConsLeft(ref List<ThuocTinh> list, List<ThuocTinh> items, ThuocTinh target)
        {
            var coquan = items.Where((item) => item.ParentId == target.Id).OrderBy(x => x.ThuTu).ToList();
            if (coquan.Count > 0)
            {
                foreach (var item in coquan)
                {
                    // target.SubItems.Add(itemDV);
                    LoopGetThuocTinhConsLeft(ref list, items, item);
                }
            }
            else
            {
                list.Add(target);
            }
            return null;
        }

        public async Task UpdateOrderThuocTinh(string bangBieuId)
        {
            var thuocTinhs = _context.ThuocTinh.Find(x => x.BangBieuId == bangBieuId && x.IsDeleted != true).ToList();
            var parents = thuocTinhs.Where(x => x.ParentId == null).OrderBy(x => x.ThuTu).ToList();
            List<ThuocTinh> list = new List<ThuocTinh>();
            foreach (var item in parents)
            {
                list.Add(item);
                LoopGetThuocTinhConsLeft1(ref list, thuocTinhs, item);
            }

            int index = 1;
            foreach (var item in list)
            {
                item.Order = index++;
                await BaseMongoDb.UpdateAsync(item);
            }
        }
        
        private List<ThuocTinhListTreeVM> LoopGetThuocTinhConsLeft1(ref List<ThuocTinh> list, List<ThuocTinh> items, ThuocTinh target)
        {
            var coquan = items.Where((item) => item.ParentId == target.Id).OrderBy(x => x.ThuTu).ToList();
            if (coquan.Count > 0)
            {
                foreach (var item in coquan)
                {
                    list.Add(item);
                    LoopGetThuocTinhConsLeft1(ref list, items, item);
                }
            }
            return null;
        }
    }
}