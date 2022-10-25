using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Params
{
    public class VanBanDenParam : PagingParam
    {
        public TrangThaiShort TrangThai { get; set; }
        public LoaiVanBan LoaiVanBan { get; set; }
        public DonVi DonViSoan { get; set; }
    }
}