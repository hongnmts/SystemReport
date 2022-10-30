using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SystemReport.WebAPI.Models
{

    public class ChiTieu : Audit, ICloneable, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Ten { get; set; }
        public int ThuTu { get; set; }
        //public int Level { get; set; }
        public string ParentId { get; set; }
        public string BangBieuId { get; set; }
        public string MauBieuId { get; set; }
        public string CloneId { get; set; }

        // Config Style
        public StyleModel FontStyle { get; set; } = new StyleModel() { Id = "normal", Name = "Bình thường" };
        public StyleModel FontWeight { get; set; }
        public StyleModel FontHorizontalAlign { get; set; } = new StyleModel() { Id = "center", Name = "Canh giữa" };
        public StyleModel FontVerticalAlign { get; set; }
        public int Width { get; set; }

        // Config Content
        public DonViTinh DonViTinh { get; set; }
        public GhiChu GhiChu { get; set; }

        public FormulaModel Formula { get; set; }
        public StyleInput StyleInput { get; set; }
        public string StringCongThuc { get; set; }
        public bool IsLabel { get; set; }

        // Config chitieu      

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}