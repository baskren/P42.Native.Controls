using System;
namespace P42.Native.Controls
{
	public interface IEmbeddedFontLoader
	{
		(bool success, string filePath) LoadFont(EmbeddedFont font);
	}
}