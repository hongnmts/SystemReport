using System;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class VanBanDenVM
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public int Number { get; set; }
        public string LoaiVanBan { get; set; }
        public string TrangThai { get; set; }
        public string SoLuuCV { get; set; }
        public string SoVBDen { get; set; }
        public DateTime? NgayNhan { get; set; }
        public DateTime? NgayBanHanh { get; set; }
        public string TrichYeu { get; set; }
        public string HinhThucNhan { get; set; }
        public string LinhVuc { get; set; }
        public string MucDoBaoMat { get; set; }
        public string MucDoTinhChat { get; set; }
        public string HoSoDonVi { get; set; }
        public string NoiLuuTru { get; set; }
        public string CoQuanGui { get; set; }
        public DateTime? HanXuLy { get; set; }
        public bool CongVanChiDoc { get; set; }
        public bool BanChinh { get; set; }
        public bool HienThiThongBao { get; set; }
        public string NguoiKy { get; set; }
        public DateTime? NgayKy { get; set; }
        public FileShort File { get; set; }
        public FileShort UploadFiles { get; set; }
    }
}