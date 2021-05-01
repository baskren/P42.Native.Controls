using System;
using Android.Graphics;

namespace P42.Native.Controls.Droid
{
    internal static class PathExtensions
    {
        public static void ArcWithCenterTo(this Path path, float x, float y, float radius, float startDegrees, float sweepDegrees)
        {
            var rect = new RectF(x - radius, y - radius, x + radius, y + radius);
            path.ArcTo(rect, startDegrees, sweepDegrees, false);
        }
    }
}
