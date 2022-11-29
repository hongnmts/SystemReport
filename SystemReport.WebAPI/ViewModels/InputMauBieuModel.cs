using System;
using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class InputMauBieuModel
    {
        public string MauBieuId { get; set; }
        public KyBaoCao KyBaoCao { get; set; }
        public DateTime? Nam { get; set; }

        public int NamLocal
        {
            get
            {
                return Nam.HasValue ? Nam.Value.ToLocalTime().Year : 9999;
            }
        }
        public ThangModel Thang { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public List<string> BangBieuIds { get; set; }
        
        public string KyBaoCaoCode { get; set; } = "TONGHOP";
    }

    public class ThangModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}