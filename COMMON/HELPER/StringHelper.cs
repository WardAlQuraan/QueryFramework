using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.HELPER
{
    public static class StringHelper
    {
        public static string ToUnderScores(this string input)
        {
            StringBuilder result = new StringBuilder();
            bool prevUnderscore = false;

            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];

                if (char.IsLetterOrDigit(currentChar))
                {
                    if (char.IsUpper(currentChar))
                    {
                        if (!prevUnderscore && i > 0)
                        {
                            result.Append('_');
                        }
                        result.Append(char.ToLower(currentChar));
                    }
                    else
                    {
                        result.Append(currentChar);
                    }
                    prevUnderscore = false;
                }
                else
                {
                    prevUnderscore = true;
                }
            }

            return result.ToString().ToUpper();
        }
        public static string ToPascalCase(this string input)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string[] words = input.Split('_');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = textInfo.ToTitleCase(words[i]);
            }
            return string.Join("", words);

        }
    }
}
