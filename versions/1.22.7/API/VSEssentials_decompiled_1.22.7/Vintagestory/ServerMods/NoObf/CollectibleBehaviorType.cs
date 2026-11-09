using Newtonsoft.Json;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.ServerMods.NoObf;

[DocumentAsJson]
public class CollectibleBehaviorType
{
	[JsonProperty]
	[DocumentAsJson("Required", "", false)]
	public string name;

	[JsonProperty]
	[JsonConverter(typeof(JsonAttributesConverter))]
	[DocumentAsJson("Optional", "None", false)]
	public JsonObject properties;
}
