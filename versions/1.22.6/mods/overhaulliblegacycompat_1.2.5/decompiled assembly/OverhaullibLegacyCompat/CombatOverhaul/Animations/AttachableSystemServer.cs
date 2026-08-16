using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Server;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000196 RID: 406
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AttachableSystemServer
	{
		// Token: 0x060013BC RID: 5052 RVA: 0x00062038 File Offset: 0x00060238
		public AttachableSystemServer(ICoreServerAPI api)
		{
			this._serverChannel = api.Network.RegisterChannel("CO-attachablesystem").RegisterMessageType<AttachableAttachPacket>().RegisterMessageType<AttachableClearPacket>().RegisterMessageType<AttachableSwitchModelsPacket>().SetMessageHandler<AttachableAttachPacket>(new NetworkClientMessageHandler<AttachableAttachPacket>(this.HandlePacket)).SetMessageHandler<AttachableSwitchModelsPacket>(new NetworkClientMessageHandler<AttachableSwitchModelsPacket>(this.HandlePacket)).SetMessageHandler<AttachableClearPacket>(new NetworkClientMessageHandler<AttachableClearPacket>(this.HandlePacket));
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x000620A3 File Offset: 0x000602A3
		private void HandlePacket(IServerPlayer player, AttachableAttachPacket packet)
		{
			this._serverChannel.BroadcastPacket<AttachableAttachPacket>(packet, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x000620BB File Offset: 0x000602BB
		private void HandlePacket(IServerPlayer player, AttachableClearPacket packet)
		{
			this._serverChannel.BroadcastPacket<AttachableClearPacket>(packet, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x000620D3 File Offset: 0x000602D3
		private void HandlePacket(IServerPlayer player, AttachableSwitchModelsPacket packet)
		{
			this._serverChannel.BroadcastPacket<AttachableSwitchModelsPacket>(packet, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x040009D1 RID: 2513
		private readonly IServerNetworkChannel _serverChannel;
	}
}
