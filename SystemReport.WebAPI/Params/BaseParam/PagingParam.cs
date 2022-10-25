using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Params
{
    public class PagingParam
    {
        public int Start { get; set; } = 1;
        public int Limit { get; set; } = 30;
        public string SortBy { get; set; }
        public bool SortDesc { get; set; }
        public string Content { get; set; }

        public int Skip
        {
            get
            {
                return (Start > 0 ? Start - 1 : 0) * Limit;
            }
        }
    }

    public class ChildPaging : PagingParam
    {
        public  string ParentId { get; set; }
    }




    public class PagingModel<T> 
    {
        public long TotalRows { get; set; }

        // public long TotalRows { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
    
    
}