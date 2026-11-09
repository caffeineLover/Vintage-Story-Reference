using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x02000095 RID: 149
	internal class CompactFontFormatSubroutinesSelector
	{
		// Token: 0x06000356 RID: 854 RVA: 0x000192A7 File Offset: 0x000174A7
		public CompactFontFormatSubroutinesSelector(CompactFontFormatIndex global, CompactFontFormatIndex local)
		{
			this.global = global;
			this.local = local;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000192BD File Offset: 0x000174BD
		public CompactFontFormatSubroutinesSelector(CompactFontFormatIndex global, CompactFontFormatIndex local, ICompactFontFormatFdSelect fdSelect, IReadOnlyList<CompactFontFormatIndex> perFontLocalSubroutines)
		{
			this.global = global;
			this.local = local;
			this.fdSelect = fdSelect;
			this.perFontLocalSubroutines = perFontLocalSubroutines;
			this.isCid = true;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000192EC File Offset: 0x000174EC
		[return: TupleElementNames(new string[]
		{
			"global",
			"local"
		})]
		public ValueTuple<CompactFontFormatIndex, CompactFontFormatIndex> GetSubroutines(int glyphId)
		{
			if (!this.isCid)
			{
				return new ValueTuple<CompactFontFormatIndex, CompactFontFormatIndex>(this.global, this.local);
			}
			int fontDictionaryIndex = this.fdSelect.GetFontDictionaryIndex(glyphId);
			if (fontDictionaryIndex < 0 || fontDictionaryIndex >= this.perFontLocalSubroutines.Count)
			{
				return new ValueTuple<CompactFontFormatIndex, CompactFontFormatIndex>(this.global, this.local);
			}
			CompactFontFormatIndex compactFontFormatIndex = this.perFontLocalSubroutines[fontDictionaryIndex];
			return new ValueTuple<CompactFontFormatIndex, CompactFontFormatIndex>(this.global, compactFontFormatIndex ?? this.local);
		}

		// Token: 0x04000324 RID: 804
		private readonly CompactFontFormatIndex global;

		// Token: 0x04000325 RID: 805
		private readonly CompactFontFormatIndex local;

		// Token: 0x04000326 RID: 806
		private readonly bool isCid;

		// Token: 0x04000327 RID: 807
		private readonly ICompactFontFormatFdSelect fdSelect;

		// Token: 0x04000328 RID: 808
		private readonly IReadOnlyList<CompactFontFormatIndex> perFontLocalSubroutines;
	}
}
