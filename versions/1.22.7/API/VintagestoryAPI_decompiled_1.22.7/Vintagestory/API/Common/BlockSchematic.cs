using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ProperVersion;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Common.Collectible.Block;

namespace Vintagestory.API.Common;

[JsonObject(MemberSerialization.OptIn)]
public class BlockSchematic
{
	[JsonProperty]
	public string GameVersion;

	[JsonProperty]
	public int SizeX;

	[JsonProperty]
	public int SizeY;

	[JsonProperty]
	public int SizeZ;

	[JsonProperty]
	public Dictionary<int, AssetLocation> BlockCodes = new Dictionary<int, AssetLocation>();

	[JsonProperty]
	public Dictionary<int, AssetLocation> ItemCodes = new Dictionary<int, AssetLocation>();

	/// <summary>
	/// X/Y/Z position. First 10 bits is X, next 10 is Z and next 10 bits is Y
	/// </summary>
	[JsonProperty]
	public List<uint> Indices = new List<uint>();

	[JsonProperty]
	public List<int> BlockIds = new List<int>();

	[JsonProperty]
	public List<uint> DecorIndices = new List<uint>();

	[JsonProperty]
	public List<long> DecorIds = new List<long>();

	[JsonProperty]
	public Dictionary<uint, string> BlockEntities = new Dictionary<uint, string>();

	[JsonProperty]
	public List<string> Entities = new List<string>();

	[JsonProperty]
	public EnumReplaceMode ReplaceMode = EnumReplaceMode.ReplaceAllNoAir;

	[JsonProperty]
	public int EntranceRotation = -1;

	[JsonProperty]
	public BlockPos OriginalPos;

	public Dictionary<BlockPos, int> BlocksUnpacked = new Dictionary<BlockPos, int>();

	public Dictionary<BlockPos, int> FluidsLayerUnpacked = new Dictionary<BlockPos, int>();

	public Dictionary<BlockPos, string> BlockEntitiesUnpacked = new Dictionary<BlockPos, string>();

	public List<Entity> EntitiesUnpacked = new List<Entity>();

	public Dictionary<BlockPos, Dictionary<int, Block>> DecorsUnpacked = new Dictionary<BlockPos, Dictionary<int, Block>>();

	public FastVec3i PackedOffset;

	public List<ConnectorMetaData> Connectors;

	public static int ConnectorBlockId;

	public static int FillerBlockId;

	public static int PathwayBlockId;

	public static int UndergroundBlockId;

	public static int AbovegroundBlockId;

	protected ushort empty;

	public bool OmitLiquids;

	public BlockFacing[] PathwaySides;

	/// <summary>
	/// Distance positions from bottom left corner of the schematic. Only the first door block.
	/// </summary>
	public BlockPos[] PathwayStarts;

	/// <summary>
	/// Distance from the bottom left door block, so the bottom left door block is always at 0,0,0
	/// </summary>
	public BlockPos[][] PathwayOffsets;

	public BlockPos[] UndergroundCheckPositions;

	public BlockPos[] AbovegroundCheckPositions;

	private static BlockPos Zero = new BlockPos(0, 0, 0);

	/// <summary>
	/// This bitmask for the position in schematics
	/// </summary>
	public const uint PosBitMask = 1023u;

	private BlockPos curPos = new BlockPos(0);

	/// <summary>
	/// Set by the RemapperAssistant in OnFinalizeAssets
	/// <br />Heads up!:  This is unordered, it will iterate through the different game versions' remaps not necessarily in the order they originally appear in remaps.json config.  If any block remaps over the years have duplicate original block names, behavior for those ones may be unpredictable
	/// </summary>
	public static IDictionary<string, Dictionary<string, string>> BlockRemaps { get; set; }

	/// <summary>
	/// Set by the RemapperAssistant in OnFinalizeAssets
	/// </summary>
	public static IDictionary<string, Dictionary<string, string>> ItemRemaps { get; set; }

	public BlockSchematic()
	{
		GameVersion = "1.22.7";
	}

	/// <summary>
	/// Construct a schematic from a specified area in the specified world
	/// </summary>
	/// <param name="world"></param>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <param name="notLiquids"></param>
	public BlockSchematic(IServerWorldAccessor world, BlockPos start, BlockPos end, bool notLiquids)
		: this(world, world.BlockAccessor, start, end, notLiquids)
	{
	}

	public BlockSchematic(IServerWorldAccessor world, IBlockAccessor blockAccess, BlockPos start, BlockPos end, bool notLiquids)
	{
		BlockPos startPos = new BlockPos(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y), Math.Min(start.Z, end.Z));
		OmitLiquids = notLiquids;
		AddArea(world, blockAccess, start, end);
		Pack(world, startPos);
	}

	public virtual void Init(IBlockAccessor blockAccessor)
	{
		Remap();
	}

	public bool TryGetVersionFromRemapKey(string remapKey, out SemVer remapVersion)
	{
		string[] remapString = remapKey.Split(":");
		if (remapKey.Length < 2)
		{
			remapVersion = null;
			return false;
		}
		if (remapString[1].StartsWithFast("v"))
		{
			remapString[1] = remapString[1].Substring(1, remapString[1].Length - 1);
		}
		SemVer.TryParse(remapString[1], out remapVersion);
		return true;
	}

	public void Remap()
	{
		SemVer.TryParse(GameVersion ?? "0.0.0", out var schematicVersion);
		foreach (KeyValuePair<string, Dictionary<string, string>> map in BlockRemaps)
		{
			if (TryGetVersionFromRemapKey(map.Key, out var remapVersion) && remapVersion <= schematicVersion)
			{
				continue;
			}
			foreach (KeyValuePair<int, AssetLocation> blockCode in BlockCodes)
			{
				if (map.Value.TryGetValue(blockCode.Value.Path, out var newBlockCode))
				{
					BlockCodes[blockCode.Key] = new AssetLocation(newBlockCode);
				}
			}
		}
		foreach (KeyValuePair<string, Dictionary<string, string>> map2 in ItemRemaps)
		{
			if (TryGetVersionFromRemapKey(map2.Key, out var remapVersion2) && remapVersion2 <= schematicVersion)
			{
				continue;
			}
			foreach (KeyValuePair<int, AssetLocation> itemCode in ItemCodes)
			{
				if (map2.Value.TryGetValue(itemCode.Value.Path, out var newItemCode))
				{
					ItemCodes[itemCode.Key] = new AssetLocation(newItemCode);
				}
			}
		}
	}

	/// <summary>
	/// Loads the meta information for each block in the schematic.
	/// </summary>
	/// <param name="blockAccessor"></param>
	/// <param name="worldForResolve"></param>
	/// <param name="fileNameForLogging"></param>
	public void LoadMetaInformationAndValidate(IBlockAccessor blockAccessor, IWorldAccessor worldForResolve, string fileNameForLogging)
	{
		List<BlockPos> undergroundPositions = new List<BlockPos>();
		List<BlockPos> abovegroundPositions = new List<BlockPos>();
		Queue<BlockPos> pathwayPositions = new Queue<BlockPos>();
		HashSet<AssetLocation> missingBlocks = new HashSet<AssetLocation>();
		for (int i = 0; i < Indices.Count; i++)
		{
			uint num = Indices[i];
			int storedBlockid = BlockIds[i];
			int dx = (int)(num & 0x3FF);
			int dy = (int)((num >> 20) & 0x3FF);
			int dz = (int)((num >> 10) & 0x3FF);
			AssetLocation blockCode = BlockCodes[storedBlockid];
			Block newBlock = blockAccessor.GetBlock(blockCode);
			if (newBlock == null)
			{
				missingBlocks.Add(blockCode);
				continue;
			}
			BlockPos pos = new BlockPos(dx, dy, dz);
			if (newBlock.Id == PathwayBlockId)
			{
				pathwayPositions.Enqueue(pos);
			}
			else if (newBlock.Id == UndergroundBlockId)
			{
				undergroundPositions.Add(pos);
			}
			else if (newBlock.Id == AbovegroundBlockId)
			{
				abovegroundPositions.Add(pos);
			}
		}
		for (int j = 0; j < DecorIds.Count; j++)
		{
			int storedBlockid2 = (int)DecorIds[j] & 0xFFFFFF;
			AssetLocation blockCode2 = BlockCodes[storedBlockid2];
			if (blockAccessor.GetBlock(blockCode2) == null)
			{
				missingBlocks.Add(blockCode2);
			}
		}
		if (missingBlocks.Count > 0)
		{
			worldForResolve.Logger.Warning("Block schematic file {0} uses blocks that could no longer be found. These will turn into air blocks! (affected: {1})", fileNameForLogging, string.Join(",", missingBlocks));
		}
		HashSet<AssetLocation> missingItems = new HashSet<AssetLocation>();
		foreach (KeyValuePair<int, AssetLocation> val in ItemCodes)
		{
			if (worldForResolve.GetItem(val.Value) == null)
			{
				missingItems.Add(val.Value);
			}
		}
		if (missingItems.Count > 0)
		{
			worldForResolve.Logger.Warning("Block schematic file {0} uses items that could no longer be found. These will turn into unknown items! (affected: {1})", fileNameForLogging, string.Join(",", missingItems));
		}
		UndergroundCheckPositions = undergroundPositions.ToArray();
		AbovegroundCheckPositions = abovegroundPositions.ToArray();
		List<List<BlockPos>> pathwayslist = new List<List<BlockPos>>();
		if (pathwayPositions.Count == 0)
		{
			PathwayStarts = Array.Empty<BlockPos>();
			PathwayOffsets = Array.Empty<BlockPos[]>();
			PathwaySides = Array.Empty<BlockFacing>();
			return;
		}
		while (pathwayPositions.Count > 0)
		{
			List<BlockPos> pathway = new List<BlockPos> { pathwayPositions.Dequeue() };
			pathwayslist.Add(pathway);
			int i2 = pathwayPositions.Count;
			while (i2-- > 0)
			{
				BlockPos pos2 = pathwayPositions.Dequeue();
				bool found = false;
				for (int k = 0; k < pathway.Count; k++)
				{
					BlockPos ppos = pathway[k];
					if (Math.Abs(pos2.X - ppos.X) + Math.Abs(pos2.Y - ppos.Y) + Math.Abs(pos2.Z - ppos.Z) == 1)
					{
						found = true;
						pathway.Add(pos2);
						break;
					}
				}
				if (!found)
				{
					pathwayPositions.Enqueue(pos2);
				}
				else
				{
					i2 = pathwayPositions.Count;
				}
			}
		}
		PathwayStarts = new BlockPos[pathwayslist.Count];
		PathwayOffsets = new BlockPos[pathwayslist.Count][];
		PathwaySides = new BlockFacing[pathwayslist.Count];
		Vec3f dirToMiddle = new Vec3f();
		for (int l = 0; l < PathwayStarts.Length; l++)
		{
			dirToMiddle.Set(0f, 0f, 0f);
			List<BlockPos> pathway2 = pathwayslist[l];
			for (int m = 0; m < pathway2.Count; m++)
			{
				BlockPos pos3 = pathway2[m];
				dirToMiddle.X += (float)pos3.X - (float)SizeX / 2f;
				dirToMiddle.Y += (float)pos3.Y - (float)SizeY / 2f;
				dirToMiddle.Z += (float)pos3.Z - (float)SizeZ / 2f;
			}
			dirToMiddle.Normalize();
			PathwaySides[l] = BlockFacing.FromNormal(dirToMiddle);
			BlockPos start = (PathwayStarts[l] = pathwayslist[l][0].Copy());
			PathwayOffsets[l] = new BlockPos[pathwayslist[l].Count];
			for (int n = 0; n < pathwayslist[l].Count; n++)
			{
				PathwayOffsets[l][n] = pathwayslist[l][n].Sub(start);
			}
		}
	}

	/// <summary>
	/// Adds an area to the schematic.
	/// </summary>
	/// <param name="world">The world the blocks are in</param>
	/// <param name="start">The start position of all the blocks.</param>
	/// <param name="end">The end position of all the blocks.</param>
	public virtual void AddArea(IWorldAccessor world, BlockPos start, BlockPos end)
	{
		AddArea(world, world.BlockAccessor, start, end);
	}

	public virtual void AddArea(IWorldAccessor world, IBlockAccessor blockAccess, BlockPos start, BlockPos end)
	{
		BlockPos startPos = new BlockPos(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y), Math.Min(start.Z, end.Z), start.dimension);
		BlockPos finalPos = new BlockPos(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y), Math.Max(start.Z, end.Z), start.dimension);
		OriginalPos = start;
		BlockPos readPos = new BlockPos(start.dimension);
		using FastMemoryStream reusableMemoryStream = new FastMemoryStream();
		for (int x = startPos.X; x < finalPos.X; x++)
		{
			for (int y = startPos.Y; y < finalPos.Y; y++)
			{
				for (int z = startPos.Z; z < finalPos.Z; z++)
				{
					readPos.Set(x, y, z);
					int blockid = blockAccess.GetBlock(readPos, 1).BlockId;
					int fluidid = blockAccess.GetBlock(readPos, 2).BlockId;
					if (fluidid == blockid)
					{
						blockid = 0;
					}
					if (OmitLiquids)
					{
						fluidid = 0;
					}
					if (blockid == 0 && fluidid == 0)
					{
						continue;
					}
					BlockPos keyPos = new BlockPos(x, y, z);
					BlocksUnpacked[keyPos] = blockid;
					FluidsLayerUnpacked[keyPos] = fluidid;
					BlockEntity be = blockAccess.GetBlockEntity(readPos);
					if (be != null)
					{
						if (be.Api == null)
						{
							be.Initialize(world.Api);
						}
						BlockEntitiesUnpacked[keyPos] = EncodeBlockEntityData(be, reusableMemoryStream);
						be.OnStoreCollectibleMappings(BlockCodes, ItemCodes);
					}
					Dictionary<int, Block> decors = blockAccess.GetSubDecors(readPos);
					if (decors != null)
					{
						DecorsUnpacked[keyPos] = decors;
					}
				}
			}
		}
		EntitiesUnpacked.AddRange(world.GetEntitiesInsideCuboid(start, end, (Entity e) => !(e is EntityPlayer)));
		foreach (Entity item in EntitiesUnpacked)
		{
			item.OnStoreCollectibleMappings(BlockCodes, ItemCodes);
		}
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="world"></param>
	/// <param name="startPos"></param>
	/// <param name="skipImportExportForEntities">when true WillExport and DidImportOrExport will not be called</param>
	/// <returns></returns>
	public virtual bool Pack(IWorldAccessor world, BlockPos startPos, bool skipImportExportForEntities = false)
	{
		Indices.Clear();
		BlockIds.Clear();
		BlockEntities.Clear();
		Entities.Clear();
		DecorIndices.Clear();
		DecorIds.Clear();
		SizeX = 0;
		SizeY = 0;
		SizeZ = 0;
		int minX = int.MaxValue;
		int minY = int.MaxValue;
		int minZ = int.MaxValue;
		foreach (KeyValuePair<BlockPos, int> val in BlocksUnpacked)
		{
			minX = Math.Min(minX, val.Key.X);
			minY = Math.Min(minY, val.Key.Y);
			minZ = Math.Min(minZ, val.Key.Z);
			int num = val.Key.X - startPos.X;
			int dy = val.Key.Y - startPos.Y;
			int dz = val.Key.Z - startPos.Z;
			if (num >= 1024 || dy >= 1024 || dz >= 1024)
			{
				world.Logger.Warning("Export format does not support areas larger than 1024 blocks in any direction. Will not pack.");
				PackedOffset = new FastVec3i(0, 0, 0);
				return false;
			}
		}
		foreach (KeyValuePair<BlockPos, int> val2 in BlocksUnpacked)
		{
			if (!FluidsLayerUnpacked.TryGetValue(val2.Key, out var fluidid))
			{
				fluidid = 0;
			}
			int blockid = val2.Value;
			if (blockid != 0 || fluidid != 0)
			{
				if (blockid != 0)
				{
					BlockCodes[blockid] = world.BlockAccessor.GetBlock(blockid).Code;
				}
				if (fluidid != 0)
				{
					BlockCodes[fluidid] = world.BlockAccessor.GetBlock(fluidid).Code;
				}
				int dx = val2.Key.X - minX;
				int dy2 = val2.Key.Y - minY;
				int dz2 = val2.Key.Z - minZ;
				SizeX = Math.Max(dx, SizeX);
				SizeY = Math.Max(dy2, SizeY);
				SizeZ = Math.Max(dz2, SizeZ);
				Indices.Add((uint)((dy2 << 20) | (dz2 << 10) | dx));
				if (fluidid == 0)
				{
					BlockIds.Add(blockid);
					continue;
				}
				if (blockid == 0)
				{
					BlockIds.Add(fluidid);
					continue;
				}
				BlockIds.Add(blockid);
				Indices.Add((uint)((dy2 << 20) | (dz2 << 10) | dx));
				BlockIds.Add(fluidid);
			}
		}
		BlockPos key;
		int key2;
		foreach (KeyValuePair<BlockPos, int> item in FluidsLayerUnpacked)
		{
			item.Deconstruct(out key, out key2);
			BlockPos pos = key;
			int blockId = key2;
			if (!BlocksUnpacked.ContainsKey(pos))
			{
				if (blockId != 0)
				{
					BlockCodes[blockId] = world.BlockAccessor.GetBlock(blockId).Code;
				}
				int dx2 = pos.X - minX;
				int dy3 = pos.Y - minY;
				int dz3 = pos.Z - minZ;
				SizeX = Math.Max(dx2, SizeX);
				SizeY = Math.Max(dy3, SizeY);
				SizeZ = Math.Max(dz3, SizeZ);
				Indices.Add((uint)((dy3 << 20) | (dz3 << 10) | dx2));
				BlockIds.Add(blockId);
			}
		}
		foreach (KeyValuePair<BlockPos, Dictionary<int, Block>> item2 in DecorsUnpacked)
		{
			item2.Deconstruct(out key, out var value);
			BlockPos blockPos = key;
			Dictionary<int, Block> decors = value;
			int dx3 = blockPos.X - minX;
			int dy4 = blockPos.Y - minY;
			int dz4 = blockPos.Z - minZ;
			SizeX = Math.Max(dx3, SizeX);
			SizeY = Math.Max(dy4, SizeY);
			SizeZ = Math.Max(dz4, SizeZ);
			foreach (KeyValuePair<int, Block> item3 in decors)
			{
				item3.Deconstruct(out key2, out var value2);
				int faceAndSubposition = key2;
				Block decorBlock = value2;
				BlockCodes[decorBlock.BlockId] = decorBlock.Code;
				DecorIndices.Add((uint)((dy4 << 20) | (dz4 << 10) | dx3));
				DecorIds.Add(((long)faceAndSubposition << 24) + decorBlock.BlockId);
			}
		}
		SizeX++;
		SizeY++;
		SizeZ++;
		foreach (KeyValuePair<BlockPos, string> val3 in BlockEntitiesUnpacked)
		{
			int dx4 = val3.Key.X - minX;
			int dy5 = val3.Key.Y - minY;
			int dz5 = val3.Key.Z - minZ;
			BlockEntities[(uint)((dy5 << 20) | (dz5 << 10) | dx4)] = val3.Value;
		}
		BlockPos minPos = new BlockPos(minX, minY, minZ, startPos.dimension);
		using (FastMemoryStream ms = new FastMemoryStream())
		{
			foreach (Entity e in EntitiesUnpacked)
			{
				ms.Reset();
				BinaryWriter writer = new BinaryWriter(ms);
				writer.Write(world.ClassRegistry.GetEntityClassName(e.GetType()));
				if (!skipImportExportForEntities)
				{
					e.WillExport(minPos);
				}
				e.ToBytes(writer, forClient: false);
				if (!skipImportExportForEntities)
				{
					e.DidImportOrExport(minPos);
				}
				Entities.Add(Ascii85.Encode(ms.ToArray()));
			}
		}
		if (Connectors != null)
		{
			for (int i = 0; i < Connectors.Count; i++)
			{
				ConnectorMetaData path = Connectors[i];
				path.Position.X -= minX;
				path.Position.Y -= minY;
				path.Position.Z -= minZ;
				Connectors[i] = path;
			}
		}
		PackedOffset = new FastVec3i(minX - startPos.X, minY - startPos.Y, minZ - startPos.Z);
		BlocksUnpacked.Clear();
		FluidsLayerUnpacked.Clear();
		DecorsUnpacked.Clear();
		BlockEntitiesUnpacked.Clear();
		EntitiesUnpacked.Clear();
		return true;
	}

	/// <summary>
	/// Will place all blocks using the configured replace mode. Note: If you use a revertable or bulk block accessor you will have to call PlaceBlockEntities() after the Commit()
	/// </summary>
	/// <param name="blockAccessor"></param>
	/// <param name="worldForCollectibleResolve"></param>
	/// <param name="startPos"></param>
	/// <param name="replaceMetaBlocks"></param>
	/// <returns></returns>
	public virtual int Place(IBlockAccessor blockAccessor, IWorldAccessor worldForCollectibleResolve, BlockPos startPos, bool replaceMetaBlocks = true)
	{
		int result = Place(blockAccessor, worldForCollectibleResolve, startPos, ReplaceMode, replaceMetaBlocks);
		PlaceDecors(blockAccessor, startPos);
		return result;
	}

	/// <summary>
	/// Will place all blocks using the supplied replace mode. Note: If you use a revertable or bulk block accessor you will have to call PlaceBlockEntities() after the Commit()
	/// </summary>
	/// <param name="blockAccessor"></param>
	/// <param name="worldForCollectibleResolve"></param>
	/// <param name="startPos"></param>
	/// <param name="mode"></param>
	/// <param name="replaceMetaBlocks"></param>
	/// <returns></returns>
	public virtual int Place(IBlockAccessor blockAccessor, IWorldAccessor worldForCollectibleResolve, BlockPos startPos, EnumReplaceMode mode, bool replaceMetaBlocks = true)
	{
		BlockPos curPos = new BlockPos(startPos.dimension);
		int placed = 0;
		PlaceBlockDelegate handler = null;
		switch (mode)
		{
		case EnumReplaceMode.ReplaceAll:
		{
			handler = PlaceReplaceAll;
			for (int dx = 0; dx < SizeX; dx++)
			{
				for (int dy = 0; dy < SizeY; dy++)
				{
					for (int dz = 0; dz < SizeZ; dz++)
					{
						curPos.Set(dx + startPos.X, dy + startPos.Y, dz + startPos.Z);
						if (blockAccessor.IsValidPos(curPos))
						{
							blockAccessor.SetBlock(0, curPos);
						}
					}
				}
			}
			break;
		}
		case EnumReplaceMode.Replaceable:
			handler = PlaceReplaceable;
			break;
		case EnumReplaceMode.ReplaceAllNoAir:
			handler = PlaceReplaceAllNoAir;
			break;
		case EnumReplaceMode.ReplaceOnlyAir:
			handler = PlaceReplaceOnlyAir;
			break;
		}
		for (int i = 0; i < Indices.Count; i++)
		{
			uint num = Indices[i];
			int storedBlockid = BlockIds[i];
			int dx2 = (int)(num & 0x3FF);
			int dy2 = (int)((num >> 20) & 0x3FF);
			int dz2 = (int)((num >> 10) & 0x3FF);
			AssetLocation blockCode = BlockCodes[storedBlockid];
			Block newBlock = blockAccessor.GetBlock(blockCode);
			if (newBlock == null || (replaceMetaBlocks && (newBlock.Id == UndergroundBlockId || newBlock.Id == AbovegroundBlockId)))
			{
				continue;
			}
			curPos.Set(dx2 + startPos.X, dy2 + startPos.Y, dz2 + startPos.Z);
			if (blockAccessor.IsValidPos(curPos))
			{
				placed += handler(blockAccessor, curPos, newBlock, replaceMetaBlocks);
				if (newBlock.LightHsv[2] > 0 && blockAccessor is IWorldGenBlockAccessor)
				{
					Block oldBlock = blockAccessor.GetBlock(curPos);
					((IWorldGenBlockAccessor)blockAccessor).ScheduleBlockLightUpdate(curPos, oldBlock.BlockId, newBlock.BlockId);
				}
			}
		}
		if (!(blockAccessor is IBlockAccessorRevertable))
		{
			PlaceEntitiesAndBlockEntities(blockAccessor, worldForCollectibleResolve, startPos, BlockCodes, ItemCodes, replaceBlockEntities: false, null, 0, null, replaceMetaBlocks);
		}
		return placed;
	}

	public virtual void PlaceDecors(IBlockAccessor blockAccessor, BlockPos startPos)
	{
		curPos.SetDimension(startPos.dimension);
		for (int i = 0; i < DecorIndices.Count; i++)
		{
			uint index = DecorIndices[i];
			int posX = startPos.X + (int)(index & 0x3FF);
			int posY = startPos.Y + (int)((index >> 20) & 0x3FF);
			int posZ = startPos.Z + (int)((index >> 10) & 0x3FF);
			long storedDecorId = DecorIds[i];
			PlaceOneDecor(blockAccessor, posX, posY, posZ, storedDecorId);
		}
	}

	public virtual void PlaceDecors(IBlockAccessor blockAccessor, BlockPos startPos, Rectanglei rect)
	{
		int i = -1;
		foreach (uint index in DecorIndices)
		{
			i++;
			int posX = startPos.X + (int)(index & 0x3FF);
			int posZ = startPos.Z + (int)((index >> 10) & 0x3FF);
			if (rect.Contains(posX, posZ))
			{
				int posY = startPos.Y + (int)((index >> 20) & 0x3FF);
				long storedDecorId = DecorIds[i];
				PlaceOneDecor(blockAccessor, posX, posY, posZ, storedDecorId);
			}
		}
	}

	private void PlaceOneDecor(IBlockAccessor blockAccessor, int posX, int posY, int posZ, long storedBlockIdAndDecoPos)
	{
		int faceAndSubPosition = (int)(storedBlockIdAndDecoPos >> 24);
		storedBlockIdAndDecoPos &= 0xFFFFFF;
		AssetLocation blockCode = BlockCodes[(int)storedBlockIdAndDecoPos];
		Block newBlock = blockAccessor.GetBlock(blockCode);
		if (newBlock != null)
		{
			curPos.Set(posX, posY, posZ);
			if (blockAccessor.IsValidPos(curPos))
			{
				blockAccessor.SetDecor(newBlock, curPos, faceAndSubPosition);
			}
		}
	}

	/// <summary>
	/// Attempts to transform each block as they are placed in directions different from the schematic.
	/// </summary>
	/// <param name="worldForResolve"></param>
	/// <param name="aroundOrigin"></param>
	/// <param name="angle"></param>
	/// <param name="flipAxis"></param>
	public virtual void TransformWhilePacked(IWorldAccessor worldForResolve, EnumOrigin aroundOrigin, int angle, EnumAxis? flipAxis = null)
	{
		BlocksUnpacked.Clear();
		FluidsLayerUnpacked.Clear();
		BlockEntitiesUnpacked.Clear();
		DecorsUnpacked.Clear();
		EntitiesUnpacked.Clear();
		angle = GameMath.Mod(angle, 360);
		if (angle == 0)
		{
			return;
		}
		if (EntranceRotation != -1)
		{
			EntranceRotation = GameMath.Mod(EntranceRotation + angle, 360);
		}
		for (int i = 0; i < Indices.Count; i++)
		{
			uint index = Indices[i];
			int storedBlockid = BlockIds[i];
			int dx = (int)(index & 0x3FF);
			int dy = (int)((index >> 20) & 0x3FF);
			int dz = (int)((index >> 10) & 0x3FF);
			AssetLocation blockCode = BlockCodes[storedBlockid];
			Block newBlock = worldForResolve.GetBlock(blockCode);
			if (newBlock == null)
			{
				BlockEntities.Remove(index);
				continue;
			}
			if (flipAxis.HasValue)
			{
				if (flipAxis == EnumAxis.Y)
				{
					dy = SizeY - dy;
					AssetLocation newCode = newBlock.GetVerticallyFlippedBlockCode();
					newBlock = worldForResolve.GetBlock(newCode);
				}
				if (flipAxis == EnumAxis.X)
				{
					dx = SizeX - dx;
					AssetLocation newCode2 = newBlock.GetHorizontallyFlippedBlockCode(flipAxis.Value);
					newBlock = worldForResolve.GetBlock(newCode2);
				}
				if (flipAxis == EnumAxis.Z)
				{
					dz = SizeZ - dz;
					AssetLocation newCode3 = newBlock.GetHorizontallyFlippedBlockCode(flipAxis.Value);
					newBlock = worldForResolve.GetBlock(newCode3);
				}
			}
			if (angle != 0)
			{
				AssetLocation newCode4 = newBlock.GetRotatedBlockCode(angle);
				Block rotBlock = worldForResolve.GetBlock(newCode4);
				if (rotBlock != null)
				{
					newBlock = rotBlock;
				}
				else
				{
					worldForResolve.Logger.Warning("Schematic rotate: Unable to rotate block {0} - its GetRotatedBlockCode() method call returns an invalid block code: {1}! Will use unrotated variant.", blockCode, newCode4);
				}
			}
			BlockPos pos = GetRotatedPos(aroundOrigin, angle, dx, dy, dz);
			if (newBlock.ForFluidsLayer)
			{
				FluidsLayerUnpacked[pos] = newBlock.BlockId;
			}
			else
			{
				BlocksUnpacked[pos] = newBlock.BlockId;
			}
		}
		for (int j = 0; j < DecorIndices.Count; j++)
		{
			uint num = DecorIndices[j];
			long num2 = DecorIds[j];
			int faceAndSubPosition = (int)(num2 >> 24);
			int faceIndex = faceAndSubPosition % 6;
			int blockId = (int)(num2 & 0xFFFFFF);
			BlockFacing face = BlockFacing.ALLFACES[faceIndex];
			int dx2 = (int)(num & 0x3FF);
			int dy2 = (int)((num >> 20) & 0x3FF);
			int dz2 = (int)((num >> 10) & 0x3FF);
			AssetLocation blockCode2 = BlockCodes[blockId];
			Block newBlock2 = worldForResolve.GetBlock(blockCode2);
			if (newBlock2 == null)
			{
				continue;
			}
			if (flipAxis.HasValue)
			{
				if (flipAxis == EnumAxis.Y)
				{
					dy2 = SizeY - dy2;
					AssetLocation newCode5 = newBlock2.GetVerticallyFlippedBlockCode();
					newBlock2 = worldForResolve.GetBlock(newCode5);
					if (face.IsVertical)
					{
						face = face.Opposite;
					}
				}
				if (flipAxis == EnumAxis.X)
				{
					dx2 = SizeX - dx2;
					AssetLocation newCode6 = newBlock2.GetHorizontallyFlippedBlockCode(flipAxis.Value);
					newBlock2 = worldForResolve.GetBlock(newCode6);
					if (face.Axis == EnumAxis.X)
					{
						face = face.Opposite;
					}
				}
				if (flipAxis == EnumAxis.Z)
				{
					dz2 = SizeZ - dz2;
					AssetLocation newCode7 = newBlock2.GetHorizontallyFlippedBlockCode(flipAxis.Value);
					newBlock2 = worldForResolve.GetBlock(newCode7);
					if (face.Axis == EnumAxis.Z)
					{
						face = face.Opposite;
					}
				}
			}
			if (angle != 0)
			{
				AssetLocation newCode8 = newBlock2.GetRotatedBlockCode(angle);
				newBlock2 = worldForResolve.GetBlock(newCode8);
			}
			BlockPos pos2 = GetRotatedPos(aroundOrigin, angle, dx2, dy2, dz2);
			DecorsUnpacked.TryGetValue(pos2, out var decorsTmp);
			if (decorsTmp == null)
			{
				decorsTmp = new Dictionary<int, Block>();
				DecorsUnpacked[pos2] = decorsTmp;
			}
			decorsTmp[faceAndSubPosition / 6 * 6 + face.GetHorizontalRotated(angle).Index] = newBlock2;
		}
		using FastMemoryStream reusableMemoryStream = new FastMemoryStream();
		foreach (KeyValuePair<uint, string> val in BlockEntities)
		{
			uint key = val.Key;
			int dx3 = (int)(key & 0x3FF);
			int dy3 = (int)((key >> 20) & 0x3FF);
			int dz3 = (int)((key >> 10) & 0x3FF);
			if (flipAxis == EnumAxis.Y)
			{
				dy3 = SizeY - dy3;
			}
			if (flipAxis == EnumAxis.X)
			{
				dx3 = SizeX - dx3;
			}
			if (flipAxis == EnumAxis.Z)
			{
				dz3 = SizeZ - dz3;
			}
			BlockPos pos3 = GetRotatedPos(aroundOrigin, angle, dx3, dy3, dz3);
			string beData = val.Value;
			Block block = worldForResolve.GetBlock(BlocksUnpacked[pos3]);
			string entityclass = block.EntityClass;
			if (entityclass != null)
			{
				BlockEntity be = worldForResolve.ClassRegistry.CreateBlockEntity(entityclass);
				ITreeAttribute tree = DecodeBlockEntityData(beData);
				if (be is IRotatable rotatable)
				{
					be.Pos = pos3;
					be.CreateBehaviors(block, worldForResolve);
					rotatable.OnTransformed(worldForResolve, tree, angle, BlockCodes, ItemCodes, flipAxis);
				}
				tree.SetString("blockCode", block.Code.ToShortString());
				beData = StringEncodeTreeAttribute(tree, reusableMemoryStream);
				BlockEntitiesUnpacked[pos3] = beData;
			}
		}
		foreach (string entity2 in Entities)
		{
			using MemoryStream ms = new MemoryStream(Ascii85.Decode(entity2));
			BinaryReader reader = new BinaryReader(ms);
			string className = reader.ReadString();
			Entity entity = worldForResolve.ClassRegistry.CreateEntity(className);
			entity.FromBytes(reader, isSync: false);
			EntityPos pos4 = entity.Pos;
			double offx = 0.0;
			double offz = 0.0;
			if (aroundOrigin != EnumOrigin.StartPos)
			{
				offx = (double)SizeX / 2.0;
				offz = (double)SizeZ / 2.0;
			}
			pos4.X -= offx;
			pos4.Z -= offz;
			double x = pos4.X;
			double z = pos4.Z;
			switch (angle)
			{
			case 90:
				pos4.X = 0.0 - z;
				pos4.Z = x;
				break;
			case 180:
				pos4.X = 0.0 - x;
				pos4.Z = 0.0 - z;
				break;
			case 270:
				pos4.X = z;
				pos4.Z = 0.0 - x;
				break;
			}
			if (aroundOrigin != EnumOrigin.StartPos)
			{
				pos4.X += offx;
				pos4.Z += offz;
			}
			pos4.Yaw -= (float)angle * ((float)Math.PI / 180f);
			entity.PositionBeforeFalling.X = pos4.X;
			entity.PositionBeforeFalling.Z = pos4.Z;
			EntitiesUnpacked.Add(entity);
		}
		BlockPos startPos = new BlockPos(1024, 1024, 1024);
		Pack(worldForResolve, startPos, skipImportExportForEntities: true);
	}

	public BlockPos GetRotatedPos(EnumOrigin aroundOrigin, int angle, int dx, int dy, int dz)
	{
		if (aroundOrigin != EnumOrigin.StartPos)
		{
			dx -= SizeX / 2;
			dz -= SizeZ / 2;
		}
		BlockPos pos = new BlockPos(dx, dy, dz);
		switch (angle)
		{
		case 90:
			pos.Set(-dz, dy, dx);
			break;
		case 180:
			pos.Set(-dx, dy, -dz);
			break;
		case 270:
			pos.Set(dz, dy, -dx);
			break;
		}
		if (aroundOrigin != EnumOrigin.StartPos)
		{
			pos.X += SizeX / 2;
			pos.Z += SizeZ / 2;
		}
		return pos;
	}

	/// <summary>
	/// Places all the entities and blocks in the schematic at the position.
	/// </summary>
	/// <param name="blockAccessor"></param>
	/// <param name="worldForCollectibleResolve"></param>
	/// <param name="startPos"></param>
	/// <param name="blockCodes"></param>
	/// <param name="itemCodes"></param>
	/// <param name="replaceBlockEntities"></param>
	/// <param name="replaceBlocks"></param>
	/// <param name="centerrockblockid"></param>
	/// <param name="layerBlockForBlockEntities"></param>
	/// <param name="resolveImports">Turn it off to spawn structures as they are. For example, in this mode, instead of traders, their meta spawners will spawn</param>
	public void PlaceEntitiesAndBlockEntities(IBlockAccessor blockAccessor, IWorldAccessor worldForCollectibleResolve, BlockPos startPos, Dictionary<int, AssetLocation> blockCodes, Dictionary<int, AssetLocation> itemCodes, bool replaceBlockEntities = false, Dictionary<int, Dictionary<int, int>> replaceBlocks = null, int centerrockblockid = 0, Dictionary<BlockPos, Block> layerBlockForBlockEntities = null, bool resolveImports = true)
	{
		BlockPos curPos = startPos.Copy();
		int schematicSeed = worldForCollectibleResolve.Rand.Next();
		foreach (KeyValuePair<uint, string> val in BlockEntities)
		{
			uint key = val.Key;
			int dx = (int)(key & 0x3FF);
			int dy = (int)((key >> 20) & 0x3FF);
			int dz = (int)((key >> 10) & 0x3FF);
			curPos.Set(dx + startPos.X, dy + startPos.Y, dz + startPos.Z);
			if (!blockAccessor.IsValidPos(curPos))
			{
				continue;
			}
			BlockEntity be = blockAccessor.GetBlockEntity(curPos);
			if ((be == null || replaceBlockEntities) && blockAccessor is IWorldGenBlockAccessor)
			{
				Block block = blockAccessor.GetBlock(curPos, 1);
				if (block.EntityClass != null)
				{
					blockAccessor.SpawnBlockEntity(block.EntityClass, curPos);
					be = blockAccessor.GetBlockEntity(curPos);
				}
			}
			if (be == null)
			{
				continue;
			}
			if (!replaceBlockEntities)
			{
				Block block2 = blockAccessor.GetBlock(curPos, 1);
				if (block2.EntityClass != worldForCollectibleResolve.ClassRegistry.GetBlockEntityClass(be.GetType()))
				{
					worldForCollectibleResolve.Logger.Warning("Could not import block entity data for schematic at {0}. There is already {1}, expected {2}. Probably overlapping ruins.", curPos, be.GetType(), block2.EntityClass);
					continue;
				}
			}
			ITreeAttribute tree = DecodeBlockEntityData(val.Value);
			string treeBlockCode = tree.GetString("blockCode");
			if (be.Block != null && treeBlockCode != null)
			{
				Block encodedBlock = worldForCollectibleResolve.GetBlock(new AssetLocation(treeBlockCode));
				if (encodedBlock != null && encodedBlock.GetType() != be.Block.GetType())
				{
					foreach (KeyValuePair<string, Dictionary<string, string>> blockRemap in BlockRemaps)
					{
						if (blockRemap.Value.TryGetValue(treeBlockCode, out var newBlockCode))
						{
							encodedBlock = worldForCollectibleResolve.GetBlock(new AssetLocation(newBlockCode));
							break;
						}
					}
					if (encodedBlock != null && encodedBlock.GetType() != be.Block.GetType())
					{
						worldForCollectibleResolve.Logger.Warning("Could not import block entity data for schematic at {0}. There is already {1}, expected {2}. Possibly overlapping ruins, or is this schematic from an old game version?", curPos, be.Block, treeBlockCode);
						continue;
					}
				}
			}
			tree.SetInt("posx", curPos.X);
			tree.SetInt("posy", curPos.InternalY);
			tree.SetInt("posz", curPos.Z);
			be.FromTreeAttributes(tree, worldForCollectibleResolve);
			be.OnLoadCollectibleMappings(worldForCollectibleResolve, blockCodes, itemCodes, schematicSeed, resolveImports);
			Block layerBlock = null;
			layerBlockForBlockEntities?.TryGetValue(curPos, out layerBlock);
			if (layerBlock != null && layerBlock.Id == 0)
			{
				layerBlock = null;
			}
			be.OnPlacementBySchematic(worldForCollectibleResolve.Api as ICoreServerAPI, blockAccessor, curPos, replaceBlocks, centerrockblockid, layerBlock, resolveImports);
			if (!(blockAccessor is IWorldGenBlockAccessor))
			{
				be.MarkDirty();
			}
		}
		if (blockAccessor is IMiniDimension)
		{
			return;
		}
		foreach (string entity2 in Entities)
		{
			using MemoryStream ms = new MemoryStream(Ascii85.Decode(entity2));
			BinaryReader reader = new BinaryReader(ms);
			string className = reader.ReadString();
			try
			{
				Entity entity = worldForCollectibleResolve.ClassRegistry.CreateEntity(className);
				entity.FromBytes(reader, isSync: false, ((IServerWorldAccessor)worldForCollectibleResolve).RemappedEntities);
				entity.DidImportOrExport(startPos);
				if (OriginalPos != null)
				{
					BlockPos prevOffset = entity.WatchedAttributes.GetBlockPos("importOffset", Zero);
					entity.WatchedAttributes.SetBlockPos("importOffset", startPos - OriginalPos + prevOffset);
				}
				if (worldForCollectibleResolve.GetEntityType(entity.Code) != null)
				{
					if (blockAccessor is IWorldGenBlockAccessor accessor)
					{
						accessor.AddEntity(entity);
						if (!entity.TryEarlyLoadCollectibleMappings(worldForCollectibleResolve, BlockCodes, ItemCodes, schematicSeed, resolveImports))
						{
							entity.OnInitialized += delegate
							{
								entity.OnLoadCollectibleMappings(worldForCollectibleResolve, BlockCodes, ItemCodes, schematicSeed, resolveImports);
							};
						}
					}
					else
					{
						worldForCollectibleResolve.SpawnEntity(entity);
						if (blockAccessor is IBlockAccessorRevertable re)
						{
							re.StoreEntitySpawnToHistory(entity);
						}
						entity.OnLoadCollectibleMappings(worldForCollectibleResolve, BlockCodes, ItemCodes, schematicSeed, resolveImports);
					}
				}
				else
				{
					worldForCollectibleResolve.Logger.Error("Couldn't import entity {0} with id {1} and code {2} - it's Type is null! Maybe from an older game version or a missing mod.", entity.GetType(), entity.EntityId, entity.Code);
				}
			}
			catch (Exception)
			{
				worldForCollectibleResolve.Logger.Error("Couldn't import entity with classname {0} - Maybe from an older game version or a missing mod.", className);
			}
		}
	}

	/// <summary>
	/// Gets just the positions of the blocks.
	/// </summary>
	/// <param name="origin">The origin point to start from</param>
	/// <returns>An array containing the BlockPos of each block in the area.</returns>
	public virtual BlockPos[] GetJustPositions(BlockPos origin)
	{
		BlockPos[] positions = new BlockPos[Indices.Count];
		for (int i = 0; i < Indices.Count; i++)
		{
			uint num = Indices[i];
			int dx = (int)(num & 0x3FF);
			int dy = (int)((num >> 20) & 0x3FF);
			int dz = (int)((num >> 10) & 0x3FF);
			BlockPos pos = new BlockPos(dx, dy, dz);
			positions[i] = pos.Add(origin);
		}
		return positions;
	}

	/// <summary>
	/// Gets the starting position of the schematic.
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="origin"></param>
	/// <returns></returns>
	public virtual BlockPos GetStartPos(BlockPos pos, EnumOrigin origin)
	{
		return AdjustStartPos(pos.Copy(), origin);
	}

	/// <summary>
	/// Adjusts the starting position of the schemtic.
	/// </summary>
	/// <param name="startpos"></param>
	/// <param name="origin"></param>
	/// <returns></returns>
	public virtual BlockPos AdjustStartPos(BlockPos startpos, EnumOrigin origin)
	{
		if (origin == EnumOrigin.TopCenter)
		{
			startpos.X -= SizeX / 2;
			startpos.Y -= SizeY;
			startpos.Z -= SizeZ / 2;
		}
		if (origin == EnumOrigin.BottomCenter)
		{
			startpos.X -= SizeX / 2;
			startpos.Z -= SizeZ / 2;
		}
		if (origin == EnumOrigin.MiddleCenter)
		{
			startpos.X -= SizeX / 2;
			startpos.Y -= SizeY / 2;
			startpos.Z -= SizeZ / 2;
		}
		return startpos;
	}

	/// <summary>
	/// Loads the schematic from a file.
	/// </summary>
	/// <param name="infilepath"></param>
	/// <param name="error"></param>
	/// <returns></returns>
	public static BlockSchematic LoadFromFile(string infilepath, ref string error)
	{
		if (!File.Exists(infilepath) && File.Exists(infilepath + ".json"))
		{
			infilepath += ".json";
		}
		if (!File.Exists(infilepath))
		{
			error = "Can't import " + infilepath + ", it does not exist";
			return null;
		}
		BlockSchematic blockdata = null;
		try
		{
			using TextReader textReader = new StreamReader(infilepath);
			blockdata = JsonConvert.DeserializeObject<BlockSchematic>(textReader.ReadToEnd());
			textReader.Close();
			return blockdata;
		}
		catch (Exception ex)
		{
			error = "Failed loading " + infilepath + " : " + ex.Message;
			return null;
		}
	}

	/// <summary>
	/// Loads a schematic from a string.
	/// </summary>
	/// <param name="jsoncode"></param>
	/// <param name="error"></param>
	/// <returns></returns>
	public static BlockSchematic LoadFromString(string jsoncode, ref string error)
	{
		try
		{
			return JsonConvert.DeserializeObject<BlockSchematic>(jsoncode);
		}
		catch (Exception ex)
		{
			error = "Failed loading schematic from json code : " + ex.Message;
			return null;
		}
	}

	/// <summary>
	/// Saves a schematic to a file.
	/// </summary>
	/// <param name="outfilepath"></param>
	/// <returns></returns>
	public virtual string Save(string outfilepath)
	{
		if (!outfilepath.EndsWithOrdinal(".json"))
		{
			outfilepath += ".json";
		}
		try
		{
			using TextWriter textWriter = new StreamWriter(outfilepath);
			textWriter.Write(JsonConvert.SerializeObject(this, Formatting.None));
			textWriter.Close();
		}
		catch (IOException ex)
		{
			return "Failed exporting: " + ex.Message;
		}
		return null;
	}

	public virtual string ToJson()
	{
		return JsonConvert.SerializeObject(this, Formatting.None);
	}

	/// <summary>
	/// Exports the block entity data to a string.
	/// </summary>
	/// <param name="be"></param>
	/// <returns></returns>
	public virtual string EncodeBlockEntityData(BlockEntity be)
	{
		using FastMemoryStream ms = new FastMemoryStream();
		return EncodeBlockEntityData(be, ms);
	}

	public virtual string EncodeBlockEntityData(BlockEntity be, FastMemoryStream ms)
	{
		TreeAttribute tree = new TreeAttribute();
		be.ToTreeAttributes(tree);
		return StringEncodeTreeAttribute(tree, ms);
	}

	/// <summary>
	/// Exports the tree attribute data to a string.
	/// </summary>
	/// <param name="tree"></param>
	/// <returns></returns>
	public virtual string StringEncodeTreeAttribute(ITreeAttribute tree)
	{
		using FastMemoryStream ms = new FastMemoryStream();
		return StringEncodeTreeAttribute(tree, ms);
	}

	public virtual string StringEncodeTreeAttribute(ITreeAttribute tree, FastMemoryStream ms)
	{
		ms.Reset();
		BinaryWriter writer = new BinaryWriter(ms);
		tree.ToBytes(writer);
		return Ascii85.Encode(ms.ToArray());
	}

	/// <summary>
	/// Imports the tree data from a string.
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	public virtual TreeAttribute DecodeBlockEntityData(string data)
	{
		byte[] buffer = Ascii85.Decode(data);
		TreeAttribute tree = new TreeAttribute();
		using MemoryStream ms = new MemoryStream(buffer);
		BinaryReader reader = new BinaryReader(ms);
		tree.FromBytes(reader);
		return tree;
	}

	public bool IsFillerOrPath(Block newBlock)
	{
		if (newBlock.Id != FillerBlockId)
		{
			return newBlock.Id == PathwayBlockId;
		}
		return true;
	}

	protected virtual int PlaceReplaceAll(IBlockAccessor blockAccessor, BlockPos pos, Block newBlock, bool replaceMeta)
	{
		int layer = ((!newBlock.ForFluidsLayer) ? 1 : 2);
		blockAccessor.SetBlock((replaceMeta && IsFillerOrPath(newBlock)) ? empty : newBlock.BlockId, pos, layer);
		return 1;
	}

	protected virtual int PlaceReplaceable(IBlockAccessor blockAccessor, BlockPos pos, Block newBlock, bool replaceMeta)
	{
		if (newBlock.ForFluidsLayer || blockAccessor.GetBlock(pos, 4).Replaceable > newBlock.Replaceable)
		{
			int layer = ((!newBlock.ForFluidsLayer) ? 1 : 2);
			blockAccessor.SetBlock((replaceMeta && IsFillerOrPath(newBlock)) ? empty : newBlock.BlockId, pos, layer);
			return 1;
		}
		return 0;
	}

	protected virtual int PlaceReplaceAllNoAir(IBlockAccessor blockAccessor, BlockPos pos, Block newBlock, bool replaceMeta)
	{
		if (newBlock.BlockId != 0)
		{
			int layer = ((!newBlock.ForFluidsLayer) ? 1 : 2);
			blockAccessor.SetBlock((replaceMeta && IsFillerOrPath(newBlock)) ? empty : newBlock.BlockId, pos, layer);
			return 1;
		}
		return 0;
	}

	protected virtual int PlaceReplaceOnlyAir(IBlockAccessor blockAccessor, BlockPos pos, Block newBlock, bool replaceMeta)
	{
		if (blockAccessor.GetMostSolidBlock(pos).BlockId == 0)
		{
			int layer = ((!newBlock.ForFluidsLayer) ? 1 : 2);
			blockAccessor.SetBlock((replaceMeta && IsFillerOrPath(newBlock)) ? empty : newBlock.BlockId, pos, layer);
			return 1;
		}
		return 0;
	}

	/// <summary>
	/// Makes a deep copy of the packed schematic. Unpacked data and loaded meta information is not cloned.
	/// </summary>
	/// <returns></returns>
	public virtual BlockSchematic ClonePacked()
	{
		return new BlockSchematic
		{
			SizeX = SizeX,
			SizeY = SizeY,
			SizeZ = SizeZ,
			GameVersion = GameVersion,
			BlockCodes = new Dictionary<int, AssetLocation>(BlockCodes),
			ItemCodes = new Dictionary<int, AssetLocation>(ItemCodes),
			Indices = new List<uint>(Indices),
			BlockIds = new List<int>(BlockIds),
			BlockEntities = new Dictionary<uint, string>(BlockEntities),
			Entities = new List<string>(Entities),
			DecorIndices = new List<uint>(DecorIndices),
			DecorIds = new List<long>(DecorIds),
			ReplaceMode = ReplaceMode,
			EntranceRotation = EntranceRotation,
			OriginalPos = OriginalPos
		};
	}

	public void PasteToMiniDimension(ICoreServerAPI sapi, IBlockAccessor blockAccess, IMiniDimension miniDimension, BlockPos originPos, bool replaceMetaBlocks)
	{
		Init(blockAccess);
		Place(miniDimension, sapi.World, originPos, EnumReplaceMode.ReplaceAll, replaceMetaBlocks);
		PlaceDecors(miniDimension, originPos);
	}
}
