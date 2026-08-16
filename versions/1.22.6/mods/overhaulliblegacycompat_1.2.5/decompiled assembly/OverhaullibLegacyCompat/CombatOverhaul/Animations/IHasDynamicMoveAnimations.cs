using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200018D RID: 397
	[NullableContext(1)]
	public interface IHasDynamicMoveAnimations : IHasDynamicIdleAnimations
	{
		// Token: 0x0600136E RID: 4974
		AnimationRequestByCode? GetWalkAnimation(EntityPlayer player, ItemSlot slot, bool mainHand);

		// Token: 0x0600136F RID: 4975
		AnimationRequestByCode? GetRunAnimation(EntityPlayer player, ItemSlot slot, bool mainHand);

		// Token: 0x06001370 RID: 4976
		AnimationRequestByCode? GetSwimAnimation(EntityPlayer player, ItemSlot slot, bool mainHand);

		// Token: 0x06001371 RID: 4977
		AnimationRequestByCode? GetSwimIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand);
	}
}
