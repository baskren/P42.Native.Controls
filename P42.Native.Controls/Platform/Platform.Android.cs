using System;

namespace P42.Native.Controls.Droid
{
    public static partial class Platform
    {
        public static partial void Init(Android.Content.Context context)
        {
            P42.Utils.Droid.Settings.Init(context);
        }
    }
}
