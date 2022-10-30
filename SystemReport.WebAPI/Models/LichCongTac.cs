using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SystemReport.WebAPI.Models
{
    public class LichCongTac : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime NgayXepLich { get; set; }
        public List<UserShort> ChuTri { get; set; }
        public List<CongViec> CongViecs { get; set; }
        public string LoaiLichCongTac { get; set; }
    }

    public class CongViec
    {
        public string Id { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public string ThoiGian { get; set; }
        public string MauSac { get; set; }
        public string DiaDiem { get; set; }
        public string NoiDung { get; set; }
        public string GhiChu { get; set; }
        public List<UserShort> ThanhPhanThamDu { get; set; }
        public string ThanhPhan { get; set; }
        public FileShort File { get; set; }
        public FileShort FileUpload { get; set; }
        public string LichCongTacId { get; set; }
        [BsonIgnore]
        public List<UserShort> ChuTri { get; set; }
        [BsonIgnore]
        public int RowSpan { get; set; }
    }

    //public class ThanhPhanThamDu
    //{
    //public string UserId { get; set; }
    //public string Ten { get; set; }
    //public string ChucVu { get; set; }
    //}
}