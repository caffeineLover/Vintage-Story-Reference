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
	// Token: 0x02000077 RID: 119
	public class TCPNetworkServer : TCPNetworkBase, IExtendedNetworkServer, INetworkServer, IDisposable
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600030B RID: 779 RVA: 0x00011C20 File Offset: 0x0000FE20
		// (remove) Token: 0x0600030C RID: 780 RVA: 0x00011C58 File Offset: 0x0000FE58
		public event Action<AudioPacket> AudioPacketReceived;

		// Token: 0x0600030D RID: 781 RVA: 0x00011C90 File Offset: 0x0000FE90
		public TCPNetworkServer(int port, string ip, bool forwardPorts) : base(Logger.server)
		{
			this.port = port;
			this.ip = IPAddress.Parse(ip ?? NetworkUtils.GetPublicIP());
			this.upnpEnabled = forwardPorts;
			this.ownEndPoint = NetworkUtils.GetEndPoint(this.GetConnectionInfo());
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00011CF2 File Offset: 0x0000FEF2
		public void Launch()
		{
			if (!NetworkUtils.IsInternalNetwork(this.ip))
			{
				this.SetupUpnp(this.port);
			}
			this.StartServer();
			this.VerifyServerReadiness();
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00011D1C File Offset: 0x0000FF1C
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

		// Token: 0x06000310 RID: 784 RVA: 0x00011D6C File Offset: 0x0000FF6C
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

		// Token: 0x06000311 RID: 785 RVA: 0x00011D98 File Offset: 0x0000FF98
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

		// Token: 0x06000312 RID: 786 RVA: 0x00011DE9 File Offset: 0x0000FFE9
		public void PlayerDisconnected(string playerId)
		{
			if (!this.playerAddresses.ContainsKey(playerId))
			{
				return;
			}
			this.playerAddresses.Remove(playerId);
			this.logger.VerboseDebug(playerId + " disconnected from CustomTCP server");
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00011E20 File Offset: 0x00010020
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

		// Token: 0x06000314 RID: 788 RVA: 0x00011EAC File Offset: 0x000100AC
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

		// Token: 0x06000315 RID: 789 RVA: 0x00011F64 File Offset: 0x00010164
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

		// Token: 0x06000316 RID: 790 RVA: 0x00011FF8 File Offset: 0x000101F8
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

		// Token: 0x06000317 RID: 791 RVA: 0x000120B4 File Offset: 0x000102B4
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

		// Token: 0x06000318 RID: 792 RVA: 0x0001216C File Offset: 0x0001036C
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

		// Token: 0x06000319 RID: 793 RVA: 0x000121FC File Offset: 0x000103FC
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

		// Token: 0x0600031A RID: 794 RVA: 0x000122D8 File Offset: 0x000104D8
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

		// Token: 0x0600031B RID: 795 RVA: 0x000123AC File Offset: 0x000105AC
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

		// Token: 0x0600031C RID: 796 RVA: 0x000123E0 File Offset: 0x000105E0
		private string ResolvePlayer(TCPConnection connection)
		{
			string playerAddress = Enumerable.FirstOrDefault<KeyValuePair<string, TCPConnection>>(this.connectionsByAddress, (KeyValuePair<string, TCPConnection> e) => e.Value == connection).Key;
			if (playerAddress == null)
			{
				return null;
			}
			return Enumerable.FirstOrDefault<KeyValuePair<string, string>>(this.playerAddresses, (KeyValuePair<string, string> e) => e.Value == playerAddress).Key;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00012448 File Offset: 0x00010648
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

		// Token: 0x04000165 RID: 357
		private Dictionary<string, TCPConnection> connectionsByAddress = new Dictionary<string, TCPConnection>();

		// Token: 0x04000166 RID: 358
		private Dictionary<string, string> playerAddresses = new Dictionary<string, string>();

		// Token: 0x04000167 RID: 359
		private IPAddress ip;

		// Token: 0x04000168 RID: 360
		protected bool upnpEnabled;

		// Token: 0x04000169 RID: 361
		private IPEndPoint ownEndPoint;

		// Token: 0x0400016A RID: 362
		private ConnectionInfo connectionInfo;

		// Token: 0x0400016B RID: 363
		private Socket socket;

		// Token: 0x0400016C RID: 364
		private Thread _listeningThread;

		// Token: 0x0400016D RID: 365
		private CancellationTokenSource _listeningCTS;
	}
}
