using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class TrangThaiTreeVM
    {
        public TrangThaiTreeVM(TrangThaiTreeVM model)
        {
            this.Id = model.Id;
            this.Code = model.Code;
            this.Label = model.Label;
        }
        public TrangThaiTreeVM(TrangThai model)
        {
            this.Id = model.Id;
            this.Label = model.Ten;
            this.Code = model.Code;
            this.Color = model.Color;
            this.BgColor = model.BgColor;
            this.Selected = false;
            this.Opened = false;
        }
        public string Id { get; set; }
        public string Label { get; set; }
        public string Code { get; set; }
        public string Color { get; set; }
        public string BgColor { get; set; }
        

        public bool Selected { get; set; } = false;
        public bool Opened { get; set; } = false;
    }
}