using System;
using System.Collections.Generic;
using RPVoiceChat.Networking;
using Vintagestory.API.Client;

namespace RPVoiceChat.Client
{
	// Token: 0x020000B1 RID: 177
	public class VoiceBanClientManager
	{
		// Token: 0x060005AB RID: 1451 RVA: 0x0001EF44 File Offset: 0x0001D144
		public VoiceBanClientManager(ICoreClientAPI api)
		{
			this.capi = api;
			this.voiceBanChannel = api.Network.RegisterChannel("RPVoiceBan").RegisterMessageType<VoiceBanStatusPacket>().SetMessageHandler<VoiceBanStatusPacket>(new NetworkServerMessageHandler<VoiceBanStatusPacket>(this.OnVoiceBanStatusReceived));
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001EF95 File Offset: 0x0001D195
		public bool IsPlayerBanned(string playerUID)
		{
			return this.bannedPlayers.Contains(playerUID);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001EFA4 File Offset: 0x0001D1A4
		private void OnVoiceBanStatusReceived(VoiceBanStatusPacket packet)
		{
			this.capi.Event.EnqueueMainThreadTask(delegate()
			{
				if (packet.IsBanned)
				{
					this.bannedPlayers.Add(packet.PlayerId);
				}
				else
				{
					this.bannedPlayers.Remove(packet.PlayerId);
				}
				this.capi.Event.PushEvent("rpvoicechat:voiceBanUpdate", null);
			}, "rpvoicechat:VoiceBanUpdate");
		}

		// Token: 0x040002DF RID: 735
		private ICoreClientAPI capi;

		// Token: 0x040002E0 RID: 736
		private HashSet<string> bannedPlayers = new HashSet<string>();

		// Token: 0x040002E1 RID: 737
		private IClientNetworkChannel voiceBanChannel;
	}
}
