using System;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200011A RID: 282
	[Flags]
	public enum DamageZone
	{
		// Token: 0x040006A9 RID: 1705
		None = 0,
		// Token: 0x040006AA RID: 1706
		Head = 1,
		// Token: 0x040006AB RID: 1707
		Face = 2,
		// Token: 0x040006AC RID: 1708
		Neck = 4,
		// Token: 0x040006AD RID: 1709
		Torso = 8,
		// Token: 0x040006AE RID: 1710
		Arms = 16,
		// Token: 0x040006AF RID: 1711
		Hands = 32,
		// Token: 0x040006B0 RID: 1712
		Legs = 64,
		// Token: 0x040006B1 RID: 1713
		Feet = 128
	}
}
