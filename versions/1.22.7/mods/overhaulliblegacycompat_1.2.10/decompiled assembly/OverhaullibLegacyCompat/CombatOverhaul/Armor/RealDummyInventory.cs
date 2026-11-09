using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200013B RID: 315
	[NullableContext(1)]
	[Nullable(0)]
	public class RealDummyInventory : DummyInventory
	{
		// Token: 0x06000F2E RID: 3886 RVA: 0x00049F3F File Offset: 0x0004813F
		public RealDummyInventory(ICoreAPI api, int quantitySlots = 1) : base(api, quantitySlots)
		{
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00049F49 File Offset: 0x00048149
		public override bool CanContain(ItemSlot sinkSlot, ItemSlot sourceSlot)
		{
			return false;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00049F4C File Offset: 0x0004814C
		public override bool CanPlayerAccess(IPlayer player, EntityPos position)
		{
			return false;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00049F4F File Offset: 0x0004814F
		public override bool CanPlayerModify(IPlayer player, EntityPos position)
		{
			return false;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00049F52 File Offset: 0x00048152
		protected override ItemSlot NewSlot(int i)
		{
			return new RealDummySlot(this)
			{
				DrawUnavailable = false
			};
		}
	}
}
