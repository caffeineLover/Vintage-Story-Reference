using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets
{
	// Token: 0x020000A8 RID: 168
	internal class CompactFontFormatFormat1Charset : CompactFontFormatCharset
	{
		// Token: 0x06000406 RID: 1030 RVA: 0x0001CC76 File Offset: 0x0001AE76
		public CompactFontFormatFormat1Charset([TupleElementNames(new string[]
		{
			"glyphId",
			"stringId",
			"name"
		})] IReadOnlyList<ValueTuple<int, int, string>> data) : base(data)
		{
		}
	}
}
