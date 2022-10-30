using System.Collections.Generic;

namespace SystemReport.WebAPI.Models
{
    public class SignDigitalVM
    {
        public string VanBanDiId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int WidthPage { get; set; }
        public int HeightPage { get; set; }
        public FileShort File { get; set; }
        public List<SignDigital> SignDigitals { get; set; }
        public float Scale { get; set; } = 1.0f;
    }
    public class SignDigital
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string TypeKySo { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public int WidthPage { get; set; }
        public int HeightPage { get; set; }
        public int Page { get; set; }
        public string Image { get; set; }
        public string ImageBase64 { get; set; }
        public dynamic Payload { get; set; }
        public dynamic File { get; set; }

        public string FontFamily { get; set; }
        public int LineHeight { get; set; }
        public List<string> Lines { get; set; }
        public int Size { get; set; }
        public string Text { get; set; }
        public string FontWeight { get; set; }
        public int FontSize { get; set; }
        public bool Absolute { get; set; } = false;
        public UserShort CurrentUser { get; set; }
    }
}