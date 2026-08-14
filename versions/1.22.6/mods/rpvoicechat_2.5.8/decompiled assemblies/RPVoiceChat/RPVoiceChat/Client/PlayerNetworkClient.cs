using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RPVoiceChat.Config;
using RPVoiceChat.Networking;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

namespace RPVoiceChat.Client
{
	// Token: 0x02000087 RID: 135
	public class PlayerNetworkClient : IDisposable
	{
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000484 RID: 1156 RVA: 0x000184F0 File Offset: 0x000166F0
		// (remove) Token: 0x06000485 RID: 1157 RVA: 0x00018528 File Offset: 0x00016728
		public event Action<AudioPacket> OnAudioReceived;

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x0001855D File Offset: 0x0001675D
		private bool isShutdown
		{
			get
			{
				if (!this.isDisposed)
				{
					ClientCoreAPI clientCoreAPI = this.api as ClientCoreAPI;
					return clientCoreAPI != null && clientCoreAPI.disposed;
				}
				return true;
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00018580 File Offset: 0x00016780
		public PlayerNetworkClient(ICoreClientAPI capi, List<INetworkClient> clientTransports)
		{
			this.api = capi;
			this._initialTransports = clientTransports;
			this.handshakeChannel = capi.Network.RegisterChannel("RPVCHandshake").RegisterMessageType<ConnectionRequest>().RegisterMessageType<ConnectionInfo>().SetMessageHandler<ConnectionRequest>(new NetworkServerMessageHandler<ConnectionRequest>(this.OnConnectionRequest));
			foreach (INetworkClient networkClient in this._initialTransports)
			{
				networkClient.OnAudioReceived += new Action<AudioPacket>(this.AudioPacketReceived);
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001862C File Offset: 0x0001682C
		public void SendAudioToServer(AudioPacket packet)
		{
			if (this.isShutdown || !this.isConnected)
			{
				return;
			}
			foreach (INetworkClient transport in this.activeTransports)
			{
				try
				{
					if (transport.SendAudioToServer(packet))
					{
						return;
					}
				}
				catch (Exception e)
				{
					Logger.client.Warning("Couldn't use " + transport.GetTransportID() + " client to send audio packet to server: " + e.Message);
				}
			}
			Logger.client.Warning("Failed to send audio packet to server: All active network clients reported a failure");
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000186DC File Offset: 0x000168DC
		private void OnConnectionRequest(ConnectionRequest connectionRequest)
		{
			this.serverConnections = connectionRequest.SupportedTransports;
			this.Connect();
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000186F0 File Offset: 0x000168F0
		private void Connect()
		{
			this.activeTransports = new List<INetworkClient>();
			foreach (INetworkClient transport in this._initialTransports)
			{
				try
				{
					this.ConnectWith(transport);
				}
				catch (Exception e)
				{
					Exception e2;
					Logger.client.Warning("Failed to connect with the " + transport.GetTransportID() + " client: " + e2.Message);
					transport.Dispose();
				}
			}
			this.isConnected = true;
			if (this.activeTransports.Count > 0)
			{
				return;
			}
			IEnumerable<string> serverTransportIDs = Enumerable.Select<ConnectionInfo, string>(this.serverConnections, (ConnectionInfo e) => e.Transport);
			throw new Exception("Failed to connect to the server. Supported transports: " + string.Join(", ", serverTransportIDs));
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x000187E8 File Offset: 0x000169E8
		private void ConnectWith(INetworkClient transport)
		{
			string transportID = transport.GetTransportID();
			Logger.client.Notification("Attempting to connect with " + transportID + " client");
			ConnectionInfo serverConnection = Enumerable.FirstOrDefault<ConnectionInfo>(this.serverConnections, (ConnectionInfo e) => e.Transport == transportID);
			if (serverConnection == null)
			{
				Logger.client.Notification("Server doesn't support " + transportID + " transport, aborting");
				return;
			}
			ConnectionInfo clientConnection = new ConnectionInfo();
			IExtendedNetworkClient extendedTransport = transport as IExtendedNetworkClient;
			if (extendedTransport != null)
			{
				clientConnection = ((extendedTransport != null) ? extendedTransport.Connect(serverConnection) : null);
				extendedTransport.OnConnectionLost += new Action<bool, IExtendedNetworkClient>(this.ConnectionLost);
			}
			clientConnection.Transport = transportID;
			clientConnection.DevicesVoiceFeedback = ModConfig.ClientConfig.DevicesVoiceFeedback;
			this.handshakeChannel.SendPacket<ConnectionInfo>(clientConnection);
			this.activeTransports.Add(transport);
			Logger.client.Notification("Successfully connected with the " + transportID + " client");
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x000188E4 File Offset: 0x00016AE4
		private void AudioPacketReceived(AudioPacket packet)
		{
			Action<AudioPacket> onAudioReceived = this.OnAudioReceived;
			if (onAudioReceived == null)
			{
				return;
			}
			onAudioReceived.Invoke(packet);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x000188F8 File Offset: 0x00016AF8
		private void ConnectionLost(bool canReconnect, IExtendedNetworkClient transport)
		{
			string transportID = transport.GetTransportID();
			Logger.client.Notification(transportID + " transport reported connection loss");
			transport.OnConnectionLost -= new Action<bool, IExtendedNetworkClient>(this.ConnectionLost);
			if (this.isShutdown)
			{
				return;
			}
			if (canReconnect && this.Reconnect(transport))
			{
				return;
			}
			if (this.isShutdown)
			{
				return;
			}
			this.activeTransports.Remove(transport);
			transport.Dispose();
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00018968 File Offset: 0x00016B68
		private bool Reconnect(IExtendedNetworkClient transport)
		{
			Logger.client.Notification("Reconnecting...");
			try
			{
				this.ConnectWith(transport);
				return true;
			}
			catch (Exception e)
			{
				if (this.isShutdown)
				{
					Logger.client.Notification("Aborting due to mod unloading.");
					return false;
				}
				Logger client = Logger.client;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(32, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to reconnect to ");
				defaultInterpolatedStringHandler.AppendFormatted(transport.GetTransportID());
				defaultInterpolatedStringHandler.AppendLiteral(" server:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
				client.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return false;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00018A08 File Offset: 0x00016C08
		public void Dispose()
		{
			if (this.isDisposed)
			{
				return;
			}
			this.isDisposed = true;
			foreach (INetworkClient networkClient in this.activeTransports)
			{
				networkClient.Dispose();
			}
		}

		// Token: 0x04000236 RID: 566
		private ICoreClientAPI api;

		// Token: 0x04000237 RID: 567
		private List<INetworkClient> _initialTransports;

		// Token: 0x04000238 RID: 568
		private List<INetworkClient> activeTransports = new List<INetworkClient>();

		// Token: 0x04000239 RID: 569
		private IClientNetworkChannel handshakeChannel;

		// Token: 0x0400023A RID: 570
		private ConnectionInfo[] serverConnections;

		// Token: 0x0400023B RID: 571
		private bool isConnected;

		// Token: 0x0400023C RID: 572
		private bool isDisposed;
	}
}
