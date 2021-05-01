using System;
using System.ComponentModel;

namespace P42.Native.Controls
{
    public struct Size : IEquatable<Size>
    {
		public static readonly Size Empty;

		private double width;

		private double height;

		public bool IsEmpty
		{
			get
			{
				if (width == 0)
				{
					return height == 0;
				}
				return false;
			}
		}

		public double Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
			}
		}

		public double Height
		{
			get
			{
				return height;
			}
			set
			{
				height = value;
			}
		}

		public Size(Point pt)
		{
			width = pt.X;
			height = pt.Y;
		}

		public Size(double width, double height)
		{
			this.width = width;
			this.height = height;
		}

		public static implicit operator Size(double s)
			=> new Size(s,s);

		public static Size operator +(Size sz1, Size sz2)
		{
			return Add(sz1, sz2);
		}

		public static Size operator -(Size sz1, Size sz2)
		{
			return Subtract(sz1, sz2);
		}

		public static Size operator *(double left, Size right)
		{
			return Multiply(right, left);
		}

		public static Size operator *(Size left, double right)
		{
			return Multiply(left, right);
		}

		public static Size operator /(Size left, double right)
		{
			return new Size(left.width / right, left.height / right);
		}

		public static bool operator ==(Size sz1, Size sz2)
		{
			if (sz1.Width == sz2.Width)
			{
				return sz1.Height == sz2.Height;
			}
			return false;
		}

		public static bool operator !=(Size sz1, Size sz2)
		{
			return !(sz1 == sz2);
		}

		public static explicit operator Point(Size size)
		{
			return new Point(size.Width, size.Height);
		}

		public static Size Add(Size sz1, Size sz2)
		{
			return new Size(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
		}

		public static Size Ceiling(Size value)
		{
			return new Size((int)Math.Ceiling(value.Width), (int)Math.Ceiling(value.Height));
		}

		public static Size Subtract(Size sz1, Size sz2)
		{
			return new Size(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
		}

		public static Size Truncate(Size value)
		{
			return new Size((int)value.Width, (int)value.Height);
		}

		public static Size Round(Size value)
		{
			return new Size((int)Math.Round(value.Width), (int)Math.Round(value.Height));
		}

		public override bool Equals(object obj)
		{
			if (obj is Size)
			{
				return Equals((Size)obj);
			}
			return false;
		}

		public bool Equals(Size other)
		{
			return this == other;
		}

		public override string ToString()
		{
			return "{Width=" + width + ", Height=" + height + "}";
		}

		private static Size Multiply(Size size, double multiplier)
		{
			return new Size(size.width * multiplier, size.height * multiplier);
		}

        public override int GetHashCode()
        {
            int hashCode = 1263118649;
            hashCode = hashCode * -1521134295 + width.GetHashCode();
            hashCode = hashCode * -1521134295 + height.GetHashCode();
            return hashCode;
        }
    }
}
