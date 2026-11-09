using System.Collections.Generic;

namespace Vintagestory.Server;

public class ServerSystemNotifyPing : ServerSystem
{
	private Timer pingtimer = new Timer
	{
		Interval = 1.0,
		MaxDeltaTime = 5.0
	};

	public ServerSystemNotifyPing(ServerMain server)
		: base(server)
	{
		server.RegisterGameTickListener(OnEveryFewSeconds, 5000);
		server.PacketHandlers[2] = HandlePingReply;
		server.PacketHandlingOnConnectingAllowed[2] = true;
	}

	public override void OnServerTick(float dt)
	{
		pingtimer.Update(PingTimerTick);
	}

	private void OnEveryFewSeconds(float t1)
	{
		server.BroadcastPlayerPings();
	}

	private void HandlePingReply(Packet_Client packet, ConnectedClient client)
	{
		client.Ping.OnReceive(server.totalUnpausedTime.ElapsedMilliseconds);
		client.LastPing = (float)client.Ping.RoundtripTimeTotalMilliseconds() / 1000f;
	}

	private void PingTimerTick()
	{
		if (server.exitState.Exit)
		{
			return;
		}
		long currentMs = server.totalUnpausedTime.ElapsedMilliseconds;
		List<int> clientsToKick = new List<int>();
		foreach (var (clientId, client) in server.Clients)
		{
			if (!client.Ping.DidReplyOnLastPing)
			{
				if (client.Ping.DidTimeout(currentMs) && !client.IsSinglePlayerClient)
				{
					long seconds = (currentMs - client.Ping.TimeSendMilliSeconds) / 1000;
					ServerMain.Logger.Notification(seconds + "s ping timeout for " + client.PlayerName + ". Kicking player...");
					clientsToKick.Add(clientId);
				}
			}
			else
			{
				server.SendPacket(clientId, ServerPackets.Ping());
				client.Ping.OnSend(currentMs);
			}
			if (!client.FallBackToTcp && !client.IsSinglePlayerClient && client.Ping.DidUdpTimeout(currentMs))
			{
				client.FallBackToTcp = true;
				Packet_Server packetTcp = new Packet_Server
				{
					Id = 78
				};
				server.SendPacket(clientId, packetTcp);
				float seconds2 = (float)(currentMs - client.Ping.TimeReceivedUdp) / 1000f;
				ServerMain.Logger.Debug($"UDP: Server did not receive any UDP packets from Client {client.Id} for {seconds2}s, telling the client to send positions over TCP, server switches to TCP too.");
			}
		}
		foreach (int key in clientsToKick)
		{
			if (server.Clients.TryGetValue(key, out var connectedClient2))
			{
				server.DisconnectPlayer(connectedClient2);
			}
		}
	}
}
