using System;
using UglyToad.PdfPig.Fonts.TrueType.Tables;

namespace UglyToad.PdfPig.Fonts.TrueType.Parser
{
	// Token: 0x0200005C RID: 92
	public class TableRegister
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000C8CF File Offset: 0x0000AACF
		public HeaderTable HeaderTable { get; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000C8D7 File Offset: 0x0000AAD7
		internal GlyphDataTable GlyphTable { get; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000C8DF File Offset: 0x0000AADF
		public HorizontalHeaderTable HorizontalHeaderTable { get; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000C8E7 File Offset: 0x0000AAE7
		public HorizontalMetricsTable HorizontalMetricsTable { get; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000C8EF File Offset: 0x0000AAEF
		public IndexToLocationTable IndexToLocationTable { get; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000C8F7 File Offset: 0x0000AAF7
		internal BasicMaximumProfileTable MaximumProfileTable { get; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000C8FF File Offset: 0x0000AAFF
		public NameTable NameTable { get; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000C907 File Offset: 0x0000AB07
		public PostScriptTable PostScriptTable { get; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000C90F File Offset: 0x0000AB0F
		public CMapTable CMapTable { get; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000C917 File Offset: 0x0000AB17
		internal KerningTable KerningTable { get; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000C91F File Offset: 0x0000AB1F
		public Os2Table Os2Table { get; }

		// Token: 0x0600026F RID: 623 RVA: 0x0000C928 File Offset: 0x0000AB28
		internal TableRegister(TableRegister.Builder builder)
		{
			if (builder == null)
			{
				throw new ArgumentNullException("builder");
			}
			HeaderTable headerTable = builder.HeaderTable;
			if (headerTable == null)
			{
				throw new ArgumentException("The builder did not contain the header table");
			}
			this.HeaderTable = headerTable;
			this.GlyphTable = builder.GlyphDataTable;
			HorizontalHeaderTable horizontalHeaderTable = builder.HorizontalHeaderTable;
			if (horizontalHeaderTable == null)
			{
				throw new ArgumentException("The builder did not contain the horizontal header table.");
			}
			this.HorizontalHeaderTable = horizontalHeaderTable;
			this.HorizontalMetricsTable = builder.HorizontalMetricsTable;
			this.IndexToLocationTable = builder.IndexToLocationTable;
			BasicMaximumProfileTable maximumProfileTable = builder.MaximumProfileTable;
			if (maximumProfileTable == null)
			{
				throw new ArgumentException("The builder did not contain the maximum profile table.");
			}
			this.MaximumProfileTable = maximumProfileTable;
			this.NameTable = builder.NameTable;
			this.PostScriptTable = builder.PostScriptTable;
			this.CMapTable = builder.CMapTable;
			this.KerningTable = builder.KerningTable;
			this.Os2Table = builder.Os2Table;
		}

		// Token: 0x020000D2 RID: 210
		internal class Builder
		{
			// Token: 0x17000253 RID: 595
			// (get) Token: 0x06000533 RID: 1331 RVA: 0x0001FA74 File Offset: 0x0001DC74
			// (set) Token: 0x06000534 RID: 1332 RVA: 0x0001FA7C File Offset: 0x0001DC7C
			public GlyphDataTable GlyphDataTable { get; set; }

			// Token: 0x17000254 RID: 596
			// (get) Token: 0x06000535 RID: 1333 RVA: 0x0001FA85 File Offset: 0x0001DC85
			// (set) Token: 0x06000536 RID: 1334 RVA: 0x0001FA8D File Offset: 0x0001DC8D
			public HeaderTable HeaderTable { get; set; }

			// Token: 0x17000255 RID: 597
			// (get) Token: 0x06000537 RID: 1335 RVA: 0x0001FA96 File Offset: 0x0001DC96
			// (set) Token: 0x06000538 RID: 1336 RVA: 0x0001FA9E File Offset: 0x0001DC9E
			public HorizontalHeaderTable HorizontalHeaderTable { get; set; }

			// Token: 0x17000256 RID: 598
			// (get) Token: 0x06000539 RID: 1337 RVA: 0x0001FAA7 File Offset: 0x0001DCA7
			// (set) Token: 0x0600053A RID: 1338 RVA: 0x0001FAAF File Offset: 0x0001DCAF
			public HorizontalMetricsTable HorizontalMetricsTable { get; set; }

			// Token: 0x17000257 RID: 599
			// (get) Token: 0x0600053B RID: 1339 RVA: 0x0001FAB8 File Offset: 0x0001DCB8
			// (set) Token: 0x0600053C RID: 1340 RVA: 0x0001FAC0 File Offset: 0x0001DCC0
			public IndexToLocationTable IndexToLocationTable { get; set; }

			// Token: 0x17000258 RID: 600
			// (get) Token: 0x0600053D RID: 1341 RVA: 0x0001FAC9 File Offset: 0x0001DCC9
			// (set) Token: 0x0600053E RID: 1342 RVA: 0x0001FAD1 File Offset: 0x0001DCD1
			public BasicMaximumProfileTable MaximumProfileTable { get; set; }

			// Token: 0x17000259 RID: 601
			// (get) Token: 0x0600053F RID: 1343 RVA: 0x0001FADA File Offset: 0x0001DCDA
			// (set) Token: 0x06000540 RID: 1344 RVA: 0x0001FAE2 File Offset: 0x0001DCE2
			public PostScriptTable PostScriptTable { get; set; }

			// Token: 0x1700025A RID: 602
			// (get) Token: 0x06000541 RID: 1345 RVA: 0x0001FAEB File Offset: 0x0001DCEB
			// (set) Token: 0x06000542 RID: 1346 RVA: 0x0001FAF3 File Offset: 0x0001DCF3
			public CMapTable CMapTable { get; set; }

			// Token: 0x1700025B RID: 603
			// (get) Token: 0x06000543 RID: 1347 RVA: 0x0001FAFC File Offset: 0x0001DCFC
			// (set) Token: 0x06000544 RID: 1348 RVA: 0x0001FB04 File Offset: 0x0001DD04
			public KerningTable KerningTable { get; set; }

			// Token: 0x1700025C RID: 604
			// (get) Token: 0x06000545 RID: 1349 RVA: 0x0001FB0D File Offset: 0x0001DD0D
			// (set) Token: 0x06000546 RID: 1350 RVA: 0x0001FB15 File Offset: 0x0001DD15
			public NameTable NameTable { get; set; }

			// Token: 0x1700025D RID: 605
			// (get) Token: 0x06000547 RID: 1351 RVA: 0x0001FB1E File Offset: 0x0001DD1E
			// (set) Token: 0x06000548 RID: 1352 RVA: 0x0001FB26 File Offset: 0x0001DD26
			public Os2Table Os2Table { get; set; }

			// Token: 0x06000549 RID: 1353 RVA: 0x0001FB2F File Offset: 0x0001DD2F
			public TableRegister Build()
			{
				return new TableRegister(this);
			}
		}
	}
}
