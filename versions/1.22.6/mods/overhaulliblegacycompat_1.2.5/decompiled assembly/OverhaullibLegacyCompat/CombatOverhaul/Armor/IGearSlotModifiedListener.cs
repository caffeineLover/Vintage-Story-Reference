using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200013A RID: 314
	[NullableContext(1)]
	public interface IGearSlotModifiedListener
	{
		// Token: 0x06000F33 RID: 3891
		void OnSlotModified(ItemSlot slot, ArmorInventory inventory, EntityPlayer player);
	}
}
