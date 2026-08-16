using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.MeleeSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000105 RID: 261
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeleeBlockSystemServer : MeleeSystem
	{
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000D18 RID: 3352 RVA: 0x000408F8 File Offset: 0x0003EAF8
		// (remove) Token: 0x06000D19 RID: 3353 RVA: 0x00040930 File Offset: 0x0003EB30
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

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x00040965 File Offset: 0x0003EB65
		internal bool HasDamageBlockedListeners
		{
			get
			{
				return this.OnDamageBlocked != null;
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00040970 File Offset: 0x0003EB70
		public MeleeBlockSystemServer(ICoreServerAPI api)
		{
			this._api = api;
			this._serverChannel = api.Network.RegisterChannel("CombatOverhaul:damage-packets").RegisterMessageType<DamageBlockPacket>().RegisterMessageType<DamageStopBlockPacket>().RegisterMessageType<DamageBlockCallbackPacket>().SetMessageHandler<DamageBlockPacket>(new NetworkClientMessageHandler<DamageBlockPacket>(this.HandlePacket)).SetMessageHandler<DamageStopBlockPacket>(new NetworkClientMessageHandler<DamageStopBlockPacket>(this.HandlePacket));
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x000409D4 File Offset: 0x0003EBD4
		private void HandlePacket(IServerPlayer player, DamageBlockPacket packet)
		{
			PlayerDamageModelBehavior behavior = player.Entity.GetBehavior<PlayerDamageModelBehavior>();
			if (behavior != null)
			{
				this._lastBlockMainHand = packet.MainHand;
				behavior.CurrentDamageBlock = packet.ToBlockStats(delegate(float damageBlocked, int attackTier, int blockTier)
				{
					this.BlockCallback(player, packet.MainHand, damageBlocked, attackTier, blockTier, packet.Id);
				});
			}
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00040A40 File Offset: 0x0003EC40
		private void HandlePacket(IServerPlayer player, DamageStopBlockPacket packet)
		{
			PlayerDamageModelBehavior behavior = player.Entity.GetBehavior<PlayerDamageModelBehavior>();
			if (behavior != null && this._lastBlockMainHand == packet.MainHand)
			{
				behavior.CurrentDamageBlock = null;
			}
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00040A74 File Offset: 0x0003EC74
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

		// Token: 0x06000D1F RID: 3359 RVA: 0x00040AF9 File Offset: 0x0003ECF9
		internal void EmitDamageBlocked(DamageBlockEventArgs args)
		{
			Action<DamageBlockEventArgs> onDamageBlocked = this.OnDamageBlocked;
			if (onDamageBlocked == null)
			{
				return;
			}
			onDamageBlocked.Invoke(args);
		}

		// Token: 0x0400066E RID: 1646
		private readonly ICoreServerAPI _api;

		// Token: 0x0400066F RID: 1647
		private readonly IServerNetworkChannel _serverChannel;

		// Token: 0x04000670 RID: 1648
		private bool _lastBlockMainHand;
	}
}
