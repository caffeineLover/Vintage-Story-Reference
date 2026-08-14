using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockAntlerMount : BlockShapeMaterialFromAttributes
{
	public override string MeshKey => "AntlerMount";

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		float degY = ((!(blockAccessor.GetBlockEntity(pos) is BlockEntityAntlerMount bect)) ? 0f : (bect.MeshAngleRad * (180f / (float)Math.PI)));
		return new Cuboidf[1] { SelectionBoxes[0].RotatedCopy(0f, degY, 0f, new Vec3d(0.5, 0.5, 0.5)) };
	}

	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		return GetSelectionBoxes(blockAccessor, pos);
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
	{
		if (blockSel.Face.IsHorizontal)
		{
			if (TryAttachTo(world, byPlayer, blockSel, itemstack, ref failureCode))
			{
				return true;
			}
			if (failureCode == "entityintersecting")
			{
				return false;
			}
		}
		BlockFacing[] faces = BlockFacing.HORIZONTALS;
		blockSel = blockSel.Clone();
		for (int i = 0; i < faces.Length; i++)
		{
			blockSel.Face = faces[i];
			if (TryAttachTo(world, byPlayer, blockSel, itemstack, ref failureCode))
			{
				return true;
			}
		}
		failureCode = "requirehorizontalattachable";
		return false;
	}

	private bool TryAttachTo(IWorldAccessor world, IPlayer player, BlockSelection blockSel, ItemStack itemstack, ref string failureCode)
	{
		BlockFacing oppositeFace = blockSel.Face.Opposite;
		BlockPos attachingBlockPos = blockSel.Position.AddCopy(oppositeFace);
		if (world.BlockAccessor.GetBlock(attachingBlockPos).CanAttachBlockAt(world.BlockAccessor, this, attachingBlockPos, blockSel.Face) && CanPlaceBlock(world, player, blockSel, ref failureCode))
		{
			DoPlaceBlock(world, player, blockSel, itemstack);
			return true;
		}
		return false;
	}

	private bool CanBlockStay(IWorldAccessor world, BlockPos pos)
	{
		BlockFacing facing = BlockFacing.HorizontalFromAngle(((world.BlockAccessor.GetBlockEntity(pos) as BlockEntityAntlerMount)?.MeshAngleRad ?? 0f) + (float)Math.PI / 2f);
		return world.BlockAccessor.GetBlock(pos.AddCopy(facing)).CanAttachBlockAt(world.BlockAccessor, this, pos.AddCopy(facing), facing.Opposite);
	}

	public override bool CanAttachBlockAt(IBlockAccessor blockAccessor, Block block, BlockPos pos, BlockFacing blockFace, Cuboidi? attachmentArea = null)
	{
		return false;
	}

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
	{
		bool val = base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);
		if (val)
		{
			BEBehaviorShapeMaterialFromAttributes bect = world.BlockAccessor.GetBlockEntity(blockSel.Position).GetBehavior<BEBehaviorShapeMaterialFromAttributes>();
			if (bect != null)
			{
				for (int i = 0; i < 4; i++)
				{
					int faceIndex = (blockSel.Face.HorizontalAngleIndex + i) % 4;
					BlockPos attachingBlockPos = blockSel.Position.AddCopy(BlockFacing.HORIZONTALS_ANGLEORDER[faceIndex]);
					if (world.BlockAccessor.GetBlock(attachingBlockPos).CanAttachBlockAt(world.BlockAccessor, this, attachingBlockPos, blockSel.Face))
					{
						bect.MeshAngleY = (float)(faceIndex * 90) * ((float)Math.PI / 180f) - (float)Math.PI / 2f;
						bect.OnBlockPlaced(byItemStack);
					}
				}
			}
		}
		return val;
	}

	public override void GetDecal(IWorldAccessor world, BlockPos pos, ITexPositionSource decalTexSource, ref MeshData decalModelData, ref MeshData blockModelData)
	{
		BlockEntityAntlerMount beb = GetBlockEntity<BlockEntityAntlerMount>(pos);
		if (beb != null && beb.Type != null && beb.Material != null)
		{
			float[] mat = Matrixf.Create().Translate(0.5f, 0.5f, 0.5f).RotateY(beb.MeshAngleRad)
				.Translate(-0.5f, -0.5f, -0.5f)
				.Values;
			blockModelData = GetOrCreateMesh(beb.Type, beb.Material).Clone().MatrixTransform(mat);
			decalModelData = GetOrCreateMesh(beb.Type, beb.Material, null, decalTexSource).Clone().MatrixTransform(mat);
		}
		else
		{
			base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData);
		}
	}

	public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
	{
		base.OnNeighbourBlockChange(world, pos, neibpos);
		if (!CanBlockStay(world, pos))
		{
			world.BlockAccessor.BreakBlock(pos, null);
		}
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityAntlerMount beshelf)
		{
			return beshelf.OnInteract(byPlayer, blockSel);
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos)
	{
		if (!(world.BlockAccessor.GetBlockEntity(pos) is BlockEntityAntlerMount bemount))
		{
			return base.GetPlacedBlockName(world, pos);
		}
		if (bemount.Inventory.Empty)
		{
			return Lang.Get("block-antlermount-" + bemount.Type);
		}
		return bemount.Inventory[0].Itemstack.GetName();
	}

	public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer)
	{
		if (!(world.BlockAccessor.GetBlockEntity(pos) is BlockEntityAntlerMount bemount))
		{
			return base.GetPlacedBlockInfo(world, pos, forPlayer);
		}
		if (bemount.Inventory.Empty)
		{
			return base.GetPlacedBlockInfo(world, pos, forPlayer) + "\n" + Lang.Get("Material: {0}", Lang.Get("material-" + bemount.Material));
		}
		return base.GetPlacedBlockInfo(world, pos, forPlayer);
	}

	public override string GetHeldItemName(ItemStack itemStack)
	{
		string type = itemStack.Attributes.GetString("type", "square");
		return Lang.Get("block-" + Code.Path + "-" + type);
	}
}
