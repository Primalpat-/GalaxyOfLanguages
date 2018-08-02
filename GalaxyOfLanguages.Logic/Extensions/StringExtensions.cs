using System;

namespace GalaxyOfLanguages.Logic.Extensions
{
    public static class StringExtensions
    {
        public static bool InsensitiveContains(this string text, string value, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            return text.IndexOf(value, stringComparison) >= 0;
        }
    }
}
