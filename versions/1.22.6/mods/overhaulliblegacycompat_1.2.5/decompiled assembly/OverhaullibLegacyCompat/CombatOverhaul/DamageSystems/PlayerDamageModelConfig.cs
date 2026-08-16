using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200011B RID: 283
	[NullableContext(1)]
	[Nullable(0)]
	public class PlayerDamageModelConfig
	{
		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x000426DC File Offset: 0x000408DC
		// (set) Token: 0x06000DBF RID: 3519 RVA: 0x000426E4 File Offset: 0x000408E4
		public PlayerDamageModelJson DamageModel { get; set; } = new PlayerDamageModelJson();

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x000426ED File Offset: 0x000408ED
		// (set) Token: 0x06000DC1 RID: 3521 RVA: 0x000426F5 File Offset: 0x000408F5
		public Dictionary<string, PlayerBodyPart> BodyParts { get; set; } = new Dictionary<string, PlayerBodyPart>();

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x000426FE File Offset: 0x000408FE
		// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x00042706 File Offset: 0x00040906
		public float SecondChanceCooldownSec { get; set; } = 300f;

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x0004270F File Offset: 0x0004090F
		// (set) Token: 0x06000DC5 RID: 3525 RVA: 0x00042717 File Offset: 0x00040917
		public bool SecondChanceAvailable { get; set; } = true;

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x00042720 File Offset: 0x00040920
		// (set) Token: 0x06000DC7 RID: 3527 RVA: 0x00042728 File Offset: 0x00040928
		public float SecondChanceGracePeriodSec { get; set; } = 8f;
	}
}
