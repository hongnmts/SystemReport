using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class Huyen : Audit,  TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        
        public  string MaHuyen { get; set;  }
        public string TenHuyen { get; set; }
        public string CapDonVi { get; set; }
        
        [BsonIgnore]
        public  int  SoLuongXa { get; set; }
        // public List<Xa> DanhSachXa { get; set; } = new List<Xa>();
    }
    public class HuyenShort
    {
        public string Id { get; set; }
        public string Ten { get; set; }
    }
   
}