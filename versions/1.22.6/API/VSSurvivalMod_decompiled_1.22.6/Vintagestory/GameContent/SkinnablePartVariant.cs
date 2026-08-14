using Vintagestory.API;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class SkinnablePartVariant
{
	[DocumentAsJson("Optional", "standard", false)]
	public string Category = "standard";

	[DocumentAsJson("Required", "", false)]
	public string Code;

	[DocumentAsJson("Optional", "None", false)]
	public CompositeShape Shape;

	[DocumentAsJson("Optional", "None", false)]
	public AssetLocation Texture;

	[DocumentAsJson("Optional", "None", false)]
	public AssetLocation Sound;

	public int Color;

	public AppliedSkinnablePartVariant AppliedCopy(string partCode)
	{
		return new AppliedSkinnablePartVariant
		{
			Code = Code,
			Shape = Shape,
			Texture = Texture,
			Color = Color,
			PartCode = partCode
		};
	}
}
