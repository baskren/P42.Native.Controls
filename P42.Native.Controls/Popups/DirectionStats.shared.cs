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
        public SizeI FreeSpace;
        public int MinFree => Math.Min(FreeSpace.Width, FreeSpace.Height);
        public int MaxFree => Math.Max(FreeSpace.Width, FreeSpace.Height);

        public int MinAvail => Math.Min(FreeSpace.Width + BorderSize.Width, FreeSpace.Height + BorderSize.Height);
        public int MaxAvail => Math.Max(FreeSpace.Width + BorderSize.Width, FreeSpace.Height + BorderSize.Height);

        public PointerDirection PointerDirection;
        public SizeI BorderSize;

        public override string ToString()
        {
            return "{PointerDirection=" + PointerDirection + ", BorderSize=" + BorderSize + ", FreeSpace=" + FreeSpace + "}";
        }
    }
}
