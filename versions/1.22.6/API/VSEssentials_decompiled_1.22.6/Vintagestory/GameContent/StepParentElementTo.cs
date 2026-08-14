using Newtonsoft.Json;
using Vintagestory.API;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class StepParentElementTo : ModelTransform
{
	[DocumentAsJson("Required", "", false)]
	[JsonProperty]
	public string ElementName;
}
