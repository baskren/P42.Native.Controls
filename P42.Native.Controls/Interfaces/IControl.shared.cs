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
    [SmartTraitsDefs.TraitInterface]
    public interface IControl : IElement
    {
        #region Control
        ThicknessI Padding { get; set; }

        double BorderWidth { get; set; }

        double CornerRadius { get; set; }

        Color BorderColor { get; set; }

        Color BackgroundColor  { get; set; }


        #endregion



    }
}
