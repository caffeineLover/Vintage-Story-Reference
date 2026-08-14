using Vintagestory.API.Common;
using Vintagestory.ServerMods.NoObf;

namespace Vintagestory.ServerMods;

public interface IBlockPatchModifier
{
	bool PreventPlacementAt(int x, int z, int category);

	bool PreventPlacementBroadlyAt(int chunkX, int chunkZ);

	BlockPatchConfig GetPatchProviderAt(int chunkX, int chunkZ, ref EnumHandling handling);
}
