using System.Text.RegularExpressions;

namespace RFRocketLibrary.Utils
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string text, string separator = " ")
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            return r.Replace(text, separator);
        }
    }
}