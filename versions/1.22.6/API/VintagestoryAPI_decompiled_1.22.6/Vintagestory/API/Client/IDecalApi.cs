using System.Collections.Generic;

namespace Vintagestory.API.Client;

public interface IDecalApi
{
	Dictionary<string, int> TextureNameToIdMapping { get; }

	TextureAtlasPosition[] DecalTextureAtlasPositionsByTextureSubId { get; }
}
