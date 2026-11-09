using Vintagestory.API.Datastructures;

namespace Vintagestory.API.Common;

public delegate void PreventTerrainHeightSmoothingDelegate(int chunkX, int chunkZ, BoolRef prevent);
