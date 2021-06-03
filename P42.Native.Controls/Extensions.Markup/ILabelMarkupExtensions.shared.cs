using System;

#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    public static class LabelMarkupExtensions
    {

        public static ElementType FontFamily<ElementType>(this ElementType element, string value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).DipFontFamily = value; return element; }

        public static ElementType FontSize<ElementType>(this ElementType element, double value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).DipFontSize = value; return element; }

        public static ElementType FontStyle<ElementType>(this ElementType element, FontStyle value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).DipFontStyle = value; return element; }

        public static ElementType TextColor<ElementType>(this ElementType element, Color value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).DipTextColor = value; return element; }

        public static ElementType TextType<ElementType>(this ElementType element, TextType value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).DipTextType = value; return element; }

        public static ElementType Text<ElementType>(this ElementType element, string value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).DipText = value; return element; }

        public static ElementType LineBreakMode<ElementType>(this ElementType element, LineBreakMode value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).DipLineBreakMode = value; return element; }

        public static ElementType HorizontalTextAlignment<ElementType>(this ElementType element, Alignment value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).DipHorizontalTextAlignment = value; return element; }

        public static ElementType VerticalTextAlignment<ElementType>(this ElementType element, Alignment value) where ElementType : Android.Views.View, P42.Native.Controls.ILabel
        { ((ILabel)element).DipVerticalTextAlignment = value; return element; }


    }
}
