using System;
using System.ComponentModel;

namespace P42.Native.Controls
{
	public struct Rect : IEquatable<Rect>
	{
		public static readonly Rect Empty;

		private double x;

		private double y;

		private double width;

		private double height;

		public Point Location
		{
			get => new Point(X, Y);
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}

		[Browsable(false)]
		public Size Size
		{
			get => new Size(Width, Height);
			set
			{
				Width = value.Width;
				Height = value.Height;
			}
		}

		public double X
		{
			get => x;
			set => x = value;
		}

		public double Y
		{
			get => y;
			set => y = value;
		}

		public double Width
		{
			get => width;
			set => width = value;
		}

		public double Height
		{
			get => height;
			set => height = value;
		}

		[Browsable(false)]
		public double Left
		{
			get => X;
			set => X = value;
		}

		[Browsable(false)]
		public double Top
		{
			get => Y;
			set => Y = value;
		}

		[Browsable(false)]
		public double Right
		{
			get => X + Width;
			set => Width = value - X;
		}

		[Browsable(false)]
		public double Bottom
		{
			get => Y + Height;
			set => Height = value - Y;
		}

		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				if (!(Width <= 0f))
					return Height <= 0f;
				return true;
			}
		}

		public double HoriztonalCenter => x + width / 2;

		public double VerticalCenter => y + height / 2;

		public Point Center
			=> new Point(HoriztonalCenter, VerticalCenter);

		public Rect(double x, double y, double width, double height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		public Rect(Point location, Size size)
		{
			x = location.X;
			y = location.Y;
			width = size.Width;
			height = size.Height;
		}

		public Rect(Size s)
        {
			x = 0;
			y = 0;
			width = s.Width;
			height = s.Height;
        }

		public static Rect FromLTRB(double left, double top, double right, double bottom)
			=> new Rect(left, top, right - left, bottom - top);
		
		public override bool Equals(object obj)
		{
			if (obj is Rect)
				return Equals((Rect)obj);
			return false;
		}

		public bool Equals(Rect other)
			=> this == other;

		public static bool operator ==(Rect left, Rect right)
		{
			if (left.X == right.X && left.Y == right.Y && left.Width == right.Width)
				return left.Height == right.Height;
			return false;
		}

		public static bool operator !=(Rect left, Rect right)
			=> !(left == right);
		

		public bool Contains(double x, double y)
		{
			if (X <= x && x < X + Width && Y <= y)
				return y < Y + Height;
			return false;
		}

		public bool Contains(Point pt)
			=> Contains(pt.X, pt.Y);
		

		public bool Contains(Rect rect)
		{
			if (X <= rect.X && rect.X + rect.Width <= X + Width && Y <= rect.Y)
				return rect.Y + rect.Height <= Y + Height;
			return false;
		}

		public override int GetHashCode()
		{
			int hashCode = -1222528132;
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			hashCode = hashCode * -1521134295 + width.GetHashCode();
			hashCode = hashCode * -1521134295 + height.GetHashCode();
			return hashCode;
		}

		public Rect Inflate(double x, double y)
		{
			X -= x;
			Y -= y;
			Width += 2 * x;
			Height += 2 * y;
			return this;
		}

		public Rect Inflate(double v)
			=> Inflate(v, v);

		public Rect Inflate(Size size)
			=> Inflate(size.Width, size.Height);

		public Rect Inflate(Thickness t)
        {
			X -= t.Left;
			Y -= t.Top;
			Width += t.Horizontal;
			Height += t.Vertical;
			return this;
        }

		public static Rect Inflate(Rect r, double v)
			=> Inflate(r, v, v);

		public static Rect Inflate(Rect r, double x, double y)
			=> new Rect(r.X - x, r.Y - y, r.Width + 2 * x, r.Height + 2 * y);

		public static Rect Inflate(Rect r, Thickness t)
			=> new Rect(r.X - t.Left, r.Y - t.Top, r.Width + t.Horizontal, r.Height + t.Vertical);

		public Rect Intersect(Rect rect)
		{
			Rect Rect = Intersect(rect, this);
			X = Rect.X;
			Y = Rect.Y;
			Width = Rect.Width;
			Height = Rect.Height;
			return this;
		}

		public static Rect Intersect(Rect a, Rect b)
		{
			double num = Math.Max(a.X, b.X);
			double num2 = Math.Min(a.X + a.Width, b.X + b.Width);
			double num3 = Math.Max(a.Y, b.Y);
			double num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
			if (num2 >= num && num4 >= num3)
				return new Rect(num, num3, num2 - num, num4 - num3);
			return Empty;
		}

		public bool IntersectsWith(Rect rect)
		{
			if (rect.X < X + Width && X < rect.X + rect.Width && rect.Y < Y + Height)
			{
				return Y < rect.Y + rect.Height;
			}
			return false;
		}

		public static Rect Union(Rect a, Rect b)
		{
			double num = Math.Min(a.X, b.X);
			double num2 = Math.Max(a.X + a.Width, b.X + b.Width);
			double num3 = Math.Min(a.Y, b.Y);
			double num4 = Math.Max(a.Y + a.Height, b.Y + b.Height);
			return new Rect(num, num3, num2 - num, num4 - num3);
		}

		public Rect Offset(Point pos)
		{
			Offset(pos.X, pos.Y);
			return this;
		}

		public Rect Offset(double x, double y)
		{
			X += x;
			Y += y;
			return this;
		}

		public override string ToString()
		=>  "{X=" + X + ",Y=" + Y + ",Width=" + Width + ",Height=" + Height + "}";

		public static Rect Scale(Rect r, double s)
			=> new Rect(r.X * s, r.Y * s, r.Width * s, r.Height * s);

		public static Rect Round(Rect r)
			=> new Rect((int)(r.X + 0.5), (int)(r.Y + 0.5), (int)(r.Width + 0.5), (int)(r.Height + 0.5));

		public static Rect Truncate(Rect r)
			=> new Rect((int)r.X, (int) r.Y, (int) r.Width, (int) r.Height);

		public static Rect operator *(Rect r, double s)
			=> Scale(r, s);

		public static Rect operator *(double s, Rect r)
			=> Scale(r, s);

		public static Rect operator /(Rect r, double s)
			=> new Rect(r.X / s, r.Y / s, r.Width / s, r.Height / s);

		public static Rect operator +(Rect r, Thickness t)
			=> Inflate(r, t);

		public static Rect operator +(Thickness t, Rect r)
			=> Inflate(r, t);

		public static Rect operator -(Rect r, Thickness t)
			=> Inflate(r, -t);

	}
}
