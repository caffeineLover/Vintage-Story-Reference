using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000BF RID: 191
	internal sealed class DecorationElementLayout
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0000E453 File Offset: 0x0000C653
		// (set) Token: 0x060005B3 RID: 1459 RVA: 0x0000E45B File Offset: 0x0000C65B
		public DecorationElementLayout.ItemCommand Before { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x0000E464 File Offset: 0x0000C664
		// (set) Token: 0x060005B5 RID: 1461 RVA: 0x0000E46C File Offset: 0x0000C66C
		public DecorationElementLayout.ItemCommand Content { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0000E475 File Offset: 0x0000C675
		// (set) Token: 0x060005B7 RID: 1463 RVA: 0x0000E47D File Offset: 0x0000C67D
		public DecorationElementLayout.ItemCommand After { get; set; }

		// Token: 0x020001A6 RID: 422
		public struct ItemCommand
		{
			// Token: 0x0400063F RID: 1599
			[Nullable(1)]
			public Element Element;

			// Token: 0x04000640 RID: 1600
			public SpacePlan Measurement;

			// Token: 0x04000641 RID: 1601
			public Position Offset;
		}
	}
}
