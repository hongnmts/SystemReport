using System;
using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Params
{
    public class QuanLyKHParam : PagingParam
    {
        public List<CommonItem> CapQuanLy { get; set; }
        public List<CommonItem> PheDuyetNV { get; set; }
        public List<CommonItem> TenDeTai { get; set; }
        public List<CommonItem> ChuTri { get; set; }
        public List<CommonItem> ChuNhiem { get; set; }
        public List<CommonItem> LinhVuc { get; set; }
        public List<CommonItem> QuyetDinhPDKQ { get; set; }
        public List<CommonItem> DangThucHien { get; set; }
        public List<CommonItem> XepLoai { get; set; }
        public List<CommonItem> QuyetDinhCQ { get; set; }
        public List<CommonItem> DonViTiepNhan { get; set; }
        public List<CommonItem> SoHopDong { get; set; }
        public DateTime? NgayPDKQStart { get; set; }
        public DateTime? NgayPDKQEnd { get; set; }


        public DateTime? NgayBatDauStart { get; set; }
        public DateTime? NgayBatDauEnd { get; set; }   
        
        public DateTime? NgayKetThucStart { get; set; }
        public DateTime? NgayKetThucEnd { get; set; }

        public DateTime? NgayGiaHanStart { get; set; }
        public DateTime? NgayGiaHanEnd { get; set; }    
        
        public DateTime? NgayNghiemThuStart { get; set; }
        public DateTime? NgayNghiemThuEnd { get; set; }  
        
        public DateTime? NgayChuyenGiaoStart { get; set; }
        public DateTime? NgayChuyenGiaoEnd { get; set; }
        public DateTime? Year { get; set; }

    }
}
