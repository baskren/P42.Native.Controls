using System;

namespace P42.Native.Controls
{
    public static class INotifiableMarkupExtensions
    {
		public static TNotifiable Assign<TNotifiable, TVariable>(this TNotifiable notifiable, out TVariable variable)
			where TNotifiable : INotifiable, TVariable
		{
			variable = notifiable;
			return notifiable;
		}

		public static TNotifiable Invoke<TNotifiable>(this TNotifiable notifiable, Action<TNotifiable> action) where TNotifiable : INotifiable
		{
			action?.Invoke(notifiable);
			return notifiable;
		}

	}
}
