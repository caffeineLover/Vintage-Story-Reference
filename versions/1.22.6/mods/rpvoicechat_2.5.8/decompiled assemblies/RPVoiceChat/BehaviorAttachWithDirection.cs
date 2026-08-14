using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

// Token: 0x02000002 RID: 2
internal class BehaviorAttachWithDirection : BlockBehavior
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public BehaviorAttachWithDirection(Block block) : base(block)
	{
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002088 File Offset: 0x00000288
	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		this.orientationCode = properties["facingCode"].AsString("orientation");
		this.sideCode = properties["sideCode"].AsString("side");
		this.dropOrientation = properties["dropOrientation"].AsString("up");
		this.dropSide = properties["dropSide"].AsString("north");
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002108 File Offset: 0x00000308
	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
	{
		handling = 2;
		Block orientedBlock = this.GetOrientedBlock(world, byPlayer, blockSel.Face);
		if (orientedBlock == null)
		{
			failureCode = "requireattachable";
			return false;
		}
		if (!orientedBlock.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
		{
			return false;
		}
		orientedBlock.DoPlaceBlock(world, byPlayer, blockSel, itemstack);
		return true;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002154 File Offset: 0x00000354
	public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
	{
		handling = 2;
		BlockFacing oppositeFace = blockSel.Face.Opposite;
		BlockPos attachingBlockPos = blockSel.Position.AddCopy(oppositeFace);
		if (world.BlockAccessor.GetBlock(attachingBlockPos).CanAttachBlockAt(world.BlockAccessor, this.block, attachingBlockPos, blockSel.Face, null))
		{
			return true;
		}
		failureCode = "requireattachable";
		return false;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000021B0 File Offset: 0x000003B0
	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropQuantityMultiplier, ref EnumHandling handled)
	{
		handled = 2;
		Block droppedBlock = world.BlockAccessor.GetBlock(this.block.CodeWithVariants(new string[]
		{
			this.orientationCode,
			this.sideCode
		}, new string[]
		{
			this.dropOrientation,
			this.dropSide
		}));
		return new ItemStack[]
		{
			new ItemStack(droppedBlock, 1)
		};
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002218 File Offset: 0x00000418
	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos, ref EnumHandling handled)
	{
		handled = 2;
		return new ItemStack(world.BlockAccessor.GetBlock(this.block.CodeWithVariants(new string[]
		{
			this.orientationCode,
			this.sideCode
		}, new string[]
		{
			this.dropOrientation,
			this.dropSide
		})), 1);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002274 File Offset: 0x00000474
	private Block GetOrientedBlock(IWorldAccessor world, IPlayer byPlayer, BlockFacing attachFace)
	{
		string orientation = attachFace.Code;
		string side = BlockFacing.HorizontalFromAngle(byPlayer.Entity.Pos.Yaw).Code;
		return world.BlockAccessor.GetBlock(this.block.CodeWithVariants(new string[]
		{
			this.orientationCode,
			this.sideCode
		}, new string[]
		{
			orientation,
			side
		}));
	}

	// Token: 0x04000001 RID: 1
	private string orientationCode = "orientation";

	// Token: 0x04000002 RID: 2
	private string sideCode = "side";

	// Token: 0x04000003 RID: 3
	private string dropOrientation = "up";

	// Token: 0x04000004 RID: 4
	private string dropSide = "north";
}
