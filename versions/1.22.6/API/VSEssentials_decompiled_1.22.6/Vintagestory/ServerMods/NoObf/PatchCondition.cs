using Vintagestory.API;

namespace Vintagestory.ServerMods.NoObf;

[DocumentAsJson]
public class PatchCondition
{
	[DocumentAsJson("Required", "", false)]
	public string When;

	[DocumentAsJson("Recommended", "None", false)]
	public string IsValue;

	[DocumentAsJson("Optional", "False", false)]
	public bool useValue;
}
