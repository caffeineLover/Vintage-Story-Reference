using System;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements.Table
{
	// Token: 0x02000107 RID: 263
	internal sealed class TableCell : ContainerElement, ITableCellContainer, IContainer
	{
		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x00016C46 File Offset: 0x00014E46
		// (set) Token: 0x0600090A RID: 2314 RVA: 0x00016C4E File Offset: 0x00014E4E
		public int Row { get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x00016C57 File Offset: 0x00014E57
		// (set) Token: 0x0600090C RID: 2316 RVA: 0x00016C5F File Offset: 0x00014E5F
		public int RowSpan { get; set; } = 1;

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x00016C68 File Offset: 0x00014E68
		// (set) Token: 0x0600090E RID: 2318 RVA: 0x00016C70 File Offset: 0x00014E70
		public int Column { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x00016C79 File Offset: 0x00014E79
		// (set) Token: 0x06000910 RID: 2320 RVA: 0x00016C81 File Offset: 0x00014E81
		public int ColumnSpan { get; set; } = 1;

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x00016C8A File Offset: 0x00014E8A
		// (set) Token: 0x06000912 RID: 2322 RVA: 0x00016C92 File Offset: 0x00014E92
		public int ZIndex { get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x00016C9B File Offset: 0x00014E9B
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x00016CA3 File Offset: 0x00014EA3
		public bool IsSemanticHorizontalHeader { get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x00016CAC File Offset: 0x00014EAC
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x00016CB4 File Offset: 0x00014EB4
		public int SemanticNodeId { get; set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x00016CBD File Offset: 0x00014EBD
		// (set) Token: 0x06000918 RID: 2328 RVA: 0x00016CC5 File Offset: 0x00014EC5
		public bool IsRendered { get; set; }
	}
}
