using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class HomeValue
    {
        public List<ItemLoaiMauBieu> LoaiMauBieus { get; set; }
        public List<ItemCountHome> CountHomes { get; set; }
    }
    public class ItemCountHome
    {
        public ItemCountHome(string icon, int count, string name)
        {
            Icon = icon;
            Count = count;
            Name = name;
        }

        public string Icon { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
    }

    public class ItemLoaiMauBieu { 
    
        public string Name { get; set; }
        
        public List<MauBieu> MauBieus { get; set; }
    }
}
