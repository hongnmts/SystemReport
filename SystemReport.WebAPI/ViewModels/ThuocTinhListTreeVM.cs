using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class ThuocTinhListTreeVM
    {
        public ThuocTinhListTreeVM(ThuocTinhListTreeVM model)
        {
            this.Id = model.Id;
            this.Label = model.Text;
            this.Text = model.Text;
            this.Link = model.Link;
            this.Icon = "mdi mdi-file-document";
        }
        public ThuocTinhListTreeVM(ThuocTinh model)
        {
            this.Id = model.Id;
            if (model.Code != default)
            {
                this.Label = $"{model.Code}-{model.Ten}";
                this.Text = $"{model.Code}-{model.Ten}";
            }
            else
            {
                this.Label = model.Ten;
                this.Text = model.Ten; 
            }

            this.Link ="";
            this.Icon =  "mdi mdi-file-document";
            this.ParentId = model.ParentId;
            this.State.Expanded = true;
            this.Opened = true;
        }
        public string Id { get; set; }
        public string Label { get; set; }
        public string Text { get; set; }
        public List<ThuocTinhListTreeVM> Children { get; set; }
        public List<ThuocTinhListTreeVM> SubItems { get; set; } = new List<ThuocTinhListTreeVM>();
        public State State { get; set; } = new State();
        public bool Opened { get; set; } = false;
        public string ParentId { get; set; }= "";
        public string Link { get; set; } = "";
        public string Icon { get; set; } = "";
        public bool Selected { get; set; } = false;
    }
}