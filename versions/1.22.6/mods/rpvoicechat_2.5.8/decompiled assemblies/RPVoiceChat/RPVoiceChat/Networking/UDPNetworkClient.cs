using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RPVoiceChat.Util;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200005C RID: 92
	public class UDPNetworkClient : UDPNetworkBase, IExtendedNetworkClient, INetworkClient, IDisposable
	{
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600028D RID: 653 RVA: 0x0000EE04 File Offset: 0x0000D004
		// (remove) Token: 0x0600028E RID: 654 RVA: 0x0000EE3C File Offset: 0x0000D03C
		public event Action<AudioPacket> OnAudioReceived;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600028F RID: 655 RVA: 0x0000EE74 File Offset: 0x0000D074
		// (remove) Token: 0x06000290 RID: 656 RVA: 0x0000EEAC File Offset: 0x0000D0AC
		public event Action<bool, IExtendedNetworkClient> OnConnectionLost = delegate(bool <p0>, IExtendedNetworkClient <p1>)
		{
		};

		// Token: 0x06000291 RID: 657 RVA: 0x0000EEE4 File Offset: 0x0000D0E4
		public UDPNetworkClient(bool forwardPorts) : base(Logger.client, forwardPorts)
		{
			base.OnMessageReceived += new Action<byte[], IPEndPoint>(this.MessageReceived);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000EF34 File Offset: 0x0000D134
		public ConnectionInfo Connect(ConnectionInfo serverConnection)
		{
			this.serverEndpoint = NetworkUtils.GetEndPoint(serverConnection);
			this.port = base.OpenUDPClient();
			if (!NetworkUtils.IsInternalNetwork(serverConnection.Address))
			{
				base.SetupUpnp(this.port);
			}
			base.StartListening();
			this.VerifyClientReadiness();
			return new ConnectionInfo(this.port, null);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000EF8C File Offset: 0x0000D18C
		public bool SendAudioToServer(AudioPacket packet)
		{
			if (this.UdpClient == null || this.serverEndpoint == null)
			{
				this.logger.Warning("UDP client or server endpoint have not been initialized.");
				return false;
			}
			byte[] data = packet.ToBytes();
			this.UdpClient.Send(data, data.Length, this.serverEndpoint);
			return true;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000EFDC File Offset: 0x0000D1DC
		private void MessageReceived(byte[] msg, IPEndPoint sender)
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

		// Token: 0x06000295 RID: 661 RVA: 0x0000F064 File Offset: 0x0000D264
		private void VerifyClientReadiness()
		{
			byte[] pingPacket = BitConverter.GetBytes(1);
			try
			{
				this.UdpClient.Send(pingPacket, pingPacket.Length, this.serverEndpoint);
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

		// Token: 0x04000128 RID: 296
		private IPEndPoint serverEndpoint;
	}
}
