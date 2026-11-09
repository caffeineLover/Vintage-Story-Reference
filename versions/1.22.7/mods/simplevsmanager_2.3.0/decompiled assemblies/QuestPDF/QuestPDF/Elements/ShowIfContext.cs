using System;

namespace QuestPDF.Elements
{
	// Token: 0x020000ED RID: 237
	public sealed class ShowIfContext
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00012E2E File Offset: 0x0001102E
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x00012E36 File Offset: 0x00011036
		public int PageNumber { get; internal set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x00012E3F File Offset: 0x0001103F
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x00012E47 File Offset: 0x00011047
		public int? TotalPages { get; internal set; }
	}
}
