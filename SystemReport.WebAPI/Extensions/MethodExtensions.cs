using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SystemReport.WebAPI.Models;

namespace SystemReport.WebAPI.Extensions
{
    public static class MethodExtensions
    {
        public static string HideString(this string input)
        {
            return Regex.Replace(input, @"(\d(\d{0,5})$)|(\A[a-z]{1,4})", "xxxxxx");
        }

        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users)
        {
            return users.Select(x => x.WithoutPassword());
        }

        public static User WithoutPassword(this User user)
        {
            //user.Password = null;
            user.PasswordHash = null;
            user.PasswordSalt = null;
            return user;
        }

        public static string ConvertVN(this string input)
        {
            const string FindText = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string ReplText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1;
            char[] arrChar = FindText.ToCharArray();
            while ((index = input.IndexOfAny(arrChar)) != -1)
            {
                int index2 = FindText.IndexOf(input[index]);
                input = input.Replace(input[index], ReplText[index2]);
            }
            return input;
        }
    }
}
