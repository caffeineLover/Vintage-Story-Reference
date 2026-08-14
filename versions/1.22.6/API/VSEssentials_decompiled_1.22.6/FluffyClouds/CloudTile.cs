using Vintagestory.API.MathTools;

namespace FluffyClouds;

public class CloudTile
{
	public short GridXOffset;

	public short GridZOffset;

	public short TargetThickness;

	public short TargetBrightnes;

	public short TargetThinCloudMode;

	public short TargetUndulatingCloudMode;

	public short TargetCloudOpaquenes;

	public short SelfThickness;

	public short Brightness;

	public short ThinCloudMode;

	public short UndulatingCloudMode;

	public short CloudOpaqueness;

	public LCGRandom brightnessRand;

	internal bool rainValuesSet;

	internal float lerpRainCloudOverlay;

	internal float lerpRainOverlay;
}
