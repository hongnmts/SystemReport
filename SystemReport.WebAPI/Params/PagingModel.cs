using System.Collections.Generic;

namespace SystemReport.WebAPI.Params
{
    public class PagingModel<T>
    {
        public long TotalRows { get; set; }
        
        // public long TotalRows { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}