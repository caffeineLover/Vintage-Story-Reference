using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.MeleeSystems;
using Vintagestory.API.Client;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200010E RID: 270
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeleeBlockSystemClient : MeleeSystem
	{
		// Token: 0x06000D6B RID: 3435 RVA: 0x00041748 File Offset: 0x0003F948
		public MeleeBlockSystemClient(ICoreClientAPI api)
		{
			this._clientChannel = api.Network.RegisterChannel("CombatOverhaul:damage-packets").RegisterMessageType<DamageBlockPacket>().RegisterMessageType<DamageStopBlockPacket>().RegisterMessageType<DamageBlockCallbackPacket>().SetMessageHandler<DamageBlockCallbackPacket>(new NetworkServerMessageHandler<DamageBlockCallbackPacket>(this.HandleCallback));
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x000417AF File Offset: 0x0003F9AF
		public void StartBlock(DamageBlockJson block, bool mainHand)
		{
			this.StartBlock(block, mainHand, EnumDamageBlockKind.Unknown);
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x000417BC File Offset: 0x0003F9BC
		public void StartBlock(DamageBlockJson block, bool mainHand, EnumDamageBlockKind kind)
		{
			DamageBlockPacket packet = block.ToPacket();
			packet.Id = 0UL;
			packet.MainHand = mainHand;
			packet.Kind = kind;
			this._clientChannel.SendPacket<DamageBlockPacket>(packet);
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x000417F2 File Offset: 0x0003F9F2
		public void StartBlock(DamageBlockJson block, bool mainHand, Action callback)
		{
			this.StartBlock(block, mainHand, EnumDamageBlockKind.Unknown, callback);
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00041800 File Offset: 0x0003FA00
		public void StartBlock(DamageBlockJson block, bool mainHand, EnumDamageBlockKind kind, Action callback)
		{
			DamageBlockPacket packet = block.ToPacket();
			DamageBlockPacket damageBlockPacket = packet;
			ulong nextId = this._nextId;
			this._nextId = nextId + 1UL;
			damageBlockPacket.Id = nextId;
			packet.MainHand = mainHand;
			packet.Kind = kind;
			this.PushCallback(packet.Id, callback);
			this._clientChannel.SendPacket<DamageBlockPacket>(packet);
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00041854 File Offset: 0x0003FA54
		public void StopBlock(bool mainHand)
		{
			this._clientChannel.SendPacket<DamageStopBlockPacket>(new DamageStopBlockPacket
			{
				MainHand = mainHand
			});
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00041870 File Offset: 0x0003FA70
		public void HandleCallback(DamageBlockCallbackPacket packet)
		{
			Action callback;
			if (this._callbacks.TryGetValue(packet.Id, ref callback))
			{
				callback.Invoke();
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00041898 File Offset: 0x0003FA98
		private void PushCallback(ulong id, Action callback)
		{
			this._ids.Enqueue(id);
			this._callbacks[id] = callback;
			if (this._ids.Count > 10)
			{
				ulong idToRemove = this._ids.Dequeue();
				this._callbacks.Remove(idToRemove);
			}
		}

		// Token: 0x0400068A RID: 1674
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x0400068B RID: 1675
		private readonly Queue<ulong> _ids = new Queue<ulong>();

		// Token: 0x0400068C RID: 1676
		private readonly Dictionary<ulong, Action> _callbacks = new Dictionary<ulong, Action>();

		// Token: 0x0400068D RID: 1677
		private const int _queueSize = 10;

		// Token: 0x0400068E RID: 1678
		private ulong _nextId = 1UL;
	}
}
