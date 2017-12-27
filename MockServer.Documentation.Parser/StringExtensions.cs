namespace MockServer.Documentation.Parser
{
    using System.Globalization;

    public static class StringExtensions
    {
        public static string ToTitleCase(this string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        }
    }
}