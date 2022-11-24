using System.Collections.Generic;

namespace SystemReport.WebAPI.ViewModels
{
    public class TieuChiThongKeVM
    {
        public List<TieuChiVM> TieuChiView { get; set; }

        public List<TieuChiVM> TieuChis { get; set; }

        public List<string> NotChiTieu { get; set; }
    }
}
