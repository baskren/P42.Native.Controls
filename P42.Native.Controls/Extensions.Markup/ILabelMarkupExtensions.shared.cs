using System;

#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    public static class LabelMarkupExtensions
    {

        public static ElementType FontFamily<ElementType>(this ElementType element, string value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).FontFamily = value; return element; }

        public static ElementType FontSize<ElementType>(this ElementType element, double value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).FontSize = value; return element; }

        public static ElementType FontStyle<ElementType>(this ElementType element, FontStyle value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).FontStyle = value; return element; }

        public static ElementType TextColor<ElementType>(this ElementType element, Color value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).TextColor = value; return element; }

        public static ElementType TextType<ElementType>(this ElementType element, TextType value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).TextType = value; return element; }

        public static ElementType Text<ElementType>(this ElementType element, string value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).Text = value; return element; }

        public static ElementType LineBreakMode<ElementType>(this ElementType element, LineBreakMode value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).LineBreakMode = value; return element; }

        public static ElementType HorizontalTextAlignment<ElementType>(this ElementType element, Alignment value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).HorizontalTextAlignment = value; return element; }

        public static ElementType VerticalTextAlignment<ElementType>(this ElementType element, Alignment value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).VerticalTextAlignment = value; return element; }


    }
}
