using System;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000110 RID: 272
	[Flags]
	public enum PlayerBodyPart
	{
		// Token: 0x04000692 RID: 1682
		None = 0,
		// Token: 0x04000693 RID: 1683
		Head = 1,
		// Token: 0x04000694 RID: 1684
		Face = 2,
		// Token: 0x04000695 RID: 1685
		Neck = 4,
		// Token: 0x04000696 RID: 1686
		Torso = 8,
		// Token: 0x04000697 RID: 1687
		LeftArm = 16,
		// Token: 0x04000698 RID: 1688
		RightArm = 32,
		// Token: 0x04000699 RID: 1689
		LeftHand = 64,
		// Token: 0x0400069A RID: 1690
		RightHand = 128,
		// Token: 0x0400069B RID: 1691
		LeftLeg = 256,
		// Token: 0x0400069C RID: 1692
		RightLeg = 512,
		// Token: 0x0400069D RID: 1693
		LeftFoot = 1024,
		// Token: 0x0400069E RID: 1694
		RightFoot = 2048
	}
}
