using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class DisplayableAttributes
{
	public EnumDisplayableBehavior Behavior;

	public Size3f Size;

	public CompositeShape Shape;

	public ModelTransform Transform;

	public int RandYRotAngle = 30;

	public string Category;

	public string[] PileableSelectiveElements;
}
