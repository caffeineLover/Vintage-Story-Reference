using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.CommandAbbr;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.Common.Collectible.Block;
using Vintagestory.GameContent;

namespace Vintagestory.ServerMods;

public class ModSystemTiledDungeons : ModSystem
{
	protected ICoreServerAPI sapi;

	public TiledDungeonConfig Tcfg;

	internal DungeonGenerator dungeonGen;

	private TextCommandCallingArgs? dungeonGenRequest;

	private int debugStopStep = -1;

	private bool placeDebugConnectors;

	private bool debugLogging;

	private DungeonPlaceTask? placeTask;

	private TiledDungeon? lastDungeon;

	public override void StartServerSide(ICoreServerAPI api)
	{
		sapi = api;
		dungeonGen = new DungeonGenerator(api.World.Logger, api.World.BlockAccessor.MapSize);
		api.Event.RegisterEventBusListener(onEvent, 0.0, "wgenregendone");
		CommandArgumentParsers parsers = api.ChatCommands.Parsers;
		api.ChatCommands.GetOrCreate("debug").BeginSub("tddebug").WithDesc("Toogle debug logging on/off for tiledd commands")
			.RequiresPrivilege(Privilege.controlserver)
			.WithArgs(parsers.OptionalBool("mode"))
			.HandleWith(delegate(TextCommandCallingArgs args)
			{
				debugLogging = (bool)args[0];
				return TextCommandResult.Success("Debug logging now " + (debugLogging ? "on" : "off"));
			})
			.EndSub()
			.BeginSub("tiledd")
			.WithDesc("Generate a dungeon at the callers position")
			.RequiresPrivilege(Privilege.controlserver)
			.WithArgs(parsers.Word("tiled dungeon code"), parsers.Int("amount of tiles"), parsers.OptionalWorldPosition("pos"), parsers.OptionalWord("randomSeed"))
			.HandleWith(OnCmdGenDeungon)
			.EndSub()
			.BeginSub("tiledds")
			.WithDesc("Generate a dungeon at the callers position, stepped")
			.RequiresPrivilege(Privilege.controlserver)
			.WithArgs(parsers.Word("tiled dungeon code"), parsers.Int("amount of tiles"), parsers.OptionalWorldPosition("pos"), parsers.OptionalWord("randomSeed"))
			.HandleWith(OnCmdGenDeungonStepped)
			.EndSub()
			.BeginSub("tdstep")
			.WithDesc("Generate a dungeon at the callers position, stepped")
			.RequiresPrivilege(Privilege.controlserver)
			.HandleWith(delegate
			{
				if (placeTask == null || lastDungeon == null)
				{
					return TextCommandResult.Error("No dungeon to step");
				}
				if (placeTask.TilePlaceTasks.Count > debugStopStep)
				{
					debugStopStep++;
				}
				placeDungeon(sapi.World.BlockAccessor, lastDungeon, debugStopStep, placeTask);
				return TextCommandResult.Success("Stepped to " + debugStopStep + " / " + placeTask.TilePlaceTasks.Count);
			})
			.EndSub()
			.BeginSub("tdstepreset")
			.WithDesc("Generate a dungeon at the callers position, stepped")
			.RequiresPrivilege(Privilege.controlserver)
			.HandleWith(delegate
			{
				debugStopStep = -1;
				lastDungeon = null;
				placeTask = null;
				return TextCommandResult.Success();
			})
			.EndSub()
			.BeginSub("ctiledd")
			.WithDesc("Clear chunks of given area radius, then generated a dungeon at the callers position")
			.RequiresPrivilege(Privilege.controlserver)
			.WithArgs(parsers.Int("chunk clear radius"), parsers.Word("tiled dungeon code"), parsers.Int("amount of tiles"), parsers.OptionalWorldPosition("pos"), parsers.OptionalWord("randomSeed"))
			.HandleWith(OnCmdClearAndTiledCungeonCode)
			.EndSub()
			.BeginSub("tileddl")
			.WithDesc("Lineup of all variations of tiles with all orientations side by side for debugging")
			.RequiresPrivilege(Privilege.controlserver)
			.WithArgs(parsers.Word("tiled dungeon code"), parsers.OptionalBool("with rotation"))
			.HandleWith(OnCmdTiledDungeonTest)
			.EndSub()
			.BeginSub("tileddd")
			.WithDesc("Toogle debug connector placing on/off for tiledd commands")
			.RequiresPrivilege(Privilege.controlserver)
			.WithArgs(parsers.OptionalBool("mode"))
			.HandleWith(OnCmdToggleDebugConnectors)
			.EndSub();
	}

	private TextCommandResult OnCmdToggleDebugConnectors(TextCommandCallingArgs args)
	{
		if (args.Parsers[0].IsMissing)
		{
			return TextCommandResult.Success("Debug connector placing is currently: " + (placeDebugConnectors ? "on" : "off"));
		}
		placeDebugConnectors = (bool)args[0];
		return TextCommandResult.Success("Debug connector placing is now: " + (placeDebugConnectors ? "on" : "off"));
	}

	private void onEvent(string eventName, ref EnumHandling handling, IAttribute data)
	{
		if (dungeonGenRequest != null)
		{
			TextCommandCallingArgs req = dungeonGenRequest;
			sapi.Event.EnqueueMainThreadTask(delegate
			{
				OnCmdTiledCungeonCode(req, 1);
			}, "gendungeon");
			dungeonGenRequest = null;
		}
	}

	private TextCommandResult OnCmdTiledDungeonTest(TextCommandCallingArgs args)
	{
		sapi.Assets.Reload(AssetCategory.worldgen);
		Init();
		string code = (string)args[0];
		TiledDungeon dungeon = Tcfg.Dungeons.FirstOrDefault((TiledDungeon td) => td.Code == code);
		if (dungeon == null)
		{
			return TextCommandResult.Error("No such dungeon defined");
		}
		int rot = ((!(bool)args[1]) ? 1 : 4);
		BlockPos pos = args.Caller.Pos.AsBlockPos;
		pos.Y = sapi.World.BlockAccessor.GetRainMapHeightAt(pos) + 5;
		IBlockAccessor ba = sapi.World.BlockAccessor;
		int orignalX = pos.X;
		int orignalZ = pos.Z;
		int offX = 0;
		Block signblock = sapi.World.GetBlock("sign-ground-north");
		if (signblock == null)
		{
			return TextCommandResult.Error("sign-ground-north block not found");
		}
		foreach (KeyValuePair<string, DungeonTile> item in dungeon.TilesByCode)
		{
			item.Deconstruct(out var key, out var value);
			string tilecode = key;
			DungeonTile dungeonTile = value;
			for (int tileIndex = 0; tileIndex < dungeonTile.ResolvedSchematics.Length; tileIndex++)
			{
				BlockSchematicPartial[] schematicByRot = dungeonTile.ResolvedSchematics[tileIndex];
				for (int i = 0; i < rot; i++)
				{
					schematicByRot[i].Place(ba, sapi.World, pos);
					schematicByRot[i].PlaceEntitiesAndBlockEntities(ba, sapi.World, pos, schematicByRot[i].BlockCodes, schematicByRot[i].ItemCodes);
					if (placeDebugConnectors)
					{
						foreach (ConnectorMetaData path in schematicByRot[i].Connectors)
						{
							BlockPos pathPosition = pos.AddCopy(path.Position).Add(path.Facing);
							ba.SetBlock(BlockSchematic.ConnectorBlockId, pathPosition);
							BETileConnector be = ba.GetBlockEntity<BETileConnector>(pathPosition);
							if (be != null)
							{
								be.Target = string.Join(",", path.Targets);
								be.Name = path.Name;
								be.Direction = path.Facing;
								be.MarkDirty();
							}
						}
					}
					pos.X += schematicByRot[i].SizeX + 3;
				}
				pos.X = orignalX;
				sapi.World.BlockAccessor.SetBlock(signblock.Id, pos.AddCopy(-3, 0, 0));
				if (sapi.World.BlockAccessor.GetBlockEntity(pos.AddCopy(-3, 0, 0)) is BlockEntitySign bes)
				{
					bes.MeshAngleRad = -(float)Math.PI / 2f;
					bes.SetText(tilecode + " / " + schematicByRot[0].FromFile);
				}
				pos.Z += schematicByRot[0].SizeZ + 3;
				if (pos.Z - orignalZ > 450)
				{
					pos.Z = orignalZ;
					offX = schematicByRot[0].SizeX * 4 + 3;
					orignalX = (pos.X = orignalX + offX);
				}
			}
			pos.Z += 3;
		}
		return TextCommandResult.Success("dungeon generated");
	}

	internal void Init()
	{
		Dictionary<AssetLocation, TiledDungeonConfig> many = sapi.Assets.GetMany<TiledDungeonConfig>(sapi.Logger, "worldgen/tileddungeons.json");
		Tcfg = new TiledDungeonConfig();
		TiledDungeonConfig defaultConf = many.First((KeyValuePair<AssetLocation, TiledDungeonConfig> v) => v.Key.Domain == "game").Value;
		Tcfg.storyStructMinDistance = defaultConf.storyStructMinDistance;
		Tcfg.minDistance = defaultConf.minDistance;
		Tcfg.RocktypeRemapGroups = new Dictionary<string, Dictionary<AssetLocation, AssetLocation>>();
		List<TiledDungeon> dungeons = new List<TiledDungeon>();
		foreach (KeyValuePair<AssetLocation, TiledDungeonConfig> item in many)
		{
			item.Deconstruct(out var key, out var value);
			TiledDungeonConfig conf = value;
			foreach (KeyValuePair<string, Dictionary<AssetLocation, AssetLocation>> remap in conf.RocktypeRemapGroups)
			{
				if (Tcfg.RocktypeRemapGroups.TryGetValue(remap.Key, out var remapGroup))
				{
					foreach (KeyValuePair<AssetLocation, AssetLocation> item2 in remap.Value)
					{
						item2.Deconstruct(out key, out var value2);
						AssetLocation source = key;
						AssetLocation target = value2;
						remapGroup.TryAdd(source, target);
					}
				}
				else
				{
					Tcfg.RocktypeRemapGroups.TryAdd(remap.Key, remap.Value);
				}
			}
			dungeons.AddRange(conf.Dungeons);
		}
		Tcfg.Dungeons = dungeons.ToArray();
		Tcfg.Init(sapi);
	}

	private TextCommandResult OnCmdClearAndTiledCungeonCode(TextCommandCallingArgs args)
	{
		sapi.ChatCommands.ExecuteUnparsed("/wgen regen " + (int)args[0], new TextCommandCallingArgs
		{
			Caller = args.Caller
		});
		dungeonGenRequest = args;
		return TextCommandResult.Success();
	}

	private TextCommandResult OnCmdGenDeungon(TextCommandCallingArgs args)
	{
		return OnCmdTiledCungeonCode(args, 0);
	}

	private TextCommandResult OnCmdGenDeungonStepped(TextCommandCallingArgs args)
	{
		debugStopStep = -1;
		lastDungeon = null;
		placeTask = null;
		debugStopStep++;
		return OnCmdTiledCungeonCode(args, 0);
	}

	private TextCommandResult OnCmdTiledCungeonCode(TextCommandCallingArgs args, int argOffset)
	{
		sapi.Assets.Reload(AssetCategory.worldgen);
		Init();
		string code = (string)args[argOffset];
		int tiles = (int)args[1 + argOffset];
		TiledDungeon dungeon = Tcfg.Dungeons.FirstOrDefault((TiledDungeon td) => td.Code == code);
		if (dungeon == null)
		{
			return TextCommandResult.Error("No such dungeon defined");
		}
		BlockPos pos;
		if (args.Parsers[2 + argOffset].IsMissing)
		{
			pos = args.Caller.Pos.AsBlockPos;
			pos.Y = sapi.World.BlockAccessor.GetTerrainMapheightAt(pos) + 5;
		}
		else
		{
			pos = ((Vec3d)args[2 + argOffset]).AsBlockPos;
		}
		long seed = sapi.WorldManager.Seed ^ 0x217F464FEL;
		if (!args.Parsers[3 + argOffset].IsMissing)
		{
			string seedString = (string)args[3 + argOffset];
			if (seedString == "true")
			{
				seed = sapi.World.Rand.NextInt64();
			}
			else if (!long.TryParse(seedString, out seed))
			{
				seed = GameMath.DotNetStringHash(seedString);
			}
		}
		sapi.Logger.Notification($"Dungeon seed is: {seed} , pos: {pos}");
		int size = sapi.WorldManager.RegionSize;
		LCGRandom rnd = new LCGRandom(seed);
		rnd.InitPositionSeed(pos.X / size * size, pos.Z / size * size);
		IBlockAccessor ba = sapi.World.BlockAccessor;
		if (TryPlaceTiledDungeon(ba, rnd, dungeon, pos, tiles, tiles, debugStopStep))
		{
			return TextCommandResult.Success("dungeon generated");
		}
		sapi.Logger.Notification($"Dungeon current seed: {rnd.currentSeed}, map: {rnd.mapGenSeed}");
		return TextCommandResult.Success($"Unable to generate dungeon with mintiles={tiles}, maxtiles={tiles}, seed={seed}");
	}

	public bool TryPlaceTiledDungeon(IBlockAccessor ba, LCGRandom rnd, TiledDungeon dungeon, BlockPos startPos, int minTiles, int maxTiles, int debugStop = -1)
	{
		lastDungeon = dungeon;
		dungeonGen.DebugLogging = debugLogging;
		placeTask = dungeonGen.TryPregenerateTiledDungeon(rnd, dungeon, new List<GeneratedStructure>(), startPos, minTiles, maxTiles);
		if (debugLogging)
		{
			sapi.Logger.Notification(string.Join("\r\n", dungeonGen.debugLogs));
			sapi.Logger.Notification("The tiles contain the following names:");
			foreach (DungeonTile val in dungeon.Tiles)
			{
				sapi.Logger.Notification(val.Code + ": " + string.Join(", ", val.CachedNames));
			}
		}
		if (placeTask != null)
		{
			return placeDungeon(ba, dungeon, debugStop, placeTask);
		}
		return false;
	}

	private bool placeDungeon(IBlockAccessor ba, TiledDungeon dungeon, int debugStop, DungeonPlaceTask dungeonPlaceTask)
	{
		for (int i = 0; i < dungeonPlaceTask.TilePlaceTasks.Count; i++)
		{
			TilePlaceTask placeTask = dungeonPlaceTask.TilePlaceTasks[i];
			if (dungeon.TilesByCode.TryGetValue(placeTask.TileCode, out DungeonTile tile))
			{
				int roomIndex = -1;
				for (int index = 0; index < tile.ResolvedSchematics.Length; index++)
				{
					if (!(tile.ResolvedSchematics[index][0].FromFile != (AssetLocation)placeTask.FileName))
					{
						roomIndex = index;
						break;
					}
				}
				if (roomIndex == -1)
				{
					sapi.Logger.Warning("Failed to generate dungeon");
					return false;
				}
				BlockSchematicPartial schem = tile.ResolvedSchematics[roomIndex][placeTask.Rotation];
				if (i == debugStop)
				{
					int blockId = sapi.World.GetBlock("creativeblock-79")?.Id ?? 1;
					ba.SetBlock(blockId, placeTask.Pos.Copy().Add(0, 0, 0));
					ba.SetBlock(blockId, placeTask.Pos.Copy().Add(schem.SizeX, 0, 0));
					ba.SetBlock(blockId, placeTask.Pos.Copy().Add(schem.SizeX, 0, schem.SizeZ));
					ba.SetBlock(blockId, placeTask.Pos.Copy().Add(0, 0, schem.SizeZ));
					ba.SetBlock(blockId, placeTask.Pos.Copy().Add(0, schem.SizeY, 0));
					ba.SetBlock(blockId, placeTask.Pos.Copy().Add(schem.SizeX, schem.SizeY, 0));
					ba.SetBlock(blockId, placeTask.Pos.Copy().Add(schem.SizeX, schem.SizeY, schem.SizeZ));
					ba.SetBlock(blockId, placeTask.Pos.Copy().Add(0, schem.SizeY, schem.SizeZ));
					return true;
				}
				schem.Place(ba, sapi.World, placeTask.Pos);
				schem.PlaceEntitiesAndBlockEntities(ba, sapi.World, placeTask.Pos, schem.BlockCodes, schem.ItemCodes);
				if (i == 0)
				{
					int blockId2 = sapi.World.GetBlock("creativeblock-79")?.Id ?? 1;
					ba.SetBlock(blockId2, placeTask.Pos.Copy().Add(schem.SizeX / 2, schem.SizeY, schem.SizeZ / 2));
				}
				if (!placeDebugConnectors)
				{
				}
			}
			else
			{
				sapi.Logger.Warning("Could not find dungeon tile: " + placeTask.TileCode + " | " + placeTask.FileName);
			}
		}
		if (placeDebugConnectors)
		{
			foreach (ConnectorMetaData posFacing in dungeonPlaceTask.OpenSet)
			{
				BlockPos blockPos = new BlockPos(posFacing.Position.X, posFacing.Position.Y, posFacing.Position.Z);
				ba.SetBlock(BlockSchematic.ConnectorBlockId, blockPos);
				BETileConnector blockEntity = ba.GetBlockEntity<BETileConnector>(blockPos);
				blockEntity.Target = string.Join(",", posFacing.Targets);
				blockEntity.Name = posFacing.Name;
				blockEntity.Direction = posFacing.Facing;
				blockEntity.MarkDirty();
			}
		}
		if (debugLogging)
		{
			sapi.Logger.Notification("Open set:\r\n" + string.Join("\r\n", dungeonPlaceTask.OpenSet.Select((ConnectorMetaData conn) => conn.Name + ":" + string.Join(",", conn.Targets))));
		}
		return true;
	}

	internal DungeonPlaceTask? TryPregenerateTiledDungeon(LCGRandom rand2, TiledDungeon dungeon, List<GeneratedStructure> generatedStructures, BlockPos blockPos, int mintiles, int maxtiles)
	{
		return dungeonGen.TryPregenerateTiledDungeon(rand2, dungeon, generatedStructures, blockPos, mintiles, maxtiles);
	}
}
