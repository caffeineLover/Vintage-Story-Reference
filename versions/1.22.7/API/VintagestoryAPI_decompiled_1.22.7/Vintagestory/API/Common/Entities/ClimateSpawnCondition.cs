using System;
using Newtonsoft.Json;

namespace Vintagestory.API.Common.Entities;

/// <summary>
/// A list of conditions based on climate.
/// </summary>
[DocumentAsJson]
public class ClimateSpawnCondition
{
	/// <summary>
	/// The minimum tempurature for the object to spawn.
	/// </summary>
	[DocumentAsJson("Optional", "-40", false)]
	public float MinTemp = -40f;

	/// <summary>
	/// The maximum tempurature for the object to spawn.
	/// </summary>
	[DocumentAsJson("Optional", "40", false)]
	public float MaxTemp = 40f;

	/// <summary>
	/// The minimum amount of rain for the object to spawn.
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public float MinRain;

	/// <summary>
	/// The maximum amount of rain for the object to spawn.
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	public float MaxRain = 1f;

	/// <summary>
	/// The minimum amount of forest cover needed for the object to spawn.
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public float MinForest;

	/// <summary>
	/// The maximum amount of forest cover needed for the object to spawn.
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	public float MaxForest = 1f;

	/// <summary>
	/// The minimum amount of shrubbery needed for the object to spawn.
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public float MinShrubs;

	/// <summary>
	/// The maximum amount of shrubbery needed for the object to spawn.
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	public float MaxShrubs = 1f;

	/// <summary>
	/// Won't span below minY. 0...1 is world bottom to sea level, 1...2 is sea level to world top
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public float MinY;

	/// <summary>
	/// Won't span above maxY. 0...1 is world bottom to sea level, 1...2 is sea level to world top
	/// </summary>
	[DocumentAsJson("Optional", "2", false)]
	public float MaxY = 2f;

	/// <summary>
	/// The minimum amount of forest or shrubs for the object to spawn.
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public float MinForestOrShrubs;

	/// <summary>
	/// If defined, further constrains the spawning areas by a large scale 2D density map
	/// </summary>
	[DocumentAsJson("Optional", "null", false)]
	public string[] RandomMapCodePool;

	[JsonIgnore]
	public string MapCode;

	/// <summary>
	/// If true, will check for suitable habitat size (e.g. when underwater and size of 2 will check that there is also water 2 blocks away in all directions
	/// For habitat land: Not implemented yet
	/// For habitat air: Not implemented yet
	/// </summary>
	[DocumentAsJson("Optional", "true", false)]
	public int HabitatSize = 2;

	public void SetFrom(ClimateSpawnCondition conds)
	{
		MinTemp = conds.MinTemp;
		MaxTemp = conds.MaxTemp;
		MinRain = conds.MinRain;
		MaxRain = conds.MaxRain;
		MinForest = conds.MinForest;
		MaxForest = conds.MaxForest;
		MinShrubs = conds.MinShrubs;
		MaxShrubs = conds.MaxShrubs;
		MinY = conds.MinY;
		MaxY = conds.MaxY;
		MinForestOrShrubs = conds.MinForestOrShrubs;
		MapCode = conds.MapCode;
		if (conds.RandomMapCodePool != null)
		{
			RandomMapCodePool = (string[])conds.RandomMapCodePool?.Clone();
		}
		HabitatSize = conds.HabitatSize;
	}

	public bool MatchesClimate(ClimateCondition climate)
	{
		if (MinTemp > climate.WorldGenTemperature || MaxTemp < climate.WorldGenTemperature)
		{
			return false;
		}
		if (MinRain > climate.Rainfall || MaxRain < climate.Rainfall)
		{
			return false;
		}
		return true;
	}

	public bool MatchesForestation(ClimateCondition climate)
	{
		if (MinForest > climate.ForestDensity || MaxForest < climate.ForestDensity)
		{
			return false;
		}
		if (MinShrubs > climate.ShrubDensity || MaxShrubs < climate.ShrubDensity)
		{
			return false;
		}
		if (MinForestOrShrubs > Math.Max(climate.ForestDensity, climate.ShrubDensity))
		{
			return false;
		}
		return true;
	}

	public ClimateSpawnCondition Clone()
	{
		ClimateSpawnCondition climateSpawnCondition = new ClimateSpawnCondition();
		climateSpawnCondition.SetFrom(this);
		return climateSpawnCondition;
	}
}
