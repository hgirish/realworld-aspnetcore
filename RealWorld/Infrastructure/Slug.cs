using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RealWorld.Infrastructure
{
    public static  class Slug
    {
        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveDiacritics().ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s]", "");

            str = Regex.Replace(str, @"\s+", " ").Trim();

            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();

            str = Regex.Replace(str, @"\s", "-");

            return str;
        }

        public static string RemoveDiacritics(this string text)
        {
            var s = new string(text.Normalize(System.Text.NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());
            return s.Normalize(System.Text.NormalizationForm.FormC);
        }
    }
}
