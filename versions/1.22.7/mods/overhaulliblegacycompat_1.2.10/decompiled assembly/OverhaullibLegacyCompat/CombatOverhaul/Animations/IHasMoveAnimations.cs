using System;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200019A RID: 410
	public interface IHasMoveAnimations : IHasIdleAnimations
	{
		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060013E2 RID: 5090
		AnimationRequestByCode WalkAnimation { get; }

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060013E3 RID: 5091
		AnimationRequestByCode RunAnimation { get; }

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060013E4 RID: 5092
		AnimationRequestByCode SwimAnimation { get; }

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060013E5 RID: 5093
		AnimationRequestByCode SwimIdleAnimation { get; }
	}
}
