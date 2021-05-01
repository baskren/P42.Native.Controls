using System;
using Android.Graphics.Drawables;

namespace P42.Native.Controls.Droid
{
    public static class ColorExtensions
    {
        public static ColorDrawable ToColorDrawable(this Android.Graphics.Color color)
            => new ColorDrawable(color);

        public static Android.Graphics.Color ToColor(this int pv)
        {
            var R = pv & 255;
            var G = (pv >> 8) & 255;
            var B = (pv >> 16) & 255;
            var A = (pv >> 24) & 255;

            return new Android.Graphics.Color(R, G, B, A);
        }

        public static Android.Graphics.Color WithAlpha(this Android.Graphics.Color color, int alpha)
            => new Android.Graphics.Color(color.R, color.G, color.B, alpha);

        public static Android.Graphics.Color WithAlpha(this Android.Graphics.Color color, double alpha)
            => new Android.Graphics.Color(color.R, color.G, color.B, (int)(alpha * 255));


    }
}
