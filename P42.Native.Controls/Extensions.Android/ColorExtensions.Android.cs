using System;
using Android.Graphics.Drawables;

using Color = Android.Graphics.Color;
using Drawable = Android.Graphics.Drawables.ColorDrawable;

namespace P42.Native.Controls
{
    public static partial class ColorExtensions
    {
        public static partial Drawable ToColorDrawable(this Color color)
            => new Drawable(color);

        public static partial Color ToColor(this int pv)
        {
            var R = pv & 255;
            var G = (pv >> 8) & 255;
            var B = (pv >> 16) & 255;
            var A = (pv >> 24) & 255;

            return new Android.Graphics.Color(R, G, B, A);
        }

        public static partial Color WithAlpha(this Color color, int alpha)
            => new Color(color.R, color.G, color.B, alpha);

        public static partial Color WithAlpha(this Color color, double alpha)
            => new Color(color.R, color.G, color.B, (int)(alpha * 255));

        public static partial Drawable AsDrawable(this Color color)
            => new Drawable(color);
    }
}
