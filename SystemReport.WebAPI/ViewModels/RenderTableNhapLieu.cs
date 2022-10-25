using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class RenderTableNhapLieu
    {
        public BangBieu BangBieu { get; set; }
        public List<HeaderTableVM> Headers { get; set; }
        public List<BodyTableVM> Body { get; set; }
        public List<RowValue> Values { get; set; }

    }
}
