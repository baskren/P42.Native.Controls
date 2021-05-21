using System;
using Java.Interop;

namespace P42.Native.Controls
{
    public static partial class DisplayExtensions
    {
        public static partial int PxWidth() => P42.Utils.Droid.Settings.Context.Resources.DisplayMetrics.WidthPixels;

        public static partial int PxHeight() => P42.Utils.Droid.Settings.Context.Resources.DisplayMetrics.HeightPixels;

        public static partial SizeI PxSize() => new SizeI(PxWidth(), PxHeight());

        public static partial Size DipSize()
        {
                var scale = Scale();
                if (scale == 0)
                    scale = 1;
                return new Size(PxWidth()/Scale(), PxHeight()/Scale());
        }

        public static partial double Scale() => P42.Utils.Droid.Settings.Context.Resources.DisplayMetrics.Density;




        public static partial int DipToPx(double dip)
            => (int)Math.Round(dip * Scale());

        public static partial double DipToPxD(double dip)
            =>  dip * Scale();

        public static partial double PxToDip(double px)
            => px / Scale();

        public static partial PointI DipToPx(Point pt)
            => PointI.Round(pt * Scale());

        public static partial Point PxToDip(PointI pt)
            => Point.Divide(pt, Scale());

        public static partial SizeI DipToPx(Size sz)
            => SizeI.Round(sz * Scale());

        public static partial Size PxToDip(SizeI sz)
            => sz / Scale();

        public static partial RectI DipToPx(Rect r)
            => RectI.Round(r * Scale());

        public static partial Rect PxToDip(RectI r)
            => r / Scale();

        public static partial ThicknessI DipToPx(Thickness t)
            => ThicknessI.Round(t * Scale());

        public static partial Thickness PxToDip(ThicknessI t)
            => t / Scale();



        public static partial int StatusBarHeight()
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
