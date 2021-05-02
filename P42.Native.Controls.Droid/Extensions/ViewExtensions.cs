using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static double DipMeasuredWidth(this View view)
            => view.MeasuredWidth / DisplayExtensions.Scale;

        public static double DipMeasuredHeight(this View view)
            => view.MeasuredHeight / DisplayExtensions.Scale;

        public static void SetPadding(this View view, Thickness t)
            => view.SetPadding((int)(t.Left + 0.5), (int)(t.Top + 0.5), (int)(t.Right + 0.5), (int)(t.Bottom + 0.5));

        /* These need to be done together ... which doesn't work so well as an Extensions.  If I only had DependencyProprties!
        public static void SetDipWidth(this View view, double value)
            => view.LayoutParameters.Width = (int)(DisplayExtensions.DipToPx(value) + 0.5);

        public static void SetDipHeight(this View view, double value)
            => view.LayoutParameters.Height = (int)(DisplayExtensions.DipToPx(value) + 0.5);

        public static void SetWidth(this View view, double value)
            => view.LayoutParameters.Width = (int)(value + 0.5);

        public static void SetHeight(this View view, double value)
            => view.LayoutParameters.Height = (int)(value + 0.5);

        public static void SetStretchWidth(this View view)
            => view.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;

        public static void SetStretchHeight(this View view)
            => view.LayoutParameters.Height = ViewGroup.LayoutParams.MatchParent;

        public static void SetStretch(this View view)
            => view.LayoutParameters.Width = view.LayoutParameters.Height = ViewGroup.LayoutParams.MatchParent;
        */
    }
}
