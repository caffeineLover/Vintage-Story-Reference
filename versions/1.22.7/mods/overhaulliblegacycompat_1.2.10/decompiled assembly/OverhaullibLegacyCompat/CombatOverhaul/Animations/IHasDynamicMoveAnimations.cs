using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200019B RID: 411
	[NullableContext(1)]
	public interface IHasDynamicMoveAnimations : IHasDynamicIdleAnimations
	{
		// Token: 0x060013E6 RID: 5094
		AnimationRequestByCode? GetWalkAnimation(EntityPlayer player, ItemSlot slot, bool mainHand);

		// Token: 0x060013E7 RID: 5095
		AnimationRequestByCode? GetRunAnimation(EntityPlayer player, ItemSlot slot, bool mainHand);

		// Token: 0x060013E8 RID: 5096
		AnimationRequestByCode? GetSwimAnimation(EntityPlayer player, ItemSlot slot, bool mainHand);

		// Token: 0x060013E9 RID: 5097
		AnimationRequestByCode? GetSwimIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand);
	}
}
