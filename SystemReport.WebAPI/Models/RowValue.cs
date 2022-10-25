using System;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class RowValue : Audit, ICloneable, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }
        public string KeyRow { get; set; }
        public int ThuTu { get; set; }
        public int Level { get; set; }
        public string ParentId { get; set; }
        public string BangBieuId { get; set; }
        public string ThuocTinhId { get; set; }
        public string CloneId { get; set; }
        public string MauBieuId { get; set; }
        
        public StyleInput StyleInput { get; set; }
        public dynamic Value { get; set; }
        public dynamic StyleValue { get; set; }
        
        public GhiChu GhiChu { get; set; }
        
        // Config Style
        public StyleModel FontStyle { get; set; } = new StyleModel() {Id = "normal", Name = "Bình thường"};
        public StyleModel FontWeight { get; set; }
        public StyleModel FontHorizontalAlign { get; set; }= new StyleModel() {Id = "center", Name = "Canh giữa"};
        public StyleModel FontVerticalAlign { get; set; }
        public int Width { get; set; }
        
        public string StringCongThuc { get; set; }
        public bool TinhTongChiTieuCon { get; set; }
        
        /// <summary>
        /// Xác định dữ liệu là để tính tổng
        /// </summary>
        public bool IsTong { get; set; }
        public string RowParent { get; set; }
        public int Order { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}