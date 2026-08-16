using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000108 RID: 264
	[NullableContext(1)]
	[Nullable(0)]
	public class DamageDataJson
	{
		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x00040B0C File Offset: 0x0003ED0C
		// (set) Token: 0x06000D25 RID: 3365 RVA: 0x00040B14 File Offset: 0x0003ED14
		public string DamageType { get; set; } = "PiercingAttack";

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x00040B1D File Offset: 0x0003ED1D
		// (set) Token: 0x06000D27 RID: 3367 RVA: 0x00040B26 File Offset: 0x0003ED26
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

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x00040B30 File Offset: 0x0003ED30
		// (set) Token: 0x06000D29 RID: 3369 RVA: 0x00040B38 File Offset: 0x0003ED38
		public int Tier { get; set; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x00040B41 File Offset: 0x0003ED41
		// (set) Token: 0x06000D2B RID: 3371 RVA: 0x00040B49 File Offset: 0x0003ED49
		public float Damage { get; set; }

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x00040B52 File Offset: 0x0003ED52
		// (set) Token: 0x06000D2D RID: 3373 RVA: 0x00040B5A File Offset: 0x0003ED5A
		public int ArmorPiercingTier { get; set; }
	}
}
