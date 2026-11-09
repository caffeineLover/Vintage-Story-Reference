using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Companion
{
	// Token: 0x0200012D RID: 301
	internal sealed class PageSnapshotIndex
	{
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x0001B97F File Offset: 0x00019B7F
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x0001B987 File Offset: 0x00019B87
		public int PageIndex { get; set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x0001B990 File Offset: 0x00019B90
		// (set) Token: 0x06000AAD RID: 2733 RVA: 0x0001B998 File Offset: 0x00019B98
		public int ZoomLevel { get; set; }

		// Token: 0x06000AAE RID: 2734 RVA: 0x0001B9A4 File Offset: 0x00019BA4
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.ZoomLevel);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.PageIndex);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
