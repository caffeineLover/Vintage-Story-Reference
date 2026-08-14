using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using RPVoiceChat.Config;
using RPVoiceChat.Util;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000059 RID: 89
	public class TCPNetworkClient : TCPNetworkBase, IExtendedNetworkClient, INetworkClient, IDisposable
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000262 RID: 610 RVA: 0x0000DD30 File Offset: 0x0000BF30
		// (remove) Token: 0x06000263 RID: 611 RVA: 0x0000DD68 File Offset: 0x0000BF68
		public event Action<AudioPacket> OnAudioReceived;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000264 RID: 612 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		// (remove) Token: 0x06000265 RID: 613 RVA: 0x0000DDD8 File Offset: 0x0000BFD8
		public event Action<bool, IExtendedNetworkClient> OnConnectionLost;

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000DE0D File Offset: 0x0000C00D
		private int maxConnectionAttempts
		{
			get
			{
				return ServerConfigManager.MaxConnectionAttempts;
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000DE14 File Offset: 0x0000C014
		public TCPNetworkClient() : base(Logger.client)
		{
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000DE21 File Offset: 0x0000C021
		public ConnectionInfo Connect(ConnectionInfo serverConnection)
		{
			this.serverEndpoint = NetworkUtils.GetEndPoint(serverConnection);
			this.Connect();
			return new ConnectionInfo(this.port, null);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000DE44 File Offset: 0x0000C044
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

		// Token: 0x0600026A RID: 618 RVA: 0x0000DE88 File Offset: 0x0000C088
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

		// Token: 0x0600026B RID: 619 RVA: 0x0000DF74 File Offset: 0x0000C174
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

		// Token: 0x0600026C RID: 620 RVA: 0x0000DFCC File Offset: 0x0000C1CC
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

		// Token: 0x0600026D RID: 621 RVA: 0x0000E034 File Offset: 0x0000C234
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

		// Token: 0x0600026E RID: 622 RVA: 0x0000E0BC File Offset: 0x0000C2BC
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

		// Token: 0x0600026F RID: 623 RVA: 0x0000E13C File Offset: 0x0000C33C
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

		// Token: 0x0400010F RID: 271
		private int connectionAttempt;

		// Token: 0x04000110 RID: 272
		private IPEndPoint serverEndpoint;

		// Token: 0x04000111 RID: 273
		private TCPConnection connection;
	}
}
