using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.Colliders
{
	// Token: 0x02000163 RID: 355
	[NullableContext(1)]
	public interface ICollider
	{
		// Token: 0x060011E8 RID: 4584
		void Render(ICoreClientAPI api, EntityAgent entityPlayer, int color = -1);

		// Token: 0x060011E9 RID: 4585
		[return: Nullable(2)]
		ICollider Transform(EntityPos origin, EntityAgent entity, ItemSlot itemSlot, ICoreClientAPI api, bool right = true);

		// Token: 0x060011EA RID: 4586
		ICollider Transform(Matrixf modelMatrix, EntityPos origin);
	}
}
