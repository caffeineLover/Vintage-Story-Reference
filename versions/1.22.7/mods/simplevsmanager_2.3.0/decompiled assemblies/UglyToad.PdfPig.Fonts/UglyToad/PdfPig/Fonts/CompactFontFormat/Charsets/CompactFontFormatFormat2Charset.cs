using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets
{
	// Token: 0x020000A9 RID: 169
	internal class CompactFontFormatFormat2Charset : CompactFontFormatCharset
	{
		// Token: 0x06000407 RID: 1031 RVA: 0x0001CC7F File Offset: 0x0001AE7F
		public CompactFontFormatFormat2Charset([TupleElementNames(new string[]
		{
			"glyphId",
			"stringId",
			"name"
		})] IReadOnlyList<ValueTuple<int, int, string>> data) : base(data)
		{
		}
	}
}
