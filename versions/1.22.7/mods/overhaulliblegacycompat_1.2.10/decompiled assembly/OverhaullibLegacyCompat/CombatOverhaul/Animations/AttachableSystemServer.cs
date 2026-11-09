using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Server;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A4 RID: 420
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AttachableSystemServer
	{
		// Token: 0x06001434 RID: 5172 RVA: 0x00063AB8 File Offset: 0x00061CB8
		public AttachableSystemServer(ICoreServerAPI api)
		{
			this._serverChannel = api.Network.RegisterChannel("CO-attachablesystem").RegisterMessageType<AttachableAttachPacket>().RegisterMessageType<AttachableClearPacket>().RegisterMessageType<AttachableSwitchModelsPacket>().SetMessageHandler<AttachableAttachPacket>(new NetworkClientMessageHandler<AttachableAttachPacket>(this.HandlePacket)).SetMessageHandler<AttachableSwitchModelsPacket>(new NetworkClientMessageHandler<AttachableSwitchModelsPacket>(this.HandlePacket)).SetMessageHandler<AttachableClearPacket>(new NetworkClientMessageHandler<AttachableClearPacket>(this.HandlePacket));
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x00063B23 File Offset: 0x00061D23
		private void HandlePacket(IServerPlayer player, AttachableAttachPacket packet)
		{
			this._serverChannel.BroadcastPacket<AttachableAttachPacket>(packet, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x00063B3B File Offset: 0x00061D3B
		private void HandlePacket(IServerPlayer player, AttachableClearPacket packet)
		{
			this._serverChannel.BroadcastPacket<AttachableClearPacket>(packet, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x00063B53 File Offset: 0x00061D53
		private void HandlePacket(IServerPlayer player, AttachableSwitchModelsPacket packet)
		{
			this._serverChannel.BroadcastPacket<AttachableSwitchModelsPacket>(packet, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x040009F9 RID: 2553
		private readonly IServerNetworkChannel _serverChannel;
	}
}
