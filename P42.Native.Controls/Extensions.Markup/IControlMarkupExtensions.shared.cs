using System;

using ElementType = P42.Native.Controls.IControl;
#if __ANDROID__
using Color = Android.Graphics.Color;
#endif


namespace P42.Native.Controls
{
    public static class IControlMarkupExtensions
    {
        public static ElementType DipPadding<ElementType>(this ElementType element, Thickness value)
        { ((IControl)element).DipPadding = value; return element; }

        public static ElementType DipBorderWidth<ElementType>(this ElementType element, double value)
        { ((IControl)element).DipBorderWidth = value; return element; }

        public static ElementType DipCornerRadius<ElementType>(this ElementType element, double value)
        { ((IControl)element).DipCornerRadius = value; return element; }

        public static ElementType BorderColor<ElementType>(this ElementType element, Color value)
        { ((IControl)element).BorderColor = value; return element; }

        public static ElementType BackgroundColor<ElementType>(this ElementType element, Color value)
        { ((IControl)element).BackgroundColor = value; return element; }


    }
}