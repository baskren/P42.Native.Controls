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

        Element b_DipContent;
        public Element DipContent
        {
            get => b_DipContent;
            set => SetField(ref b_DipContent, value);
        }

        int b_NtvShadowRadius = (10.0).DipToPx();
        public int NtvShadowRadius
        {
            get => b_NtvShadowRadius;
            set => SetField(ref b_NtvShadowRadius, value);
        }
        public double DipShadowRadius
        {
            get => NtvShadowRadius.PxToDip();
            set => NtvShadowRadius = value.DipToPx();
        }

        PointI b_NtvShadowShift = new Point(2,2).DipToPx();
        public PointI NtvShadowShift
        {
            get => b_NtvShadowShift;
            set => SetField(ref b_NtvShadowShift, value);
        }

        public Point DipShadowShift
        {
            get => b_NtvShadowShift.PxToDip();
            set => b_NtvShadowShift = value.DipToPx();
        }

        bool b_IsShadow;
        public bool DipHasShadow
        {
            get => b_IsShadow;
            set => SetField(ref b_IsShadow, value);
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

        public static int NtvDefaultPointerLength = DisplayExtensions.DipToPx(10);
        int b_NtvPointerLength = NtvDefaultPointerLength;
        public int NtvPointerLength
        {
            get => b_NtvPointerLength;
            set => SetLayoutField(ref b_NtvPointerLength, value);
        }

        public double DipPointerLength
        {
            get => DisplayExtensions.PxToDip(b_NtvPointerLength);
            set => NtvPointerLength = DisplayExtensions.DipToPx(value);
        }

        public static double NtvDefaultPointerTipRadius = DisplayExtensions.DipToPx(2);
        double b_NtvPointerTipRadius = NtvDefaultPointerTipRadius;
        public double NtvPointerTipRadius
        {
            get => b_NtvPointerTipRadius;
            set => SetRedrawField(ref b_NtvPointerTipRadius, value);
        }

        public double DipPointerTipRadius
        {
            get => DisplayExtensions.PxToDip(b_NtvPointerTipRadius);
            set => NtvPointerTipRadius = DisplayExtensions.DipToPx(value);
        }

        public static double NtvDefaultPointerAxialPosition = 0.5; 
        double b_NtvPointerAxialPosition = NtvDefaultPointerAxialPosition;
        public double NtvPointerAxialPosition
        {
            get => b_NtvPointerAxialPosition;
            set => SetRedrawField(ref b_NtvPointerAxialPosition, value);
        }

        public double DipPointerAxialPosition
        {
            get => DisplayExtensions.PxToDip(b_NtvPointerAxialPosition);
            set
            {
                if (value <= 1)
                    NtvPointerAxialPosition = value;
                else
                    NtvPointerAxialPosition = DisplayExtensions.DipToPx(value);
            }
        }

        public static PointerDirection DipDefaultPointerDirection = PointerDirection.Any;
        PointerDirection b_DipPointerDirection = DipDefaultPointerDirection;
        public PointerDirection DipPointerDirection
        {
            get => b_DipPointerDirection;
            set
            {
                if (DipPointerDirection.IsHorizontal() == value.IsHorizontal())
                    SetRedrawField(ref b_DipPointerDirection, value);
                else
                    SetLayoutField(ref b_DipPointerDirection, value);
            }
        }

        public static double NtvDefaultPointerCornerRadius = DisplayExtensions.DipToPx(2);
        double b_NtvPointerCornerRadius = NtvDefaultPointerCornerRadius;
        public double NtvPointerCornerRadius
        {
            get => b_NtvPointerCornerRadius;
            set => SetField(ref b_NtvPointerCornerRadius, value);
        }

        public double DipPointerCornerRadius
        {
            get => DisplayExtensions.PxToDip(b_NtvPointerCornerRadius);
            set => NtvPointerCornerRadius = DisplayExtensions.DipToPx(value);
        }
        #endregion

        #endregion


    }
}
