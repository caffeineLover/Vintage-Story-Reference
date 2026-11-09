using System;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000115 RID: 277
	public readonly struct DamageData
	{
		// Token: 0x06000D90 RID: 3472 RVA: 0x00041BCF File Offset: 0x0003FDCF
		public DamageData(EnumDamageType damageType, int tier, int armorPiercingTier)
		{
			this.DamageType = damageType;
			this.Tier = tier;
			this.ArmorPiercingTier = armorPiercingTier;
		}

		// Token: 0x04000698 RID: 1688
		public readonly EnumDamageType DamageType;

		// Token: 0x04000699 RID: 1689
		public readonly int Tier;

		// Token: 0x0400069A RID: 1690
		public readonly int ArmorPiercingTier;
	}
}
