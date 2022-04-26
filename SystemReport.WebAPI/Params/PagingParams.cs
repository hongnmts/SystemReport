using System;

namespace SystemReport.WebAPI.Params
{
    public class PagingParams
    {
        private int _pageNumber = 0;

        public int PageNumber
        {
            get => (_pageNumber > 0) ? _pageNumber - 1 : 0;
            set => _pageNumber = value;
        }

        public int PageSize { get; set; } = 10;

        public string SortBy { get; set; }
        public bool SortDesc { get; set; }
        public string Content { get; set; }
    }
}