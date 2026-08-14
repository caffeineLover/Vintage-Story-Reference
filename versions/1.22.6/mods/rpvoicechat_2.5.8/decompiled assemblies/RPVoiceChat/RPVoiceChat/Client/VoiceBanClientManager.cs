using System;
using System.Collections.Generic;
using RPVoiceChat.Networking;
using Vintagestory.API.Client;

namespace RPVoiceChat.Client
{
	// Token: 0x02000088 RID: 136
	public class VoiceBanClientManager
	{
		// Token: 0x06000490 RID: 1168 RVA: 0x00018A68 File Offset: 0x00016C68
		public VoiceBanClientManager(ICoreClientAPI api)
		{
			this.capi = api;
			this.voiceBanChannel = api.Network.RegisterChannel("RPVoiceBan").RegisterMessageType<VoiceBanStatusPacket>().SetMessageHandler<VoiceBanStatusPacket>(new NetworkServerMessageHandler<VoiceBanStatusPacket>(this.OnVoiceBanStatusReceived));
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00018AB9 File Offset: 0x00016CB9
		public bool IsPlayerBanned(string playerUID)
		{
			return this.bannedPlayers.Contains(playerUID);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00018AC8 File Offset: 0x00016CC8
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

		// Token: 0x0400023D RID: 573
		private ICoreClientAPI capi;

		// Token: 0x0400023E RID: 574
		private HashSet<string> bannedPlayers = new HashSet<string>();

		// Token: 0x0400023F RID: 575
		private IClientNetworkChannel voiceBanChannel;
	}
}
