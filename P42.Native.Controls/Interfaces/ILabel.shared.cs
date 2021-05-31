using System;

#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    public interface ILabel : IElement
    {
        #region Properties

        public string FontFamily { get; set; }

        public double FontSize  { get; set; }

        public FontStyle FontStyle { get; set; }

        public Color TextColor { get; set; }

        public TextType TextType { get; set; }

        public string Text { get; set; }

        public LineBreakMode LineBreakMode { get; set; }

        public Alignment HorizontalTextAlignment { get; set; }

        public Alignment VerticalTextAlignment { get; set; }

        #endregion
    }
}