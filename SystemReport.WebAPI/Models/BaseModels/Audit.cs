using System;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class Audit
    {
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }= DateTime.Now;
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; } 

        [BsonDefaultValue(false)]
        public bool IsDeleted { get; set; }
        [BsonIgnore]
        public string CreatedAtShow
        {
            get { return CreatedAt.HasValue ? CreatedAt.Value.ToLocalTime().ToString(FormatDate) : ""; }
        }
        [BsonIgnore]
        public string CreatedAtFullShow
        {
            get { return CreatedAt.HasValue ? CreatedAt.Value.ToLocalTime().ToString(FormatDateFull) : ""; }
        }
        [BsonIgnore]
        public string CreatedAtTimeShow
        {
            get { return CreatedAt.HasValue ? CreatedAt.Value.ToLocalTime().ToString(FormatTime) : ""; }
        }
        [BsonIgnore]
        public string LastModifiedShow
        {
            get { return ModifiedAt.HasValue ? ModifiedAt.Value.ToLocalTime().ToString(FormatDate) : ""; }
        }
        [BsonIgnore]
        public string FormatDate
        {
            get { return "dd/MM/yyyy"; }
        }
        [BsonIgnore]
        public string FormatTime
        {
            get { return "HH:mm:ss"; }
        }
        [BsonIgnore]
        public string FormatDateFull
        {
            get { return "dd/MM/yyyy HH:mm:ss"; }
        }
    }
}