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
		// (get) Token: 0x060001FD RID: 509 RVA: 0x000086D4 File Offset: 0x000068D4
		// (set) Token: 0x060001FE RID: 510 RVA: 0x000086DC File Offset: 0x000068DC
		public string Code { get; set; } = "";

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001FF RID: 511 RVA: 0x000086E5 File Offset: 0x000068E5
		// (set) Token: 0x06000200 RID: 512 RVA: 0x000086ED File Offset: 0x000068ED
		public string Attribute { get; set; } = "";

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000201 RID: 513 RVA: 0x000086F6 File Offset: 0x000068F6
		// (set) Token: 0x06000202 RID: 514 RVA: 0x000086FE File Offset: 0x000068FE
		public string Default { get; set; } = "";

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00008707 File Offset: 0x00006907
		// (set) Token: 0x06000204 RID: 516 RVA: 0x0000870F File Offset: 0x0000690F
		public string[] HandbookValues { get; set; } = Array.Empty<string>();
	}
}
