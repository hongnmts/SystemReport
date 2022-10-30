using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SystemReport.WebAPI.Models
{
    public class ModuleTrangThai : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public TrangThai StartTrangThai { get; set; }
        public string StartTrangThaiId { get; set; }
        public List<TrangThai> EndTrangThai { get; set; }
        public List<string> EndTrangThaiIds { get; set; }
    }
}