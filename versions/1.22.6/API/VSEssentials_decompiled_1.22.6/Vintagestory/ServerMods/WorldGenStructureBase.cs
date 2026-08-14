using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.Common.Collectible.Block;

namespace Vintagestory.ServerMods;

public abstract class WorldGenStructureBase
{
	private struct DungeonTileEdgeFlags
	{
		public bool X0;

		public bool Y0;

		public bool Z0;

		public bool X1;

		public bool Y1;

		public bool Z1;

		public bool X0HasBlocks;

		public bool Y0HasBlocks;

		public bool Z0HasBlocks;

		public bool X1HasBlocks;

		public bool Y1HasBlocks;

		public bool Z1HasBlocks;

		public bool MoveX0
		{
			get
			{
				if (X0)
				{
					return !X0HasBlocks;
				}
				return false;
			}
		}

		public bool MoveY0
		{
			get
			{
				if (Y0)
				{
					return !Y0HasBlocks;
				}
				return false;
			}
		}

		public bool MoveZ0
		{
			get
			{
				if (Z0)
				{
					return !Z0HasBlocks;
				}
				return false;
			}
		}

		public bool MoveX1
		{
			get
			{
				if (X1)
				{
					return !X1HasBlocks;
				}
				return false;
			}
		}

		public bool MoveY1
		{
			get
			{
				if (Y1)
				{
					return !Y1HasBlocks;
				}
				return false;
			}
		}

		public bool MoveZ1
		{
			get
			{
				if (Z1)
				{
					return !Z1HasBlocks;
				}
				return false;
			}
		}
	}

	[JsonProperty]
	public string Code;

	[JsonProperty]
	public string Name;

	[JsonProperty]
	public AssetLocation[] Schematics;

	[JsonProperty]
	public EnumStructurePlacement Placement;

	[JsonProperty]
	public NatFloat Depth;

	[JsonProperty]
	public bool BuildProtected;

	[JsonProperty]
	public string BuildProtectionDesc;

	[JsonProperty]
	public string BuildProtectionName;

	[JsonProperty]
	public bool AllowUseEveryone = true;

	[JsonProperty]
	public bool AllowTraverseEveryone = true;

	[JsonProperty]
	public int ProtectionLevel = 10;

	[JsonProperty]
	public string RockTypeRemapGroup;

	[JsonProperty]
	public Dictionary<AssetLocation, AssetLocation> RockTypeRemaps;

	[JsonProperty]
	public AssetLocation[] InsideBlockCodes;

	[JsonProperty]
	public EnumOrigin Origin;

	[JsonProperty]
	public int? OffsetY;

	[JsonProperty]
	public int MaxYDiff = 3;

	[JsonProperty]
	public int? StoryLocationMaxAmount;

	[JsonProperty]
	public int MinSpawnDistance;

	[JsonProperty]
	public int MaxBelowSealevel = 20;

	public const uint PosBitMask = 1023u;

	protected T[][] LoadSchematicsWithRotations<T>(ICoreAPI api, WorldGenStructureBase struc, BlockLayerConfig config, WorldGenStructuresConfig structureConfig, Dictionary<string, int> schematicYOffsets, string pathPrefix = "schematics/", bool isDungeon = false) where T : BlockSchematicStructure
	{
		List<T[]> schematics = new List<T[]>();
		for (int i = 0; i < struc.Schematics.Length; i++)
		{
			AssetLocation schematicLoc = Schematics[i];
			IAsset[] assets = ((!struc.Schematics[i].Path.EndsWith('*')) ? new IAsset[1] { api.Assets.Get(schematicLoc.Clone().WithPathPrefixOnce("worldgen/" + pathPrefix).WithPathAppendixOnce(".json")) } : api.Assets.GetManyInCategory("worldgen", pathPrefix + schematicLoc.Path.Substring(0, schematicLoc.Path.Length - 1), schematicLoc.Domain).ToArray());
			foreach (IAsset asset in assets)
			{
				int offsety = getOffsetY(schematicYOffsets, struc.OffsetY, asset);
				T[] sch = LoadSchematic<T>(api, asset, config, structureConfig, struc, offsety, isDungeon);
				if (sch != null)
				{
					schematics.Add(sch);
				}
			}
		}
		return schematics.ToArray();
	}

	public static int getOffsetY(Dictionary<string, int> schematicYOffsets, int? defaultOffsetY, IAsset asset)
	{
		string assloc = asset.Location.PathOmittingPrefixAndSuffix("worldgen/schematics/", ".json");
		int offsety = 0;
		if ((schematicYOffsets == null || !schematicYOffsets.TryGetValue(assloc, out offsety)) && defaultOffsetY.HasValue)
		{
			offsety = defaultOffsetY.Value;
		}
		return offsety;
	}

	public static T[] LoadSchematic<T>(ICoreAPI api, IAsset asset, BlockLayerConfig config, WorldGenStructuresConfig structureConfig, WorldGenStructureBase struc, int offsety, bool isDungeon = false) where T : BlockSchematicStructure
	{
		string cacheKey = asset.Location.ToShortString() + "~" + offsety;
		if (structureConfig != null && structureConfig.LoadedSchematicsCache.TryGetValue(cacheKey, out var cached) && cached is T[] result)
		{
			return result;
		}
		T schematic = asset.ToObject<T>();
		if (schematic == null)
		{
			api.World.Logger.Warning("Could not load schematic {0}", asset.Location);
			if (structureConfig != null)
			{
				structureConfig.LoadedSchematicsCache[cacheKey] = null;
			}
			return null;
		}
		schematic.Remap();
		if (isDungeon)
		{
			InitDungeonData(api, schematic);
		}
		schematic.OffsetY = offsety;
		schematic.FromFile = asset.Location.Clone();
		schematic.MaxYDiff = struc?.MaxYDiff ?? 3;
		schematic.MaxBelowSealevel = struc?.MaxBelowSealevel ?? 3;
		schematic.StoryLocationMaxAmount = struc?.StoryLocationMaxAmount;
		T[] rotations = new T[4] { schematic, null, null, null };
		for (int k = 0; k < 4; k++)
		{
			if (k > 0)
			{
				T unrotated = rotations[0];
				rotations[k] = unrotated.ClonePacked() as T;
				if (isDungeon)
				{
					List<ConnectorMetaData> pathways = (rotations[k].Connectors = new List<ConnectorMetaData>());
					List<ConnectorMetaData> pathwaysSource = unrotated.Connectors;
					for (int index = 0; index < pathwaysSource.Count; index++)
					{
						ConnectorMetaData path = pathwaysSource[index];
						BlockPos rotatedPos = unrotated.GetRotatedPos(EnumOrigin.BottomCenter, k * 90, path.Position.X, path.Position.Y, path.Position.Z);
						pathways.Add(new ConnectorMetaData(new FastVec3i(rotatedPos), path.Facing.GetHorizontalRotated(k * 90), k, path.Name, path.Targets, null));
					}
					rotations[k].TransformWhilePacked(api.World, EnumOrigin.BottomCenter, k * 90);
				}
			}
			rotations[k].blockLayerConfig = config;
		}
		if (structureConfig != null)
		{
			Dictionary<string, BlockSchematicStructure[]> loadedSchematicsCache = structureConfig.LoadedSchematicsCache;
			BlockSchematicStructure[] value = rotations;
			loadedSchematicsCache[cacheKey] = value;
		}
		return rotations;
	}

	private static void InitDungeonData(ICoreAPI api, BlockSchematicStructure schematic)
	{
		int connectorBlockId = schematic.BlockCodes.First((KeyValuePair<int, AssetLocation> s) => s.Value.Path.Equals("meta-connector")).Key;
		schematic.Connectors = new List<ConnectorMetaData>();
		int schematicMaxX = schematic.SizeX - 1;
		int schematicMaxZ = schematic.SizeZ - 1;
		int schematicMaxY = schematic.SizeY - 1;
		DungeonTileEdgeFlags flagSum = default(DungeonTileEdgeFlags);
		List<int> connectorBlockIndices = new List<int>();
		for (int i = 0; i < schematic.Indices.Count; i++)
		{
			if (schematic.BlockIds[i] != connectorBlockId)
			{
				uint num = schematic.Indices[i];
				int idx = (int)(num & 0x3FF);
				int idy = (int)((num >> 20) & 0x3FF);
				uint num2 = (num >> 10) & 0x3FF;
				if (idx == 0)
				{
					flagSum.X0HasBlocks = true;
				}
				if (idy == 0)
				{
					flagSum.Y0HasBlocks = true;
				}
				if (num2 == 0)
				{
					flagSum.Z0HasBlocks = true;
				}
				if (idx == schematicMaxX)
				{
					flagSum.X1HasBlocks = true;
				}
				if (idy == schematicMaxY)
				{
					flagSum.Y1HasBlocks = true;
				}
				if (num2 == (uint)schematicMaxZ)
				{
					flagSum.Z1HasBlocks = true;
				}
			}
			else
			{
				connectorBlockIndices.Add(i);
			}
		}
		for (int i2 = 0; i2 < connectorBlockIndices.Count; i2++)
		{
			uint index = schematic.Indices[connectorBlockIndices[i2]];
			int dx = (int)(index & 0x3FF);
			int dy = (int)((index >> 20) & 0x3FF);
			int dz = (int)((index >> 10) & 0x3FF);
			DungeonTileEdgeFlags flags = new DungeonTileEdgeFlags
			{
				X0 = (dx == 0),
				Z0 = (dz == 0),
				X1 = (dx == schematicMaxX),
				Z1 = (dz == schematicMaxZ)
			};
			bool edge = flags.X0 || flags.X1 || flags.Z0 || flags.Z1;
			flags.Y0 = dy == 0 && !edge;
			flags.Y1 = dy == schematicMaxY && !edge;
			flagSum.X0 |= flags.X0;
			flagSum.Y0 |= flags.Y0;
			flagSum.Z0 |= flags.Z0;
			flagSum.X1 |= flags.X1;
			flagSum.Y1 |= flags.Y1;
			flagSum.Z1 |= flags.Z1;
			TreeAttribute tree = ExtractDungeonPathConstraint(schematic, index, api.Logger);
			if (tree != null)
			{
				schematic.Connectors.Add(new ConnectorMetaData(new FastVec3i(dx, dy, dz), BlockFacing.ALLFACES[tree.GetInt("direction")], BlockFacing.NORTH.Index, tree.GetString("name"), tree.GetString("target"), null));
			}
		}
		connectorBlockIndices.Reverse();
		foreach (int i3 in connectorBlockIndices)
		{
			schematic.Indices.RemoveAt(i3);
			schematic.BlockIds.RemoveAt(i3);
		}
		if (flagSum.MoveX0)
		{
			schematic.SizeX--;
		}
		if (flagSum.MoveX1)
		{
			schematic.SizeX--;
		}
		if (flagSum.MoveZ0)
		{
			schematic.SizeZ--;
		}
		if (flagSum.MoveZ1)
		{
			schematic.SizeZ--;
		}
		if (flagSum.MoveY0)
		{
			schematic.SizeY--;
		}
		if (flagSum.MoveY1)
		{
			schematic.SizeY--;
		}
		if (flagSum.MoveX0 || flagSum.MoveY0 || flagSum.MoveZ0)
		{
			for (int i4 = 0; i4 < schematic.Indices.Count; i4++)
			{
				schematic.Indices[i4] = move(flagSum, schematic.Indices[i4]);
			}
			for (int i5 = 0; i5 < schematic.DecorIndices.Count; i5++)
			{
				schematic.DecorIndices[i5] = move(flagSum, schematic.DecorIndices[i5]);
			}
			Dictionary<uint, string> tmpBlockEntities = new Dictionary<uint, string>();
			foreach (var (index2, data) in schematic.BlockEntities)
			{
				tmpBlockEntities[move(flagSum, index2)] = data;
			}
			schematic.BlockEntities = tmpBlockEntities;
			schematic.EntitiesUnpacked.Clear();
			foreach (string entity3 in schematic.Entities)
			{
				using MemoryStream ms = new MemoryStream(Ascii85.Decode(entity3));
				BinaryReader reader = new BinaryReader(ms);
				string className = reader.ReadString();
				Entity entity = api.ClassRegistry.CreateEntity(className);
				entity.FromBytes(reader, isSync: false);
				if (flagSum.MoveX0)
				{
					entity.Pos.X--;
					entity.PositionBeforeFalling.X -= 1.0;
				}
				if (flagSum.MoveY0)
				{
					entity.Pos.Y--;
					entity.PositionBeforeFalling.Y -= 1.0;
				}
				if (flagSum.MoveZ0)
				{
					entity.Pos.Z--;
					entity.PositionBeforeFalling.Z -= 1.0;
				}
				schematic.EntitiesUnpacked.Add(entity);
			}
			schematic.Entities.Clear();
			if (schematic.EntitiesUnpacked.Count > 0)
			{
				using FastMemoryStream ms2 = new FastMemoryStream();
				foreach (Entity entity2 in schematic.EntitiesUnpacked)
				{
					ms2.Reset();
					BinaryWriter writer = new BinaryWriter(ms2);
					writer.Write(api.ClassRegistry.GetEntityClassName(entity2.GetType()));
					entity2.ToBytes(writer, forClient: false);
					schematic.Entities.Add(Ascii85.Encode(ms2.ToArray()));
				}
			}
		}
		for (int i6 = 0; i6 < schematic.Connectors.Count; i6++)
		{
			ConnectorMetaData path = schematic.Connectors[i6];
			int dx2 = 0;
			int dy2 = 0;
			int dz2 = 0;
			if (flagSum.MoveX0 && path.Position.X > 0)
			{
				dx2--;
			}
			if (flagSum.MoveY0 && path.Position.Y > 0)
			{
				dy2--;
			}
			if (flagSum.MoveZ0 && path.Position.Z > 0)
			{
				dz2--;
			}
			if (flagSum.MoveX1 && path.Position.X >= schematicMaxX)
			{
				dx2--;
			}
			if (flagSum.MoveY1 && path.Position.Y >= schematicMaxY)
			{
				dy2--;
			}
			if (flagSum.MoveZ1 && path.Position.Z >= schematicMaxZ)
			{
				dz2--;
			}
			path.Position.X += dx2;
			path.Position.Z += dz2;
			path.Position.Y += dy2;
			schematic.Connectors[i6] = path;
		}
	}

	public T[] LoadSchematics<T>(ICoreAPI api, AssetLocation[] locs, BlockLayerConfig config, string pathPrefix = "schematics/") where T : BlockSchematicStructure
	{
		List<T> schematics = new List<T>();
		for (int i = 0; i < locs.Length; i++)
		{
			string error = "";
			AssetLocation schematicLoc = Schematics[i];
			IAsset[] assets = ((!locs[i].Path.EndsWith('*')) ? new IAsset[1] { api.Assets.Get(schematicLoc.Clone().WithPathPrefixOnce("worldgen/" + pathPrefix).WithPathAppendixOnce(".json")) } : api.Assets.GetManyInCategory("worldgen", pathPrefix + schematicLoc.Path.Substring(0, schematicLoc.Path.Length - 1), schematicLoc.Domain).ToArray());
			foreach (IAsset asset in assets)
			{
				T schematic = asset.ToObject<T>();
				if (schematic == null)
				{
					api.World.Logger.Warning("Could not load {0}: {1}", Schematics[i], error);
				}
				else
				{
					schematic.FromFile = asset.Location.Clone();
					schematics.Add(schematic);
				}
			}
		}
		return schematics.ToArray();
	}

	private static uint move(DungeonTileEdgeFlags flagSum, uint index)
	{
		int dx = (int)(index & 0x3FF);
		int dy = (int)((index >> 20) & 0x3FF);
		int dz = (int)((index >> 10) & 0x3FF);
		if (flagSum.MoveX0)
		{
			dx--;
		}
		if (flagSum.MoveZ0)
		{
			dz--;
		}
		if (flagSum.MoveY0)
		{
			dy--;
		}
		return (uint)((dy << 20) | (dz << 10) | dx);
	}

	private static TreeAttribute ExtractDungeonPathConstraint(BlockSchematicStructure schematic, uint index, ILogger logger)
	{
		try
		{
			string beData = schematic.BlockEntities[index];
			TreeAttribute result = schematic.DecodeBlockEntityData(beData);
			schematic.BlockEntities.Remove(index);
			return result;
		}
		catch (Exception)
		{
			logger.Warning($"Unable to decode block entity data in schematic: {schematic.FromFile}");
		}
		return null;
	}
}
