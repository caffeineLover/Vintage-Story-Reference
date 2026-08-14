using Vintagestory.API.Client;

namespace Vintagestory.GameContent;

public interface IFallingBlockMeshSource
{
	string GetCacheKey(EntityBlockFalling fallingBlock);

	void OnFallingBlockTesselation(EntityBlockFalling fallingBlock, ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator);
}
