using System;
using System.ComponentModel;

namespace P42.Native.Controls
{
	public struct RectI : IEquatable<RectI>
	{
		public static readonly RectI Empty;

		private int x;

		private int y;

		private int width;

		private int height;

		public PointI Location
		{
			get => new PointI(X, Y);
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}

		[Browsable(false)]
		public SizeI Size
		{
			get => new SizeI(Width, Height);
			set
			{
				Width = value.Width;
				Height = value.Height;
			}
		}

		public int X
		{
			get => x;
			set => x = value;
		}

		public int Y
		{
			get => y;
			set => y = value;
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

		[Browsable(false)]
		public int Left
		{
			get => X;
			set => X = value;
		}

		[Browsable(false)]
		public int Top
		{
			get => Y;
			set => Y = value;
		}

		[Browsable(false)]
		public int Right
		{
			get => X + Width;
			set => Width = value - X;
		}

		[Browsable(false)]
		public int Bottom
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

		public RectI(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		public RectI(PointI location, SizeI size)
		{
			x = location.X;
			y = location.Y;
			width = size.Width;
			height = size.Height;
		}

		public RectI(SizeI s)
        {
			x = 0;
			y = 0;
			width = s.Width;
			height = s.Height;
        }

		public static RectI FromLTRB(int left, int top, int right, int bottom)
			=> new RectI(left, top, right - left, bottom - top);
		
		public override bool Equals(object obj)
		{
			if (obj is RectI)
				return Equals((RectI)obj);
			return false;
		}

		public bool Equals(RectI other)
			=> this == other;

		public static bool operator ==(RectI left, RectI right)
		{
			if (left.X == right.X && left.Y == right.Y && left.Width == right.Width)
				return left.Height == right.Height;
			return false;
		}

		public static bool operator !=(RectI left, RectI right)
			=> !(left == right);
		

		public bool Contains(int x, int y)
		{
			if (X <= x && x < X + Width && Y <= y)
				return y < Y + Height;
			return false;
		}

		public bool Contains(PointI pt)
			=> Contains(pt.X, pt.Y);
		

		public bool Contains(RectI rect)
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

		public RectI Inflate(int x, int y)
		{
			X -= x;
			Y -= y;
			Width += 2 * x;
			Height += 2 * y;
			return this;
		}

		public RectI Inflate(int v)
			=> Inflate(v, v);

		public RectI Inflate(SizeI size)
			=> Inflate(size.Width, size.Height);

		public RectI Inflate(ThicknessI t)
        {
			X -= t.Left;
			Y -= t.Top;
			Width += t.Horizontal;
			Height += t.Vertical;
			return this;
        }

		public static RectI Inflate(RectI r, int v)
			=> Inflate(r, v, v);

		public static RectI Inflate(RectI r, int x, int y)
			=> new RectI(r.X - x, r.Y - y, r.Width + 2 * x, r.Height + 2 * y);

		public static RectI Inflate(RectI r, ThicknessI t)
			=> new RectI(r.X - t.Left, r.Y - t.Top, r.Width + t.Horizontal, r.Height + t.Vertical);

		public RectI Intersect(RectI rect)
		{
			RectI Rect = Intersect(rect, this);
			X = Rect.X;
			Y = Rect.Y;
			Width = Rect.Width;
			Height = Rect.Height;
			return this;
		}

		public static RectI Intersect(RectI a, RectI b)
		{
			int num = Math.Max(a.X, b.X);
			int num2 = Math.Min(a.X + a.Width, b.X + b.Width);
			int num3 = Math.Max(a.Y, b.Y);
			int num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
			if (num2 >= num && num4 >= num3)
				return new RectI(num, num3, num2 - num, num4 - num3);
			return Empty;
		}

		public bool IntersectsWith(RectI rect)
		{
			if (rect.X < X + Width && X < rect.X + rect.Width && rect.Y < Y + Height)
			{
				return Y < rect.Y + rect.Height;
			}
			return false;
		}

		public static RectI Union(RectI a, RectI b)
		{
			int num = Math.Min(a.X, b.X);
			int num2 = Math.Max(a.X + a.Width, b.X + b.Width);
			int num3 = Math.Min(a.Y, b.Y);
			int num4 = Math.Max(a.Y + a.Height, b.Y + b.Height);
			return new RectI(num, num3, num2 - num, num4 - num3);
		}

		public RectI Offset(PointI pos)
		{
			Offset(pos.X, pos.Y);
			return this;
		}

		public RectI Offset(int x, int y)
		{
			X += x;
			Y += y;
			return this;
		}

		public static RectI Ceiling(Rect r)
			=> new RectI((int)Math.Ceiling(r.X), (int)Math.Ceiling(r.Y), (int)Math.Ceiling(r.Width), (int)Math.Ceiling(r.Height));

		public static RectI Round(Rect r)
			=> new RectI((int)(r.X + 0.5), (int)(r.Y + 0.5), (int)(r.Width + 0.5), (int)(r.Height + 0.5));

		public static RectI Truncate(Rect r)
			=> new RectI((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);

		public override string ToString()
		=>  "{X=" + X + ",Y=" + Y + ",Width=" + Width + ",Height=" + Height + "}";

		public static Rect Scale(RectI r, double s)
			=> new Rect(r.X * s, r.Y * s, r.Width * s, r.Height * s);

		public static Rect operator *(RectI r, double s)
			=> Scale(r, s);

		public static Rect operator *(double s, RectI r)
			=> Scale(r, s);

		public static Rect operator /(RectI r, double s)
			=> new Rect(r.X / s, r.Y / s, r.Width / s, r.Height / s);

		public static RectI operator +(RectI r, ThicknessI t)
			=> Inflate(r, t);

		public static RectI operator +(ThicknessI t, RectI r)
			=> Inflate(r, t);

		public static RectI operator -(RectI r, ThicknessI t)
			=> Inflate(r, -t);

	}
}
