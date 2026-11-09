namespace Vintagestory.API.Common;

public struct SolarSphericalCoords(float zenithAngle, float azimuthAngle)
{
	public float ZenithAngle = zenithAngle;

	public float AzimuthAngle = azimuthAngle;
}
