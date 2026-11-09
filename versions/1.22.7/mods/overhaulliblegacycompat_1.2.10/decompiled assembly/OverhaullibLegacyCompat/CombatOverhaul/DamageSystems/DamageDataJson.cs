using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000113 RID: 275
	[NullableContext(1)]
	[Nullable(0)]
	public class DamageDataJson
	{
		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000D80 RID: 3456 RVA: 0x00041B30 File Offset: 0x0003FD30
		// (set) Token: 0x06000D81 RID: 3457 RVA: 0x00041B38 File Offset: 0x0003FD38
		public string DamageType { get; set; } = "PiercingAttack";

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x00041B41 File Offset: 0x0003FD41
		// (set) Token: 0x06000D83 RID: 3459 RVA: 0x00041B4A File Offset: 0x0003FD4A
		[Obsolete]
		public float Strength
		{
			get
			{
				return (float)this.Tier;
			}
			set
			{
				this.Tier = (int)value;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x00041B54 File Offset: 0x0003FD54
		// (set) Token: 0x06000D85 RID: 3461 RVA: 0x00041B5C File Offset: 0x0003FD5C
		public int Tier { get; set; }

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x00041B65 File Offset: 0x0003FD65
		// (set) Token: 0x06000D87 RID: 3463 RVA: 0x00041B6D File Offset: 0x0003FD6D
		public float Damage { get; set; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x00041B76 File Offset: 0x0003FD76
		// (set) Token: 0x06000D89 RID: 3465 RVA: 0x00041B7E File Offset: 0x0003FD7E
		public int ArmorPiercingTier { get; set; }
	}
}
