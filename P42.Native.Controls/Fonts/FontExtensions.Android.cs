using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Android.Graphics;
using AApplication = Android.App.Application;

namespace P42.Native.Controls
{
	public static class FontExtensions
	{
		internal static Typeface ToTypeface(this string fontfamily, FontStyle style = FontStyle.Normal)
		{
			if (string.IsNullOrWhiteSpace(fontfamily))
				return Typeface.DefaultFromStyle(ToTypefaceStyle(style));

			var result = fontfamily.TryGetFromAssets();
			if (result.success)
			{
				return result.typeface;
			}
			else
			{
				var typefaceStyle = ToTypefaceStyle(style);
				return Typeface.Create(fontfamily, typefaceStyle);
			}

		}

		static (bool success, Typeface typeface) TryGetFromAssets(this string fontName)
		{
			//First check Alias
			var (hasFontAlias, fontPostScriptName) = FontRegistrar.HasFont(fontName);
			if (hasFontAlias)
				return (true, Typeface.CreateFromFile(fontPostScriptName));

			var isAssetFont = IsAssetFontFamily(fontName);
			if (isAssetFont)
			{
				return LoadTypefaceFromAsset(fontName);
			}

			var folders = new[]
			{
				"",
				"Fonts/",
				"fonts/",
			};


			//copied text
			var fontFile = FontFile.FromString(fontName);

			if (!string.IsNullOrWhiteSpace(fontFile.Extension))
			{
				var (hasFont, fontPath) = FontRegistrar.HasFont(fontFile.FileNameWithExtension());
				if (hasFont)
				{
					return (true, Typeface.CreateFromFile(fontPath));
				}
			}
			else
			{
				foreach (var ext in FontFile.Extensions)
				{
					var formated = fontFile.FileNameWithExtension(ext);
					var (hasFont, fontPath) = FontRegistrar.HasFont(formated);
					if (hasFont)
					{
						return (true, Typeface.CreateFromFile(fontPath));
					}

					foreach (var folder in folders)
					{
						formated = $"{folder}{fontFile.FileNameWithExtension()}#{fontFile.PostScriptName}";
						var result = LoadTypefaceFromAsset(formated);
						if (result.success)
							return result;
					}

				}
			}

			return (false, null);
		}

		static (bool success, Typeface typeface) LoadTypefaceFromAsset(string fontfamily)
		{
			try
			{
				var result = Typeface.CreateFromAsset(AApplication.Context.Assets, FontNameToFontFile(fontfamily));
				return (true, result);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				return (false, null);
			}
		}


		static bool IsAssetFontFamily(string name)
		{
			return name != null && (name.Contains(".ttf#") || name.Contains(".otf#"));
		}


		public static TypefaceStyle ToTypefaceStyle(FontStyle style)
		{
			var result = (TypefaceStyle)style;
			return result;
		}

		static string FontNameToFontFile(string fontFamily)
		{
			fontFamily = fontFamily ?? String.Empty;
			int hashtagIndex = fontFamily.IndexOf('#');
			if (hashtagIndex >= 0)
				return fontFamily.Substring(0, hashtagIndex);

			throw new InvalidOperationException($"Can't parse the {nameof(fontFamily)} {fontFamily}");
		}
	}
}