using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Colliders;
using CombatOverhaul.DamageSystems;

namespace CombatOverhaul.Compatibility
{
	// Token: 0x02000162 RID: 354
	[NullableContext(2)]
	[Nullable(0)]
	public class PlayerModelConfig
	{
		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x00054E39 File Offset: 0x00053039
		// (set) Token: 0x060011C4 RID: 4548 RVA: 0x00054E41 File Offset: 0x00053041
		[Nullable(1)]
		public string Domain { [NullableContext(1)] get; [NullableContext(1)] set; } = "";

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x00054E4A File Offset: 0x0005304A
		// (set) Token: 0x060011C6 RID: 4550 RVA: 0x00054E52 File Offset: 0x00053052
		public CollidersConfig Colliders { get; set; }

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060011C7 RID: 4551 RVA: 0x00054E5B File Offset: 0x0005305B
		// (set) Token: 0x060011C8 RID: 4552 RVA: 0x00054E63 File Offset: 0x00053063
		public PlayerDamageModelConfig DamageModel { get; set; }
	}
}
