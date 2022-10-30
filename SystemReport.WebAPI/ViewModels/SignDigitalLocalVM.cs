using System.Collections.Generic;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.ViewModels
{
    public class SignDigitalLocalVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Content { get; set; }
        public bool Reject { get; set; } = false;
        public bool ChoPhepKy { get; set; } = false;

        public string VanBanDiId { get; set; }
        public List<SignDigitalLocal> SignDigitals { get; set; }
        public float Scale { get; set; } = 1.0f;
    }
}