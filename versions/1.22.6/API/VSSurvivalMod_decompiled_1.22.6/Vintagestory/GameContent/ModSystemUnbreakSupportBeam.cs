using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class ModSystemUnbreakSupportBeam : ModSystem
{
	private ICoreClientAPI capi;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Client;
	}

	public override void StartClientSide(ICoreClientAPI api)
	{
		capi = api;
		api.Event.OnUnBreakingBlock += Event_OnUnBreakingBlock;
	}

	private void Event_OnUnBreakingBlock(BlockDamage bd)
	{
		IBlockAccessor blockAccessor = capi.World.BlockAccessor;
		Block block = blockAccessor.GetBlock(bd.Position);
		BEBehaviorSupportBeam be = blockAccessor.GetBlockEntity(bd.Position)?.GetBehavior<BEBehaviorSupportBeam>();
		if (be?.Beams != null)
		{
			PlacedBeam[] beams = be.Beams;
			foreach (PlacedBeam obj in beams)
			{
				obj.RemainingResistance = GameMath.Min(obj.RemainingResistance + 0.1f * block.Resistance, block.Resistance);
			}
		}
	}
}
