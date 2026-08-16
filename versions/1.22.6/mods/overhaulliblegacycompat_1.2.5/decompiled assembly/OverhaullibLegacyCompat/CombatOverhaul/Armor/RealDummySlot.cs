using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000130 RID: 304
	[NullableContext(1)]
	[Nullable(0)]
	public class RealDummySlot : ItemSlot
	{
		// Token: 0x06000EBF RID: 3775 RVA: 0x00048661 File Offset: 0x00046861
		public RealDummySlot(InventoryBase inventory) : base(inventory)
		{
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0004866A File Offset: 0x0004686A
		public override bool CanTake()
		{
			return false;
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0004866D File Offset: 0x0004686D
		public override bool CanHold(ItemSlot sourceSlot)
		{
			return false;
		}
	}
}
