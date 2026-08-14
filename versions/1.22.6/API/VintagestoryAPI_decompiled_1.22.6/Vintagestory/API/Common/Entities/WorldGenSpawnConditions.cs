using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common.Entities;

/// <summary>
/// A set of spawn conditions for when chunks are generated. Most properties are got from <see cref="T:Vintagestory.API.Common.Entities.BaseSpawnConditions" />.
/// </summary>
[DocumentAsJson]
public class WorldGenSpawnConditions : BaseSpawnConditions
{
	/// <summary>
	/// The amount of times the object will attempt to spawn per chunk.
	/// </summary>
	[DocumentAsJson("Recommended", "0", false)]
	public NatFloat TriesPerChunk = NatFloat.Zero;

	public new WorldGenSpawnConditions Clone()
	{
		return new WorldGenSpawnConditions
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
			ClimateValueMode = ClimateValueMode,
			MinForest = MinForest,
			MaxForest = MaxForest,
			MinShrubs = MinShrubs,
			MaxShrubs = MaxShrubs,
			MinForestOrShrubs = MinForestOrShrubs,
			RandomMapCodePool = (string[])RandomMapCodePool?.Clone(),
			MapCode = MapCode,
			TriesPerChunk = TriesPerChunk?.Clone(),
			HabitatSize = HabitatSize
		};
	}
}
