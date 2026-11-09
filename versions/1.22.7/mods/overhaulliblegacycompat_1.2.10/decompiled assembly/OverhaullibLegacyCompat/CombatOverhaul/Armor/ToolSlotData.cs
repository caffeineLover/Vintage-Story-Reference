using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000153 RID: 339
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ToolSlotData
	{
		// Token: 0x060010B8 RID: 4280 RVA: 0x00051AB2 File Offset: 0x0004FCB2
		public ToolSlotData(SlotConfig config, [Nullable(2)] ItemStack stack, string toolBagId, int toolBagIndex, bool mainHand, string icon, string color, int slotIndex)
		{
			this.Config = config;
			this.Stack = stack;
			this.ToolBagId = toolBagId;
			this.ToolBagIndex = toolBagIndex;
			this.MainHand = mainHand;
			this.Icon = icon;
			this.Color = color;
			this.SlotIndex = slotIndex;
		}

		// Token: 0x040007AF RID: 1967
		public readonly SlotConfig Config;

		// Token: 0x040007B0 RID: 1968
		[Nullable(2)]
		public readonly ItemStack Stack;

		// Token: 0x040007B1 RID: 1969
		public readonly string ToolBagId;

		// Token: 0x040007B2 RID: 1970
		public readonly int ToolBagIndex;

		// Token: 0x040007B3 RID: 1971
		public readonly bool MainHand;

		// Token: 0x040007B4 RID: 1972
		public readonly string Icon;

		// Token: 0x040007B5 RID: 1973
		public readonly string Color;

		// Token: 0x040007B6 RID: 1974
		public readonly int SlotIndex;
	}
}
