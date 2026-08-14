using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common.Entities;

public class TrackedPlayerProperties
{
	public int EyesInWaterColorShift;

	public int EyesInLavaColorShift;

	public float EyesInLavaDepth;

	public float EyesInWaterDepth;

	public float DayLight = 1f;

	public float DistanceToSpawnPoint;

	public float MoonLight;

	public double FallSpeed;

	public readonly BlockPos PlayerChunkPos = new BlockPos(0);

	public readonly BlockPos PlayerPosDiv8 = new BlockPos(0);

	/// <summary>
	/// Relative value. bottom 0...1 sealevel, 1 .... 2 max-y
	/// </summary>
	public float posY;

	/// <summary>
	/// 0...32
	/// </summary>
	public float sunSlight = 21f;

	/// <summary>
	/// The servers playstyle
	/// </summary>
	public string Playstyle;

	public string PlayListCode;
}
