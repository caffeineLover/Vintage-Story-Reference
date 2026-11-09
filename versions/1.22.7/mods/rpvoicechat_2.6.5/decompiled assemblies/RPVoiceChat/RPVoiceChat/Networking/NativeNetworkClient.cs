using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200006A RID: 106
	public class NativeNetworkClient : NativeNetworkBase, INetworkClient, IDisposable
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600028D RID: 653 RVA: 0x00010474 File Offset: 0x0000E674
		// (remove) Token: 0x0600028E RID: 654 RVA: 0x000104AC File Offset: 0x0000E6AC
		public event Action<AudioPacket> OnAudioReceived;

		// Token: 0x0600028F RID: 655 RVA: 0x000104E4 File Offset: 0x0000E6E4
		public NativeNetworkClient(ICoreClientAPI api) : base(api)
		{
			this.capi = api;
			this.channel = api.Network.GetChannel("RPAudioChannel").SetMessageHandler<AudioPacket>(new NetworkServerMessageHandler<AudioPacket>(this.HandleAudioPacket));
			if (api.IsSinglePlayer)
			{
				this.singleplayerChannel = api.Network.RegisterChannel("RPAudioChannel_SP").RegisterMessageType<AudioPacket>();
			}
			SystemNetworkProcessPatch.OnProcessInBackground += new Func<int, Packet_CustomPacket, bool>(this.ProcessInBackground);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0001055A File Offset: 0x0000E75A
		public bool SendAudioToServer(AudioPacket packet)
		{
			(this.singleplayerChannel ?? this.channel).SendPacket<AudioPacket>(packet);
			return true;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00010574 File Offset: 0x0000E774
		private bool ProcessInBackground(int channelId, Packet_CustomPacket customPacket)
		{
			NetworkChannel nativeChannel = this.channel as NetworkChannel;
			if (nativeChannel == null)
			{
				return false;
			}
			int expectedChannelId = (int)NativeNetworkClient.channelIdField.GetValue(this.channel);
			if (channelId != expectedChannelId)
			{
				return false;
			}
			try
			{
				nativeChannel.OnPacket(customPacket);
			}
			catch (Exception e)
			{
				ILogger logger = this.capi.Logger;
				string text = "Error while processing packet: ";
				Exception ex = e;
				logger.Error(text + ((ex != null) ? ex.ToString() : null));
			}
			return true;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000105F4 File Offset: 0x0000E7F4
		private void HandleAudioPacket(AudioPacket packet)
		{
			Action<AudioPacket> onAudioReceived = this.OnAudioReceived;
			if (onAudioReceived == null)
			{
				return;
			}
			onAudioReceived.Invoke(packet);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00010607 File Offset: 0x0000E807
		public void Dispose()
		{
			SystemNetworkProcessPatch.OnProcessInBackground -= new Func<int, Packet_CustomPacket, bool>(this.ProcessInBackground);
		}

		// Token: 0x04000114 RID: 276
		private IClientNetworkChannel channel;

		// Token: 0x04000115 RID: 277
		private IClientNetworkChannel singleplayerChannel;

		// Token: 0x04000116 RID: 278
		private ICoreClientAPI capi;

		// Token: 0x04000117 RID: 279
		private static FieldInfo channelIdField = AccessTools.Field(typeof(NetworkChannel), "channelId");
	}
}
