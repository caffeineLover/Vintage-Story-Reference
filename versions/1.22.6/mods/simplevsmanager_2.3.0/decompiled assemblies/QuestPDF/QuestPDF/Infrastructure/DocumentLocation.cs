using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200005A RID: 90
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class DocumentLocation
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000554C File Offset: 0x0000374C
		// (set) Token: 0x0600021B RID: 539 RVA: 0x00005554 File Offset: 0x00003754
		public int DocumentId { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000555D File Offset: 0x0000375D
		// (set) Token: 0x0600021D RID: 541 RVA: 0x00005565 File Offset: 0x00003765
		public string Name { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000556E File Offset: 0x0000376E
		// (set) Token: 0x0600021F RID: 543 RVA: 0x00005576 File Offset: 0x00003776
		public int PageStart { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000557F File Offset: 0x0000377F
		// (set) Token: 0x06000221 RID: 545 RVA: 0x00005587 File Offset: 0x00003787
		public int PageEnd { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00005590 File Offset: 0x00003790
		public int Length
		{
			get
			{
				return this.PageEnd - this.PageStart + 1;
			}
		}
	}
}
