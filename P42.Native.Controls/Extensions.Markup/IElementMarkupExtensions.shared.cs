using System;

//using ElementType = P42.Native.Controls.IElement;

namespace P42.Native.Controls
{

    public static class IElementMarkupExtensions
    {

        public static ElementType DipRequestedWidth<ElementType>(this ElementType element, double value) where ElementType : Android.Views.View, P42.Native.Controls.IElement
        { ((IElement)element).DipRequestedWidth = value; return element; }

        public static ElementType DipRequestedHeight<ElementType>(this ElementType element, double value) where ElementType : Android.Views.View, P42.Native.Controls.IElement
        { ((IElement)element).DipRequestedHeight = value; return element; }

        public static ElementType HorizontalAlighment<ElementType>(this ElementType element, Alignment value) where ElementType : Android.Views.View, P42.Native.Controls.IElement
        { ((IElement)element).HorizontalAlignment = value; return element; }

        public static ElementType VerticalAlignment<ElementType>(this ElementType element, Alignment value) where ElementType : Android.Views.View, P42.Native.Controls.IElement
        { ((IElement)element).VerticalAlignment = value; return element; }

        public static ElementType DipMargin<ElementType>(this ElementType element, Thickness value) where ElementType : Android.Views.View, P42.Native.Controls.IElement
        { ((IElement)element).DipMargin = value; return element; }

        public static ElementType DipMinWidth<ElementType>(this ElementType element, double value) where ElementType : Android.Views.View, P42.Native.Controls.IElement
        { ((IElement)element).DipMinWidth = value; return element; }

        public static ElementType DipMinHeight<ElementType>(this ElementType element, double value) where ElementType : Android.Views.View, P42.Native.Controls.IElement
        { ((IElement)element).DipMinHeight = value; return element; }

        public static ElementType DipMaxWidth<ElementType>(this ElementType element, double value) where ElementType : Android.Views.View, P42.Native.Controls.IElement
        { ((IElement)element).DipMaxWidth = value; return element; }

        public static ElementType DipMaxHeight<ElementType>(this ElementType element, double value) where ElementType : Android.Views.View, P42.Native.Controls.IElement
        { ((IElement)element).DipMaxHeight = value; return element; }

        public static ElementType Opacity<ElementType>(this ElementType element, double value) where ElementType : Android.Views.View, P42.Native.Controls.IElement
        { ((IElement)element).Opacity = value; return element; }

        public static ElementType IsVisible<ElementType>(this ElementType element, bool value) where ElementType : Android.Views.View, P42.Native.Controls.IElement
        { ((IElement)element).IsVisible = value; return element; }


    }

}