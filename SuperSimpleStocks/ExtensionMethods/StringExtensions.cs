using System;

namespace SuperSimpleStocks.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this String input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
    }
}
