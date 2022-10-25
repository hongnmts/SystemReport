using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class BodyTableVM
    {
        public string KeyRow { get; set; }
        public int ThuTu { get; set; }
        public List<RowValue> RowValues { get; set; }
        public bool IsDeleted { get; set; }
    }
}