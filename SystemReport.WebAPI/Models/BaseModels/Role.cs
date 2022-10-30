using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SystemReport.WebAPI.Models
{
    public class Role : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Ten { get; set; }
        public int ThuTu { get; set; }

        public List<Menu> Menus { get; set; } = new List<Menu>();
        public List<Permission> Permissions { get; set; } = new List<Permission>();
        [BsonIgnore]
        public Role Value { get; set; }
        [BsonIgnore]
        public string Label { get; set; }
        public Role()
        {

        }

        public Role(Role model)
        {
            this.Id = model.Id;
            this.Code = model.Code;
            this.Ten = model.Ten;
            this.ThuTu = model.ThuTu;
            this.Menus = model.Menus;
            this.Permissions = model.Permissions;
            this.Value = model;
            this.Label = Code + "-" + Ten;
        }
    }
}