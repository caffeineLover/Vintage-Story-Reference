using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace Vintagestory.Server;

internal class CmdAnnounce
{
	public CmdAnnounce(ServerMain server)
	{
		IChatCommandApi cmdapi = server.api.ChatCommands;
		CommandArgumentParsers parsers = server.api.ChatCommands.Parsers;
		cmdapi.Create("announce").RequiresPrivilege(Privilege.announce).WithDescription("Announce a server wide message in all groups. If range is set, then it will only send this message to players in this block range.")
			.WithArgs(parsers.All("announcement message"), parsers.OptionalInt("range"))
			.HandleWith(delegate(TextCommandCallingArgs args)
			{
				ServerMain.Logger.Event($"{args.Caller.GetName()} announced: {(string)args[0]}.");
				server.BroadcastMessageToAllGroups("<strong><font color=\"orange\">" + (string)args[0] + "</font></strong>", EnumChatType.AllGroups);
				return TextCommandResult.Success();
			});
		cmdapi.Create("announcenear").RequiresPrivilege(Privilege.announce).WithDescription("Announce a server wide message in all groups. Only sends the message to players in given block range.")
			.WithArgs(parsers.OptionalInt("range"), parsers.All("announcement message"))
			.HandleWith(delegate(TextCommandCallingArgs args)
			{
				int num = (int)args[0];
				IPlayer[] allOnlinePlayers = server.api.World.AllOnlinePlayers;
				for (int i = 0; i < allOnlinePlayers.Length; i++)
				{
					IServerPlayer serverPlayer = (IServerPlayer)allOnlinePlayers[i];
					if (args.Caller.Pos.DistanceTo(serverPlayer.Entity.Pos.XYZ) <= (float)num)
					{
						serverPlayer.SendMessage(GlobalConstants.AllChatGroups, "<strong><font color=\"orange\">" + (string)args[1] + "</font></strong>", EnumChatType.Notification);
					}
				}
				return TextCommandResult.Success();
			});
	}
}
