using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F4 RID: 244
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ImpaleSystemClient
	{
		// Token: 0x06000C4D RID: 3149 RVA: 0x0003D88C File Offset: 0x0003BA8C
		public ImpaleSystemClient(ICoreClientAPI api)
		{
			this._channel = api.Network.RegisterChannel("CombatOverhaul:impale").RegisterMessageType<ImpaleAttachPacket>().RegisterMessageType<ImpaleReleasePacket>().RegisterMessageType<ImpaleThrowRequestPacket>().SetMessageHandler<ImpaleAttachPacket>(new NetworkServerMessageHandler<ImpaleAttachPacket>(this.HandlePacket)).SetMessageHandler<ImpaleReleasePacket>(new NetworkServerMessageHandler<ImpaleReleasePacket>(this.HandlePacket));
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0003D8F1 File Offset: 0x0003BAF1
		public bool HasImpaled(long attackerEntityId, bool mainHand)
		{
			return this._states.ContainsKey(new ImpaleKey(attackerEntityId, mainHand));
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0003D908 File Offset: 0x0003BB08
		public void RequestThrow(bool mainHand, Vec3f direction)
		{
			this._channel.SendPacket<ImpaleThrowRequestPacket>(new ImpaleThrowRequestPacket
			{
				MainHand = mainHand,
				Direction = new float[]
				{
					direction.X,
					direction.Y,
					direction.Z
				}
			});
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0003D955 File Offset: 0x0003BB55
		private void HandlePacket(ImpaleAttachPacket packet)
		{
			this._states[new ImpaleKey(packet.AttackerEntityId, packet.MainHand)] = packet.TargetEntityId;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x0003D979 File Offset: 0x0003BB79
		private void HandlePacket(ImpaleReleasePacket packet)
		{
			this._states.Remove(new ImpaleKey(packet.AttackerEntityId, packet.MainHand));
		}

		// Token: 0x040005E7 RID: 1511
		public const string ChannelId = "CombatOverhaul:impale";

		// Token: 0x040005E8 RID: 1512
		private readonly IClientNetworkChannel _channel;

		// Token: 0x040005E9 RID: 1513
		private readonly Dictionary<ImpaleKey, long> _states = new Dictionary<ImpaleKey, long>();
	}
}
