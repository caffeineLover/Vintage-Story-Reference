using System;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200010F RID: 271
	[Flags]
	public enum DamageZone
	{
		// Token: 0x04000688 RID: 1672
		None = 0,
		// Token: 0x04000689 RID: 1673
		Head = 1,
		// Token: 0x0400068A RID: 1674
		Face = 2,
		// Token: 0x0400068B RID: 1675
		Neck = 4,
		// Token: 0x0400068C RID: 1676
		Torso = 8,
		// Token: 0x0400068D RID: 1677
		Arms = 16,
		// Token: 0x0400068E RID: 1678
		Hands = 32,
		// Token: 0x0400068F RID: 1679
		Legs = 64,
		// Token: 0x04000690 RID: 1680
		Feet = 128
	}
}
