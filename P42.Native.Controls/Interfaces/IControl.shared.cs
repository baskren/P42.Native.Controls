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
    public interface IControl : IFrameworkElement
    {
        #region Control
        ThicknessI Padding { get; set; }
        public virtual Thickness DipPadding
        {
            get => DisplayExtensions.PxToDip(Padding);
            set => Padding = DisplayExtensions.DipToPx(value);
        }

        double BorderWidth { get; set; }
        public virtual double DipBorderWidth
        {
            get => DisplayExtensions.PxToDip(BorderWidth);
            set => BorderWidth = DisplayExtensions.DipToPx(value);
        }

        double CornerRadius { get; set; }
        public virtual double DipCornerRadius
        {
            get => DisplayExtensions.PxToDip(CornerRadius);
            set => CornerRadius = DisplayExtensions.DipToPx(value);
        }

        Color BorderColor { get; set; }

        Color BackgroundColor  { get; set; }


        #endregion



    }
}
