using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements.Table
{
	// Token: 0x02000108 RID: 264
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class TableCellRenderingCommand
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x00016CE4 File Offset: 0x00014EE4
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x00016CEC File Offset: 0x00014EEC
		public TableCell Cell { get; set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x00016CF5 File Offset: 0x00014EF5
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x00016CFD File Offset: 0x00014EFD
		public SpacePlan Measurement { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x00016D06 File Offset: 0x00014F06
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x00016D0E File Offset: 0x00014F0E
		public Size Size { get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000920 RID: 2336 RVA: 0x00016D17 File Offset: 0x00014F17
		// (set) Token: 0x06000921 RID: 2337 RVA: 0x00016D1F File Offset: 0x00014F1F
		public Position Offset { get; set; }
	}
}
