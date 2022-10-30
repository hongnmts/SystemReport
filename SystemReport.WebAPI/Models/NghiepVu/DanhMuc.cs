using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SystemReport.WebAPI.Models
{
    public class DanhMuc : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }

        public string Ten { get; set; }
        public string TenNgan { get; set; }
        public string KyHieu { get; set; }
        public string ParentId { get; set; }
        public LoaiDanhMuc LoaiDanhMuc { get; set; }
    }
}