using System;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000198 RID: 408
	public interface IHasIdleAnimations
	{
		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060013DE RID: 5086
		AnimationRequestByCode IdleAnimation { get; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060013DF RID: 5087
		AnimationRequestByCode ReadyAnimation { get; }
	}
}
