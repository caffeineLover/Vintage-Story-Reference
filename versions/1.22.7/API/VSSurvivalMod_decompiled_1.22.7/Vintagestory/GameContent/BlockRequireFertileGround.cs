using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockRequireFertileGround : Block
{
	protected bool disappearOnSoilRemoved;

	protected bool isWaterPlant;

	public virtual bool skipPlantCheck { get; set; }

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		disappearOnSoilRemoved = Attributes?["disappearOnSoilRemoved"].AsBool() ?? false;
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
	{
		if (skipPlantCheck)
		{
			return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
		}
		if (Variant.ContainsKey("side"))
		{
			return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
		}
		if (CanPlantStay(world.BlockAccessor, blockSel.Position))
		{
			return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
		}
		failureCode = "requirefertileground";
		return false;
	}

	public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
	{
		if (!skipPlantCheck && !CanPlantStay(world.BlockAccessor, pos))
		{
			if (world.BlockAccessor.GetBlock(pos.DownCopy()).Id == 0 && disappearOnSoilRemoved)
			{
				world.BlockAccessor.SetBlock(0, pos);
			}
			else
			{
				world.BlockAccessor.BreakBlock(pos, null);
			}
		}
		base.OnNeighbourBlockChange(world, pos, neibpos);
	}

	public virtual bool CanPlantStay(IBlockAccessor blockAccessor, BlockPos pos)
	{
		if (Variant.ContainsKey("side"))
		{
			BlockFacing facing = BlockFacing.FromCode(Variant["side"]);
			BlockPos npos = pos.AddCopy(facing);
			return blockAccessor.GetBlock(npos).CanAttachBlockAt(blockAccessor, this, npos, facing.Opposite);
		}
		if (blockAccessor.GetBlockBelow(pos).Fertility <= 0)
		{
			return false;
		}
		return true;
	}

	public override bool TryPlaceBlockForWorldGen(IBlockAccessor blockAccessor, BlockPos pos, BlockFacing onBlockFace, IRandom worldGenRand, BlockPatchAttributes attributes = null)
	{
		if (!CanPlantStay(blockAccessor, pos))
		{
			return false;
		}
		if (isWaterPlant)
		{
			bool canPlace = true;
			BlockPos tmpPos = pos.Copy();
			for (int x = -1; x < 2; x++)
			{
				for (int z = -1; z < 2; z++)
				{
					tmpPos.Set(pos.X + x, pos.Y, pos.Z + z);
					if (blockAccessor.GetBlock(tmpPos, 1) is BlockWaterLilyGiant)
					{
						canPlace = false;
					}
				}
			}
			if (!canPlace)
			{
				return false;
			}
		}
		else if (blockAccessor.GetBlock(pos, 2).Id != 0)
		{
			return false;
		}
		return base.TryPlaceBlockForWorldGen(blockAccessor, pos, onBlockFace, worldGenRand, attributes);
	}

	public override void OnBeingLookedAt(IPlayer byPlayer, BlockSelection blockSel, bool firstTick)
	{
		if (firstTick && api is ICoreServerAPI sapi)
		{
			(sapi.World.BlockAccessor.GetBlockEntity(blockSel.Position) as BlockEntityFastForwardGrowth)?.SendFullUpdateToClient(sapi, (IServerPlayer)byPlayer);
		}
	}
}
