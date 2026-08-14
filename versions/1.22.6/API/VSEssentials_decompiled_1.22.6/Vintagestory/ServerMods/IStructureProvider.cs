using Vintagestory.API.Common;

namespace Vintagestory.ServerMods;

public interface IStructureProvider
{
	IStructureLocation GetBlockingStructureAt(int x, int z, int category);
}
