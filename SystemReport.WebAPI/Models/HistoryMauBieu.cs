using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class HistoryMauBieu : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public TrangThaiShort Status { get; set; }
        public string FormKey { get; set; }
        public string Collection { get; set; }
        public string CollectionName { get; set; }
        public string CollectionId { get; set; }
        public string Action { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ShowFullName { get => $"{UserName} - {FullName}"; }
        public string OldValue { get; set; }

        public string JsonData { get; set; }
        public string Hash { get; set; }
        public string ReferenceHash { get; set; }
    }
}
