using Newtonsoft.Json;

namespace Vintagestory.ServerMods;

public class LandformConstraint
{
	[JsonProperty]
	public string Code;

	[JsonProperty]
	public float Value;

	[JsonProperty]
	public string Type;
}
