using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class Permission : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public int Sort { get; set; }
        public bool IsView { get; set; }

        [BsonIgnore]
        public string IdModule { get; set; }
        [BsonIgnore]
        public bool Checked { get; set; }
    }

    public class PermissionShort
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public int Sort { get; set; }
        public bool IsView { get; set; }
    }
}