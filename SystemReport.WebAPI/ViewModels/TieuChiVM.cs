using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class TieuChiVM
    {
        public TieuChiVM() { }
        public TieuChiVM(ThuocTinh model)
        {
            this.Id = model.Id;
            this.Label = model.Ten;
            this.Text = model.Ten;
            this.Link = "";
            this.Icon = "";
            this.ParentId = model.ParentId;
            this.State.Expanded = false;
            this.Opened = false;
            this.Type = true;
            this.BangBieuId = model.BangBieuId;
        }
        public TieuChiVM(RowValue model)
        {
            this.Id = model.Id;
            this.Label = model.Value;
            this.Text = model.Value;
            this.Link = "";
            this.Icon = "";
            this.ParentId = model.ParentId;
            this.State.Expanded = false;
            this.Opened = false;
            this.Type = false;
            this.ThuocTinhId = model.ThuocTinhId;
            this.BangBieuId = model.BangBieuId;
        }

        // true -> thuộc tính; false -> chỉ tiêu
        public bool Type { get; set; } = false;
        public string Id { get; set; }
        public string ThuocTinhId { get; set; }
        public string BangBieuId { get; set; }
        public string Label { get; set; }
        public string Text { get; set; }
        public List<TieuChiVM> Children { get; set; } = new List<TieuChiVM>();
        public List<TieuChiVM> SubItems { get; set; } = new List<TieuChiVM>();
        public State State { get; set; } = new State();
        public bool Opened { get; set; } = false;
        public string ParentId { get; set; } = "";
        public string Link { get; set; } = "";
        public string Icon { get; set; } = "";
        public bool Selected { get; set; } = true;
    }
}
