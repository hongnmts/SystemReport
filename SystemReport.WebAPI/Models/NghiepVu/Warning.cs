using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SystemReport.WebAPI.Models
{
    public class Warning : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Content { get; set; }
        [BsonDateTimeOptions]
        public DateTime WarningDate { get; set; }
        public string Priority { get; set; } //WarningPriority
        public List<FileShort> FileManagers { get; set; }
        [BsonIgnore]
        public List<FileShort> UploadFiles { get; set; }
    }

    public enum EWarningPriority
    {
        LEVEL0,
        LEVEL1,
        LEVEL2,
        LEVEL3,
        NONE
    }
}