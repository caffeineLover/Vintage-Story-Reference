using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common;

public delegate bool TrySpawnGroupDelegate(IBlockAccessor blockaccessor, EntityProperties props, BlockPos pos);
