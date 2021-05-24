using System;
using System.Collections.Generic;
using System.Drawing;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;

namespace P42.Native.Controls
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

        public static T FindAncestor<T>(this View view) where T : View
        {
            while (view != null)
            {
                view = view.Parent as View;
                if (view is T)
                    return view as T;
            }
            return null;
        }

        public static double DipMeasuredWidth(this View view)
            => view.MeasuredWidth / DisplayExtensions.Scale();

        public static double DipMeasuredHeight(this View view)
            => view.MeasuredHeight / DisplayExtensions.Scale();

        public static void SetPadding(this View view, ThicknessI t)
            => view.SetPadding(t.Left, t.Top, t.Right, t.Bottom);

        public static bool HasPrescribedWidth(this View view)
            => view.LayoutParameters.Width > -1;

        public static bool HasPrescribedHeight(this View view)
            => view.LayoutParameters.Height > -1;


        public static RectI GetBoundsRelativeTo(this View view, View relativeToView)
        {
            var viewLocation = new int[2];
            view.GetLocationOnScreen(viewLocation);

            var relativeToViewLocation = new int[2];
            relativeToView.GetLocationOnScreen(relativeToViewLocation);

            var result = new RectI(viewLocation[0] - relativeToViewLocation[0],
                                    viewLocation[1] - relativeToViewLocation[1],
                                    view.Width, view.Height);

            return result;
        }

        public static MeasureSpecMode AsMeasureSpecMode(this Alignment alignment)
        {
            return alignment == Alignment.Stretch
                ? MeasureSpecMode.Exactly
                : MeasureSpecMode.AtMost;
        }

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
