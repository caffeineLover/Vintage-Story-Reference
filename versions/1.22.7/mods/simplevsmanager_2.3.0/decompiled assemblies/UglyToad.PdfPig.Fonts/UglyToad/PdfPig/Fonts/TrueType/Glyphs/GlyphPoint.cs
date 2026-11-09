using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.TrueType.Glyphs
{
	// Token: 0x02000067 RID: 103
	internal struct GlyphPoint
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000D538 File Offset: 0x0000B738
		public readonly short X { get; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000D540 File Offset: 0x0000B740
		public readonly short Y { get; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000D548 File Offset: 0x0000B748
		public readonly bool IsOnCurve { get; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000D550 File Offset: 0x0000B750
		public readonly bool IsEndOfContour { get; }

		// Token: 0x06000299 RID: 665 RVA: 0x0000D558 File Offset: 0x0000B758
		public GlyphPoint(short x, short y, bool isOnCurve, bool isEndOfContour)
		{
			this.X = x;
			this.Y = y;
			this.IsOnCurve = isOnCurve;
			this.IsEndOfContour = isEndOfContour;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000D578 File Offset: 0x0000B778
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(10, 4);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted<short>(this.X);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<short>(this.Y);
			defaultInterpolatedStringHandler.AppendLiteral(") | ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsOnCurve);
			defaultInterpolatedStringHandler.AppendLiteral(" | ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsEndOfContour);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
