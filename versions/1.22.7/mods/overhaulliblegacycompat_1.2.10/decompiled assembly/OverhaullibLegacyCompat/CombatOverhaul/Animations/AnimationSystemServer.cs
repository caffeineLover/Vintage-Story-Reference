using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Server;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000177 RID: 375
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnimationSystemServer
	{
		// Token: 0x060012A1 RID: 4769 RVA: 0x0005A460 File Offset: 0x00058660
		public AnimationSystemServer(ICoreServerAPI api)
		{
			this._serverChannel = api.Network.RegisterChannel("CO-animation-system").RegisterMessageType<AnimationRequestPacket>().RegisterMessageType<AnimationStopRequestPacket>().SetMessageHandler<AnimationRequestPacket>(new NetworkClientMessageHandler<AnimationRequestPacket>(this.HandlePacket)).SetMessageHandler<AnimationStopRequestPacket>(new NetworkClientMessageHandler<AnimationStopRequestPacket>(this.HandlePacket));
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0005A4B5 File Offset: 0x000586B5
		private void HandlePacket(IServerPlayer player, AnimationRequestPacket packet)
		{
			this._serverChannel.BroadcastPacket<AnimationRequestPacket>(packet, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0005A4CD File Offset: 0x000586CD
		private void HandlePacket(IServerPlayer player, AnimationStopRequestPacket packet)
		{
			this._serverChannel.BroadcastPacket<AnimationStopRequestPacket>(packet, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x040008AC RID: 2220
		private readonly IServerNetworkChannel _serverChannel;
	}
}
