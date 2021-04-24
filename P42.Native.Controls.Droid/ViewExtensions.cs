using System;
using System.Collections.Generic;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;

namespace P42.Native.Controls.Droid
{
    public static class ViewExtensions
    {
        public static List<Android.Views.View> Children(this Android.Views.ViewGroup viewGroup)
        {
            var children = new List<Android.Views.View>();
            for (int i = 0; i < viewGroup.ChildCount; i++)
                children.Add(viewGroup.GetChildAt(i));
            return children;
        }

        public static ColorDrawable ToColorDrawable(this Color color)
            => new ColorDrawable(color);

        public static Color ToColor(this int pv)
        {
            var R = pv & 255;
            var G = (pv >> 8) & 255;
            var B = (pv >> 16) & 255;
            var A = (pv >> 24) & 255;

            return new Color(R, G, B, A);
        }

        public static float ConvertFromDipToPx(float dip)
        {
            float scale = P42.Utils.Droid.Settings.Context.Resources.DisplayMetrics.Density;
            // convert the DP into pixel
            return dip * scale;
        }

        public static float ConvertFromDipToPx(double dip)
        {
            float scale = P42.Utils.Droid.Settings.Context.Resources.DisplayMetrics.Density;
            // convert the DP into pixel
            return (float)dip * scale;
        }

        public static void SetPadding(this View view, Thickness thickness)
        {
            view.SetPadding((int)thickness.Left, (int)thickness.Top, (int)thickness.Right, (int)thickness.Bottom);

        }
    }
}
