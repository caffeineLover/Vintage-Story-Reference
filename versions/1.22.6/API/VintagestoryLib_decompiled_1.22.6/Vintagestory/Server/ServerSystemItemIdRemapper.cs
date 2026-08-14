using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProtoBuf;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace Vintagestory.Server;

public class ServerSystemItemIdRemapper : ServerSystem
{
	public ServerSystemItemIdRemapper(ServerMain server)
		: base(server)
	{
		server.ModEventManager.AssetsFirstLoaded += RemapItems;
		CommandArgumentParsers parsers = server.api.ChatCommands.Parsers;
		server.api.ChatCommands.Create("iir").RequiresPrivilege(Privilege.controlserver).WithDescription("Item id remapper info and fixing tool")
			.BeginSubCommand("list")
			.WithDescription("list")
			.HandleWith(OnCmdList)
			.EndSubCommand()
			.BeginSubCommand("getcode")
			.WithDescription("getcode")
			.WithArgs(parsers.Int("itemId"))
			.HandleWith(OnCmdGetcode)
			.EndSubCommand()
			.BeginSubCommand("getid")
			.WithDescription("getid")
			.WithArgs(parsers.Word("domainAndPath"))
			.HandleWith(OnCmdGetid)
			.EndSubCommand()
			.BeginSubCommand("map")
			.WithDescription("map")
			.RequiresPlayer()
			.WithArgs(parsers.Word("new_item"), parsers.Word("old_item"), parsers.OptionalWord("force"))
			.HandleWith(OnCmdMap)
			.EndSubCommand()
			.BeginSubCommand("remap")
			.WithAlias("remapq")
			.WithDescription("map")
			.RequiresPlayer()
			.WithArgs(parsers.Word("new_item"), parsers.Word("old_item"), parsers.OptionalWord("force"))
			.HandleWith(OnCmdReMap)
			.EndSubCommand();
	}

	private TextCommandResult OnCmdReMap(TextCommandCallingArgs args)
	{
		Dictionary<int, AssetLocation> storedItemCodesById = LoadStoredItemCodesById();
		bool quiet = args.SubCmdCode == "remapq";
		string newItem = args[0] as string;
		string oldItem = args[1] as string;
		bool force = !args.Parsers[2].IsMissing && args[2] as string == "force";
		IServerPlayer player = args.Caller.Player as IServerPlayer;
		if (int.TryParse(newItem, out var newItemId) && int.TryParse(oldItem, out var oldItemId))
		{
			MapById(storedItemCodesById, newItemId, oldItemId, player, args.Caller.FromChatGroupId, remap: true, force, quiet);
			return TextCommandResult.Success();
		}
		MapByCode(storedItemCodesById, new AssetLocation(newItem), new AssetLocation(oldItem), player, args.Caller.FromChatGroupId, remap: true, force, quiet);
		return TextCommandResult.Success();
	}

	private TextCommandResult OnCmdMap(TextCommandCallingArgs args)
	{
		Dictionary<int, AssetLocation> storedItemCodesById = LoadStoredItemCodesById();
		string newItem = args[0] as string;
		string oldItem = args[1] as string;
		bool force = !args.Parsers[2].IsMissing && args[2] as string == "force";
		IServerPlayer player = args.Caller.Player as IServerPlayer;
		if (int.TryParse(newItem, out var newItemId) && int.TryParse(oldItem, out var oldItemId))
		{
			MapById(storedItemCodesById, newItemId, oldItemId, player, args.Caller.FromChatGroupId, remap: false, force, quiet: false);
			return TextCommandResult.Success();
		}
		MapByCode(storedItemCodesById, new AssetLocation(newItem), new AssetLocation(oldItem), player, args.Caller.FromChatGroupId, remap: false, force, quiet: false);
		return TextCommandResult.Success();
	}

	private TextCommandResult OnCmdGetid(TextCommandCallingArgs args)
	{
		Dictionary<int, AssetLocation> storedItemCodesById = LoadStoredItemCodesById();
		string domainAndPath = args[0] as string;
		if (!storedItemCodesById.ContainsValue(new AssetLocation(domainAndPath)))
		{
			return TextCommandResult.Success("No mapping for itemcode " + domainAndPath + " found");
		}
		return TextCommandResult.Success("Itemcode " + domainAndPath + " is currently mapped to " + storedItemCodesById.FirstOrDefault((KeyValuePair<int, AssetLocation> x) => x.Value.Equals(new AssetLocation(domainAndPath))).Key);
	}

	private TextCommandResult OnCmdGetcode(TextCommandCallingArgs args)
	{
		Dictionary<int, AssetLocation> storedItemCodesById = LoadStoredItemCodesById();
		int itemId = (int)args[0];
		if (!storedItemCodesById.ContainsKey(itemId))
		{
			return TextCommandResult.Success("No mapping for itemid " + itemId + " found");
		}
		return TextCommandResult.Success("itemid " + itemId + " is currently mapped to " + storedItemCodesById[itemId]);
	}

	private TextCommandResult OnCmdList(TextCommandCallingArgs args)
	{
		Dictionary<int, AssetLocation> dictionary = LoadStoredItemCodesById();
		ServerMain.Logger.Notification("Current item id mapping (issued by /bir list command)");
		foreach (KeyValuePair<int, AssetLocation> val in dictionary)
		{
			ServerMain.Logger.Notification("  " + val.Key + ": " + val.Value);
		}
		return TextCommandResult.Success("Full mapping printed to console and main log file");
	}

	private void MapById(Dictionary<int, AssetLocation> storedItemCodesById, int newItemId, int oldItemId, IServerPlayer player, int groupId, bool remap, bool force, bool quiet)
	{
		if (!force && storedItemCodesById.TryGetValue(oldItemId, out var value))
		{
			player.SendMessage(groupId, string.Concat(new string[11]
			{
				"newitemid ",
				oldItemId.ToString(),
				" is already mapped to ",
				value,
				", type '/bir ",
				remap ? "remap" : "map",
				" ",
				newItemId.ToString(),
				" ",
				oldItemId.ToString(),
				" force' to overwrite"
			}), EnumChatType.CommandError);
			return;
		}
		AssetLocation newCode = (storedItemCodesById[oldItemId] = storedItemCodesById[newItemId]);
		if (remap)
		{
			storedItemCodesById.Remove(newItemId);
		}
		if (!quiet)
		{
			string type = (remap ? "remapped" : "mapped");
			player.SendMessage(groupId, string.Concat(new string[5]
			{
				newCode,
				" is now ",
				type,
				" to id ",
				oldItemId.ToString()
			}), EnumChatType.CommandSuccess);
		}
		StoreItemCodesById(storedItemCodesById);
	}

	private void MapByCode(Dictionary<int, AssetLocation> storedItemCodesById, AssetLocation newCode, AssetLocation oldCode, IServerPlayer player, int groupId, bool remap, bool force, bool quiet)
	{
		if (!storedItemCodesById.ContainsValue(newCode))
		{
			player.SendMessage(groupId, string.Concat("No mapping for itemcode ", newCode, " found"), EnumChatType.CommandError);
			return;
		}
		if (!storedItemCodesById.ContainsValue(oldCode))
		{
			player.SendMessage(groupId, string.Concat("No mapping for itemcode ", oldCode, " found"), EnumChatType.CommandError);
			return;
		}
		if (!force)
		{
			player.SendMessage(groupId, string.Concat(new string[7]
			{
				"Both item codes found. Type '/bir ",
				remap ? "remap" : "map",
				" ",
				newCode,
				" ",
				oldCode,
				" force' to make the remap permanent."
			}), EnumChatType.CommandError);
			return;
		}
		int newItemId = storedItemCodesById.FirstOrDefault((KeyValuePair<int, AssetLocation> x) => x.Value.Equals(newCode)).Key;
		int oldItemId = storedItemCodesById.FirstOrDefault((KeyValuePair<int, AssetLocation> x) => x.Value.Equals(oldCode)).Key;
		storedItemCodesById[oldItemId] = newCode;
		if (remap)
		{
			storedItemCodesById.Remove(newItemId);
		}
		if (!quiet)
		{
			string type = (remap ? "remapped" : "mapped");
			player.SendMessage(groupId, string.Concat(new string[5]
			{
				newCode,
				" is now ",
				type,
				" to id ",
				oldItemId.ToString()
			}), EnumChatType.CommandSuccess);
		}
		StoreItemCodesById(storedItemCodesById);
	}

	public bool AutoRemap(string oldItem, string newItem, Dictionary<int, AssetLocation> storedItemCodesById)
	{
		AssetLocation oldCode = new AssetLocation(oldItem);
		int oldId = storedItemCodesById.FirstOrDefault((KeyValuePair<int, AssetLocation> x) => x.Value.Equals(oldCode)).Key;
		if (oldId == 0 && !storedItemCodesById.ContainsValue(oldCode))
		{
			return false;
		}
		storedItemCodesById[oldId] = new AssetLocation(newItem);
		return true;
	}

	public void RemapItems()
	{
		ServerMain.Logger.Debug("ItemID Remapper: Begin");
		Dictionary<AssetLocation, int> currentItemIdsByCode = new Dictionary<AssetLocation, int>();
		Dictionary<int, AssetLocation> storedItemCodesById = new Dictionary<int, AssetLocation>();
		Dictionary<int, AssetLocation> newItemCodesById = new Dictionary<int, AssetLocation>();
		Dictionary<int, AssetLocation> missingItemCodesById = new Dictionary<int, AssetLocation>();
		Dictionary<int, int> remappedItemIds = new Dictionary<int, int>();
		for (int i = 0; i < server.Items.Count; i++)
		{
			Item item = server.Items[i];
			if (item != null && !(item.Code == null))
			{
				currentItemIdsByCode[item.Code] = item.ItemId;
			}
		}
		storedItemCodesById = LoadStoredItemCodesById();
		if (storedItemCodesById == null)
		{
			storedItemCodesById = new Dictionary<int, AssetLocation>();
		}
		int maxItemId = 0;
		foreach (KeyValuePair<int, AssetLocation> val in storedItemCodesById)
		{
			AssetLocation code = val.Value;
			int oldItemId = val.Key;
			maxItemId = Math.Max(oldItemId, maxItemId);
			if (!currentItemIdsByCode.ContainsKey(code))
			{
				missingItemCodesById.Add(oldItemId, code);
				continue;
			}
			int newItemId = currentItemIdsByCode[code];
			if (newItemId != oldItemId)
			{
				remappedItemIds[newItemId] = oldItemId;
			}
		}
		for (int j = 0; j < server.Items.Count; j++)
		{
			if (server.Items[j] != null)
			{
				maxItemId = Math.Max(server.Items[j].ItemId, maxItemId);
			}
		}
		server.nextFreeItemId = maxItemId + 1;
		bool isNewWorld = storedItemCodesById.Count == 0;
		HashSet<AssetLocation> storedItemCodes = new HashSet<AssetLocation>(storedItemCodesById.Values);
		foreach (KeyValuePair<AssetLocation, int> val2 in currentItemIdsByCode)
		{
			AssetLocation code2 = val2.Key;
			int ItemID = val2.Value;
			if (ItemID != 0 && !storedItemCodes.Contains(code2))
			{
				newItemCodesById[ItemID] = code2;
			}
		}
		ServerMain.Logger.Debug("Found {0} Item requiring remapping", remappedItemIds.Count);
		StringBuilder codes = new StringBuilder();
		List<Item> newItems = new List<Item>();
		foreach (KeyValuePair<int, AssetLocation> item2 in newItemCodesById)
		{
			int curItemId = item2.Key;
			Item Item = server.Items[curItemId];
			newItems.Add(Item);
			if (!isNewWorld)
			{
				server.Items[curItemId] = new Item();
			}
		}
		List<Item> ItemsToRemap = new List<Item>();
		foreach (KeyValuePair<int, int> val3 in remappedItemIds)
		{
			if (codes.Length > 0)
			{
				codes.Append(", ");
			}
			int oldItemId2 = val3.Key;
			int newItemId2 = val3.Value;
			Item Item2 = server.Items[oldItemId2];
			Item2.ItemId = newItemId2;
			ItemsToRemap.Add(Item2);
			server.Items[oldItemId2] = new Item();
			codes.Append(oldItemId2 + "=>" + newItemId2);
		}
		foreach (Item Item3 in ItemsToRemap)
		{
			server.RemapItem(Item3);
		}
		if (!isNewWorld)
		{
			int newItemsCount = 0;
			foreach (Item Item4 in newItems)
			{
				if (Item4.ItemId != 0)
				{
					server.ItemsByCode.Remove(Item4.Code);
					server.RegisterItem(Item4);
					newItemsCount++;
				}
			}
			ServerMain.Logger.Debug("Remapped {0} new Itemids", newItemsCount);
		}
		maxItemId = 0;
		for (int k = 0; k < server.Items.Count; k++)
		{
			if (server.Items[k] != null)
			{
				maxItemId = Math.Max(server.Items[k].ItemId, maxItemId);
			}
		}
		server.nextFreeItemId = maxItemId + 1;
		if (codes.Length > 0)
		{
			ServerMain.Logger.VerboseDebug("Remapped existing Itemids: {0}", codes.ToString());
		}
		ServerMain.Logger.Debug("Found {0} missing Items", missingItemCodesById.Count);
		codes = new StringBuilder();
		foreach (KeyValuePair<int, AssetLocation> val4 in missingItemCodesById)
		{
			if (codes.Length > 0)
			{
				codes.Append(", ");
			}
			server.FillMissingItem(val4.Key, new Item
			{
				Textures = new Dictionary<string, CompositeTexture> { 
				{
					"all",
					new CompositeTexture(new AssetLocation("unknown"))
				} },
				IsMissing = true,
				Code = val4.Value
			});
			codes.Append(val4.Value.ToShortString());
		}
		if (codes.Length > 0)
		{
			ServerMain.Logger.Debug("Added unknown Item for {0} Items", missingItemCodesById.Count);
			ServerMain.Logger.Debug(codes.ToString());
		}
		StringBuilder newItemCodes = new StringBuilder();
		foreach (Item Item5 in newItems)
		{
			storedItemCodesById[Item5.ItemId] = Item5.Code;
			if (newItemCodes.Length > 0)
			{
				newItemCodes.Append(", ");
			}
			newItemCodes.Append(string.Concat(Item5.Code, "(", Item5.ItemId.ToString(), ")"));
		}
		if (newItems.Count > 0)
		{
			ServerMain.Logger.Debug("Added {0} new Items to the mapping", newItems.Count);
		}
		StoreItemCodesById(storedItemCodesById);
	}

	public Dictionary<int, AssetLocation> LoadStoredItemCodesById()
	{
		Dictionary<int, AssetLocation> Items = new Dictionary<int, AssetLocation>();
		try
		{
			byte[] data = server.api.WorldManager.SaveGame.GetData("ItemIDs");
			if (data != null)
			{
				Dictionary<int, string> dictionary = Serializer.Deserialize<Dictionary<int, string>>(new MemoryStream(data));
				Items = new Dictionary<int, AssetLocation>();
				foreach (KeyValuePair<int, string> entry in dictionary)
				{
					Items.Add(entry.Key, new AssetLocation(entry.Value));
				}
				ServerMain.Logger.Debug("Item IDs loaded from savegame.");
			}
			else
			{
				ServerMain.Logger.Debug("Item IDs not found in savegame.");
			}
		}
		catch
		{
			throw new Exception("Error at loading Items!");
		}
		return Items;
	}

	public void StoreItemCodesById(Dictionary<int, AssetLocation> storedItemCodesById)
	{
		MemoryStream ms = new MemoryStream();
		Dictionary<int, string> itemsOld = new Dictionary<int, string>();
		foreach (KeyValuePair<int, AssetLocation> entry in storedItemCodesById)
		{
			itemsOld.Add(entry.Key, entry.Value.ToShortString());
		}
		Serializer.Serialize((Stream)ms, itemsOld);
		server.api.WorldManager.SaveGame.StoreData("ItemIDs", ms.ToArray());
		ServerMain.Logger.Debug("Item IDs have been written to savegame");
	}
}
