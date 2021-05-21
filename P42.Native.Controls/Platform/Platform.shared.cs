using System;

#if __ANDROID__
using AppContext = Android.Content.Context;
#endif

namespace P42.Native.Controls.Droid
{
    public static partial class Platform
    {
        public static partial void Init(AppContext context);
    }
}
