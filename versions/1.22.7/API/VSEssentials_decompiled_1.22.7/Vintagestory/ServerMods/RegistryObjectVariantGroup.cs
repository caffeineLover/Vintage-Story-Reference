using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.ServerMods;

[DocumentAsJson]
public class RegistryObjectVariantGroup
{
	[DocumentAsJson("Optional", "None", false)]
	public AssetLocation LoadFromProperties;

	[DocumentAsJson("Optional", "None", false)]
	public AssetLocation[] LoadFromPropertiesCombine;

	[DocumentAsJson("Required", "", false)]
	public string Code;

	[DocumentAsJson("Required", "None", false)]
	public string[] States;

	[DocumentAsJson("Optional", "Multiply", false)]
	public EnumCombination Combine = EnumCombination.Multiply;

	[DocumentAsJson("Optional", "None", false)]
	public string OnVariant;

	public string IsValue;
}
