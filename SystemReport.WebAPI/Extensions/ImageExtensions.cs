using System;

namespace SystemReport.WebAPI.Extensions
{
    public static class ImageExtensions
    {
        public static byte[] ConvertStringToBase64(string strImage)
        {
            var imageClip = strImage.Split(",");
            byte[] image = Convert.FromBase64String(imageClip[1]);

            return image;
        }
    }
}