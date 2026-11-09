using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200005B RID: 91
	[NullableContext(1)]
	[Nullable(0)]
	public class PageElementLocation
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000224 RID: 548 RVA: 0x000055A9 File Offset: 0x000037A9
		// (set) Token: 0x06000225 RID: 549 RVA: 0x000055B1 File Offset: 0x000037B1
		public string Id { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000226 RID: 550 RVA: 0x000055BA File Offset: 0x000037BA
		// (set) Token: 0x06000227 RID: 551 RVA: 0x000055C2 File Offset: 0x000037C2
		public int PageNumber { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000228 RID: 552 RVA: 0x000055CB File Offset: 0x000037CB
		// (set) Token: 0x06000229 RID: 553 RVA: 0x000055D3 File Offset: 0x000037D3
		public float Width { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600022A RID: 554 RVA: 0x000055DC File Offset: 0x000037DC
		// (set) Token: 0x0600022B RID: 555 RVA: 0x000055E4 File Offset: 0x000037E4
		public float Height { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600022C RID: 556 RVA: 0x000055ED File Offset: 0x000037ED
		// (set) Token: 0x0600022D RID: 557 RVA: 0x000055F5 File Offset: 0x000037F5
		public float X { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600022E RID: 558 RVA: 0x000055FE File Offset: 0x000037FE
		// (set) Token: 0x0600022F RID: 559 RVA: 0x00005606 File Offset: 0x00003806
		public float Y { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000560F File Offset: 0x0000380F
		// (set) Token: 0x06000231 RID: 561 RVA: 0x00005617 File Offset: 0x00003817
		public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;
	}
}
