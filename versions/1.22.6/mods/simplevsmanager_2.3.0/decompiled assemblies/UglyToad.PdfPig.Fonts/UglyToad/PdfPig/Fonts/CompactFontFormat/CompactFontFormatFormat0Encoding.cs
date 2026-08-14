using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x02000089 RID: 137
	internal class CompactFontFormatFormat0Encoding : CompactFontFormatBuiltInEncoding
	{
		// Token: 0x06000329 RID: 809 RVA: 0x00016804 File Offset: 0x00014A04
		public CompactFontFormatFormat0Encoding([TupleElementNames(new string[]
		{
			"code",
			"sid",
			"str"
		})] IReadOnlyList<ValueTuple<int, int, string>> values, IReadOnlyList<CompactFontFormatBuiltInEncoding.Supplement> supplements) : base(supplements)
		{
			base.Add(0, 0, ".notdef");
			foreach (ValueTuple<int, int, string> valueTuple in values)
			{
				base.Add(valueTuple.Item1, valueTuple.Item2, valueTuple.Item3);
			}
		}
	}
}
