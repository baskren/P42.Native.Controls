
using System;
using SmartTraitsDefs;

#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    [SimpleTrait]
    partial class TControl : TElement, IControl
    {

        #region Defaults
        public static ThicknessI NtvDefaultPadding = (ThicknessI)DisplayExtensions.DipToPx(10);
        public static double NtvDefaultBorderWidth = DisplayExtensions.DipToPxD(1);
        public static Color NtvDefaultBorderColor = Color.Black;
        public static double NtvDefaultCornerRadius = DisplayExtensions.DipToPxD(5);
        public static Color NtvDefaultBackgroundColor = Color.Gray;
        #endregion

        
        #region Properties
        internal protected ThicknessI b_NtvPadding = NtvDefaultPadding;
        public virtual ThicknessI NtvPadding
        {
            get => b_NtvPadding;
            set => SetField(ref b_NtvPadding, value, NtvUpdatePadding);
        }
        public virtual Thickness DipPadding
        {
            get => DisplayExtensions.PxToDip(NtvPadding);
            set => NtvPadding = value.DipToPx();
        }


        internal protected double b_NtvBorderWidth = NtvDefaultBorderWidth;
        public virtual double NtvBorderWidth
        {
            get => b_NtvBorderWidth;
            set => SetLayoutField(ref b_NtvBorderWidth, value);
        }
        public virtual double DipBorderWidth
        {
            get => DisplayExtensions.PxToDip(NtvBorderWidth);
            set => NtvBorderWidth = value.DipToPxD();
        }



        internal protected Color b_DipBorderColor = NtvDefaultBackgroundColor;
        public virtual Color DipBorderColor
        {
            get => b_DipBorderColor;
            set => SetRedrawField(ref b_DipBorderColor, value);
        }

        internal protected double b_NtvCornerRadius = NtvDefaultCornerRadius;
        public virtual double NtvCornerRadius
        {
            get => b_NtvCornerRadius;
            set => SetRedrawField(ref b_NtvCornerRadius, value);
        }
        public virtual double DipCornerRadius
        {
            get => DisplayExtensions.PxToDip(NtvCornerRadius);
            set => NtvCornerRadius = value.DipToPxD();
        }


        internal protected Color b_DipBackgroundColor = NtvDefaultBackgroundColor;
        public virtual Color DipBackgroundColor
        {
            get => b_DipBackgroundColor;
            set => SetRedrawField(ref b_DipBackgroundColor, value);
        }
        #endregion
        
        
        #region Methods

#if __ANDROID__

        void NtvUpdatePadding()
        {
            NtvBaseView.SetPadding((int)(b_NtvPadding.Left + 0.5), (int)(b_NtvPadding.Top + 0.5), (int)(b_NtvPadding.Right + 0.5), (int)(b_NtvPadding.Bottom + 0.5));
        }

#else

        void NtvUpdatePadding() {}

#endif

        #endregion
        
    }
}

