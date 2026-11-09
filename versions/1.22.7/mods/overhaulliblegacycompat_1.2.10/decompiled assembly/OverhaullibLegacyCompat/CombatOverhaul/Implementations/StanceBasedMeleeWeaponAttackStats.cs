using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.MeleeSystems;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000091 RID: 145
	[NullableContext(2)]
	[Nullable(0)]
	public class StanceBasedMeleeWeaponAttackStats : MeleeAttackStats
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0002DA87 File Offset: 0x0002BC87
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x0002DA8F File Offset: 0x0002BC8F
		public DamageBlockJson Parry { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0002DA98 File Offset: 0x0002BC98
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x0002DAA0 File Offset: 0x0002BCA0
		public MeleeAttackStats HandleAttack { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0002DAA9 File Offset: 0x0002BCA9
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x0002DAB1 File Offset: 0x0002BCB1
		public string AttackHitSound { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x0002DABA File Offset: 0x0002BCBA
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x0002DAC2 File Offset: 0x0002BCC2
		public string HandleHitSound { get; set; }
	}
}
