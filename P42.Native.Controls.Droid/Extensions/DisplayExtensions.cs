using System;
using Java.Interop;

namespace P42.Native.Controls.Droid
{
    public static class DisplayExtensions
    {
        public static int Width => P42.Utils.Droid.Settings.Context.Resources.DisplayMetrics.WidthPixels;

        public static int Height => P42.Utils.Droid.Settings.Context.Resources.DisplayMetrics.HeightPixels;

        public static Size Size => new Size(Width, Height);

        public static Size DipSize
        {
            get
            {
                var scale = Scale;
                if (scale == 0)
                    scale = 1;
                return new Size(Width/Scale, Height/Scale);
            }
        }

        public static double Scale => P42.Utils.Droid.Settings.Context.Resources.DisplayMetrics.Density;

        public static float DipToPx(float dip)
            => (float)(dip * Scale);
        

        public static float DipToPx(double dip)
            =>  (float)(dip * Scale);

        public static double PxToDip(float px)
            => px / Scale;

        public static double PxToDip(double px)
            => px / Scale;


        public static Thickness DipToPx(Thickness t)
            => t * Scale;

        public static Thickness PxToDip(Thickness t)
            => t / Scale;

        public static Size DipToPx(Size sz)
            => sz * Scale;

        public static Size PxToDip(Size sz)
            => sz / Scale;

        public static Rect DipToPx(Rect r)
            => r * Scale;

        public static Rect PxToDip(Rect r)
            => r / Scale;

        public static double StatusBarHeight()
        {
            if (App.Current.CurrentView is Android.Views.View view)
            {

                var rect = new Android.Graphics.Rect();
                var window = ((Android.App.Activity)view.Context).Window;
                window.DecorView.GetWindowVisibleDisplayFrame(rect);
                return rect.Top;
            }
            return 0;
        }
    }
}
