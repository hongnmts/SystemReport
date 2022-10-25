using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class RowValueTemp
    {
        public string KeyRow { get; set; }
        public List<RowValue>  RowValues { get; set; }
    }
}