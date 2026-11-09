using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000DC RID: 220
	[NullableContext(1)]
	public interface IServerRangedWeaponLogic
	{
		// Token: 0x06000B4B RID: 2891
		bool Reload(IServerPlayer player, ItemSlot slot, [Nullable(2)] ItemSlot ammoSlot, ReloadPacket packet);

		// Token: 0x06000B4C RID: 2892
		bool Shoot(IServerPlayer player, ItemSlot slot, ShotPacket packet, Entity shooter);
	}
}
