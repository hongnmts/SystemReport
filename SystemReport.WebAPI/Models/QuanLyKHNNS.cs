using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SystemReport.WebAPI.Models
{
    public class QuanLyKHNNS : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public CommonItem TenDeTai { get; set; }
        public CommonItem ChuTri { get; set; }
        public CommonItem ChuNhiem { get; set; }
        //public List<CommonItem> DanhSachThamGia { get; set; }

        public DateTime? NgaySinh { get; set; }
        [BsonIgnore]
        public int DoTuoi { get; set; }
        public CommonItem TrinhDo { get; set; }
        public CommonItem GioiTinh { get; set; }
        public DateTime? NgayTiepNhan { get; set; }
        public string ToChucHoatDong { get; set; }
        public DateTime? NgayTraHoSo { get; set; }
        public string GiayChungNhan { get; set; }
        public DateTime? NgayCap { get; set; }

    }
}
