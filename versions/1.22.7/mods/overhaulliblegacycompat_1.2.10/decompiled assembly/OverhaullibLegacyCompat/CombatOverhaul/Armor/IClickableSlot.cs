using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000148 RID: 328
	[NullableContext(1)]
	public interface IClickableSlot
	{
		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000FE0 RID: 4064
		// (set) Token: 0x06000FDF RID: 4063
		IClickableSlot.SlotClickedDelegate OnSlotClicked { get; set; }

		// Token: 0x0200029E RID: 670
		// (Invoke) Token: 0x0600182F RID: 6191
		[NullableContext(0)]
		public delegate bool SlotClickedDelegate(ItemSlot thisSlot, ItemSlot sourceSlot, ref ItemStackMoveOperation operation);
	}
}
