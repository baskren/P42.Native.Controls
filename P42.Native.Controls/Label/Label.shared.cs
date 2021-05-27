using System;
using SmartTraitsDefs;

#if __ANDROID__
using Color = Android.Graphics.Color;
#endif

namespace P42.Native.Controls
{
    [AddTrait(typeof(TLabel))]
    [AddTrait(typeof(TElement))]
    [AddTrait(typeof(TNotifiable))]
    public partial class Label
    {

    }
}