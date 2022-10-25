using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using SystemReport.WebAPI.Extensions;

namespace SystemReport.WebAPI.Models
{
    public class Answer: Audit , TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string Content { get; set; }

        public List<string> ListLink { get; set; } = new List<string>();
        public string UserName { get; set; }
        
        public string FullName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string QuestionId { get; set; }

        [BsonIgnore]
        public string UserNameShow
        {
            get { return UserName.HideString(); }
        }

        public StatusQuestion Status { get; set; }
        public List<FileShort> FileManagers { get; set; } = new List<FileShort>();
        
        [BsonIgnore]
        public List<FileShort> UploadFiles { get; set; } = new List<FileShort>();
        
        public List<FileShort> FileImage { get; set; } = new List<FileShort>();
        
        public List<FileShort> FilSystemReport { get; set; } = new List<FileShort>();
    }
}