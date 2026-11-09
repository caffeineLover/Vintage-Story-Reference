using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.PdfFonts
{
	// Token: 0x0200008A RID: 138
	internal readonly struct CharacterIdentifierSystemInfo
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000FB99 File Offset: 0x0000DD99
		public string Registry { get; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000FBA1 File Offset: 0x0000DDA1
		public string Ordering { get; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000FBA9 File Offset: 0x0000DDA9
		public int Supplement { get; }

		// Token: 0x06000233 RID: 563 RVA: 0x0000FBB1 File Offset: 0x0000DDB1
		public CharacterIdentifierSystemInfo(string registry, string ordering, int supplement)
		{
			this.Registry = registry;
			this.Ordering = ordering;
			this.Supplement = supplement;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000FBC8 File Offset: 0x0000DDC8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted(this.Registry);
			defaultInterpolatedStringHandler.AppendLiteral("-");
			defaultInterpolatedStringHandler.AppendFormatted(this.Ordering);
			defaultInterpolatedStringHandler.AppendLiteral("-");
			defaultInterpolatedStringHandler.AppendFormatted(this.Supplement.ToString(CultureInfo.InvariantCulture));
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
