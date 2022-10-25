using System.Collections.Generic;
using SystemReport.WebAPI.Models;
using SystemReport.WebAPI.ViewModels;

namespace SystemReport.WebAPI.Params
{
    public class MauBieuParam : PagingParam
    {
        public LoaiMauBieu LoaiMauBieu { get; set; }
        public InputMauBieuModel ParamMauBieu { get; set; }
        public List<string> LoaiMauBieuIds { get; set; }
        public List<int> NamXuatBanFilter { get; set; }
    }
}