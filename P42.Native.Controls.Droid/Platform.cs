using System;
namespace P42.Native.Controls.Droid
{
    public static class Platform
    {
        public static void Init(Android.Content.Context context)
        {
            P42.Utils.Droid.Settings.Init(context);
        }
    }
}
