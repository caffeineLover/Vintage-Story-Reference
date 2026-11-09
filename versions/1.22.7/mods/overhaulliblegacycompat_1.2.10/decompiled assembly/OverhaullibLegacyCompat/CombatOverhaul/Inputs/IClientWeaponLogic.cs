using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace CombatOverhaul.Inputs
{
	// Token: 0x02000069 RID: 105
	[NullableContext(1)]
	public interface IClientWeaponLogic
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000507 RID: 1287
		int ItemId { get; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000508 RID: 1288
		DirectionsConfiguration DirectionsType { get; }

		// Token: 0x06000509 RID: 1289
		void OnSelected(ItemSlot slot, EntityPlayer player, bool mainHand, ref int state);

		// Token: 0x0600050A RID: 1290
		void OnDeselected(EntityPlayer player, bool mainHand, ref int state);

		// Token: 0x0600050B RID: 1291
		void OnRegistered(ActionsManagerPlayerBehavior behavior, ICoreClientAPI api);
	}
}
