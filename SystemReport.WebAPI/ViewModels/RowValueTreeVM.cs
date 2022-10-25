using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{

    public class RowValueTreeVM
    {
        public RowValueTreeVM(RowValue model)
        {
            this.Id = model.Id;
            this.Label = model.Value;
            this.Selected = false;
            this.Opened = true;
        }
        public string Id { get; set; }
        public dynamic Label { get; set; }
        

        public bool Selected { get; set; } = false;
        public bool Opened { get; set; } = false;
        
        public List<RowValueTreeVM> Children { get; set; }
    }
}