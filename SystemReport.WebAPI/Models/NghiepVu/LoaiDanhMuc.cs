using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class LoaiDanhMuc : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Ten { get; set; }
        public string TenNgan { get; set; }
        public int ThuTu { get; set; }

        [BsonIgnore]
        public int CountLoaiDanhMuc { get; set; }
    }
}