using System;

namespace SystemReport.WebAPI.Params
{
    public class LichCongTacParam : PagingParam
    {
        public string LoaiLichCongTac { get; set; }
        public DateTime? SelectDay { get; set; }
    }

    public class PagingParamDate
    {
        public DateRange DateRange { get; set; }
        public string selectedHour { get; set; }
        public string selectedMinute { get; set; }
    }

    public class DateRange
    {
        public string end { get; set; }
        public string start { get; set; }
    }

}