using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.Colliders
{
	// Token: 0x0200016F RID: 367
	[NullableContext(1)]
	public interface ICollider
	{
		// Token: 0x0600125C RID: 4700
		void Render(ICoreClientAPI api, EntityAgent entityPlayer, int color = -1);

		// Token: 0x0600125D RID: 4701
		[return: Nullable(2)]
		ICollider Transform(EntityPos origin, EntityAgent entity, ItemSlot itemSlot, ICoreClientAPI api, bool right = true);

		// Token: 0x0600125E RID: 4702
		ICollider Transform(Matrixf modelMatrix, EntityPos origin);
	}
}
