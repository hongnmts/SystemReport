using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class XacMinhVM
    {
        public User User { get; set; }
        public List<FileShort> UploadFiles { get; set; }
    }
}