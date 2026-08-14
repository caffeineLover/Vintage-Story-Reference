using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables.Kerning
{
	// Token: 0x02000049 RID: 73
	public struct KernPair
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000A972 File Offset: 0x00008B72
		public readonly int LeftGlyphIndex { get; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000A97A File Offset: 0x00008B7A
		public readonly int RightGlyphIndex { get; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000A982 File Offset: 0x00008B82
		public readonly short Value { get; }

		// Token: 0x06000217 RID: 535 RVA: 0x0000A98A File Offset: 0x00008B8A
		public KernPair(int leftGlyphIndex, int rightGlyphIndex, short value)
		{
			this.LeftGlyphIndex = leftGlyphIndex;
			this.RightGlyphIndex = rightGlyphIndex;
			this.Value = value;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000A9A4 File Offset: 0x00008BA4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(24, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Left: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.LeftGlyphIndex);
			defaultInterpolatedStringHandler.AppendLiteral(", Right: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.RightGlyphIndex);
			defaultInterpolatedStringHandler.AppendLiteral(", Value ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(this.Value);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
