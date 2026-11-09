namespace Vintagestory.API.Common.Entities;

/// <summary>
/// A set of spawn conditions for chunks that have already been generated. Most properties are got from <see cref="T:Vintagestory.API.Common.Entities.BaseSpawnConditions" />.
/// </summary>
[DocumentAsJson]
public class RuntimeSpawnConditions : BaseSpawnConditions
{
	/// <summary>
	/// The chance, usually between 0 (0% chance) and 1 (100% chance), for the entity to spawn during the spawning round. 
	/// </summary>
	[DocumentAsJson("Recommended", "1", false)]
	public double Chance = 1.0;

	/// <summary>
	/// The max number of this entity that can ever exist in the world for a single player.
	/// With more than one player, the max number is actually (this)x(current player count)x(<see cref="F:Vintagestory.API.Common.Entities.RuntimeSpawnConditions.SpawnCapPlayerScaling" />).
	/// Consider using <see cref="F:Vintagestory.API.Common.Entities.RuntimeSpawnConditions.MaxQuantityByGroup" /> to allow a max quantity based from many entities.
	/// </summary>
	[DocumentAsJson("Recommended", "20", false)]
	public int MaxQuantity = 20;

	/// <summary>
	/// The max quantity of objects to spawn based on a wildcard group of entities.<br />
	/// For example, using <see cref="F:Vintagestory.API.Common.Entities.RuntimeSpawnConditions.MaxQuantity" /> will allow a max of 20 pig-wild-male instances.
	/// Using this with a group of "pig-*" will allow a max of 20 pig entities, regardless if male, female, or piglet.
	/// </summary>
	[DocumentAsJson("Optional", "None", false)]
	public QuantityByGroup MaxQuantityByGroup;

	/// <summary>
	/// The maximum number of this entity that can exist in the world is <see cref="F:Vintagestory.API.Common.Entities.RuntimeSpawnConditions.MaxQuantity" /> x (current player count) x (this).
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	public float SpawnCapPlayerScaling = 1f;

	/// <summary>
	/// The minimum distance from the player that an object will spawn.
	/// </summary>
	[DocumentAsJson("Optional", "18", false)]
	public int MinDistanceToPlayer = 18;

	/// <summary>
	/// Set server-side after this has been loaded once, used only for error logging purposes
	/// </summary>
	public bool doneInitialLoad;

	/// <summary>
	/// Creates a deep copy of this set of spawn conditions.
	/// </summary>
	/// <returns></returns>
	public new RuntimeSpawnConditions Clone()
	{
		return new RuntimeSpawnConditions
		{
			Group = Group,
			MinLightLevel = MinLightLevel,
			MaxLightLevel = MaxLightLevel,
			LightLevelType = LightLevelType,
			HerdSize = HerdSize?.Clone(),
			Companions = (Companions?.Clone() as AssetLocation[]),
			InsideBlockCodes = (InsideBlockCodes?.Clone() as AssetLocation[]),
			RequireSolidGround = RequireSolidGround,
			TryOnlySurface = TryOnlySurface,
			MinTemp = MinTemp,
			MaxTemp = MaxTemp,
			MinRain = MinRain,
			MaxRain = MaxRain,
			MinForest = MinForest,
			MaxForest = MaxForest,
			MinShrubs = MinShrubs,
			MaxShrubs = MaxShrubs,
			ClimateValueMode = ClimateValueMode,
			MinForestOrShrubs = MinForestOrShrubs,
			Chance = Chance,
			MaxQuantity = MaxQuantity,
			MinDistanceToPlayer = MinDistanceToPlayer,
			MaxQuantityByGroup = MaxQuantityByGroup,
			SpawnCapPlayerScaling = SpawnCapPlayerScaling,
			RandomMapCodePool = (string[])RandomMapCodePool?.Clone(),
			MapCode = MapCode,
			HabitatSize = HabitatSize
		};
	}
}
