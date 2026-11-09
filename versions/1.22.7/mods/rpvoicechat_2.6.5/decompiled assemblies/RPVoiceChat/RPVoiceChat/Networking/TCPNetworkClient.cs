using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using RPVoiceChat.Config;
using RPVoiceChat.Util;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000076 RID: 118
	public class TCPNetworkClient : TCPNetworkBase, IExtendedNetworkClient, INetworkClient, IDisposable
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060002FD RID: 765 RVA: 0x000117FC File Offset: 0x0000F9FC
		// (remove) Token: 0x060002FE RID: 766 RVA: 0x00011834 File Offset: 0x0000FA34
		public event Action<AudioPacket> OnAudioReceived;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060002FF RID: 767 RVA: 0x0001186C File Offset: 0x0000FA6C
		// (remove) Token: 0x06000300 RID: 768 RVA: 0x000118A4 File Offset: 0x0000FAA4
		public event Action<bool, IExtendedNetworkClient> OnConnectionLost;

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000301 RID: 769 RVA: 0x000118D9 File Offset: 0x0000FAD9
		private int maxConnectionAttempts
		{
			get
			{
				return ServerConfigManager.MaxConnectionAttempts;
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000118E0 File Offset: 0x0000FAE0
		public TCPNetworkClient() : base(Logger.client)
		{
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000118ED File Offset: 0x0000FAED
		public ConnectionInfo Connect(ConnectionInfo serverConnection)
		{
			this.serverEndpoint = NetworkUtils.GetEndPoint(serverConnection);
			this.Connect();
			return new ConnectionInfo(this.port, null);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00011910 File Offset: 0x0000FB10
		public bool SendAudioToServer(AudioPacket packet)
		{
			if (!this.isReady || this.connection == null)
			{
				this.logger.Warning("Attempting to send audio over CustomTCP while client isn't ready. Skipping sending.");
				return false;
			}
			byte[] data = packet.ToBytes();
			this.connection.Send(data);
			return true;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00011954 File Offset: 0x0000FB54
		private ConnectionInfo Connect()
		{
			this.connectionAttempt++;
			ConnectionInfo result;
			try
			{
				this.connection = this.OpenConnection(this.serverEndpoint);
				this.VerifyClientReadiness();
				this.connectionAttempt = 0;
				result = new ConnectionInfo(this.port, null);
			}
			catch
			{
				if (this.connectionAttempt > this.maxConnectionAttempts)
				{
					throw;
				}
				Logger logger = this.logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(56, 3);
				defaultInterpolatedStringHandler.AppendLiteral("[Internal] Connection attempt over ");
				defaultInterpolatedStringHandler.AppendFormatted("CustomTCP");
				defaultInterpolatedStringHandler.AppendLiteral(" failed, retrying (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.connectionAttempt);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.maxConnectionAttempts);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				logger.VerboseDebug(defaultInterpolatedStringHandler.ToStringAndClear());
				result = this.Connect();
			}
			return result;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00011A40 File Offset: 0x0000FC40
		private TCPConnection OpenConnection(IPEndPoint endPoint)
		{
			TCPConnection connection = new TCPConnection(this.logger, null);
			connection.OnMessageReceived += new Action<byte[], TCPConnection>(this.MessageReceived);
			connection.OnDisconnected += new Action<bool, bool, TCPConnection>(this.ConnectionClosed);
			connection.Connect(endPoint);
			connection.StartListening();
			this.port = connection.port;
			return connection;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00011A98 File Offset: 0x0000FC98
		private void ConnectionClosed(bool isGraceful, bool isHalfClosed, TCPConnection closedConnection)
		{
			this.isReady = false;
			string closeType = isGraceful ? "gracefully" : "unexpectedly";
			closeType = (isHalfClosed ? "by server's request" : closeType);
			this.logger.VerboseDebug("Connection with CustomTCP server was closed " + closeType);
			closedConnection.Dispose();
			bool canReconnect = !isGraceful || isHalfClosed;
			Action<bool, IExtendedNetworkClient> onConnectionLost = this.OnConnectionLost;
			if (onConnectionLost == null)
			{
				return;
			}
			onConnectionLost.Invoke(canReconnect, this);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00011B00 File Offset: 0x0000FD00
		private void MessageReceived(byte[] msg, TCPConnection _)
		{
			PacketType code = (PacketType)BitConverter.ToInt32(msg, 0);
			if (code == PacketType.Pong)
			{
				this.isReady = true;
				this._readinessProbeCTS.Cancel();
				return;
			}
			if (code != PacketType.Audio)
			{
				Logger logger = this.logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(59, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Received unsupported packet type: ");
				defaultInterpolatedStringHandler.AppendFormatted<PacketType>(code);
				defaultInterpolatedStringHandler.AppendLiteral(", proceeding to ignore it");
				logger.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			AudioPacket packet = NetworkPacket.FromBytes<AudioPacket>(msg);
			Action<AudioPacket> onAudioReceived = this.OnAudioReceived;
			if (onAudioReceived == null)
			{
				return;
			}
			onAudioReceived.Invoke(packet);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00011B88 File Offset: 0x0000FD88
		private void VerifyClientReadiness()
		{
			byte[] pingPacket = BitConverter.GetBytes(1);
			this._readinessProbeCTS = new CancellationTokenSource();
			try
			{
				this.connection.SendAsync(pingPacket, this._readinessProbeCTS.Token);
				Task.Delay(3000, this._readinessProbeCTS.Token).GetAwaiter().GetResult();
			}
			catch (TaskCanceledException)
			{
			}
			if (this.isReady)
			{
				return;
			}
			throw new HealthCheckException(NetworkSide.Client);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00011C08 File Offset: 0x0000FE08
		public override void Dispose()
		{
			base.Dispose();
			TCPConnection tcpconnection = this.connection;
			if (tcpconnection == null)
			{
				return;
			}
			tcpconnection.Dispose();
		}

		// Token: 0x04000161 RID: 353
		private int connectionAttempt;

		// Token: 0x04000162 RID: 354
		private IPEndPoint serverEndpoint;

		// Token: 0x04000163 RID: 355
		private TCPConnection connection;
	}
}
