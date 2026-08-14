using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200004D RID: 77
	public class NativeNetworkClient : NativeNetworkBase, INetworkClient, IDisposable
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060001F2 RID: 498 RVA: 0x0000C9A8 File Offset: 0x0000ABA8
		// (remove) Token: 0x060001F3 RID: 499 RVA: 0x0000C9E0 File Offset: 0x0000ABE0
		public event Action<AudioPacket> OnAudioReceived;

		// Token: 0x060001F4 RID: 500 RVA: 0x0000CA18 File Offset: 0x0000AC18
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

		// Token: 0x060001F5 RID: 501 RVA: 0x0000CA8E File Offset: 0x0000AC8E
		public bool SendAudioToServer(AudioPacket packet)
		{
			(this.singleplayerChannel ?? this.channel).SendPacket<AudioPacket>(packet);
			return true;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000CAA8 File Offset: 0x0000ACA8
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

		// Token: 0x060001F7 RID: 503 RVA: 0x0000CB28 File Offset: 0x0000AD28
		private void HandleAudioPacket(AudioPacket packet)
		{
			Action<AudioPacket> onAudioReceived = this.OnAudioReceived;
			if (onAudioReceived == null)
			{
				return;
			}
			onAudioReceived.Invoke(packet);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000CB3B File Offset: 0x0000AD3B
		public void Dispose()
		{
			SystemNetworkProcessPatch.OnProcessInBackground -= new Func<int, Packet_CustomPacket, bool>(this.ProcessInBackground);
		}

		// Token: 0x040000C2 RID: 194
		private IClientNetworkChannel channel;

		// Token: 0x040000C3 RID: 195
		private IClientNetworkChannel singleplayerChannel;

		// Token: 0x040000C4 RID: 196
		private ICoreClientAPI capi;

		// Token: 0x040000C5 RID: 197
		private static FieldInfo channelIdField = AccessTools.Field(typeof(NetworkChannel), "channelId");
	}
}
