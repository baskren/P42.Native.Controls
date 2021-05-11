using System;
using Android.Graphics;

namespace P42.Native.Controls.Droid
{
    public partial class TargetedPopup
    {
        #region Control
        Thickness b_Padding = DisplayExtensions.DipToPx(10);
        public Thickness Padding
        {
            get => b_Padding;
            set => SetField(ref b_Padding, value, ()=> { m_Border.Padding = Padding; });
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
            set => SetField(ref b_BorderWidth, value, () => { m_Border.BorderWidth = BorderWidth; });
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
            set => SetField(ref b_BorderColor, value, () => { m_Border.BorderColor = BorderColor; });
        }

        double b_cornerRadius = DisplayExtensions.DipToPx(5);
        public double CornerRadius
        {
            get => b_cornerRadius;
            set => SetField(ref b_cornerRadius, value, ()=> { m_Border.CornerRadius = CornerRadius; });
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
            set => SetField(ref b_BackgroundColor, value, ()=> { m_Border.BackgroundColor = BackgroundColor; });
        }
        #endregion
    }
}
