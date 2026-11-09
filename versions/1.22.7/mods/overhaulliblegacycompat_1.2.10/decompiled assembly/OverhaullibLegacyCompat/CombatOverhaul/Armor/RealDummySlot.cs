using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200013C RID: 316
	[NullableContext(1)]
	[Nullable(0)]
	public class RealDummySlot : ItemSlot
	{
		// Token: 0x06000F33 RID: 3891 RVA: 0x00049F61 File Offset: 0x00048161
		public RealDummySlot(InventoryBase inventory) : base(inventory)
		{
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00049F6A File Offset: 0x0004816A
		public override bool CanTake()
		{
			return false;
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00049F6D File Offset: 0x0004816D
		public override bool CanHold(ItemSlot sourceSlot)
		{
			return false;
		}
	}
}
