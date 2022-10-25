using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class Xa : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        
        public  string MaXa { get; set; }
        // public string XaId { get; set; }
        public string TenXa { get; set; }
        public string HuyenId { get; set; }
        public string CapDonVi { get; set; }
    }
    public class XaShort
    {
        public string Id { get; set; }
        public string Ten { get; set; }
    }
}