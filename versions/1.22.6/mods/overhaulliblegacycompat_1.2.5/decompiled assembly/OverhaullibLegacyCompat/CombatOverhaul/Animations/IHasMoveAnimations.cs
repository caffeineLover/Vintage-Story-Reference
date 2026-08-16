using System;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200018C RID: 396
	public interface IHasMoveAnimations : IHasIdleAnimations
	{
		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x0600136A RID: 4970
		AnimationRequestByCode WalkAnimation { get; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x0600136B RID: 4971
		AnimationRequestByCode RunAnimation { get; }

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x0600136C RID: 4972
		AnimationRequestByCode SwimAnimation { get; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x0600136D RID: 4973
		AnimationRequestByCode SwimIdleAnimation { get; }
	}
}
