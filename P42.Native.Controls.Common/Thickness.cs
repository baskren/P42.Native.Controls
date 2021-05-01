using System;
namespace P42.Native.Controls
{
    public struct Thickness
    {
        public double Left { get; set; } 

        public double Top { get; set; }

        public double Right { get; set; }

        public double Bottom { get; set; }

        public Thickness(double all = 0)
        {
            Left = Top = Right = Bottom = all;
        }

        public Thickness(double horizontal, double vertical)
        {
            Left = Right = horizontal;
            Top = Bottom = vertical;
        }

        public Thickness(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public double Horizontal => Left + Right;

        public double Vertical => Top + Bottom;


		public static implicit operator Thickness(double t)
			=> new Thickness(t);

		public static Thickness operator +(Thickness sz1, Thickness sz2)
		{
			return Add(sz1, sz2);
		}

		public static Thickness operator -(Thickness sz1, Thickness sz2)
		{
			return Subtract(sz1, sz2);
		}

		public static Thickness operator *(double left, Thickness right)
		{
			return Multiply(right, left);
		}

		public static Thickness operator *(Thickness left, double right)
		{
			return Multiply(left, right);
		}

		public static Thickness operator /(Thickness t, double s)
		{
			return new Thickness(t.Left / s, t.Top / s, t.Right / s, t.Bottom / s);
		}

		public static bool operator ==(Thickness t1, Thickness t2)
		{
			return (t1.Left == t2.Left &&
				t1.Top == t2.Top &&
				t1.Right == t2.Right &&
				t1.Bottom == t2.Bottom);
		}

		public static bool operator !=(Thickness t1, Thickness t2)
		{
			return !(t1 == t2);
		}

		public static Thickness Add(Thickness t1, Thickness t2)
		{
			return new Thickness(t1.Left + t2.Left, t1.Top + t2.Top, t1.Right + t2.Right, t1.Bottom + t2.Bottom);
		}

		public static Thickness Ceiling(Thickness t)
		{
			return new Thickness((int)Math.Ceiling(t.Left), (int)Math.Ceiling(t.Top), (int)Math.Ceiling(t.Right), (int)Math.Ceiling(t.Bottom));
		}

		public static Thickness Subtract(Thickness t1, Thickness t2)
		{
			return new Thickness(t1.Left - t2.Left, t1.Top - t2.Top, t1.Right - t2.Right, t1.Bottom - t2.Bottom);
		}

		public static Thickness Truncate(Thickness t)
		{
			return new Thickness((int)t.Left, (int)t.Top, (int)t.Right, (int)t.Bottom);
		}

		public static Thickness Round(Thickness t)
		{
			return new Thickness((int)Math.Round(t.Left), (int)Math.Round(t.Top), (int)Math.Round(t.Right), (int)Math.Round(t.Bottom));
		}

		public override bool Equals(object obj)
		{
			if (obj is Thickness)
			{
				return Equals((Thickness)obj);
			}
			return false;
		}

		public bool Equals(Thickness other)
		{
			return this == other;
		}

		public override string ToString()
		{
			return "{l=" + Left + ", t=" + Top + ", r=" + Right + ", b=" + Bottom + "}";
		}

		private static Thickness Multiply(Thickness t, double m)
		{
			return new Thickness(t.Left * m, t.Top * m, t.Right * m, t.Bottom * m);
		}

	}
}
