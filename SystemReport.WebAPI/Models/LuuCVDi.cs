using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class LuuCVDi : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string VanBanId { get; set; }
        public int Version { get; set; }
        public int Number { get; set; }
        public int Identity { get; set; }
        public LoaiVanBan LoaiVanBan { get; set; }
        public TrangThaiShort TrangThai { get; set; }
        public string SoLuuCV { get; set; }
        public string SoVBDi{ get; set; }
        public DateTime? NgayNhap { get; set; }
        public DateTime? NgayBanHanh { get; set; }
        public DateTime? NgayTraLoi { get; set; }
        public string TraLoiCVSo { get; set; }
        public int SoBan { get; set; }
        public string TrichYeu { get; set; }
        public DonVi DonViSoan { get; set; }
        public string DonViSoanTen { get; set; }
        public User CanBoSoan { get; set; }
        public string CanBoSoanTen { get; set; }
        public HinhThucGui HinhThucGui { get; set; }
        public LinhVuc LinhVuc { get; set; }
        public string LinhVucTen { get; set; }
        public EnumModel MucDoBaoMat { get; set; }
        public string MucDoBaoMatTen { get; set; }
        public EnumModel MucDoTinhChat { get; set; }
        public string MucDoTinhChatTen { get; set; }
        public HoSoDonVi HoSoDonVi { get; set; }
        public string HoSoDonViTen { get; set; }
        public string NoiLuuTru { get; set; }
        public CoQuan CoQuanNhan { get; set; }
        public string CoQuanNhanTen { get; set; }
        
        public KhoiCoQuan KhoiCoQuanNhan { get; set; }
        public string KhoiCoQuanNhanTen { get; set; }
        
        public CoQuan CoQuanGui { get; set; }
        public string CoQuanGuiTen { get; set; }
        public KhoiCoQuan KhoiCoQuanGui { get; set; }
        public string KhoiCoQuanGuiTen { get; set; }
        public DateTime? HanXuLy { get; set; }
        public bool CongVanChiDoc { get; set; }
        public bool BanChinh { get; set; }
        public bool HienThiThongBao { get; set; }
        public bool TrinhLanhDaoTruong { get; set; }
        public User NguoiKy { get; set; }
        public DateTime? NgayKy { get; set; }
        public string FileKySoPhapLy { get; set; }
        public string NoiDungTuChoi { get; set; }
        public List<FileShort> File { get; set; }
        public List<FileShort> FilePDF { get; set; }
        public List<PhanCongKySo> PhanCongKySo { get; set; }
        [BsonIgnore]
        public List<FileShort> UploadFiles{ get; set; }
        public List<NguoiPhanCong> NguoiPhanCong { get; set; }
        public UserShort Ower { get; set; }
        public List<string> ListOwerId { get; set; } = new List<string>();
        public List<NhomNguoiTiepNhanVBTrinhLD> NhomNguoiTiepNhanVBTrinhLD { get; set; }
        public NhomNguoiTiepNhanVBTrinhLD LanhDaoDonVi { get; set; }
        public List<string> NguoiDuocBanHanh { get; set; }

        public List<SignDigital> SignDigitals { get; set; }
    }
}