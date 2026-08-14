using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace Vintagestory.Server;

public class ServerSystemRemapperAssistant : ServerSystem
{
	private Vintagestory.API.Datastructures.OrderedDictionary<string, string[]> remaps = new Vintagestory.API.Datastructures.OrderedDictionary<string, string[]>();

	public ServerSystemRemapperAssistant(ServerMain server)
		: base(server)
	{
		server.ModEventManager.AssetsFirstLoaded += AutoRemapBlocksAndItems;
		server.api.ChatCommands.Create("fixmapping").RequiresPrivilege(Privilege.controlserver).BeginSubCommand("doremap")
			.WithDescription("Do remap")
			.WithArgs(server.api.ChatCommands.Parsers.Word("code"))
			.HandleWith(OnCmdDoremap)
			.EndSubCommand()
			.BeginSubCommand("ignoreall")
			.WithDescription("Ignore all remappings")
			.HandleWith(OnCmdIgnoreall)
			.EndSubCommand()
			.BeginSubCommand("applyall")
			.WithDescription("Apply all remappings")
			.WithArgs(server.api.ChatCommands.Parsers.OptionalWord("force"))
			.HandleWith(OnCmdApplyall)
			.EndSubCommand();
	}

	private void AutoRemapBlocksAndItems()
	{
		Dictionary<AssetLocation, Vintagestory.API.Datastructures.OrderedDictionary<string, string[]>> allRemaps = server.AssetManager.GetMany<Vintagestory.API.Datastructures.OrderedDictionary<string, string[]>>(ServerMain.Logger, "config/remaps.json");
		remaps = allRemaps.First((KeyValuePair<AssetLocation, Vintagestory.API.Datastructures.OrderedDictionary<string, string[]>> v) => v.Key.Domain == "game").Value;
		foreach (KeyValuePair<AssetLocation, Vintagestory.API.Datastructures.OrderedDictionary<string, string[]>> item in allRemaps.Where((KeyValuePair<AssetLocation, Vintagestory.API.Datastructures.OrderedDictionary<string, string[]>> v) => v.Key.Domain != "game"))
		{
			foreach (KeyValuePair<string, string[]> remapSet in item.Value)
			{
				if (remapSet.Value.Length != 0)
				{
					if (!remaps.TryGetValue(remapSet.Key, out var existingRemapItems))
					{
						remaps.Add(remapSet.Key, remapSet.Value);
						continue;
					}
					string[] array = existingRemapItems;
					string[] value = remapSet.Value;
					int num = array.Length + value.Length;
					List<string> list = new List<string>(num);
					CollectionsMarshal.SetCount(list, num);
					Span<string> span = CollectionsMarshal.AsSpan(list);
					int num2 = 0;
					ReadOnlySpan<string> readOnlySpan = new ReadOnlySpan<string>(array);
					readOnlySpan.CopyTo(span.Slice(num2, readOnlySpan.Length));
					num2 += readOnlySpan.Length;
					ReadOnlySpan<string> readOnlySpan2 = new ReadOnlySpan<string>(value);
					readOnlySpan2.CopyTo(span.Slice(num2, readOnlySpan2.Length));
					num2 += readOnlySpan2.Length;
					List<string> mergedRemapItems = list;
					remaps[remapSet.Key] = mergedRemapItems.ToArray();
				}
			}
		}
		if (server.SaveGameData.IsNewWorld)
		{
			foreach (string code in remaps.Keys)
			{
				server.SaveGameData.RemappingsAppliedByCode[code] = true;
			}
			return;
		}
		Vintagestory.API.Datastructures.OrderedDictionary<string, string[]> remapsToApply = new Vintagestory.API.Datastructures.OrderedDictionary<string, string[]>(remaps);
		foreach (string code2 in server.SaveGameData.RemappingsAppliedByCode.Keys)
		{
			remapsToApply.Remove(code2);
		}
		if (remapsToApply.Count > 0)
		{
			if (server.Config.DisableAutoRemap)
			{
				server.requiresRemaps = true;
			}
			else
			{
				AutoRemapBlocksItems(remapsToApply);
			}
		}
		else
		{
			ServerMain.Logger.Notification("Remapper: No applicable remappings found for this save game, all seems good for now");
		}
	}

	private TextCommandResult OnCmdApplyall(TextCommandCallingArgs args)
	{
		int commandsExecuted = 0;
		int setsprocessed = 0;
		bool force = args[0] as string == "force";
		IServerPlayer player = args.Caller.Player as IServerPlayer;
		if (player == null && args.Caller.Type == EnumCallerType.Console)
		{
			player = server.ServerConsoleClient.Player;
		}
		foreach (KeyValuePair<string, string[]> val in remaps)
		{
			string code = val.Key;
			if (!(!server.SaveGameData.RemappingsAppliedByCode.ContainsKey(code) || force))
			{
				continue;
			}
			setsprocessed++;
			string[] value = val.Value;
			for (int i = 0; i < value.Length; i++)
			{
				string command = value[i].Trim();
				if (command.Length != 0)
				{
					server.HandleChatMessage(player, args.Caller.FromChatGroupId, command);
					commandsExecuted++;
				}
			}
			server.SaveGameData.RemappingsAppliedByCode[code] = true;
		}
		if (commandsExecuted == 0)
		{
			return TextCommandResult.Success("No applicable remappings found, seems all good for now!");
		}
		return TextCommandResult.Success($"Okay, {setsprocessed} remapping sets with a total of {commandsExecuted} remappings commands have been executed. You can now restart your game/server");
	}

	private void AutoRemapBlocksItems(Vintagestory.API.Datastructures.OrderedDictionary<string, string[]> remapsToApply)
	{
		ServerSystemBlockIdRemapper blockRemapper = server.Systems.First((ServerSystem s) => s is ServerSystemBlockIdRemapper) as ServerSystemBlockIdRemapper;
		ServerSystemItemIdRemapper itemRemapper = server.Systems.First((ServerSystem s) => s is ServerSystemItemIdRemapper) as ServerSystemItemIdRemapper;
		int commandsExecuted = 0;
		int setsprocessed = 0;
		Dictionary<int, AssetLocation> storedBlockCodesById = null;
		Dictionary<int, AssetLocation> storedItemCodesById = null;
		foreach (KeyValuePair<string, string[]> val in remapsToApply)
		{
			setsprocessed++;
			string[] value = val.Value;
			for (int num = 0; num < value.Length; num++)
			{
				string cmdTrimmed = value[num].Trim();
				if (cmdTrimmed.Length == 0)
				{
					continue;
				}
				string[] cmdSplit = cmdTrimmed.Split(" ");
				if (cmdSplit.Length < 4)
				{
					continue;
				}
				string command = cmdSplit[0];
				if (command.Equals("/bir"))
				{
					if (storedBlockCodesById == null)
					{
						storedBlockCodesById = blockRemapper.LoadStoredBlockCodesById();
					}
					if (blockRemapper.AutoRemap(cmdSplit[3], cmdSplit[2], storedBlockCodesById))
					{
						commandsExecuted++;
					}
					else
					{
						ServerMain.Logger.Warning("Failed remaps entry (" + cmdSplit[3] + " not found): " + cmdTrimmed);
					}
				}
				else if (command.Equals("/iir"))
				{
					if (storedItemCodesById == null)
					{
						storedItemCodesById = itemRemapper.LoadStoredItemCodesById();
					}
					if (itemRemapper.AutoRemap(cmdSplit[3], cmdSplit[2], storedItemCodesById))
					{
						commandsExecuted++;
					}
					else
					{
						ServerMain.Logger.Warning("Failed remaps entry (" + cmdSplit[3] + " not found): " + cmdTrimmed);
					}
				}
				else
				{
					ServerMain.Logger.Debug("Unrecognised remaps command: " + cmdTrimmed);
				}
			}
			if (storedBlockCodesById != null)
			{
				blockRemapper.StoreBlockCodesById(storedBlockCodesById);
			}
			if (storedItemCodesById != null)
			{
				itemRemapper.StoreItemCodesById(storedItemCodesById);
			}
			server.SaveGameData.RemappingsAppliedByCode[val.Key] = true;
		}
		if (setsprocessed > 0)
		{
			ServerMain.Logger.Notification($"Remapper: Updating a saved world from earlier game version. Applied {setsprocessed} remapping sets with a total of {commandsExecuted} remappings.");
		}
	}

	private TextCommandResult OnCmdIgnoreall(TextCommandCallingArgs args)
	{
		foreach (KeyValuePair<string, string[]> remap in remaps)
		{
			string code = remap.Key;
			if (!server.SaveGameData.RemappingsAppliedByCode.ContainsKey(code))
			{
				server.SaveGameData.RemappingsAppliedByCode[code] = false;
			}
		}
		return TextCommandResult.Success(Lang.Get("Okay, ignoring all new remappings. You can still manually remap them using /fixmapping doremap [code]"));
	}

	private TextCommandResult OnCmdDoremap(TextCommandCallingArgs args)
	{
		string code = args[0] as string;
		if (!remaps.ContainsKey(code))
		{
			return TextCommandResult.Success(Lang.Get("No remapping group found under this code"));
		}
		string[] array = remaps[code];
		int commandsExecuted = 0;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string command = array2[i].Trim();
			if (command.Length != 0)
			{
				server.HandleChatMessage(args.Caller.Player as IServerPlayer, args.Caller.FromChatGroupId, command);
				commandsExecuted++;
			}
		}
		server.SaveGameData.RemappingsAppliedByCode[code] = true;
		return TextCommandResult.Success(Lang.Get("Ok, {0} commands executed.", commandsExecuted));
	}

	public override void Dispose()
	{
		BlockSchematic.BlockRemaps = null;
		BlockSchematic.ItemRemaps = null;
	}

	public override void OnFinalizeAssets()
	{
		extractRemapsForSchematicImports();
		foreach (KeyValuePair<AssetLocation, string[]> item in server.AssetManager.GetMany<string[]>(ServerMain.Logger, "config/remapentities.json"))
		{
			string[] value = item.Value;
			for (int i = 0; i < value.Length; i++)
			{
				string[] cmdSplit = value[i].Split(" ");
				if (cmdSplit[0].Equals("/eir"))
				{
					server.EntityCodeRemappings.TryAdd(cmdSplit[3], cmdSplit[2]);
				}
			}
		}
	}

	private void extractRemapsForSchematicImports()
	{
		BlockSchematic.BlockRemaps = new Vintagestory.API.Datastructures.OrderedDictionary<string, Dictionary<string, string>>();
		BlockSchematic.ItemRemaps = new Vintagestory.API.Datastructures.OrderedDictionary<string, Dictionary<string, string>>();
		foreach (KeyValuePair<string, string[]> remapping in remaps)
		{
			Dictionary<string, string> blockMapping = new Dictionary<string, string>();
			Dictionary<string, string> itemMapping = new Dictionary<string, string>();
			string[] value = remapping.Value;
			foreach (string cmd in value)
			{
				string[] cmdSplit = cmd.Split(" ");
				string command = cmdSplit[0];
				if (command.Equals("/bir"))
				{
					blockMapping.TryAdd(cmdSplit[3], cmdSplit[2]);
				}
				else if (command.Equals("/iir"))
				{
					itemMapping.TryAdd(cmdSplit[3], cmdSplit[2]);
				}
				else
				{
					ServerMain.Logger.Debug("Unrecognised remaps command: " + cmd);
				}
			}
			if (blockMapping.Count > 0)
			{
				BlockSchematic.BlockRemaps.TryAdd(remapping.Key, blockMapping);
			}
			if (itemMapping.Count > 0)
			{
				BlockSchematic.ItemRemaps.TryAdd(remapping.Key, itemMapping);
			}
		}
	}
}
