using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200010F RID: 271
	[NullableContext(1)]
	public interface IHasServerBlockCallback
	{
		// Token: 0x06000D73 RID: 3443
		void BlockCallback(IServerPlayer player, ItemSlot slot, bool mainHand, float damageBlocked, int attackTier, int blockTier);
	}
}
