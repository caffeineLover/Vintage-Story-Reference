using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000120 RID: 288
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoundEffectData
	{
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x00042F4A File Offset: 0x0004114A
		// (set) Token: 0x06000DF3 RID: 3571 RVA: 0x00042F52 File Offset: 0x00041152
		public string Code { get; set; } = "";

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x00042F5B File Offset: 0x0004115B
		// (set) Token: 0x06000DF5 RID: 3573 RVA: 0x00042F63 File Offset: 0x00041163
		public bool RandomizePitch { get; set; }

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x00042F6C File Offset: 0x0004116C
		// (set) Token: 0x06000DF7 RID: 3575 RVA: 0x00042F74 File Offset: 0x00041174
		public float Range { get; set; } = 32f;

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x00042F7D File Offset: 0x0004117D
		// (set) Token: 0x06000DF9 RID: 3577 RVA: 0x00042F85 File Offset: 0x00041185
		public float Volume { get; set; } = 1f;
	}
}
