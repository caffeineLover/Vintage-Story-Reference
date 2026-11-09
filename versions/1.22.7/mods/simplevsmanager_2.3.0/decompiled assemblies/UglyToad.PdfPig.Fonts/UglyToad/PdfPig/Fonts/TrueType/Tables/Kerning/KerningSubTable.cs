using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables.Kerning
{
	// Token: 0x02000048 RID: 72
	internal class KerningSubTable
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000A93D File Offset: 0x00008B3D
		public int Version { get; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000A945 File Offset: 0x00008B45
		public KernCoverage Coverage { get; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000A94D File Offset: 0x00008B4D
		public IReadOnlyList<KernPair> Pairs { get; }

		// Token: 0x06000213 RID: 531 RVA: 0x0000A955 File Offset: 0x00008B55
		public KerningSubTable(int version, KernCoverage coverage, IReadOnlyList<KernPair> pairs)
		{
			this.Version = version;
			this.Coverage = coverage;
			this.Pairs = pairs;
		}
	}
}
