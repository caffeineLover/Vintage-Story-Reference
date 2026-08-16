using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Inputs
{
	// Token: 0x0200006A RID: 106
	[NullableContext(1)]
	public interface IOnGameTick
	{
		// Token: 0x0600050C RID: 1292
		void OnGameTick(ItemSlot slot, EntityPlayer player, ref int state, bool mainHand);
	}
}
