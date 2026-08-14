using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class TypedTextureSource : ITexPositionSource
{
	protected ICoreClientAPI capi;

	protected ITextureAtlasAPI atlas;

	protected string domain;

	public Dictionary<string, string> materials;

	public Dictionary<string, string> textureMapping;

	public TextureAtlasPosition this[string textureCode]
	{
		get
		{
			AssetLocation loc = getMappedTexture(textureCode, domain, materials, textureMapping);
			atlas.GetOrInsertTexture(loc, out var _, out var texPos);
			return texPos;
		}
	}

	public Size2i AtlasSize => atlas.Size;

	public TypedTextureSource(ICoreClientAPI capi, ITextureAtlasAPI atlas, string defaultDomain, Dictionary<string, string> textureMapping, Dictionary<string, string> materials)
	{
		this.capi = capi;
		this.atlas = atlas;
		domain = defaultDomain;
		this.textureMapping = textureMapping;
		this.materials = materials;
	}

	public int GetTextureSubId(string textureCode)
	{
		AssetLocation loc = getMappedTexture(textureCode, domain, materials, textureMapping);
		atlas.GetOrInsertTexture(loc, out var subid, out var _);
		return subid;
	}

	public static AssetLocation getMappedTexture(string textureCode, string domain, Dictionary<string, string> materials, Dictionary<string, string> textureMapping)
	{
		string texturePath = textureMapping[textureCode];
		foreach (KeyValuePair<string, string> mat in materials)
		{
			texturePath = texturePath.Replace("{" + mat.Key + "}", mat.Value);
		}
		return AssetLocation.Create(texturePath, domain);
	}
}
