using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class HeaderTableVM
    {
        public int Level { get; set; }
        public List<THeaderVM> THeaderVms { get; set; }
    }

    public class THeaderVM
    {
        public string TenThuocTinh { get; set; }
        public int RowSpan { get; set; }
        public int ColSpan { get; set; }
        public int Level { get; set; }
        
        // Config Style
        public StyleModel FontStyle { get; set; }
        public StyleModel FontWeight { get; set; }
        public StyleModel FontHorizontalAlign { get; set; }
        public StyleModel FontVerticalAlign { get; set; }
        public int Width { get; set; }
        
        // Config Content
        public DonViTinh DonViTinh { get; set; }
        public GhiChu GhiChu { get; set; }
        public  FormulaModel Formula { get; set; }
        public  StyleInput StyleInput { get; set; }
        public string StringCongThuc { get; set; }
        
        public bool IsChiTieu { get; set; }
        public bool HasChildren { get; set; }
        public int ThuTu { get; set; }
        
        public int Order { get; set; }
    }
}