using Vintagestory.API;

namespace Vintagestory.ServerMods.NoObf;

[DocumentAsJson]
public class PatchModDependence
{
	[DocumentAsJson("Required", "", false)]
	public string modid;

	[DocumentAsJson("Optional", "False", false)]
	public bool invert;
}
