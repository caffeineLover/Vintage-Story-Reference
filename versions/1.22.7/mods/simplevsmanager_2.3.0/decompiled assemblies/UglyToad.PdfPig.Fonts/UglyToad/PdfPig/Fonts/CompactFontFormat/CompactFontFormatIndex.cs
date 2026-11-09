using System;
using System.Collections;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x0200008C RID: 140
	internal class CompactFontFormatIndex : IReadOnlyList<IReadOnlyList<byte>>, IEnumerable<IReadOnlyList<byte>>, IEnumerable, IReadOnlyCollection<IReadOnlyList<byte>>
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000333 RID: 819 RVA: 0x000169BE File Offset: 0x00014BBE
		public int Count
		{
			get
			{
				return this.bytes.Count;
			}
		}

		// Token: 0x1700017C RID: 380
		public IReadOnlyList<byte> this[int index]
		{
			get
			{
				return this.bytes[index];
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000335 RID: 821 RVA: 0x000169D9 File Offset: 0x00014BD9
		public static CompactFontFormatIndex None { get; } = new CompactFontFormatIndex(new byte[0][]);

		// Token: 0x06000336 RID: 822 RVA: 0x000169E0 File Offset: 0x00014BE0
		public CompactFontFormatIndex(byte[][] bytes)
		{
			this.bytes = (bytes ?? EmptyArray<IReadOnlyList<byte>>.Instance);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00016A05 File Offset: 0x00014C05
		public IEnumerator<IReadOnlyList<byte>> GetEnumerator()
		{
			return this.bytes.GetEnumerator();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00016A12 File Offset: 0x00014C12
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000315 RID: 789
		private readonly IReadOnlyList<IReadOnlyList<byte>> bytes;
	}
}
