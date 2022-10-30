using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class LoaiMauBieuTreeVM
    {

        public LoaiMauBieuTreeVM(LoaiMauBieuTreeVM model)
        {
            this.Id = model.Id;
            this.Label = model.Label;
        }
        public LoaiMauBieuTreeVM(LoaiMauBieu model)
        {
            this.Id = model.Id;
            this.Label = model.Ten;
            this.Selected = false;
            this.Opened = false;
        }
        public string Id { get; set; }
        public string Label { get; set; }


        public bool Selected { get; set; } = false;
        public bool Opened { get; set; } = false;
    }
}