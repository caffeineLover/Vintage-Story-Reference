using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.Common.Collectible.Block;

namespace Vintagestory.ServerMods;

public class DungeonGenerator
{
	protected ILogger logger;

	protected Vec3i mapsize;

	public bool DebugLogging;

	public List<string> debugLogs;

	public DungeonGenerator(ILogger logger, Vec3i mapsize, bool debugLogging = false)
	{
		this.logger = logger;
		this.mapsize = mapsize;
		DebugLogging = debugLogging;
	}

	public DungeonPlaceTask? TryPregenerateTiledDungeon(LCGRandom rnd, TiledDungeon dungeon, List<GeneratedStructure> existingStructures, BlockPos startPos, int minTiles, int maxTiles)
	{
		if (DebugLogging)
		{
			debugLogs = new List<string>();
		}
		int rot = rnd.NextInt(4);
		List<ConnectorMetaData> openSet = new List<ConnectorMetaData>();
		List<TilePlaceTask> placeTasks = new List<TilePlaceTask>();
		List<GeneratedStructure> gennedStructures = new List<GeneratedStructure>();
		BlockSchematicPartial schematic = dungeon.Start[rot];
		Cuboidi newloc = new Cuboidi(startPos.X, startPos.Y, startPos.Z, startPos.X + schematic.SizeX, startPos.Y + schematic.SizeY, startPos.Z + schematic.SizeZ);
		DungeonGenWorkspace dgd = new DungeonGenWorkspace(dungeon, minTiles, maxTiles, openSet, placeTasks, existingStructures, gennedStructures, null);
		addTile(dgd, default(ConnectorMetaData), dungeon.start, rot, schematic, new FastVec3i(startPos), newloc);
		if (TryGenerateTiles(rnd, dgd))
		{
			logger.Notification($"Dungeon with {placeTasks.Count} schematics generated");
			if (DebugLogging && dgd.OpenSet.Count > 0)
			{
				debugLogs.Add($"But {dgd.OpenSet.Count} sides could not be closed");
			}
			ConnectorMetaData stairsCon = openSet.FirstOrDefault((ConnectorMetaData c) => c.Name == dungeon.SurfaceConnectorName);
			if (stairsCon.Valid)
			{
				stairsCon.FacingInt = stairsCon.Facing.Index;
			}
			return new DungeonPlaceTask(dungeon.Code, placeTasks, gennedStructures, openSet, stairsCon);
		}
		return null;
	}

	private bool TryGenerateTiles(LCGRandom lcgRnd, DungeonGenWorkspace dgd)
	{
		if (DebugLogging)
		{
			debugLogs.Add("==================");
			debugLogs.Add("Attempting to generate tiles for dungeon gen " + dgd.DungeonGenerator.Code);
		}
		int tries = dgd.MinTiles * 10;
		int[] tileIndices = new int[dgd.DungeonGenerator.Tiles.Count];
		for (int i = 0; i < tileIndices.Length; i++)
		{
			tileIndices[i] = i;
		}
		ConnectorMetaData surfaceConnector = default(ConnectorMetaData);
		while (tries-- > 0 && dgd.OpenSet.Count > 0 && dgd.PlacedTiles < dgd.MaxTiles)
		{
			ConnectorMetaData openSide = dgd.OpenSet.PopFirst();
			if (openSide.Name.Equals(dgd.DungeonGenerator.SurfaceConnectorName))
			{
				surfaceConnector = openSide;
				openSide = dgd.OpenSet.PopFirst();
			}
			if (openSide.Targets.Length == 0 && openSide.TargetsForParent != null && openSide.TargetsForParent.Length != 0)
			{
				dgd.OpenSet.Add(openSide);
				continue;
			}
			if (DebugLogging)
			{
				debugLogs.Add($"Attempting to close connector {openSide}");
			}
			int pickTileTries = tileIndices.Length;
			while (pickTileTries-- > 0)
			{
				if (DebugLogging)
				{
					debugLogs.Add("Attempting to pick next tile");
				}
				DungeonTile tile = pickTile(dgd, tileIndices, lcgRnd, openSide);
				if (tile == null)
				{
					dgd.OpenSet.Add(openSide);
					break;
				}
				if (tile.TileGenerator != null)
				{
					if (TryGenSubGenerator(lcgRnd, tile, openSide, dgd) == 0)
					{
						dgd.OpenSet.Add(openSide);
						break;
					}
					dgd.PlacedTiles++;
					if (tile.GroupMaxName != null && dgd.GroupMaxCount.TryGetValue(tile.GroupMaxName, out var cur))
					{
						dgd.GroupMaxCount[tile.GroupMaxName] = cur + 1;
					}
					break;
				}
				BlockSchematicPartial schematic = null;
				int rot = 0;
				FastVec3i offsetPos = default(FastVec3i);
				tile.FreshShuffleIndices(lcgRnd);
				int len = tile.ResolvedSchematics.Length;
				for (int k = 0; k < len; k++)
				{
					BlockSchematicPartial[] schematicByRot = tile.ResolvedSchematics[tile.ShuffledIndices[k]];
					int startRot = openSide.Rotation;
					if (openSide.Facing.IsHorizontal)
					{
						startRot = lcgRnd.NextInt(4);
					}
					for (int j = 0; j < 4; j++)
					{
						rot = (startRot + j) % 4;
						ConnectorMetaData path = schematicByRot[rot].Connectors.FirstOrDefault((ConnectorMetaData p) => p.ConnectsTo(openSide));
						if (path.Valid)
						{
							offsetPos = path.Position;
							schematic = schematicByRot[rot];
							k = len;
							break;
						}
					}
				}
				if (schematic == null)
				{
					continue;
				}
				FastVec3i startPos = openSide.Position.AddCopy(openSide.Facing).Sub(offsetPos);
				Cuboidi newloc = new Cuboidi(startPos.X, startPos.Y, startPos.Z, startPos.X + schematic.SizeX, startPos.Y + schematic.SizeY, startPos.Z + schematic.SizeZ);
				if (Intersects(dgd.GeneratedStructures, newloc) || Intersects(dgd.ExistingStructures, newloc))
				{
					continue;
				}
				if (startPos.X < 0 || startPos.Y < 0 || startPos.Z < 0 || startPos.X >= mapsize.X || startPos.Y >= mapsize.Y || startPos.Z >= mapsize.Z)
				{
					logger.Warning($"Dungeon at {startPos} would reach out of bounds with tile {tile.Code} and schematic {schematic.FromFile}");
				}
				else if (!CheckIfNewPathsAreBlocked(tile, dgd, schematic, openSide, startPos))
				{
					if (tile.GroupMaxName != null && dgd.GroupMaxCount.TryGetValue(tile.GroupMaxName, out var curr))
					{
						dgd.GroupMaxCount[tile.GroupMaxName] = curr + 1;
					}
					addTile(dgd, openSide, tile.Code, rot, schematic, startPos, newloc);
					dgd.PlacedTiles++;
					break;
				}
			}
		}
		if (dgd.PlacedTiles >= dgd.MinTiles)
		{
			AddEndCaps(dgd, lcgRnd);
			if (surfaceConnector.Valid)
			{
				dgd.OpenSet.Add(surfaceConnector);
			}
			return true;
		}
		if (DebugLogging)
		{
			debugLogs.Add(string.Format("Could not finish dungeon gen " + dgd.DungeonGenerator.Code + " only {0} of {1} placeable", dgd.PlacedTiles, dgd.MinTiles));
		}
		return false;
	}

	private bool CheckIfNewPathsAreBlocked(DungeonTile tilefordebug, DungeonGenWorkspace dgd, BlockSchematicPartial schematic, ConnectorMetaData openSide, FastVec3i startPos)
	{
		if (dgd.DungeonGenerator.RequireUnblocked == null)
		{
			return false;
		}
		foreach (ConnectorMetaData con in schematic.Connectors)
		{
			if (!dgd.DungeonGenerator.RequireUnblocked.Contains(con.Name) || con.ConnectsTo(openSide, startPos) || dgd.OpenSet.Any((ConnectorMetaData o) => con.ConnectsTo(o, startPos)))
			{
				continue;
			}
			FastVec3i position = con.Position;
			FastVec3i newpos = position.Add(startPos).Add(con.Facing.Normali * 2);
			if (dgd.GeneratedStructures.Any((GeneratedStructure s) => s.Location.Contains(newpos)))
			{
				if (DebugLogging)
				{
					logger.Notification("Failed to place tile {0} at {1} because it would block potential connector {2} at {3} with other Dungeon tiles", tilefordebug.Code, startPos, con.ToString(), newpos);
				}
				return true;
			}
			if (dgd.ExistingStructures.Any((GeneratedStructure s) => s.Location.Contains(newpos)))
			{
				if (DebugLogging)
				{
					logger.Notification("Failed to place tile {0} at {1} because it would block potential connector {2} at {3} with ExistingStructures", tilefordebug.Code, startPos, con.ToString(), newpos);
				}
				return true;
			}
		}
		return false;
	}

	private void addTile(DungeonGenWorkspace dgd, ConnectorMetaData openSide, string tileCode, int rot, BlockSchematicPartial schematic, FastVec3i startPos, Cuboidi newloc)
	{
		TilePlaceTask placeTask = new TilePlaceTask
		{
			TileCode = tileCode,
			Rotation = rot,
			Pos = new BlockPos(startPos.X, startPos.Y, startPos.Z),
			FileName = schematic.FromFile,
			SizeX = schematic.SizeX,
			SizeY = schematic.SizeY,
			SizeZ = schematic.SizeZ
		};
		dgd.PlaceTasks.Add(placeTask);
		dgd.GeneratedStructures.Add(new GeneratedStructure
		{
			Code = "dungeon-" + tileCode,
			Location = newloc,
			SuppressRivulets = true
		});
		if (DebugLogging)
		{
			debugLogs.Add($"Added tile {tileCode} (total place tasks={dgd.PlaceTasks.Count})");
			debugLogs.Add(string.Join(",", dgd.PlaceTasks.Select((TilePlaceTask t) => t.FileName)));
		}
		FilterOpenSet(schematic, startPos, dgd, openSide);
		dgd.OnTileAdded(tileCode);
	}

	private void AddEndCaps(DungeonGenWorkspace dgd, LCGRandom lcgRnd)
	{
		if (dgd.DungeonGenerator.EndSchematics == null)
		{
			return;
		}
		int endLength = dgd.DungeonGenerator.EndSchematics.Length;
		List<ConnectorMetaData> list = new List<ConnectorMetaData>(dgd.OpenSet);
		int added = 0;
		foreach (ConnectorMetaData openCon in list)
		{
			int startSchematic = lcgRnd.NextInt(endLength);
			for (int k = 0; k < endLength; k++)
			{
				int curSchematicIndex = (startSchematic + k) % endLength;
				BlockSchematicPartial[] schematicByRot = dgd.DungeonGenerator.EndSchematics[curSchematicIndex];
				int startRot = openCon.Rotation;
				if (openCon.Facing.IsHorizontal)
				{
					startRot = lcgRnd.NextInt(4);
				}
				int rot = 0;
				FastVec3i offsetPos = default(FastVec3i);
				BlockSchematicPartial schematic = null;
				for (int i = 0; i < 4; i++)
				{
					rot = (startRot + i) % 4;
					ConnectorMetaData path = schematicByRot[rot].Connectors.FirstOrDefault((ConnectorMetaData p) => p.ConnectsTo(openCon));
					if (path.Valid)
					{
						offsetPos = path.Position;
						schematic = schematicByRot[rot];
						k = endLength;
						break;
					}
				}
				if (schematic != null)
				{
					FastVec3i position = openCon.Position;
					FastVec3i startPos = position.AddCopy(openCon.Facing).Sub(offsetPos);
					Cuboidi newloc = new Cuboidi(startPos.X, startPos.Y, startPos.Z, startPos.X + schematic.SizeX, startPos.Y + schematic.SizeY, startPos.Z + schematic.SizeZ);
					if (startPos.X >= 0 && startPos.Y >= 0 && startPos.Z >= 0 && startPos.X < mapsize.X && startPos.Y < mapsize.Y && startPos.Z < mapsize.Z)
					{
						addTile(dgd, openCon, dgd.DungeonGenerator.ends[curSchematicIndex], rot, schematic, startPos, newloc);
						added++;
					}
				}
			}
		}
	}

	private double GetTotalWeight(DungeonGenWorkspace dgd)
	{
		double weight = 0.0;
		for (int i = 0; i < dgd.CanGenerate.Count; i++)
		{
			DungeonTile tile = dgd.CanGenerate[i];
			if (dgd.TileQuantityByCode[tile.Code] < tile.Max)
			{
				weight += (double)tile.Chance;
			}
		}
		return weight;
	}

	public DungeonTile? pickTile(DungeonGenWorkspace dgd, int[] tileIndices, LCGRandom lcgrand, ConnectorMetaData openSide)
	{
		DungeonTile currentBestTile = null;
		tileIndices.Shuffle(lcgrand);
		float rndval = (float)lcgrand.NextDouble() * dgd.DungeonGenerator.totalChance;
		int cnt = dgd.DungeonGenerator.Tiles.Count;
		if (openSide.Targets.Length == 0)
		{
			if (DebugLogging)
			{
				debugLogs.Add($"Connector {openSide} in the open set has no targets. Skipping.");
			}
			return null;
		}
		for (int k = 0; k < cnt; k++)
		{
			DungeonTile tile = dgd.DungeonGenerator.Tiles[tileIndices[k]];
			dgd.TileQuantityByCode.TryGetValue(tile.Code, out var quantity);
			if (quantity >= tile.Max || (tile.GroupMaxName != null && dgd.DungeonGenerator.GroupMax != null && dgd.DungeonGenerator.GroupMax.TryGetValue(tile.GroupMaxName, out var max) && dgd.GroupMaxCount.TryGetValue(tile.GroupMaxName, out var cur) && cur + 1 > max))
			{
				continue;
			}
			if (!tile.CachedNames.Any((string n) => openSide.ConnectsTo(n)))
			{
				if (DebugLogging)
				{
					debugLogs.Add($"Attempt to connect {openSide}. Tile '{tile.Code}' unsuitable. It has no connector with such name");
				}
				continue;
			}
			currentBestTile = tile;
			float tileChance = tile.Chance;
			if (quantity > tile.Max)
			{
				tileChance = 0f;
			}
			rndval -= tileChance;
			if (!(rndval > 0f))
			{
				if (DebugLogging && tile != null)
				{
					debugLogs.Add($"For open connector '{openSide}' found suitable tile '{currentBestTile.Code}'");
				}
				return tile;
			}
		}
		if (DebugLogging && currentBestTile != null)
		{
			debugLogs.Add($"For open connector '{openSide}' found suitable tile '{currentBestTile.Code}'");
		}
		return currentBestTile;
	}

	private static ConnectorMetaData delegateRequireOpenedToParent(string[]? requireOpened, ConnectorMetaData connector)
	{
		if (requireOpened == null)
		{
			return connector;
		}
		string[] targets = connector.Targets;
		string[] targetsForParent = connector.TargetsForParent;
		for (int i = 0; i < requireOpened.Length; i++)
		{
			int index = targets.IndexOf<string>(requireOpened[i]);
			if (index >= 0)
			{
				targetsForParent = ((targetsForParent != null) ? targetsForParent.Append<string>(requireOpened[i]) : new string[1] { requireOpened[i] });
				targets = targets.RemoveAt(index);
			}
		}
		return new ConnectorMetaData(connector.Position, connector.Facing, connector.Rotation, connector.Name, targets, targetsForParent);
	}

	private int TryGenSubGenerator(LCGRandom rnd, DungeonTile dungeonTile, ConnectorMetaData openside, DungeonGenWorkspace dgd)
	{
		if (DebugLogging)
		{
			debugLogs.Add("=============================");
			debugLogs.Add("Enter sub dungeon generator for tile '" + dungeonTile.Code + "'");
		}
		if (dungeonTile.TileGenerator.MinTiles == 0)
		{
			logger.Warning("Child generator '{0}' has mintiles set to 0, will not generate anything", dungeonTile.TileGenerator.Code);
		}
		DungeonGenWorkspace childDgd = dgd.SpawnChild(dungeonTile);
		int tries = 20;
		while (tries-- > 0)
		{
			if (DebugLogging)
			{
				debugLogs.Add("Sub dungeon generator for tile '" + dungeonTile.Code + "', attempt nr. " + tries + "/20");
			}
			childDgd.Reset(openside, dgd.GeneratedStructures);
			if (!TryGenerateTiles(rnd, childDgd))
			{
				continue;
			}
			if (dungeonTile.TileGenerator.RequireClosed != null && childDgd.OpenSet.Any((ConnectorMetaData s) => dungeonTile.TileGenerator.RequireClosed.Any((string n) => (s.Name.Length > 0 && s.Name.Equals(n)) || (s.Targets.Length != 0 && s.Targets.Contains(n)))))
			{
				if (DebugLogging)
				{
					IEnumerable<ConnectorMetaData> conn = childDgd.OpenSet.Where((ConnectorMetaData s) => dungeonTile.TileGenerator.RequireClosed.Any((string n) => (s.Name.Length > 0 && s.Name.Equals(n)) || (s.Targets.Length != 0 && s.Targets.Contains(n))));
					debugLogs.Add(string.Format("Sub dungeon generator for tile " + dungeonTile.Code + ", cannot complete. 'RequireClosed' Connector {0} was not closable", string.Join(",", conn)));
				}
				rnd.NextDouble();
				continue;
			}
			childDgd.CommitToParent(DebugLogging, debugLogs);
			return childDgd.PlaceTasks.Count - 1;
		}
		if (DebugLogging)
		{
			debugLogs.Add("Unable to create sub dungeon. Giving up.");
		}
		return 0;
	}

	public bool Intersects(List<GeneratedStructure> gennedStructures, Cuboidi newloc)
	{
		for (int i = 0; i < gennedStructures.Count; i++)
		{
			if (gennedStructures[i].Location.Intersects(newloc))
			{
				return true;
			}
		}
		return false;
	}

	public void FilterOpenSet(BlockSchematicPartial schematic, FastVec3i startPos, DungeonGenWorkspace dgd, ConnectorMetaData attachingCon)
	{
		List<ConnectorMetaData> newOpenSet = dgd.OpenSet.ToList();
		List<ConnectorMetaData> toAdd = new List<ConnectorMetaData>();
		foreach (ConnectorMetaData selfCon in schematic.Connectors)
		{
			if (selfCon.ConnectsTo(attachingCon, startPos))
			{
				continue;
			}
			bool found = false;
			for (int i = 0; i < newOpenSet.Count; i++)
			{
				ConnectorMetaData openCon = newOpenSet[i];
				if (selfCon.ConnectsTo(openCon, startPos))
				{
					newOpenSet.RemoveAt(i);
					found = true;
					break;
				}
			}
			if (!found)
			{
				ConnectorMetaData connector = selfCon.Clone().Offset(startPos);
				connector = delegateRequireOpenedToParent(dgd.DungeonGenerator.RequireOpened, connector);
				if (DebugLogging)
				{
					connector.FromSchematicForDebug = schematic.FromFile;
				}
				toAdd.Add(connector);
			}
		}
		dgd.OpenSet.Clear();
		dgd.OpenSet.AddRange(newOpenSet);
		dgd.OpenSet.AddRange(toAdd);
		if (DebugLogging && toAdd.Count > 0)
		{
			debugLogs.Add(string.Format("Added connectors {0} to open set", string.Join(",", toAdd.Select((ConnectorMetaData conn) => conn.ToString()))));
		}
	}
}
