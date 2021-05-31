using System;
using System.Threading.Tasks;
using SmartTraitsDefs;

#if __ANDROID__
using Color = Android.Graphics.Color;
using Element = Android.Views.View;
#endif

namespace P42.Native.Controls
{
    [AddSimpleTrait(typeof(TControl))]
    public partial class BubbleBorder  
    {
        #region Properties

        Element b_Content;
        public Element Content
        {
            get => b_Content;
            set => SetField(ref b_Content, value);
        }

        #region Pointer
        /*
        double b_PointerBias;
        public double PointerBias
        {
            get => b_PointerBias;
            set => SetRedrawField(ref b_PointerBias, value);
        }
        */

        public static int DefaultPointerLength = DisplayExtensions.DipToPx(10);
        int b_PointerLength = DefaultPointerLength;
        public int PointerLength
        {
            get => b_PointerLength;
            set => SetLayoutField(ref b_PointerLength, value);
        }

        public double DipPointerLength
        {
            get => DisplayExtensions.PxToDip(b_PointerLength);
            set => PointerLength = DisplayExtensions.DipToPx(value);
        }

        public static double DefaultPointerTipRadius = DisplayExtensions.DipToPx(2);
        double b_PointerTipRadius = DefaultPointerTipRadius;
        public double PointerTipRadius
        {
            get => b_PointerTipRadius;
            set => SetRedrawField(ref b_PointerTipRadius, value);
        }

        public double DipPointerTipRadius
        {
            get => DisplayExtensions.PxToDip(b_PointerTipRadius);
            set => PointerTipRadius = DisplayExtensions.DipToPx(value);
        }

        public static double DefaultPointerAxialPosition = 0.5; 
        double b_PointerAxialPosition = DefaultPointerAxialPosition;
        public double PointerAxialPosition
        {
            get => b_PointerAxialPosition;
            set => SetRedrawField(ref b_PointerAxialPosition, value);
        }

        public double DipPointerAxialPosition
        {
            get => DisplayExtensions.PxToDip(b_PointerAxialPosition);
            set
            {
                if (value <= 1)
                    PointerAxialPosition = value;
                else
                    PointerAxialPosition = DisplayExtensions.DipToPx(value);
            }
        }

        public static PointerDirection DefaultPointerDirection = PointerDirection.Any;
        PointerDirection b_PointerDirection = DefaultPointerDirection;
        public PointerDirection PointerDirection
        {
            get => b_PointerDirection;
            set
            {
                if (PointerDirection.IsHorizontal() == value.IsHorizontal())
                    SetRedrawField(ref b_PointerDirection, value);
                else
                    SetLayoutField(ref b_PointerDirection, value);
            }
        }

        public static double DefaultPointerCornerRadius = DisplayExtensions.DipToPx(2);
        double b_PointerCornerRadius = DefaultPointerCornerRadius;
        public double PointerCornerRadius
        {
            get => b_PointerCornerRadius;
            set => SetField(ref b_PointerCornerRadius, value);
        }

        public double DipPointerCornerRadius
        {
            get => DisplayExtensions.PxToDip(b_PointerCornerRadius);
            set => PointerCornerRadius = DisplayExtensions.DipToPx(value);
        }
        #endregion

        #endregion


    }
}
