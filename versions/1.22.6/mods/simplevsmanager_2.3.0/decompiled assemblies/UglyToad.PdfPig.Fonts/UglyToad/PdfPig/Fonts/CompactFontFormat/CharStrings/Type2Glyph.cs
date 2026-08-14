using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.CharStrings
{
	// Token: 0x020000A2 RID: 162
	internal class Type2Glyph
	{
		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0001B7C0 File Offset: 0x000199C0
		public IReadOnlyList<PdfSubpath> Path { get; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0001B7C8 File Offset: 0x000199C8
		public double? Width { get; }

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001B7D0 File Offset: 0x000199D0
		public Type2Glyph(IReadOnlyList<PdfSubpath> path, double? width)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			this.Path = path;
			this.Width = width;
		}
	}
}
