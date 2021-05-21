using System;

#if __ANDROID__
using Color = Android.Graphics.Color;
using Drawable = Android.Graphics.Drawables.ColorDrawable;
#endif

namespace P42.Native.Controls
{
    public static partial class ColorExtensions
    {
        public static partial Drawable ToColorDrawable(this Color color);

        public static partial Color ToColor(this int pv);

        public static partial Color WithAlpha(this Color color, int alpha);

        public static partial Color WithAlpha(this Color color, double alpha);

        public static partial Drawable AsDrawable(this Color color);
    }
}
