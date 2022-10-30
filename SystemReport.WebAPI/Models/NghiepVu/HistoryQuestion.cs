using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class HistoryVanBanDi : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string VanBanId { get; set; }
        public TrangThaiShort TrangThai { get; set; }
        public string Action { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Hash { get; set; }
        public string ReferenceHash { get; set; }
        public dynamic OldValue { get; set; }
    }
}