using System;
namespace P42.Native.Controls
{
	public struct Point : IEquatable<Point>
	{
		public static readonly Point Empty;

		private double x;

		private double y;

		public bool IsEmpty
		{
			get
			{
				if (x == 0)
					return y == 0;
				return false;
			}
		}

		public double X
		{
			get=> x;
			set=> x = value;
		}

		public double Y
		{
			get => y;
			set => y = value;
		}

		public Point(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		public Point(Size sz)
		{
			x = sz.Width;
			y = sz.Height;
		}

		public static explicit operator Point(PointI pI)
			=> new Point(pI.X, pI.Y);

		public static Point operator +(Point pt, Size sz)
			=> Add(pt, sz);

		public static Point operator -(Point pt, Size sz)
			=> Subtract(pt, sz);

		public static Point operator -(Point pt)
			=> new Point(-pt.X, -pt.Y);

		public static Point operator *(Point pt, double s)
			=> Multiply(pt, s);

		public static Point operator *(double s, Point pt)
			=> Multiply(pt, s);

		public static Point operator /(Point pt, double s)
			=> Divide(pt, s);

		public static bool operator ==(Point left, Point right)
		{
			if (left.X == right.X)
				return left.Y == right.Y;
			return false;
		}

		public static bool operator !=(Point left, Point right)
			=> !(left == right);

		public static Point Add(Point pt, Size sz)
			=> new Point(pt.X + sz.Width, pt.Y + sz.Height);

		public static Point Subtract(Point pt, Size sz)
			=> new Point(pt.X - sz.Width, pt.Y - sz.Height);

		public static Point Multiply(Point pt, double s)
			=> new Point(pt.X * s, pt.Y * s);

		public static Point Multiply(PointI pt, double s)
			=> new Point(pt.X * s, pt.Y * s);

		public static Point Divide(Point pt, double s)
			=> new Point(pt.X / s, pt.Y / s);

		public static Point Divide(PointI pt, double s)
			=> new Point(pt.X / s, pt.Y / s);

		public override bool Equals(object obj)
		{
			if (obj is Point)
				return Equals((Point)obj);
			return false;
		}

		public bool Equals(Point other)
			=> this == other;

		public override int GetHashCode()
		{
			int hashCode = 1502939027;
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			return hashCode;
		}

		public Point Offset(double dx, double dy)
		{
			X += dx;
			Y += dy;
			return this;
		}

		public Point Offset(Point p)
			=> Offset(p.X, p.Y);

		public override string ToString()
			=> "{X=" + X + ",Y=" + Y + "}";

		private static short HighInt16(int n)
			=> (short)((n >> 16) & 0xFFFF);

		private static short LowInt16(int n)
			=> (short)(n & 0xFFFF);

    }
}
