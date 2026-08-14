using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class BlockBehaviorFiniteSpreadingLiquid(Block block) : BlockBehavior(block)
{
	public class PathPos : BlockPos
	{
		public BlockPos PrevPos;

		public BlockPos PositionCopy => new BlockPos(X, Y, Z);

		public PathPos(int x, int y, int z)
			: base(x, y, z)
		{
		}

		public PathPos(BlockPos pos)
			: base(pos.X, pos.Y, pos.Z)
		{
		}

		public override PathPos Copy()
		{
			return new PathPos(X, Y, Z)
			{
				PrevPos = PrevPos
			};
		}
	}

	private const int MAXLEVEL = 7;

	private const float MAXLEVEL_float = 7f;

	public static Vec2i[] downPaths = ShapeUtil.GetSquarePointsSortedByMDist(3);

	public static SimpleParticleProperties steamParticles;

	public static int ReplacableThreshold = 5000;

	[DocumentAsJson]
	protected AssetLocation collisionReplaceSound;

	[DocumentAsJson("Recommended", "150", false)]
	protected int spreadDelay = 150;

	[DocumentAsJson("Optional", "None", false)]
	protected string collidesWith;

	[DocumentAsJson("Optional", "None", false)]
	protected AssetLocation liquidSourceCollisionReplacement;

	[DocumentAsJson("Optional", "None", false)]
	protected AssetLocation liquidFlowingCollisionReplacement;

	[DocumentAsJson("Optional", "None", false)]
	public bool multiplySpread = true;

	protected bool carver;

	[ThreadStatic]
	protected static IBlockAccessor bulkBlockAccessorMinimal;

	protected static readonly byte[] ShuffleTable = new byte[24]
	{
		228, 180, 216, 120, 156, 108, 225, 177, 201, 57,
		141, 45, 210, 114, 198, 54, 78, 30, 147, 99,
		135, 39, 75, 27
	};

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		spreadDelay = properties["spreadDelay"].AsInt();
		collisionReplaceSound = CreateAssetLocation(properties, "sounds/", "liquidCollisionSound");
		liquidSourceCollisionReplacement = CreateAssetLocation(properties, "sourceReplacementCode");
		liquidFlowingCollisionReplacement = CreateAssetLocation(properties, "flowingReplacementCode");
		collidesWith = properties["collidesWith"]?.AsString();
		multiplySpread = properties["multiplySpread"].AsBool(defaultValue: true);
		carver = properties["carver"].AsBool();
	}

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack, ref EnumHandling handling)
	{
		if (world is IServerWorldAccessor)
		{
			world.RegisterCallbackUnique(OnDelayedWaterUpdateCheck, blockSel.Position, spreadDelay);
		}
		return base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack, ref handling);
	}

	public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		if (world is IServerWorldAccessor)
		{
			world.RegisterCallbackUnique(OnDelayedWaterUpdateCheck, pos, spreadDelay);
		}
	}

	private void OnDelayedWaterUpdateCheck(IWorldAccessor world, BlockPos pos, float dt)
	{
		if (carver)
		{
			if (world.Side == EnumAppSide.Server)
			{
				if (bulkBlockAccessorMinimal == null)
				{
					bulkBlockAccessorMinimal = world.GetBlockAccessorBulkMinimalUpdate(synchronize: true);
				}
				Block waterblock = world.GetBlock(AssetLocation.Create(base.block.Variant["type"] + "-still-7", base.block.Code.Domain));
				bulkBlockAccessorMinimal.SetBlock(waterblock.Id, pos, 2);
				waterblock.GetBehavior<BlockBehaviorFiniteSpreadingLiquid>().carveRivulet(world, bulkBlockAccessorMinimal, pos);
				bulkBlockAccessorMinimal.Commit();
			}
			return;
		}
		SpreadAndUpdateLiquidLevels(world, pos);
		world.BulkBlockAccessor.Commit();
		Block block = world.BlockAccessor.GetBlock(pos, 2);
		if (block.HasBehavior<BlockBehaviorFiniteSpreadingLiquid>())
		{
			updateOwnFlowDir(block, world, pos);
		}
		BlockPos npos = pos.Copy();
		Cardinal[] aLL = Cardinal.ALL;
		foreach (Cardinal val in aLL)
		{
			npos.Set(pos.X + val.Normali.X, pos.Y, pos.Z + val.Normali.Z);
			Block neib = world.BlockAccessor.GetBlock(npos, 2);
			if (neib.HasBehavior<BlockBehaviorFiniteSpreadingLiquid>())
			{
				updateOwnFlowDir(neib, world, npos);
			}
		}
	}

	protected PathPos findCarvePath(IWorldAccessor world, BlockPos startPos, Block ourBlock)
	{
		Queue<PathPos> uncheckedPositions = new Queue<PathPos>();
		HashSet<PathPos> checkedPositions = new HashSet<PathPos>();
		uncheckedPositions.Enqueue(new PathPos(startPos.X, startPos.Y, startPos.Z));
		BlockPos npos = new BlockPos(startPos.dimension);
		int maxDist = 7;
		while (uncheckedPositions.Count > 0)
		{
			PathPos pos = uncheckedPositions.Dequeue();
			checkedPositions.Add(pos);
			int dist = pos.ManhattanDistance(startPos);
			if (dist > maxDist)
			{
				continue;
			}
			npos.Set(pos);
			for (int i = 0; i < BlockFacing.HORIZONTALS.Length; i++)
			{
				BlockFacing.HORIZONTALS[i].IterateThruFacingOffsets(npos);
				if (world.BlockAccessor.GetMostSolidBlock(npos).GetLiquidBarrierHeightOnSide(BlockFacing.HORIZONTALS[i].Opposite, npos) >= (float)(ourBlock.LiquidLevel - pos.ManhattanDistance(startPos)) / 7f)
				{
					continue;
				}
				PathPos ppos = new PathPos(npos)
				{
					PrevPos = pos.Copy()
				};
				if (!(world as IServerWorldAccessor).IsFullyLoadedChunk(pos))
				{
					return null;
				}
				if (dist <= 0 || !world.BlockAccessor.GetMostSolidBlock(npos.UpCopy()).SideSolid.Any)
				{
					if (!world.BlockAccessor.GetMostSolidBlock(npos.DownCopy()).SideSolid.Any)
					{
						return ppos;
					}
					if (!checkedPositions.Contains(npos))
					{
						uncheckedPositions.Enqueue(ppos);
					}
				}
			}
		}
		return null;
	}

	private void carveRivulet(IWorldAccessor world, IBlockAccessor blockAccessor, BlockPos pos)
	{
		if (block.LiquidLevel == 0)
		{
			return;
		}
		PathPos pathpos = findCarvePath(world, pos, block);
		if (pathpos == null)
		{
			return;
		}
		int num = 1;
		List<BlockPos> list = new List<BlockPos>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<BlockPos> span = CollectionsMarshal.AsSpan(list);
		int index = 0;
		span[index] = pathpos;
		List<BlockPos> path = list;
		while (pathpos.PrevPos != null)
		{
			pathpos = (PathPos)pathpos.PrevPos;
			path.Add(pathpos);
		}
		path.Reverse();
		Block lblock = block;
		for (int i = 1; i < path.Count; i++)
		{
			pos = path[i];
			if (i == 1 && !placeRiver(world, blockAccessor, pos, ref lblock))
			{
				return;
			}
			pos.Down();
			Block belowBlock = world.BlockAccessor.GetBlock(pos);
			while (pos.Y > 0 && !belowBlock.SideSolid.Any)
			{
				blockAccessor.SetBlock(0, pos);
				blockAccessor.SetBlock(world.GetBlock(block.CodeWithParts("d", 6.ToString() ?? "")).BlockId, pos, 2);
				belowBlock = world.BlockAccessor.GetBlock(pos.Down());
			}
			lblock = world.Blocks[GetLiquidBlockId(world, pos, lblock, 6)];
			if (!placeRiver(world, blockAccessor, pos, ref lblock))
			{
				return;
			}
		}
		lblock.GetBehavior<BlockBehaviorFiniteSpreadingLiquid>().carveRivulet(world, blockAccessor, pos);
	}

	private bool placeRiver(IWorldAccessor world, IBlockAccessor blockAccessor, BlockPos pos, ref Block lblock)
	{
		blockAccessor.SetBlock(0, pos);
		blockAccessor.SetBlock(0, pos.UpCopy());
		BlockPos belowPos = pos.DownCopy();
		Block belowBlock = world.BlockAccessor.GetBlock(belowPos);
		while (belowPos.Y > 0 && !belowBlock.SideSolid.Any)
		{
			belowBlock = world.BlockAccessor.GetBlock(belowPos.Down());
		}
		blockAccessor.SetBlock(world.GetBlock(new AssetLocation("muddygravel")).Id, belowPos);
		lblock = world.Blocks[GetLessLiquidBlockId(world, pos, lblock)];
		if (lblock.Id == 0)
		{
			return false;
		}
		blockAccessor.SetBlock(lblock.Id, pos, 2);
		return true;
	}

	private void SpreadAndUpdateLiquidLevels(IWorldAccessor world, BlockPos pos)
	{
		Block ourBlock = world.BlockAccessor.GetBlock(pos, 2);
		int liquidLevel = ourBlock.LiquidLevel;
		if (liquidLevel <= 0 || TryLoweringLiquidLevel(ourBlock, world, pos))
		{
			return;
		}
		BlockPos downPos = pos.DownCopy();
		Block mostSolidBlock = world.BlockAccessor.GetMostSolidBlock(downPos);
		Block ourSolid = world.BlockAccessor.GetBlock(pos, 1);
		if (((double)mostSolidBlock.GetLiquidBarrierHeightOnSide(BlockFacing.UP, downPos) != 1.0 && (double)ourSolid.GetLiquidBarrierHeightOnSide(BlockFacing.DOWN, pos) != 1.0 && TrySpreadDownwards(world, ourSolid, ourBlock, pos)) || liquidLevel <= 1)
		{
			return;
		}
		List<PosAndDist> downwardPaths = FindDownwardPaths(world, pos, ourBlock);
		if (downwardPaths.Count > 0)
		{
			FlowTowardDownwardPaths(downwardPaths, ourBlock, ourSolid, pos, world);
			return;
		}
		TrySpreadHorizontal(ourBlock, ourSolid, world, pos);
		if (IsLiquidSourceBlock(ourBlock))
		{
			return;
		}
		int nearbySourceBlockCount = CountNearbySourceBlocks(world.BlockAccessor, pos, ourBlock);
		if (nearbySourceBlockCount < 3 && (nearbySourceBlockCount != 2 || CountNearbyDiagonalSources(world.BlockAccessor, pos, ourBlock) < 3))
		{
			return;
		}
		world.BlockAccessor.SetBlock(GetMoreLiquidBlockId(world, pos, ourBlock), pos, 2);
		BlockPos npos = pos.Copy();
		for (int i = 0; i < BlockFacing.HORIZONTALS.Length; i++)
		{
			BlockFacing.HORIZONTALS[i].IterateThruFacingOffsets(npos);
			Block nblock = world.BlockAccessor.GetBlock(npos, 2);
			if (nblock.HasBehavior<BlockBehaviorFiniteSpreadingLiquid>())
			{
				updateOwnFlowDir(nblock, world, npos);
			}
		}
	}

	private int CountNearbySourceBlocks(IBlockAccessor blockAccessor, BlockPos pos, Block ourBlock)
	{
		BlockPos qpos = pos.Copy();
		int nearbySourceBlockCount = 0;
		for (int i = 0; i < BlockFacing.HORIZONTALS.Length; i++)
		{
			BlockFacing.HORIZONTALS[i].IterateThruFacingOffsets(qpos);
			Block nblock = blockAccessor.GetBlock(qpos, 2);
			if (IsSameLiquid(ourBlock, nblock) && IsLiquidSourceBlock(nblock))
			{
				nearbySourceBlockCount++;
			}
		}
		return nearbySourceBlockCount;
	}

	private int CountNearbyDiagonalSources(IBlockAccessor blockAccessor, BlockPos pos, Block ourBlock)
	{
		BlockPos npos = pos.Copy();
		int nearbySourceBlockCount = 0;
		Cardinal[] aLL = Cardinal.ALL;
		foreach (Cardinal val in aLL)
		{
			if (val.IsDiagonal)
			{
				npos.Set(pos.X + val.Normali.X, pos.Y, pos.Z + val.Normali.Z);
				Block nblock = blockAccessor.GetBlock(npos, 2);
				if (IsSameLiquid(ourBlock, nblock) && IsLiquidSourceBlock(nblock))
				{
					nearbySourceBlockCount++;
				}
			}
		}
		return nearbySourceBlockCount;
	}

	private void FlowTowardDownwardPaths(List<PosAndDist> downwardPaths, Block liquidBlock, Block solidBlock, BlockPos pos, IWorldAccessor world)
	{
		if (!multiplySpread)
		{
			GameMath.Shuffle(world.Rand, downwardPaths);
		}
		foreach (PosAndDist pod in downwardPaths)
		{
			BlockFacing pathFacing = pod.pos.FacingFrom(pos);
			if (CanSpreadIntoBlock(liquidBlock, solidBlock, pos, pod.pos, pathFacing, world))
			{
				Block neighborLiquid = world.BlockAccessor.GetBlock(pod.pos, 2);
				if (CollidesWith(liquidBlock, neighborLiquid))
				{
					ReplaceLiquidBlock(neighborLiquid, pod.pos, world);
				}
				else
				{
					SpreadLiquid(GetLessLiquidBlockId(world, pod.pos, liquidBlock), pod.pos, world);
				}
				if (!multiplySpread)
				{
					RemoveOtherLowerNeighbours(liquidBlock, pos, pathFacing, world);
					break;
				}
			}
		}
	}

	private void RemoveOtherLowerNeighbours(Block liquidBlock, BlockPos pos, BlockFacing pathFacing, IWorldAccessor world)
	{
		BlockPos origPos = pos.Copy();
		for (int i = 0; i < 4; i++)
		{
			pos.IterateHorizontalOffsets(i);
			if (BlockFacing.HORIZONTALS[i] != pathFacing)
			{
				Block neighborLiquid = world.BlockAccessor.GetBlock(pos, 2);
				if (neighborLiquid.IsLiquid() && (neighborLiquid.LiquidLevel < liquidBlock.LiquidLevel || (pathFacing == BlockFacing.DOWN && neighborLiquid.LiquidLevel == liquidBlock.LiquidLevel)) && IsSameLiquid(liquidBlock, neighborLiquid))
				{
					world.BulkBlockAccessor.SetBlock(0, pos, 2);
					UpdateNeighbouringLiquids(pos, world, origPos, liquidBlock);
				}
			}
		}
		pos.East();
	}

	private bool TrySpreadDownwards(IWorldAccessor world, Block ourSolid, Block ourBlock, BlockPos pos)
	{
		BlockPos npos = pos.DownCopy();
		Block belowLiquid = world.BlockAccessor.GetBlock(npos, 2);
		if (CanSpreadIntoBlock(ourBlock, ourSolid, pos, npos, BlockFacing.DOWN, world))
		{
			if (CollidesWith(ourBlock, belowLiquid))
			{
				ReplaceLiquidBlock(belowLiquid, npos, world);
				TryFindSourceAndSpread(npos, world);
			}
			else
			{
				bool fillWithSource = false;
				if (IsLiquidSourceBlock(ourBlock))
				{
					if (CountNearbySourceBlocks(world.BlockAccessor, npos, ourBlock) > 1)
					{
						fillWithSource = true;
					}
					else
					{
						npos.Down();
						if ((double)world.BlockAccessor.GetBlock(npos, 4).GetLiquidBarrierHeightOnSide(BlockFacing.UP, npos) == 1.0 || (double)ourSolid.GetLiquidBarrierHeightOnSide(BlockFacing.DOWN, pos) == 1.0 || IsLiquidSourceBlock(world.BlockAccessor.GetBlock(npos, 2)))
						{
							fillWithSource = CountNearbySourceBlocks(world.BlockAccessor, pos, ourBlock) >= 2;
						}
						npos.Up();
					}
				}
				SpreadLiquid(fillWithSource ? ourBlock.BlockId : GetFallingLiquidBlockId(ourBlock, world), npos, world);
				if (!multiplySpread)
				{
					RemoveOtherLowerNeighbours(ourBlock, pos, BlockFacing.DOWN, world);
				}
			}
			return true;
		}
		if (IsLiquidSourceBlock(ourBlock))
		{
			return !IsLiquidSourceBlock(belowLiquid);
		}
		return true;
	}

	private void TrySpreadHorizontal(Block ourblock, Block ourSolid, IWorldAccessor world, BlockPos pos)
	{
		if (!multiplySpread)
		{
			BlockFacing foundPathFacing = null;
			byte shuffledIndices = ShuffleTable[GameMath.MurmurHash3Mod(pos.X, pos.Y, pos.Z, ShuffleTable.Length)];
			for (int i = 0; i < 4; i++)
			{
				BlockFacing facing = BlockFacing.HORIZONTALS[(shuffledIndices >> i * 2) & 3];
				if (TrySpreadIntoBlock(ourblock, ourSolid, pos, pos.AddCopy(facing), facing, world))
				{
					foundPathFacing = facing;
					break;
				}
			}
			if (foundPathFacing != null)
			{
				RemoveOtherLowerNeighbours(ourblock, pos, foundPathFacing, world);
			}
		}
		else
		{
			BlockFacing[] hORIZONTALS = BlockFacing.HORIZONTALS;
			foreach (BlockFacing facing2 in hORIZONTALS)
			{
				TrySpreadIntoBlock(ourblock, ourSolid, pos, pos.AddCopy(facing2), facing2, world);
			}
		}
	}

	private void ReplaceLiquidBlock(Block liquidBlock, BlockPos pos, IWorldAccessor world)
	{
		Block replacementBlock = GetReplacementBlock(liquidBlock, world);
		if (replacementBlock != null)
		{
			world.BulkBlockAccessor.SetBlock(replacementBlock.BlockId, pos);
			world.BulkBlockAccessor.SetBlock(0, pos, 2);
			BlockBehaviorBreakIfFloating bh = replacementBlock.GetBehavior<BlockBehaviorBreakIfFloating>();
			if (bh != null && bh.IsSurroundedByNonSolid(world, pos))
			{
				world.BulkBlockAccessor.SetBlock(replacementBlock.BlockId, pos.DownCopy());
			}
			UpdateNeighbouringLiquids(pos, world, null, null);
			GenerateSteamParticles(pos, world);
			world.PlaySoundAt(collisionReplaceSound, pos, 0.0, null, randomizePitch: true, 16f);
		}
	}

	private void SpreadLiquid(int blockId, BlockPos pos, IWorldAccessor world)
	{
		world.BulkBlockAccessor.SetBlock(blockId, pos, 2);
		if (blockId != 0)
		{
			world.RegisterCallbackUnique(OnDelayedWaterUpdateCheck, pos, spreadDelay);
			Block ourBlock = world.GetBlock(blockId);
			TryReplaceNearbyLiquidBlocks(ourBlock, pos, world);
		}
	}

	private void updateOwnFlowDir(Block block, IWorldAccessor world, BlockPos pos)
	{
		int blockId = GetLiquidBlockId(world, pos, block, block.LiquidLevel);
		if (block.BlockId != blockId)
		{
			world.BlockAccessor.SetBlock(blockId, pos, 2);
		}
	}

	private void TryReplaceNearbyLiquidBlocks(Block ourBlock, BlockPos pos, IWorldAccessor world)
	{
		BlockPos npos = pos.Copy();
		BlockFacing[] hORIZONTALS = BlockFacing.HORIZONTALS;
		for (int i = 0; i < hORIZONTALS.Length; i++)
		{
			hORIZONTALS[i].IterateThruFacingOffsets(npos);
			Block neib = world.BlockAccessor.GetBlock(npos, 2);
			if (CollidesWith(ourBlock, neib))
			{
				ReplaceLiquidBlock(ourBlock, npos, world);
			}
		}
	}

	private bool TryFindSourceAndSpread(BlockPos startingPos, IWorldAccessor world)
	{
		BlockPos sourceBlockPos = startingPos.UpCopy();
		Block sourceBlock = world.BlockAccessor.GetBlock(sourceBlockPos, 2);
		while (sourceBlock.IsLiquid())
		{
			if (IsLiquidSourceBlock(sourceBlock))
			{
				Block ourSolid = world.BlockAccessor.GetBlock(sourceBlockPos, 1);
				TrySpreadHorizontal(sourceBlock, ourSolid, world, sourceBlockPos);
				return true;
			}
			sourceBlockPos.Add(0, 1, 0);
			sourceBlock = world.BlockAccessor.GetBlock(sourceBlockPos, 2);
		}
		return false;
	}

	private void GenerateSteamParticles(BlockPos pos, IWorldAccessor world)
	{
		float maxQuantity = 100f;
		int color = ColorUtil.ToRgba(100, 225, 225, 225);
		Vec3d minPos = new Vec3d();
		Vec3d addPos = new Vec3d();
		Vec3f minVelocity = new Vec3f(-0.25f, 0.1f, -0.25f);
		Vec3f maxVelocity = new Vec3f(0.25f, 0.1f, 0.25f);
		float lifeLength = 2f;
		float gravityEffect = -0.015f;
		float minSize = 0.1f;
		float maxSize = 0.1f;
		SimpleParticleProperties steamParticles = new SimpleParticleProperties(50f, maxQuantity, color, minPos, addPos, minVelocity, maxVelocity, lifeLength, gravityEffect, minSize, maxSize, EnumParticleModel.Quad);
		steamParticles.Async = true;
		steamParticles.MinPos.Set(pos.ToVec3d().AddCopy(0.5, 1.1, 0.5));
		steamParticles.AddPos.Set(new Vec3d(0.5, 1.0, 0.5));
		steamParticles.SizeEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARINCREASE, 1f);
		world.SpawnParticles(steamParticles);
	}

	private void UpdateNeighbouringLiquids(BlockPos pos, IWorldAccessor world, BlockPos skip, Block potentiallyHigher)
	{
		world.BulkBlockAccessor.ReadFromStagedByDefault = true;
		BlockPos npos = pos.DownCopy();
		Block neib = world.BulkBlockAccessor.GetBlock(npos, 2);
		BlockBehaviorFiniteSpreadingLiquid bbfsl = neib.GetBehavior<BlockBehaviorFiniteSpreadingLiquid>();
		if (bbfsl != null)
		{
			world.RegisterCallbackUnique(bbfsl.OnDelayedWaterUpdateCheck, npos.Copy(), spreadDelay);
		}
		npos.Up(2);
		neib = world.BulkBlockAccessor.GetBlock(npos, 2);
		bbfsl = neib.GetBehavior<BlockBehaviorFiniteSpreadingLiquid>();
		if (bbfsl != null)
		{
			world.RegisterCallbackUnique(bbfsl.OnDelayedWaterUpdateCheck, npos.Copy(), spreadDelay);
		}
		npos.Down();
		Cardinal[] aLL = Cardinal.ALL;
		foreach (Cardinal val in aLL)
		{
			npos.Set(pos.X + val.Normali.X, pos.Y, pos.Z + val.Normali.Z);
			if (npos.Equals(skip))
			{
				continue;
			}
			neib = world.BulkBlockAccessor.GetBlock(npos, 2);
			if (potentiallyHigher == null || neib.LiquidLevel < potentiallyHigher.LiquidLevel)
			{
				bbfsl = neib.GetBehavior<BlockBehaviorFiniteSpreadingLiquid>();
				if (bbfsl != null)
				{
					world.RegisterCallbackUnique(bbfsl.OnDelayedWaterUpdateCheck, npos.Copy(), spreadDelay);
				}
			}
		}
	}

	private Block GetReplacementBlock(Block neighborBlock, IWorldAccessor world)
	{
		AssetLocation replacementLocation = liquidFlowingCollisionReplacement;
		if (IsLiquidSourceBlock(neighborBlock))
		{
			replacementLocation = liquidSourceCollisionReplacement;
		}
		if (!(replacementLocation == null))
		{
			return world.GetBlock(replacementLocation);
		}
		return null;
	}

	private bool CollidesWith(Block block, Block other)
	{
		if (other.IsLiquid() && block.IsLiquid())
		{
			return other.LiquidCode == collidesWith;
		}
		return false;
	}

	private bool IsSameLiquid(Block block, Block other)
	{
		return block.LiquidCode == other.LiquidCode;
	}

	private bool IsLiquidSourceBlock(Block block)
	{
		return block.LiquidLevel == 7;
	}

	private bool TryLoweringLiquidLevel(Block ourBlock, IWorldAccessor world, BlockPos pos)
	{
		if (!IsLiquidSourceBlock(ourBlock))
		{
			if (GetMaxNeighbourLiquidLevel(ourBlock, world, pos, out var fedByOneNeighbour) <= ourBlock.LiquidLevel || !fedByOneNeighbour)
			{
				LowerLiquidLevelAndNotifyNeighbors(ourBlock, pos, world);
				return true;
			}
			if (!multiplySpread && ourBlock.Variant["flow"] != "d")
			{
				BlockPos npos = pos.Copy();
				for (int i = 0; i < BlockFacing.HORIZONTALS.Length; i++)
				{
					BlockFacing.HORIZONTALS[i].IterateThruFacingOffsets(npos);
					if (HasBetterFlowDirection(npos, pos, world, ourBlock))
					{
						LowerLiquidLevelAndNotifyNeighbors(ourBlock, pos, world);
						return true;
					}
				}
			}
		}
		return false;
	}

	private void LowerLiquidLevelAndNotifyNeighbors(Block block, BlockPos pos, IWorldAccessor world)
	{
		SpreadLiquid(GetLessLiquidBlockId(world, pos, block), pos, world);
		BlockPos npos = pos.Copy();
		for (int i = 0; i < 6; i++)
		{
			BlockFacing.ALLFACES[i].IterateThruFacingOffsets(npos);
			Block liquidBlock = world.BlockAccessor.GetBlock(npos, 2);
			if (liquidBlock.BlockId != 0)
			{
				liquidBlock.OnNeighbourBlockChange(world, npos, pos);
			}
		}
	}

	private bool HasBetterFlowDirection(BlockPos ourpos, BlockPos npos, IWorldAccessor world, Block neibBlock)
	{
		Block ourLiquid = world.BlockAccessor.GetBlock(ourpos, 2);
		if (!IsSameLiquid(neibBlock, ourLiquid))
		{
			return false;
		}
		if (ourLiquid.Variant["flow"] != "d")
		{
			return false;
		}
		bool num = world.BlockAccessor.GetBlock(ourpos.Down(), 1).GetLiquidBarrierHeightOnSide(BlockFacing.UP, ourpos) < (float)ourLiquid.LiquidLevel / 7f;
		ourpos.Up();
		if (num)
		{
			return true;
		}
		Block ourSolid = world.BlockAccessor.GetBlock(ourpos.Up(), 1);
		if (CanSpreadIntoBlock(ourLiquid, ourSolid, ourpos, ourpos.DownCopy(), BlockFacing.DOWN, world))
		{
			return true;
		}
		return false;
	}

	private bool TrySpreadIntoBlock(Block ourblock, Block ourSolid, BlockPos pos, BlockPos npos, BlockFacing facing, IWorldAccessor world)
	{
		if (CanSpreadIntoBlock(ourblock, ourSolid, pos, npos, facing, world))
		{
			Block neighborLiquid = world.BlockAccessor.GetBlock(npos, 2);
			if (CollidesWith(ourblock, neighborLiquid))
			{
				ReplaceLiquidBlock(neighborLiquid, npos, world);
			}
			else
			{
				SpreadLiquid(GetLessLiquidBlockId(world, npos, ourblock), npos, world);
			}
			return true;
		}
		return false;
	}

	public int GetLessLiquidBlockId(IWorldAccessor world, BlockPos pos, Block block)
	{
		return GetLiquidBlockId(world, pos, block, block.LiquidLevel - 1);
	}

	public int GetMoreLiquidBlockId(IWorldAccessor world, BlockPos pos, Block block)
	{
		return GetLiquidBlockId(world, pos, block, Math.Min(7, block.LiquidLevel + 1));
	}

	public int GetLiquidBlockId(IWorldAccessor world, BlockPos pos, Block block, int liquidLevel)
	{
		if (liquidLevel < 1)
		{
			return 0;
		}
		Vec3i dir = new Vec3i();
		bool anySideFree = false;
		BlockPos npos = pos.Copy();
		IBlockAccessor blockAccessor = world.BlockAccessor;
		Cardinal[] aLL = Cardinal.ALL;
		foreach (Cardinal val in aLL)
		{
			npos.Set(pos.X + val.Normali.X, pos.Y, pos.Z + val.Normali.Z);
			Block nblock = blockAccessor.GetBlock(npos, 2);
			if (nblock.LiquidLevel != liquidLevel && nblock.Replaceable >= 6000 && nblock.IsLiquid())
			{
				Vec3i normal = ((nblock.LiquidLevel < liquidLevel) ? val.Normali : val.Opposite.Normali);
				if (!val.IsDiagonal)
				{
					nblock = blockAccessor.GetBlock(npos, 1);
					anySideFree |= (double)nblock.GetLiquidBarrierHeightOnSide(BlockFacing.ALLFACES[val.Opposite.Index / 2], npos) != 1.0;
					nblock = blockAccessor.GetBlock(pos, 1);
					anySideFree |= (double)nblock.GetLiquidBarrierHeightOnSide(BlockFacing.ALLFACES[val.Index / 2], pos) != 1.0;
				}
				dir.X += normal.X;
				dir.Z += normal.Z;
			}
		}
		if (Math.Abs(dir.X) > Math.Abs(dir.Z))
		{
			dir.Z = 0;
		}
		else if (Math.Abs(dir.Z) > Math.Abs(dir.X))
		{
			dir.X = 0;
		}
		dir.X = Math.Sign(dir.X);
		dir.Z = Math.Sign(dir.Z);
		Cardinal flowDir = Cardinal.FromNormali(dir);
		if (flowDir == null)
		{
			Block downBlock = blockAccessor.GetBlock(pos.DownCopy(), 2);
			Block upBlock = blockAccessor.GetBlock(pos.UpCopy(), 2);
			bool num = IsSameLiquid(downBlock, block);
			bool upLiquid = IsSameLiquid(upBlock, block);
			if ((num && downBlock.Variant["flow"] == "d") || (upLiquid && upBlock.Variant["flow"] == "d"))
			{
				return world.GetBlock(block.CodeWithParts("d", liquidLevel.ToString() ?? "")).BlockId;
			}
			if (anySideFree)
			{
				return world.GetBlock(block.CodeWithParts("d", liquidLevel.ToString() ?? "")).BlockId;
			}
			return world.GetBlock(block.CodeWithParts("still", liquidLevel.ToString() ?? "")).BlockId;
		}
		return world.GetBlock(block.CodeWithParts(flowDir.Initial, liquidLevel.ToString() ?? "")).BlockId;
	}

	private int GetFallingLiquidBlockId(Block ourBlock, IWorldAccessor world)
	{
		return world.GetBlock(ourBlock.CodeWithParts("d", "6")).BlockId;
	}

	public int GetMaxNeighbourLiquidLevel(Block ourblock, IWorldAccessor world, BlockPos pos, out bool oneNeighbourFeedsThis)
	{
		Block ourSolid = world.BlockAccessor.GetBlock(pos, 1);
		BlockPos npos = pos.UpCopy();
		Block ublock = world.BlockAccessor.GetBlock(npos, 2);
		Block uSolid = world.BlockAccessor.GetBlock(npos, 1);
		if (IsSameLiquid(ourblock, ublock) && (double)ourSolid.GetLiquidBarrierHeightOnSide(BlockFacing.UP, pos) == 0.0 && (double)uSolid.GetLiquidBarrierHeightOnSide(BlockFacing.DOWN, npos) == 0.0)
		{
			oneNeighbourFeedsThis = true;
			return 7;
		}
		int level = 0;
		int ourlevel = ourblock.LiquidLevel;
		oneNeighbourFeedsThis = false;
		npos.Down();
		for (int i = 0; i < BlockFacing.HORIZONTALS.Length; i++)
		{
			BlockFacing.HORIZONTALS[i].IterateThruFacingOffsets(npos);
			Block nblock = world.BlockAccessor.GetBlock(npos, 2);
			if (IsSameLiquid(ourblock, nblock))
			{
				int nLevel = nblock.LiquidLevel;
				if (nLevel == ourlevel + 1)
				{
					oneNeighbourFeedsThis = true;
				}
				if (!(ourSolid.GetLiquidBarrierHeightOnSide(BlockFacing.HORIZONTALS[i], pos) >= (float)nLevel / 7f) && !(world.BlockAccessor.GetBlock(npos, 1).GetLiquidBarrierHeightOnSide(BlockFacing.HORIZONTALS[i].Opposite, npos) >= (float)nLevel / 7f))
				{
					level = Math.Max(level, nLevel);
				}
			}
		}
		return level;
	}

	[Obsolete("Instead Use CanSpreadIntoBlock(Block, BlockPos, IWorldAccessor) to read from the liquid layer correctly, as well as the block layer")]
	public bool CanSpreadIntoBlock(Block ourblock, Block neighborBlock, IWorldAccessor world)
	{
		if (!IsSameLiquid(ourblock, neighborBlock) || neighborBlock.LiquidLevel >= ourblock.LiquidLevel)
		{
			if (!IsSameLiquid(ourblock, neighborBlock))
			{
				return neighborBlock.Replaceable >= ReplacableThreshold;
			}
			return false;
		}
		return true;
	}

	public bool CanSpreadIntoBlock(Block ourblock, Block ourSolid, BlockPos pos, BlockPos npos, BlockFacing facing, IWorldAccessor world)
	{
		if (ourSolid.GetLiquidBarrierHeightOnSide(facing, pos) >= (float)ourblock.LiquidLevel / 7f)
		{
			return false;
		}
		if (world.BlockAccessor.GetBlock(npos, 1).GetLiquidBarrierHeightOnSide(facing.Opposite, npos) >= (float)ourblock.LiquidLevel / 7f)
		{
			return false;
		}
		Block neighborLiquid = world.BlockAccessor.GetBlock(npos, 2);
		if (IsSameLiquid(ourblock, neighborLiquid))
		{
			if (!IsLiquidSourceBlock(ourblock) && neighborLiquid is IBlockFlowing neighborFlowing && ourblock is IBlockFlowing ourFlowing)
			{
				float neibFlow = neighborFlowing.FlowRate(npos);
				float ourFlow = ourFlowing.FlowRate(pos);
				if (neibFlow == ourFlow)
				{
					if (neighborLiquid.LiquidLevel >= ourblock.LiquidLevel)
					{
						if (facing == BlockFacing.DOWN)
						{
							return neighborLiquid.Variant["flow"] != "d";
						}
						return false;
					}
					return true;
				}
				if (ourFlow < neibFlow)
				{
					if (neighborLiquid.LiquidLevel <= ourblock.LiquidLevel)
					{
						return true;
					}
					if (npos.Y < pos.Y)
					{
						return true;
					}
				}
				return false;
			}
			return neighborLiquid.LiquidLevel < ourblock.LiquidLevel;
		}
		if (neighborLiquid.LiquidLevel == 7 && !CollidesWith(ourblock, neighborLiquid))
		{
			return false;
		}
		if (neighborLiquid.BlockId != 0)
		{
			return neighborLiquid.Replaceable >= ourblock.Replaceable;
		}
		if (ourblock.LiquidLevel <= 1)
		{
			return facing == BlockFacing.DOWN;
		}
		return true;
	}

	public override bool IsReplacableBy(Block byBlock, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		if (block.IsLiquid() || block.Replaceable >= ReplacableThreshold)
		{
			return byBlock.Replaceable <= block.Replaceable;
		}
		return false;
	}

	public List<PosAndDist> FindDownwardPaths(IWorldAccessor world, BlockPos pos, Block ourBlock)
	{
		List<PosAndDist> paths = new List<PosAndDist>();
		Queue<BlockPos> uncheckedPositions = new Queue<BlockPos>();
		int shortestPath = 99;
		BlockPos npos = new BlockPos(pos.dimension);
		for (int i = 0; i < downPaths.Length; i++)
		{
			Vec2i offset = downPaths[i];
			npos.Set(pos.X + offset.X, pos.Y - 1, pos.Z + offset.Y);
			Block blockBelow = world.BlockAccessor.GetBlock(npos);
			npos.Y++;
			Block neibLiquid = world.BlockAccessor.GetBlock(npos, 2);
			Block neibBlock = world.BlockAccessor.GetBlock(npos, 1);
			if (neibLiquid.LiquidLevel >= ourBlock.LiquidLevel || blockBelow.Replaceable < ReplacableThreshold || neibBlock.Replaceable < ReplacableThreshold || (ourBlock.Code.PathStartsWith("rapidwater-") && neibLiquid.Code.PathStartsWith("water-")))
			{
				continue;
			}
			uncheckedPositions.Enqueue(new BlockPos(pos.X + offset.X, pos.Y, pos.Z + offset.Y, pos.dimension));
			BlockPos foundPos = BfsSearchPath(world, uncheckedPositions, pos, ourBlock);
			if (foundPos != null)
			{
				PosAndDist pad = new PosAndDist
				{
					pos = foundPos,
					dist = pos.ManhattanDistance(pos.X + offset.X, pos.Y, pos.Z + offset.Y)
				};
				if (pad.dist == 1 && ourBlock.LiquidLevel < 7)
				{
					paths.Clear();
					paths.Add(pad);
					return paths;
				}
				paths.Add(pad);
				shortestPath = Math.Min(shortestPath, pad.dist);
			}
		}
		for (int j = 0; j < paths.Count; j++)
		{
			if (paths[j].dist > shortestPath)
			{
				paths.RemoveAt(j);
				j--;
			}
		}
		return paths;
	}

	private BlockPos BfsSearchPath(IWorldAccessor world, Queue<BlockPos> uncheckedPositions, BlockPos origin, Block ourBlock)
	{
		BlockPos npos = new BlockPos(origin.dimension);
		while (uncheckedPositions.Count > 0)
		{
			BlockPos pos = uncheckedPositions.Dequeue();
			int curDist = pos.ManhattanDistance(origin);
			Block curBlock = world.BlockAccessor.GetMostSolidBlock(pos);
			npos.Set(pos);
			for (int i = 0; i < BlockFacing.HORIZONTALS.Length; i++)
			{
				BlockFacing facing = BlockFacing.HORIZONTALS[i];
				facing.IterateThruFacingOffsets(npos);
				int distFromOrigin = npos.ManhattanDistance(origin);
				if (distFromOrigin <= curDist && !(curBlock.GetLiquidBarrierHeightOnSide(facing, pos) >= (float)(ourBlock.LiquidLevel - curDist) / 7f) && !(world.BlockAccessor.GetMostSolidBlock(npos).GetLiquidBarrierHeightOnSide(facing.Opposite, npos) >= (float)(ourBlock.LiquidLevel - curDist) / 7f))
				{
					if (distFromOrigin == 0)
					{
						return pos;
					}
					uncheckedPositions.Enqueue(npos.Copy());
				}
			}
		}
		return null;
	}

	public override bool ShouldReceiveClientParticleTicks(IWorldAccessor world, IPlayer byPlayer, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		if (block.ParticleProperties == null || block.ParticleProperties.Length == 0)
		{
			return false;
		}
		if (block.LiquidCode == "lava")
		{
			return world.BlockAccessor.GetBlockAbove(pos).Replaceable > ReplacableThreshold;
		}
		handled = EnumHandling.PassThrough;
		return false;
	}

	private static AssetLocation CreateAssetLocation(JsonObject properties, string propertyName)
	{
		return CreateAssetLocation(properties, null, propertyName);
	}

	private static AssetLocation CreateAssetLocation(JsonObject properties, string prefix, string propertyName)
	{
		string value = properties[propertyName]?.AsString();
		if (value == null)
		{
			return null;
		}
		if (prefix != null)
		{
			return new AssetLocation(prefix + value);
		}
		return new AssetLocation(value);
	}
}
