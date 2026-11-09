using Newtonsoft.Json;
using Vintagestory.API.Common;

namespace Vintagestory.ServerMods.NoObf;

[JsonObject(MemberSerialization.OptIn)]
public class LandformsWorldProperty : WorldProperty<LandformVariant>
{
	[JsonIgnore]
	public LandformVariant[] LandFormsByIndex;

	public int GetIndexByCode(string code)
	{
		if (LandFormsByIndex == null)
		{
			return -1;
		}
		for (int i = 0; i < LandFormsByIndex.Length; i++)
		{
			if (LandFormsByIndex[i]?.Code == (AssetLocation)code)
			{
				return i;
			}
		}
		return -1;
	}
}
