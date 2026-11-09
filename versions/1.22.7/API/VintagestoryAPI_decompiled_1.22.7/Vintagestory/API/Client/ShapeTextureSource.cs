using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Client;

public class ShapeTextureSource : ITexPositionSource
{
	private ICoreClientAPI capi;

	private Shape shape;

	private string filenameForLogging;

	public Dictionary<string, CompositeTexture> textures = new Dictionary<string, CompositeTexture>();

	public TextureAtlasPosition? firstTexPos;

	private HashSet<AssetLocation> missingTextures = new HashSet<AssetLocation>();

	public TextureAtlasPosition this[string textureCode]
	{
		get
		{
			int textureSubId;
			TextureAtlasPosition texPos;
			if (textures.TryGetValue(textureCode, out CompositeTexture ctex))
			{
				capi.BlockTextureAtlas.GetOrInsertTexture(ctex, out textureSubId, out texPos);
			}
			else
			{
				shape.Textures.TryGetValue(textureCode, out var texturePath);
				if (texturePath == null)
				{
					if (!missingTextures.Contains(textureCode))
					{
						capi.Logger.Warning("Shape {0} has an element using texture code {1}, but no such texture exists", filenameForLogging, textureCode);
						missingTextures.Add(textureCode);
					}
					return capi.BlockTextureAtlas.UnknownTexturePosition;
				}
				capi.BlockTextureAtlas.GetOrInsertTexture(texturePath, out textureSubId, out texPos);
			}
			if (texPos == null)
			{
				return capi.BlockTextureAtlas.UnknownTexturePosition;
			}
			if (firstTexPos == null)
			{
				firstTexPos = texPos;
			}
			return texPos;
		}
	}

	public Size2i AtlasSize => capi.BlockTextureAtlas.Size;

	public ShapeTextureSource(ICoreClientAPI capi, Shape shape, string filenameForLogging)
	{
		this.capi = capi;
		this.shape = shape;
		this.filenameForLogging = filenameForLogging;
	}

	public ShapeTextureSource(ICoreClientAPI capi, Shape shape, string filenameForLogging, IDictionary<string, CompositeTexture> texturesSource, TexturePathUpdater pathUpdater)
		: this(capi, shape, filenameForLogging)
	{
		foreach (KeyValuePair<string, CompositeTexture> val in texturesSource)
		{
			CompositeTexture ctex = val.Value.Clone();
			ctex.Base.Path = pathUpdater(ctex.Base.Path);
			ctex.Bake(capi.Assets);
			textures[val.Key] = ctex;
		}
	}
}
