using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C7 RID: 199
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AutoAnimationConfig
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x00036B7B File Offset: 0x00034D7B
		// (set) Token: 0x06000A4B RID: 2635 RVA: 0x00036B83 File Offset: 0x00034D83
		public string Animation { get; set; } = "";

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x00036B8C File Offset: 0x00034D8C
		// (set) Token: 0x06000A4D RID: 2637 RVA: 0x00036B94 File Offset: 0x00034D94
		public bool StopWhenStuck { get; set; } = true;

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x00036B9D File Offset: 0x00034D9D
		// (set) Token: 0x06000A4F RID: 2639 RVA: 0x00036BA5 File Offset: 0x00034DA5
		public float MinimumSpeed { get; set; }
	}
}
