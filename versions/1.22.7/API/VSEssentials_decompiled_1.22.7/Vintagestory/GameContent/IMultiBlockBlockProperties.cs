using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public interface IMultiBlockBlockProperties
{
	bool MBCanAttachBlockAt(IBlockAccessor blockAccessor, Block block, BlockPos pos, BlockFacing blockFace, Cuboidi attachmentArea, Vec3i offsetInv);

	bool MBSideIsSolid(BlockPos pos, int faceIndex, Vec3i offsetInv)
	{
		return false;
	}

	bool MBSideIsSolid(IBlockAccessor blockAccess, BlockPos pos, int faceIndex, Vec3i offsetInv)
	{
		return false;
	}

	float MBGetLiquidBarrierHeightOnSide(BlockFacing face, BlockPos pos, Vec3i offsetInv);

	int MBGetRetention(BlockPos pos, BlockFacing facing, EnumRetentionType type, Vec3i offsetInv);

	JsonObject MBGetAttributes(IBlockAccessor blockAccessor, BlockPos pos);
}
