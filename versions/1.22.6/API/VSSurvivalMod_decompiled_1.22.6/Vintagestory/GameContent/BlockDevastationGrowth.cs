using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.ServerMods;

namespace Vintagestory.GameContent;

public class BlockDevastationGrowth : Block
{
	public override bool TryPlaceBlockForWorldGen(IBlockAccessor blockAccessor, BlockPos pos, BlockFacing onBlockFace, IRandom worldGenRand, BlockPatchAttributes attributes = null)
	{
		int belowBlockId = blockAccessor.GetBlockBelow(pos, 1, 1).Id;
		if (!GenDevastationLayer.DevastationBlockIds.Contains(belowBlockId))
		{
			return false;
		}
		if (blockAccessor.GetBlock(pos.DownCopy(), 1) is BlockDevastationGrowth)
		{
			return false;
		}
		return base.TryPlaceBlockForWorldGen(blockAccessor, pos, onBlockFace, worldGenRand, attributes);
	}
}
