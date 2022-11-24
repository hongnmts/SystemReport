using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SystemReport.WebAPI.Models
{
    public class QuanLyKH: Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public CommonItem TenDeTai { get; set; }
        public CommonItem ChuTri { get; set; }
        public CommonItem ChuNhiem { get; set; }
        public CommonItem LinhVuc { get; set; }
        public CommonItem QuyetDinhPDKQ { get; set; }
        public DateTime? NgayPDKQ { get; set; }
        public decimal NguonNSNN { get; set; }
        public decimal NguonKhac { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public DateTime? NgayGiaHan { get; set; }
        /// <summary>
        /// Đang thực hiện chưa rõ
        /// </summary>
        public CommonItem DangThucHien { get; set; }
        public DateTime? NgayNghiemThu { get; set; }
        public CommonItem XepLoai { get; set; }
        public CommonItem QuyetDinhCQ { get; set; }
        public DateTime? NgayChuyenGiao { get; set; }
        public List<CommonItem> DonViTiepNhan { get; set; }
        /// <summary>
        /// Ghi Chú
        /// </summary>
        public string MucTieu { get; set; }
        public string NoiDung { get; set; }
        public string SanPham { get; set; }
    }
}
