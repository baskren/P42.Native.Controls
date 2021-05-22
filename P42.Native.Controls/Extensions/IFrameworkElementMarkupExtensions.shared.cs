using System;

using ElementType = P42.Native.Controls.IFrameworkElement;

namespace P42.Native.Controls
{

    public static class IFrameworkElementMarkupExtensions
    {

        public static ElementType DipRequestedWidth<ElementType>(this ElementType element, double value)
        { ((IFrameworkElement)element).DipRequestedWidth = value; return element; }

        public static ElementType DipRequestedHeight<ElementType>(this ElementType element, double value)
        { ((IFrameworkElement)element).DipRequestedHeight = value; return element; }

        public static ElementType HorizontalAlighment<ElementType>(this ElementType element, Alignment value)
        { ((IFrameworkElement)element).HorizontalAlignment = value; return element; }

        public static ElementType VerticalAlignment<ElementType>(this ElementType element, Alignment value)
        { ((IFrameworkElement)element).VerticalAlignment = value; return element; }

        public static ElementType DipMargin<ElementType>(this ElementType element, Thickness value)
        { ((IFrameworkElement)element).DipMargin = value; return element; }

        public static ElementType DipMinWidth<ElementType>(this ElementType element, double value)
        { ((IFrameworkElement)element).DipMinWidth = value; return element; }

        public static ElementType DipMinHeight<ElementType>(this ElementType element, double value)
        { ((IFrameworkElement)element).DipMinHeight = value; return element; }

        public static ElementType DipMaxWidth<ElementType>(this ElementType element, double value)
        { ((IFrameworkElement)element).DipMaxWidth = value; return element; }

        public static ElementType DipMaxHeight<ElementType>(this ElementType element, double value)
        { ((IFrameworkElement)element).DipMaxHeight = value; return element; }


    }

}