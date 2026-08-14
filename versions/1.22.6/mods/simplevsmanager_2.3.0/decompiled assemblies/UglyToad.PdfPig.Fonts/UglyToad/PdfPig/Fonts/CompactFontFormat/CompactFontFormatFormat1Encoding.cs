using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x0200008A RID: 138
	internal class CompactFontFormatFormat1Encoding : CompactFontFormatBuiltInEncoding
	{
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00016874 File Offset: 0x00014A74
		// (set) Token: 0x0600032B RID: 811 RVA: 0x0001687C File Offset: 0x00014A7C
		public int NumberOfRanges { get; set; }

		// Token: 0x0600032C RID: 812 RVA: 0x00016888 File Offset: 0x00014A88
		public CompactFontFormatFormat1Encoding(int numberOfRanges, [TupleElementNames(new string[]
		{
			"code",
			"sid",
			"str"
		})] IReadOnlyList<ValueTuple<int, int, string>> values, IReadOnlyList<CompactFontFormatBuiltInEncoding.Supplement> supplements) : base(supplements)
		{
			this.NumberOfRanges = numberOfRanges;
			base.Add(0, 0, ".notdef");
			foreach (ValueTuple<int, int, string> valueTuple in values)
			{
				base.Add(valueTuple.Item1, valueTuple.Item2, valueTuple.Item3);
			}
		}
	}
}
