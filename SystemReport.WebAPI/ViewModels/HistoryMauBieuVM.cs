using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class HistoryMauBieuVM
    {
        public BangBieu BangBieu { get; set; }
        public List<ThuocTinh> ThuocTinhs { get; set; }
        public List<RowValue> RowValues { get; set; }
    }
}
