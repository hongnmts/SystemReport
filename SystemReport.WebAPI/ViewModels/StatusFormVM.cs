using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class StatusFormVM
    {
        public string MauBieuId { get; set; }
        public TrangThai CurrentTrangThai { get; set; }
        public TrangThai NextTrangThai { get; set; }
        public string Content { get; set; }
    }
}
