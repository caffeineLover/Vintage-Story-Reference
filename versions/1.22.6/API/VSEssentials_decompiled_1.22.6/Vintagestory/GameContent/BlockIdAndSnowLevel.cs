using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public struct BlockIdAndSnowLevel(Block block, float snowLevel)
{
	public Block Block = block;

	public float SnowLevel = snowLevel;
}
