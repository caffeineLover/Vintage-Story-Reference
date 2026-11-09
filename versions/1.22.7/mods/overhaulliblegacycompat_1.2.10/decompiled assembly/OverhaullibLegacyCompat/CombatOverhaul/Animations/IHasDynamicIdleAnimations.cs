using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000199 RID: 409
	[NullableContext(1)]
	public interface IHasDynamicIdleAnimations
	{
		// Token: 0x060013E0 RID: 5088
		AnimationRequestByCode? GetIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand);

		// Token: 0x060013E1 RID: 5089
		AnimationRequestByCode? GetReadyAnimation(EntityPlayer player, ItemSlot slot, bool mainHand);
	}
}
