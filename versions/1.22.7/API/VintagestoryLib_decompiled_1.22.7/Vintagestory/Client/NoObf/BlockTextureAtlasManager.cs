using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public class BlockTextureAtlasManager(ClientMain game) : TextureAtlasManager(game), IBlockTextureAtlasAPI, ITextureAtlasAPI
{
	private List<KeyValuePair<string, CompositeTexture>> replacements = new List<KeyValuePair<string, CompositeTexture>>();

	internal void CollectTextures(IList<Block> blocks, Vintagestory.API.Datastructures.OrderedDictionary<AssetLocation, UnloadableShape> shapes)
	{
		Block snowLayerBlock = null;
		int snowTextureSubId = 0;
		AssetLocation snowlayerloc = new AssetLocation("snowlayer-1");
		Dictionary<AssetLocation, CompositeTexture> shapeTexturesCache = new Dictionary<AssetLocation, CompositeTexture>();
		foreach (Block block in blocks)
		{
			if (!(block.Code == null))
			{
				block.EnsureValidTextures(game.Logger);
				ResolveTextureCodes(block, shapes, shapeTexturesCache);
			}
		}
		foreach (Block block2 in blocks)
		{
			if (game.disposed)
			{
				return;
			}
			if (!(block2.Code == null) && block2.DrawType == EnumDrawType.TopSoil)
			{
				collectTexturesForBlock(block2, shapes);
			}
		}
		foreach (Block block3 in blocks)
		{
			if (game.disposed)
			{
				return;
			}
			if (!(block3.Code == null) && block3.DrawType != EnumDrawType.TopSoil)
			{
				collectTexturesForBlock(block3, shapes);
				if (snowLayerBlock == null && block3.Code.Equals(snowlayerloc))
				{
					snowLayerBlock = block3;
					compose(snowLayerBlock, 0);
					snowTextureSubId = snowLayerBlock?.Textures[BlockFacing.UP.Code].Baked.TextureSubId ?? 0;
				}
			}
		}
		foreach (Block block4 in blocks)
		{
			if (block4 != null && !(block4.Code == null))
			{
				compose(block4, snowTextureSubId);
			}
		}
	}

	private void collectTexturesForBlock(Block block, Vintagestory.API.Datastructures.OrderedDictionary<AssetLocation, UnloadableShape> shapes)
	{
		block.OnCollectTextures(game.api, this);
		if (block.Shape != null)
		{
			collectAndBakeTexturesFromShape(block, block.Shape, inv: false, shapes);
		}
		if (block.ShapeInventory != null)
		{
			collectAndBakeTexturesFromShape(block, block.ShapeInventory, inv: true, shapes);
		}
	}

	private void compose(Block block, int snowtextureSubid)
	{
		int blockId = block.BlockId;
		foreach (KeyValuePair<string, CompositeTexture> val in block.TexturesInventory)
		{
			val.Value.Baked.TextureSubId = textureNamesDict[val.Value.Baked.BakedName];
		}
		foreach (KeyValuePair<string, CompositeTexture> texture in block.Textures)
		{
			BakedCompositeTexture bct = texture.Value.Baked;
			bct.TextureSubId = textureNamesDict[bct.BakedName];
			if (bct.BakedVariants != null)
			{
				for (int i = 0; i < bct.BakedVariants.Length; i++)
				{
					bct.BakedVariants[i].TextureSubId = textureNamesDict[bct.BakedVariants[i].BakedName];
				}
			}
			if (bct.BakedTiles != null)
			{
				for (int j = 0; j < bct.BakedTiles.Length; j++)
				{
					bct.BakedTiles[j].TextureSubId = textureNamesDict[bct.BakedTiles[j].BakedName];
				}
			}
		}
		if (block.DrawType != EnumDrawType.JSON)
		{
			BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
			foreach (BlockFacing facing in aLLFACES)
			{
				if (block.Textures.TryGetValue(facing.Code, out var faceTexture))
				{
					int textureSubid = faceTexture.Baked.TextureSubId;
					game.FastBlockTextureSubidsByBlockAndFace[blockId][facing.Index] = textureSubid;
				}
			}
			if (block.Textures.TryGetValue("specialSecondTexture", out var secondTexture))
			{
				game.FastBlockTextureSubidsByBlockAndFace[blockId][6] = secondTexture.Baked.TextureSubId;
			}
			else
			{
				game.FastBlockTextureSubidsByBlockAndFace[blockId][6] = game.FastBlockTextureSubidsByBlockAndFace[blockId][BlockFacing.UP.Index];
			}
		}
		if (block.DrawType == EnumDrawType.JSONAndSnowLayer || block.DrawType == EnumDrawType.CrossAndSnowlayer || block.DrawType == EnumDrawType.CrossAndSnowlayer_2 || block.DrawType == EnumDrawType.CrossAndSnowlayer_3 || block.DrawType == EnumDrawType.CrossAndSnowlayer_4)
		{
			game.FastBlockTextureSubidsByBlockAndFace[blockId][6] = snowtextureSubid;
		}
		if (block.BleedOverlayTextureBaked?.Baked != null && textureNamesDict.TryGetValue(block.BleedOverlayTextureBaked.Baked.BakedName, out var overlaySubId))
		{
			game.BleedOverlayTextureSubIdsByBlockId[blockId] = overlaySubId;
		}
	}

	private void collectAndBakeTexturesFromShape(Block block, CompositeShape shape, bool inv, Vintagestory.API.Datastructures.OrderedDictionary<AssetLocation, UnloadableShape> shapes)
	{
		if (shapes.TryGetValue(shape.Base, out var compositeShape))
		{
			IDictionary<string, CompositeTexture> targetDict = (inv ? block.TexturesInventory : block.Textures);
			CollectAndBakeTexturesFromShape(compositeShape, targetDict, shape.Base);
		}
		if (shape.BakedAlternates != null)
		{
			CompositeShape[] bakedAlternates = shape.BakedAlternates;
			foreach (CompositeShape val in bakedAlternates)
			{
				collectAndBakeTexturesFromShape(block, val, inv, shapes);
			}
		}
	}

	public override void CollectAndBakeTexturesFromShape(Shape compositeShape, IDictionary<string, CompositeTexture> targetDict, AssetLocation baseLoc)
	{
		Dictionary<string, AssetLocation> shapeTextures = compositeShape.Textures;
		if (shapeTextures == null)
		{
			return;
		}
		foreach (KeyValuePair<string, AssetLocation> val in shapeTextures)
		{
			if (!targetDict.ContainsKey(val.Key))
			{
				CompositeTexture ct = new CompositeTexture(val.Value);
				ct.Bake(game.AssetManager);
				AssetLocationAndSource locS = new AssetLocationAndSource(ct.Baked.BakedName, "Shape file ", baseLoc);
				if (val.Key == "specialSecondTexture")
				{
					locS.AddToAllAtlasses = true;
				}
				ct.Baked.TextureSubId = GetOrAddTextureLocation(locS);
				targetDict[val.Key] = ct;
			}
		}
	}

	public void ResolveTextureCodes(Block block, Vintagestory.API.Datastructures.OrderedDictionary<AssetLocation, UnloadableShape> blockShapes, Dictionary<AssetLocation, CompositeTexture> basicTexturesCache)
	{
		blockShapes.TryGetValue(block.Shape.Base, out var baseShape);
		UnloadableShape inventoryShape = ((block.ShapeInventory == null) ? null : blockShapes[block.ShapeInventory.Base]);
		if (baseShape != null && !baseShape.Loaded)
		{
			baseShape.Load(game, new AssetLocationAndSource(block.Shape.Base));
		}
		if (inventoryShape != null && !inventoryShape.Loaded)
		{
			inventoryShape.Load(game, new AssetLocationAndSource(block.Shape.Base));
		}
		bool blockTexturesContainsAll = block.Textures.ContainsKey("all");
		bool invTexturesContainsAll = block.TexturesInventory.ContainsKey("all");
		if (blockTexturesContainsAll || invTexturesContainsAll)
		{
			LoadAllTextureCodes(block, baseShape);
		}
		if (block.Textures.Count > 0)
		{
			ResolveTextureDict((TextureDictionary)block.Textures);
		}
		if (block.TexturesInventory.Count > 0)
		{
			ResolveTextureDict((TextureDictionary)block.TexturesInventory);
		}
		if (baseShape?.Textures != null)
		{
			if (baseShape.TexturesResolved == null)
			{
				baseShape.ResolveTextures(basicTexturesCache);
			}
			foreach (KeyValuePair<string, CompositeTexture> val in baseShape.TexturesResolved)
			{
				string textureCode = val.Key;
				CompositeTexture shapefileTexture = val.Value;
				if (block.Textures.TryGetValue(textureCode, out var tex))
				{
					if (tex.Base.Path == "inherit")
					{
						tex.Base = shapefileTexture.Base;
					}
					if (tex.BlendedOverlays == null)
					{
						continue;
					}
					BlendedOverlayTexture[] overlays = tex.BlendedOverlays;
					for (int i = 0; i < overlays.Length; i++)
					{
						if (overlays[i].Base.Path == "inherit")
						{
							overlays[i].Base = shapefileTexture.Base;
						}
					}
				}
				else if (!blockTexturesContainsAll || !(textureCode == "all"))
				{
					block.Textures.Add(textureCode, shapefileTexture);
				}
			}
		}
		replacements.Clear();
		foreach (KeyValuePair<string, CompositeTexture> val2 in block.Textures)
		{
			CompositeTexture tex2 = val2.Value;
			if (tex2.IsBasic())
			{
				if (basicTexturesCache.TryGetValue(tex2.Base, out var cachedTex))
				{
					if (tex2 != cachedTex)
					{
						replacements.Add(new KeyValuePair<string, CompositeTexture>(val2.Key, cachedTex));
						tex2 = cachedTex;
					}
				}
				else
				{
					basicTexturesCache.Add(tex2.Base, tex2);
				}
			}
			((TextureDictionary)block.TexturesInventory).AddIfNotPresent(val2.Key, tex2);
		}
		foreach (KeyValuePair<string, CompositeTexture> val3 in replacements)
		{
			block.Textures[val3.Key] = val3.Value;
		}
		if (inventoryShape == null || inventoryShape.Textures == null)
		{
			return;
		}
		if (inventoryShape.TexturesResolved == null)
		{
			inventoryShape.ResolveTextures(basicTexturesCache);
		}
		foreach (KeyValuePair<string, CompositeTexture> val4 in inventoryShape.TexturesResolved)
		{
			string textureCode2 = val4.Key;
			if (!invTexturesContainsAll || !(textureCode2 == "all"))
			{
				((TextureDictionary)block.TexturesInventory).AddIfNotPresent(textureCode2, val4.Value);
			}
		}
	}

	public void LoadAllTextureCodes(Block block, Shape blockShape)
	{
		LoadShapeTextureCodes(blockShape);
		if (block.DrawType == EnumDrawType.Cube)
		{
			textureCodes.Add("west");
			textureCodes.Add("east");
			textureCodes.Add("north");
			textureCodes.Add("south");
			textureCodes.Add("up");
			textureCodes.Add("down");
		}
	}

	public TextureAtlasPosition GetPosition(Block block, string textureName, bool returnNullWhenMissing = false)
	{
		return new TextureSource(game, base.Size, block)
		{
			returnNullWhenMissing = returnNullWhenMissing
		}[textureName];
	}

	public override TextureAtlas RuntimeCreateNewAtlas(string itemclass)
	{
		TextureAtlas atlas = base.RuntimeCreateNewAtlas(itemclass);
		int[] textureIds = game.TerrainChunkTesselator.RuntimeCreateNewBlockTextureAtlas(atlas.textureId);
		game.chunkRenderer.RuntimeAddBlockTextureAtlas(textureIds);
		return atlas;
	}
}
