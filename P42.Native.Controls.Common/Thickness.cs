using System;
namespace P42.Native.Controls
{
    public struct Thickness
    {
        public float Left { get; set; } 

        public float Top { get; set; }

        public float Right { get; set; }

        public float Bottom { get; set; }

        public float Width => Left + Right;

        public float Height => Top + Bottom;

        public Thickness(float all = 0)
        {
            Left = Top = Right = Bottom = all;
        }

        public Thickness(float horizontal, float vertical)
        {
            Left = Right = horizontal;
            Top = Bottom = vertical;
        }

        public Thickness(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}
