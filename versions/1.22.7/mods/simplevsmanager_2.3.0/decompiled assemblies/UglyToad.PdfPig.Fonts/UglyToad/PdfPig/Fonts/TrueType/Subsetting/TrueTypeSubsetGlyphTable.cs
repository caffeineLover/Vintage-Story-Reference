using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Fonts.TrueType.Glyphs;

namespace UglyToad.PdfPig.Fonts.TrueType.Subsetting
{
	// Token: 0x02000053 RID: 83
	internal class TrueTypeSubsetGlyphTable
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000B7DF File Offset: 0x000099DF
		public byte[] Bytes { get; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000B7E7 File Offset: 0x000099E7
		public uint[] GlyphOffsets { get; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000B7EF File Offset: 0x000099EF
		public HorizontalMetric[] HorizontalMetrics { get; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000B7F7 File Offset: 0x000099F7
		public ushort GlyphCount
		{
			get
			{
				return (ushort)(this.GlyphOffsets.Length - 1);
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000B804 File Offset: 0x00009A04
		public TrueTypeSubsetGlyphTable(byte[] bytes, uint[] glyphOffsets, HorizontalMetric[] horizontalMetrics)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			this.Bytes = bytes;
			if (glyphOffsets == null)
			{
				throw new ArgumentNullException("glyphOffsets");
			}
			this.GlyphOffsets = glyphOffsets;
			if (horizontalMetrics == null)
			{
				throw new ArgumentNullException("horizontalMetrics");
			}
			this.HorizontalMetrics = horizontalMetrics;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000B85C File Offset: 0x00009A5C
		public long[] OffsetsAsLongs()
		{
			long[] array = new long[this.GlyphOffsets.Length];
			for (int i = 0; i < this.GlyphOffsets.Length; i++)
			{
				array[i] = (long)((ulong)this.GlyphOffsets[i]);
			}
			return array;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000B898 File Offset: 0x00009A98
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(24, 2);
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(this.GlyphCount);
			defaultInterpolatedStringHandler.AppendLiteral(" glyphs. Data is ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Bytes.Length);
			defaultInterpolatedStringHandler.AppendLiteral(" bytes.");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
