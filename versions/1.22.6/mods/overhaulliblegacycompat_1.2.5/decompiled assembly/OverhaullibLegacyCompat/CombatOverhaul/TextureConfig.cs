using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul
{
	// Token: 0x0200001C RID: 28
	[NullableContext(1)]
	[Nullable(0)]
	public class TextureConfig
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001FD RID: 509 RVA: 0x00008694 File Offset: 0x00006894
		// (set) Token: 0x060001FE RID: 510 RVA: 0x0000869C File Offset: 0x0000689C
		public string Code { get; set; } = "";

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001FF RID: 511 RVA: 0x000086A5 File Offset: 0x000068A5
		// (set) Token: 0x06000200 RID: 512 RVA: 0x000086AD File Offset: 0x000068AD
		public string Attribute { get; set; } = "";

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000201 RID: 513 RVA: 0x000086B6 File Offset: 0x000068B6
		// (set) Token: 0x06000202 RID: 514 RVA: 0x000086BE File Offset: 0x000068BE
		public string Default { get; set; } = "";

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000203 RID: 515 RVA: 0x000086C7 File Offset: 0x000068C7
		// (set) Token: 0x06000204 RID: 516 RVA: 0x000086CF File Offset: 0x000068CF
		public string[] HandbookValues { get; set; } = Array.Empty<string>();
	}
}
