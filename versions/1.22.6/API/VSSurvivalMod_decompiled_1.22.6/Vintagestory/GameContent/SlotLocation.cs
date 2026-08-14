using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class SlotLocation : Vec3f
{
	public int PlacementSurfaceIndex;

	public bool IsPlacedItem;

	public string EncodedLocation
	{
		get
		{
			if (!(Y > 0f))
			{
				return PlacementSurfaceIndex + "-" + X + "-" + Z;
			}
			return PlacementSurfaceIndex + "-" + X + "-" + Z + "-" + Y;
		}
	}

	public SlotLocation(float x, float y, float z)
		: base(x, y, z)
	{
	}

	public SlotLocation UpCopy()
	{
		return new SlotLocation(X, Y + 1f, Z)
		{
			PlacementSurfaceIndex = PlacementSurfaceIndex,
			IsPlacedItem = IsPlacedItem
		};
	}
}
