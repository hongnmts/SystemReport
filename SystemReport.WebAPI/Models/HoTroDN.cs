using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SystemReport.WebAPI.Models
{
    public class HoTroDN : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public CommonItem ToChuc { get; set; }
        public CommonItem LoaiHinh { get; set; }
        public string DiaChi { get; set; }
        public CommonItem DonViHanhChinh { get; set; }
        public List<CommonItem> NoiDungHoTro { get; set; }
        public decimal SoTien { get; set; }
        public CommonItem QuyetDinh { get; set; }
        public DateTime? NgayKy { get; set; }
    }

    public class CommonItem: Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public string Type { get; set; }
    }
}
