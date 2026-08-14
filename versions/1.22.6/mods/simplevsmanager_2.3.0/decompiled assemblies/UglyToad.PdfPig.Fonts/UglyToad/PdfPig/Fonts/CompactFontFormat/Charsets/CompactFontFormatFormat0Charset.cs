using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets
{
	// Token: 0x020000A7 RID: 167
	internal class CompactFontFormatFormat0Charset : CompactFontFormatCharset
	{
		// Token: 0x06000405 RID: 1029 RVA: 0x0001CC6D File Offset: 0x0001AE6D
		public CompactFontFormatFormat0Charset([TupleElementNames(new string[]
		{
			"glyphId",
			"stringId",
			"name"
		})] IReadOnlyList<ValueTuple<int, int, string>> data) : base(data)
		{
		}
	}
}
