using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200018E RID: 398
	[NullableContext(1)]
	public interface IMeshReplacer
	{
		// Token: 0x06001372 RID: 4978
		[return: Nullable(2)]
		MultiTextureMeshRef GetReplacement(ICoreClientAPI api, ItemSlot itemSlot, Entity entity);
	}
}
