using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Common.CommandAbbr;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.Common;

namespace Vintagestory.Server;

internal class CmdGlobalList
{
	private ServerMain server;

	public CmdGlobalList(ServerMain server)
	{
		this.server = server;
		server.api.commandapi.Create("list").RequiresPrivilege(Privilege.readlists).WithDesc("Show global lists (clients, banned, roles or privileges)")
			.BeginSub("clients")
			.WithAlias("c")
			.WithDesc("Players who are currently online")
			.HandleWith(listClients)
			.EndSub()
			.BeginSub("banned")
			.WithAlias("b")
			.WithDesc("Users who are banned from this server")
			.HandleWith(listBanned)
			.EndSub()
			.BeginSub("roles")
			.WithAlias("r")
			.WithDesc("Available roles")
			.HandleWith(listRoles)
			.EndSub()
			.BeginSub("privileges")
			.WithAlias("p")
			.WithDesc("Available privileges")
			.HandleWith(listPrivileges)
			.EndSub()
			.HandleWith(handleList);
	}

	private TextCommandResult listClients(TextCommandCallingArgs args)
	{
		StringBuilder text = new StringBuilder();
		text.AppendLine(Lang.Get("List of online Players"));
		foreach (ConnectedClient client in new List<ConnectedClient>(server.Clients.Values).OrderBy((ConnectedClient c) => c.State))
		{
			if (client.State == EnumClientState.Queued)
			{
				int q = server.ConnectionQueue.FindIndex((QueuedClient c) => c.Client.Id == client.Id);
				if (q >= 0)
				{
					QueuedClient queueClient = server.ConnectionQueue[q];
					global::_003C_003Ey__InlineArray6<object> buffer = default(global::_003C_003Ey__InlineArray6<object>);
					buffer[0] = client.Id;
					buffer[1] = queueClient.Identification.Playername;
					buffer[2] = client.Socket.RemoteEndPoint();
					buffer[3] = (int)(client.LastPing * 1000f);
					buffer[4] = q + 1;
					buffer[5] = client.State;
					text.AppendLine(string.Format("{5} [{0}] {1} {2} ({3}ms) | Queue position: ({4})", (ReadOnlySpan<object?>)buffer));
				}
				else
				{
					ServerMain.Logger.Warning("Client {0} not found in connection queue", client.Id);
				}
			}
			else
			{
				long inactiveSeconds = (server.ElapsedMilliseconds - client.LastActivityTotalMs) / 1000;
				global::_003C_003Ey__InlineArray6<object> buffer2 = default(global::_003C_003Ey__InlineArray6<object>);
				buffer2[0] = client.Id;
				buffer2[1] = client.PlayerName;
				buffer2[2] = client.Socket.RemoteEndPoint();
				buffer2[3] = (int)(client.LastPing * 1000f);
				buffer2[4] = client.State;
				buffer2[5] = inactiveSeconds;
				text.AppendLine(string.Format("{4} [{0}] {1} {2} ({3}ms) ({5}s inactive)", (ReadOnlySpan<object?>)buffer2));
			}
		}
		return TextCommandResult.Success(text.ToString());
	}

	private TextCommandResult listBanned(TextCommandCallingArgs args)
	{
		StringBuilder text = new StringBuilder();
		text.AppendLine(Lang.Get("List of Banned Users:"));
		foreach (PlayerEntry entry in server.PlayerDataManager.BannedPlayers)
		{
			string reason = entry.Reason;
			if (string.IsNullOrEmpty(reason))
			{
				reason = "";
			}
			if (entry.UntilDate >= DateTime.Now)
			{
				text.AppendLine($"{entry.PlayerName} until {entry.UntilDate}. Reason: {reason}");
			}
		}
		return TextCommandResult.Success(text.ToString());
	}

	private TextCommandResult listRoles(TextCommandCallingArgs args)
	{
		StringBuilder text = new StringBuilder();
		text.AppendLine(Lang.Get("List of roles:"));
		foreach (PlayerRole group in server.Config.Roles)
		{
			text.AppendLine(group.ToString());
		}
		return TextCommandResult.Success(text.ToString());
	}

	private TextCommandResult listPrivileges(TextCommandCallingArgs args)
	{
		StringBuilder text = new StringBuilder();
		text.AppendLine(Lang.Get("Available privileges:"));
		foreach (string privilege in server.AllPrivileges)
		{
			text.AppendLine(privilege.ToString());
		}
		return TextCommandResult.Success(text.ToString());
	}

	private TextCommandResult handleList(TextCommandCallingArgs args)
	{
		return TextCommandResult.Error("Syntax error, requires argument clients|banned|roles|privileges or c|b|r|p");
	}
}
