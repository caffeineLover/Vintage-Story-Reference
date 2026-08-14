using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RPVoiceChat.Util;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200005D RID: 93
	public class UDPNetworkServer : UDPNetworkBase, IExtendedNetworkServer, INetworkServer, IDisposable
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000296 RID: 662 RVA: 0x0000F0D4 File Offset: 0x0000D2D4
		// (remove) Token: 0x06000297 RID: 663 RVA: 0x0000F10C File Offset: 0x0000D30C
		public event Action<AudioPacket> AudioPacketReceived;

		// Token: 0x06000298 RID: 664 RVA: 0x0000F144 File Offset: 0x0000D344
		public UDPNetworkServer(int port, string ip, bool forwardPorts) : base(Logger.server, forwardPorts)
		{
			this.port = port;
			this.ip = IPAddress.Parse(ip ?? NetworkUtils.GetPublicIP());
			this.ownEndPoint = NetworkUtils.GetEndPoint(this.GetConnectionInfo());
			base.OnMessageReceived += new Action<byte[], IPEndPoint>(this.MessageReceived);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000F1B2 File Offset: 0x0000D3B2
		public void Launch()
		{
			if (!NetworkUtils.IsInternalNetwork(this.ip))
			{
				base.SetupUpnp(this.port);
			}
			base.OpenUDPClient(this.port);
			base.StartListening();
			this.VerifyServerReadiness();
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000F1E8 File Offset: 0x0000D3E8
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

		// Token: 0x0600029B RID: 667 RVA: 0x0000F238 File Offset: 0x0000D438
		public bool SendPacket(NetworkPacket packet, string playerId)
		{
			ConnectionInfo connectionInfo;
			if (!this.connectionsByPlayer.TryGetValue(playerId, ref connectionInfo))
			{
				return false;
			}
			byte[] data = packet.ToBytes();
			IPEndPoint destination = NetworkUtils.GetEndPoint(connectionInfo);
			this.UdpClient.Send(data, data.Length, destination);
			return true;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000F278 File Offset: 0x0000D478
		public void PlayerConnected(string playerId, ConnectionInfo connectionInfo)
		{
			ConnectionInfo oldConn;
			if (this.connectionsByPlayer.TryGetValue(playerId, ref oldConn))
			{
				this.playerByAddress.Remove(NetworkUtils.GetEndPoint(oldConn).ToString());
			}
			string addressKey = NetworkUtils.GetEndPoint(connectionInfo).ToString();
			if (this.playerByAddress.ContainsKey(addressKey))
			{
				this.playerByAddress.Remove(addressKey);
			}
			this.connectionsByPlayer[playerId] = connectionInfo;
			this.playerByAddress[addressKey] = playerId;
			this.logger.VerboseDebug(playerId + " connected over UDP");
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000F304 File Offset: 0x0000D504
		public void PlayerDisconnected(string playerId)
		{
			ConnectionInfo conn;
			if (!this.connectionsByPlayer.TryGetValue(playerId, ref conn))
			{
				return;
			}
			string addressKey = NetworkUtils.GetEndPoint(conn).ToString();
			this.connectionsByPlayer.Remove(playerId);
			this.playerByAddress.Remove(addressKey);
			this.logger.VerboseDebug(playerId + " disconnected from UDP server");
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000F360 File Offset: 0x0000D560
		private void MessageReceived(byte[] msg, IPEndPoint sender)
		{
			PacketType code = (PacketType)BitConverter.ToInt32(msg, 0);
			if (code != PacketType.SelfPing)
			{
				if (code == PacketType.Ping)
				{
					this.SendEchoPacket(sender);
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
				string senderPlayerId;
				if (!this.playerByAddress.TryGetValue(sender.ToString(), ref senderPlayerId))
				{
					return;
				}
				packet.PlayerId = senderPlayerId;
				Action<AudioPacket> audioPacketReceived = this.AudioPacketReceived;
				if (audioPacketReceived == null)
				{
					return;
				}
				audioPacketReceived.Invoke(packet);
				return;
			}
			else
			{
				if (!this.IsValidReadinessSelfPing(msg))
				{
					return;
				}
				this.isReady = true;
				this._readinessProbeCTS.Cancel();
				return;
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000F408 File Offset: 0x0000D608
		private void SendEchoPacket(IPEndPoint endPoint)
		{
			byte[] echoPacket = BitConverter.GetBytes(2);
			this.UdpClient.Send(echoPacket, echoPacket.Length, endPoint);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000F430 File Offset: 0x0000D630
		private void VerifyServerReadiness()
		{
			this.readinessProbeToken = Random.Shared.NextInt64();
			byte[] selfPingPacket = UDPNetworkServer.CreateSelfPingPacket(this.readinessProbeToken);
			try
			{
				this.UdpClient.Send(selfPingPacket, selfPingPacket.Length, new IPEndPoint(IPAddress.Loopback, this.port));
				if (!IPAddress.IsLoopback(this.ownEndPoint.Address))
				{
					this.UdpClient.Send(selfPingPacket, selfPingPacket.Length, this.ownEndPoint);
				}
				Task.Delay(5000, this._readinessProbeCTS.Token).GetAwaiter().GetResult();
			}
			catch (TaskCanceledException)
			{
			}
			this.readinessProbeToken = 0L;
			if (this.isReady)
			{
				return;
			}
			throw new HealthCheckException(NetworkSide.Server);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000F4F0 File Offset: 0x0000D6F0
		private static byte[] CreateSelfPingPacket(long token)
		{
			byte[] packet = new byte[12];
			BitConverter.GetBytes(0).CopyTo(packet, 0);
			BitConverter.GetBytes(token).CopyTo(packet, 4);
			return packet;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000F520 File Offset: 0x0000D720
		private bool IsValidReadinessSelfPing(byte[] msg)
		{
			return this.readinessProbeToken != 0L && msg.Length >= 12 && BitConverter.ToInt32(msg, 0) == 0 && BitConverter.ToInt64(msg, 4) == this.readinessProbeToken;
		}

		// Token: 0x0400012A RID: 298
		private Dictionary<string, ConnectionInfo> connectionsByPlayer = new Dictionary<string, ConnectionInfo>();

		// Token: 0x0400012B RID: 299
		private Dictionary<string, string> playerByAddress = new Dictionary<string, string>();

		// Token: 0x0400012C RID: 300
		private IPAddress ip;

		// Token: 0x0400012D RID: 301
		private IPEndPoint ownEndPoint;

		// Token: 0x0400012E RID: 302
		private ConnectionInfo connectionInfo;

		// Token: 0x0400012F RID: 303
		private long readinessProbeToken;
	}
}
