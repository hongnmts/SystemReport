using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class KhoiCoQuan : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }
        public int ThuTu { get; set; }
        public string KhoiCoQuanId { get; set; }
    }

    public class KhoiCoQuanShort
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }
        public string KhoiCoQuanId { get; set; }
    }
}