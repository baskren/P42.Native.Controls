using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P42.Native.Controls
{
    struct DirectionStats
    {
        public Size FreeSpace;
        public double MinFree => Math.Min(FreeSpace.Width, FreeSpace.Height);
        public double MaxFree => Math.Max(FreeSpace.Width, FreeSpace.Height);

        public double MinAvail => Math.Min(FreeSpace.Width + BorderSize.Width, FreeSpace.Height + BorderSize.Height);
        public double MaxAvail => Math.Max(FreeSpace.Width + BorderSize.Width, FreeSpace.Height + BorderSize.Height);

        public PointerDirection PointerDirection;
        public Size BorderSize;

        public override string ToString()
        {
            return "{PointerDirection=" + PointerDirection + ", BorderSize=" + BorderSize + ", FreeSpace=" + FreeSpace + "}";
        }
    }
}
