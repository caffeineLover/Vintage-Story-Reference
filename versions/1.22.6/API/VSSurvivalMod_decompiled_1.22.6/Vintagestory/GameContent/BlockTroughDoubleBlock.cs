using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockTroughDoubleBlock : BlockTroughBase
{
	public override void OnLoaded(ICoreAPI api)
	{
		if (Variant["part"] == "large-feet")
		{
			RootOffset.Set(BlockFacing.FromCode(Variant["side"]).Opposite.Normali);
		}
		base.OnLoaded(api);
		init();
	}

	public BlockFacing OtherPartFacing()
	{
		BlockFacing facing = BlockFacing.FromCode(Variant["side"]);
		if (Variant["part"] == "large-feet")
		{
			facing = facing.Opposite;
		}
		return facing;
	}

	public BlockPos OtherPartPos(BlockPos pos)
	{
		BlockFacing facing = BlockFacing.FromCode(Variant["side"]);
		if (Variant["part"] == "large-feet")
		{
			facing = facing.Opposite;
		}
		return pos.AddCopy(facing);
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
	{
		if (CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
		{
			BlockFacing[] horVer = Block.SuggestedHVOrientation(byPlayer, blockSel);
			BlockPos secondPos = blockSel.Position.AddCopy(horVer[0]);
			if (!CanPlaceBlock(world, byPlayer, new BlockSelection
			{
				Position = secondPos,
				Face = blockSel.Face
			}, ref failureCode))
			{
				return false;
			}
			string code = horVer[0].Opposite.Code;
			world.BlockAccessor.GetBlock(CodeWithVariants(new string[2] { "part", "side" }, new string[2] { "large-head", code })).DoPlaceBlock(world, byPlayer, new BlockSelection
			{
				Position = secondPos,
				Face = blockSel.Face
			}, itemstack);
			AssetLocation feetCode = CodeWithVariants(new string[2] { "part", "side" }, new string[2] { "large-feet", code });
			world.BlockAccessor.GetBlock(feetCode).DoPlaceBlock(world, byPlayer, blockSel, itemstack);
			return true;
		}
		return false;
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (blockSel != null)
		{
			bool ok = (world.BlockAccessor.GetBlockEntity(blockSel.Position.AddCopy(RootOffset)) as BlockEntityTrough)?.OnInteract(byPlayer, blockSel) ?? false;
			if (ok && world.Side == EnumAppSide.Client)
			{
				(byPlayer as IClientPlayer).TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
				return true;
			}
			return ok;
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override void OnBlockRemoved(IWorldAccessor world, BlockPos pos)
	{
		BlockFacing facing = BlockFacing.FromCode(Variant["side"]);
		if (Variant["part"] == "large-feet")
		{
			facing = facing.Opposite;
		}
		Block secondPlock = world.BlockAccessor.GetBlock(pos.AddCopy(facing));
		if (secondPlock is BlockTroughDoubleBlock && secondPlock.Variant["part"] != Variant["part"])
		{
			if (Variant["part"] == "large-feet")
			{
				(world.BlockAccessor.GetBlockEntity(pos.AddCopy(facing)) as BlockEntityTrough)?.OnBlockBroken();
			}
			world.BlockAccessor.SetBlock(0, pos.AddCopy(facing));
		}
		base.OnBlockRemoved(world, pos);
	}

	public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
	{
		return GetHandbookDropsFromBreakDrops(handbookStack, forPlayer);
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		return new ItemStack[1]
		{
			new ItemStack(world.BlockAccessor.GetBlock(CodeWithVariants(new string[2] { "part", "side" }, new string[2] { "large-head", "north" })))
		};
	}

	public override AssetLocation GetRotatedBlockCode(int angle)
	{
		int rotatedIndex = GameMath.Mod(BlockFacing.FromCode(Variant["side"]).HorizontalAngleIndex - angle / 90, 4);
		BlockFacing nowFacing = BlockFacing.HORIZONTALS_ANGLEORDER[rotatedIndex];
		return CodeWithParts(nowFacing.Code);
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		return new ItemStack(world.BlockAccessor.GetBlock(CodeWithVariants(new string[2] { "part", "side" }, new string[2] { "large-head", "north" })));
	}

	public override AssetLocation GetHorizontallyFlippedBlockCode(EnumAxis axis)
	{
		BlockFacing facing = BlockFacing.FromCode(Variant["side"]);
		if (facing.Axis == axis)
		{
			return CodeWithParts(facing.Opposite.Code);
		}
		return Code;
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
	}

	public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer)
	{
		if (Variant["part"] == "large-feet")
		{
			BlockFacing facing = BlockFacing.FromCode(Variant["side"]).Opposite;
			pos = pos.AddCopy(facing);
		}
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityTrough betr)
		{
			StringBuilder dsc = new StringBuilder();
			betr.GetBlockInfo(forPlayer, dsc);
			return dsc.ToString();
		}
		return base.GetPlacedBlockInfo(world, pos, forPlayer);
	}

	public override int GetRandomColor(ICoreClientAPI capi, BlockPos pos, BlockFacing facing, int rndIndex = -1)
	{
		if (Textures.TryGetValue("aged", out var ctex))
		{
			capi.BlockTextureAtlas.GetRandomColor(ctex.Baked.TextureSubId, rndIndex);
		}
		return base.GetRandomColor(capi, pos, facing, rndIndex);
	}

	public override int GetColorWithoutTint(ICoreClientAPI capi, BlockPos pos)
	{
		if (Textures.TryGetValue("aged", out var ctex))
		{
			return capi.BlockTextureAtlas.GetAverageColor(ctex.Baked.TextureSubId);
		}
		return base.GetColorWithoutTint(capi, pos);
	}
}
