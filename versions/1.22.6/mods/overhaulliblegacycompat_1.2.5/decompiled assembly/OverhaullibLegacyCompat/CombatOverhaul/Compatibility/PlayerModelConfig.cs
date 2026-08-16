using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Colliders;
using CombatOverhaul.DamageSystems;

namespace CombatOverhaul.Compatibility
{
	// Token: 0x02000156 RID: 342
	[NullableContext(2)]
	[Nullable(0)]
	public class PlayerModelConfig
	{
		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x0600114F RID: 4431 RVA: 0x00053531 File Offset: 0x00051731
		// (set) Token: 0x06001150 RID: 4432 RVA: 0x00053539 File Offset: 0x00051739
		[Nullable(1)]
		public string Domain { [NullableContext(1)] get; [NullableContext(1)] set; } = "";

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x00053542 File Offset: 0x00051742
		// (set) Token: 0x06001152 RID: 4434 RVA: 0x0005354A File Offset: 0x0005174A
		public CollidersConfig Colliders { get; set; }

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x00053553 File Offset: 0x00051753
		// (set) Token: 0x06001154 RID: 4436 RVA: 0x0005355B File Offset: 0x0005175B
		public PlayerDamageModelConfig DamageModel { get; set; }
	}
}
