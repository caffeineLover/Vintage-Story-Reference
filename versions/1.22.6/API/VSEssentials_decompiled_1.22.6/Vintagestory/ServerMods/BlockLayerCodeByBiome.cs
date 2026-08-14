using System.Collections.Generic;
using Newtonsoft.Json;
using Vintagestory.API.Common;

namespace Vintagestory.ServerMods;

[JsonObject(MemberSerialization.OptIn)]
public class BlockLayerCodeByBiome
{
	[JsonProperty]
	public AssetLocation BlockCode;

	[JsonProperty]
	public float MinY;

	[JsonProperty]
	public float MaxY = 1f;

	[JsonProperty]
	public int Biome = -1;

	public int BlockId;

	public Dictionary<int, int> BlockIdMapping;
}
