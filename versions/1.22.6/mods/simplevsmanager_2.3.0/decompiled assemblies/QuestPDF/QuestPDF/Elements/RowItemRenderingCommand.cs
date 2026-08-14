using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000E5 RID: 229
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class RowItemRenderingCommand
	{
		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x00011C9E File Offset: 0x0000FE9E
		// (set) Token: 0x0600076F RID: 1903 RVA: 0x00011CA6 File Offset: 0x0000FEA6
		public RowItem RowItem { get; set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x00011CAF File Offset: 0x0000FEAF
		// (set) Token: 0x06000771 RID: 1905 RVA: 0x00011CB7 File Offset: 0x0000FEB7
		public SpacePlan Measurement { get; set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x00011CC0 File Offset: 0x0000FEC0
		// (set) Token: 0x06000773 RID: 1907 RVA: 0x00011CC8 File Offset: 0x0000FEC8
		public Size Size { get; set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x00011CD1 File Offset: 0x0000FED1
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x00011CD9 File Offset: 0x0000FED9
		public Position Offset { get; set; }
	}
}
