using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace CombatOverhaul.Vanity
{
	// Token: 0x020000B8 RID: 184
	[NullableContext(1)]
	[Nullable(0)]
	public class VanitySystemServer
	{
		// Token: 0x06000982 RID: 2434 RVA: 0x000332A8 File Offset: 0x000314A8
		public VanitySystemServer(ICoreServerAPI api)
		{
			this._api = api;
			this._serverChannel = api.Network.RegisterChannel("CombatOverhaul:stats").RegisterMessageType<VanityPacket>().RegisterMessageType<BackpackVanityPacket>().SetMessageHandler<BackpackVanityPacket>(new NetworkClientMessageHandler<BackpackVanityPacket>(this.HandlePacket));
			this._listener = this._api.World.RegisterGameTickListener(delegate(float _)
			{
				this.Resync();
			}, 60000, 3000);
			this._playerNowPlayingHandler = delegate(IServerPlayer _)
			{
				this.Resync();
			};
			this._api.Event.PlayerNowPlaying += this._playerNowPlayingHandler;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00033347 File Offset: 0x00031547
		public void SendUpdate(IPlayer player, byte[] slots)
		{
			IServerNetworkChannel serverChannel = this._serverChannel;
			VanityPacket vanityPacket = new VanityPacket();
			EntityPlayer entity = player.Entity;
			vanityPacket.PlayerEntityId = ((entity != null) ? entity.EntityId : 0L);
			vanityPacket.HiddenSlots = slots;
			serverChannel.BroadcastPacket<VanityPacket>(vanityPacket, Array.Empty<IServerPlayer>());
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0003337E File Offset: 0x0003157E
		public void Dispose()
		{
			this._api.World.UnregisterGameTickListener(this._listener);
			this._api.Event.PlayerNowPlaying -= this._playerNowPlayingHandler;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x000333AC File Offset: 0x000315AC
		private void HandlePacket(IServerPlayer player, BackpackVanityPacket packet)
		{
			EntityPlayer entity = player.Entity;
			if (entity != null)
			{
				SyncedTreeAttribute watchedAttributes = entity.WatchedAttributes;
				if (watchedAttributes != null)
				{
					watchedAttributes.SetBool("combatoverhaul:hide-backpack", packet.Hide);
				}
			}
			this._serverChannel.BroadcastPacket<BackpackVanityPacket>(packet, Array.Empty<IServerPlayer>());
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x000333E8 File Offset: 0x000315E8
		private void Resync()
		{
			IEnumerable<IPlayer> enumerable = Enumerable.Where<IPlayer>(this._api.World.AllOnlinePlayers, (IPlayer player) => player.Entity != null);
			Func<IPlayer, VanityInventory> func;
			if ((func = VanitySystemServer.<>O.<0>__GetVanityInventory) == null)
			{
				func = (VanitySystemServer.<>O.<0>__GetVanityInventory = new Func<IPlayer, VanityInventory>(GeneralUtils.GetVanityInventory));
			}
			EnumerableExtensions.Foreach<VanityInventory>(Enumerable.Select<IPlayer, VanityInventory>(enumerable, func), delegate(VanityInventory inventory)
			{
				if (inventory != null)
				{
					inventory.Synchronize();
				}
			});
		}

		// Token: 0x0400045A RID: 1114
		public const string HideBackpackAttribute = "combatoverhaul:hide-backpack";

		// Token: 0x0400045B RID: 1115
		private const string _networkChannelId = "CombatOverhaul:stats";

		// Token: 0x0400045C RID: 1116
		private readonly IServerNetworkChannel _serverChannel;

		// Token: 0x0400045D RID: 1117
		private readonly ICoreServerAPI _api;

		// Token: 0x0400045E RID: 1118
		private readonly long _listener;

		// Token: 0x0400045F RID: 1119
		private readonly PlayerDelegate _playerNowPlayingHandler;

		// Token: 0x02000242 RID: 578
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000C3C RID: 3132
			[Nullable(0)]
			public static Func<IPlayer, VanityInventory> <0>__GetVanityInventory;
		}
	}
}
