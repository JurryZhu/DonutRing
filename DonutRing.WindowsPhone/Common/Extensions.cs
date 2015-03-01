namespace DonutRing.Shared.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media;

    public static class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Convert hex string to Color struct, the prefix # will be removed regardless
        /// </summary>
        public static Color ColorFromString(this string hexString)
        {
            hexString = hexString.Replace("#", string.Empty);
            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;
            int start = 0;
            if (hexString.Length == 8)
            {
                a = byte.Parse(hexString.Substring(0, 2), NumberStyles.HexNumber);
                start = 2;
            }

            r = byte.Parse(hexString.Substring(start, 2), NumberStyles.HexNumber);
            g = byte.Parse(hexString.Substring(start + 2, 2), NumberStyles.HexNumber);
            b = byte.Parse(hexString.Substring(start + 4, 2), NumberStyles.HexNumber);
            return Color.FromArgb(a, r, g, b);
        }

        #endregion
    }
}
