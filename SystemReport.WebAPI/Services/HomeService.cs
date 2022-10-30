using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemReport.WebAPI.Data;
using SystemReport.WebAPI.Interfaces;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Services
{
    public class HomeService : BaseService, IHomeService
    {
        private DataContext _context;
        private IDbSettings _settings;


        public HomeService(ILoggingService logger, IDbSettings settings, DataContext context,
            IRowValueService rowValueService,
            IHttpContextAccessor contextAccessor)
            : base(context, contextAccessor)
        {
            _context = context;
            _settings = settings;
        }


        public HomeValue GetHomeValue()
        {
            var homeValue = new HomeValue();
            // Lấy ra dữ liệu mẫu biểu được xuất bản
            var itemLoaiMauBieus = new List<ItemLoaiMauBieu>();

            var loaiMauBieu = _context.LoaiMauBieu.Find(x => x.IsDeleted != true).ToList();

            var mauBieuXuatBan = _context.MauBieu.Find(x => x.LastStatus != default && x.LastStatus.Code == "XB").ToList();
            foreach (var lmb in loaiMauBieu)
            {
                var itemLoaiMauBieu = new ItemLoaiMauBieu();
                itemLoaiMauBieu.Name = lmb.Ten;

                itemLoaiMauBieu.MauBieus = mauBieuXuatBan.Where(x => x.LoaiMauBieu != default && x.LoaiMauBieu.Id == lmb.Id).ToList();

                itemLoaiMauBieus.Add(itemLoaiMauBieu);
            }

            homeValue.LoaiMauBieus = itemLoaiMauBieus;

            /// End lấy dữ liệu mẫu biểu từ mẫu biểu
            /// 

            var countHomes = new List<ItemCountHome>();
            // Mẫu biểu
            var soMauBieu = _context.MauBieu.Find(x => x.IsTemplate != true).ToList();

            var itemMauBieu = new ItemCountHome("mdi mdi-layers", soMauBieu.Count, "Mẫu biểu");

            countHomes.Add(itemMauBieu);

            // Bảng biểu
            var soBangBieu = _context.BangBieu.Find(x => x.IsDeleted != true).ToList();

            var itemBangBieu = new ItemCountHome("mdi mdi-book-variant", soBangBieu.Count, "Bảng biểu");

            countHomes.Add(itemBangBieu);

            // Loại mẫu biểu
            var soLoaiMauBieu = _context.LoaiMauBieu.Find(x => x.IsDeleted != true).ToList();

            var itemLMauBieu = new ItemCountHome("mdi mdi-lightbulb-on-outline", soLoaiMauBieu.Count, "Loại mẫu biểu");

            countHomes.Add(itemLMauBieu);

            // Số chỉ tiêu
            var soChiTieu = _context.RowValue.Find(x => x.IsDeleted != true).ToList();

            var itemChiTieu = new ItemCountHome("mdi mdi-lightbulb-on-outline", soChiTieu.Count, "Chỉ tiêu");

            countHomes.Add(itemChiTieu);

            // Thuộc tính
            var soThuocTinh = _context.ThuocTinh.Find(x => x.IsDeleted != true).ToList();

            var itemThuocTinh = new ItemCountHome("mdi mdi-clipboard-check", soThuocTinh.Count, "Thuộc tính");

            countHomes.Add(itemThuocTinh);

            // Thuộc tính
            var soLinhVuc = _context.LinhVuc.Find(x => x.IsDeleted != true).ToList();

            var itemLinhVuc = new ItemCountHome("mdi mdi-memory", soLinhVuc.Count, "Lĩnh vực");

            countHomes.Add(itemLinhVuc);

            homeValue.CountHomes = countHomes;
            return homeValue;
        }


        public async Task<PagingModel<MauBieu>> GetDanhSachMauBieu(MauBieuParam param)
        {
            PagingModel<MauBieu> result = new PagingModel<MauBieu>();
            var builder = Builders<MauBieu>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Where(x => x.IsTemplate == false && x.IsDeleted == false));

            if (param.LoaiMauBieuIds != default && param.LoaiMauBieuIds.Count > 0)
            {
                filter = builder.And(filter,
                  builder.Where(x => x.LoaiMauBieu != default && param.LoaiMauBieuIds.Contains(x.LoaiMauBieu.Id)));
            }

            if (param.NamXuatBanFilter != default && param.NamXuatBanFilter.Count > 0)
            {
                int max = param.NamXuatBanFilter.Max();
                int min = param.NamXuatBanFilter.Min();
                DateTime firstDay = DateTime.Now;
                DateTime lastDay = DateTime.Now;
                if (param.NamXuatBanFilter.Count() > 1)
                {
                    firstDay = new DateTime(min, 1, 1);
                    lastDay = new DateTime(max, 1, 1).AddYears(1).AddTicks(-1);
                }
                else
                {
                    firstDay = new DateTime(max, 1, 1);
                    lastDay = firstDay.AddYears(1).AddTicks(-1);
                }

                filter = builder.And(filter,
                   builder.Where(x => x.DenNgay != default && x.DenNgay <= lastDay && x.DenNgay >= firstDay));
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

            string sortBy = nameof(MauBieu.ThuTu);
            result.TotalRows = await _context.MauBieu.CountDocumentsAsync(filter);
            result.Data = await _context.MauBieu.Find(filter)
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
    }
}
