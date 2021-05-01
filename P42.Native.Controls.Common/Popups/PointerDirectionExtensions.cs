using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P42.Native.Controls
{
	/// <summary>
	/// Pointer direction extensions.
	/// </summary>
	public static class PointerDirectionExtensions
	{
		/// <summary>
		/// Determines if pointer direction is vertical.
		/// </summary>
		/// <returns><c>true</c> if is vertical the specified dir; otherwise, <c>false</c>.</returns>
		/// <param name="dir">Dir.</param>
		public static bool IsVertical(this PointerDirection dir)
		{
			return dir == PointerDirection.Up || dir == PointerDirection.Down;
		}
		/// <summary>
		/// Determines if pointer direction is horizontal.
		/// </summary>
		/// <returns><c>true</c> if is horizontal the specified dir; otherwise, <c>false</c>.</returns>
		/// <param name="dir">Dir.</param>
		public static bool IsHorizontal(this PointerDirection dir)
		{
			return dir == PointerDirection.Left || dir == PointerDirection.Right;
		}

		/// <summary>
		/// Determines if a pointer is allowed to point left.
		/// </summary>
		/// <returns><c>true</c>, if allowed was lefted, <c>false</c> otherwise.</returns>
		/// <param name="dir">Dir.</param>
		public static bool LeftAllowed(this PointerDirection dir)
		{
			return (dir & PointerDirection.Left) != 0;
		}
		/// <summary>
		/// Determines if a pointer is allowed to point right.
		/// </summary>
		/// <returns><c>true</c>, if allowed was righted, <c>false</c> otherwise.</returns>
		/// <param name="dir">Dir.</param>
		public static bool RightAllowed(this PointerDirection dir)
		{
			return (dir & PointerDirection.Right) != 0;
		}
		/// <summary>
		/// Determines if a pointer is allowed to point up.
		/// </summary>
		/// <returns><c>true</c>, if allowed was uped, <c>false</c> otherwise.</returns>
		/// <param name="dir">Dir.</param>
		public static bool UpAllowed(this PointerDirection dir)
		{
			return (dir & PointerDirection.Up) != 0;
		}
		/// <summary>
		/// Determines if a pointer is allowed to down.
		/// </summary>
		/// <returns><c>true</c>, if allowed was downed, <c>false</c> otherwise.</returns>
		/// <param name="dir">Dir.</param>
		public static bool DownAllowed(this PointerDirection dir)
		{
			return (dir & PointerDirection.Down) != 0;
		}

		public static PointerDirection BestFitDirection(this PointerDirection pointerDirection, Thickness available)
		{
			var fitDirections = BestFits(available);
			foreach (var direction in fitDirections)
            {
				if (pointerDirection == PointerDirection.None)
					return direction;
				if ((pointerDirection & direction) != 0)
					return direction;
            }
			return PointerDirection.None;
		}

		public static IEnumerable<PointerDirection> BestFits(Thickness thickness)
		{

			List<BestFitPlace> fits = new List<BestFitPlace>();
			if (thickness.Left >= 0)
				fits.Add(new BestFitPlace { Space = thickness.Left, Direction = PointerDirection.Right });
			if (thickness.Right >= 0)
				fits.Add(new BestFitPlace { Space = thickness.Right, Direction = PointerDirection.Left });
			if (thickness.Top >= 0)
				fits.Add(new BestFitPlace { Space = thickness.Top, Direction = PointerDirection.Down });
			if (thickness.Bottom >=0)
				fits.Add(new BestFitPlace { Space = thickness.Bottom, Direction = PointerDirection.Up });

			fits = fits.OrderByDescending((place) => place.Space).ToList();
			var result = fits.Select(place => place.Direction);
			return result;
		}
	}

}
