using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class LichCongTacVM
    {
        public string NgayXepLich { get; set; }
        public List<CongViec> CongViecs { get; set; }
    }
}