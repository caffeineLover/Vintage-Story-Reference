using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000104 RID: 260
	[NullableContext(1)]
	public interface IHasServerBlockCallback
	{
		// Token: 0x06000D17 RID: 3351
		void BlockCallback(IServerPlayer player, ItemSlot slot, bool mainHand, float damageBlocked, int attackTier, int blockTier);
	}
}
