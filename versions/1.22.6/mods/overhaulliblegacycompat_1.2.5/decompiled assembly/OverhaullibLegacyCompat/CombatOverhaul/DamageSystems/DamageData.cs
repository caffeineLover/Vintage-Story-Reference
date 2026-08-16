using System;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200010A RID: 266
	public readonly struct DamageData
	{
		// Token: 0x06000D34 RID: 3380 RVA: 0x00040BAB File Offset: 0x0003EDAB
		public DamageData(EnumDamageType damageType, int tier, int armorPiercingTier)
		{
			this.DamageType = damageType;
			this.Tier = tier;
			this.ArmorPiercingTier = armorPiercingTier;
		}

		// Token: 0x04000677 RID: 1655
		public readonly EnumDamageType DamageType;

		// Token: 0x04000678 RID: 1656
		public readonly int Tier;

		// Token: 0x04000679 RID: 1657
		public readonly int ArmorPiercingTier;
	}
}
