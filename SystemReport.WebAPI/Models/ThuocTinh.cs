using System;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{

    public class ThuocTinh : Audit, ICloneable, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Ten { get; set; }
        public string TenKhongDau { get; set; }
        public int ThuTu { get; set; }
        public int Level { get; set; }
        public string ParentId { get; set; }
        public string BangBieuId { get; set; }
        public string MauBieuId { get; set; }
        public string CloneId { get; set; }
        
        // Config Style
        public StyleModel FontStyle { get; set; } = new StyleModel() {Id = "normal", Name = "Bình thường"};
        public StyleModel FontWeight { get; set; }
        public StyleModel FontHorizontalAlign { get; set; }= new StyleModel() {Id = "center", Name = "Canh giữa"};
        public StyleModel FontVerticalAlign { get; set; }
        public int Width { get; set; }
        
        // Config Content
        public DonViTinh DonViTinh { get; set; }
        public GhiChu GhiChu { get; set; }
        
        public FormulaModel Formula { get; set; }
        public StyleInput StyleInput { get; set; }
        public string StringCongThuc { get; set; }
        public bool TinhChiTieuCon { get; set; }
        
        // Config chitieu
        public bool IsChiTieu { get; set; }
        
        public int Order { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class GhiChu
    {
        public string KyHieu { get; set; }
        public string NoiDung { get; set; }
    }

    public class FormulaModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    
    public class StyleInput
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}