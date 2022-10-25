using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{

    public class LoaiTrangThai : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public  string Ten { get; set;  }
        public string Code { get; set; }
        public string MoTa { get; set; }
        public int ThuTu { get; set; }
        public List<TrangThaiShort> ListTrangThai { get; set; }
    }

    public class TrangThaiShort
    {
        public string Id { get; set; }
        public string Ten { get; set; }
        public string Code { get; set; }
        public string BgColor { get; set; }
        public string Color { get; set; }
    }
}