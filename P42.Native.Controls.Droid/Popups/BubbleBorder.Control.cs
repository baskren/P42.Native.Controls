using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;

namespace P42.Native.Controls.Droid
{
    public partial class BubbleBorder
    {
        #region Control
        Thickness b_Padding = DisplayExtensions.DipToPx(10);
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

        double b_BorderWidth = DisplayExtensions.DipToPx(1);
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

        Color b_BorderColor = Color.Black;
        public Color BorderColor
        {
            get => b_BorderColor;
            set => SetRedrawField(ref b_BorderColor, value);
        }

        double b_cornerRadius = DisplayExtensions.DipToPx(5);
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

        Color b_BackgroundColor = Color.Gray.WithAlpha(0.5);
        public Color BackgroundColor
        {
            get => b_BackgroundColor;
            set => SetRedrawField(ref b_BackgroundColor, value);
        }
        #endregion

    }
}
