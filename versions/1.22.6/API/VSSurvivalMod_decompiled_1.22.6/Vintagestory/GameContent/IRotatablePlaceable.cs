using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public interface IRotatablePlaceable
{
	float MeshAngleRad { get; set; }

	bool DoPartialSelection();

	Cuboidf[] GetCollisionBoxes();

	Cuboidf[] GetSelectionBoxes();
}
