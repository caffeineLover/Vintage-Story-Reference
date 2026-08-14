using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Server;
using Vintagestory.Server;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200004E RID: 78
	public class NativeNetworkServer : NativeNetworkBase, INetworkServer, IDisposable
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060001FA RID: 506 RVA: 0x0000CB6C File Offset: 0x0000AD6C
		// (remove) Token: 0x060001FB RID: 507 RVA: 0x0000CBA4 File Offset: 0x0000ADA4
		public event Action<AudioPacket> AudioPacketReceived;

		// Token: 0x060001FC RID: 508 RVA: 0x0000CBDC File Offset: 0x0000ADDC
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

		// Token: 0x060001FD RID: 509 RVA: 0x0000CC72 File Offset: 0x0000AE72
		public void Launch()
		{
			this.networkProcess = new ServerSystemNetworkProcess(this.api);
			this.networkProcess.OnProcessInBackground += new Func<int, Packet_CustomPacket, IServerPlayer, bool>(this.ProcessInBackground);
			this.networkProcess.Launch();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000CCA7 File Offset: 0x0000AEA7
		public ConnectionInfo GetConnectionInfo()
		{
			return new ConnectionInfo();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000CCB0 File Offset: 0x0000AEB0
		public bool SendPacket(NetworkPacket packet, string playerId)
		{
			IServerPlayer player = this.api.World.PlayerByUid(playerId) as IServerPlayer;
			this.channel.SendPacket<AudioPacket>(packet as AudioPacket, new IServerPlayer[]
			{
				player
			});
			return true;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000CCF0 File Offset: 0x0000AEF0
		private bool ProcessInBackground(int channelId, Packet_CustomPacket customPacket, IServerPlayer sender)
		{
			if (!this.ShouldProcessInBackground(channelId))
			{
				return false;
			}
			((NetworkChannel)this.channel).OnPacket(customPacket, sender);
			return true;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000CD10 File Offset: 0x0000AF10
		private bool ShouldProcessInBackground(int channelId)
		{
			if (!(this.channel is NetworkChannel))
			{
				return false;
			}
			int expectedChannelId = (int)NativeNetworkServer.channelIdField.GetValue(this.channel);
			return channelId == expectedChannelId;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000CD46 File Offset: 0x0000AF46
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

		// Token: 0x06000203 RID: 515 RVA: 0x0000CD65 File Offset: 0x0000AF65
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

		// Token: 0x040000C7 RID: 199
		private ICoreServerAPI api;

		// Token: 0x040000C8 RID: 200
		private IServerNetworkChannel channel;

		// Token: 0x040000C9 RID: 201
		private ServerSystemNetworkProcess networkProcess;

		// Token: 0x040000CA RID: 202
		private static FieldInfo channelIdField = AccessTools.Field(typeof(NetworkChannel), "channelId");
	}
}
