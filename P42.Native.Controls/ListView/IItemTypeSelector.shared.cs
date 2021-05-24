
using System;
using System.Collections.Generic;

#if __ANDROID__
using UIElement = Android.Views.View;
#endif

namespace P42.Native.Controls
{
    public interface IItemTypeSelector : IDisposable
    {
        Type GetViewType(object item);

        IEnumerable<Type> ViewTypes { get; }
    }
}
