using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RPVoiceChat.Util;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200007A RID: 122
	public class UDPNetworkServer : UDPNetworkBase, IExtendedNetworkServer, INetworkServer, IDisposable
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000331 RID: 817 RVA: 0x00012BA0 File Offset: 0x00010DA0
		// (remove) Token: 0x06000332 RID: 818 RVA: 0x00012BD8 File Offset: 0x00010DD8
		public event Action<AudioPacket> AudioPacketReceived;

		// Token: 0x06000333 RID: 819 RVA: 0x00012C10 File Offset: 0x00010E10
		public UDPNetworkServer(int port, string ip, bool forwardPorts) : base(Logger.server, forwardPorts)
		{
			this.port = port;
			this.ip = IPAddress.Parse(ip ?? NetworkUtils.GetPublicIP());
			this.ownEndPoint = NetworkUtils.GetEndPoint(this.GetConnectionInfo());
			base.OnMessageReceived += new Action<byte[], IPEndPoint>(this.MessageReceived);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00012C7E File Offset: 0x00010E7E
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

		// Token: 0x06000335 RID: 821 RVA: 0x00012CB4 File Offset: 0x00010EB4
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

		// Token: 0x06000336 RID: 822 RVA: 0x00012D04 File Offset: 0x00010F04
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

		// Token: 0x06000337 RID: 823 RVA: 0x00012D44 File Offset: 0x00010F44
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

		// Token: 0x06000338 RID: 824 RVA: 0x00012DD0 File Offset: 0x00010FD0
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

		// Token: 0x06000339 RID: 825 RVA: 0x00012E2C File Offset: 0x0001102C
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

		// Token: 0x0600033A RID: 826 RVA: 0x00012ED4 File Offset: 0x000110D4
		private void SendEchoPacket(IPEndPoint endPoint)
		{
			byte[] echoPacket = BitConverter.GetBytes(2);
			this.UdpClient.Send(echoPacket, echoPacket.Length, endPoint);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00012EFC File Offset: 0x000110FC
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

		// Token: 0x0600033C RID: 828 RVA: 0x00012FBC File Offset: 0x000111BC
		private static byte[] CreateSelfPingPacket(long token)
		{
			byte[] packet = new byte[12];
			BitConverter.GetBytes(0).CopyTo(packet, 0);
			BitConverter.GetBytes(token).CopyTo(packet, 4);
			return packet;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00012FEC File Offset: 0x000111EC
		private bool IsValidReadinessSelfPing(byte[] msg)
		{
			return this.readinessProbeToken != 0L && msg.Length >= 12 && BitConverter.ToInt32(msg, 0) == 0 && BitConverter.ToInt64(msg, 4) == this.readinessProbeToken;
		}

		// Token: 0x0400017C RID: 380
		private Dictionary<string, ConnectionInfo> connectionsByPlayer = new Dictionary<string, ConnectionInfo>();

		// Token: 0x0400017D RID: 381
		private Dictionary<string, string> playerByAddress = new Dictionary<string, string>();

		// Token: 0x0400017E RID: 382
		private IPAddress ip;

		// Token: 0x0400017F RID: 383
		private IPEndPoint ownEndPoint;

		// Token: 0x04000180 RID: 384
		private ConnectionInfo connectionInfo;

		// Token: 0x04000181 RID: 385
		private long readinessProbeToken;
	}
}
