namespace SystemReport.WebAPI.Models
{
    public class NhomNguoiTiepNhanVBTrinhLD
    {
        public string Id { get; set; }
        public string RoleCode { get; set; }
        public UserShort NguoiXuLy { get; set; }
        public TrangThaiShort TrangThaiXuLy { get; set; }
        public string NoiDung { get; set; }
        public int ThuTu { get; set; } = 0;
    }
}