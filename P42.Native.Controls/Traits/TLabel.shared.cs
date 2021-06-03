using System;
using SmartTraitsDefs;

#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    [SimpleTrait]
    partial class TLabel : TElement, ILabel
    {
    
        #region Properties
        string b_DipFontFamily;
        public string DipFontFamily
        {
            get => b_DipFontFamily;
            set => SetField(ref b_DipFontFamily, value);
        }

        double b_DipFontSize;
        public double DipFontSize
        {
            get => b_DipFontSize;
            set => SetField(ref b_DipFontSize, value);
        }

        FontStyle b_DipFontStyle;
        public FontStyle DipFontStyle
        {
            get => b_DipFontStyle;
            set => SetField(ref b_DipFontStyle, value);
        }


        Color b_DipTextColor;
        public Color DipTextColor
        {
            get => b_DipTextColor;
            set => SetField(ref b_DipTextColor, value);
        }

        TextType b_DipTextType;
        public TextType DipTextType
        {
            get => b_DipTextType;
            set => SetField(ref b_DipTextType, value);
        }

        string b_DipText;
        public string DipText
        {
            get => b_DipText;
            set => SetField(ref b_DipText, value);
        }


        LineBreakMode b_DipLineBreakMode;
        public LineBreakMode DipLineBreakMode
        {
            get => b_DipLineBreakMode;
            set => SetField(ref b_DipLineBreakMode, value);
        }

        Alignment b_DipHorizontalTextAlignment;
        public Alignment DipHorizontalTextAlignment
        {
            get => b_DipHorizontalTextAlignment;
            set => SetField(ref b_DipHorizontalTextAlignment, value);
        }

        Alignment b_DipVerticalTextAlignment;
        public Alignment DipVerticalTextAlignment
        {
            get => b_DipVerticalTextAlignment;
            set => SetField(ref b_DipVerticalTextAlignment, value);
        }

        /* will implement this and Lines when it's time to start working at STRETCH
        double b_MinFontSize = -1;
        public double MinFontSize
        {
            get => b_MinFontSize;
            set => SetField(ref b_MinFontSize, value);
        }
        */
        #endregion
    }
}

