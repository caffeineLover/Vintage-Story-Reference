using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.ServerMods.WorldEdit;

[ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
public class WorldEditWorkspace
{
	public bool ToolsEnabled;

	public string PlayerUID;

	public EnumWorldEditConstraint WorldEditConstraint;

	private BlockPos prevStartMarker;

	private BlockPos prevEndMarker;

	private BlockPos startMarker;

	private BlockPos endMarker;

	public Vec3d StartMarkerExact;

	public Vec3d EndMarkerExact;

	private Vec3d _prevStartMarkerExact;

	private Vec3d _prevEndMarkerExact;

	public int ImportAngle;

	public bool ImportFlipped;

	public bool DoRelight = true;

	internal IBlockAccessorRevertable revertableBlockAccess;

	internal IWorldAccessor world;

	public bool serverOverloadProtection = true;

	public EnumToolOffsetMode ToolOffsetMode;

	public Dictionary<string, float> FloatValues = new Dictionary<string, float>();

	public Dictionary<string, int> IntValues = new Dictionary<string, int>();

	public Dictionary<string, string> StringValues = new Dictionary<string, string>();

	public Dictionary<string, byte[]> ByteDataValues = new Dictionary<string, byte[]>();

	public string ToolName;

	internal ToolBase ToolInstance;

	internal BlockSchematic clipboardBlockData;

	internal IMiniDimension previewBlocks;

	private ICoreServerAPI sapi;

	public BlockPos StartMarker
	{
		get
		{
			return startMarker;
		}
		set
		{
			if (world != null)
			{
				world.Api.ObjectCache["weStartMarker-" + PlayerUID] = value;
			}
			startMarker = value;
		}
	}

	public BlockPos EndMarker
	{
		get
		{
			return endMarker;
		}
		set
		{
			if (world != null)
			{
				world.Api.ObjectCache["weEndMarker-" + PlayerUID] = value;
			}
			endMarker = value;
		}
	}

	[ProtoIgnore]
	public BlockSchematic PreviewBlockData { get; set; }

	[ProtoIgnore]
	public BlockPos PreviewPos { get; set; }

	public bool Rsp
	{
		get
		{
			return IntValues["std.settingsRsp"] > 0;
		}
		set
		{
			IntValues["std.settingsRsp"] = (value ? 1 : 0);
		}
	}

	public int ToolAxisLock
	{
		get
		{
			return IntValues["std.settingsAxisLock"];
		}
		set
		{
			IntValues["std.settingsAxisLock"] = value;
		}
	}

	public int StepSize
	{
		get
		{
			return IntValues["std.stepSize"];
		}
		set
		{
			IntValues["std.stepSize"] = value;
		}
	}

	public int DimensionId
	{
		get
		{
			return IntValues["std.dimensionId"];
		}
		set
		{
			IntValues["std.dimensionId"] = value;
		}
	}

	public WorldEditWorkspace()
	{
	}

	public WorldEditWorkspace(IWorldAccessor world, IBlockAccessorRevertable blockAccessor)
	{
		revertableBlockAccess = blockAccessor;
		this.world = world;
		blockAccessor.OnStoreHistoryState += BlockAccessor_OnStoreHistoryState;
		blockAccessor.OnRestoreHistoryState += BlockAccessor_OnRestoreHistoryState;
	}

	public void Init(ICoreServerAPI api)
	{
		sapi = api;
		if (!IntValues.ContainsKey("std.stepSize"))
		{
			StepSize = 1;
		}
		if (!IntValues.ContainsKey("std.settingsAxisLock"))
		{
			ToolAxisLock = 0;
		}
		if (!IntValues.ContainsKey("std.settingsRsp"))
		{
			Rsp = true;
		}
		if (!IntValues.ContainsKey("std.dimensionId"))
		{
			DimensionId = -1;
		}
		previewBlocks = world.BlockAccessor.CreateMiniDimension(new Vec3d());
		if (DimensionId == -1)
		{
			DimensionId = sapi.Server.LoadMiniDimension(previewBlocks);
		}
		else
		{
			sapi.Server.SetMiniDimension(previewBlocks, DimensionId);
		}
		previewBlocks.SetSubDimensionId(DimensionId);
		previewBlocks.BlocksPreviewSubDimension_Server = DimensionId;
	}

	private void BlockAccessor_OnRestoreHistoryState(HistoryState obj, int dir)
	{
		if (dir > 0)
		{
			StartMarker = obj.NewStartMarker?.Copy();
			EndMarker = obj.NewEndMarker?.Copy();
			StartMarkerExact = obj.NewStartMarkerExact?.Clone();
			EndMarkerExact = obj.NewEndMarkerExact?.Clone();
		}
		else
		{
			StartMarker = obj.OldStartMarker?.Copy();
			EndMarker = obj.OldEndMarker?.Copy();
			StartMarkerExact = obj.OldStartMarkerExact?.Clone();
			EndMarkerExact = obj.OldEndMarkerExact?.Clone();
		}
		HighlightSelectedArea();
	}

	private void BlockAccessor_OnStoreHistoryState(HistoryState obj)
	{
		obj.OldStartMarker = prevStartMarker?.Copy();
		obj.OldEndMarker = prevEndMarker?.Copy();
		obj.NewStartMarker = StartMarker?.Copy();
		obj.NewEndMarker = EndMarker?.Copy();
		prevStartMarker = StartMarker?.Copy();
		prevEndMarker = EndMarker?.Copy();
		obj.OldStartMarkerExact = _prevStartMarkerExact?.Clone();
		obj.OldEndMarkerExact = _prevEndMarkerExact?.Clone();
		obj.NewStartMarkerExact = StartMarkerExact?.Clone();
		obj.NewEndMarkerExact = EndMarkerExact?.Clone();
		_prevStartMarkerExact = StartMarkerExact?.Clone();
		_prevEndMarkerExact = EndMarkerExact?.Clone();
	}

	public void SetTool(string toolname, ICoreAPI api)
	{
		ToolName = toolname;
		if (ToolInstance != null)
		{
			ToolInstance.Unload(api);
		}
		if (toolname != null)
		{
			ToolInstance = ToolRegistry.InstanceFromType(toolname, this, revertableBlockAccess);
			if (ToolInstance == null)
			{
				ToolName = null;
			}
			else
			{
				ToolInstance.Load(api);
			}
		}
	}

	public void ToBytes(BinaryWriter writer)
	{
		writer.Write(ToolsEnabled);
		writer.Write(PlayerUID);
		writer.Write(StartMarker == null);
		if (StartMarker != null)
		{
			writer.Write(StartMarker.X);
			writer.Write(StartMarker.InternalY);
			writer.Write(StartMarker.Z);
		}
		writer.Write(EndMarker == null);
		if (EndMarker != null)
		{
			writer.Write(EndMarker.X);
			writer.Write(EndMarker.InternalY);
			writer.Write(EndMarker.Z);
		}
		writer.Write(FloatValues.Count);
		foreach (KeyValuePair<string, float> val in FloatValues)
		{
			writer.Write(val.Key);
			writer.Write(val.Value);
		}
		writer.Write(IntValues.Count);
		foreach (KeyValuePair<string, int> val2 in IntValues)
		{
			writer.Write(val2.Key);
			writer.Write(val2.Value);
		}
		writer.Write(StringValues.Count);
		foreach (KeyValuePair<string, string> val3 in StringValues)
		{
			writer.Write(val3.Value == null);
			if (val3.Value != null)
			{
				writer.Write(val3.Key);
				writer.Write(val3.Value);
			}
		}
		writer.Write(ByteDataValues.Count);
		foreach (KeyValuePair<string, byte[]> val4 in ByteDataValues)
		{
			writer.Write(val4.Value == null);
			if (val4.Value != null)
			{
				writer.Write(val4.Key);
				writer.Write(val4.Value.Length);
				writer.Write(val4.Value);
			}
		}
		writer.Write(ToolName == null);
		if (ToolName != null)
		{
			writer.Write(ToolName);
		}
		writer.Write((int)ToolOffsetMode);
		writer.Write(DoRelight);
		writer.Write(serverOverloadProtection);
	}

	public void FromBytes(BinaryReader reader)
	{
		try
		{
			ToolsEnabled = reader.ReadBoolean();
			PlayerUID = reader.ReadString();
			if (!reader.ReadBoolean())
			{
				StartMarker = new BlockPos(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
				StartMarkerExact = StartMarker.ToVec3d().Add(0.5);
			}
			else
			{
				StartMarker = null;
			}
			if (!reader.ReadBoolean())
			{
				EndMarker = new BlockPos(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
				EndMarkerExact = EndMarker.ToVec3d().Add(-0.5);
			}
			else
			{
				EndMarker = null;
			}
			FloatValues = new Dictionary<string, float>();
			IntValues = new Dictionary<string, int>();
			StringValues = new Dictionary<string, string>();
			ByteDataValues = new Dictionary<string, byte[]>();
			int floatValCount = reader.ReadInt32();
			for (int i = 0; i < floatValCount; i++)
			{
				FloatValues[reader.ReadString()] = reader.ReadSingle();
			}
			int intValCount = reader.ReadInt32();
			for (int j = 0; j < intValCount; j++)
			{
				IntValues[reader.ReadString()] = reader.ReadInt32();
			}
			int stringValCount = reader.ReadInt32();
			for (int k = 0; k < stringValCount; k++)
			{
				if (!reader.ReadBoolean())
				{
					StringValues[reader.ReadString()] = reader.ReadString();
				}
			}
			int byteDataValCount = reader.ReadInt32();
			for (int l = 0; l < byteDataValCount; l++)
			{
				if (!reader.ReadBoolean())
				{
					string key = reader.ReadString();
					int qbytes = reader.ReadInt32();
					ByteDataValues[key] = reader.ReadBytes(qbytes);
				}
			}
			if (!reader.ReadBoolean())
			{
				ToolName = reader.ReadString();
				SetTool(ToolName, world.Api);
			}
			ToolOffsetMode = (EnumToolOffsetMode)reader.ReadInt32();
			DoRelight = reader.ReadBoolean();
			revertableBlockAccess.Relight = DoRelight;
			serverOverloadProtection = reader.ReadBoolean();
		}
		catch (Exception)
		{
		}
	}

	public BlockPos GetMarkedMinPos()
	{
		return new BlockPos(Math.Min(StartMarker.X, EndMarker.X), Math.Min(StartMarker.InternalY, EndMarker.InternalY), Math.Min(StartMarker.Z, EndMarker.Z));
	}

	public BlockPos GetMarkedMaxPos()
	{
		return new BlockPos(Math.Max(StartMarker.X, EndMarker.X), Math.Max(StartMarker.InternalY, EndMarker.InternalY), Math.Max(StartMarker.Z, EndMarker.Z));
	}

	public void ResendBlockHighlights()
	{
		IPlayer player = world.PlayerByUid(PlayerUID);
		List<BlockPos> blockPosList = new List<BlockPos>();
		EnumHighlightBlocksMode mode;
		if (ToolsEnabled && ToolInstance != null)
		{
			mode = EnumHighlightBlocksMode.CenteredToSelectedBlock;
			if (ToolInstance.GetType().Name.Equals("MicroblockTool"))
			{
				if (ToolOffsetMode == EnumToolOffsetMode.Attach)
				{
					mode = EnumHighlightBlocksMode.AttachedToBlockSelectionIndex;
				}
			}
			else if (ToolOffsetMode == EnumToolOffsetMode.Attach)
			{
				mode = EnumHighlightBlocksMode.AttachedToSelectedBlock;
			}
			ToolBase toolInstance = ToolInstance;
			if (!(toolInstance is ImportTool))
			{
				if (toolInstance is MoveTool)
				{
					if (PreviewBlockData != null)
					{
						CreatePreview(PreviewBlockData, PreviewPos);
					}
					HighlightSelectedArea();
					return;
				}
				if (toolInstance is SelectTool || toolInstance is RepeatTool)
				{
					goto IL_00e3;
				}
			}
			else if (PreviewBlockData != null && PreviewPos != null)
			{
				CreatePreview(PreviewBlockData, PreviewPos);
				HighlightSelectedArea();
				return;
			}
			DestroyPreview();
			goto IL_00e3;
		}
		DestroyPreview();
		world.HighlightBlocks(player, 1, blockPosList, null);
		world.HighlightBlocks(player, 0, blockPosList, null);
		world.HighlightBlocks(player, 4, blockPosList);
		world.HighlightBlocks(player, 5, blockPosList);
		return;
		IL_00e3:
		HighlightSelectedArea();
		ToolInstance.HighlightBlocks(player, sapi, mode);
	}

	public void HighlightSelectedArea()
	{
		IPlayer player = world.PlayerByUid(PlayerUID);
		List<BlockPos> blockPosList = new List<BlockPos>();
		if (StartMarker != null && EndMarker != null)
		{
			world.HighlightBlocks(player, 1, blockPosList);
			world.HighlightBlocks(player, 0, new List<BlockPos>(new BlockPos[2] { StartMarker, EndMarker }), EnumHighlightBlocksMode.Absolute, EnumHighlightShape.Cube);
			world.HighlightBlocks(player, 4, new List<BlockPos>(new BlockPos[1] { StartMarkerExact.AsBlockPos ?? StartMarker }), new List<int> { ColorUtil.ColorFromRgba(0, 255, 0, 60) });
			world.HighlightBlocks(player, 5, new List<BlockPos>(new BlockPos[1] { EndMarkerExact.AsBlockPos ?? EndMarker }), new List<int> { ColorUtil.ColorFromRgba(255, 0, 0, 60) });
		}
		else
		{
			world.HighlightBlocks(player, 1, blockPosList);
			world.HighlightBlocks(player, 0, blockPosList);
			world.HighlightBlocks(player, 4, blockPosList);
			world.HighlightBlocks(player, 5, blockPosList);
		}
	}

	public void UpdateSelection()
	{
		if (StartMarkerExact != null && EndMarkerExact != null)
		{
			StartMarker = new BlockPos((int)Math.Min(StartMarkerExact.X, EndMarkerExact.X), (int)Math.Min(StartMarkerExact.Y, EndMarkerExact.Y), (int)Math.Min(StartMarkerExact.Z, EndMarkerExact.Z));
			EndMarker = new BlockPos((int)Math.Ceiling(Math.Max(StartMarkerExact.X, EndMarkerExact.X)), (int)Math.Ceiling(Math.Max(StartMarkerExact.Y, EndMarkerExact.Y)), (int)Math.Ceiling(Math.Max(StartMarkerExact.Z, EndMarkerExact.Z)));
		}
		if (!(StartMarker == null) && !(EndMarker == null))
		{
			EnsureInsideMap(StartMarker);
			EnsureInsideMap(EndMarker);
			HighlightSelectedArea();
			revertableBlockAccess.StoreHistoryState(HistoryState.Empty());
		}
	}

	private void EnsureInsideMap(BlockPos pos)
	{
		pos.X = GameMath.Clamp(pos.X, 0, world.BlockAccessor.MapSizeX - 1);
		pos.Y = GameMath.Clamp(pos.Y, 0, world.BlockAccessor.MapSizeY - 1);
		pos.Z = GameMath.Clamp(pos.Z, 0, world.BlockAccessor.MapSizeZ - 1);
	}

	public void GrowSelection(BlockFacing facing, int amount)
	{
		if (facing == BlockFacing.WEST)
		{
			StartMarkerExact.X -= amount;
		}
		if (facing == BlockFacing.EAST)
		{
			EndMarkerExact.X += amount;
		}
		if (facing == BlockFacing.NORTH)
		{
			StartMarkerExact.Z -= amount;
		}
		if (facing == BlockFacing.SOUTH)
		{
			EndMarkerExact.Z += amount;
		}
		if (facing == BlockFacing.DOWN)
		{
			StartMarkerExact.Y -= amount;
		}
		if (facing == BlockFacing.UP)
		{
			EndMarkerExact.Y += amount;
		}
		UpdateSelection();
	}

	public BlockFacing GetFacing(EntityPos pos)
	{
		switch (ToolAxisLock)
		{
		case 0:
		{
			Vec3f lookVec = pos.GetViewVector();
			return BlockFacing.FromVector(lookVec.X, lookVec.Y, lookVec.Z);
		}
		case 1:
			return BlockFacing.EAST;
		case 2:
			return BlockFacing.UP;
		case 3:
			return BlockFacing.NORTH;
		case 4:
			return BlockFacing.HorizontalFromYaw(pos.Yaw);
		default:
			return BlockFacing.NORTH;
		}
	}

	internal int FillArea(ItemStack blockStack, BlockPos start, BlockPos end, bool notLiquids = false)
	{
		IServerPlayer player = (IServerPlayer)world.PlayerByUid(PlayerUID);
		int updated = 0;
		BlockPos startPos = new BlockPos(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y), Math.Min(start.Z, end.Z), start.dimension);
		BlockPos finalPos = new BlockPos(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y), Math.Max(start.Z, end.Z), start.dimension);
		BlockPos curPos = startPos.Copy();
		int dx = finalPos.X - startPos.X;
		int dy = finalPos.Y - startPos.Y;
		int dz = finalPos.Z - startPos.Z;
		int quantityBlocks = dx * dy * dz;
		int blockId = 0;
		Block block = blockStack?.Block;
		if (block != null)
		{
			blockId = block.Id;
		}
		if (block != null && !MayPlace(block, quantityBlocks))
		{
			return 0;
		}
		if (quantityBlocks > 1000)
		{
			WorldEdit.Good(player, ((block == null) ? "Clearing" : "Placing") + " " + dx * dy * dz + " blocks...");
		}
		while (curPos.X < finalPos.X)
		{
			curPos.Y = startPos.Y;
			while (curPos.Y < finalPos.Y)
			{
				curPos.Z = startPos.Z;
				while (curPos.Z < finalPos.Z)
				{
					if (!notLiquids)
					{
						revertableBlockAccess.SetBlock(0, curPos, 2);
					}
					revertableBlockAccess.SetBlock(blockId, curPos, blockStack);
					curPos.Z++;
					updated++;
				}
				curPos.Y++;
			}
			curPos.X++;
		}
		List<BlockUpdate> updatedBlocks = revertableBlockAccess.Commit();
		if (block == null)
		{
			revertableBlockAccess.PostCommitCleanup(updatedBlocks);
		}
		return updated;
	}

	public bool MayPlace(Block block, int quantityBlocks)
	{
		IServerPlayer player = (IServerPlayer)world.PlayerByUid(PlayerUID);
		if (serverOverloadProtection)
		{
			if (quantityBlocks > 100 && block.LightHsv[2] > 5)
			{
				WorldEdit.Bad(player, "Operation rejected. Server overload protection is on. Might kill the server to place that many light sources.");
				return false;
			}
			if (quantityBlocks > 8000000)
			{
				WorldEdit.Bad(player, "Operation rejected. Server overload protection is on. Might kill the server to (potentially) place that many blocks.");
				return false;
			}
			ItemStack stack = new ItemStack(block);
			if (quantityBlocks > 1000 && block.GetBlockMaterial(world.BlockAccessor, null, stack) == EnumBlockMaterial.Plant)
			{
				WorldEdit.Bad(player, "Operation rejected. Server overload protection is on. Might kill the server when placing that many plants (might cause massive neighbour block updates if one plant is broken).");
				return false;
			}
		}
		return true;
	}

	public int MoveArea(Vec3i offset, BlockPos start, BlockPos end)
	{
		int updated = 0;
		BlockPos startPos = new BlockPos(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y), Math.Min(start.Z, end.Z), start.dimension);
		BlockPos endPos = new BlockPos(Math.Max(start.X, end.X), Math.Max(start.Y, end.Y), Math.Max(start.Z, end.Z), start.dimension);
		BlockPos curPos = startPos.Copy();
		int quantityBlocks = offset.X * offset.Y * offset.Z;
		Block block = world.Blocks[0];
		if (!MayPlace(block, quantityBlocks))
		{
			return 0;
		}
		Dictionary<BlockPos, ((int, int), Dictionary<int, Block>)> blocksByNewPos = new Dictionary<BlockPos, ((int, int), Dictionary<int, Block>)>();
		Dictionary<BlockPos, TreeAttribute> blockEntityDataByNewPos = new Dictionary<BlockPos, TreeAttribute>();
		revertableBlockAccess.BeginMultiEdit();
		while (curPos.X < endPos.X)
		{
			curPos.Y = startPos.Y;
			while (curPos.Y < endPos.Y)
			{
				curPos.Z = startPos.Z;
				while (curPos.Z < endPos.Z)
				{
					BlockPos newPos = curPos.AddCopy(offset);
					BlockEntity be = revertableBlockAccess.GetBlockEntity(curPos);
					if (be != null)
					{
						TreeAttribute tree = new TreeAttribute();
						be.ToTreeAttributes(tree);
						blockEntityDataByNewPos[newPos] = tree;
					}
					Dictionary<int, Block> decors = revertableBlockAccess.GetSubDecors(curPos);
					int solidBlockId = revertableBlockAccess.GetBlock(curPos).Id;
					int fluidBlockId = revertableBlockAccess.GetBlock(curPos, 2)?.Id ?? (-1);
					blocksByNewPos[newPos] = ((solidBlockId, fluidBlockId), decors);
					revertableBlockAccess.SetBlock(0, curPos);
					if (fluidBlockId > 0)
					{
						revertableBlockAccess.SetBlock(0, curPos, 2);
					}
					curPos.Z++;
				}
				curPos.Y++;
			}
			curPos.X++;
		}
		BlockPos startOriginal = start.Copy();
		BlockPos endOriginal = end.Copy();
		start.Add(offset);
		end.Add(offset);
		revertableBlockAccess.Commit();
		foreach (KeyValuePair<BlockPos, ((int, int), Dictionary<int, Block>)> item2 in blocksByNewPos)
		{
			item2.Deconstruct(out var key, out var value);
			((int, int), Dictionary<int, Block>) tuple = value;
			(int, int) item = tuple.Item1;
			BlockPos pos = key;
			int solidBlockId2 = item.Item1;
			int fluidBlockId2 = item.Item2;
			Dictionary<int, Block> decors2 = tuple.Item2;
			revertableBlockAccess.SetBlock(solidBlockId2, pos);
			if (fluidBlockId2 >= 0)
			{
				revertableBlockAccess.SetBlock(fluidBlockId2, pos, 2);
			}
			if (decors2 == null)
			{
				continue;
			}
			foreach (KeyValuePair<int, Block> entry in decors2)
			{
				if (entry.Value != null)
				{
					revertableBlockAccess.SetDecor(entry.Value, pos, entry.Key);
				}
			}
		}
		foreach (BlockUpdate update in revertableBlockAccess.Commit())
		{
			if (update.OldBlockId == 0 && blockEntityDataByNewPos.TryGetValue(update.Pos, out var betree))
			{
				betree.SetInt("posx", update.Pos.X);
				betree.SetInt("posy", update.Pos.InternalY);
				betree.SetInt("posz", update.Pos.Z);
				update.NewBlockEntityData = betree.ToBytes();
			}
		}
		foreach (KeyValuePair<BlockPos, TreeAttribute> val in blockEntityDataByNewPos)
		{
			BlockPos pos2 = val.Key;
			BlockEntity be2 = revertableBlockAccess.GetBlockEntity(pos2);
			if (be2 != null)
			{
				val.Value.SetInt("posx", pos2.X);
				val.Value.SetInt("posy", pos2.InternalY);
				val.Value.SetInt("posz", pos2.Z);
				be2.FromTreeAttributes(val.Value, world);
			}
		}
		revertableBlockAccess.EndMultiEdit();
		revertableBlockAccess.StoreEntityMoveToHistory(startOriginal, endOriginal, offset);
		return updated;
	}

	public BlockSchematic CopyArea(BlockPos start, BlockPos end, bool notLiquids = false)
	{
		BlockPos startPos = new BlockPos(Math.Min(start.X, end.X), Math.Min(start.Y, end.Y), Math.Min(start.Z, end.Z), start.dimension);
		BlockSchematic blockSchematic = new BlockSchematic();
		blockSchematic.OmitLiquids = notLiquids;
		blockSchematic.AddArea(sapi.World, start, end);
		blockSchematic.Pack(sapi.World, startPos);
		return blockSchematic;
	}

	public void PasteBlockData(BlockSchematic blockData, BlockPos startPos, EnumOrigin origin, IBlockAccessor blockAccessor = null)
	{
		BlockPos originPos = blockData.GetStartPos(startPos, origin);
		EnumAxis? axis = null;
		if (ImportFlipped)
		{
			axis = EnumAxis.Y;
		}
		if (blockAccessor == null)
		{
			blockAccessor = revertableBlockAccess;
		}
		BlockSchematic blockSchematic = blockData.ClonePacked();
		blockSchematic.TransformWhilePacked(sapi.World, origin, ImportAngle, axis);
		blockSchematic.Init(blockAccessor);
		EnumReplaceMode enumReplaceMode = EnumReplaceMode.ReplaceAll;
		if (ToolInstance is ImportTool importTool)
		{
			enumReplaceMode = importTool.ReplaceMode;
		}
		blockSchematic.Place(blockAccessor, sapi.World, originPos, enumReplaceMode, WorldEdit.ReplaceMetaBlocks);
		blockSchematic.PlaceDecors(blockAccessor, originPos);
		if (blockAccessor is IBlockAccessorRevertable revertable)
		{
			revertable.Commit();
			blockData.PlaceEntitiesAndBlockEntities(revertable, sapi.World, originPos, blockData.BlockCodes, blockData.ItemCodes, replaceBlockEntities: false, null, 0, null, WorldEdit.ReplaceMetaBlocks);
			revertable.CommitBlockEntityData();
		}
	}

	public TextCommandResult ImportArea(string filename, BlockPos startPos, EnumOrigin origin, bool isLarge)
	{
		string infilepath = Path.Combine(WorldEdit.ExportFolderPath, filename);
		string error = "";
		BlockSchematic blockData = BlockSchematic.LoadFromFile(infilepath, ref error);
		if (blockData == null)
		{
			return TextCommandResult.Error(error);
		}
		if (blockData.SizeX >= 1024 || blockData.SizeY >= 1024 || blockData.SizeZ >= 1024)
		{
			return TextCommandResult.Error("Can not load schematics larger than 1024x1024x1024");
		}
		IBlockAccessor worldBlockAccessor = (isLarge ? sapi.World.BlockAccessor : null);
		PasteBlockData(blockData, startPos, origin, worldBlockAccessor);
		return TextCommandResult.Success(filename + " imported.");
	}

	public TextCommandResult ExportArea(string filename, BlockPos start, BlockPos end, IServerPlayer sendToPlayer = null)
	{
		BlockSchematic blockdata = CopyArea(start, end);
		int exported = blockdata.BlockIds.Count;
		int exportedEntities = blockdata.Entities.Count;
		string outfilepath = Path.Combine(WorldEdit.ExportFolderPath, filename);
		if (sendToPlayer != null)
		{
			sapi.Network.GetChannel("worldedit").SendPacket(new SchematicJsonPacket
			{
				Filename = filename,
				JsonCode = blockdata.ToJson()
			}, sendToPlayer);
			return TextCommandResult.Success(exported + " blocks schematic sent to client.");
		}
		string error = blockdata.Save(outfilepath);
		if (error != null)
		{
			return TextCommandResult.Success("Failed exporting: " + error);
		}
		return TextCommandResult.Success(exported + " blocks and " + exportedEntities + " Entities exported");
	}

	public virtual void CreatePreview(BlockSchematic schematic, BlockPos origin)
	{
		EnumOrigin originPrev = ((ToolInstance is ImportTool its) ? its.Origin : EnumOrigin.StartPos);
		IMiniDimension dim = CreateDimensionFromSchematic(schematic, origin, originPrev);
		previewBlocks.UnloadUnusedServerChunks();
		SendPreviewOriginToClient(origin, dim.subDimensionId);
	}

	public IMiniDimension CreateDimensionFromSchematic(BlockSchematic blockData, BlockPos startPos, EnumOrigin origin)
	{
		BlockPos originPos = startPos.Copy();
		if (previewBlocks == null)
		{
			previewBlocks = revertableBlockAccess.CreateMiniDimension(new Vec3d(originPos.X, originPos.Y, originPos.Z));
			sapi.Server.SetMiniDimension(previewBlocks, DimensionId);
		}
		else
		{
			previewBlocks.ClearChunks();
			previewBlocks.CurrentPos.SetPos(originPos);
		}
		previewBlocks.SetSubDimensionId(DimensionId);
		previewBlocks.BlocksPreviewSubDimension_Server = DimensionId;
		originPos.Sub(startPos);
		originPos.SetDimension(1);
		previewBlocks.AdjustPosForSubDimension(originPos);
		EnumAxis? axis = (ImportFlipped ? new EnumAxis?(EnumAxis.Y) : ((EnumAxis?)null));
		BlockSchematic blockSchematic = blockData.ClonePacked();
		blockSchematic.TransformWhilePacked(world, origin, ImportAngle, axis);
		blockSchematic.PasteToMiniDimension(sapi, revertableBlockAccess, previewBlocks, originPos, WorldEdit.ReplaceMetaBlocks);
		return previewBlocks;
	}

	public void DestroyPreview()
	{
		previewBlocks.ClearChunks();
		previewBlocks.UnloadUnusedServerChunks();
		SendPreviewOriginToClient(previewBlocks.selectionTrackingOriginalPos, -1);
	}

	public void SendPreviewOriginToClient(BlockPos origin, int dim, bool trackSelection = false)
	{
		IServerNetworkChannel channel = sapi.Network.GetChannel("worldedit");
		IServerPlayer player = (IServerPlayer)world.PlayerByUid(PlayerUID);
		channel.SendPacket(new PreviewBlocksPacket
		{
			pos = origin,
			dimId = dim,
			TrackSelection = trackSelection
		}, player);
	}

	public TextCommandResult SetStartPos(Vec3d pos, bool update = true)
	{
		StartMarkerExact = pos.Clone();
		if (update)
		{
			UpdateSelection();
		}
		else
		{
			StartMarker = pos.AsBlockPos;
		}
		return TextCommandResult.Success("Start position " + StartMarker?.ToString() + " marked");
	}

	public TextCommandResult SetEndPos(Vec3d pos)
	{
		EndMarkerExact = pos.Clone();
		UpdateSelection();
		return TextCommandResult.Success("End position " + EndMarker?.ToString() + " marked");
	}

	public TextCommandResult ModifyMarker(BlockFacing facing, int amount, bool quiet = false)
	{
		if (StartMarker == null || EndMarker == null)
		{
			return TextCommandResult.Error("Start marker or end marker not set");
		}
		GrowSelection(facing, amount);
		if (!quiet)
		{
			return TextCommandResult.Success($"Area grown by {amount} blocks towards {facing}");
		}
		return TextCommandResult.Success();
	}

	public TextCommandResult HandleRotateCommand(int angle)
	{
		if (StartMarker == null || EndMarker == null)
		{
			return TextCommandResult.Error("Start marker or end marker not set");
		}
		EnumOrigin origin = EnumOrigin.BottomCenter;
		BlockPos mid = (StartMarker + EndMarker) / 2;
		mid.Y = StartMarker.Y;
		BlockSchematic schematic = CopyArea(StartMarker, EndMarker);
		revertableBlockAccess.BeginMultiEdit();
		schematic.TransformWhilePacked(sapi.World, origin, angle);
		FillArea(null, StartMarker, EndMarker);
		PasteBlockData(schematic, mid, origin);
		StartMarker = schematic.GetStartPos(mid, origin);
		EndMarker = StartMarker.AddCopy(schematic.SizeX, schematic.SizeY, schematic.SizeZ);
		revertableBlockAccess.EndMultiEdit();
		HighlightSelectedArea();
		return TextCommandResult.Success($"Selection rotated by {angle} degrees.");
	}

	public TextCommandResult HandleFlipCommand(EnumAxis axis)
	{
		if (StartMarker == null || EndMarker == null)
		{
			return TextCommandResult.Error("Start marker or end marker not set");
		}
		EnumOrigin origin = EnumOrigin.BottomCenter;
		BlockPos mid = (StartMarker + EndMarker) / 2;
		mid.Y = StartMarker.Y;
		BlockSchematic schematic = CopyArea(StartMarker, EndMarker);
		revertableBlockAccess.BeginMultiEdit();
		schematic.TransformWhilePacked(sapi.World, origin, 0, axis);
		FillArea(null, StartMarker, EndMarker);
		PasteBlockData(schematic, mid, origin);
		StartMarker = schematic.GetStartPos(mid, origin);
		EndMarker = StartMarker.AddCopy(schematic.SizeX, schematic.SizeY, schematic.SizeZ);
		revertableBlockAccess.EndMultiEdit();
		HighlightSelectedArea();
		return TextCommandResult.Success($"Selection flipped in {axis} axis.");
	}

	public TextCommandResult HandleRepeatCommand(Vec3i dir, int repeats, string selectMode)
	{
		if (StartMarker == null || EndMarker == null)
		{
			return TextCommandResult.Error("Start marker or end marker not set");
		}
		bool selectNewArea = selectMode == "sn";
		bool growNewArea = selectMode == "gn";
		BlockPos startPos = GetMarkedMinPos();
		BlockPos endPos = GetMarkedMaxPos();
		return RepeatArea(startPos, endPos, dir, repeats, selectNewArea, growNewArea);
	}

	public TextCommandResult RepeatArea(BlockPos startPos, BlockPos endPos, Vec3i dir, int repeats, bool selectNewArea, bool growNewArea)
	{
		if (StartMarker == null || EndMarker == null)
		{
			return TextCommandResult.Error("Start marker or end marker not set");
		}
		int curRepeats = 0;
		Dictionary<BlockPos, TreeAttribute> blockEntityData = new Dictionary<BlockPos, TreeAttribute>();
		BlockPos pos = new BlockPos(startPos.dimension);
		BlockPos offset = null;
		while (curRepeats++ < repeats)
		{
			BlockPos curPos = startPos.Copy();
			offset = new BlockPos((endPos.X - startPos.X) * dir.X * curRepeats, (endPos.Y - startPos.Y) * dir.Y * curRepeats, (endPos.Z - startPos.Z) * dir.Z * curRepeats);
			while (curPos.X < endPos.X)
			{
				curPos.Y = startPos.Y;
				while (curPos.Y < endPos.Y)
				{
					curPos.Z = startPos.Z;
					while (curPos.Z < endPos.Z)
					{
						Block block = revertableBlockAccess.GetBlock(curPos);
						Block blockFluid = revertableBlockAccess.GetBlock(curPos, 2);
						Dictionary<int, Block> decors = revertableBlockAccess.GetSubDecors(curPos);
						if (block.EntityClass != null)
						{
							TreeAttribute tree = new TreeAttribute();
							revertableBlockAccess.GetBlockEntity(curPos)?.ToTreeAttributes(tree);
							blockEntityData[curPos + offset] = tree;
						}
						pos.Set(curPos.X + offset.X, curPos.Y + offset.Y, curPos.Z + offset.Z);
						revertableBlockAccess.SetBlock(block.Id, pos);
						if (blockFluid != null)
						{
							revertableBlockAccess.SetBlock(blockFluid.Id, pos, 2);
						}
						if (decors != null)
						{
							foreach (KeyValuePair<int, Block> entry in decors)
							{
								if (entry.Value != null)
								{
									revertableBlockAccess.SetDecor(entry.Value, pos, entry.Key);
								}
							}
						}
						curPos.Z++;
					}
					curPos.Y++;
				}
				curPos.X++;
			}
		}
		BlockPos originalStart = startPos.Copy();
		if (selectNewArea)
		{
			StartMarker.Add(offset);
			EndMarker.Add(offset);
			StartMarkerExact = StartMarker.ToVec3d().Add(0.5);
			EndMarkerExact = EndMarker.ToVec3d().Add(-1.0);
			ResendBlockHighlights();
		}
		if (growNewArea)
		{
			StartMarker.Set(startPos.X + ((offset.X < 0) ? offset.X : 0), startPos.Y + ((offset.Y < 0) ? offset.Y : 0), startPos.Z + ((offset.Z < 0) ? offset.Z : 0));
			EndMarker.Set(endPos.X + ((offset.X > 0) ? offset.X : 0), endPos.Y + ((offset.Y > 0) ? offset.Y : 0), endPos.Z + ((offset.Z > 0) ? offset.Z : 0));
			StartMarkerExact = StartMarker.ToVec3d().Add(0.5);
			EndMarkerExact = EndMarker.ToVec3d().Add(-1.0);
			ResendBlockHighlights();
		}
		revertableBlockAccess.Commit();
		curRepeats = 0;
		while (curRepeats++ < repeats)
		{
			EntityPos curPos2 = new EntityPos(originalStart.X, originalStart.Y, originalStart.Z);
			offset = new BlockPos((endPos.X - originalStart.X) * dir.X * curRepeats, (endPos.Y - originalStart.Y) * dir.Y * curRepeats, (endPos.Z - originalStart.Z) * dir.Z * curRepeats);
			EntityPos entityPos = new EntityPos();
			Entity[] entitiesInsideCuboid = world.GetEntitiesInsideCuboid(originalStart, endPos, (Entity e) => !(e is EntityPlayer));
			foreach (Entity entity in entitiesInsideCuboid)
			{
				curPos2.SetPos(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
				entityPos.SetPos(curPos2.X + (double)offset.X, curPos2.Y + (double)offset.Y, curPos2.Z + (double)offset.Z);
				Entity newEntity = world.ClassRegistry.CreateEntity(entity.Properties);
				newEntity.DidImportOrExport(entityPos.AsBlockPos);
				newEntity.Pos.SetPos(entityPos);
				newEntity.Pos.SetAngles(entity.Pos);
				world.SpawnEntity(newEntity);
				revertableBlockAccess.StoreEntitySpawnToHistory(newEntity);
			}
		}
		foreach (KeyValuePair<BlockPos, TreeAttribute> val in blockEntityData)
		{
			BlockEntity blockEntity = revertableBlockAccess.GetBlockEntity(val.Key);
			val.Value.SetInt("posx", val.Key.X);
			val.Value.SetInt("posy", val.Key.Y);
			val.Value.SetInt("posz", val.Key.Z);
			blockEntity?.FromTreeAttributes(val.Value, world);
		}
		return TextCommandResult.Success("Marked area repeated " + repeats + ((repeats > 1) ? " times" : " time"));
	}

	public TextCommandResult HandleMirrorCommand(BlockFacing face, string selectMode)
	{
		if (StartMarker == null || EndMarker == null)
		{
			return TextCommandResult.Error("Start marker or end marker not set");
		}
		bool selectNewArea = selectMode == "sn";
		bool growNewArea = selectMode == "gn";
		BlockPos startPos = GetMarkedMinPos();
		BlockPos endPos = GetMarkedMaxPos();
		MirrorArea(startPos, endPos, face, selectNewArea, growNewArea);
		return TextCommandResult.Success("Marked area mirrored " + face.Code);
	}

	public void MirrorArea(BlockPos startPos, BlockPos endPos, BlockFacing dir, bool selectNewArea, bool growNewArea)
	{
		BlockPos curPos = startPos.Copy();
		BlockPos offset = new BlockPos((endPos.X - startPos.X) * dir.Normali.X, (endPos.Y - startPos.Y) * dir.Normali.Y, (endPos.Z - startPos.Z) * dir.Normali.Z);
		BlockPos pos = new BlockPos(startPos.dimension);
		Dictionary<BlockPos, ITreeAttribute> blockEntityData = new Dictionary<BlockPos, ITreeAttribute>();
		while (curPos.X < endPos.X)
		{
			curPos.Y = startPos.Y;
			while (curPos.Y < endPos.Y)
			{
				curPos.Z = startPos.Z;
				while (curPos.Z < endPos.Z)
				{
					int blockId = revertableBlockAccess.GetBlockId(curPos);
					Dictionary<int, Block> decors = revertableBlockAccess.GetSubDecors(curPos);
					Block block = ((dir.Axis != EnumAxis.Y) ? sapi.World.GetBlock(sapi.World.Blocks[blockId].GetHorizontallyFlippedBlockCode(dir.Axis)) : sapi.World.GetBlock(sapi.World.Blocks[blockId].GetVerticallyFlippedBlockCode()));
					int mX = ((dir.Axis == EnumAxis.X) ? (startPos.X + (endPos.X - curPos.X) - 1) : curPos.X);
					int mY = ((dir.Axis == EnumAxis.Y) ? (startPos.Y + (endPos.Y - curPos.Y) - 1) : curPos.Y);
					int mZ = ((dir.Axis == EnumAxis.Z) ? (startPos.Z + (endPos.Z - curPos.Z) - 1) : curPos.Z);
					pos.Set(mX + offset.X, mY + offset.Y, mZ + offset.Z);
					BlockEntity be = revertableBlockAccess.GetBlockEntity(curPos);
					if (be != null)
					{
						TreeAttribute tree = new TreeAttribute();
						be.ToTreeAttributes(tree);
						blockEntityData[pos.Copy()] = tree;
					}
					revertableBlockAccess.SetBlock(block.BlockId, pos);
					if (decors != null)
					{
						foreach (KeyValuePair<int, Block> entry in decors)
						{
							if (entry.Value != null)
							{
								BlockFacing blockFacing = BlockFacing.ALLFACES[entry.Key % 6];
								if (blockFacing.Axis == dir.Axis)
								{
									revertableBlockAccess.SetDecor(entry.Value, pos, entry.Key / 6 * 6 + blockFacing.Opposite.Index);
								}
								else
								{
									revertableBlockAccess.SetDecor(entry.Value, pos, entry.Key);
								}
							}
						}
					}
					curPos.Z++;
				}
				curPos.Y++;
			}
			curPos.X++;
		}
		if (selectNewArea)
		{
			StartMarker.Add(offset);
			EndMarker.Add(offset);
			StartMarkerExact = StartMarker.ToVec3d().Add(0.5);
			EndMarkerExact = EndMarker.ToVec3d().Add(-1.0);
			ResendBlockHighlights();
		}
		BlockPos startOriginal = startPos.Copy();
		BlockPos endOriginal = endPos.Copy();
		if (growNewArea)
		{
			StartMarker.Set(startPos.X + ((offset.X < 0) ? offset.X : 0), startPos.Y + ((offset.Y < 0) ? offset.Y : 0), startPos.Z + ((offset.Z < 0) ? offset.Z : 0));
			EndMarker.Set(endPos.X + ((offset.X > 0) ? offset.X : 0), endPos.Y + ((offset.Y > 0) ? offset.Y : 0), endPos.Z + ((offset.Z > 0) ? offset.Z : 0));
			StartMarkerExact = StartMarker.ToVec3d().Add(0.5);
			EndMarkerExact = EndMarker.ToVec3d().Add(-1.0);
			ResendBlockHighlights();
		}
		revertableBlockAccess.Commit();
		EntityPos curPosE = new EntityPos();
		EntityPos posE = new EntityPos();
		Entity[] entitiesInsideCuboid = world.GetEntitiesInsideCuboid(startOriginal, endOriginal, (Entity e) => !(e is EntityPlayer));
		foreach (Entity entity in entitiesInsideCuboid)
		{
			curPosE.SetPos(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
			double mX2 = ((dir.Axis == EnumAxis.X) ? ((double)startOriginal.X + ((double)endOriginal.X - curPosE.X)) : curPosE.X);
			double mY2 = ((dir.Axis == EnumAxis.Y) ? ((double)startOriginal.Y + ((double)endOriginal.Y - curPosE.Y)) : curPosE.Y);
			double mZ2 = ((dir.Axis == EnumAxis.Z) ? ((double)startOriginal.Z + ((double)endOriginal.Z - curPosE.Z)) : curPosE.Z);
			posE.SetPos(mX2 + (double)offset.X, mY2 + (double)offset.Y, mZ2 + (double)offset.Z);
			Entity newEntity = world.ClassRegistry.CreateEntity(entity.Properties);
			newEntity.DidImportOrExport(posE.AsBlockPos.Copy());
			newEntity.Pos.SetPos(posE);
			newEntity.Pos.SetAngles(entity.Pos);
			world.SpawnEntity(newEntity);
			revertableBlockAccess.StoreEntitySpawnToHistory(newEntity);
		}
		foreach (KeyValuePair<BlockPos, ITreeAttribute> val in blockEntityData)
		{
			BlockEntity be2 = revertableBlockAccess.GetBlockEntity(val.Key);
			if (be2 != null)
			{
				ITreeAttribute tree2 = val.Value;
				tree2.SetInt("posx", val.Key.X);
				tree2.SetInt("posy", val.Key.Y);
				tree2.SetInt("posz", val.Key.Z);
				if (be2 is IRotatable rotatable)
				{
					Dictionary<int, AssetLocation> empty = new Dictionary<int, AssetLocation>();
					rotatable.OnTransformed(sapi.World, tree2, 0, empty, empty, dir.Axis);
				}
				be2.FromTreeAttributes(tree2, sapi.World);
			}
		}
	}

	public TextCommandResult HandleMoveCommand(Vec3i offset, bool quiet = false)
	{
		if (PreviewPos == null)
		{
			if (StartMarker == null || EndMarker == null)
			{
				return TextCommandResult.Success("You need to have at least an active selection");
			}
			PreviewPos = StartMarker.Copy();
			PreviewBlockData = CopyArea(StartMarker, EndMarker);
			CreatePreview(PreviewBlockData, PreviewPos);
		}
		PreviewPos.Add(offset);
		SendPreviewOriginToClient(PreviewPos, previewBlocks.subDimensionId);
		if (!quiet)
		{
			return TextCommandResult.Success("Moved marked area by x/y/z = " + offset.X + "/" + offset.Y + "/" + offset.Z);
		}
		return TextCommandResult.Success();
	}

	public TextCommandResult HandleShiftCommand(Vec3i dir, bool quiet = false)
	{
		if (StartMarker == null || EndMarker == null)
		{
			return TextCommandResult.Error("Start marker or end marker not set");
		}
		StartMarkerExact.Add(dir.X, dir.Y, dir.Z);
		EndMarkerExact.Add(dir.X, dir.Y, dir.Z);
		UpdateSelection();
		if (!quiet)
		{
			return TextCommandResult.Success("Shifted marked area by x/y/z = " + dir.X + "/" + dir.Y + "/" + dir.Z);
		}
		return TextCommandResult.Success();
	}
}
