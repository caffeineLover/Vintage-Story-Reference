using System;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000CE RID: 206
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class GridElement
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x0000F849 File Offset: 0x0000DA49
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x0000F851 File Offset: 0x0000DA51
		public int Columns { get; set; } = 1;

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0000F85A File Offset: 0x0000DA5A
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x0000F862 File Offset: 0x0000DA62
		public Element Child { get; set; }
	}
}
