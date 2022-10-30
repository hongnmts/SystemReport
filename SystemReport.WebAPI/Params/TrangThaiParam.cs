using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Params
{
    public class TrangThaiParam : PagingParam
    {
        public string LoaiTrangThai { get; set; }
        public TrangThaiShort CurrentTrangThai { get; set; }
        public TrangThaiShort NewTrangThai { get; set; }
        public string MauBieuId { get; set; }
        public string NoiDung { get; set; }
        public string UserName { get; set; }
    }
}