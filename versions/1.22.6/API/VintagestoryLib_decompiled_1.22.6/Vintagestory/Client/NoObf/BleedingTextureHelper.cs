using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public static class BleedingTextureHelper
{
	public static bool IsValidTextureSubId(TextureAtlasPosition[] positions, int textureSubId)
	{
		if (textureSubId >= 0 && textureSubId < positions.Length)
		{
			return positions[textureSubId] != null;
		}
		return false;
	}

	public static BleedTextureResult ComputeBleedForFace(TCTCache vars, int baseTextureSubId, int worldFace, int rotationSteps)
	{
		BleedTextureResult result = default(BleedTextureResult);
		BlendedTextureManager blendedTexManager = vars.tct.game.BlendedTextureManager;
		BlockTextureAtlasManager atlasManager = vars.tct.game.BlockAtlasManager;
		if (blendedTexManager == null || atlasManager == null || !vars.block.CanReceiveBleed)
		{
			return result;
		}
		if (baseTextureSubId < 0)
		{
			return result;
		}
		TextureAtlasPosition[] positions = atlasManager.TextureAtlasPositionsByTextureSubId;
		if (!IsValidTextureSubId(positions, baseTextureSubId))
		{
			return result;
		}
		TextureBleedHelper.BleedDirections bleedDirs = TextureBleedHelper.GetBleedDirections(vars.tct.currentChunkBlocksExt, vars.extIndex3d, worldFace, vars.block, vars.tct.game.FastBlockTextureSubidsByBlockAndFace, vars.tct.game.BleedOverlayTextureSubIdsByBlockId);
		if (bleedDirs.HasBleed)
		{
			int blendedSubId = blendedTexManager.GetOrCreateBlendedTexture(baseTextureSubId, bleedDirs.BleedTextureSubIds, bleedDirs.BleedPriorities, bleedDirs.OverlayTextureSubIds, atlasManager, rotationSteps);
			if (blendedSubId != baseTextureSubId && blendedSubId >= 0 && blendedSubId < positions.Length)
			{
				TextureAtlasPosition texPos = positions[blendedSubId];
				if (texPos != null && texPos.atlasTextureId > 0)
				{
					result.HasBleed = true;
					result.BlendedTextureSubId = blendedSubId;
					result.BlendedTexPos = texPos;
				}
			}
		}
		return result;
	}

	public static int GetFaceTextureSubId(TCTCache vars, int tileSide)
	{
		if (tileSide < 0 || tileSide >= 6)
		{
			return -1;
		}
		bool hasAlternates = vars.block.HasAlternates;
		bool hasTiles = vars.block.HasTiles;
		if (hasTiles || hasAlternates)
		{
			BakedCompositeTexture[][] textures = vars.block.FastTextureVariants;
			if (textures != null && tileSide < textures.Length)
			{
				int randomSelector = GameMath.MurmurHash3(vars.posX, vars.posY, vars.posZ);
				if (hasTiles)
				{
					BakedCompositeTexture[] tiles = textures[tileSide];
					if (tiles != null)
					{
						int positionSelector = BakedCompositeTexture.GetTiledTexturesSelector(tiles, tileSide, vars.posX, vars.posY, vars.posZ);
						return tiles[GameMath.Mod(positionSelector, tiles.Length)].TextureSubId;
					}
				}
				if (hasAlternates)
				{
					BakedCompositeTexture[] variants = textures[tileSide];
					if (variants != null)
					{
						return variants[GameMath.Mod(randomSelector, variants.Length)].TextureSubId;
					}
				}
			}
		}
		if (vars.fastBlockTextureSubidsByFace != null && tileSide < vars.fastBlockTextureSubidsByFace.Length)
		{
			int subId = vars.fastBlockTextureSubidsByFace[tileSide];
			if (subId > 0)
			{
				return subId;
			}
		}
		return -1;
	}

	public static int GetFaceTextureSubIdWithFallback(TCTCache vars, int tileSide)
	{
		int subId = GetFaceTextureSubId(vars, tileSide);
		if (subId > 0)
		{
			return subId;
		}
		IDictionary<string, CompositeTexture> blockTextures = vars.block.Textures;
		if (blockTextures != null)
		{
			string faceCode = BlockFacing.ALLFACES[tileSide].Code;
			if (blockTextures.TryGetValue(faceCode, out var faceTexture) && faceTexture?.Baked != null)
			{
				subId = faceTexture.Baked.TextureSubId;
				if (subId > 0)
				{
					return subId;
				}
			}
			if (blockTextures.TryGetValue("all", out var allTexture) && allTexture?.Baked != null)
			{
				subId = allTexture.Baked.TextureSubId;
				if (subId > 0)
				{
					return subId;
				}
			}
			foreach (KeyValuePair<string, CompositeTexture> kvp in blockTextures)
			{
				if (kvp.Value?.Baked != null)
				{
					subId = kvp.Value.Baked.TextureSubId;
					if (subId > 0)
					{
						return subId;
					}
				}
			}
		}
		return -1;
	}
}
