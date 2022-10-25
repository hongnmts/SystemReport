using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class TrangThai : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Ten { get; set; }
        public string Color { get; set; }
        public string BgColor { get; set; }
        public int ThuTu { get; set; }
        public List<TrangThaiShort> NextTrangThai { get; set; }
        public List<PermissionShort> Actions { get; set; }
        public User CurrentUser { get; set; }
        public string Content { get; set; }
    }
    
    public class TrangThaiName
    {
        public string Id { get; set; }
        public string Ten { get; set; }
        public int SoNgay { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool Default { get; set; } = false;
        public List<PermissionShort> Permissions { get; set; }
     }
}