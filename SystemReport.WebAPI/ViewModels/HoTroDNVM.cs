using System;
using System.Collections.Generic;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.Params;

namespace SystemReport.WebAPI.ViewModels
{
    public class HoTroDNVM : PagingParam
    {
        public List<CommonItem> ToChuc { get; set; }
        public List<CommonItem> LoaiHinh { get; set; }
        public List<CommonItem> DonViHanhChinh { get; set; }
        public List<CommonItem> NoiDungHoTro { get; set; }
        public List<CommonItem> QuyetDinh { get; set; }
        public DateTime? NgayNhapStart { get; set; }
        public DateTime? NgayNhapEnd { get; set; }
        public DateTime? NgayKyStart { get; set; }
        public DateTime? NgayKyEnd { get; set; }
    }
}
