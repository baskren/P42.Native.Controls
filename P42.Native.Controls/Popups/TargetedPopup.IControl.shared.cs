using System;
using Android.Graphics;

namespace P42.Native.Controls
{
    public partial class TargetedPopup : IControl
    {
        #region Control
        ThicknessI b_Padding = BubbleBorder.DefaultPadding;
        public ThicknessI Padding
        {
            get => b_Padding;
            set => ((INotifiable)this).SetField(ref b_Padding, value);
        }

        double b_BorderWidth = BubbleBorder.DefaultBorderWidth;
        public double BorderWidth
        {
            get => b_BorderWidth;
            set => ((INotifiable)this).SetField(ref b_BorderWidth, value);
        }

        Color b_BorderColor = BubbleBorder.DefaultBorderColor;
        public Color BorderColor
        {
            get => b_BorderColor;
            set => ((INotifiable)this).SetField(ref b_BorderColor, value);
        }

        double b_cornerRadius = BubbleBorder.DefaultCornerRadius;
        public double CornerRadius
        {
            get => b_cornerRadius;
            set => ((INotifiable)this).SetField(ref b_cornerRadius, value);
        }

        Color b_BackgroundColor = BubbleBorder.DefaultBackgroundColor;
        public Color BackgroundColor
        {
            get => b_BackgroundColor;
            set => ((INotifiable)this).SetField(ref b_BackgroundColor, value);
        }
        #endregion


        public Alignment HorizontalContentAlignment
        {
            get => m_Border.HorizontalContentAlignment;
            set => m_Border.HorizontalContentAlignment = value;
        }

        public Alignment VerticalContentAlignment
        {
            get => m_Border.VerticalContentAlignment;
            set => m_Border.VerticalContentAlignment = value;
        }



    }
}
