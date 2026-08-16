using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000137 RID: 311
	[NullableContext(1)]
	public interface IEnableAdditionalSlots
	{
		// Token: 0x06000F1F RID: 3871
		bool GetIfEnabled(ItemStack stack, IInventory inventory, string slotType);

		// Token: 0x06000F20 RID: 3872
		[return: Nullable(2)]
		string GetIcon(ItemStack stack, IInventory inventory, string slotType);

		// Token: 0x06000F21 RID: 3873
		[return: Nullable(2)]
		SlotConfig GetConfig(ItemStack stack, IInventory inventory, string slotType);
	}
}
