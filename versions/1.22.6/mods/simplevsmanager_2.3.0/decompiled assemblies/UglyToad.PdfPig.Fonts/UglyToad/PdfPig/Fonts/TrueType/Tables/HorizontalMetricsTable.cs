using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.TrueType.Glyphs;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x0200003A RID: 58
	public class HorizontalMetricsTable : ITrueTypeTable, IWriteable
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00009874 File Offset: 0x00007A74
		public string Tag
		{
			get
			{
				return "hmtx";
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000987B File Offset: 0x00007A7B
		public TrueTypeHeaderTable DirectoryTable { get; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00009883 File Offset: 0x00007A83
		public IReadOnlyList<HorizontalMetric> HorizontalMetrics { get; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000988B File Offset: 0x00007A8B
		public IReadOnlyList<short> AdditionalLeftSideBearings { get; }

		// Token: 0x060001A2 RID: 418 RVA: 0x00009893 File Offset: 0x00007A93
		public HorizontalMetricsTable(TrueTypeHeaderTable directoryTable, IReadOnlyList<HorizontalMetric> horizontalMetrics, IReadOnlyList<short> additionalLeftSideBearings)
		{
			this.DirectoryTable = directoryTable;
			if (horizontalMetrics == null)
			{
				throw new ArgumentNullException("horizontalMetrics");
			}
			this.HorizontalMetrics = horizontalMetrics;
			if (additionalLeftSideBearings == null)
			{
				throw new ArgumentNullException("additionalLeftSideBearings");
			}
			this.AdditionalLeftSideBearings = additionalLeftSideBearings;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000098D0 File Offset: 0x00007AD0
		public ushort GetAdvanceWidth(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index cannot be less than zero.");
			}
			if (index < this.HorizontalMetrics.Count)
			{
				return this.HorizontalMetrics[index].AdvanceWidth;
			}
			return this.HorizontalMetrics[this.HorizontalMetrics.Count - 1].AdvanceWidth;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00009934 File Offset: 0x00007B34
		public void Write(Stream stream)
		{
			for (int i = 0; i < this.HorizontalMetrics.Count; i++)
			{
				HorizontalMetric horizontalMetric = this.HorizontalMetrics[i];
				WritingExtensions.WriteUShort(stream, horizontalMetric.AdvanceWidth);
				WritingExtensions.WriteShort(stream, horizontalMetric.LeftSideBearing);
			}
			for (int j = 0; j < this.AdditionalLeftSideBearings.Count; j++)
			{
				short num = this.AdditionalLeftSideBearings[j];
				WritingExtensions.WriteShort(stream, num);
			}
		}
	}
}
