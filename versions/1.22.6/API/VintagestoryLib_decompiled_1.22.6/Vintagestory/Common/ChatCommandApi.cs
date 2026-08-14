using System;
using System.Collections;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.CommandAbbr;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace Vintagestory.Common;

public class ChatCommandApi : IChatCommandApi, IEnumerable<KeyValuePair<string, IChatCommand>>, IEnumerable
{
	public static string ClientCommandPrefix = ".";

	public static string ServerCommandPrefix = "/";

	internal Dictionary<string, IChatCommand> ichatCommands = new Dictionary<string, IChatCommand>(StringComparer.OrdinalIgnoreCase);

	private ICoreAPI api;

	private CommandArgumentParsers parsers;

	public string CommandPrefix
	{
		get
		{
			if (api.Side != EnumAppSide.Client)
			{
				return ServerCommandPrefix;
			}
			return ClientCommandPrefix;
		}
	}

	public CommandArgumentParsers Parsers => parsers;

	public int Count => ichatCommands.Count;

	public IChatCommand this[string name] => ichatCommands[name];

	public IEnumerator<IChatCommand> GetEnumerator()
	{
		foreach (IChatCommand value in ichatCommands.Values)
		{
			yield return value;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ichatCommands.GetEnumerator();
	}

	public ChatCommandApi(ICoreAPI api)
	{
		this.api = api;
		parsers = new CommandArgumentParsers(api);
	}

	public IChatCommand Get(string name)
	{
		ichatCommands.TryGetValue(name, out var cmd);
		return cmd;
	}

	public IChatCommand Create()
	{
		return new ChatCommandImpl(this);
	}

	public IChatCommand Create(string commandName)
	{
		return new ChatCommandImpl(this).WithName(commandName.ToLowerInvariant());
	}

	public IChatCommand GetOrCreate(string commandName)
	{
		commandName = commandName.ToLowerInvariant();
		return Get(commandName) ?? Create().WithName(commandName);
	}

	public IEnumerable<IChatCommand> ListAll()
	{
		return ichatCommands.Values;
	}

	public Dictionary<string, IChatCommand> AllSubcommands()
	{
		return ichatCommands;
	}

	public void Execute(string commandName, TextCommandCallingArgs args, Action<TextCommandResult> onCommandComplete = null)
	{
		commandName = commandName.ToLowerInvariant();
		if (ichatCommands.TryGetValue(commandName, out var cmd))
		{
			if (api.Side == EnumAppSide.Server && cmd.Incomplete)
			{
				throw new InvalidOperationException("Programming error: Incomplete command - no name or required privilege has been set");
			}
			if (api.Side == EnumAppSide.Client && (cmd as ChatCommandImpl).AnyPrivilegeSet)
			{
				args.Caller.CallerPrivileges = null;
			}
			args.LanguageCode = (args.Caller.Player as IServerPlayer)?.LanguageCode ?? Lang.CurrentLocale;
			cmd.Execute(args, onCommandComplete);
		}
		else
		{
			onCommandComplete(new TextCommandResult
			{
				Status = EnumCommandStatus.NoSuchCommand,
				ErrorCode = "nosuchcommand"
			});
		}
	}

	public void ExecuteUnparsed(string message, TextCommandCallingArgs args, Action<TextCommandResult> onCommandComplete = null)
	{
		message = message.Substring(1);
		int argsStart = message.IndexOf(' ');
		string strargs;
		string command;
		if (argsStart > 0)
		{
			strargs = message.Substring(argsStart + 1);
			command = message.Substring(0, argsStart);
		}
		else
		{
			strargs = "";
			command = message;
		}
		args.RawArgs = new CmdArgs(strargs);
		Execute(command, args, onCommandComplete);
	}

	public void Execute(string commandName, IServerPlayer player, int groupId, string args, Action<TextCommandResult> onCommandComplete = null)
	{
		api.Logger.Audit("Handling command for {0} /{1} {2}", player.PlayerName, commandName, args);
		string langCode = player.LanguageCode;
		try
		{
			Execute(commandName, new TextCommandCallingArgs
			{
				Caller = new Caller
				{
					Player = player,
					Pos = player.Entity?.Pos.XYZ,
					FromChatGroupId = groupId
				},
				RawArgs = new CmdArgs(args)
			}, delegate(TextCommandResult results)
			{
				if (results.StatusMessage != null && results.StatusMessage.Length > 0)
				{
					string message = results.StatusMessage;
					if (results.StatusMessage.IndexOf('\n') == -1)
					{
						message = ((results.MessageParams == null) ? Lang.GetL(langCode, results.StatusMessage) : Lang.GetL(langCode, results.StatusMessage, results.MessageParams));
					}
					player.SendMessage(groupId, message, (results.Status != EnumCommandStatus.Success) ? EnumChatType.CommandError : EnumChatType.CommandSuccess);
				}
				if (results.Status == EnumCommandStatus.NoSuchCommand)
				{
					player.SendMessage(groupId, Lang.GetL(langCode, "No such command exists"), EnumChatType.CommandError);
					SuggestCommands(player, groupId, commandName);
				}
				if (results.Status == EnumCommandStatus.Error)
				{
					player.SendMessage(groupId, Lang.GetL(langCode, "For help, type <code>/help {0}</code>", commandName), EnumChatType.CommandError);
				}
				onCommandComplete?.Invoke(results);
			});
		}
		catch (Exception ex)
		{
			api.Logger.Error("Player {0}/{1} caused an exception through a command.", player.PlayerName, player.PlayerUID);
			api.Logger.Error("Command: /{0} {1}", commandName, args);
			api.Logger.Error(ex);
			string err = "An Exception was thrown while executing Command: {0}. Check error log for more detail.";
			player.SendMessage(groupId, Lang.GetL(langCode, err, ex.Message), EnumChatType.CommandError);
			onCommandComplete?.Invoke(TextCommandResult.Error(err, "exception"));
		}
	}

	public void Execute(string commandName, IClientPlayer player, int groupId, string args, Action<TextCommandResult> onCommandComplete = null)
	{
		Execute(commandName, new TextCommandCallingArgs
		{
			Caller = new Caller
			{
				Player = player,
				FromChatGroupId = groupId,
				CallerPrivileges = new string[1] { "*" }
			},
			RawArgs = new CmdArgs(args)
		}, delegate(TextCommandResult results)
		{
			if (results.StatusMessage != null)
			{
				player.ShowChatNotification(Lang.Get(results.StatusMessage));
			}
			if (results.Status == EnumCommandStatus.NoSuchCommand)
			{
				player.ShowChatNotification(Lang.Get("No such command exists"));
			}
			onCommandComplete?.Invoke(results);
		});
	}

	private void SuggestCommands(IServerPlayer player, int groupId, string commandName)
	{
		string similarCommand = null;
		int minDist = 99;
		foreach (KeyValuePair<string, IChatCommand> val in ichatCommands)
		{
			int distance = LevenshteinDistance(val.Key, commandName);
			if (distance < 4 && distance < commandName.Length / 2 && minDist > distance)
			{
				similarCommand = val.Key;
				minDist = distance;
			}
		}
		if (similarCommand != null)
		{
			player.SendMessage(groupId, Lang.Get("command-suggestion", similarCommand), EnumChatType.CommandError);
		}
	}

	public static int LevenshteinDistance(string source1, string source2)
	{
		int source1Length = source1.Length;
		int source2Length = source2.Length;
		int[,] matrix = new int[source1Length + 1, source2Length + 1];
		if (source1Length == 0)
		{
			return source2Length;
		}
		if (source2Length == 0)
		{
			return source1Length;
		}
		int i = 0;
		while (i <= source1Length)
		{
			matrix[i, 0] = i++;
		}
		int j = 0;
		while (j <= source2Length)
		{
			matrix[0, j] = j++;
		}
		for (int k = 1; k <= source1Length; k++)
		{
			for (int l = 1; l <= source2Length; l++)
			{
				int cost = ((source2[l - 1] != source1[k - 1]) ? 1 : 0);
				matrix[k, l] = Math.Min(Math.Min(matrix[k - 1, l] + 1, matrix[k, l - 1] + 1), matrix[k - 1, l - 1] + cost);
			}
		}
		return matrix[source1Length, source2Length];
	}

	internal void UnregisterCommand(string name)
	{
		ichatCommands.Remove(name);
	}

	internal virtual bool RegisterCommand(string command, string descriptionMsg, string syntaxMsg, ClientChatCommandDelegate handler, string requiredPrivilege = null)
	{
		try
		{
			Create(command).WithDesc(descriptionMsg + "\nSyntax:" + syntaxMsg).RequiresPrivilege(requiredPrivilege).WithArgs(parsers.Unparsed("legacy args"))
				.HandleWith(delegate(TextCommandCallingArgs args)
				{
					handler(args.Caller.FromChatGroupId, args.RawArgs);
					return new TextCommandResult
					{
						Status = EnumCommandStatus.UnknownLegacy
					};
				});
		}
		catch (InvalidOperationException e)
		{
			api.Logger.Warning("Command {0}{1} already registered:", ClientCommandPrefix, command);
			api.Logger.Warning(e);
			return false;
		}
		return true;
	}

	internal virtual bool RegisterCommand(ChatCommand chatCommand)
	{
		try
		{
			Create(chatCommand.Command).WithDesc(chatCommand.Description + "\nSyntax:" + chatCommand.Syntax).RequiresPrivilege(chatCommand.RequiredPrivilege).WithArgs(parsers.Unparsed("legacy args"))
				.HandleWith(delegate(TextCommandCallingArgs args)
				{
					chatCommand.CallHandler(args.Caller.Player, args.Caller.FromChatGroupId, args.RawArgs);
					return new TextCommandResult
					{
						Status = EnumCommandStatus.UnknownLegacy
					};
				});
		}
		catch (InvalidOperationException e)
		{
			api.Logger.Warning("Command {0}{1} already registered:", (chatCommand is ServerChatCommand) ? ServerCommandPrefix : ClientCommandPrefix, chatCommand.Command);
			api.Logger.Warning(e);
			return false;
		}
		return true;
	}

	[Obsolete("Better to directly use new ChatCommands api instead")]
	public bool RegisterCommand(string command, string descriptionMsg, string syntaxMsg, ServerChatCommandDelegate handler, string requiredPrivilege = null)
	{
		try
		{
			Create(command).WithDesc(descriptionMsg + "\nSyntax:" + syntaxMsg).RequiresPrivilege(requiredPrivilege).WithArgs(parsers.Unparsed("legacy args"))
				.HandleWith(delegate(TextCommandCallingArgs args)
				{
					handler(args.Caller.Player as IServerPlayer, args.Caller.FromChatGroupId, args.RawArgs);
					return new TextCommandResult
					{
						Status = EnumCommandStatus.UnknownLegacy
					};
				});
		}
		catch (InvalidOperationException e)
		{
			api.Logger.Warning("Command {0}{1} already registered:", ClientCommandPrefix, command);
			api.Logger.Warning(e);
			return false;
		}
		return true;
	}

	public bool RegisterCommand(string command, string descriptionMsg, string syntaxMsg, ClientChatCommandDelegate handler)
	{
		return RegisterCommand(command, descriptionMsg, syntaxMsg, handler, null);
	}

	IEnumerator<KeyValuePair<string, IChatCommand>> IEnumerable<KeyValuePair<string, IChatCommand>>.GetEnumerator()
	{
		return ichatCommands.GetEnumerator();
	}
}
