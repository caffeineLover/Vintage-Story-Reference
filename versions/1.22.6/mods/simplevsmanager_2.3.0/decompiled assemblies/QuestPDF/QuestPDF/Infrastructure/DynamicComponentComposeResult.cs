using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200004D RID: 77
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DynamicComponentComposeResult
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00004FFF File Offset: 0x000031FF
		// (set) Token: 0x060001DF RID: 479 RVA: 0x00005007 File Offset: 0x00003207
		public IElement Content { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00005010 File Offset: 0x00003210
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00005018 File Offset: 0x00003218
		public bool HasMoreContent { get; set; }
	}
}
