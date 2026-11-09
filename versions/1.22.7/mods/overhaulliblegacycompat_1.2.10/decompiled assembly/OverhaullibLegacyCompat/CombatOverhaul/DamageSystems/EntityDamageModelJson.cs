using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200011F RID: 287
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EntityDamageModelJson
	{
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x00042E2D File Offset: 0x0004102D
		// (set) Token: 0x06000DDE RID: 3550 RVA: 0x00042E35 File Offset: 0x00041035
		public float TorsoDamageMultiplier { get; set; } = 1f;

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x00042E3E File Offset: 0x0004103E
		// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x00042E46 File Offset: 0x00041046
		public float LimbsDamageMultiplier { get; set; } = 0.5f;

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x00042E4F File Offset: 0x0004104F
		// (set) Token: 0x06000DE2 RID: 3554 RVA: 0x00042E57 File Offset: 0x00041057
		public float HeadDamageMultiplier { get; set; } = 1.5f;

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x00042E60 File Offset: 0x00041060
		// (set) Token: 0x06000DE4 RID: 3556 RVA: 0x00042E68 File Offset: 0x00041068
		public float CriticalDamageMultiplier { get; set; } = 2f;

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000DE5 RID: 3557 RVA: 0x00042E71 File Offset: 0x00041071
		// (set) Token: 0x06000DE6 RID: 3558 RVA: 0x00042E79 File Offset: 0x00041079
		public float ResistantDamageMultiplier { get; set; }

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000DE7 RID: 3559 RVA: 0x00042E82 File Offset: 0x00041082
		// (set) Token: 0x06000DE8 RID: 3560 RVA: 0x00042E8A File Offset: 0x0004108A
		public Dictionary<string, float> DefaultResists { get; set; } = new Dictionary<string, float>();

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x00042E93 File Offset: 0x00041093
		// (set) Token: 0x06000DEA RID: 3562 RVA: 0x00042E9B File Offset: 0x0004109B
		public Dictionary<string, Dictionary<string, float>> ResistsForColliders { get; set; } = new Dictionary<string, Dictionary<string, float>>();

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000DEB RID: 3563 RVA: 0x00042EA4 File Offset: 0x000410A4
		// (set) Token: 0x06000DEC RID: 3564 RVA: 0x00042EAC File Offset: 0x000410AC
		public Dictionary<string, SoundEffectData> HitSounds { get; set; } = new Dictionary<string, SoundEffectData>();

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000DED RID: 3565 RVA: 0x00042EB5 File Offset: 0x000410B5
		// (set) Token: 0x06000DEE RID: 3566 RVA: 0x00042EBD File Offset: 0x000410BD
		public Dictionary<string, string> HitParticles { get; set; } = new Dictionary<string, string>();

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000DEF RID: 3567 RVA: 0x00042EC6 File Offset: 0x000410C6
		// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x00042ECE File Offset: 0x000410CE
		public bool ScaleParticlesCountWithDamage { get; set; } = true;
	}
}
