using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000115 RID: 277
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoundEffectData
	{
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x00041F26 File Offset: 0x00040126
		// (set) Token: 0x06000D97 RID: 3479 RVA: 0x00041F2E File Offset: 0x0004012E
		public string Code { get; set; } = "";

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x00041F37 File Offset: 0x00040137
		// (set) Token: 0x06000D99 RID: 3481 RVA: 0x00041F3F File Offset: 0x0004013F
		public bool RandomizePitch { get; set; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x00041F48 File Offset: 0x00040148
		// (set) Token: 0x06000D9B RID: 3483 RVA: 0x00041F50 File Offset: 0x00040150
		public float Range { get; set; } = 32f;

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x00041F59 File Offset: 0x00040159
		// (set) Token: 0x06000D9D RID: 3485 RVA: 0x00041F61 File Offset: 0x00040161
		public float Volume { get; set; } = 1f;
	}
}
