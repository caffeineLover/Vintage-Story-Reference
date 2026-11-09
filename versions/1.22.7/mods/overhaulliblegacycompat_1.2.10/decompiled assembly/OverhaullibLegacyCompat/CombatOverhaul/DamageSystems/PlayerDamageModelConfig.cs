using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000126 RID: 294
	[NullableContext(1)]
	[Nullable(0)]
	public class PlayerDamageModelConfig
	{
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x00043700 File Offset: 0x00041900
		// (set) Token: 0x06000E1B RID: 3611 RVA: 0x00043708 File Offset: 0x00041908
		public PlayerDamageModelJson DamageModel { get; set; } = new PlayerDamageModelJson();

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x00043711 File Offset: 0x00041911
		// (set) Token: 0x06000E1D RID: 3613 RVA: 0x00043719 File Offset: 0x00041919
		public Dictionary<string, PlayerBodyPart> BodyParts { get; set; } = new Dictionary<string, PlayerBodyPart>();

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x00043722 File Offset: 0x00041922
		// (set) Token: 0x06000E1F RID: 3615 RVA: 0x0004372A File Offset: 0x0004192A
		public float SecondChanceCooldownSec { get; set; } = 300f;

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x00043733 File Offset: 0x00041933
		// (set) Token: 0x06000E21 RID: 3617 RVA: 0x0004373B File Offset: 0x0004193B
		public bool SecondChanceAvailable { get; set; } = true;

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x00043744 File Offset: 0x00041944
		// (set) Token: 0x06000E23 RID: 3619 RVA: 0x0004374C File Offset: 0x0004194C
		public float SecondChanceGracePeriodSec { get; set; } = 8f;
	}
}
