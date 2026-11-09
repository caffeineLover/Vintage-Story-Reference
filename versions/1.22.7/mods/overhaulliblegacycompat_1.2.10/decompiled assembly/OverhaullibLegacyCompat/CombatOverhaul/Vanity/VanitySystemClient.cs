using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace CombatOverhaul.Vanity
{
	// Token: 0x020000B7 RID: 183
	[NullableContext(1)]
	[Nullable(0)]
	public class VanitySystemClient
	{
		// Token: 0x0600097E RID: 2430 RVA: 0x0003309C File Offset: 0x0003129C
		public VanitySystemClient(ICoreClientAPI api)
		{
			this._api = api;
			this._clientChannel = api.Network.RegisterChannel("CombatOverhaul:stats").RegisterMessageType<VanityPacket>().RegisterMessageType<BackpackVanityPacket>().SetMessageHandler<VanityPacket>(new NetworkServerMessageHandler<VanityPacket>(this.HandlePacket)).SetMessageHandler<BackpackVanityPacket>(new NetworkServerMessageHandler<BackpackVanityPacket>(this.HandlePacket));
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x000330F8 File Offset: 0x000312F8
		public void HideBackpack(bool hide)
		{
			IClientNetworkChannel clientChannel = this._clientChannel;
			BackpackVanityPacket backpackVanityPacket = new BackpackVanityPacket();
			backpackVanityPacket.Hide = hide;
			IClientPlayer player = this._api.World.Player;
			long? num;
			if (player == null)
			{
				num = default(long?);
			}
			else
			{
				EntityPlayer entity = player.Entity;
				num = ((entity != null) ? new long?(entity.EntityId) : default(long?));
			}
			long? num2 = num;
			backpackVanityPacket.PlayerEntityId = num2.GetValueOrDefault();
			clientChannel.SendPacket<BackpackVanityPacket>(backpackVanityPacket);
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00033168 File Offset: 0x00031368
		private void HandlePacket(BackpackVanityPacket packet)
		{
			EntityPlayer player = this._api.World.GetEntityById(packet.PlayerEntityId) as EntityPlayer;
			if (player == null)
			{
				return;
			}
			player.MarkShapeModified();
			SyncedTreeAttribute watchedAttributes = player.WatchedAttributes;
			if ((watchedAttributes != null && watchedAttributes.GetBool("combatoverhaul:hide-backpack", false)) != packet.Hide)
			{
				SyncedTreeAttribute watchedAttributes2 = player.WatchedAttributes;
				if (watchedAttributes2 != null)
				{
					watchedAttributes2.SetBool("combatoverhaul:hide-backpack", packet.Hide);
				}
			}
			player.MarkShapeModified();
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x000331E0 File Offset: 0x000313E0
		private void HandlePacket(VanityPacket packet)
		{
			EntityPlayer player = this._api.World.GetEntityById(packet.PlayerEntityId) as EntityPlayer;
			if (player == null)
			{
				return;
			}
			VanityInventory inventory = GeneralUtils.GetVanityInventory(player.Player);
			if (inventory == null)
			{
				return;
			}
			bool retesselate = false;
			foreach (ISlotContentCanHide slot in Enumerable.OfType<ISlotContentCanHide>(inventory))
			{
				if (slot.Hide)
				{
					byte index = (byte)(slot as ItemSlot).Inventory.GetSlotId(slot as ItemSlot);
					if (!ArrayExtensions.Contains<byte>(packet.HiddenSlots, index))
					{
						retesselate = true;
						slot.Hide = false;
					}
				}
				else
				{
					byte index2 = (byte)(slot as ItemSlot).Inventory.GetSlotId(slot as ItemSlot);
					if (ArrayExtensions.Contains<byte>(packet.HiddenSlots, index2))
					{
						retesselate = true;
						slot.Hide = true;
					}
				}
			}
			inventory.RecolorSlots();
			if (retesselate)
			{
				player.MarkShapeModified();
			}
		}

		// Token: 0x04000457 RID: 1111
		private const string _networkChannelId = "CombatOverhaul:stats";

		// Token: 0x04000458 RID: 1112
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x04000459 RID: 1113
		private readonly ICoreClientAPI _api;
	}
}
