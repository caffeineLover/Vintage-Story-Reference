using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class SystemRemotePlayerTracking : ModSystem
{
	private INetworkChannel channel;

	private ICoreAPI api;

	private readonly Queue<IServerPlayer> playerQueue = new Queue<IServerPlayer>();

	private readonly Dictionary<string, PacketPlayerPosition> trackedPlayerPackets = new Dictionary<string, PacketPlayerPosition>();

	public PacketPlayerPosition? GetPlayerPositionInformation(string uid)
	{
		return trackedPlayerPackets.GetValueOrDefault(uid);
	}

	public IEnumerable<PacketPlayerPosition> GetAllTrackedPlayerPositions()
	{
		return trackedPlayerPackets.Values;
	}

	public override void StartPre(ICoreAPI api)
	{
		channel = api.Network.RegisterChannel("remoteplayertracker");
		channel.RegisterMessageType<PacketPlayerPosition>();
		this.api = api;
		ICoreAPI coreAPI = api;
		if (!(coreAPI is ICoreServerAPI sapi) || !api.World.Config.GetBool("allowMap"))
		{
			if (!(coreAPI is ICoreClientAPI))
			{
				return;
			}
			((IClientNetworkChannel)channel).SetMessageHandler(delegate(PacketPlayerPosition p)
			{
				if (!string.IsNullOrEmpty(p.PlayerUid))
				{
					if (p.Despawn)
					{
						trackedPlayerPackets.Remove(p.PlayerUid);
					}
					else
					{
						p.AssociatedPlayer = api.World.PlayerByUid(p.PlayerUid);
						trackedPlayerPackets[p.PlayerUid] = p;
					}
				}
			});
		}
		else
		{
			sapi.Event.PlayerJoin += ServerEvent_PlayerJoin;
			sapi.Event.PlayerDisconnect += ServerEvent_PlayerDisconnect;
			sapi.Event.RegisterGameTickListener(OnTick, 100);
		}
	}

	private void OnTick(float dt)
	{
		if (playerQueue.Count != 0)
		{
			IServerPlayer player = playerQueue.Dequeue();
			if (player.Entity != null && player.ConnectionState == EnumClientState.Playing)
			{
				sendPlayerPacket(player);
			}
			playerQueue.Enqueue(player);
		}
	}

	private void sendPlayerPacket(IServerPlayer player)
	{
		EntityPos playerPos = player.Entity.Pos;
		PacketPlayerPosition packet = new PacketPlayerPosition
		{
			PlayerUid = player.PlayerUID,
			PosX = playerPos.X,
			PosZ = playerPos.Z,
			Yaw = playerPos.Yaw
		};
		PacketPlayerPosition despawnPacket = new PacketPlayerPosition
		{
			PlayerUid = player.PlayerUID,
			Despawn = true
		};
		((IServerNetworkChannel)channel).SendPacket(packet, player);
		if (api.World.Config.GetBool("mapHideOtherPlayers") || player.WorldData.CurrentGameMode == EnumGameMode.Spectator)
		{
			((IServerNetworkChannel)channel).BroadcastPacket(despawnPacket, player);
			return;
		}
		double playerRenderDistance = api.World.Config.GetFloat("mapPlayerRenderDistance", 1000f);
		if (playerRenderDistance < 0.0)
		{
			((IServerNetworkChannel)channel).BroadcastPacket(packet, player);
			return;
		}
		int[] ourGroups = player.Groups.Select((PlayerGroupMembership group) => group.GroupUid).ToArray();
		bool shouldCheckGroups = api.World.Config.GetBool("mapShowGroupPlayers", defaultValue: true) && ourGroups.Length != 0;
		foreach (IServerPlayer playerToReceive in api.World.AllOnlinePlayers.Cast<IServerPlayer>())
		{
			if (!(playerToReceive.PlayerUID == player.PlayerUID))
			{
				EntityPlayer entity = playerToReceive.Entity;
				if ((entity != null && entity.Pos.HorDistanceTo(playerPos) <= playerRenderDistance) || (shouldCheckGroups && playerToReceive.Groups.Any((PlayerGroupMembership group) => ourGroups.Contains(group.GroupUid))))
				{
					((IServerNetworkChannel)channel).SendPacket(packet, playerToReceive);
				}
				else
				{
					((IServerNetworkChannel)channel).SendPacket(despawnPacket, playerToReceive);
				}
			}
		}
	}

	private void ServerEvent_PlayerJoin(IServerPlayer byPlayer)
	{
		playerQueue.Enqueue(byPlayer);
	}

	private void ServerEvent_PlayerDisconnect(IServerPlayer byPlayer)
	{
		for (int i = playerQueue.Count; i > 0; i--)
		{
			IServerPlayer sp = playerQueue.Dequeue();
			if (sp.PlayerUID == byPlayer.PlayerUID)
			{
				break;
			}
			playerQueue.Enqueue(sp);
		}
		PacketPlayerPosition packet = new PacketPlayerPosition
		{
			PlayerUid = byPlayer.PlayerUID,
			Despawn = true
		};
		((IServerNetworkChannel)channel).BroadcastPacket(packet);
	}
}
