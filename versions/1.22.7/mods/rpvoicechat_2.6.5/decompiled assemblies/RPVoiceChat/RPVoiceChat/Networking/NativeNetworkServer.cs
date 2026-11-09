using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Server;
using Vintagestory.Server;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200006B RID: 107
	public class NativeNetworkServer : NativeNetworkBase, INetworkServer, IDisposable
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000295 RID: 661 RVA: 0x00010638 File Offset: 0x0000E838
		// (remove) Token: 0x06000296 RID: 662 RVA: 0x00010670 File Offset: 0x0000E870
		public event Action<AudioPacket> AudioPacketReceived;

		// Token: 0x06000297 RID: 663 RVA: 0x000106A8 File Offset: 0x0000E8A8
		public NativeNetworkServer(ICoreServerAPI sapi) : base(sapi)
		{
			this.api = sapi;
			this.channel = this.api.Network.GetChannel("RPAudioChannel").SetMessageHandler<AudioPacket>(new NetworkClientMessageHandler<AudioPacket>(this.ReceivedAudioPacketFromClient));
			if (!this.api.Server.IsDedicated)
			{
				this.api.Network.RegisterChannel("RPAudioChannel_SP").RegisterMessageType<AudioPacket>().SetMessageHandler<AudioPacket>(new NetworkClientMessageHandler<AudioPacket>(this.ReceivedAudioPacketFromClient));
			}
			NetworkAPIPatch.OnHandleCustomPacket += new Func<int, bool>(this.ShouldProcessInBackground);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0001073E File Offset: 0x0000E93E
		public void Launch()
		{
			this.networkProcess = new ServerSystemNetworkProcess(this.api);
			this.networkProcess.OnProcessInBackground += new Func<int, Packet_CustomPacket, IServerPlayer, bool>(this.ProcessInBackground);
			this.networkProcess.Launch();
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00010773 File Offset: 0x0000E973
		public ConnectionInfo GetConnectionInfo()
		{
			return new ConnectionInfo();
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0001077C File Offset: 0x0000E97C
		public bool SendPacket(NetworkPacket packet, string playerId)
		{
			IServerPlayer player = this.api.World.PlayerByUid(playerId) as IServerPlayer;
			this.channel.SendPacket<AudioPacket>(packet as AudioPacket, new IServerPlayer[]
			{
				player
			});
			return true;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000107BC File Offset: 0x0000E9BC
		private bool ProcessInBackground(int channelId, Packet_CustomPacket customPacket, IServerPlayer sender)
		{
			if (!this.ShouldProcessInBackground(channelId))
			{
				return false;
			}
			((NetworkChannel)this.channel).OnPacket(customPacket, sender);
			return true;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000107DC File Offset: 0x0000E9DC
		private bool ShouldProcessInBackground(int channelId)
		{
			if (!(this.channel is NetworkChannel))
			{
				return false;
			}
			int expectedChannelId = (int)NativeNetworkServer.channelIdField.GetValue(this.channel);
			return channelId == expectedChannelId;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00010812 File Offset: 0x0000EA12
		private void ReceivedAudioPacketFromClient(IServerPlayer player, AudioPacket packet)
		{
			packet.PlayerId = player.PlayerUID;
			Action<AudioPacket> audioPacketReceived = this.AudioPacketReceived;
			if (audioPacketReceived == null)
			{
				return;
			}
			audioPacketReceived.Invoke(packet);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00010831 File Offset: 0x0000EA31
		public void Dispose()
		{
			NetworkAPIPatch.OnHandleCustomPacket -= new Func<int, bool>(this.ShouldProcessInBackground);
			ServerSystemNetworkProcess serverSystemNetworkProcess = this.networkProcess;
			if (serverSystemNetworkProcess == null)
			{
				return;
			}
			serverSystemNetworkProcess.Dispose();
		}

		// Token: 0x04000119 RID: 281
		private ICoreServerAPI api;

		// Token: 0x0400011A RID: 282
		private IServerNetworkChannel channel;

		// Token: 0x0400011B RID: 283
		private ServerSystemNetworkProcess networkProcess;

		// Token: 0x0400011C RID: 284
		private static FieldInfo channelIdField = AccessTools.Field(typeof(NetworkChannel), "channelId");
	}
}
