using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Params
{
    public class VanBanDiParam : PagingParam
    {
        public string LinhVuc { get; set; }
        public TrangThaiShort TrangThai { get; set; }
        public LoaiVanBan LoaiVanBan { get; set; }
        public DonVi DonViSoan { get; set; }
    }
}