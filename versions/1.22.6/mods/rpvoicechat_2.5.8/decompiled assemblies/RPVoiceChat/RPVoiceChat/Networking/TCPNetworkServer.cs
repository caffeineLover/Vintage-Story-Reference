using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Open.Nat;
using RPVoiceChat.Util;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200005A RID: 90
	public class TCPNetworkServer : TCPNetworkBase, IExtendedNetworkServer, INetworkServer, IDisposable
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000270 RID: 624 RVA: 0x0000E154 File Offset: 0x0000C354
		// (remove) Token: 0x06000271 RID: 625 RVA: 0x0000E18C File Offset: 0x0000C38C
		public event Action<AudioPacket> AudioPacketReceived;

		// Token: 0x06000272 RID: 626 RVA: 0x0000E1C4 File Offset: 0x0000C3C4
		public TCPNetworkServer(int port, string ip, bool forwardPorts) : base(Logger.server)
		{
			this.port = port;
			this.ip = IPAddress.Parse(ip ?? NetworkUtils.GetPublicIP());
			this.upnpEnabled = forwardPorts;
			this.ownEndPoint = NetworkUtils.GetEndPoint(this.GetConnectionInfo());
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000E226 File Offset: 0x0000C426
		public void Launch()
		{
			if (!NetworkUtils.IsInternalNetwork(this.ip))
			{
				this.SetupUpnp(this.port);
			}
			this.StartServer();
			this.VerifyServerReadiness();
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000E250 File Offset: 0x0000C450
		public ConnectionInfo GetConnectionInfo()
		{
			if (this.connectionInfo != null)
			{
				return this.connectionInfo;
			}
			this.connectionInfo = new ConnectionInfo
			{
				Address = this.ip.MapToIPv4().ToString(),
				Port = this.port
			};
			return this.connectionInfo;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000E2A0 File Offset: 0x0000C4A0
		public bool SendPacket(NetworkPacket packet, string playerId)
		{
			TCPConnection connection = this.ResolveConnection(playerId);
			if (connection == null)
			{
				return false;
			}
			byte[] data = packet.ToBytes();
			connection.Send(data);
			return true;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000E2CC File Offset: 0x0000C4CC
		public void PlayerConnected(string playerId, ConnectionInfo connectionInfo)
		{
			if (this.playerAddresses.ContainsKey(playerId))
			{
				this.PlayerDisconnected(playerId);
			}
			string playerAddress = NetworkUtils.GetEndPoint(connectionInfo).ToString();
			this.playerAddresses.Add(playerId, playerAddress);
			this.logger.VerboseDebug(playerId + " connected over CustomTCP");
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000E31D File Offset: 0x0000C51D
		public void PlayerDisconnected(string playerId)
		{
			if (!this.playerAddresses.ContainsKey(playerId))
			{
				return;
			}
			this.playerAddresses.Remove(playerId);
			this.logger.VerboseDebug(playerId + " disconnected from CustomTCP server");
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000E354 File Offset: 0x0000C554
		private void StartServer()
		{
			this.socket = new Socket(2, 1, 6);
			this.socket.ReceiveTimeout = -1;
			this.socket.Bind(new IPEndPoint(IPAddress.Any, this.port));
			this.socket.Listen(10);
			this._listeningCTS = new CancellationTokenSource();
			this._listeningThread = new Thread(new ParameterizedThreadStart(this.AcceptConnection));
			this._listeningThread.Start(this._listeningCTS.Token);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000E3E0 File Offset: 0x0000C5E0
		private void AcceptConnection(object cancellationToken)
		{
			CancellationToken ct = (CancellationToken)cancellationToken;
			while (this._listeningThread.IsAlive && !ct.IsCancellationRequested)
			{
				try
				{
					Socket connectionSocket = this.socket.Accept();
					this.ConnectionAccepted(connectionSocket);
				}
				catch (Exception e)
				{
					SocketException se = e as SocketException;
					if ((se != null && (se.SocketErrorCode == 10004 || se.SocketErrorCode == 995)) || ct.IsCancellationRequested)
					{
						break;
					}
					Logger logger = this.logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(88, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Caught exception outside of main thread! Proceeding to ignore it to avoid server crash:\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
					logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000E498 File Offset: 0x0000C698
		private void ConnectionAccepted(Socket socket)
		{
			IPEndPoint sender = socket.RemoteEndPoint as IPEndPoint;
			Logger logger = this.logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(25, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Accepted connection from ");
			defaultInterpolatedStringHandler.AppendFormatted<IPEndPoint>(sender);
			logger.VerboseDebug(defaultInterpolatedStringHandler.ToStringAndClear());
			TCPConnection connection = new TCPConnection(this.logger, socket);
			connection.OnMessageReceived += new Action<byte[], TCPConnection>(this.MessageReceived);
			connection.OnDisconnected += new Action<bool, bool, TCPConnection>(this.ConnectionClosed);
			connection.StartListening();
			this.connectionsByAddress[sender.ToString()] = connection;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000E52C File Offset: 0x0000C72C
		private void ConnectionClosed(bool isGraceful, bool isHalfClosed, TCPConnection connection)
		{
			string address = connection.remoteEndpoint.ToString();
			if (!this.connectionsByAddress.ContainsKey(address))
			{
				return;
			}
			string player = this.ResolvePlayer(connection);
			this.connectionsByAddress.Remove(address);
			connection.Dispose();
			string closeType = isGraceful ? "gracefully" : "unexpectedly";
			closeType = (isHalfClosed ? "by client's request" : closeType);
			Logger logger = this.logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(29, 3);
			defaultInterpolatedStringHandler.AppendFormatted("CustomTCP");
			defaultInterpolatedStringHandler.AppendLiteral(" connection with ");
			defaultInterpolatedStringHandler.AppendFormatted(address);
			defaultInterpolatedStringHandler.AppendLiteral(" was closed ");
			defaultInterpolatedStringHandler.AppendFormatted(closeType);
			logger.VerboseDebug(defaultInterpolatedStringHandler.ToStringAndClear());
			if (player != null)
			{
				this.PlayerDisconnected(player);
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000E5E8 File Offset: 0x0000C7E8
		private void MessageReceived(byte[] msg, TCPConnection channel)
		{
			PacketType code = (PacketType)BitConverter.ToInt32(msg, 0);
			if (code != PacketType.SelfPing)
			{
				if (code == PacketType.Ping)
				{
					this.SendEchoPacket(channel);
					return;
				}
				if (code != PacketType.Audio)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(25, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported packet type: ");
					defaultInterpolatedStringHandler.AppendFormatted<PacketType>(code);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				AudioPacket packet = NetworkPacket.FromBytes<AudioPacket>(msg);
				string senderPlayerId = this.ResolvePlayer(channel);
				if (senderPlayerId != null)
				{
					packet.PlayerId = senderPlayerId;
					Task.Run(delegate()
					{
						Action<AudioPacket> audioPacketReceived = this.AudioPacketReceived;
						if (audioPacketReceived == null)
						{
							return;
						}
						audioPacketReceived.Invoke(packet);
					});
					return;
				}
				return;
			}
			else
			{
				if (this.isReady)
				{
					return;
				}
				this.isReady = true;
				this._readinessProbeCTS.Cancel();
				return;
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000E6A0 File Offset: 0x0000C8A0
		private void SendEchoPacket(TCPConnection connection)
		{
			byte[] echoPacket = BitConverter.GetBytes(2);
			try
			{
				connection.SendAsync(echoPacket, default(CancellationToken));
			}
			catch (Exception e)
			{
				Logger logger = this.logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to send echo request to ");
				defaultInterpolatedStringHandler.AppendFormatted<IPEndPoint>(connection.remoteEndpoint);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(e.Message);
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
				connection.Disconnect(true, false);
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000E730 File Offset: 0x0000C930
		private void SetupUpnp(int port)
		{
			if (!this.upnpEnabled)
			{
				return;
			}
			try
			{
				this.logger.VerboseDebug("Attempting to portforward with UPnP");
				NatDiscoverer discoverer = new NatDiscoverer();
				CancellationTokenSource cts = new CancellationTokenSource(5000);
				NatDevice result = Task.Run<NatDevice>(() => discoverer.DiscoverDeviceAsync(2, cts)).GetAwaiter().GetResult();
				if (result == null)
				{
					throw new NatDeviceNotFoundException("NatDiscoverer have not returned the NatDevice");
				}
				this.logger.VerboseDebug("Found a UPnP device, creating port map");
				result.CreatePortMapAsync(new Mapping(0, port, port, "Vintage Story Voice Chat"));
			}
			catch (TaskCanceledException)
			{
				this.logger.Warning("Device discovery got aborted, assuming public IP");
			}
			catch (NatDeviceNotFoundException)
			{
				this.logger.Warning("Failed to port forward with UPnP. CustomTCP server may not be available if your IP is private or you are behind NAT");
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000E80C File Offset: 0x0000CA0C
		private void VerifyServerReadiness()
		{
			byte[] selfPingPacket = BitConverter.GetBytes(0);
			this._readinessProbeCTS = new CancellationTokenSource();
			try
			{
				using (TCPConnection connection = new TCPConnection(this.logger, null))
				{
					connection.ConnectAsync(this.ownEndPoint).ContinueWith<ValueTask<int>>(([Nullable(1)] Task _) => connection.SendAsync(selfPingPacket, this._readinessProbeCTS.Token), 393216);
					Task.Delay(5000, this._readinessProbeCTS.Token).GetAwaiter().GetResult();
				}
			}
			catch (TaskCanceledException)
			{
			}
			if (this.isReady)
			{
				return;
			}
			throw new HealthCheckException(NetworkSide.Server);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000E8E0 File Offset: 0x0000CAE0
		private TCPConnection ResolveConnection(string playerId)
		{
			string playerAddress;
			if (!this.playerAddresses.TryGetValue(playerId, ref playerAddress))
			{
				return null;
			}
			TCPConnection connection;
			if (!this.connectionsByAddress.TryGetValue(playerAddress, ref connection))
			{
				return null;
			}
			return connection;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000E914 File Offset: 0x0000CB14
		private string ResolvePlayer(TCPConnection connection)
		{
			string playerAddress = Enumerable.FirstOrDefault<KeyValuePair<string, TCPConnection>>(this.connectionsByAddress, (KeyValuePair<string, TCPConnection> e) => e.Value == connection).Key;
			if (playerAddress == null)
			{
				return null;
			}
			return Enumerable.FirstOrDefault<KeyValuePair<string, string>>(this.playerAddresses, (KeyValuePair<string, string> e) => e.Value == playerAddress).Key;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000E97C File Offset: 0x0000CB7C
		public override void Dispose()
		{
			base.Dispose();
			CancellationTokenSource listeningCTS = this._listeningCTS;
			if (listeningCTS != null)
			{
				listeningCTS.Cancel();
			}
			CancellationTokenSource listeningCTS2 = this._listeningCTS;
			if (listeningCTS2 != null)
			{
				listeningCTS2.Dispose();
			}
			Socket socket = this.socket;
			if (socket != null)
			{
				socket.Close();
			}
			foreach (TCPConnection tcpconnection in this.connectionsByAddress.Values)
			{
				if (tcpconnection != null)
				{
					tcpconnection.Dispose();
				}
			}
		}

		// Token: 0x04000113 RID: 275
		private Dictionary<string, TCPConnection> connectionsByAddress = new Dictionary<string, TCPConnection>();

		// Token: 0x04000114 RID: 276
		private Dictionary<string, string> playerAddresses = new Dictionary<string, string>();

		// Token: 0x04000115 RID: 277
		private IPAddress ip;

		// Token: 0x04000116 RID: 278
		protected bool upnpEnabled;

		// Token: 0x04000117 RID: 279
		private IPEndPoint ownEndPoint;

		// Token: 0x04000118 RID: 280
		private ConnectionInfo connectionInfo;

		// Token: 0x04000119 RID: 281
		private Socket socket;

		// Token: 0x0400011A RID: 282
		private Thread _listeningThread;

		// Token: 0x0400011B RID: 283
		private CancellationTokenSource _listeningCTS;
	}
}
