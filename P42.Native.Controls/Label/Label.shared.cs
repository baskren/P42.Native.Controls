using System;

#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    public partial class Label : IFrameworkElement
    {
        #region Properties

        ThicknessI b_Padding;
        public ThicknessI Padding
        {
            get => b_Padding;
            set => ((INotifiable)this).SetField(ref b_Padding, value);
        }

        public Thickness DipPadding
        {
            get => Padding.PxToDip();
            set => Padding = value.DipToPx();
        }

        string b_FontFamily;
        public string FontFamily
        {
            get => b_FontFamily;
            set => ((INotifiable)this).SetField(ref b_FontFamily, value);
        }


        double b_FontSize;
        public double FontSize
        {
            get => b_FontSize;
            set => ((INotifiable)this).SetField(ref b_FontSize, value);
        }

        FontStyle b_FontStyle;
        public FontStyle FontStyle
        {
            get => b_FontStyle;
            set => ((INotifiable)this).SetField(ref b_FontStyle, value);
        }

        Color b_TextColor;
        public Color TextColor
        {
            get => b_TextColor;
            set => ((INotifiable)this).SetField(ref b_TextColor, value);
        }

        TextType b_TextType;
        public TextType TextType
        {
            get => b_TextType;
            set => ((INotifiable)this).SetField(ref b_TextType, value);
        }

        string b_Text;
        public new string Text
        {
            get => b_Text;
            set => ((INotifiable)this).SetField(ref b_Text, value);
        }

        LineBreadMode b_LineBreakMode;
        public LineBreadMode LineBreakMode
        {
            get => b_LineBreakMode;
            set => ((INotifiable)this).SetField(ref b_LineBreakMode, value);
        }

        Alignment b_HorizontalTextAlignment;
        public Alignment HorizontalTextAlignment
        {
            get => b_HorizontalTextAlignment;
            set => ((INotifiable)this).SetField(ref b_HorizontalTextAlignment, value);
        }

        Alignment b_VerticalTextAlignment;
        public Alignment VerticalTextAlignment
        {
            get => b_VerticalTextAlignment;
            set => ((INotifiable)this).SetField(ref b_VerticalTextAlignment, value);
        }

        double b_MinFontSize = -1;
        public double MinFontSize
        {
            get => b_MinFontSize;
            set => ((INotifiable)this).SetField(ref b_MinFontSize, value);
        }
        #endregion
    }
}