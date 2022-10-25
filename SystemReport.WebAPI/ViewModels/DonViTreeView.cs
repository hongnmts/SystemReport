using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class DonViTreeVM
    {

        public DonViTreeVM(DonViTreeVM model)
        {
            this.Id = model.Id;
            this.Label = model.Label;
        }
        public DonViTreeVM(DonVi model)
        {
            this.Id = model.Id;
            this.Label = model.Ten;
            this.Selected = false;
            this.Opened = true;
        }
        public string Id { get; set; }
        public string Label { get; set; }
        

        public bool Selected { get; set; } = false;
        public bool Opened { get; set; } = false;
        
        public List<DonViTreeVM> Children { get; set; }
    }
}