using System.Collections.Generic;

namespace SystemReport.WebAPI.ViewModels
{
    public class ThongKeHTDNVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public List<ThongKeHTDItem> Items { get; set; }

    }

    public class ThongKeHTDItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
