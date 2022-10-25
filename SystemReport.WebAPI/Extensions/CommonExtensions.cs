using System;
using System.Globalization;
using System.Linq;

namespace SystemReport.WebAPI.Extensions
{
    public static class CommonExtensions
    {
        public static string formatDateToString(DateTime datetime)
        {
            if (datetime.ToString("dd/MM/yyyy").Equals("01/01/0001"))
            {
                return "";
            }
            else
            {
                CultureInfo viVn = new CultureInfo("vi-VN");
                if (datetime != default)
                {
                    return datetime.ToString("d", viVn);
                }
            }

            return "";
        }

        public static DateTime formatStringToDate(string dateStr = "01/01/1900")
        {
            DateTime dt = new DateTime(2100, 1, 1);
            if (string.IsNullOrEmpty(dateStr))
                dateStr = "01/01/1900";
            string formats = "dd/MM/yyyy";
            if (dateStr != null && !dateStr.Equals(""))
            {
                dt = DateTime.ParseExact(dateStr, formats, CultureInfo.InvariantCulture);
            }
            return dt;
        }

        public static DateTime formatStringToDate_WithStartTime(string dateStr = "01/01/1900")
        {
            DateTime dt = new DateTime(2100, 1, 1, 0, 0, 1);
            if (string.IsNullOrEmpty(dateStr))
                dateStr = "01/01/1900";
            string formats = "dd/MM/yyyy";
            if (dateStr != null && !dateStr.Equals(""))
            {
                dt = DateTime.ParseExact(dateStr, formats, CultureInfo.InvariantCulture);
            }
            return dt;
        }
        public static DateTime formatStringToDate_WithEndTime(string dateStr = "01/01/1900")
        {
            DateTime dt = new DateTime(2100, 1, 1, 23, 59, 59);
            if (string.IsNullOrEmpty(dateStr))
                dateStr = "01/01/1900";
            string formats = "dd/MM/yyyy";
            if (dateStr != null && !dateStr.Equals(""))
            {
                dt = DateTime.ParseExact(dateStr, formats, CultureInfo.InvariantCulture);
                dt = dt.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            return dt;
        }

        public static string ChangeFileNameToPrePDF(string fileName)
        {
      
            var dateTimeNow = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            string str = "";
            try
            {
                var array = fileName.Split(".").ToList();
                if (array.Count > 0)
                {
                    array = array.Take(array.Count - 1).ToList();
                    str = string.Join("", array) + "_" + dateTimeNow + "_signed.pdf";
                    return str;
                }

                return fileName;
            }
            catch (Exception e)
            {
                return fileName;
            }
        }
        
        public static string ChangeFileNameToPreLocalPDF(string fileName)
        {
      
            var dateTimeNow = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            string str = "";
            try
            {
                var array = fileName.Split(".").ToList();
                if (array.Count > 0)
                {
                    array = array.Take(array.Count - 1).ToList();
                    str = string.Join("", array) + "_" + dateTimeNow + "_local_signed.pdf";
                    return str;
                }

                return fileName;
            }
            catch (Exception e)
            {
                return fileName;
            }
        }
        public static string GenerateNewRandomDigit()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                r = GenerateNewRandomDigit();
            }
            return r;
        }
    }
}
