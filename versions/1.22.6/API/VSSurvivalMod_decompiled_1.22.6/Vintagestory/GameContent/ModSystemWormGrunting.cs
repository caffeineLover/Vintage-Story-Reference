using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.CommandAbbr;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class ModSystemWormGrunting : ModSystem
{
	protected NormalizedSimplexNoise noiseGen;

	protected ICoreServerAPI sapi;

	protected Dictionary<BlockPos, CreatureHarvest> harvestedLocations = new Dictionary<BlockPos, CreatureHarvest>();

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
		api.Event.ServerRunPhase(EnumServerRunPhase.WorldReady, onWorldReady);
		api.Event.SaveGameLoaded += Event_SaveGameLoaded;
		api.Event.GameWorldSave += Event_GameWorldSave;
		api.Event.RegisterGameTickListener(restoreEarthWorms, 10000, sapi.World.Rand.Next(1000));
		sapi.ChatCommands.GetOrCreate("wgen").BeginSub("wormmap").WithDesc("generates a bunch of blocks at y=150 to visualize worm density map")
			.HandleWith(onCmdWormMap);
	}

	private TextCommandResult onCmdWormMap(TextCommandCallingArgs args)
	{
		for (int dx = -15; dx <= 15; dx++)
		{
			for (int dz = -15; dz <= 15; dz++)
			{
				BlockPos pos = args.Caller.Pos.AsBlockPos.Add(dx, 0, dz);
				pos.Y = 150;
				float f = GetInitialDensity(pos);
				int num = (int)GameMath.Clamp(Math.Round(64f + 15f * f), 64.0, 79.0);
				Block block = sapi.World.GetBlock("creativeblock-" + num);
				sapi.World.BlockAccessor.SetBlock(block.Id, pos);
			}
		}
		return TextCommandResult.Success("generated");
	}

	private void Event_GameWorldSave()
	{
		sapi.WorldManager.SaveGame.StoreData("harvestedEarthWormLocations", harvestedLocations);
	}

	private void Event_SaveGameLoaded()
	{
		try
		{
			harvestedLocations = sapi.WorldManager.SaveGame.GetData<Dictionary<BlockPos, CreatureHarvest>>("harvestedEarthWormLocations");
		}
		catch
		{
		}
		if (harvestedLocations == null)
		{
			harvestedLocations = new Dictionary<BlockPos, CreatureHarvest>();
		}
	}

	private void restoreEarthWorms(float dt)
	{
		List<BlockPos> list = new List<BlockPos>(harvestedLocations.Keys);
		double totaldays = sapi.World.Calendar.TotalDays;
		foreach (BlockPos pos in list)
		{
			if (totaldays - harvestedLocations[pos].TotalDays > 7.0)
			{
				harvestedLocations.Remove(pos);
			}
		}
	}

	private void onWorldReady()
	{
		noiseGen = NormalizedSimplexNoise.FromDefaultOctaves(2, 1.0, 0.9, sapi.World.Seed);
	}

	public float GetInitialDensity(BlockPos pos)
	{
		float num = Math.Max(0f, (float)(noiseGen.Noise(pos.X, pos.Z) - 0.5) * 3f);
		ClimateCondition climate = sapi.World.BlockAccessor.GetClimateAt(pos, EnumGetClimateMode.WorldGenValues);
		return num * Math.Max(0f, (climate.Fertility - 0.2f) * 1.3f);
	}

	public float GetEarthWormAmount(BlockPos pos)
	{
		float availableWorms = GetInitialDensity(pos) * 10f;
		float temp = sapi.World.BlockAccessor.GetClimateAt(pos).Temperature;
		availableWorms *= GameMath.Clamp(temp / 10f, 0f, 1f);
		if (harvestedLocations.TryGetValue(pos / 3, out var harvest))
		{
			availableWorms -= (float)harvest.Quantity;
		}
		return availableWorms;
	}

	public void AddHarvest(BlockPos pos, int quantity)
	{
		harvestedLocations.TryGetValue(pos / 3, out var harvest);
		harvestedLocations[pos / 3] = new CreatureHarvest
		{
			TotalDays = sapi.World.Calendar.TotalDays,
			Quantity = harvest.Quantity + quantity
		};
	}
}
