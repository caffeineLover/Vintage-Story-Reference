using System.Collections.Generic;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class SkinnablePart
{
	[DocumentAsJson("Optional", "False", false)]
	public bool Hidden;

	[DocumentAsJson("Optional", "False", false)]
	public bool Colbreak;

	[DocumentAsJson("Optional", "False", false)]
	public bool UseDropDown;

	[DocumentAsJson("Required", "", false)]
	public string Code;

	[DocumentAsJson("Optional", "EnumSkinnableType.Shape", false)]
	public EnumSkinnableType Type;

	[DocumentAsJson("Optional", "None", false)]
	public string[] DisableElements;

	[DocumentAsJson("Optional", "None", false)]
	public CompositeShape ShapeTemplate;

	public SkinnablePartVariant[] Variants;

	[DocumentAsJson("Optional", "None", false)]
	public Vec2i TextureRenderTo;

	[DocumentAsJson("Optional", "None", false)]
	public string TextureTarget;

	[DocumentAsJson("Optional", "None", false)]
	public AssetLocation TextureTemplate;

	public Dictionary<string, SkinnablePartVariant> VariantsByCode;
}
