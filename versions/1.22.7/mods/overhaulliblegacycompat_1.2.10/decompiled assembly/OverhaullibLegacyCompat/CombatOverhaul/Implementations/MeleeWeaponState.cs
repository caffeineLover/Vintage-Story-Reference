using System;

namespace CombatOverhaul.Implementations
{
	// Token: 0x0200007B RID: 123
	public enum MeleeWeaponState
	{
		// Token: 0x04000285 RID: 645
		Idle,
		// Token: 0x04000286 RID: 646
		WindingUp,
		// Token: 0x04000287 RID: 647
		Attacking,
		// Token: 0x04000288 RID: 648
		Parrying,
		// Token: 0x04000289 RID: 649
		Blocking,
		// Token: 0x0400028A RID: 650
		BlockBashWindingUp,
		// Token: 0x0400028B RID: 651
		BlockBashAttacking,
		// Token: 0x0400028C RID: 652
		BlockBashCooldown,
		// Token: 0x0400028D RID: 653
		Cooldown,
		// Token: 0x0400028E RID: 654
		StartingAim,
		// Token: 0x0400028F RID: 655
		Aiming,
		// Token: 0x04000290 RID: 656
		Throwing
	}
}
