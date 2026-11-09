using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class ModSystemClimateSpecificTraderTypes : ModSystem
{
	private ICoreServerAPI sapi;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Server;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		sapi = api;
		api.Event.RegisterEventBusListener(onSpawnerSpawnAttempt, 1.0, "onattemptspawnerspawn");
	}

	private void onSpawnerSpawnAttempt(string eventName, ref EnumHandling handling, IAttribute data)
	{
		TreeAttribute tree = data as TreeAttribute;
		string type = tree.GetString("type");
		if (type.StartsWith("trader"))
		{
			ClimateCondition conds = sapi.World.BlockAccessor.GetClimateAt(tree.GetBlockPos("pos"), EnumGetClimateMode.WorldGenValues);
			if (conds.Temperature >= 15f && (double)conds.Rainfall <= 0.4)
			{
				tree.SetString("type", type.Replace("-temperate", "-desert"));
			}
			else if (conds.Temperature < -4f)
			{
				tree.SetString("type", type.Replace("-temperate", "-cold"));
			}
		}
	}
}
