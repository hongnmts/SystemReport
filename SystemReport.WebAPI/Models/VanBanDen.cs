using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemReport.WebAPI.Models
{
    public class VanBanDen : Audit, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public int Version { get; set; }
        public int Number { get; set; }
        public int Identity { get; set; }
        public LoaiVanBan LoaiVanBan { get; set; }
        public TrangThaiShort TrangThai { get; set; }
        public string TrangThaiTen { get; set; }
        public string SoLuuCV { get; set; }
        public string SoVBDen { get; set; }
        public DateTime? NgayNhap { get; set; }
        public DateTime? NgayNhan { get; set; }
        public DateTime? NgayBanHanh { get; set; }
        public DateTime? NgayTraLoi { get; set; }
        public string TraLoiCVSo { get; set; }
        public string SoBan { get; set; }
        public string TrichYeu { get; set; }
        public DonVi DonViSoan { get; set; }
        public string DonViSoanTen { get; set; }
        public User CanBoSoan { get; set; }
        public HinhThucGui HinhThucNhan { get; set; }

        public HinhThucGui HinhThucGui { get; set; }

        public LinhVuc LinhVuc { get; set; }

        public EnumModel MucDoBaoMat { get; set; }

        public EnumModel MucDoTinhChat { get; set; }

        public HoSoDonVi HoSoDonVi { get; set; }

        public string NoiLuuTru { get; set; }
        public CoQuan CoQuanNhan { get; set; }


        public KhoiCoQuan KhoiCoQuanNhan { get; set; }


        public CoQuan CoQuanGui { get; set; }
        public KhoiCoQuan KhoiCoQuanGui { get; set; }
        public DateTime? HanXuLy { get; set; }
        public bool CongVanChiDoc { get; set; }
        public bool BanChinh { get; set; }
        public bool HienThiThongBao { get; set; }
        public User NguoiKy { get; set; }
        public DateTime? NgayKy { get; set; }
        public List<FileShort> File { get; set; }
        [BsonIgnore]
        public List<FileShort> UploadFiles { get; set; }
        public List<NguoiPhanCong> NguoiPhanCong { get; set; }

        public ButPhe ButPhe { get; set; }
        public List<DonViXuLy> DonViXuLy { get; set; }

        public List<PhanCong> PhanCong { get; set; } = new List<PhanCong>();

        public UserShort Ower { get; set; }

        public List<string> ListOwerId { get; set; } = new List<string>();
        public List<NhomNguoiTiepNhanVBTrinhLD> NhomNguoiTiepNhanVBTrinhLD { get; set; }

        public NhomNguoiTiepNhanVBTrinhLD LanhDaoDonVi { get; set; }
        public UserShort GetOwerWithRole(string role)
        {
            var owerTemp = this.NhomNguoiTiepNhanVBTrinhLD
                .Where(x => x.RoleCode == role).FirstOrDefault();
            if (owerTemp != default)
                return owerTemp.NguoiXuLy;
            return null;
        }

        public List<string> NguoiDuocBanHanh { get; set; }
        public bool TrinhLanhDaoTruong { get; set; }
        public string NoiDungTuChoi { get; set; }
    }

    public class ButPhe
    {
        public string Id { get; set; }
        public string VanBanDenId { get; set; }
        public string NoiDungButPhe { get; set; }
        public UserShort NguoiButPhe { get; set; }
        public DateTime? NgayButPhe { get; set; }
        public List<FileShort> File { get; set; }
        [BsonIgnore]
        public List<FileShort> UploadFiles { get; set; }
        public EnumModel MucDoQuanTrong { get; set; }
        public List<UserShort> NguoiPhuTrach { get; set; }
        public UserShort NguoiChuTri { get; set; }
        public List<UserShort> NguoiPhoiHopXuLy { get; set; }
        public List<DonViShort> DonViPhoiHop { get; set; }
        public List<UserShort> NguoiXemDeBiet { get; set; }
    }

    public class DonViXuLy
    {
        public string Id { get; set; }
        public string VanBanDenId { get; set; }
        public User NguoiPhuTrach { get; set; }
        public User NguoiChuTri { get; set; }
        public User NguoiPhoiHopXuLy { get; set; }
        public User NguoiXemDeBiet { get; set; }
        public DonVi DonViNhanXuLy { get; set; }
        public DonVi DonViPhoiHop { get; set; }
        public TrangThai TrangThaiDuyetVB { get; set; }
        public User NguoiDuyetVB { get; set; }
        public DateTime? NgayDuyetVB { get; set; }
        public string GhiChu { get; set; }
    }

    public class PhanCong
    {
        public string Id { get; set; }
        public string VanBanDenId { get; set; }
        public string YKienChiDao { get; set; }
        public User NguoiButPhe { get; set; }
        public List<User> NguoiNhanXuLy { get; set; }
    }
}