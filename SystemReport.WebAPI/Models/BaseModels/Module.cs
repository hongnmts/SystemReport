using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class Module : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public List<Permission> Permissions { get; set; } = new List<Permission>();
        
        [BsonIgnore]
        public bool Selected { get; set; } = false;
    }
    
    
}
