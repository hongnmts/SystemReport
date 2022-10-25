using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SystemReport.WebAPI.Models
{
    public class FormatStyle
    {
        public int FontSize { get; set; }
        public int FontWeight { get; set; }
        /// <summary>
        /// Normal/Itali
        /// </summary>
        public string FontStyle { get; set; } = EHorizontal.LEFT.ToString();
        public string Horizontal { get; set; } = EHorizontal.LEFT.ToString();
        public string Vertical { get; set; } = EHorizontal.LEFT.ToString();
    }

    public enum EFontStyle
    {
        NORMAL,
        I
    }
    public enum EHorizontal
    {
        LEFT,
        RIGHT,
        CENTER
    }

    public enum EVertical
    {
        LEFT,
        RIGHT,
        CENTER
    }

    public class StyleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
