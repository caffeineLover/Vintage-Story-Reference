using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public interface IBellowsAirReceiver
{
	void BlowAirInto(IWorldAccessor world, BlockPos pos, float amount, BlockFacing direction);
}
