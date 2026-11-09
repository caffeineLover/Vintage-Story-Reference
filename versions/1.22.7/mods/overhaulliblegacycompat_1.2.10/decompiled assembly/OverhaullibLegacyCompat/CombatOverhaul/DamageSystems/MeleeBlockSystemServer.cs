using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.MeleeSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000110 RID: 272
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeleeBlockSystemServer : MeleeSystem
	{
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000D74 RID: 3444 RVA: 0x000418E8 File Offset: 0x0003FAE8
		// (remove) Token: 0x06000D75 RID: 3445 RVA: 0x00041920 File Offset: 0x0003FB20
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public event Action<DamageBlockEventArgs> OnDamageBlocked;

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x00041955 File Offset: 0x0003FB55
		internal bool HasDamageBlockedListeners
		{
			get
			{
				return this.OnDamageBlocked != null;
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x00041960 File Offset: 0x0003FB60
		public MeleeBlockSystemServer(ICoreServerAPI api)
		{
			this._api = api;
			this._serverChannel = api.Network.RegisterChannel("CombatOverhaul:damage-packets").RegisterMessageType<DamageBlockPacket>().RegisterMessageType<DamageStopBlockPacket>().RegisterMessageType<DamageBlockCallbackPacket>().SetMessageHandler<DamageBlockPacket>(new NetworkClientMessageHandler<DamageBlockPacket>(this.HandlePacket)).SetMessageHandler<DamageStopBlockPacket>(new NetworkClientMessageHandler<DamageStopBlockPacket>(this.HandlePacket));
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x000419C4 File Offset: 0x0003FBC4
		private void HandlePacket(IServerPlayer player, DamageBlockPacket packet)
		{
			PlayerDamageModelBehavior behavior = player.Entity.GetBehavior<PlayerDamageModelBehavior>();
			if (behavior == null)
			{
				return;
			}
			DamageBlockPacket authorized;
			if (!ServerDamageBlockProfiles.TryResolve(player, packet, out authorized))
			{
				return;
			}
			behavior.CurrentDamageBlock = authorized.ToBlockStats(delegate(float damageBlocked, int attackTier, int blockTier)
			{
				this.BlockCallback(player, authorized.MainHand, damageBlocked, attackTier, blockTier, authorized.Id);
			});
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00041A2C File Offset: 0x0003FC2C
		private void HandlePacket(IServerPlayer player, DamageStopBlockPacket packet)
		{
			PlayerDamageModelBehavior behavior = player.Entity.GetBehavior<PlayerDamageModelBehavior>();
			bool? flag;
			if (behavior == null)
			{
				flag = default(bool?);
			}
			else
			{
				DamageBlockStats currentDamageBlock = behavior.CurrentDamageBlock;
				flag = ((currentDamageBlock != null) ? new bool?(currentDamageBlock.MainHand) : default(bool?));
			}
			bool? flag2 = flag;
			bool mainHand = packet.MainHand;
			if (flag2.GetValueOrDefault() == mainHand & flag2 != null)
			{
				behavior.CurrentDamageBlock = null;
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00041A98 File Offset: 0x0003FC98
		private void BlockCallback(IServerPlayer player, bool mainHand, float damageBlocked, int attackTier, int blockTier, ulong id)
		{
			ItemSlot slot = mainHand ? player.Entity.RightHandItemSlot : player.Entity.LeftHandItemSlot;
			ItemStack itemstack = slot.Itemstack;
			IHasServerBlockCallback hasServerBlockCallback;
			if (itemstack == null)
			{
				hasServerBlockCallback = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				hasServerBlockCallback = ((collectible != null) ? collectible.GetCollectibleInterface<IHasServerBlockCallback>() : null);
			}
			IHasServerBlockCallback item = hasServerBlockCallback;
			if (item == null)
			{
				return;
			}
			item.BlockCallback(player, slot, mainHand, damageBlocked, attackTier, blockTier);
			if (id != 0UL)
			{
				IServerNetworkChannel serverChannel = this._serverChannel;
				if (serverChannel == null)
				{
					return;
				}
				serverChannel.SendPacket<DamageBlockCallbackPacket>(new DamageBlockCallbackPacket
				{
					Id = id
				}, new IServerPlayer[]
				{
					player
				});
			}
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00041B1D File Offset: 0x0003FD1D
		internal void EmitDamageBlocked(DamageBlockEventArgs args)
		{
			Action<DamageBlockEventArgs> onDamageBlocked = this.OnDamageBlocked;
			if (onDamageBlocked == null)
			{
				return;
			}
			onDamageBlocked.Invoke(args);
		}

		// Token: 0x04000690 RID: 1680
		private readonly ICoreServerAPI _api;

		// Token: 0x04000691 RID: 1681
		private readonly IServerNetworkChannel _serverChannel;
	}
}
