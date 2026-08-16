using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200018B RID: 395
	[NullableContext(1)]
	public interface IHasDynamicIdleAnimations
	{
		// Token: 0x06001368 RID: 4968
		AnimationRequestByCode? GetIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand);

		// Token: 0x06001369 RID: 4969
		AnimationRequestByCode? GetReadyAnimation(EntityPlayer player, ItemSlot slot, bool mainHand);
	}
}
