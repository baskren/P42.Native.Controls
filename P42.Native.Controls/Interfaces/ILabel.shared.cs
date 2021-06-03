using System;

#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    public interface ILabel : IElement
    {
        #region Properties

        public string DipFontFamily { get; set; }

        public double DipFontSize  { get; set; }

        public FontStyle DipFontStyle { get; set; }

        public Color DipTextColor { get; set; }

        public TextType DipTextType { get; set; }

        public string DipText { get; set; }

        public LineBreakMode DipLineBreakMode { get; set; }

        public Alignment DipHorizontalTextAlignment { get; set; }

        public Alignment DipVerticalTextAlignment { get; set; }

        #endregion
    }
}