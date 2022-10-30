using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace SystemReport.WebAPI.Models
{
    public class CongVan : Audit, TEntity<string>
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
        public List<FileShort> UploadFiles { get; set; }

    }
}