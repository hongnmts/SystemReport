using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class LinhVuc : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }
        public int ThuTu { get; set; }
    
        public List<DonViShort> DonVis { get; set; } = new List<DonViShort>();
    }
    public class LinhVucShort
    {
        public string Id { get; set; }
        public string Ten { get; set; }
    }
}