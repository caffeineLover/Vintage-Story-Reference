using System;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200011B RID: 283
	[Flags]
	public enum PlayerBodyPart
	{
		// Token: 0x040006B3 RID: 1715
		None = 0,
		// Token: 0x040006B4 RID: 1716
		Head = 1,
		// Token: 0x040006B5 RID: 1717
		Face = 2,
		// Token: 0x040006B6 RID: 1718
		Neck = 4,
		// Token: 0x040006B7 RID: 1719
		Torso = 8,
		// Token: 0x040006B8 RID: 1720
		LeftArm = 16,
		// Token: 0x040006B9 RID: 1721
		RightArm = 32,
		// Token: 0x040006BA RID: 1722
		LeftHand = 64,
		// Token: 0x040006BB RID: 1723
		RightHand = 128,
		// Token: 0x040006BC RID: 1724
		LeftLeg = 256,
		// Token: 0x040006BD RID: 1725
		RightLeg = 512,
		// Token: 0x040006BE RID: 1726
		LeftFoot = 1024,
		// Token: 0x040006BF RID: 1727
		RightFoot = 2048
	}
}
