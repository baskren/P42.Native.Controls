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




        public static partial int DipToPx(double dip);

        public static partial double DipToPxD(double dip);

        public static partial double PxToDip(double px);

        public static partial PointI DipToPx(Point pt);

        public static partial Point PxToDip(PointI pt);
            
        public static partial SizeI DipToPx(Size sz);

        public static partial Size PxToDip(SizeI sz);
            
        public static partial RectI DipToPx(Rect r);

        public static partial Rect PxToDip(RectI r);

        public static partial ThicknessI DipToPx(Thickness t);

        public static partial Thickness PxToDip(ThicknessI t);



        public static partial int StatusBarHeight();
    }
}
