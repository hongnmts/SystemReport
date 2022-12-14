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
using SystemReport.WebAPI.ViewModels;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;
using System.Linq;

namespace SystemReport.WebAPI.Services
{
    public class QuanLyKHNNSService : BaseService, IQuanLyKHNNSService
    {
        private DataContext _context;
        private BaseMongoDb<QuanLyKHNNS, string> BaseMongoDb;
        private IMongoCollection<QuanLyKHNNS> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public QuanLyKHNNSService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<QuanLyKHNNS, string>(_context.QuanLyKHNNS);
            _collection = context.QuanLyKHNNS;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.QuanLyKHNNSCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<QuanLyKHNNS> Create(QuanLyKHNNS model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = new QuanLyKHNNS
            {
                TenDeTai = model.TenDeTai,
                ChuTri = model.ChuTri,
                ChuNhiem = model.ChuNhiem,
                NgaySinh = model.NgaySinh,
                TrinhDo = model.TrinhDo,
                GioiTinh = model.GioiTinh,
                NgayTiepNhan = model.NgayTiepNhan,
                ToChucHoatDong = model.ToChucHoatDong,
                NgayTraHoSo = model.NgayTraHoSo,
                GiayChungNhan = model.GiayChungNhan,
                NgayCap= model.NgayCap,
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
        public async Task<QuanLyKHNNS> Update(QuanLyKHNNS model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.QuanLyKHNNS.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.TenDeTai = model.TenDeTai;
            entity.ChuTri = model.ChuTri;
            entity.ChuNhiem = model.ChuNhiem;
            entity.NgaySinh = model.NgaySinh;
            entity.TrinhDo = model.TrinhDo;
            entity.GioiTinh = model.GioiTinh;
            entity.NgayTiepNhan = model.NgayTiepNhan;
            entity.ToChucHoatDong = model.ToChucHoatDong;
            entity.NgayTraHoSo = model.NgayTraHoSo;
            entity.GiayChungNhan = model.GiayChungNhan;
            entity.NgayCap = model.NgayCap;
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


            var entity = _context.QuanLyKHNNS.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public async Task<List<QuanLyKHNNS>> Get()
        {
            return await _context.QuanLyKHNNS.Find(x => x.IsDeleted != true).SortByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<QuanLyKHNNS> GetById(string id)
        {
            return await _context.QuanLyKHNNS.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<QuanLyKHNNS>> GetPaging(QuanLyKHNNSParam param)
        {
            var result = new PagingModel<QuanLyKHNNS>();
            var builder = Builders<QuanLyKHNNS>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                param.Content = param.Content.ToLower();
                filter = builder.And(filter,
                    builder.Where(x => (x.TenDeTai != default && x.TenDeTai.Name.ToLower().Contains(param.Content))
                    || (x.ChuTri != default && x.ChuTri.Name.ToLower().Contains(param.Content))
                    || (x.ChuNhiem != default && x.ChuNhiem.Name.ToLower().Contains(param.Content))
                    ));
            }

            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            result.Data = await _collection.Find(filter)
                .SortByDescending(x => x.ModifiedAt)
                .Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            return result;
        }

        public async Task<PagingModel<QuanLyKHNNS>> GetPagingThongKe(QuanLyKHNNSParam param)
        {
            var result = new PagingModel<QuanLyKHNNS>();
            var builder = Builders<QuanLyKHNNS>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            //if (param.CapQuanLy != default && param.CapQuanLy.Count > 0)
            //{
            //    var ids = param.CapQuanLy.Select(x => x.Id).ToList();
            //    filter &= builder.Where(x => x.CapQuanLy != default && ids.Contains(x.CapQuanLy.Id));
            //}
            //if (param.PheDuyetNV != default && param.PheDuyetNV.Count > 0)
            //{
            //    var ids = param.PheDuyetNV.Select(x => x.Id).ToList();
            //    filter &= builder.Where(x => x.PheDuyetNhiemVu != default && ids.Contains(x.PheDuyetNhiemVu.Id));
            //}
            //if (param.TenDeTai != default && param.TenDeTai.Count > 0)
            //{
            //    var ids = param.TenDeTai.Select(x => x.Id).ToList();
            //    filter &= builder.Where(x => x.TenDeTai != default && ids.Contains(x.TenDeTai.Id));
            //}
            //if (param.ChuTri != default && param.ChuTri.Count > 0)
            //{
            //    var ids = param.ChuTri.Select(x => x.Id).ToList();
            //    filter &= builder.Where(x => x.ChuTri != default && ids.Contains(x.ChuTri.Id));
            //}
            //if (param.ChuNhiem != default && param.ChuNhiem.Count > 0)
            //{
            //    var ids = param.ChuNhiem.Select(x => x.Id).ToList();
            //    filter &= builder.Where(x => x.ChuNhiem != default && ids.Contains(x.ChuNhiem.Id));
            //}
            //if (param.DonViTiepNhan != default && param.DonViTiepNhan.Count > 0)
            //{
            //    var ids = param.DonViTiepNhan.Select(x => x.Id).ToList();
            //    filter &= builder.Where(x => x.DonViTiepNhan != default && x.DonViTiepNhan.Any(b => ids.Contains(b.Id)));
            //}
            //if (param.LinhVuc != default && param.LinhVuc.Count > 0)
            //{
            //    var ids = param.LinhVuc.Select(x => x.Id).ToList();
            //    filter &= builder.Where(x => x.LinhVuc != default && ids.Contains(x.LinhVuc.Id));
            //}
            //if (param.SoHopDong != default && param.SoHopDong.Count > 0)
            //{
            //    var ids = param.SoHopDong.Select(x => x.Id).ToList();
            //    filter &= builder.Where(x => x.SoHopDong != default && ids.Contains(x.SoHopDong.Id));
            //}
            //if (param.QuyetDinhPDKQ != default && param.QuyetDinhPDKQ.Count > 0)
            //{
            //    var ids = param.QuyetDinhPDKQ.Select(x => x.Id).ToList();
            //    filter &= builder.Where(x => x.QuyetDinhPDKQ != default && ids.Contains(x.QuyetDinhPDKQ.Id));
            //}
            //if (param.DangThucHien != default && param.DangThucHien.Count > 0)
            //{
            //    var ids = param.DangThucHien.Select(x => x.Id).ToList();
            //    filter &= builder.Where(x => x.DangThucHien != default && ids.Contains(x.DangThucHien.Id));
            //}
            //if (param.XepLoai != default && param.XepLoai.Count > 0)
            //{
            //    var ids = param.XepLoai.Select(x => x.Id).ToList();
            //    filter &= builder.Where(x => x.XepLoai != default && ids.Contains(x.XepLoai.Id));
            //}
            //if (param.QuyetDinhCQ != default && param.QuyetDinhCQ.Count > 0)
            //{
            //    var ids = param.QuyetDinhCQ.Select(x => x.Id).ToList();
            //    filter &= builder.Where(x => x.QuyetDinhCQ != default && ids.Contains(x.QuyetDinhCQ.Id));
            //}

            //if (param.NgayPDKQStart != default && param.NgayPDKQEnd != default)
            //{
            //    var start = DateTime.SpecifyKind((DateTime)param.NgayPDKQStart, DateTimeKind.Utc);
            //    var end = DateTime.SpecifyKind((DateTime)param.NgayPDKQEnd, DateTimeKind.Utc);

            //    filter &= builder.Where(x => x.NgayPDKQ >= start && x.NgayPDKQ <= end);
            //}
            //else
            //{
            //    if (param.NgayPDKQStart != default)
            //    {
            //        var start = DateTime.SpecifyKind((DateTime)param.NgayPDKQStart, DateTimeKind.Utc);

            //        filter &= builder.Where(x => x.NgayPDKQ >= start);
            //    }
            //    if (param.NgayPDKQEnd != default)
            //    {
            //        var end = DateTime.SpecifyKind((DateTime)param.NgayPDKQEnd, DateTimeKind.Utc);

            //        filter &= builder.Where(x => x.NgayPDKQ <= end);
            //    }
            //}

            // if (param.NgayBatDauStart != default && param.NgayBatDauEnd != default)
            // {
            //     var start = DateTime.SpecifyKind((DateTime)param.NgayBatDauStart, DateTimeKind.Utc);
            //     var end = DateTime.SpecifyKind((DateTime)param.NgayBatDauEnd, DateTimeKind.Utc);
            //
            //     filter &= builder.Where(x => x.NgayBatDau >= start && x.NgayKetThuc <= end);
            // }
            // else
            // {
            //     if (param.NgayBatDauStart != default)
            //     {
            //         var start = DateTime.SpecifyKind((DateTime)param.NgayBatDauStart, DateTimeKind.Utc);
            //
            //         filter &= builder.Where(x => x.NgayBatDau >= start);
            //     }
            //     if (param.NgayBatDauEnd != default)
            //     {
            //         var end = DateTime.SpecifyKind((DateTime)param.NgayBatDauEnd, DateTimeKind.Utc);
            //
            //         filter &= builder.Where(x => x.NgayKetThuc <= end);
            //     }
            // }

            //if (param.NgayBatDauStart != default && param.NgayBatDauEnd != default)
            //{
            //    var start = DateTime.SpecifyKind((DateTime)param.NgayBatDauStart, DateTimeKind.Utc);
            //    var end = DateTime.SpecifyKind((DateTime)param.NgayBatDauEnd, DateTimeKind.Utc);

            //    filter &= builder.Where(x => x.NgayBatDau >= start && x.NgayBatDau <= end);
            //}
            //else
            //{
            //    if (param.NgayBatDauStart != default)
            //    {
            //        var start = DateTime.SpecifyKind((DateTime)param.NgayBatDauStart, DateTimeKind.Utc);

            //        filter &= builder.Where(x => x.NgayBatDau >= start);
            //    }
            //    if (param.NgayBatDauEnd != default)
            //    {
            //        var end = DateTime.SpecifyKind((DateTime)param.NgayBatDauEnd, DateTimeKind.Utc);

            //        filter &= builder.Where(x => x.NgayBatDau <= end);
            //    }
            //}

            //if (param.NgayGiaHanStart != default && param.NgayGiaHanEnd != default)
            //{
            //    var start = DateTime.SpecifyKind((DateTime)param.NgayGiaHanStart, DateTimeKind.Utc);
            //    var end = DateTime.SpecifyKind((DateTime)param.NgayGiaHanEnd, DateTimeKind.Utc);

            //    filter &= builder.Where(x => x.NgayGiaHan >= start && x.NgayGiaHan <= end);
            //}
            //else
            //{
            //    if (param.NgayGiaHanStart != default)
            //    {
            //        var start = DateTime.SpecifyKind((DateTime)param.NgayGiaHanStart, DateTimeKind.Utc);

            //        filter &= builder.Where(x => x.NgayGiaHan >= start);
            //    }
            //    if (param.NgayGiaHanEnd != default)
            //    {
            //        var end = DateTime.SpecifyKind((DateTime)param.NgayGiaHanEnd, DateTimeKind.Utc);

            //        filter &= builder.Where(x => x.NgayGiaHan <= end);
            //    }
            //}

            //if (param.NgayNghiemThuStart != default && param.NgayNghiemThuEnd != default)
            //{
            //    var start = DateTime.SpecifyKind((DateTime)param.NgayNghiemThuStart, DateTimeKind.Utc);
            //    var end = DateTime.SpecifyKind((DateTime)param.NgayNghiemThuEnd, DateTimeKind.Utc);

            //    filter &= builder.Where(x => x.NgayNghiemThu >= start && x.NgayNghiemThu <= end);
            //}
            //else
            //{
            //    if (param.NgayNghiemThuStart != default)
            //    {
            //        var start = DateTime.SpecifyKind((DateTime)param.NgayNghiemThuStart, DateTimeKind.Utc);

            //        filter &= builder.Where(x => x.NgayNghiemThu >= start);
            //    }
            //    if (param.NgayNghiemThuEnd != default)
            //    {
            //        var end = DateTime.SpecifyKind((DateTime)param.NgayNghiemThuEnd, DateTimeKind.Utc);

            //        filter &= builder.Where(x => x.NgayNghiemThu <= end);
            //    }
            //}

            //if (param.NgayChuyenGiaoStart != default && param.NgayChuyenGiaoEnd != default)
            //{
            //    var start = DateTime.SpecifyKind((DateTime)param.NgayChuyenGiaoStart, DateTimeKind.Utc);
            //    var end = DateTime.SpecifyKind((DateTime)param.NgayChuyenGiaoEnd, DateTimeKind.Utc);

            //    filter &= builder.Where(x => x.NgayChuyenGiao >= start && x.NgayChuyenGiao <= end);
            //}
            //else
            //{
            //    if (param.NgayChuyenGiaoStart != default)
            //    {
            //        var start = DateTime.SpecifyKind((DateTime)param.NgayChuyenGiaoStart, DateTimeKind.Utc);

            //        filter &= builder.Where(x => x.NgayChuyenGiao >= start);
            //    }
            //    if (param.NgayChuyenGiaoEnd != default)
            //    {
            //        var end = DateTime.SpecifyKind((DateTime)param.NgayChuyenGiaoEnd, DateTimeKind.Utc);

            //        filter &= builder.Where(x => x.NgayChuyenGiao <= end);
            //    }
            //}

            string sortBy = nameof(QuanLyKH.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            var data = await _collection.Find(filter).SortByDescending(x => x.ModifiedAt)
                .ToListAsync();
            result.Data = await _collection.Find(filter).SortByDescending(x => x.ModifiedAt).Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            //result.SoHoSo = data.Count();
            //result.Money = data.Sum(x => x.SoTien);

            //result.ThongKeHTDNVMs = new List<ThongKeHTDNVM>();
            //result.ThongKeHTDNVMs.Add(GetThongKe(QuanLyDeTai.DETAI, "Đề tài", data));
            //result.ThongKeHTDNVMs.Add(GetThongKe(QuanLyDeTai.CHUTRI, "Chủ trì", data));
            //result.ThongKeHTDNVMs.Add(GetThongKe(QuanLyDeTai.CHUNHIEM, "Chủ nhiệm", data));
            //result.ThongKeHTDNVMs.Add(GetThongKe(QuanLyDeTai.LINHVUC, "Lĩnh vực", data));
            //result.ThongKeHTDNVMs.Add(GetThongKe(QuanLyDeTai.QUYETDINHPHEQUYET, "Quyết định phê duyệt", data));
            //result.ThongKeHTDNVMs.Add(GetThongKe(QuanLyDeTai.DANGTHUCHIEN, "Đang thực hiện", data));
            //result.ThongKeHTDNVMs.Add(GetThongKe(QuanLyDeTai.XEPLOAI, "Xếp loại", data));
            //result.ThongKeHTDNVMs.Add(GetThongKe(QuanLyDeTai.QUYETDINHCHUYENGIAO, "Quyết định chuyển giao", data));
            //result.ThongKeHTDNVMs.Add(GetThongKe(QuanLyDeTai.DONVITIEPNHAN, "Đơn vị tiếp nhận", data));
            //result.ThongKeHTDNVMs.Add(GetThongKe(QuanLyDeTai.CAPQUANLY, "Cấp quản lý", data));
            //result.ThongKeHTDNVMs.Add(GetThongKe(QuanLyDeTai.PHEDUYETNV, "Phê duyệt nhiệm vụ", data));

            return result;
        }

        private ThongKeHTDNVM GetThongKe(string type, string name, IEnumerable<QuanLyKH> data)
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
                if (type == QuanLyDeTai.DETAI)
                {
                    cItem.Count = data.Where(x => x.TenDeTai != default && x.TenDeTai.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }

                if (type == QuanLyDeTai.CHUTRI)
                {
                    cItem.Count = data.Where(x => x.ChuTri != default && x.ChuTri.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }

                if (type == QuanLyDeTai.CHUNHIEM)
                {
                    cItem.Count = data.Where(x => x.ChuNhiem != default && x.ChuNhiem.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }

                if (type == QuanLyDeTai.LINHVUC)
                {
                    cItem.Count = data.Where(x => x.LinhVuc != default && x.LinhVuc.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }

                if (type == QuanLyDeTai.QUYETDINHPHEQUYET)
                {
                    cItem.Count = data.Where(x => x.QuyetDinhPDKQ != default && x.QuyetDinhPDKQ.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }
                if (type == QuanLyDeTai.DANGTHUCHIEN)
                {
                    cItem.Count = data.Where(x => x.DangThucHien != default && x.DangThucHien.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }
                if (type == QuanLyDeTai.XEPLOAI)
                {
                    cItem.Count = data.Where(x => x.XepLoai != default && x.XepLoai.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }
                if (type == QuanLyDeTai.QUYETDINHCHUYENGIAO)
                {
                    cItem.Count = data.Where(x => x.QuyetDinhCQ != default && x.QuyetDinhCQ.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }
                if (type == QuanLyDeTai.CAPQUANLY)
                {
                    cItem.Count = data.Where(x => x.CapQuanLy != default && x.CapQuanLy.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }
                if (type == QuanLyDeTai.PHEDUYETNV)
                {
                    cItem.Count = data.Where(x => x.PheDuyetNhiemVu != default && x.PheDuyetNhiemVu.Id == item.Id).Count();
                    result.Count += cItem.Count;
                }
                if (type == QuanLyDeTai.DONVITIEPNHAN)
                {
                    cItem.Count = data.Where(x => x.DonViTiepNhan != default && x.DonViTiepNhan.Any(a => a.Id == item.Id)).Count();
                    result.Count += cItem.Count;
                }


                result.Items.Add(cItem);
            }

            // Khác
            var cItemKhac = new ThongKeHTDItem();

            cItemKhac.Id = "";
            cItemKhac.Name = "Khác";
            if (type == QuanLyDeTai.DETAI)
            {
                cItemKhac.Count = data.Where(x => x.TenDeTai == default).Count();
                result.Count += cItemKhac.Count;
            }

            if (type == QuanLyDeTai.CHUTRI)
            {
                cItemKhac.Count = data.Where(x => x.ChuTri == default).Count();
                result.Count += cItemKhac.Count;
            }

            if (type == QuanLyDeTai.CHUNHIEM)
            {
                cItemKhac.Count = data.Where(x => x.ChuNhiem == default).Count();
                result.Count += cItemKhac.Count;
            }

            if (type == QuanLyDeTai.LINHVUC)
            {
                cItemKhac.Count = data.Where(x => x.LinhVuc == default).Count();
                result.Count += cItemKhac.Count;
            }

            if (type == QuanLyDeTai.QUYETDINHPHEQUYET)
            {
                cItemKhac.Count = data.Where(x => x.QuyetDinhPDKQ == default).Count();
                result.Count += cItemKhac.Count;
            }
            if (type == QuanLyDeTai.DANGTHUCHIEN)
            {
                cItemKhac.Count = data.Where(x => x.DangThucHien == default).Count();
                result.Count += cItemKhac.Count;
            }
            if (type == QuanLyDeTai.XEPLOAI)
            {
                cItemKhac.Count = data.Where(x => x.XepLoai == default).Count();
                result.Count += cItemKhac.Count;
            }
            if (type == QuanLyDeTai.QUYETDINHCHUYENGIAO)
            {
                cItemKhac.Count = data.Where(x => x.QuyetDinhCQ == default).Count();
                result.Count += cItemKhac.Count;
            }
            if (type == QuanLyDeTai.CAPQUANLY)
            {
                cItemKhac.Count = data.Where(x => x.CapQuanLy == default).Count();
                result.Count += cItemKhac.Count;
            }
            if (type == QuanLyDeTai.PHEDUYETNV)
            {
                cItemKhac.Count = data.Where(x => x.PheDuyetNhiemVu == default).Count();
                result.Count += cItemKhac.Count;
            }
            if (type == QuanLyDeTai.DONVITIEPNHAN)
            {
                cItemKhac.Count = data.Where(x => x.DonViTiepNhan == default).Count();
                result.Count += cItemKhac.Count;
            }

            result.Items.Add(cItemKhac);

            return result;

        }
    }
}
