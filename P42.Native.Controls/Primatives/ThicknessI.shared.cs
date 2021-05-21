using System;
namespace P42.Native.Controls
{
    public struct ThicknessI : IEquatable<ThicknessI>
    {
        public int Left { get; set; } 

        public int Top { get; set; }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public ThicknessI(int all = 0)
			=> Left = Top = Right = Bottom = all;

        public ThicknessI(int horizontal, int vertical)
        {
            Left = Right = horizontal;
            Top = Bottom = vertical;
        }

        public ThicknessI(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public int Horizontal => Left + Right;

        public int Vertical => Top + Bottom;


		public static explicit operator ThicknessI(int t)
			=> new ThicknessI(t);

		public static ThicknessI operator +(ThicknessI sz1, ThicknessI sz2)
			=> Add(sz1, sz2);

		public static ThicknessI operator -(ThicknessI sz1, ThicknessI sz2)
			=> Subtract(sz1, sz2);

		public static ThicknessI operator -(ThicknessI t)
			=> new ThicknessI(-t.Left, -t.Top, -t.Right, -t.Bottom);

		public static Thickness operator *(double left, ThicknessI right)
			=>  Multiply(right, left);

		public static Thickness operator *(ThicknessI left, double right)
			=> Multiply(left, right);
		

		public static Thickness operator /(ThicknessI t, double s)
			=> new Thickness(t.Left / s, t.Top / s, t.Right / s, t.Bottom / s);

		public static bool operator ==(ThicknessI t1, ThicknessI t2)
		{
			return (t1.Left == t2.Left &&
				t1.Top == t2.Top &&
				t1.Right == t2.Right &&
				t1.Bottom == t2.Bottom);
		}

		public static bool operator !=(ThicknessI t1, ThicknessI t2)
			=> !(t1 == t2);

		public static ThicknessI Add(ThicknessI t1, ThicknessI t2)
			=> new ThicknessI(t1.Left + t2.Left, t1.Top + t2.Top, t1.Right + t2.Right, t1.Bottom + t2.Bottom);

		public static ThicknessI Subtract(ThicknessI t1, ThicknessI t2)
			=> new ThicknessI(t1.Left - t2.Left, t1.Top - t2.Top, t1.Right - t2.Right, t1.Bottom - t2.Bottom);

		public static ThicknessI Ceiling(Thickness t)
			=> new ThicknessI((int)Math.Ceiling(t.Left), (int)Math.Ceiling(t.Top), (int)Math.Ceiling(t.Right), (int)Math.Ceiling(t.Bottom));

		public static ThicknessI Truncate(Thickness t)
			=> new ThicknessI((int)t.Left, (int)t.Top, (int)t.Right, (int)t.Bottom);

		public static ThicknessI Round(Thickness t)
			=> new ThicknessI((int)Math.Round(t.Left), (int)Math.Round(t.Top), (int)Math.Round(t.Right), (int)Math.Round(t.Bottom));

		public override bool Equals(object obj)
		{
			if (obj is ThicknessI)
				return Equals((ThicknessI)obj);
			return false;
		}

		public bool Equals(ThicknessI other)
			=> this == other;

		public override string ToString()
			=> "{l=" + Left + ", t=" + Top + ", r=" + Right + ", b=" + Bottom + "}";

		private static Thickness Multiply(ThicknessI t, double m)
			=> new Thickness(t.Left * m, t.Top * m, t.Right * m, t.Bottom * m);

        public override int GetHashCode()
        {
            int hashCode = -1819631549;
            hashCode = hashCode * -1521134295 + Left.GetHashCode();
            hashCode = hashCode * -1521134295 + Top.GetHashCode();
            hashCode = hashCode * -1521134295 + Right.GetHashCode();
            hashCode = hashCode * -1521134295 + Bottom.GetHashCode();
            return hashCode;
        }
    }
}
