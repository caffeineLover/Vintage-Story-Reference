using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200012F RID: 303
	[NullableContext(1)]
	[Nullable(0)]
	public class RealDummyInventory : DummyInventory
	{
		// Token: 0x06000EBA RID: 3770 RVA: 0x0004863F File Offset: 0x0004683F
		public RealDummyInventory(ICoreAPI api, int quantitySlots = 1) : base(api, quantitySlots)
		{
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x00048649 File Offset: 0x00046849
		public override bool CanContain(ItemSlot sinkSlot, ItemSlot sourceSlot)
		{
			return false;
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0004864C File Offset: 0x0004684C
		public override bool CanPlayerAccess(IPlayer player, EntityPos position)
		{
			return false;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0004864F File Offset: 0x0004684F
		public override bool CanPlayerModify(IPlayer player, EntityPos position)
		{
			return false;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00048652 File Offset: 0x00046852
		protected override ItemSlot NewSlot(int i)
		{
			return new RealDummySlot(this)
			{
				DrawUnavailable = false
			};
		}
	}
}
