using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace Vintagestory.Server;

public class ServerSystemKickAfkPlayers : ServerSystem
{
	private HashSet<string> warnedClients = new HashSet<string>();

	public override int GetUpdateInterval()
	{
		return 1000;
	}

	public ServerSystemKickAfkPlayers(ServerMain server)
		: base(server)
	{
	}

	public override void OnServerTick(float dt)
	{
		if (server.Config.KickClientsAfterAfkSeconds <= 0)
		{
			return;
		}
		foreach (var (_, client) in server.Clients)
		{
			if (client.State != EnumClientState.Playing || client.Player == null)
			{
				continue;
			}
			long inactiveSeconds = (server.ElapsedMilliseconds - client.LastActivityTotalMs) / 1000;
			if (server.Config.WarnClientsAfterAfkSeconds > 0 && inactiveSeconds >= server.Config.WarnClientsAfterAfkSeconds)
			{
				if (!warnedClients.Contains(client.Player.PlayerUID))
				{
					warnedClients.Add(client.Player.PlayerUID);
					string msg = Lang.Get("You have been inactive for {0} seconds. You will be kicked after {1} seconds of inactivity.", (int)inactiveSeconds, server.Config.KickClientsAfterAfkSeconds);
					server.SendMessage(client.Player, GlobalConstants.CurrentChatGroup, msg, EnumChatType.Notification);
				}
			}
			else
			{
				warnedClients.Remove(client.Player.PlayerUID);
			}
			if (inactiveSeconds >= server.Config.KickClientsAfterAfkSeconds)
			{
				ServerMain.Logger.Audit("Kicking player {0} for being inactive for {1} seconds", client.Player.PlayerName, inactiveSeconds);
				server.DisconnectPlayer(client, Lang.Get("{0} has been kicked for being inactive for too long", client.Player.PlayerName), Lang.Get("You have been kicked for being inactive for too long."));
			}
		}
	}
}
