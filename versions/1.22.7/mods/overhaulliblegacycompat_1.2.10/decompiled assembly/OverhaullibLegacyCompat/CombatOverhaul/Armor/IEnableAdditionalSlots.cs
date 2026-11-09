using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000143 RID: 323
	[NullableContext(1)]
	public interface IEnableAdditionalSlots
	{
		// Token: 0x06000F93 RID: 3987
		bool GetIfEnabled(ItemStack stack, IInventory inventory, string slotType);

		// Token: 0x06000F94 RID: 3988
		[return: Nullable(2)]
		string GetIcon(ItemStack stack, IInventory inventory, string slotType);

		// Token: 0x06000F95 RID: 3989
		[return: Nullable(2)]
		SlotConfig GetConfig(ItemStack stack, IInventory inventory, string slotType);
	}
}
