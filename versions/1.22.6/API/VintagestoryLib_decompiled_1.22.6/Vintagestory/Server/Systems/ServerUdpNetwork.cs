using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ProtoBuf;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;
using Vintagestory.Common;
using Vintagestory.Common.Network.Packets;

namespace Vintagestory.Server.Systems;

public class ServerUdpNetwork : ServerSystem
{
	public readonly Dictionary<string, ConnectedClient> connectingClients = new Dictionary<string, ConnectedClient>();

	public PhysicsManager physicsManager;

	internal ServerUdpQueue ImmediateUdpQueue;

	public ServerUdpNetwork(ServerMain server)
		: base(server)
	{
		server.RegisterGameTickListener(ServerTickUdp, 15);
		physicsManager = new PhysicsManager(server.api, this);
		server.PacketHandlers[35] = EnqueueUdpPacket;
	}

	private void EnqueueUdpPacket(Packet_Client packet, ConnectedClient player)
	{
		UdpPacket udpPacket = new UdpPacket
		{
			Packet = packet.UdpPacket,
			Client = player
		};
		server.UdpSockets[1].EnqueuePacket(udpPacket);
	}

	private void ServerTickUdp(float obj)
	{
		UNetServer[] udpSockets = server.UdpSockets;
		foreach (UNetServer udpSocket in udpSockets)
		{
			UdpPacket[] packets = udpSocket?.ReadMessage();
			if (packets == null)
			{
				continue;
			}
			UdpPacket[] array = packets;
			for (int j = 0; j < array.Length; j++)
			{
				UdpPacket packet = array[j];
				server.TotalReceivedBytesUdp += packet.Packet.Length;
				switch (packet.Packet.Id)
				{
				case 1:
					HandleConnectionRequest(packet, udpSocket);
					break;
				case 2:
					HandlePlayerPosition(packet.Packet.EntityPosition, packet.Client.Player);
					break;
				case 3:
					HandleMountPosition(packet.Packet.EntityPosition, packet.Client.Player, packet.Packet.ChannelPacket);
					break;
				case 6:
					server.HandleCustomUdpPackets(packet.Packet.ChannelPacket, packet.Client.Player);
					break;
				case 7:
					packet.Client.Ping.OnReceiveUdp(server.ElapsedMilliseconds);
					break;
				}
			}
		}
	}

	public override void OnBeginInitialization()
	{
		physicsManager.Init();
	}

	public override void OnPlayerDisconnect(ServerPlayer player)
	{
		KeyValuePair<string, ConnectedClient> cc = connectingClients.FirstOrDefault((KeyValuePair<string, ConnectedClient> c) => c.Value.Player?.Equals(player) ?? false);
		if (cc.Key != null)
		{
			connectingClients.Remove(cc.Key);
		}
		if (player.client.IsSinglePlayerClient)
		{
			server.UdpSockets[0].Remove(player);
		}
		else
		{
			server.UdpSockets[1].Remove(player);
		}
	}

	private void HandleConnectionRequest(UdpPacket udpPacket, UNetServer uNetServer)
	{
		try
		{
			Packet_ConnectionPacket packet = udpPacket.Packet.ConnectionPacket;
			if (packet == null)
			{
				return;
			}
			connectingClients.TryGetValue(packet?.LoginToken, out var client);
			if (client == null || uNetServer.EndPoints.ContainsKey(udpPacket.EndPoint))
			{
				return;
			}
			connectingClients.Remove(packet.LoginToken);
			uNetServer.Add(udpPacket.EndPoint, client.Id);
			client.ServerDidReceiveUdp = true;
			server.api.Logger.Debug($"UDP: Client {client.Id} connected via: {udpPacket.EndPoint}");
			Packet_Server didRecive = new Packet_Server
			{
				Id = 81
			};
			server.SendPacket(client.Id, didRecive);
			string clientLoginToken = client.LoginToken;
			if (client.IsSinglePlayerClient)
			{
				return;
			}
			TyronThreadPool.QueueTask(async delegate
			{
				Packet_UdpPacket con = new Packet_UdpPacket
				{
					Id = 1,
					ConnectionPacket = new Packet_ConnectionPacket
					{
						LoginToken = clientLoginToken
					}
				};
				for (int i = 0; i < 20; i++)
				{
					server.ServerUdpNetwork.SendPacket_Threadsafe(client, con);
					await Task.Delay(500);
					if (client.State == EnumClientState.Offline || client.FallBackToTcp)
					{
						break;
					}
				}
			}, $"UDPServerSendConnectTo_{client.Id}");
		}
		catch (Exception e)
		{
			server.api.Logger.Warning($"Error when connecting UDP client from {udpPacket.EndPoint}");
			server.api.Logger.Warning(e);
		}
	}

	public void HandlePlayerPosition(Packet_EntityPosition packet, ServerPlayer player)
	{
		if (packet == null)
		{
			return;
		}
		EntityPlayer entity = player.Entity;
		int version = entity.WatchedAttributes.GetInt("positionVersionNumber");
		if (packet.PositionVersion < version)
		{
			return;
		}
		player.LastReceivedClientPosition = server.ElapsedMilliseconds;
		int currentTick = entity.Attributes.GetInt("tick");
		currentTick++;
		entity.Attributes.SetInt("tick", currentTick);
		if (!entity.Pos.SetFromPacket(packet, entity))
		{
			entity.Api.Logger.Audit("Rejected player position update for " + player.PlayerName + ". Client sent " + packet.X / 16384 + "," + packet.Y / 16384 + "," + packet.Z / 16384 + ", server pos was " + entity.Pos);
			SendSinglePositionPacket(player, entity, currentTick);
			return;
		}
		foreach (EntityBehavior behavior in entity.SidedProperties.Behaviors)
		{
			if (behavior is IRemotePhysics remote)
			{
				remote.OnReceivedClientPos(version);
				break;
			}
		}
		Packet_EntityPosition entityPositionPacket = ServerPackets.getEntityPositionPacket(entity.Pos, entity, currentTick);
		entityPositionPacket.BodyYaw = CollectibleNet.SerializeFloatPrecise(entity.BodyYawServer);
		Packet_UdpPacket packetBytesUdp = new Packet_UdpPacket
		{
			Id = 5,
			EntityPosition = entityPositionPacket
		};
		entity.IsTeleport = false;
		AnimationPacket animationPacket = null;
		bool animationsDirty = entity.AnimManager?.AnimationsDirty ?? false;
		if (animationsDirty)
		{
			animationPacket = new AnimationPacket(entity);
			entity.AnimManager.AnimationsDirty = false;
		}
		foreach (ConnectedClient client in server.Clients.Values)
		{
			ServerPlayer sp = client.Player;
			if (sp != null && sp != player && client.TrackedEntities.Contains(entity.EntityId))
			{
				ImmediateUdpQueue.QueuePacket(client, packetBytesUdp);
				if (animationsDirty)
				{
					physicsManager.AnimationsAndTagsChannel.SendPacket(animationPacket, sp);
				}
			}
		}
	}

	private void SendSinglePositionPacket(ServerPlayer player, Entity entity, int currentTick)
	{
		Packet_UdpPacket udpPacket = new Packet_UdpPacket
		{
			Id = 5,
			EntityPosition = ServerPackets.getEntityPositionPacket(entity.Pos, entity, currentTick)
		};
		ImmediateUdpQueue.QueuePacket(player.client, udpPacket);
	}

	public void HandleMountPosition(Packet_EntityPosition packet, ServerPlayer player, Packet_CustomPacket clientAnimationsAndGaitPacket)
	{
		if (packet == null)
		{
			return;
		}
		Entity entity = server.api.World.GetEntityById(packet.EntityId);
		IMountable mount = entity?.GetInterface<IMountable>();
		if (mount == null || !mount.IsMountedBy(player.Entity))
		{
			return;
		}
		int version = entity.WatchedAttributes.GetInt("positionVersionNumber");
		if (packet.PositionVersion < version)
		{
			return;
		}
		int currentTick = entity.Attributes.GetInt("tick");
		currentTick++;
		entity.Attributes.SetInt("tick", currentTick);
		if (!entity.Pos.SetFromPacket(packet, entity))
		{
			entity.Api.Logger.Audit("Rejected mount position update for " + player.PlayerName + ". Client sent " + packet.X / 16384 + "," + packet.Y / 16384 + "," + packet.Z / 16384 + ", server pos was " + entity.Pos);
			SendSinglePositionPacket(player, entity, currentTick);
			return;
		}
		((entity.SidedProperties == null) ? null : entity.GetInterface<IMountable>()?.ControllingControls)?.FromInt(packet.MountControls);
		foreach (EntityBehavior behavior in entity.SidedProperties.Behaviors)
		{
			if (behavior is IRemotePhysics remote)
			{
				remote.OnReceivedClientPos(version);
				break;
			}
		}
		Packet_EntityPosition entityPositionPacket = ServerPackets.getEntityPositionPacket(entity.Pos, entity, currentTick);
		Packet_UdpPacket packetBytesUdp = new Packet_UdpPacket
		{
			Id = 5,
			EntityPosition = entityPositionPacket
		};
		entity.IsTeleport = false;
		AnimationPacket animationPacket = null;
		string gaitCode = null;
		if (clientAnimationsAndGaitPacket != null)
		{
			entity.AnimManager.AnimationsDirty = false;
			MountAnimationPacket combiPacket = null;
			using (MemoryStream ms = new MemoryStream(clientAnimationsAndGaitPacket.Data))
			{
				combiPacket = Serializer.Deserialize<MountAnimationPacket>(ms);
			}
			if (combiPacket != null)
			{
				gaitCode = combiPacket.gaitCode;
				animationPacket = combiPacket.animPacket;
				server.api.Event.TriggerMountGaitReceived(entity, gaitCode);
			}
		}
		ICollection<ConnectedClient> values = server.Clients.Values;
		List<ServerPlayer> otherPlayersInRange = ((values.Count <= 1 || animationPacket == null) ? null : new List<ServerPlayer>());
		foreach (ConnectedClient client in values)
		{
			if (client.Player != null && client.Player != player && client.TrackedEntities.Contains(entity.EntityId))
			{
				ImmediateUdpQueue.QueuePacket(client, packetBytesUdp);
				if (otherPlayersInRange != null && client.Player != player)
				{
					otherPlayersInRange.Add(client.Player);
				}
			}
		}
		if (otherPlayersInRange != null && otherPlayersInRange.Count > 0)
		{
			IServerNetworkChannel animationsAndTagsChannel = physicsManager.AnimationsAndTagsChannel;
			AnimationPacket message = animationPacket;
			IServerPlayer[] players = otherPlayersInRange.ToArray();
			animationsAndTagsChannel.SendPacket(message, players);
		}
	}

	public void SendPacket_Threadsafe(ConnectedClient client, Packet_BulkEntityPosition contentPacket)
	{
		Packet_UdpPacket packet = new Packet_UdpPacket
		{
			Id = 4,
			BulkPositions = contentPacket
		};
		ImmediateUdpQueue.QueuePacket(client, packet);
	}

	public void SendPacket_Threadsafe(ConnectedClient client, Packet_CustomPacket prePacked)
	{
		Packet_UdpPacket udpPacket = new Packet_UdpPacket
		{
			Id = 6,
			ChannelPacket = prePacked
		};
		ImmediateUdpQueue.QueuePacket(client, udpPacket);
	}

	public void SendPacket_Threadsafe(ConnectedClient client, Packet_UdpPacket prePacked)
	{
		ImmediateUdpQueue.QueuePacket(client, prePacked);
	}
}
