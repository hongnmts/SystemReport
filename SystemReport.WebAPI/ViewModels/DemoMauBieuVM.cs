using System.Collections.Generic;

namespace SystemReport.WebAPI.ViewModels
{
    public class DemoMauBieuVM
    {
        public string TenBangBieu { get; set; }
        public bool IsHienThiTen { get; set; }
        public int ThuTu { get; set; }
        public List<HeaderTableVM> Headers { get; set; }
        public List<BodyTableVM> Body { get; set; }
    }
}