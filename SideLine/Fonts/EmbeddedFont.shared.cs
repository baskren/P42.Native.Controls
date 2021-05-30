using System;
using System.IO;

namespace P42.Native.Controls
{
	public class EmbeddedFont
	{
		public string FontName { get; set; }
		public Stream ResourceStream { get; set; }
	}
}