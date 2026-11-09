using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.CrossReference
{
	// Token: 0x020001D9 RID: 473
	internal class CrossReferenceTablePart
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000B28 RID: 2856 RVA: 0x0003931B File Offset: 0x0003751B
		public IReadOnlyDictionary<IndirectReference, long> ObjectOffsets { get; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00039323 File Offset: 0x00037523
		// (set) Token: 0x06000B2A RID: 2858 RVA: 0x0003932B File Offset: 0x0003752B
		public long Offset { get; private set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x00039334 File Offset: 0x00037534
		public long Previous { get; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x0003933C File Offset: 0x0003753C
		// (set) Token: 0x06000B2D RID: 2861 RVA: 0x00039344 File Offset: 0x00037544
		public DictionaryToken Dictionary { get; private set; }

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x0003934D File Offset: 0x0003754D
		public CrossReferenceType Type { get; }

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000B2F RID: 2863 RVA: 0x00039355 File Offset: 0x00037555
		public long? TiedToXrefAtOffset { get; }

		// Token: 0x06000B30 RID: 2864 RVA: 0x0003935D File Offset: 0x0003755D
		public CrossReferenceTablePart(IReadOnlyDictionary<IndirectReference, long> objectOffsets, long offset, long previous, DictionaryToken dictionary, CrossReferenceType type, long? tiedToXrefAtOffset)
		{
			this.ObjectOffsets = objectOffsets;
			this.Offset = offset;
			this.Previous = previous;
			this.Dictionary = dictionary;
			this.Type = type;
			this.TiedToXrefAtOffset = tiedToXrefAtOffset;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00039392 File Offset: 0x00037592
		public void FixOffset(long offset)
		{
			this.Offset = offset;
			this.Dictionary = this.Dictionary.With(NameToken.Prev, new NumericToken(offset));
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x000393BC File Offset: 0x000375BC
		public long GetPreviousOffset()
		{
			IToken token;
			if (this.Dictionary.TryGet(NameToken.Prev, ref token))
			{
				NumericToken numericToken = token as NumericToken;
				if (numericToken != null)
				{
					return numericToken.Long;
				}
			}
			return -1L;
		}
	}
}
