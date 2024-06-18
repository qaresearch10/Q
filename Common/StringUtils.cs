using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Q.Common
{
    public static class StringUtils
    {
        public static string FirstCharacterToUpperCase(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            return char.ToUpper(source[0]) + source.Substring(1);
        }

        public static string CreateRandomString(int length, bool numbers = true, bool chars = true, bool dateTime = false)
        {
            if (length < 1)
            {
                throw new ArgumentException("Length must be greater than zero", nameof(length));
            }

            const string numberChars = "0123456789";
            const string letterChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            StringBuilder characterPool = new StringBuilder();

            if (numbers)
            {
                characterPool.Append(numberChars);
            }

            if (chars)
            {
                characterPool.Append(letterChars);
            }

            if (characterPool.Length == 0)
            {
                throw new ArgumentException("At least one of 'numbers' or 'chars' must be true.");
            }

            Random random = new Random();
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(characterPool[random.Next(characterPool.Length)]);
            }

            if (dateTime)
            {
                result.Append(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            }

            return result.ToString();
        }        
    }
}
