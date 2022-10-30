using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Models
{
    public class HopThu : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string TieuDe { get; set; }
        public List<UserTreeChilVM> NguoiNhans { get; set; }
        public List<UserTreeChilVM> Cc { get; set; }
        public string NoiDung { get; set; }
        public List<FileShort> Files { get; set; }
        [BsonIgnore]
        public List<FileShort> UploadFiles { get; set; }
        public UserShort NguoiGui { get; set; }
        public DateTime? NgayGui { get; set; }
        public UserShort NguoiNhan { get; set; }
        public DateTime? NgayNhan { get; set; }
        public bool DaXem { get; set; } = false;
        [BsonIgnore]
        public string NgayNhanFull
        {
            get { return NgayNhan.HasValue ? NgayNhan.Value.ToLocalTime().ToString(FormatDateFull) : ""; }
        }
    }
}