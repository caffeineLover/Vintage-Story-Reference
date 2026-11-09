using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class BlockEntityBerryBushFarmland : BlockEntitySoilNutrition
{
	protected double lastOccupiedTotalDays;

	protected bool HasBerryBush => Api.World.BlockAccessor.GetBlockEntity(upPos)?.GetBehavior<BEBehaviorFruitingBush>() != null;

	protected override bool ConsidersMoistureLevels => false;

	protected override int RainHeightOffset => HasBerryBush ? 1 : 0;

	protected override bool RecoverFertility => !HasBerryBush;

	protected override void beginIntervalledUpdate(out FarmlandFastForwardUpdate onInterval, out FarmlandUpdateEnd onEnd)
	{
		base.beginIntervalledUpdate(out onInterval, out onEnd);
		FarmlandFastForwardUpdate baseOnInterval = onInterval;
		BEBehaviorFruitingBush bhfb = Api.World.BlockAccessor.GetBlockEntity(upPos)?.GetBehavior<BEBehaviorFruitingBush>();
		if (bhfb == null)
		{
			if (Api.World.Calendar.TotalDays - lastOccupiedTotalDays > (double)Api.World.Calendar.DaysPerYear)
			{
				Api.World.BlockAccessor.RemoveBlockEntity(Pos);
			}
			return;
		}
		lastOccupiedTotalDays = Api.World.Calendar.TotalDays;
		FarmlandFastForwardUpdate callback = bhfb.onUpdate();
		onInterval = delegate(double hourIntervall, ClimateCondition conds, double lightGrowthSpeedFactor, bool growthPaused)
		{
			baseOnInterval?.Invoke(hourIntervall, conds, lightGrowthSpeedFactor, growthPaused);
			callback?.Invoke(hourIntervall, conds, lightGrowthSpeedFactor, growthPaused);
		};
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		lastOccupiedTotalDays = tree.GetDouble("lastOccupiedTotalDays");
		base.FromTreeAttributes(tree, worldForResolving);
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		tree.SetDouble("lastOccupiedTotalDays", lastOccupiedTotalDays);
		base.ToTreeAttributes(tree);
	}
}
