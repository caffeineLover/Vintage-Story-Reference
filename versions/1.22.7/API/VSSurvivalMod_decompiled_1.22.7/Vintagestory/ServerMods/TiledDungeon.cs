using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace Vintagestory.ServerMods;

public class TiledDungeon
{
	public bool Worldgen;

	public string Code;

	public List<DungeonTile> Tiles = new List<DungeonTile>();

	public Dictionary<string, DungeonTile> TilesByCode = new Dictionary<string, DungeonTile>();

	[JsonIgnore]
	public float totalChance;

	public float chance;

	public int MinSpawnDistance;

	public string? start;

	[JsonIgnore]
	public BlockSchematicPartial[]? Start;

	public string? surface;

	[JsonIgnore]
	public BlockSchematicPartial[]? Surface;

	public string? SurfaceConnectorName;

	public int surfaceYOffset;

	public TiledDungeon? StairCase;

	public string[]? ends;

	[JsonIgnore]
	public BlockSchematicPartial[][]? EndSchematics;

	[JsonProperty]
	public int MaxTiles;

	[JsonProperty]
	public int MinTiles;

	[JsonProperty]
	public string[]? RequireClosed;

	[JsonProperty]
	public string[]? RequireOpened;

	[JsonProperty]
	public HashSet<string>? RequireUnblocked;

	[JsonProperty]
	public bool BuildProtected;

	[JsonProperty]
	public string? BuildProtectionName;

	[JsonProperty]
	public string? BuildProtectionDesc;

	[JsonProperty]
	public LandformConstraint[]? RequiredLandform;

	internal Dictionary<int, Dictionary<int, int>>? resolvedRockTypeRemaps;

	public string? RockTypeRemapGroup;

	[JsonProperty]
	public AssetLocation[]? ReplaceWithBlocklayers;

	internal int[] replacewithblocklayersBlockids = Array.Empty<int>();

	[JsonProperty]
	public Dictionary<string, int>? GroupMax;

	[JsonProperty]
	public int MaxDepth { get; set; }

	[JsonProperty]
	public int MinDepth { get; set; }

	public void Init(ICoreServerAPI api)
	{
		totalChance = 0f;
		BlockLayerConfig blockLayerConfig = BlockLayerConfig.GetInstance(api);
		blockLayerConfig.ResolveBlockIds(api);
		if (surface != null)
		{
			IAsset asset = api.Assets.GetManyInCategory("worldgen", "dungeontiles/" + surface + ".json").FirstOrDefault();
			Surface = WorldGenStructureBase.LoadSchematic<BlockSchematicPartial>(api, asset, blockLayerConfig, null, null, 0, isDungeon: true);
			TilesByCode[surface] = new DungeonTile
			{
				ResolvedSchematics = new BlockSchematicPartial[1][] { Surface },
				Code = surface
			};
		}
		if (start != null)
		{
			IAsset assetStart = api.Assets.GetManyInCategory("worldgen", "dungeontiles/" + start + ".json").FirstOrDefault();
			Start = WorldGenStructureBase.LoadSchematic<BlockSchematicPartial>(api, assetStart, blockLayerConfig, null, null, 0, isDungeon: true);
			TilesByCode[start] = new DungeonTile
			{
				ResolvedSchematics = new BlockSchematicPartial[1][] { Start },
				Code = start
			};
		}
		if (this.ends != null)
		{
			List<BlockSchematicPartial[]> ends = new List<BlockSchematicPartial[]>();
			string[] array = this.ends;
			foreach (string e in array)
			{
				IAsset assetEnd = api.Assets.GetManyInCategory("worldgen", "dungeontiles/" + e + ".json").FirstOrDefault();
				BlockSchematicPartial[] blockSchematicPartials = WorldGenStructureBase.LoadSchematic<BlockSchematicPartial>(api, assetEnd, blockLayerConfig, null, null, 0, isDungeon: true);
				ends.Add(blockSchematicPartials);
				if (!TilesByCode.TryAdd(e, new DungeonTile
				{
					ResolvedSchematics = new BlockSchematicPartial[1][] { blockSchematicPartials },
					Code = e
				}))
				{
					api.Logger.Error($"Dungeon {Code} has a end with duplicate tile code: {e}. Tile will be skipped.");
				}
			}
			EndSchematics = ends.ToArray();
		}
		StairCase?.Init(api);
		if (RequiredLandform != null && RequiredLandform.Any((LandformConstraint l) => l.Code == null || l.Type == null))
		{
			api.Logger.Error("Dungeon " + Code + " has RequiredLandform with missing Code or Type.");
		}
		for (int i2 = 0; i2 < Tiles.Count; i2++)
		{
			DungeonTile tile = Tiles[i2];
			if (Tiles[i2].Code == null)
			{
				api.Logger.Error($"Dungeon {Code} has a Tile at index: {i2} without a code specified. Will skip initialization");
				continue;
			}
			tile.Init(api, blockLayerConfig);
			if (tile.TileGenerator != null)
			{
				foreach (DungeonTile genTile in tile.TileGenerator.Tiles)
				{
					if (!TilesByCode.TryAdd(genTile.Code, genTile))
					{
						api.Logger.Error($"Dungeon {Code} has a TileGenerator with duplicate tile code: {genTile.Code}. Tile will be skipped.");
					}
				}
			}
			else if (!TilesByCode.TryAdd(tile.Code, tile))
			{
				api.Logger.Error($"Dungeon {Code} has a TileGenerator with duplicate tile code: {tile.Code}. Tile will be skipped.");
			}
			totalChance += tile.Chance;
		}
		if (RequireClosed == null && RequireOpened != null)
		{
			HashSet<string> toBeClosed = new HashSet<string>();
			foreach (DungeonTile tile2 in Tiles)
			{
				string[] array = tile2.CachedNames;
				foreach (string name in array)
				{
					if (!RequireOpened.Contains(name))
					{
						toBeClosed.Add(name);
					}
				}
			}
			RequireClosed = toBeClosed.ToArray();
			api.Logger.Debug("Dungeon " + Code + " Setting RequireClosed to " + string.Join(",", RequireClosed));
		}
		if (ReplaceWithBlocklayers == null)
		{
			return;
		}
		replacewithblocklayersBlockids = new int[ReplaceWithBlocklayers.Length];
		for (int j = 0; j < replacewithblocklayersBlockids.Length; j++)
		{
			Block block = api.World.GetBlock(ReplaceWithBlocklayers[j]);
			if (block == null)
			{
				throw new Exception($"Dungeon with code {Code} has replace block layer {ReplaceWithBlocklayers[j]} defined, but no such block found!");
			}
			replacewithblocklayersBlockids[j] = block.Id;
		}
	}
}
