using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace Vintagestory.GameContent;

public class BlockEntityWildFruitingBush : BlockEntityFastForwardGrowth
{
	protected override int RainHeightOffset => 0;

	protected override void beginIntervalledUpdate(out FarmlandFastForwardUpdate onInterval, out FarmlandUpdateEnd onEnd)
	{
		base.beginIntervalledUpdate(out onInterval, out onEnd);
		FarmlandFastForwardUpdate baseOnInterval = onInterval;
		BEBehaviorFruitingBush bhfb = GetBehavior<BEBehaviorFruitingBush>();
		if (bhfb != null)
		{
			FarmlandFastForwardUpdate callback = bhfb.onUpdate();
			onInterval = delegate(double hourIntervall, ClimateCondition conds, double lightGrowthSpeedFactor, bool growthPaused)
			{
				baseOnInterval?.Invoke(hourIntervall, conds, lightGrowthSpeedFactor, growthPaused);
				callback?.Invoke(hourIntervall, conds, lightGrowthSpeedFactor, growthPaused);
			};
		}
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		base.GetBlockInfo(forPlayer, dsc);
		if (roomness > 0)
		{
			dsc.AppendLine();
			dsc.AppendLine(Lang.Get("greenhousetempbonus"));
		}
	}
}
