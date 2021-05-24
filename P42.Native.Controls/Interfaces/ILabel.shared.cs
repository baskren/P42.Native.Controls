using System;

#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    public interface ILabel : IElement
    {
        #region Properties

        string FontFamily { get; set; }

        double FontSize  { get; set; }

        FontStyle FontStyle { get; set; }

        Color TextColor { get; set; }

        TextType TextType { get; set; }

        string Text { get; set; }

        LineBreakMode LineBreakMode { get; set; }

        Alignment HorizontalTextAlignment { get; set; }

        Alignment VerticalTextAlignment { get; set; }

        #endregion
    }
}