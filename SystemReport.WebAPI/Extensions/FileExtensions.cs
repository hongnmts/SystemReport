using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SystemReport.WebAPI.Extensions
{
    public class FileExtensions
    {
        static List<string> ImageExtentions = new List<string>
        {
            ".jpg", ".jpeg", ".png"
        };
        public static bool IsImageExtention(string ext)
        {
            return ImageExtentions.Any(x => x.Equals(ext));
        }
        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        public static string GeneratePathFile()
        {
            return Path.Combine("Files",
                        DateTime.Now.Year.ToString(),
                        DateTime.Now.Month.ToString(),
                        DateTime.Now.Day.ToString());
        }
        public static string GenerateThumbPathFile()
        {
            return Path.Combine("Thumbs",
                        DateTime.Now.Year.ToString(),
                        DateTime.Now.Month.ToString(),
                        DateTime.Now.Day.ToString());
        }
        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        public static bool Resize(Stream input, string savePath, int maxWidth, int maxHeight)
        {
            try
            {
                using (var inputStream = new SKManagedStream(input))
                {
                    using (var original = SKBitmap.Decode(inputStream))
                    {
                        int width, height;
                        if (original.Width > original.Height)
                        {
                            width = maxWidth;
                            height = original.Height * maxWidth / original.Width;
                        }
                        else
                        {
                            width = original.Width * maxHeight / original.Height;
                            height = maxHeight;
                        }

                        using (var resized = original.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium))
                        {
                            if (resized == null) return false;

                            using (var image = SKImage.FromBitmap(resized))
                            {
                                FileInfo file = new System.IO.FileInfo(savePath);
                                file.Directory.Create();
                                using (var stream = new FileStream(savePath, FileMode.Create))
                                {
                                    image.Encode(SKEncodedImageFormat.Jpeg, 75).SaveTo(stream);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
