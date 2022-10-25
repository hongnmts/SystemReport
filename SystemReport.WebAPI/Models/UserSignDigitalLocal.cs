using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class SignDigitalLocal
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        
        public string Content { get; set; }
        public bool Reject { get; set; } = false;
        public bool ChoPhepKy { get; set; } = false;
        public DateTime? NgayKy { get; set; }
        public string VanBanDiId { get; set; }
        
        
        // Thông tin chữ ký
        public string Type { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public int WidthPage { get; set; }
        public int HeightPage { get; set; }
        public int Page { get; set; }
        public string ImageBase64 { get; set; }
        
        public int LineHeight { get; set; }
        public List<string> Lines { get; set; }
        public string Text { get; set; }
        public string FontWeight { get; set; }
        public int FontSize { get; set; }
        public int Size { get; set; }
        public bool Absolute { get; set; } = false;
        
        
        [BsonIgnore]
        public string ShowNgayKy
        {
            get { return NgayKy.HasValue ? NgayKy.Value.ToLocalTime().ToString(FormatDateFull) : ""; }
        }

        [BsonIgnore]
        public string FormatDateFull
        {
            get { return "dd/MM/yyyy HH:mm:ss"; }
        }
    }
}