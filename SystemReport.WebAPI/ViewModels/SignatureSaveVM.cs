using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class SignatureSaveVM
    {
        public string UserName { get; set; }
        public string Action { get; set; }
        public List<SignatureSave> SignatureSaves { get; set; }
    }
}