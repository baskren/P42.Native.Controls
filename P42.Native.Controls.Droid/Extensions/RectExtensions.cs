using System;
using Android.Graphics;

namespace P42.Native.Controls.Droid
{
    public static class RectExtensions
    {
        public static RectF AsRectF(this Rect r)
            => new RectF((float)r.Left, (float)r.Top, (float)r.Right, (float)r.Bottom);
    }
}
