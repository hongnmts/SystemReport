using System;
using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class MauBieuItemVM
    {
        public string Id { get; set; }
        public string Ten { get; set; }
        public List<BangBieuItemVM> BangBieus { get; set; } = new List<BangBieuItemVM>();
    }

    public class BangBieuItemVM
    {
        public string Id { get; set; }
        public string Ten { get; set; }
        public List<KyBaoItemVM> KyBaos { get; set; } = new List<KyBaoItemVM>();
    }

    public class KyBaoItemVM
    {
        public string Id { get; set; }
        public string Ten { get; set; }
        public string BangBieuId { get; set; }
        public KyBaoCao KyBaoCao { get; set; }
        public BangBieu BangBieu { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
    }
}