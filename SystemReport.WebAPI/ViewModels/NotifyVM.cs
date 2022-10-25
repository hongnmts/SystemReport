using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class NotifyVM
    {
        public long Number { get; set; }
        public List<Notify> ListNotify { get; set; }
    }
}