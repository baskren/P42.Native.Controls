using System;
namespace P42.Native.Controls
{
    public struct Thickness : IEquatable<Thickness>
    {
        public double Left { get; set; } 

        public double Top { get; set; }

        public double Right { get; set; }

        public double Bottom { get; set; }

        public Thickness(double all = 0)
			=> Left = Top = Right = Bottom = all;

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


		public static explicit operator Thickness(double t)
			=> new Thickness(t);

		public static explicit operator Thickness(ThicknessI t)
			=> new Thickness(t.Left, t.Top, t.Right, t.Bottom);

		public static Thickness operator +(Thickness sz1, Thickness sz2)
			=> Add(sz1, sz2);

		public static Thickness operator -(Thickness sz1, Thickness sz2)
			=> Subtract(sz1, sz2);

		public static Thickness operator -(Thickness t)
			=> new Thickness(-t.Left, -t.Top, -t.Right, -t.Bottom);

		public static Thickness operator *(double left, Thickness right)
			=>  Multiply(right, left);

		public static Thickness operator *(Thickness left, double right)
			=> Multiply(left, right);
		

		public static Thickness operator /(Thickness t, double s)
			=> new Thickness(t.Left / s, t.Top / s, t.Right / s, t.Bottom / s);

		public static bool operator ==(Thickness t1, Thickness t2)
		{
			return (t1.Left == t2.Left &&
				t1.Top == t2.Top &&
				t1.Right == t2.Right &&
				t1.Bottom == t2.Bottom);
		}

		public static bool operator !=(Thickness t1, Thickness t2)
			=> !(t1 == t2);

		public static Thickness Add(Thickness t1, Thickness t2)
			=> new Thickness(t1.Left + t2.Left, t1.Top + t2.Top, t1.Right + t2.Right, t1.Bottom + t2.Bottom);

		public static Thickness Subtract(Thickness t1, Thickness t2)
			=> new Thickness(t1.Left - t2.Left, t1.Top - t2.Top, t1.Right - t2.Right, t1.Bottom - t2.Bottom);

		public override bool Equals(object obj)
		{
			if (obj is Thickness)
				return Equals((Thickness)obj);
			return false;
		}

		public bool Equals(Thickness other)
			=> this == other;

		public override string ToString()
			=> "{l=" + Left + ", t=" + Top + ", r=" + Right + ", b=" + Bottom + "}";

		private static Thickness Multiply(Thickness t, double m)
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
