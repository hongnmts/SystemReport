using System.ComponentModel;

namespace SystemReport.WebAPI.Models
{
    public enum EMucDo
    {
        [Description("Thấp")]
        THAP,
        [Description("Trung bình")]
        TRUNGBINH,
        [Description("Cao")]
        CAO,
    }
}