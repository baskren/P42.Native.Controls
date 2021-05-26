using System;

using ElementType = P42.Native.Controls.ILabel;

#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    public static class LabelMarkupExtensions
    {

        public static ElementType FontFamily<ElementType>(this ElementType element, string value) 
        { ((ILabel)element).FontFamily = value; return element; }

        public static ElementType FontSize<ElementType>(this ElementType element, double value)
        { ((ILabel)element).FontSize = value; return element; }

        public static ElementType FontStyle<ElementType>(this ElementType element, FontStyle value)
        { ((ILabel)element).FontStyle = value; return element; }

        public static ElementType TextColor<ElementType>(this ElementType element, Color value)
        { ((ILabel)element).TextColor = value; return element; }

        public static ElementType TextType<ElementType>(this ElementType element, TextType value)
        { ((ILabel)element).TextType = value; return element; }

        public static ElementType Text<ElementType>(this ElementType element, string value)
        { ((ILabel)element).Text = value; return element; }

        public static ElementType LineBreakMode<ElementType>(this ElementType element, LineBreakMode value)
        { ((ILabel)element).LineBreakMode = value; return element; }

        public static ElementType HorizontalTextAlignment<ElementType>(this ElementType element, Alignment value)
        { ((ILabel)element).HorizontalTextAlignment = value; return element; }

        public static ElementType VerticalTextAlignment<ElementType>(this ElementType element, Alignment value)
        { ((ILabel)element).VerticalTextAlignment = value; return element; }


    }
}
