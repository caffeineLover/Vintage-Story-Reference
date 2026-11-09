using Newtonsoft.Json;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace Vintagestory.ServerMods.NoObf;

[DocumentAsJson]
public class ServerEntityConfig
{
	[JsonProperty(ItemConverterType = typeof(JsonAttributesConverter))]
	[DocumentAsJson("Optional", "None", false)]
	public JsonObject[] Behaviors;

	[JsonProperty]
	[JsonConverter(typeof(JsonAttributesConverter))]
	[DocumentAsJson("Optional", "None", false)]
	public JsonObject Attributes;

	[JsonProperty]
	[DocumentAsJson("Recommended", "None", false)]
	public SpawnConditions SpawnConditions;
}
