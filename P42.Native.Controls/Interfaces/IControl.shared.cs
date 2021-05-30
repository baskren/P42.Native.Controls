using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json;
#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    public interface IControl : IElement
    {
        #region Control
        public ThicknessI Padding { get; set; }
        public Thickness DipPadding { get; set; }

        public double BorderWidth { get; set; }
        public double DipBorderWidth { get; set; }

        public double CornerRadius { get; set; }
        public double DipCornerRadius { get; set; }

        public Color BorderColor { get; set; }

        public Color BackgroundColor  { get; set; }


        #endregion



    }
}
