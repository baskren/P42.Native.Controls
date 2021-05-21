using System;
namespace P42.Native.Controls
{
	public struct PointI : IEquatable<PointI>
	{
		public static readonly PointI Empty;

		private int x;

		private int y;

		public bool IsEmpty
		{
			get
			{
				if (x == 0)
					return y == 0;
				return false;
			}
		}

		public int X
		{
			get=> x;
			set=> x = value;
		}

		public int Y
		{
			get => y;
			set => y = value;
		}

		public PointI(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public PointI(SizeI sz)
		{
			x = sz.Width;
			y = sz.Height;
		}

		public static PointI operator +(PointI pt, SizeI sz)
			=> Add(pt, sz);

		public static PointI operator -(PointI pt, SizeI sz)
			=> Subtract(pt, sz);

		public static PointI operator -(PointI pt)
			=> new PointI(-pt.X, -pt.Y);

		public static bool operator ==(PointI left, PointI right)
		{
			if (left.X == right.X)
				return left.Y == right.Y;
			return false;
		}

		public static bool operator !=(PointI left, PointI right)
			=> !(left == right);

		public static PointI Add(PointI pt, SizeI sz)
			=> new PointI(pt.X + sz.Width, pt.Y + sz.Height);

		public static PointI Subtract(PointI pt, SizeI sz)
			=> new PointI(pt.X - sz.Width, pt.Y - sz.Height);

		public static PointI Ceiling(Point value)
			=> new PointI((int)Math.Ceiling(value.X), (int)Math.Ceiling(value.Y));

		public static PointI Truncate(Point value)
			=> new PointI((int)value.X, (int)value.Y);

		public static PointI Round(Point value)
			=> new PointI((int)Math.Round(value.X), (int)Math.Round(value.Y));

		public override bool Equals(object obj)
		{
			if (obj is PointI)
				return Equals((PointI)obj);
			return false;
		}

		public bool Equals(PointI other)
			=> this == other;

		public override int GetHashCode()
		{
			int hashCode = 1502939027;
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			return hashCode;
		}

		public PointI Offset(int dx, int dy)
		{
			X += dx;
			Y += dy;
			return this;
		}

		public PointI Offset(PointI p)
			=> Offset(p.X, p.Y);

		public override string ToString()
			=> "{X=" + X + ",Y=" + Y + "}";

		private static short HighInt16(int n)
			=> (short)((n >> 16) & 0xFFFF);

		private static short LowInt16(int n)
			=> (short)(n & 0xFFFF);

    }
}
