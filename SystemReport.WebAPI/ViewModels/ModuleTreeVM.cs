using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class ModuleTreeVM
    {
        public  ModuleTreeVM(){}
        public ModuleTreeVM(ModuleTreeVM model)
        {
            this.Id = model.Id;
            this.Label = model.Label;
        }
        public ModuleTreeVM(Module model)
        {
            this.Id = model.Id;
            this.Label = model.Name;
            if (model.Permissions != default)
            {
                this.Children = new List<ModuleTreeChilVM>();
                foreach (var per in model.Permissions)
                {
                    var item = new ModuleTreeChilVM()
                    {
                        Id = per.Id,
                        Label = per.Name
                    };
                    this.Children.Add(item);
                }
            }
        }
        public string Id { get; set; }
        public string Label { get; set; }
        
        
        public List<ModuleTreeChilVM> Children { get; set; }
    }

    public class ModuleTreeChilVM
    {
        public string Id { get; set; }
        public string Label { get; set; }
    } 
}