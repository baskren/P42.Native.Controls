
using System;
using System.Collections.Generic;

#if __ANDROID__
using UIElement = Android.Views.View;
#endif

namespace P42.Native.Controls
{
    public interface IItemViewFactory : IDisposable
    {
        UIElement GetUIElement(object item);

        Type GetViewType(object item);
        //void RecycleElement(Element element); // may have to implement when we get to iOS?

        IEnumerable<Type> ViewTypes { get; }
    }
}
