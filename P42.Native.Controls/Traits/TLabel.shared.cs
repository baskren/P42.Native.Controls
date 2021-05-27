﻿using System;
using SmartTraitsDefs;
#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    [Trait]
    abstract partial class TLabel : TElement, ILabel
    {

        string b_FontFamily;
        public string FontFamily
        {
            get => b_FontFamily;
            set => SetField(ref b_FontFamily, value);
        }

        double b_FontSize;
        public double FontSize
        {
            get => b_FontSize;
            set => SetField(ref b_FontSize, value);
        }

        FontStyle b_FontStyle;
        public FontStyle FontStyle
        {
            get => b_FontStyle;
            set => SetField(ref b_FontStyle, value);
        }


        Color b_TextColor;
        public Color TextColor
        {
            get => b_TextColor;
            set => SetField(ref b_TextColor, value);
        }

        TextType b_TextType;
        public TextType TextType
        {
            get => b_TextType;
            set => SetField(ref b_TextType, value);
        }

        string b_Text;
        public new string Text
        {
            get => b_Text;
            set => SetField(ref b_Text, value);
        }


        LineBreakMode b_LineBreakMode;
        public LineBreakMode LineBreakMode
        {
            get => b_LineBreakMode;
            set => SetField(ref b_LineBreakMode, value);
        }

        Alignment b_HorizontalTextAlignment;
        public Alignment HorizontalTextAlignment
        {
            get => b_HorizontalTextAlignment;
            set => SetField(ref b_HorizontalTextAlignment, value);
        }

        Alignment b_VerticalTextAlignment;
        public Alignment VerticalTextAlignment
        {
            get => b_VerticalTextAlignment;
            set => SetField(ref b_VerticalTextAlignment, value);
        }

        /* will implement this and Lines when it's time to start working at STRETCH
        double b_MinFontSize = -1;
        public double MinFontSize
        {
            get => b_MinFontSize;
            set => SetField(ref b_MinFontSize, value);
        }
        */

    }
}