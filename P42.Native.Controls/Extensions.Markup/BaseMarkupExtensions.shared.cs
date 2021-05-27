using System;


namespace P42.Native.Controls
{
    public static class BaseMarkupExtensions
    {
		public static T Assign<T>(this T notifiable, out T variable)
			where T : DINotifiable
		{
			variable = notifiable;
			return notifiable;
		}

		public static T Invoke<T>(this T notifiable, Action<T> action) where T : DINotifiable
		{
			action?.Invoke(notifiable);
			return notifiable;
		}

	}
}
