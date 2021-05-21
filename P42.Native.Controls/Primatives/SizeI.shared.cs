using System;
using System.ComponentModel;

namespace P42.Native.Controls
{
    public struct SizeI : IEquatable<SizeI>
    {
		public static readonly SizeI Empty;

		private int width;

		private int height;

		public bool IsEmpty
		{
			get
			{
				if (width == 0)
					return height == 0;
				return false;
			}
		}

		public int Width
		{
			get => width;
			set => width = value;
		}

		public int Height
		{
			get => height;
			set => height = value;
		}

		public SizeI(PointI pt)
		{
			width = pt.X;
			height = pt.Y;
		}

		public SizeI(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		public static explicit operator SizeI(int s)
			=> new SizeI(s,s);

		public static explicit operator SizeI(PointI p)
			=> new SizeI(p.X, p.Y);

		public static SizeI operator +(SizeI sz1, SizeI sz2)
			=> Add(sz1, sz2);

		public static SizeI operator -(SizeI sz1, SizeI sz2)
			=> Subtract(sz1, sz2);

		public static Size operator *(double left, SizeI right)
			=> Multiply(right, left);
		
		public static Size operator *(SizeI left, double right)
			=>  Multiply(left, right);

		public static Size operator /(SizeI left, double right)
			=>  new Size(left.width / right, left.height / right);

		public static bool operator ==(SizeI sz1, SizeI sz2)
		{
			if (sz1.Width == sz2.Width)
				return sz1.Height == sz2.Height;
			return false;
		}

		public static bool operator !=(SizeI sz1, SizeI sz2)
			=> !(sz1 == sz2);

		public static explicit operator Point(SizeI size)
			=> new Point(size.Width, size.Height);

		public static SizeI Add(SizeI sz1, SizeI sz2)
			=> new SizeI(sz1.Width + sz2.Width, sz1.Height + sz2.Height);

		public static SizeI Subtract(SizeI sz1, SizeI sz2)
			=> new SizeI(sz1.Width - sz2.Width, sz1.Height - sz2.Height);

		public static SizeI Ceiling(Size value)
			=> new SizeI((int)Math.Ceiling(value.Width), (int)Math.Ceiling(value.Height));

		public static SizeI Truncate(Size value)
			=> new SizeI((int)value.Width, (int)value.Height);

		public static SizeI Round(Size value)
			=> new SizeI((int)Math.Round(value.Width), (int)Math.Round(value.Height));

		public override bool Equals(object obj)
		{
			if (obj is SizeI)
				return Equals((SizeI)obj);
			return false;
		}

		public bool Equals(SizeI other)
			=> this == other;

		public override string ToString()
			=> "{Width=" + width + ", Height=" + height + "}";

		private static Size Multiply(SizeI size, double multiplier)
			=> new Size(size.width * multiplier, size.height * multiplier);

        public override int GetHashCode()
        {
            int hashCode = 1263118649;
            hashCode = hashCode * -1521134295 + width.GetHashCode();
            hashCode = hashCode * -1521134295 + height.GetHashCode();
            return hashCode;
        }
    }
}
