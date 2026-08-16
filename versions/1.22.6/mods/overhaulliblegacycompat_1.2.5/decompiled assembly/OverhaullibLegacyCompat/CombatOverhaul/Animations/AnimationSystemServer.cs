using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Server;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200016B RID: 363
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnimationSystemServer
	{
		// Token: 0x0600122D RID: 4653 RVA: 0x00058B40 File Offset: 0x00056D40
		public AnimationSystemServer(ICoreServerAPI api)
		{
			this._serverChannel = api.Network.RegisterChannel("CO-animation-system").RegisterMessageType<AnimationRequestPacket>().RegisterMessageType<AnimationStopRequestPacket>().SetMessageHandler<AnimationRequestPacket>(new NetworkClientMessageHandler<AnimationRequestPacket>(this.HandlePacket)).SetMessageHandler<AnimationStopRequestPacket>(new NetworkClientMessageHandler<AnimationStopRequestPacket>(this.HandlePacket));
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x00058B95 File Offset: 0x00056D95
		private void HandlePacket(IServerPlayer player, AnimationRequestPacket packet)
		{
			this._serverChannel.BroadcastPacket<AnimationRequestPacket>(packet, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00058BAD File Offset: 0x00056DAD
		private void HandlePacket(IServerPlayer player, AnimationStopRequestPacket packet)
		{
			this._serverChannel.BroadcastPacket<AnimationStopRequestPacket>(packet, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x04000886 RID: 2182
		private readonly IServerNetworkChannel _serverChannel;
	}
}
