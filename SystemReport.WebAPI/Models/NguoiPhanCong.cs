using System;

namespace SystemReport.WebAPI.Models
{
    public class NguoiPhanCong
    {
        public string UserId { get; set; }
        public string Ten { get; set; }
        public string GhiChu { get; set; }
        public DateTime? Sign { get; set; }
    }
}