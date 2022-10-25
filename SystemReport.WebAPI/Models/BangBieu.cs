using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class BangBieu: Audit,ICloneable, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Ten { get; set; }
        public int ThuTu { get; set; }
        public string Code { get; set; }
        public bool HienThiTen { get; set; }
        public DonViTinh DonViTinh { get; set; }
        public string MauBieuId { get; set; }
        public string CloneId { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}