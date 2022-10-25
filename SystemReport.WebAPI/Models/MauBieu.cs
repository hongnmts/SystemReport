using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace SystemReport.WebAPI.Models
{
    public class MauBieu: Audit, ICloneable, TEntity<string>
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }

        public string Ten { get; set; }
        public string TenNgan { get; set; }
        public string KyHieu { get; set; }
        public DonVi DonViNhan { get; set; }
        public DonVi DonViBaoCao { get; set; }
        [BsonIgnore]
        public string Nam { get
            {
                return DenNgay.HasValue ? DenNgay.Value.ToLocalTime().Year.ToString() : "";
            } }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public KyBaoCao KyBaoCao { get; set; }

        public string ShowKyBaoCao
        {
            get
            {
                if (KyBaoCao == default)
                {
                    return "";
                }
                if (KyBaoCao.LoaiKyBaoCao == "nam")
                {
                    return $"{KyBaoCao.Ten} - {DenNgay.Value.ToLocalTime().Year}";
                }else if (KyBaoCao.LoaiKyBaoCao == "giaidoan")
                {
                    return $"Giai đoạn {TuNgay.Value.ToLocalTime().Year}-{DenNgay.Value.ToLocalTime().Year}";
                }
                else
                {
                    var date = DenNgay.Value.ToLocalTime();
                    return $"Ngày {date.Day} tháng {date.Month} năm {date.Year}";
                }
            }
        }

        public bool IsTemplate { get; set; } = false;

        public int ThuTu { get; set; }
        /// Footter
        
        public LoaiMauBieu LoaiMauBieu { get; set; }
        
        // Phân quyền báo cáo
        public List<DonVi> PhanQuyenDonVi { get; set; }
        public List<string> PhanQuyenDonViIds { get; set; }

        public string CloneId { get; set; }
        public List<string> OwerIds { get; set; }
        public string OwerId { get; set; }
        public TrangThai LastStatus { get; set; }
        public List<TrangThai> ListStatus { get; set; }
        [BsonIgnore]
        public List<BangBieu> BangBieus { get; set; }

        [BsonIgnore]
        public string TuNgayDenNgay
        {
            get
            {
                if (TuNgay.HasValue)
                {
                    var tn = TuNgay.Value.ToLocalTime();
                    var dn = DenNgay.Value.ToLocalTime();
                    return $"Từ ngày {tn.Day} tháng {tn.Month} năm {tn.Year} đến ngày {dn.Day} tháng {dn.Month} năm {dn.Year}";
                }
                return "";
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}