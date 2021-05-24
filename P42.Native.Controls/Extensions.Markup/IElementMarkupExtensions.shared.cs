using System;

using ElementType = P42.Native.Controls.IElement;

namespace P42.Native.Controls
{

    public static class IElementMarkupExtensions
    {

        public static ElementType DipRequestedWidth<ElementType>(this ElementType element, double value)
        { ((IElement)element).DipRequestedWidth = value; return element; }

        public static ElementType DipRequestedHeight<ElementType>(this ElementType element, double value)
        { ((IElement)element).DipRequestedHeight = value; return element; }

        public static ElementType HorizontalAlighment<ElementType>(this ElementType element, Alignment value)
        { ((IElement)element).HorizontalAlignment = value; return element; }

        public static ElementType VerticalAlignment<ElementType>(this ElementType element, Alignment value)
        { ((IElement)element).VerticalAlignment = value; return element; }

        public static ElementType DipMargin<ElementType>(this ElementType element, Thickness value)
        { ((IElement)element).DipMargin = value; return element; }

        public static ElementType DipMinWidth<ElementType>(this ElementType element, double value)
        { ((IElement)element).DipMinWidth = value; return element; }

        public static ElementType DipMinHeight<ElementType>(this ElementType element, double value)
        { ((IElement)element).DipMinHeight = value; return element; }

        public static ElementType DipMaxWidth<ElementType>(this ElementType element, double value)
        { ((IElement)element).DipMaxWidth = value; return element; }

        public static ElementType DipMaxHeight<ElementType>(this ElementType element, double value)
        { ((IElement)element).DipMaxHeight = value; return element; }

        public static ElementType Opacity<ElementType>(this ElementType element, double value) 
        { ((IElement)element).Opacity = value; return element; }

        public static ElementType IsVisible<ElementType>(this ElementType element, bool value) 
        { ((IElement)element).IsVisible = value; return element; }


    }

}