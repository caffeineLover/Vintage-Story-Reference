using Newtonsoft.Json;
using Vintagestory.API.Common;

namespace Vintagestory.ServerMods;

[JsonObject(MemberSerialization.OptIn)]
public class TallGrassBlockCodeByBiome
{
	[JsonProperty]
	public AssetLocation BlockCode;

	[JsonProperty]
	public int Biome = -1;

	public int BlockId;
}
