using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RPVoiceChat.Util;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000079 RID: 121
	public class UDPNetworkClient : UDPNetworkBase, IExtendedNetworkClient, INetworkClient, IDisposable
	{
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000328 RID: 808 RVA: 0x000128D0 File Offset: 0x00010AD0
		// (remove) Token: 0x06000329 RID: 809 RVA: 0x00012908 File Offset: 0x00010B08
		public event Action<AudioPacket> OnAudioReceived;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600032A RID: 810 RVA: 0x00012940 File Offset: 0x00010B40
		// (remove) Token: 0x0600032B RID: 811 RVA: 0x00012978 File Offset: 0x00010B78
		public event Action<bool, IExtendedNetworkClient> OnConnectionLost = delegate(bool <p0>, IExtendedNetworkClient <p1>)
		{
		};

		// Token: 0x0600032C RID: 812 RVA: 0x000129B0 File Offset: 0x00010BB0
		public UDPNetworkClient(bool forwardPorts) : base(Logger.client, forwardPorts)
		{
			base.OnMessageReceived += new Action<byte[], IPEndPoint>(this.MessageReceived);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00012A00 File Offset: 0x00010C00
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

		// Token: 0x0600032E RID: 814 RVA: 0x00012A58 File Offset: 0x00010C58
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

		// Token: 0x0600032F RID: 815 RVA: 0x00012AA8 File Offset: 0x00010CA8
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

		// Token: 0x06000330 RID: 816 RVA: 0x00012B30 File Offset: 0x00010D30
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

		// Token: 0x0400017A RID: 378
		private IPEndPoint serverEndpoint;
	}
}
