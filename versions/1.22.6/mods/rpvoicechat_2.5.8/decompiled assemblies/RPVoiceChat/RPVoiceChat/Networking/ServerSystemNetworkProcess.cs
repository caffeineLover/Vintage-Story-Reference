using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using HarmonyLib;
using RPVoiceChat.Util;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Common;
using Vintagestory.Server;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200004F RID: 79
	public class ServerSystemNetworkProcess : IDisposable
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000205 RID: 517 RVA: 0x0000CDA4 File Offset: 0x0000AFA4
		// (remove) Token: 0x06000206 RID: 518 RVA: 0x0000CDDC File Offset: 0x0000AFDC
		public event Func<int, Packet_CustomPacket, IServerPlayer, bool> OnProcessInBackground;

		// Token: 0x06000207 RID: 519 RVA: 0x0000CE11 File Offset: 0x0000B011
		public ServerSystemNetworkProcess(ICoreServerAPI sapi)
		{
			this.api = sapi;
			this.packetProcessingThread = new Thread(new ParameterizedThreadStart(this.NetworkProcess));
			this._packetProcessingCTS = new CancellationTokenSource();
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000CE42 File Offset: 0x0000B042
		public void Launch()
		{
			this.packetProcessingThread.Start(this._packetProcessingCTS.Token);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000CE60 File Offset: 0x0000B060
		private void NetworkProcess(object cancellationToken)
		{
			if (ServerMain.FrameProfiler == null)
			{
				ServerMain.FrameProfiler = new FrameProfilerUtil("[RPVC - Network Process]");
			}
			CancellationToken ct = (CancellationToken)cancellationToken;
			while (this.packetProcessingThread.IsAlive && !ct.IsCancellationRequested)
			{
				try
				{
					NetIncomingMessage msg;
					while ((msg = TcpNetServerPatch.ReadMessage()) != null)
					{
						NetConnection connection = (NetConnection)ServerSystemNetworkProcess.senderConnectionField.GetValue(msg);
						IServerPlayer player = this.ResolveServerPlayer(connection);
						if (player != null)
						{
							byte[] data = (byte[])ServerSystemNetworkProcess.messageField.GetValue(msg);
							int length = (int)ServerSystemNetworkProcess.messageLengthField.GetValue(msg);
							this.TryReadPacket(data, length, player);
						}
					}
					Thread.Sleep(1);
				}
				catch (Exception e)
				{
					Logger server = Logger.server;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(88, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Caught exception outside of main thread! Proceeding to ignore it to avoid server crash:\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
					server.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000CF4C File Offset: 0x0000B14C
		private void TryReadPacket(byte[] data, int dataLength, IServerPlayer sender)
		{
			Packet_Client packet = new Packet_Client();
			Packet_ClientSerializer.DeserializeBuffer(data, dataLength, packet);
			this.ProcessInBackground(packet, sender);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000CF74 File Offset: 0x0000B174
		private bool ProcessInBackground(Packet_Client packet, IServerPlayer sender)
		{
			if ((int)ServerSystemNetworkProcess.packetIdField.GetValue(packet) != 23)
			{
				return false;
			}
			Packet_CustomPacket customPacket = (Packet_CustomPacket)ServerSystemNetworkProcess.customPacketField.GetValue(packet);
			int channelId = (int)ServerSystemNetworkProcess.channelIdField.GetValue(customPacket);
			Func<int, Packet_CustomPacket, IServerPlayer, bool> onProcessInBackground = this.OnProcessInBackground;
			return onProcessInBackground != null && onProcessInBackground.Invoke(channelId, customPacket, sender);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000CFD0 File Offset: 0x0000B1D0
		private IServerPlayer ResolveServerPlayer(NetConnection connection)
		{
			try
			{
				foreach (ConnectedClient connectedClient in Enumerable.ToList<ConnectedClient>(((ServerMain)this.api.World).Clients.Values))
				{
					NetConnection serverConnection = (NetConnection)ServerSystemNetworkProcess.SocketField.GetValue(connectedClient);
					if (!connectedClient.IsSinglePlayerClient && serverConnection.EqualsConnection(connection))
					{
						return (IServerPlayer)ServerSystemNetworkProcess.PlayerField.GetValue(connectedClient);
					}
				}
			}
			catch (Exception e)
			{
				if (e is InvalidOperationException)
				{
					return this.ResolveServerPlayer(connection);
				}
				Logger server = Logger.server;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(76, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to resolve IServerPlayer from NetConnection, packet will be dropped:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
				server.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return null;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000D0C4 File Offset: 0x0000B2C4
		public void Dispose()
		{
			CancellationTokenSource packetProcessingCTS = this._packetProcessingCTS;
			if (packetProcessingCTS != null)
			{
				packetProcessingCTS.Cancel();
			}
			CancellationTokenSource packetProcessingCTS2 = this._packetProcessingCTS;
			if (packetProcessingCTS2 != null)
			{
				packetProcessingCTS2.Dispose();
			}
			this._packetProcessingCTS = null;
			this.OnProcessInBackground = null;
		}

		// Token: 0x040000CC RID: 204
		private const int _customPacketId = 23;

		// Token: 0x040000CD RID: 205
		private ICoreServerAPI api;

		// Token: 0x040000CE RID: 206
		private Thread packetProcessingThread;

		// Token: 0x040000CF RID: 207
		private CancellationTokenSource _packetProcessingCTS;

		// Token: 0x040000D0 RID: 208
		private static FieldInfo senderConnectionField = AccessTools.Field(typeof(NetIncomingMessage), "SenderConnection");

		// Token: 0x040000D1 RID: 209
		private static FieldInfo messageField = AccessTools.Field(typeof(NetIncomingMessage), "message");

		// Token: 0x040000D2 RID: 210
		private static FieldInfo messageLengthField = AccessTools.Field(typeof(NetIncomingMessage), "messageLength");

		// Token: 0x040000D3 RID: 211
		private static FieldInfo packetIdField = AccessTools.Field(typeof(Packet_Client), "Id");

		// Token: 0x040000D4 RID: 212
		private static FieldInfo customPacketField = AccessTools.Field(typeof(Packet_Client), "CustomPacket");

		// Token: 0x040000D5 RID: 213
		private static FieldInfo channelIdField = AccessTools.Field(typeof(Packet_CustomPacket), "ChannelId");

		// Token: 0x040000D6 RID: 214
		private static FieldInfo SocketField = AccessTools.Field(typeof(ConnectedClient), "socket");

		// Token: 0x040000D7 RID: 215
		private static FieldInfo PlayerField = AccessTools.Field(typeof(ConnectedClient), "Player");
	}
}
