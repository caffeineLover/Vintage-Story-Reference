using System;
using System.Text;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
[AddDocumentationProperty("TriggerSound", "Sets both OpenSound & CloseSound.", "Vintagestory.API.Common.AssetLocation", "Optional", "sounds/block/door", true)]
public class BlockBehaviorDoor : StrongBlockBehavior, IMultiBlockColSelBoxes, IMultiBlockBlockProperties, IClaimTraverseable
{
	[DocumentAsJson("Optional", "sounds/block/door", true)]
	public AssetLocation OpenSound;

	[DocumentAsJson("Optional", "sounds/block/door", true)]
	public AssetLocation CloseSound;

	[DocumentAsJson("Optional", "1", true)]
	public int width;

	[DocumentAsJson("Optional", "1", true)]
	public int height;

	[DocumentAsJson("Optional", "True", true)]
	public bool handopenable;

	[DocumentAsJson("Optional", "True", true)]
	public bool airtight;

	[DocumentAsJson("Optional", "1.0 for each block", true)]
	public float[][] liquidBarrierHeight;

	private ICoreAPI api;

	public MeshData animatableOrigMesh;

	public Shape animatableShape;

	public string animatableDictKey;

	public BlockBehaviorDoor(Block block)
		: base(block)
	{
		airtight = block.Attributes["airtight"].AsBool(defaultValue: true);
		width = block.Attributes["width"].AsInt(1);
		height = block.Attributes["height"].AsInt(1);
		handopenable = block.Attributes["handopenable"].AsBool(defaultValue: true);
		liquidBarrierHeight = block.Attributes["liquidBarrierHeight"].AsObject<float[][]>();
	}

	public override void OnLoaded(ICoreAPI api)
	{
		this.api = api;
		OpenSound = (CloseSound = AssetLocation.Create(block.Attributes["triggerSound"].AsString("sounds/block/door")));
		JsonObject soundAttribute = block.Attributes["openSound"];
		if (soundAttribute.Exists)
		{
			OpenSound = AssetLocation.Create(soundAttribute.AsString("sounds/block/door"));
		}
		soundAttribute = block.Attributes["closeSound"];
		if (soundAttribute.Exists)
		{
			CloseSound = AssetLocation.Create(soundAttribute.AsString("sounds/block/door"));
		}
		base.OnLoaded(api);
	}

	public override void Activate(IWorldAccessor world, Caller caller, BlockSelection blockSel, ITreeAttribute activationArgs, ref EnumHandling handled)
	{
		BEBehaviorDoor beh = world.BlockAccessor.GetBlockEntity(blockSel.Position)?.GetBehavior<BEBehaviorDoor>();
		bool opened = !beh.Opened;
		if (activationArgs != null)
		{
			opened = activationArgs.GetBool("opened", opened);
		}
		if (beh.Opened != opened)
		{
			beh.ToggleDoorState(null, opened);
		}
	}

	public override void OnDecalTesselation(IWorldAccessor world, MeshData decalMesh, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		BEBehaviorDoor beh = world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorDoor>();
		if (beh != null)
		{
			decalMesh.Rotate(0f, beh.RotateYRad, 0f);
		}
	}

	public static BEBehaviorDoor getDoorAt(IWorldAccessor world, BlockPos pos)
	{
		BEBehaviorDoor door = world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorDoor>();
		if (door != null)
		{
			return door;
		}
		if (world.BlockAccessor.GetBlock(pos) is BlockMultiblock blockMb)
		{
			return world.BlockAccessor.GetBlockEntity(pos.AddCopy(blockMb.OffsetInv))?.GetBehavior<BEBehaviorDoor>();
		}
		return null;
	}

	public static bool HasCombinableLeftDoor(IWorldAccessor world, float RotateYRad, BlockPos pos, int width, out BEBehaviorDoor leftDoor, out int leftOffset)
	{
		leftOffset = 0;
		leftDoor = null;
		BlockFacing leftFacing = BlockFacing.HorizontalFromYaw(RotateYRad).GetCW();
		BlockPos leftPos = pos.AddCopy(leftFacing);
		leftDoor = getDoorAt(world, leftPos);
		if (width > 1)
		{
			if (leftDoor == null)
			{
				for (int i = 2; i <= width; i++)
				{
					leftPos = pos.AddCopy(leftFacing, i);
					leftDoor = getDoorAt(world, leftPos);
					if (leftDoor != null)
					{
						break;
					}
				}
			}
			if (leftDoor != null)
			{
				BlockPos offsetPos = leftDoor.Pos.AddCopy(leftFacing.Opposite, leftDoor.InvertHandles ? width : (width + leftDoor.doorBh.width - 1));
				leftOffset = (int)pos.DistanceTo(offsetPos);
				if ((leftDoor.facingWhenClosed.Axis == EnumAxis.X && leftPos.X != leftDoor.Pos.X) || (leftDoor.facingWhenClosed.Axis == EnumAxis.Z && leftPos.Z != leftDoor.Pos.Z))
				{
					leftDoor = null;
					leftOffset = 0;
				}
			}
		}
		if (leftDoor != null && leftDoor.LeftDoor == null && leftDoor.RightDoor == null && leftDoor.facingWhenClosed == BlockFacing.HorizontalFromYaw(RotateYRad))
		{
			return true;
		}
		return false;
	}

	public static bool HasCombinableRightDoor(IWorldAccessor world, float RotateYRad, BlockPos pos, int width, out BEBehaviorDoor rightDoor, out int rightOffset)
	{
		rightOffset = 0;
		rightDoor = null;
		BlockFacing rightFacing = BlockFacing.HorizontalFromYaw(RotateYRad).GetCCW();
		BlockPos rightPos = pos.AddCopy(rightFacing);
		rightDoor = getDoorAt(world, rightPos);
		if (width > 1)
		{
			if (rightDoor == null)
			{
				for (int i = 2; i <= width; i++)
				{
					rightPos = pos.AddCopy(rightFacing, i);
					rightDoor = getDoorAt(world, rightPos);
					if (rightDoor != null)
					{
						break;
					}
				}
			}
			if (rightDoor != null)
			{
				BlockPos offsetPos = rightDoor.Pos.AddCopy(rightFacing.Opposite, (!rightDoor.InvertHandles) ? width : (width + rightDoor.doorBh.width - 1));
				rightOffset = (int)pos.DistanceTo(offsetPos);
				if ((rightDoor.facingWhenClosed.Axis == EnumAxis.X && rightPos.X != rightDoor.Pos.X) || (rightDoor.facingWhenClosed.Axis == EnumAxis.Z && rightPos.Z != rightDoor.Pos.Z))
				{
					rightDoor = null;
					rightOffset = 0;
				}
			}
		}
		if (rightDoor != null && rightDoor.RightDoor == null && rightDoor.LeftDoor == null && rightDoor.facingWhenClosed == BlockFacing.HorizontalFromYaw(RotateYRad))
		{
			return true;
		}
		return false;
	}

	public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
	{
		BlockPos pos = blockSel.Position.Copy();
		float rotRad = BEBehaviorDoor.getRotateYRad(byPlayer, blockSel);
		BlockFacing facing = BlockFacing.HorizontalFromYaw(rotRad);
		bool blocked = false;
		BEBehaviorDoor otherDoor;
		int offset;
		bool invertHandle = HasCombinableLeftDoor(world, rotRad, blockSel.Position, width, out otherDoor, out offset);
		if (invertHandle && width > 1 && offset != 0)
		{
			pos.Add(facing.GetCCW(), offset);
		}
		if (!invertHandle && HasCombinableRightDoor(world, rotRad, blockSel.Position, width, out otherDoor, out offset) && width > 1 && offset != 0)
		{
			pos.Add(facing.GetCW(), offset);
		}
		IterateOverEach(pos, rotRad, invertHandle, delegate(BlockPos mpos)
		{
			if (!world.BlockAccessor.GetBlock(mpos, 1).IsReplacableBy(block))
			{
				blocked = true;
				return false;
			}
			return true;
		});
		if (blocked)
		{
			handling = EnumHandling.PreventDefault;
			failureCode = "notenoughspace";
			return false;
		}
		return base.CanPlaceBlock(world, byPlayer, blockSel, ref handling, ref failureCode);
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
	{
		handling = EnumHandling.PreventDefault;
		BlockPos pos = blockSel.Position.Copy();
		IBlockAccessor ba = world.BlockAccessor;
		if (block.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
		{
			float rotRad = BEBehaviorDoor.getRotateYRad(byPlayer, blockSel);
			BlockFacing facing = BlockFacing.HorizontalFromYaw(rotRad);
			if (HasCombinableLeftDoor(world, rotRad, blockSel.Position, width, out var otherDoor, out var offset))
			{
				if (width > 1 && offset != 0)
				{
					pos.Add(facing.GetCCW(), offset);
				}
			}
			else if (HasCombinableRightDoor(world, rotRad, blockSel.Position, width, out otherDoor, out offset) && width > 1 && offset != 0)
			{
				pos.Add(facing.GetCW(), offset);
			}
			return placeDoor(world, byPlayer, itemstack, blockSel, pos, ba);
		}
		return false;
	}

	public bool placeDoor(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, BlockPos pos, IBlockAccessor ba)
	{
		ba.SetBlock(block.BlockId, pos, itemstack);
		(ba.GetBlockEntity(pos)?.GetBehavior<BEBehaviorDoor>()).OnBlockPlaced(itemstack, byPlayer, blockSel);
		if (world.Side == EnumAppSide.Server)
		{
			placeMultiblockParts(world, pos);
		}
		return true;
	}

	public void placeMultiblockParts(IWorldAccessor world, BlockPos pos)
	{
		BEBehaviorDoor beh = world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorDoor>();
		float rotRad = beh?.RotateYRad ?? 0f;
		IterateOverEach(pos, rotRad, beh?.InvertHandles ?? false, delegate(BlockPos mpos)
		{
			if (mpos == pos)
			{
				return true;
			}
			int num = mpos.X - pos.X;
			int num2 = mpos.Y - pos.Y;
			int num3 = mpos.Z - pos.Z;
			string text = ((num < 0) ? "n" : ((num > 0) ? "p" : "")) + Math.Abs(num);
			string text2 = ((num2 < 0) ? "n" : ((num2 > 0) ? "p" : "")) + Math.Abs(num2);
			string text3 = ((num3 < 0) ? "n" : ((num3 > 0) ? "p" : "")) + Math.Abs(num3);
			AssetLocation blockCode = new AssetLocation("multiblock-monolithic-" + text + "-" + text2 + "-" + text3);
			Block block = world.GetBlock(blockCode);
			world.BlockAccessor.SetBlock(block.Id, mpos);
			if (world.Side == EnumAppSide.Server)
			{
				world.BlockAccessor.TriggerNeighbourBlockUpdate(mpos);
			}
			return true;
		});
	}

	public override void OnBlockRemoved(IWorldAccessor world, BlockPos pos, ref EnumHandling handling)
	{
		if (world.Side == EnumAppSide.Client)
		{
			return;
		}
		BEBehaviorDoor beh = world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorDoor>();
		float rotRad = beh?.RotateYRad ?? 0f;
		IterateOverEach(pos, rotRad, beh?.InvertHandles ?? false, delegate(BlockPos mpos)
		{
			if (mpos == pos)
			{
				return true;
			}
			if (world.BlockAccessor.GetBlock(mpos) is BlockMultiblock)
			{
				world.BlockAccessor.SetBlock(0, mpos);
				if (world.Side == EnumAppSide.Server)
				{
					world.BlockAccessor.TriggerNeighbourBlockUpdate(mpos);
				}
			}
			return true;
		});
		base.OnBlockRemoved(world, pos, ref handling);
	}

	public void IterateOverEach(BlockPos pos, float yRotRad, bool invertHandle, ActionConsumable<BlockPos> onBlock)
	{
		BlockPos tmpPos = new BlockPos(pos.dimension);
		for (int dx = 0; dx < width; dx++)
		{
			for (int dy = 0; dy < height; dy++)
			{
				for (int dz = 0; dz < width; dz++)
				{
					Vec3i offset = BEBehaviorDoor.getAdjacentOffset(dx, dz, dy, yRotRad, invertHandle);
					tmpPos.Set(pos.X + offset.X, pos.Y + offset.Y, pos.Z + offset.Z);
					if (!onBlock(tmpPos))
					{
						return;
					}
				}
			}
		}
	}

	public Cuboidf[] MBGetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos, Vec3i offset)
	{
		return getColSelBoxes(blockAccessor, pos, offset);
	}

	public Cuboidf[] MBGetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos, Vec3i offset)
	{
		return getColSelBoxes(blockAccessor, pos, offset);
	}

	private static Cuboidf[] getColSelBoxes(IBlockAccessor blockAccessor, BlockPos pos, Vec3i offset)
	{
		BEBehaviorDoor beh = blockAccessor.GetBlockEntity(pos.AddCopy(offset.X, offset.Y, offset.Z))?.GetBehavior<BEBehaviorDoor>();
		if (beh == null)
		{
			return null;
		}
		Vec3i rightBackOffset = beh.getAdjacentOffset(-1, -1);
		if (offset.X == rightBackOffset.X && offset.Z == rightBackOffset.Z)
		{
			return null;
		}
		if (beh.Opened)
		{
			Vec3i rightOffset = beh.getAdjacentOffset(-1);
			if (offset.X == rightOffset.X && offset.Z == rightOffset.Z)
			{
				return null;
			}
		}
		else
		{
			Vec3i backOffset = beh.getAdjacentOffset(0, -1);
			if (offset.X == backOffset.X && offset.Z == backOffset.Z)
			{
				return null;
			}
		}
		return beh.ColSelBoxes;
	}

	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventSubsequent;
		return blockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorDoor>()?.ColSelBoxes ?? null;
	}

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventSubsequent;
		return blockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorDoor>()?.ColSelBoxes ?? null;
	}

	public override Cuboidf[] GetParticleCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventSubsequent;
		return blockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorDoor>()?.ColSelBoxes ?? null;
	}

	public override Cuboidf GetParticleBreakBox(IBlockAccessor blockAccess, BlockPos pos, BlockFacing facing, ref EnumHandling handled)
	{
		return base.GetParticleBreakBox(blockAccess, pos, facing, ref handled);
	}

	public override void GetDecal(IWorldAccessor world, BlockPos pos, ITexPositionSource decalTexSource, ref MeshData decalModelData, ref MeshData blockModelData, ref EnumHandling handled)
	{
		BEBehaviorDoor beh = world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorDoor>();
		if (beh.Opened)
		{
			float rot = (beh.InvertHandles ? 90 : (-90));
			decalModelData = decalModelData.Rotate(0f, rot * ((float)Math.PI / 180f), 0f);
			if (!beh.InvertHandles)
			{
				decalModelData = decalModelData.Scale(1f, 1f, -1f);
			}
		}
		base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData, ref handled);
	}

	public override void GetHeldItemName(StringBuilder sb, ItemStack itemStack)
	{
		doorNameWithMaterial(sb);
	}

	public override void GetPlacedBlockName(StringBuilder sb, IWorldAccessor world, BlockPos pos)
	{
	}

	private void doorNameWithMaterial(StringBuilder sb)
	{
		if (block.Variant.ContainsKey("wood"))
		{
			string doorname = sb.ToString();
			sb.Clear();
			sb.Append(Lang.Get("doorname-with-material", doorname, Lang.Get("material-" + block.Variant["wood"])));
		}
	}

	public override float GetLiquidBarrierHeightOnSide(BlockFacing face, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		BEBehaviorDoor beh = block.GetBEBehavior<BEBehaviorDoor>(pos);
		if (beh == null)
		{
			return 0f;
		}
		if (!beh.IsSideSolid(face))
		{
			return 0f;
		}
		if (liquidBarrierHeight != null)
		{
			return liquidBarrierHeight[height - 1][0];
		}
		if (!airtight)
		{
			return 0f;
		}
		return 1f;
	}

	public float MBGetLiquidBarrierHeightOnSide(BlockFacing face, BlockPos pos, Vec3i offset)
	{
		if (offset.X != 0 && offset.Z != 0)
		{
			return 0f;
		}
		BEBehaviorDoor beh = block.GetBEBehavior<BEBehaviorDoor>(pos.AddCopy(offset.X, offset.Y, offset.Z));
		if (beh == null)
		{
			return 0f;
		}
		EnumAxis axis = (beh.Opened ? beh.facingWhenOpened : beh.facingWhenClosed).Axis;
		if (!beh.IsSideSolid(face) || (axis == EnumAxis.X && offset.X != 0) || (axis == EnumAxis.Z && offset.Z != 0))
		{
			return 0f;
		}
		if (liquidBarrierHeight != null)
		{
			int sideOffset = ((axis == EnumAxis.X) ? Math.Abs(offset.Z) : Math.Abs(offset.X));
			return liquidBarrierHeight[height + offset.Y - 1][sideOffset];
		}
		if (!airtight)
		{
			return 0f;
		}
		return 1f;
	}

	public override int GetRetention(BlockPos pos, BlockFacing facing, EnumRetentionType type, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		BEBehaviorDoor beh = block.GetBEBehavior<BEBehaviorDoor>(pos);
		if (beh == null)
		{
			return 0;
		}
		if (type == EnumRetentionType.Sound)
		{
			if (!beh.IsSideSolid(facing))
			{
				return 0;
			}
			return 3;
		}
		if (!airtight)
		{
			return 0;
		}
		if (api.World.Config.GetBool("openDoorsNotSolid"))
		{
			if (!beh.IsSideSolid(facing))
			{
				return 0;
			}
			return getInsulation(pos);
		}
		if (!beh.IsSideSolid(facing) && !beh.IsSideSolid(facing.Opposite))
		{
			return 3;
		}
		return getInsulation(pos);
	}

	public int MBGetRetention(BlockPos pos, BlockFacing facing, EnumRetentionType type, Vec3i offset)
	{
		BlockPos offsetPos = pos.AddCopy(offset.X, offset.Y, offset.Z);
		BEBehaviorDoor beh = block.GetBEBehavior<BEBehaviorDoor>(offsetPos);
		if (beh == null)
		{
			return 0;
		}
		if (type == EnumRetentionType.Sound)
		{
			EnumAxis axis = (beh.Opened ? beh.facingWhenOpened : beh.facingWhenClosed).Axis;
			if ((axis == EnumAxis.X && offset.X != 0) || (axis == EnumAxis.Z && offset.Z != 0))
			{
				return 0;
			}
			if (!beh.IsSideSolid(facing))
			{
				return 0;
			}
			return 3;
		}
		if (!airtight)
		{
			return 0;
		}
		if (api.World.Config.GetBool("openDoorsNotSolid"))
		{
			EnumAxis axis2 = (beh.Opened ? beh.facingWhenOpened : beh.facingWhenClosed).Axis;
			if ((axis2 == EnumAxis.X && offset.X != 0) || (axis2 == EnumAxis.Z && offset.Z != 0))
			{
				return 0;
			}
			if (!beh.IsSideSolid(facing))
			{
				return 0;
			}
			return getInsulation(offsetPos);
		}
		if (!beh.IsSideSolid(facing) && !beh.IsSideSolid(facing.Opposite))
		{
			return 3;
		}
		return getInsulation(offsetPos);
	}

	private int getInsulation(BlockPos pos)
	{
		EnumBlockMaterial mat = block.GetBlockMaterial(api.World.BlockAccessor, pos);
		if (mat == EnumBlockMaterial.Ore || mat == EnumBlockMaterial.Stone || mat == EnumBlockMaterial.Soil || mat == EnumBlockMaterial.Ceramic)
		{
			return -1;
		}
		return 1;
	}

	public bool MBCanAttachBlockAt(IBlockAccessor blockAccessor, Block block, BlockPos pos, BlockFacing blockFace, Cuboidi attachmentArea, Vec3i offsetInv)
	{
		return false;
	}

	public JsonObject MBGetAttributes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		return null;
	}

	public override bool SideIsSolid(BlockPos pos, int faceIndex, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventSubsequent;
		return block.GetBEBehavior<BEBehaviorDoor>(pos)?.IsSideSolid(BlockFacing.ALLFACES[faceIndex]) ?? false;
	}

	public bool MBSideIsSolid(BlockPos pos, int faceIndex, Vec3i offset)
	{
		BlockPos offsetPos = pos.AddCopy(offset.X, offset.Y, offset.Z);
		return block.GetBEBehavior<BEBehaviorDoor>(offsetPos)?.IsSideSolid(BlockFacing.ALLFACES[faceIndex]) ?? false;
	}

	public override bool SideIsSolid(IBlockAccessor blockAccess, BlockPos pos, int faceIndex, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventSubsequent;
		return block.GetBEBehavior<BEBehaviorDoor>(pos)?.IsSideSolid(BlockFacing.ALLFACES[faceIndex]) ?? false;
	}

	public bool MBSideIsSolid(IBlockAccessor blockAccess, BlockPos pos, int faceIndex, Vec3i offset)
	{
		BlockPos offsetPos = pos.AddCopy(offset.X, offset.Y, offset.Z);
		return block.GetBEBehavior<BEBehaviorDoor>(offsetPos)?.IsSideSolid(BlockFacing.ALLFACES[faceIndex]) ?? false;
	}
}
