using System;

namespace SystemReport.WebAPI.Params
{
    public class QuestionParam : PagingParam
    {

    }

    public class QuestionParamFilter : QuestionParam
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DonViId { get; set; }
        public string LinhVucId { get; set; }
        public string Title { get; set; }
        public string StatusId { get; set; }
    }
}