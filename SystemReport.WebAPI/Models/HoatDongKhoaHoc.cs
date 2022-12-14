using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SystemReport.WebAPI.Models
{
    public class HoatDongKhoaHoc : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public CommonItem ThamQuyenThanhLap { get; set; }
        public CommonItem LoaiHinh { get; set; }
        public CommonItem LinhVucKhoaHoc { get; set; }
        public string LanhDaoDonVi { get; set; }
        public CommonItem GioiTinh { get; set; }
        public CommonItem TrinhDo { get; set; }
        public CommonItem QuocTich { get; set; }
        public DateTime? NamSinh { get; set; }
        public DateTime? NgayTiepNhan { get; set; }
        public DateTime? DaThamDinh { get; set; }
        public DateTime? TraHoSo { get; set; }
        public string CapMoi { get; set; }
        public string CapLai { get; set; }
        public string GiayChungNhan { get; set; }
        public DateTime? NgayCap { get; set; }
    }
}
