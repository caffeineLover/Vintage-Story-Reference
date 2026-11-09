using System;

namespace QuestPDF.Elements.Table
{
	// Token: 0x02000109 RID: 265
	internal sealed class TableColumnDefinition
	{
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x00016D30 File Offset: 0x00014F30
		public float ConstantSize { get; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x00016D38 File Offset: 0x00014F38
		public float RelativeSize { get; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x00016D40 File Offset: 0x00014F40
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x00016D48 File Offset: 0x00014F48
		internal float Width { get; set; }

		// Token: 0x06000927 RID: 2343 RVA: 0x00016D51 File Offset: 0x00014F51
		public TableColumnDefinition(float constantSize, float relativeSize)
		{
			this.ConstantSize = constantSize;
			this.RelativeSize = relativeSize;
		}
	}
}
