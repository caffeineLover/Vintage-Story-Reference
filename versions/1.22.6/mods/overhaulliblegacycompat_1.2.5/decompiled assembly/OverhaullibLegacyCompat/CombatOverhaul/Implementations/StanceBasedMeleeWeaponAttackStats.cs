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
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0002DA4F File Offset: 0x0002BC4F
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x0002DA57 File Offset: 0x0002BC57
		public DamageBlockJson Parry { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0002DA60 File Offset: 0x0002BC60
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x0002DA68 File Offset: 0x0002BC68
		public MeleeAttackStats HandleAttack { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0002DA71 File Offset: 0x0002BC71
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x0002DA79 File Offset: 0x0002BC79
		public string AttackHitSound { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x0002DA82 File Offset: 0x0002BC82
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x0002DA8A File Offset: 0x0002BC8A
		public string HandleHitSound { get; set; }
	}
}
