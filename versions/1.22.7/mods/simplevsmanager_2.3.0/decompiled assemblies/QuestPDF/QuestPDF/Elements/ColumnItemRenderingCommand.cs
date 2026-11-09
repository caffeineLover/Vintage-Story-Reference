using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000B6 RID: 182
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ColumnItemRenderingCommand
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x0000DC17 File Offset: 0x0000BE17
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x0000DC1F File Offset: 0x0000BE1F
		public Element Element { get; set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0000DC28 File Offset: 0x0000BE28
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x0000DC30 File Offset: 0x0000BE30
		public SpacePlan Measurement { get; set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0000DC39 File Offset: 0x0000BE39
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x0000DC41 File Offset: 0x0000BE41
		public Position Offset { get; set; }
	}
}
