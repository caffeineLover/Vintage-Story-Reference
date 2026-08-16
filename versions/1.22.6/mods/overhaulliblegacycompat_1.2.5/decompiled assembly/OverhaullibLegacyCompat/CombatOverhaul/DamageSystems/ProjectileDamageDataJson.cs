using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000109 RID: 265
	[NullableContext(1)]
	[Nullable(0)]
	public class ProjectileDamageDataJson
	{
		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000D2F RID: 3375 RVA: 0x00040B76 File Offset: 0x0003ED76
		// (set) Token: 0x06000D30 RID: 3376 RVA: 0x00040B7E File Offset: 0x0003ED7E
		public string DamageType { get; set; } = "PiercingAttack";

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x00040B87 File Offset: 0x0003ED87
		// (set) Token: 0x06000D32 RID: 3378 RVA: 0x00040B8F File Offset: 0x0003ED8F
		public float Damage { get; set; }
	}
}
