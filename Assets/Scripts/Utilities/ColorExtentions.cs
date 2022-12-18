using UnityEngine;

namespace Extensions.Colors
{
    public static class ColorExtentions
    {
        public static Color SetRed(this Color c, float r) => new Color(r, c.g, c.b, c.a);
        public static Color SetGreen(this Color c, float g) => new Color(c.r, g, c.b, c.a);
        public static Color SetBlue(this Color c, float b) => new Color(c.r, c.g, b, c.a);
        public static Color SetAlpha(this Color c, float a) => new Color(c.r, c.g, c.b, a);
    }
}
