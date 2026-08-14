using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class ModSystemFishDepletion : ModSystem
{
	protected ICoreServerAPI sapi;

	protected Dictionary<BlockPos, CreatureHarvest> harvestedLocations = new Dictionary<BlockPos, CreatureHarvest>();

	public int Scale = 8;

	public static int MaxHarvestablePerLocation = 12;

	public static double RestoreFishAfterDays = 14.0;

	public override bool ShouldLoad(EnumAppSide forSide)
	{
		return forSide == EnumAppSide.Server;
	}

	public override double ExecuteOrder()
	{
		return 1.0;
	}

	public override void StartServerSide(ICoreServerAPI api)
	{
		base.StartServerSide(api);
		sapi = api;
		api.Event.SaveGameLoaded += Event_SaveGameLoaded;
		api.Event.GameWorldSave += Event_GameWorldSave;
		api.Event.RegisterGameTickListener(restoreFish, 10000, sapi.World.Rand.Next(1000));
	}

	private void Event_GameWorldSave()
	{
		sapi.WorldManager.SaveGame.StoreData("harvestedFishLocations", harvestedLocations);
	}

	private void Event_SaveGameLoaded()
	{
		try
		{
			harvestedLocations = sapi.WorldManager.SaveGame.GetData<Dictionary<BlockPos, CreatureHarvest>>("harvestedFishLocations");
		}
		catch
		{
		}
		if (harvestedLocations == null)
		{
			harvestedLocations = new Dictionary<BlockPos, CreatureHarvest>();
		}
	}

	private void restoreFish(float dt)
	{
		List<BlockPos> list = new List<BlockPos>(harvestedLocations.Keys);
		double totaldays = sapi.World.Calendar.TotalDays;
		foreach (BlockPos pos in list)
		{
			if (totaldays - harvestedLocations[pos].TotalDays > RestoreFishAfterDays)
			{
				harvestedLocations.Remove(pos);
			}
		}
	}

	public void AddHarvest(BlockPos pos, int quantity)
	{
		harvestedLocations.TryGetValue(pos / Scale, out var harvest);
		harvestedLocations[pos / Scale] = new CreatureHarvest
		{
			TotalDays = sapi.World.Calendar.TotalDays,
			Quantity = harvest.Quantity + quantity
		};
	}

	public float GetHarvestAmount(BlockPos pos)
	{
		if (harvestedLocations.TryGetValue(pos / Scale, out var harvest))
		{
			return harvest.Quantity;
		}
		return 0f;
	}
}
