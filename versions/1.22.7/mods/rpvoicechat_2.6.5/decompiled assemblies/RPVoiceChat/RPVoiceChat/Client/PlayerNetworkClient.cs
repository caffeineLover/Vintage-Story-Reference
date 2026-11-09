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
	// Token: 0x020000B0 RID: 176
	public class PlayerNetworkClient : IDisposable
	{
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x0600059F RID: 1439 RVA: 0x0001E9CC File Offset: 0x0001CBCC
		// (remove) Token: 0x060005A0 RID: 1440 RVA: 0x0001EA04 File Offset: 0x0001CC04
		public event Action<AudioPacket> OnAudioReceived;

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0001EA39 File Offset: 0x0001CC39
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

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001EA5C File Offset: 0x0001CC5C
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

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001EB08 File Offset: 0x0001CD08
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

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001EBB8 File Offset: 0x0001CDB8
		private void OnConnectionRequest(ConnectionRequest connectionRequest)
		{
			this.serverConnections = connectionRequest.SupportedTransports;
			this.Connect();
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001EBCC File Offset: 0x0001CDCC
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

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001ECC4 File Offset: 0x0001CEC4
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

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001EDC0 File Offset: 0x0001CFC0
		private void AudioPacketReceived(AudioPacket packet)
		{
			Action<AudioPacket> onAudioReceived = this.OnAudioReceived;
			if (onAudioReceived == null)
			{
				return;
			}
			onAudioReceived.Invoke(packet);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001EDD4 File Offset: 0x0001CFD4
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

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001EE44 File Offset: 0x0001D044
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

		// Token: 0x060005AA RID: 1450 RVA: 0x0001EEE4 File Offset: 0x0001D0E4
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

		// Token: 0x040002D8 RID: 728
		private ICoreClientAPI api;

		// Token: 0x040002D9 RID: 729
		private List<INetworkClient> _initialTransports;

		// Token: 0x040002DA RID: 730
		private List<INetworkClient> activeTransports = new List<INetworkClient>();

		// Token: 0x040002DB RID: 731
		private IClientNetworkChannel handshakeChannel;

		// Token: 0x040002DC RID: 732
		private ConnectionInfo[] serverConnections;

		// Token: 0x040002DD RID: 733
		private bool isConnected;

		// Token: 0x040002DE RID: 734
		private bool isDisposed;
	}
}
