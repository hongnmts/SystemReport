using System.Collections.Generic;
using SystemReport.WebAPI.Models;
using Spire.Pdf.Exporting.XPS.Schema;

namespace SystemReport.WebAPI.ViewModels
{
    public class LichCongTacVM
    {
        public string NgayXepLich { get; set; }
        public List<CongViec> CongViecs { get; set; }
    }
}