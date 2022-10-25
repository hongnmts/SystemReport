using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class Menu : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Ten { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public bool Active { get; set; }
        public  string Action { get; set; }
        public string ParentId { get; set; }
        public int Level { get; set; }
        public int Sort { get; set; }
        
        [BsonIgnore]
        public bool Selected { get; set; } = false;
    }
}