using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000146 RID: 326
	[NullableContext(1)]
	public interface IGearSlotModifiedListener
	{
		// Token: 0x06000FA7 RID: 4007
		void OnSlotModified(ItemSlot slot, ArmorInventory inventory, EntityPlayer player);
	}
}
