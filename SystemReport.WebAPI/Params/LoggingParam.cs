using System;

namespace SystemReport.WebAPI.Params
{
    public class LoggingParam : PagingParam
    {
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
    }
}