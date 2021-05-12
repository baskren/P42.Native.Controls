using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;

namespace P42.Native.Controls.Droid
{
    public partial class BubbleBorder
    {
        #region Control
        public static Thickness DefaultPadding = DisplayExtensions.DipToPx(10);
        Thickness b_Padding = DefaultPadding;
        public Thickness Padding
        {
            get => b_Padding;
            set => SetLayoutField(ref b_Padding, value);
        }

        public Thickness DipPadding
        {
            get => DisplayExtensions.PxToDip(b_Padding);
            set => Padding = DisplayExtensions.DipToPx(value);
        }

        public static double DefaultBorderWidth = DisplayExtensions.DipToPx(1);
        double b_BorderWidth = DefaultBorderWidth;
        public double BorderWidth
        {
            get => b_BorderWidth;
            set => SetLayoutField(ref b_BorderWidth, value);
        }

        public double DipBorderWidth
        {
            get => DisplayExtensions.PxToDip(b_BorderWidth);
            set => BorderWidth = DisplayExtensions.DipToPx(value);
        }

        public static Color DefaultBorderColor = Color.Black;
        Color b_BorderColor = DefaultBorderColor;
        public Color BorderColor
        {
            get => b_BorderColor;
            set => SetRedrawField(ref b_BorderColor, value);
        }

        public static double DefaultCornerRadius = DisplayExtensions.DipToPx(5);
        double b_cornerRadius = DefaultCornerRadius;
        public double CornerRadius
        {
            get => b_cornerRadius;
            set => SetRedrawField(ref b_cornerRadius, value);
        }

        public double DipCornerRadius
        {
            get => DisplayExtensions.PxToDip(b_cornerRadius);
            set => CornerRadius = DisplayExtensions.DipToPx(value);
        }

        public static Color DefaultBackgroundColor = Color.Gray.WithAlpha(0.5);
        Color b_BackgroundColor = DefaultBorderColor;
        public Color BackgroundColor
        {
            get => b_BackgroundColor;
            set => SetRedrawField(ref b_BackgroundColor, value);
        }
        #endregion


        Alignment b_HorizontalContentAlignment = Alignment.Center;
        public Alignment HorizontalContentAlignment
        {
            get => b_HorizontalContentAlignment;
            set => SetField(ref b_HorizontalContentAlignment, value);
        }

        Alignment b_VerticalContentAlignment = Alignment.Center;
        public Alignment VerticalContentAlignment
        {
            get => b_VerticalContentAlignment;
            set => SetField(ref b_VerticalContentAlignment, value);
        }


    }
}
