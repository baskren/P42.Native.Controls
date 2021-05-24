using Android.Views;
using ATextAlignment = Android.Views.TextAlignment;

namespace P42.Native.Controls
{
	internal static class AlignmentExtensions
	{
		internal static ATextAlignment ToTextAlignment(this Alignment alignment)
		{
			switch (alignment)
			{
				case Alignment.Center:
					return ATextAlignment.Center;
				case Alignment.End:
					return ATextAlignment.ViewEnd;
				default:
					return ATextAlignment.ViewStart;
			}
		}

		internal static GravityFlags ToHorizontalGravityFlags(this Alignment alignment)
		{
			switch (alignment)
			{
				case Alignment.Center:
					return GravityFlags.CenterHorizontal;
				case Alignment.End:
					return GravityFlags.End;
				default:
					return GravityFlags.Start;
			}
		}

		internal static GravityFlags ToVerticalGravityFlags(this Alignment alignment)
		{
			switch (alignment)
			{
				case Alignment.Start:
					return GravityFlags.Top;
				case Alignment.End:
					return GravityFlags.Bottom;
				default:
					return GravityFlags.CenterVertical;
			}
		}
	}
}