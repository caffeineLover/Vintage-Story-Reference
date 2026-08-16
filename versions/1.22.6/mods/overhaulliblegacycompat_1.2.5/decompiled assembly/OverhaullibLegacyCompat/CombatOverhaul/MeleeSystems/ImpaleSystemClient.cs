using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000E9 RID: 233
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ImpaleSystemClient
	{
		// Token: 0x06000BF1 RID: 3057 RVA: 0x0003C89C File Offset: 0x0003AA9C
		public ImpaleSystemClient(ICoreClientAPI api)
		{
			this._channel = api.Network.RegisterChannel("CombatOverhaul:impale").RegisterMessageType<ImpaleAttachPacket>().RegisterMessageType<ImpaleReleasePacket>().RegisterMessageType<ImpaleThrowRequestPacket>().SetMessageHandler<ImpaleAttachPacket>(new NetworkServerMessageHandler<ImpaleAttachPacket>(this.HandlePacket)).SetMessageHandler<ImpaleReleasePacket>(new NetworkServerMessageHandler<ImpaleReleasePacket>(this.HandlePacket));
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x0003C901 File Offset: 0x0003AB01
		public bool HasImpaled(long attackerEntityId, bool mainHand)
		{
			return this._states.ContainsKey(new ImpaleKey(attackerEntityId, mainHand));
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0003C918 File Offset: 0x0003AB18
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

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0003C965 File Offset: 0x0003AB65
		private void HandlePacket(ImpaleAttachPacket packet)
		{
			this._states[new ImpaleKey(packet.AttackerEntityId, packet.MainHand)] = packet.TargetEntityId;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0003C989 File Offset: 0x0003AB89
		private void HandlePacket(ImpaleReleasePacket packet)
		{
			this._states.Remove(new ImpaleKey(packet.AttackerEntityId, packet.MainHand));
		}

		// Token: 0x040005C5 RID: 1477
		public const string ChannelId = "CombatOverhaul:impale";

		// Token: 0x040005C6 RID: 1478
		private readonly IClientNetworkChannel _channel;

		// Token: 0x040005C7 RID: 1479
		private readonly Dictionary<ImpaleKey, long> _states = new Dictionary<ImpaleKey, long>();
	}
}
