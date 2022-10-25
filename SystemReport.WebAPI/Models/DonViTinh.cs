using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class DonViTinh : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Ten { get; set; }
        public int ThuTu { get; set; }
    }
}
