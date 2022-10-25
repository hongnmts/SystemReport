using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class ThuocTinhTreeVM
    {
        public ThuocTinhTreeVM(DonViTreeVM model)
        {
            this.Id = model.Id;
            this.Label = model.Label;
        }
        public ThuocTinhTreeVM(ThuocTinh model)
        {
            this.Id = model.Id;
            if (model.Code != default)
            {
                this.Label = $"{model.Code}-{model.Ten}";
            }
            else
            {
                this.Label =  model.Ten;
            }

            this.Selected = false;
            this.Opened = true;
        }
        public string Id { get; set; }
        public string Label { get; set; }
        

        public bool Selected { get; set; } = false;
        public bool Opened { get; set; } = false;
        
        public List<ThuocTinhTreeVM> Children { get; set; }
    }
}