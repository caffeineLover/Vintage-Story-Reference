using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Inputs
{
	// Token: 0x02000071 RID: 113
	[NullableContext(1)]
	public interface IOnInInventory
	{
		// Token: 0x06000558 RID: 1368
		void OnInInventory(EntityPlayer player, ItemSlot slot);
	}
}
