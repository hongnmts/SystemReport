using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class UserTreeVM
    {
        public UserTreeVM(UserTreeVM model)
        {
            this.Id = model.Id;
            this.Label = model.Label;
        }
        public UserTreeVM(User model)
        {
            this.Id = model.Id;
            this.Label = model.FullName;
            this.Selected = false;
            this.Opened = false;
        }
        public string Id { get; set; }
        public string Label { get; set; }


        public bool Selected { get; set; } = false;
        public bool Opened { get; set; } = false;
    }

    public class DonViTreeMail
    {
        public DonViTreeMail(DonVi model)
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

        public List<UserTreeChilVM> Children { get; set; }
    }
    public class UserTreeChilVM
    {
        public UserTreeChilVM() { }
        public UserTreeChilVM(User model)
        {
            this.Id = model.Id;
            this.FullName = model.FullName;
            this.UserName = model.UserName;
            this.DonVi = model.DonVi;
            this.ChucVu = model.ChucVu;
            this.Label = model.UserName + " - " + model.FullName;
            this.Selected = false;
            this.Opened = false;
        }
        public string Id { get; set; }
        public string Label { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DonVi DonVi { get; set; }
        public ChucVu ChucVu { get; set; }
        public bool Selected { get; set; } = false;
        public bool Opened { get; set; } = false;
    }
}