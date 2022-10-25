using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class NavMenuVM
    {
        public NavMenuVM(Menu model)
        {
            this.Id = model.Id  ?? "";
            this.Label = model.Ten  ?? "";
            this.Icon = model.Icon  ?? "";
            this.ParentId = model.ParentId ?? "";
            this.Link = string.IsNullOrEmpty(model.Path)? "/" : model.Path;
        }
        public string Id { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public string ParentId { get; set; }
        public string Link { get; set; }
        public bool IsTitle { get; set; }
        public List<NavMenuVM> SubItems { get; set; } = new List<NavMenuVM>();
    }
}