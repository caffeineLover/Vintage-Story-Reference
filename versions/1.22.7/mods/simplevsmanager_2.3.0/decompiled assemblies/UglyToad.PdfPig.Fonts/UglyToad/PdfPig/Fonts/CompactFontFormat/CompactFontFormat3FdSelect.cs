using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x02000091 RID: 145
	internal class CompactFontFormat3FdSelect : ICompactFontFormatFdSelect
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00017308 File Offset: 0x00015508
		public RegistryOrderingSupplement RegistryOrderingSupplement { get; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00017310 File Offset: 0x00015510
		public IReadOnlyList<CompactFontFormat3FdSelect.Range3> Ranges { get; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00017318 File Offset: 0x00015518
		public int Sentinel { get; }

		// Token: 0x0600034C RID: 844 RVA: 0x00017320 File Offset: 0x00015520
		public CompactFontFormat3FdSelect(RegistryOrderingSupplement registryOrderingSupplement, IReadOnlyList<CompactFontFormat3FdSelect.Range3> ranges, int sentinel)
		{
			if (registryOrderingSupplement == null)
			{
				throw new ArgumentNullException("registryOrderingSupplement");
			}
			this.RegistryOrderingSupplement = registryOrderingSupplement;
			if (ranges == null)
			{
				throw new ArgumentNullException("ranges");
			}
			this.Ranges = ranges;
			this.Sentinel = sentinel;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0001735C File Offset: 0x0001555C
		public int GetFontDictionaryIndex(int glyphId)
		{
			for (int i = 0; i < this.Ranges.Count; i++)
			{
				if (this.Ranges[i].First <= glyphId)
				{
					if (i + 1 < this.Ranges.Count)
					{
						if (this.Ranges[i + 1].First > glyphId)
						{
							return this.Ranges[i].FontDictionary;
						}
					}
					else
					{
						if (this.Sentinel > glyphId)
						{
							return this.Ranges[i].FontDictionary;
						}
						return -1;
					}
				}
			}
			return 0;
		}

		// Token: 0x020000DA RID: 218
		internal struct Range3
		{
			// Token: 0x17000267 RID: 615
			// (get) Token: 0x0600056F RID: 1391 RVA: 0x000202F4 File Offset: 0x0001E4F4
			public readonly int First { get; }

			// Token: 0x17000268 RID: 616
			// (get) Token: 0x06000570 RID: 1392 RVA: 0x000202FC File Offset: 0x0001E4FC
			public readonly int FontDictionary { get; }

			// Token: 0x06000571 RID: 1393 RVA: 0x00020304 File Offset: 0x0001E504
			public Range3(int first, int fontDictionary)
			{
				this.First = first;
				this.FontDictionary = fontDictionary;
			}

			// Token: 0x06000572 RID: 1394 RVA: 0x00020314 File Offset: 0x0001E514
			public override string ToString()
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(20, 2);
				defaultInterpolatedStringHandler.AppendLiteral("First ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.First);
				defaultInterpolatedStringHandler.AppendLiteral(", Dictionary ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.FontDictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}
	}
}
