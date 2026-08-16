using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D1 RID: 209
	[NullableContext(1)]
	public interface IServerRangedWeaponLogic
	{
		// Token: 0x06000AEF RID: 2799
		bool Reload(IServerPlayer player, ItemSlot slot, [Nullable(2)] ItemSlot ammoSlot, ReloadPacket packet);

		// Token: 0x06000AF0 RID: 2800
		bool Shoot(IServerPlayer player, ItemSlot slot, ShotPacket packet, Entity shooter);
	}
}
