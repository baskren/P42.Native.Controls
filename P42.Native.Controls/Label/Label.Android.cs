
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using Android.Util;
using Android.Views;
using AndroidX.Core.View;
using AView = Android.Views.View;

namespace P42.Native.Controls
{
	public partial class Label : Android.Widget.TextView
	{
		static Color NtvDefaultTextColor;
		static double NtvDefaultFontSize;

		static Label()
        {
			var defaultLabel = new Android.Widget.TextView(global::P42.Utils.Droid.Settings.Context);
			NtvDefaultTextColor = defaultLabel.TextColors.DefaultColor.ToColor();
			NtvDefaultFontSize = defaultLabel.TextSize;
			defaultLabel.Dispose();
        }


		#region Construction / Disposal
		public Label() : this(global::P42.Utils.Droid.Settings.Context) { }

		public Label(Context context) : base(context)
		{
			Build();
		}


		void Build()
        {
			NtvBaseView = this;
			b_DipTextColor = NtvDefaultTextColor;
			b_DipFontSize = NtvDefaultFontSize;
        }

		bool _disposed;
		protected override void Dispose(bool disposing)
		{
			if (disposing && !_disposed)
			{
				_disposed = true;
				Background?.Dispose();
			}

			base.Dispose(disposing);
		}
        #endregion



        #region Android Measure / Layout / Draw
        /*
		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout(changed, left, top, right, bottom);
		}
		*/
        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
			DipHasDrawn = true;
			NtvActualSize = new SizeI(canvas.Width, canvas.Height);
		}
		#endregion



		void UpdateLineBreakMode()
		{
			int maxLines = Int32.MaxValue;
			bool singleLine = false;
			switch (DipLineBreakMode)
			{
				case LineBreakMode.NoWrap:
					maxLines = 1;
					Ellipsize = null;
					break;
				case LineBreakMode.WordWrap:
					Ellipsize = null;
					break;
				case LineBreakMode.CharacterWrap:
					Ellipsize = null;
					break;
				case LineBreakMode.HeadTruncation:
					maxLines = 1;
					singleLine = true; // Workaround for bug in older Android API versions (https://bugzilla.xamarin.com/show_bug.cgi?id=49069)
					Ellipsize = TextUtils.TruncateAt.Start;
					break;
				case LineBreakMode.TailTruncation:
					maxLines = 1;
					Ellipsize = TextUtils.TruncateAt.End;
					break;
				case LineBreakMode.MiddleTruncation:
					maxLines = 1;
					singleLine = true; // Workaround for bug in older Android API versions (https://bugzilla.xamarin.com/show_bug.cgi?id=49069)
					Ellipsize = TextUtils.TruncateAt.Middle;
					break;
			}
			SetSingleLine(singleLine);
			SetMaxLines(maxLines);
		}

		void UpdateText()
		{
			switch (DipTextType)
			{
				case TextType.Html:
					if (Xamarin.Essentials.DeviceInfo.Version >= new Version(7,0)) // Nouget+
						SetText(Html.FromHtml(Text ?? string.Empty, FromHtmlOptions.ModeCompact), BufferType.Spannable);
					else
#pragma warning disable CS0618 // Type or member is obsolete
						SetText(Html.FromHtml(Text ?? string.Empty), BufferType.Spannable);
#pragma warning restore CS0618 // Type or member is obsolete
					break;

				default:
					base.Text = Text;
					break;
			}
		}



		#region INotifiable

		#region Methods
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (_disposed)
				return;

			if (propertyName == nameof(DipFontFamily) || propertyName == nameof(DipFontStyle))
				Typeface = FontExtensions.ToTypeface(DipFontFamily, DipFontStyle);
			else if (propertyName == nameof(DipFontSize))
				SetTextSize(ComplexUnitType.Sp, (float)DipFontSize);
			else if (propertyName == nameof(DipText) || propertyName == nameof(DipTextType))
				UpdateText();
			else if (propertyName == nameof(DipTextColor))
				SetTextColor(b_DipTextColor);
			else if (propertyName == nameof(DipLineBreakMode))
				UpdateLineBreakMode();
			else if (propertyName == nameof(DipHorizontalTextAlignment) || propertyName == nameof(DipVerticalTextAlignment))
				Gravity = DipHorizontalTextAlignment.ToHorizontalGravityFlags() | DipVerticalTextAlignment.ToVerticalGravityFlags();
			else if (propertyName == nameof(DipMargin))
				SetPadding(NtvMargin.Left, NtvMargin.Top, NtvMargin.Right, NtvMargin.Bottom);
		}

		#endregion

		#endregion



	}
}