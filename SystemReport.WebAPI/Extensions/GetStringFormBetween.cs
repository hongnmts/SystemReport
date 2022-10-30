using System.Collections.Generic;

namespace SystemReport.WebAPI.Extensions
{
    public class GetStringFormBetween
    {
        private string @string = "";
        private List<string> @array = new List<string>();
        public GetStringFormBetween() { }
        private string GetFromBetween(string sub1, string sub2)
        {
            if (@string.IndexOf(sub1) < 0 || @string.IndexOf(sub2) < 0) return string.Empty;
            var SP = @string.IndexOf(sub1) + sub1.Length;
            var string1 = @string.Substring(0, SP);
            var string2 = @string.Substring(SP);
            var TP = string2.IndexOf(sub2);
            return @string.Substring(SP, TP);
        }

        private bool RemoveFromBetween(string sub1, string sub2)
        {
            if (@string.IndexOf(sub1) < 0 || @string.IndexOf(sub2) < 0) return false;

            var removal = sub1 + GetFromBetween(sub1, sub2) + sub2;
            @string = @string.Replace(removal, "");
            return true;
        }

        private bool GetAllResults(string sub1, string sub2)
        {
            if (@string.IndexOf(sub1) < 0 || @string.IndexOf(sub2) < 0) return false;

            var result = GetFromBetween(sub1, sub2);

            @array.Add(result);

            RemoveFromBetween(sub1, sub2);

            if (@string.IndexOf(sub1) > -1 && @string.IndexOf(sub2) > -1)
                GetAllResults(sub1, sub2);
            return true;
        }

        public List<string> Get(string str = null, string sub1 = null, string sub2 = null)
        {
            @string = str;
            @array = new List<string>();
            GetAllResults(sub1, sub2);

            return @array;
        }
    }
}
