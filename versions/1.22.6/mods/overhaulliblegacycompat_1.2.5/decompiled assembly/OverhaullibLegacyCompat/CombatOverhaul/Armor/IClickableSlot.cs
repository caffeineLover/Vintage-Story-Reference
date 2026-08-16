using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200013C RID: 316
	[NullableContext(1)]
	public interface IClickableSlot
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000F6C RID: 3948
		// (set) Token: 0x06000F6B RID: 3947
		IClickableSlot.SlotClickedDelegate OnSlotClicked { get; set; }

		// Token: 0x0200028B RID: 651
		// (Invoke) Token: 0x0600178A RID: 6026
		[NullableContext(0)]
		public delegate bool SlotClickedDelegate(ItemSlot thisSlot, ItemSlot sourceSlot, ref ItemStackMoveOperation operation);
	}
}
