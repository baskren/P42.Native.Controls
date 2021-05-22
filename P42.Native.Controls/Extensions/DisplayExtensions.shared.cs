using System;

namespace P42.Native.Controls
{
    public static partial class DisplayExtensions
    {
        public static partial int PxWidth();

        public static partial int PxHeight();

        public static partial SizeI PxSize();

        public static partial Size DipSize();

        public static partial double Scale();


        public static int DipToPx(this double value)
        {
            if (double.IsNaN(value))
                return 0;
            if (double.IsPositiveInfinity(value) || value > int.MaxValue)
                return int.MaxValue;
            if (double.IsNegativeInfinity(value) || value < int.MinValue)
                return int.MinValue;
            if (value < 0.0)
                return (int)Math.Ceiling(value * Scale());
            return (int)(value * Scale() + 0.5);
        }

        public static double DipToPxD(this double value)
        {
            if (double.IsNaN(value))
                return 0;
            if (double.IsPositiveInfinity(value) || value > int.MaxValue)
                return int.MaxValue;
            if (double.IsNegativeInfinity(value) || value < int.MinValue)
                return int.MinValue;
            return value * Scale();
        }

        public static double PxToDip(this double px)
            => px / Scale();

        public static double PxToDip(this int px)
            => px / Scale();

        public static PointI DipToPx(this Point pt)
            => new PointI(DipToPx(pt.X), DipToPx(pt.Y));

        public static Point PxToDip(this PointI pt)
            => Point.Divide(pt, Scale());

        public static SizeI DipToPx(this Size sz)
            => new SizeI(DipToPx(sz.Width), DipToPx(sz.Height));

        public static Size PxToDip(this SizeI sz)
            => sz / Scale();

        public static RectI DipToPx(this Rect r)
            => new RectI(DipToPx(r.X), DipToPx(r.Y), DipToPx(r.Width), DipToPx(r.Height));

        public static Rect PxToDip(this RectI r)
            => r / Scale();

        public static ThicknessI DipToPx(this Thickness t)
            => new ThicknessI(DipToPx(t.Left), DipToPx(t.Top), DipToPx(t.Right), DipToPx(t.Bottom));

        public static Thickness PxToDip(this ThicknessI t)
            => t / Scale();




        public static partial int StatusBarHeight();
    }
}
