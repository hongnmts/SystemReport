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
using NPOI.SS.Formula.Functions;
using System.Runtime.CompilerServices;
using System.Linq;

namespace SystemReport.WebAPI.Services
{
    public class HoatDongKhoaHocService : BaseService
    {
        private DataContext _context;
        private BaseMongoDb<HoatDongKhoaHoc, string> BaseMongoDb;
        private IMongoCollection<HoatDongKhoaHoc> _collection;
        private IDbSettings _settings;
        private ILoggingService _logger;

        public HoatDongKhoaHocService(ILoggingService logger, IDbSettings settings, DataContext context,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<HoatDongKhoaHoc, string>(_context.HoatDongKhoaHoc);
            _collection = context.HoatDongKhoaHoc;
            _settings = settings;
            _logger = logger.WithCollectionName(_settings.HoatDongKhoaHocCollectionName)
                .WithDatabaseName(_settings.DatabaseName)
                .WithUserName(CurrentUserName);
        }

        public async Task<HoatDongKhoaHoc> Create(HoatDongKhoaHoc model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = new HoatDongKhoaHoc
            {
                ThamQuyenThanhLap = model.ThamQuyenThanhLap,
                LoaiHinh = model.LoaiHinh,
                LinhVucKhoaHoc = model.LinhVucKhoaHoc,
                LanhDaoDonVi = model.LanhDaoDonVi,
                GioiTinh = model.GioiTinh,
                TrinhDo = model.TrinhDo,
                QuocTich = model.QuocTich,
                NamSinh = model.NamSinh,
                NgayTiepNhan = model.NgayTiepNhan,
                DaThamDinh = model.DaThamDinh,
                TraHoSo = model.TraHoSo,
                CapLai = model.CapLai,
                CapMoi = model.CapMoi,
                GiayChungNhan = model.GiayChungNhan,
                NgayCap = model.NgayCap,
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
        public async Task<HoatDongKhoaHoc> Update(HoatDongKhoaHoc model)
        {
            if (model == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
            }

            var entity = _context.HoatDongKhoaHoc.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage(DefaultMessage.DATA_NOT_FOUND);
            }

            entity.ThamQuyenThanhLap = model.ThamQuyenThanhLap;
            entity.LoaiHinh = model.LoaiHinh;
            entity.LinhVucKhoaHoc = model.LinhVucKhoaHoc;
            entity.LanhDaoDonVi = model.LanhDaoDonVi;
            entity.GioiTinh = model.GioiTinh;
            entity.TrinhDo = model.TrinhDo;
            entity.QuocTich = model.QuocTich;
            entity.NamSinh = model.NamSinh;
            entity.NgayTiepNhan = model.NgayTiepNhan;
            entity.DaThamDinh = model.DaThamDinh;
            entity.TraHoSo = model.TraHoSo;
            entity.CapLai = model.CapLai;
            entity.CapMoi = model.CapMoi;
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


            var entity = _context.HoatDongKhoaHoc.Find(x => x.Id == id && x.IsDeleted != true).FirstOrDefault();
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

        public async Task<List<HoatDongKhoaHoc>> Get()
        {
            return await _context.HoatDongKhoaHoc.Find(x => x.IsDeleted != true).SortByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<HoatDongKhoaHoc> GetById(string id)
        {
            return await _context.HoatDongKhoaHoc.Find(x => x.Id == id && x.IsDeleted != true)
                .FirstOrDefaultAsync();
        }

        public async Task<PagingModel<HoatDongKhoaHoc>> GetPaging(HoatDongKhoaHocParam param)
        {
            var result = new PagingModel<HoatDongKhoaHoc>();
            var builder = Builders<HoatDongKhoaHoc>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));
            if (!String.IsNullOrEmpty(param.Content))
            {
                param.Content = param.Content.ToLower();
                filter = builder.And(filter,
                    builder.Where(x => (x.ThamQuyenThanhLap != default && x.ThamQuyenThanhLap.Name.ToLower().Contains(param.Content))
                    || (x.LoaiHinh != default && x.LoaiHinh.Name.ToLower().Contains(param.Content))
                    || (x.LinhVucKhoaHoc != default && x.LinhVucKhoaHoc.Name.ToLower().Contains(param.Content))
                    || (x.GioiTinh != default && x.GioiTinh.Name.ToLower().Contains(param.Content))
                    || (x.TrinhDo != default && x.TrinhDo.Name.ToLower().Contains(param.Content))
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

        public async Task<PagingModel<HoatDongKhoaHoc>> GetPagingThongKe(HoatDongKhoaHocParam param)
        {
            var result = new PagingModel<HoatDongKhoaHoc>();
            var builder = Builders<HoatDongKhoaHoc>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsDeleted == false));

            string sortBy = nameof(QuanLyKH.ModifiedAt);
            result.TotalRows = await _collection.CountDocumentsAsync(filter);
            var data = await _collection.Find(filter).SortByDescending(x => x.ModifiedAt)
                .ToListAsync();
            result.Data = await _collection.Find(filter).SortByDescending(x => x.ModifiedAt).Skip(param.Skip)
                .Limit(param.Limit)
                .ToListAsync();
            result.SoHoSo = data.Count();
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
