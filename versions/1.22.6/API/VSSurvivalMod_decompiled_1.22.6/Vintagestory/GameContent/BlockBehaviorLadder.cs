using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class BlockBehaviorLadder : BlockBehavior
{
	[DocumentAsJson("Optional", "north", false)]
	private string dropBlockFace = "north";

	[DocumentAsJson("Optional", "False", false)]
	public bool isFlexible;

	[DocumentAsJson("Optional", "False", false)]
	public bool isCrude;

	private static readonly Cuboidi[] upHalf = new Cuboidi[4]
	{
		new Cuboidi(0, 0, 0, 14, 7, 1),
		new Cuboidi(13, 0, 0, 15, 7, 15),
		new Cuboidi(0, 0, 13, 15, 7, 15),
		new Cuboidi(0, 0, 0, 1, 7, 15)
	};

	private static readonly Cuboidi[] downHalf = new Cuboidi[4]
	{
		new Cuboidi(0, 8, 0, 14, 15, 1),
		new Cuboidi(13, 8, 0, 15, 15, 15),
		new Cuboidi(0, 8, 13, 15, 15, 15),
		new Cuboidi(0, 8, 0, 1, 15, 15)
	};

	public string LadderType => block.Variant["material"];

	public BlockBehaviorLadder(Block block)
		: base(block)
	{
	}

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		dropBlockFace = properties["dropBlockFace"].AsString("north");
		isFlexible = properties["isFlexible"].AsBool();
		isCrude = properties["isCrude"].AsBool();
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		if (isFlexible && !byPlayer.Entity.Controls.ShiftKey)
		{
			TryCollectLowest(byPlayer, world, blockSel.Position);
			handling = EnumHandling.PreventDefault;
			return true;
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel, ref handling);
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
	{
		base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling, ref handling);
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref EnumHandling handled, ref string failureCode)
	{
		handled = EnumHandling.PreventDefault;
		if (isFlexible && !byPlayer.Entity.Controls.ShiftKey)
		{
			failureCode = "sneaktoplace";
			return false;
		}
		BlockPos pos = blockSel.Position;
		BlockPos aimedAtPos = (blockSel.DidOffset ? pos.AddCopy(blockSel.Face.Opposite) : blockSel.Position);
		bool placed = false;
		if (blockSel.HitPosition.Y < 0.5)
		{
			placed = TryStackDown(byPlayer, world, aimedAtPos, blockSel.Face, itemstack);
		}
		if (placed || TryStackUp(byPlayer, world, aimedAtPos, blockSel.Face, itemstack) || TryStackDown(byPlayer, world, aimedAtPos, blockSel.Face, itemstack))
		{
			return true;
		}
		AssetLocation blockCode;
		if (blockSel.Face.IsVertical)
		{
			BlockFacing[] faces = Block.SuggestedHVOrientation(byPlayer, blockSel);
			blockCode = block.CodeWithParts(faces[0].Code);
		}
		else
		{
			blockCode = block.CodeWithParts(blockSel.Face.Opposite.Code);
		}
		Block orientedBlock = world.BlockAccessor.GetBlock(blockCode);
		if (HasSupport(orientedBlock, world.BlockAccessor, pos) && orientedBlock.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
		{
			orientedBlock.DoPlaceBlock(world, byPlayer, blockSel, itemstack);
			return true;
		}
		failureCode = "cantattachladder";
		return false;
	}

	protected bool TryStackUp(IPlayer byPlayer, IWorldAccessor world, BlockPos pos, BlockFacing face, ItemStack itemstack)
	{
		if (isFlexible)
		{
			return false;
		}
		Block ladderBlock = world.BlockAccessor.GetBlock(pos);
		string obj = ladderBlock.GetBehavior<BlockBehaviorLadder>()?.LadderType;
		ladderBlock = world.BlockAccessor.GetBlock(itemstack.Block?.CodeWithVariant("side", ladderBlock.Variant["side"]) ?? ((AssetLocation)"")) ?? ladderBlock;
		if (obj != LadderType)
		{
			return false;
		}
		BlockPos abovePos = pos.UpCopy();
		Block aboveBlock = null;
		while (abovePos.Y < world.BlockAccessor.MapSizeY)
		{
			aboveBlock = world.BlockAccessor.GetBlock(abovePos);
			if (aboveBlock.GetBehavior<BlockBehaviorLadder>()?.LadderType != LadderType)
			{
				break;
			}
			abovePos.Up();
		}
		string useless = "";
		if (aboveBlock == null)
		{
			return false;
		}
		if (!ladderBlock.CanPlaceBlock(world, byPlayer, new BlockSelection
		{
			Position = abovePos,
			Face = face
		}, ref useless))
		{
			return false;
		}
		ladderBlock.DoPlaceBlock(world, byPlayer, new BlockSelection
		{
			Position = abovePos,
			Face = face
		}, itemstack);
		BlockPos neibPos = new BlockPos(pos.dimension);
		BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
		foreach (BlockFacing facing in aLLFACES)
		{
			neibPos.Set(abovePos).Offset(facing);
			world.BlockAccessor.GetBlock(neibPos).OnNeighbourBlockChange(world, neibPos, abovePos);
		}
		return true;
	}

	protected bool TryStackDown(IPlayer byPlayer, IWorldAccessor world, BlockPos pos, BlockFacing face, ItemStack itemstack)
	{
		Block ladderBlock = world.BlockAccessor.GetBlock(pos);
		string obj = ladderBlock.GetBehavior<BlockBehaviorLadder>()?.LadderType;
		ladderBlock = world.BlockAccessor.GetBlock(itemstack.Block?.CodeWithVariant("side", ladderBlock.Variant["side"]) ?? ((AssetLocation)"")) ?? ladderBlock;
		if (obj != LadderType)
		{
			return false;
		}
		BlockPos belowPos = pos.DownCopy();
		Block belowBlock = null;
		while (belowPos.Y > 0)
		{
			belowBlock = world.BlockAccessor.GetBlock(belowPos);
			if (belowBlock.GetBehavior<BlockBehaviorLadder>()?.LadderType != LadderType)
			{
				break;
			}
			belowPos.Down();
		}
		string useless = "";
		if (belowBlock == null)
		{
			return false;
		}
		if (!belowBlock.IsReplacableBy(block))
		{
			return false;
		}
		if (!ladderBlock.CanPlaceBlock(world, byPlayer, new BlockSelection
		{
			Position = belowPos,
			Face = face
		}, ref useless))
		{
			return false;
		}
		ladderBlock.DoPlaceBlock(world, byPlayer, new BlockSelection
		{
			Position = belowPos,
			Face = face
		}, itemstack);
		BlockPos neibPos = new BlockPos(pos.dimension);
		BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
		foreach (BlockFacing facing in aLLFACES)
		{
			neibPos.Set(belowPos).Offset(facing);
			world.BlockAccessor.GetBlock(neibPos).OnNeighbourBlockChange(world, neibPos, belowPos);
		}
		return true;
	}

	protected bool TryCollectLowest(IPlayer byPlayer, IWorldAccessor world, BlockPos pos)
	{
		BlockPos belowPos = pos.DownCopy();
		while (belowPos.Y > 0 && !(world.BlockAccessor.GetBlock(belowPos).GetBehavior<BlockBehaviorLadder>()?.LadderType != LadderType))
		{
			belowPos.Down();
		}
		belowPos.Up();
		Block collectBlock = world.BlockAccessor.GetBlock(belowPos);
		BlockBehaviorLadder bh = collectBlock.GetBehavior<BlockBehaviorLadder>();
		if (bh == null || !bh.isFlexible)
		{
			return false;
		}
		if (!world.Claims.TryAccess(byPlayer, belowPos, EnumBlockAccessFlags.BuildOrBreak))
		{
			return false;
		}
		ItemStack[] stacks = collectBlock.GetDrops(world, pos, byPlayer);
		world.BlockAccessor.SetBlock(0, belowPos);
		world.PlaySoundAt(collectBlock.Sounds.Break, pos, 0.0, byPlayer);
		if (stacks.Length != 0 && !byPlayer.InventoryManager.TryGiveItemstack(stacks[0], slotNotifyEffect: true))
		{
			world.SpawnItemEntity(stacks[0], byPlayer.Entity.Pos.XYZ);
		}
		return true;
	}

	public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos, ref EnumHandling handling)
	{
		if (!HasSupport(block, world.BlockAccessor, pos))
		{
			handling = EnumHandling.PreventSubsequent;
			world.BlockAccessor.BreakBlock(pos, null);
			world.BlockAccessor.TriggerNeighbourBlockUpdate(pos.Copy());
		}
		else
		{
			base.OnNeighbourBlockChange(world, pos, neibpos, ref handling);
		}
	}

	public bool HasSupportUp(Block forBlock, IBlockAccessor blockAccess, BlockPos pos)
	{
		BlockFacing ownFacing = BlockFacing.FromCode(forBlock.LastCodePart());
		BlockPos upPos = pos.UpCopy();
		Block upBlock = ((pos.Y < blockAccess.MapSizeY - 1) ? blockAccess.GetBlock(upPos) : null);
		string upLadderType = upBlock?.GetBehavior<BlockBehaviorLadder>()?.LadderType;
		if (!SideSolid(blockAccess, pos, ownFacing) && (isCrude || !SideSolid(blockAccess, pos, BlockFacing.UP)))
		{
			if (upLadderType != null && upLadderType == forBlock.GetBehavior<BlockBehaviorLadder>()?.LadderType)
			{
				return HasSupportUp(upBlock, blockAccess, upPos);
			}
			return false;
		}
		return true;
	}

	public bool HasSupportDown(Block forBlock, IBlockAccessor blockAccess, BlockPos pos)
	{
		BlockFacing ownFacing = BlockFacing.FromCode(forBlock.LastCodePart());
		BlockPos downPos = pos.DownCopy();
		Block downBlock = ((pos.Y > 0) ? blockAccess.GetBlock(downPos) : null);
		string downLadderType = downBlock?.GetBehavior<BlockBehaviorLadder>()?.LadderType;
		if (!SideSolid(blockAccess, pos, ownFacing) && (isCrude || !SideSolid(blockAccess, pos, BlockFacing.DOWN)))
		{
			if (downLadderType != null && downLadderType == forBlock.GetBehavior<BlockBehaviorLadder>()?.LadderType)
			{
				return HasSupportDown(downBlock, blockAccess, downPos);
			}
			return false;
		}
		return true;
	}

	public bool HasSupport(Block forBlock, IBlockAccessor blockAccess, BlockPos pos)
	{
		BlockFacing ownFacing = BlockFacing.FromCode(forBlock.LastCodePart());
		string ladderType = forBlock.GetBehavior<BlockBehaviorLadder>()?.LadderType;
		BlockPos downPos = pos.DownCopy();
		Block downBlock = ((pos.Y > 0) ? blockAccess.GetBlock(downPos) : null);
		string downLadderType = downBlock?.GetBehavior<BlockBehaviorLadder>()?.LadderType;
		BlockPos upPos = pos.UpCopy();
		string upLadderType = ((pos.Y < blockAccess.MapSizeY - 1) ? blockAccess.GetBlock(upPos) : null)?.GetBehavior<BlockBehaviorLadder>()?.LadderType;
		if (!SideSolid(blockAccess, pos, ownFacing) && (isFlexible || isCrude || !SideSolid(blockAccess, pos, BlockFacing.DOWN)) && (isCrude || !SideSolid(blockAccess, pos, BlockFacing.UP)) && (upLadderType == null || !(upLadderType == ladderType) || !HasSupportUp(forBlock, blockAccess, upPos)))
		{
			if (!isFlexible && downLadderType != null && downLadderType == ladderType)
			{
				return HasSupportDown(downBlock, blockAccess, downPos);
			}
			return false;
		}
		return true;
	}

	public bool SideSolid(IBlockAccessor blockAccess, BlockPos pos, BlockFacing facing)
	{
		BlockPos neibPos = pos.AddCopy(facing);
		Block neibBlock = blockAccess.GetBlock(neibPos);
		if (neibBlock.Id == 0)
		{
			return false;
		}
		if (facing.IsVertical)
		{
			if (neibBlock.CanAttachBlockAt(blockAccess, neibBlock, neibPos, facing.Opposite))
			{
				return true;
			}
		}
		else
		{
			if (neibBlock.CanAttachBlockAt(blockAccess, neibBlock, neibPos, facing.Opposite, upHalf[facing.Index]))
			{
				return true;
			}
			if (neibBlock.CanAttachBlockAt(blockAccess, neibBlock, neibPos, facing.Opposite, downHalf[facing.Index]))
			{
				return true;
			}
		}
		return false;
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropQuantityMultiplier, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		return new ItemStack[1]
		{
			new ItemStack(world.BlockAccessor.GetBlock(block.CodeWithParts(dropBlockFace)))
		};
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		return new ItemStack(world.BlockAccessor.GetBlock(block.CodeWithParts(dropBlockFace)));
	}

	public override AssetLocation GetRotatedBlockCode(int angle, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		int rotatedIndex = GameMath.Mod(BlockFacing.FromCode(block.LastCodePart()).HorizontalAngleIndex - angle / 90, 4);
		BlockFacing nowFacing = BlockFacing.HORIZONTALS_ANGLEORDER[rotatedIndex];
		return block.CodeWithParts(nowFacing.Code);
	}

	public override AssetLocation GetHorizontallyFlippedBlockCode(EnumAxis axis, ref EnumHandling handling)
	{
		handling = EnumHandling.PreventDefault;
		BlockFacing facing = BlockFacing.FromCode(block.LastCodePart());
		if (facing.Axis == axis)
		{
			return block.CodeWithParts(facing.Opposite.Code);
		}
		return block.Code;
	}
}
