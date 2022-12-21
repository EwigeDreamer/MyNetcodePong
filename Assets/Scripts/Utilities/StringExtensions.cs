using UnityEngine;

namespace Extensions.Strings
{
    public static class StringExtensions
    {
        public static string Bold(this string text) => string.Format("<b>{0}</b>", text);
        public static string Italic(this string text) => string.Format("<i>{0}</i>", text);
        public static string Color(this string text, Color color) => text.Color(ColorUtility.ToHtmlStringRGB(color));
        public static string Color(this string text, string hex) => string.Format("<color=#{1}>{0}</color>", text, hex);
    }
}