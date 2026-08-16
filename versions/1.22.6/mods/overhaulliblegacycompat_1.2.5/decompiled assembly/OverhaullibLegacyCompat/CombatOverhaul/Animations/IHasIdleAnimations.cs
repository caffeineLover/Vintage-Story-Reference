using System;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200018A RID: 394
	public interface IHasIdleAnimations
	{
		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001366 RID: 4966
		AnimationRequestByCode IdleAnimation { get; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001367 RID: 4967
		AnimationRequestByCode ReadyAnimation { get; }
	}
}
