using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000147 RID: 327
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ToolSlotData
	{
		// Token: 0x06001044 RID: 4164 RVA: 0x000501AA File Offset: 0x0004E3AA
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

		// Token: 0x04000789 RID: 1929
		public readonly SlotConfig Config;

		// Token: 0x0400078A RID: 1930
		[Nullable(2)]
		public readonly ItemStack Stack;

		// Token: 0x0400078B RID: 1931
		public readonly string ToolBagId;

		// Token: 0x0400078C RID: 1932
		public readonly int ToolBagIndex;

		// Token: 0x0400078D RID: 1933
		public readonly bool MainHand;

		// Token: 0x0400078E RID: 1934
		public readonly string Icon;

		// Token: 0x0400078F RID: 1935
		public readonly string Color;

		// Token: 0x04000790 RID: 1936
		public readonly int SlotIndex;
	}
}
