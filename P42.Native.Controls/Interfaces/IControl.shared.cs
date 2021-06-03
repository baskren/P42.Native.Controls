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
        public ThicknessI NtvPadding { get; set; }
        public Thickness DipPadding { get; set; }

        public double NtvBorderWidth { get; set; }
        public double DipBorderWidth { get; set; }

        public double NtvCornerRadius { get; set; }
        public double DipCornerRadius { get; set; }

        public Color DipBorderColor { get; set; }

        public Color DipBackgroundColor  { get; set; }


        #endregion



    }
}
