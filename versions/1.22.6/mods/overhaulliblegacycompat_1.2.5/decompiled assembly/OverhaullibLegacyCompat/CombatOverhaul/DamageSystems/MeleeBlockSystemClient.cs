using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.MeleeSystems;
using Vintagestory.API.Client;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000103 RID: 259
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeleeBlockSystemClient : MeleeSystem
	{
		// Token: 0x06000D0F RID: 3343 RVA: 0x00040758 File Offset: 0x0003E958
		public MeleeBlockSystemClient(ICoreClientAPI api)
		{
			this._clientChannel = api.Network.RegisterChannel("CombatOverhaul:damage-packets").RegisterMessageType<DamageBlockPacket>().RegisterMessageType<DamageStopBlockPacket>().RegisterMessageType<DamageBlockCallbackPacket>().SetMessageHandler<DamageBlockCallbackPacket>(new NetworkServerMessageHandler<DamageBlockCallbackPacket>(this.HandleCallback));
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x000407BF File Offset: 0x0003E9BF
		public void StartBlock(DamageBlockJson block, bool mainHand)
		{
			this.StartBlock(block, mainHand, EnumDamageBlockKind.Unknown);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x000407CC File Offset: 0x0003E9CC
		public void StartBlock(DamageBlockJson block, bool mainHand, EnumDamageBlockKind kind)
		{
			DamageBlockPacket packet = block.ToPacket();
			packet.Id = 0UL;
			packet.MainHand = mainHand;
			packet.Kind = kind;
			this._clientChannel.SendPacket<DamageBlockPacket>(packet);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00040802 File Offset: 0x0003EA02
		public void StartBlock(DamageBlockJson block, bool mainHand, Action callback)
		{
			this.StartBlock(block, mainHand, EnumDamageBlockKind.Unknown, callback);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00040810 File Offset: 0x0003EA10
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

		// Token: 0x06000D14 RID: 3348 RVA: 0x00040864 File Offset: 0x0003EA64
		public void StopBlock(bool mainHand)
		{
			this._clientChannel.SendPacket<DamageStopBlockPacket>(new DamageStopBlockPacket
			{
				MainHand = mainHand
			});
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00040880 File Offset: 0x0003EA80
		public void HandleCallback(DamageBlockCallbackPacket packet)
		{
			Action callback;
			if (this._callbacks.TryGetValue(packet.Id, ref callback))
			{
				callback.Invoke();
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x000408A8 File Offset: 0x0003EAA8
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

		// Token: 0x04000668 RID: 1640
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x04000669 RID: 1641
		private readonly Queue<ulong> _ids = new Queue<ulong>();

		// Token: 0x0400066A RID: 1642
		private readonly Dictionary<ulong, Action> _callbacks = new Dictionary<ulong, Action>();

		// Token: 0x0400066B RID: 1643
		private const int _queueSize = 10;

		// Token: 0x0400066C RID: 1644
		private ulong _nextId = 1UL;
	}
}
