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
	// Token: 0x0200006C RID: 108
	public class ServerSystemNetworkProcess : IDisposable
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060002A0 RID: 672 RVA: 0x00010870 File Offset: 0x0000EA70
		// (remove) Token: 0x060002A1 RID: 673 RVA: 0x000108A8 File Offset: 0x0000EAA8
		public event Func<int, Packet_CustomPacket, IServerPlayer, bool> OnProcessInBackground;

		// Token: 0x060002A2 RID: 674 RVA: 0x000108DD File Offset: 0x0000EADD
		public ServerSystemNetworkProcess(ICoreServerAPI sapi)
		{
			this.api = sapi;
			this.packetProcessingThread = new Thread(new ParameterizedThreadStart(this.NetworkProcess));
			this._packetProcessingCTS = new CancellationTokenSource();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0001090E File Offset: 0x0000EB0E
		public void Launch()
		{
			this.packetProcessingThread.Start(this._packetProcessingCTS.Token);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0001092C File Offset: 0x0000EB2C
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

		// Token: 0x060002A5 RID: 677 RVA: 0x00010A18 File Offset: 0x0000EC18
		private void TryReadPacket(byte[] data, int dataLength, IServerPlayer sender)
		{
			Packet_Client packet = new Packet_Client();
			Packet_ClientSerializer.DeserializeBuffer(data, dataLength, packet);
			this.ProcessInBackground(packet, sender);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00010A40 File Offset: 0x0000EC40
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

		// Token: 0x060002A7 RID: 679 RVA: 0x00010A9C File Offset: 0x0000EC9C
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

		// Token: 0x060002A8 RID: 680 RVA: 0x00010B90 File Offset: 0x0000ED90
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

		// Token: 0x0400011E RID: 286
		private const int _customPacketId = 23;

		// Token: 0x0400011F RID: 287
		private ICoreServerAPI api;

		// Token: 0x04000120 RID: 288
		private Thread packetProcessingThread;

		// Token: 0x04000121 RID: 289
		private CancellationTokenSource _packetProcessingCTS;

		// Token: 0x04000122 RID: 290
		private static FieldInfo senderConnectionField = AccessTools.Field(typeof(NetIncomingMessage), "SenderConnection");

		// Token: 0x04000123 RID: 291
		private static FieldInfo messageField = AccessTools.Field(typeof(NetIncomingMessage), "message");

		// Token: 0x04000124 RID: 292
		private static FieldInfo messageLengthField = AccessTools.Field(typeof(NetIncomingMessage), "messageLength");

		// Token: 0x04000125 RID: 293
		private static FieldInfo packetIdField = AccessTools.Field(typeof(Packet_Client), "Id");

		// Token: 0x04000126 RID: 294
		private static FieldInfo customPacketField = AccessTools.Field(typeof(Packet_Client), "CustomPacket");

		// Token: 0x04000127 RID: 295
		private static FieldInfo channelIdField = AccessTools.Field(typeof(Packet_CustomPacket), "ChannelId");

		// Token: 0x04000128 RID: 296
		private static FieldInfo SocketField = AccessTools.Field(typeof(ConnectedClient), "socket");

		// Token: 0x04000129 RID: 297
		private static FieldInfo PlayerField = AccessTools.Field(typeof(ConnectedClient), "Player");
	}
}
