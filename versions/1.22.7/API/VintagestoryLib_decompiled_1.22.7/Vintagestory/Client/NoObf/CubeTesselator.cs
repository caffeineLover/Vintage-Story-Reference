using Vintagestory.API.Client;
using Vintagestory.API.Client.Tesselation;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public class CubeTesselator : IBlockTesselator
{
	private float blockHeight = 1f;

	public CubeTesselator(float blockHeight)
	{
		this.blockHeight = blockHeight;
	}

	public void Tesselate(TCTCache vars)
	{
		float blockHeight = this.blockHeight;
		int verts = 0;
		TextureAtlasPosition[] textureAtlasPositionsByTextureSubId = vars.textureAtlasPositionsByTextureSubId;
		int[] fastBlockTextureSubidsByFace = vars.fastBlockTextureSubidsByFace;
		bool hasAlternates = vars.block.HasAlternates;
		bool hasTiles = vars.block.HasTiles;
		int colorMapDataValue = vars.ColorMapData.Value;
		int drawFaceFlags = vars.drawFaceFlags;
		int extIndex3d = vars.extIndex3d;
		int allFlags = vars.VertexFlags;
		FastVec3f[][] blockFaceVertices = vars.blockFaceVertices;
		BakedCompositeTexture[][] textures = null;
		int randomSelector = 0;
		if (hasAlternates || hasTiles)
		{
			textures = vars.block.FastTextureVariants;
			randomSelector = GameMath.MurmurHash3(vars.posX, vars.posY, vars.posZ);
		}
		MeshData[] meshPools = vars.tct.GetPoolForPass(vars.RenderPass, 1);
		int[] moveIndices = TileSideEnum.MoveIndex;
		for (int tileSide = 0; tileSide < moveIndices.Length; tileSide++)
		{
			if ((drawFaceFlags & (1 << tileSide)) == 0)
			{
				continue;
			}
			vars.CalcBlockFaceLight(tileSide, extIndex3d + moveIndices[tileSide]);
			int textureSubId;
			if (hasTiles)
			{
				BakedCompositeTexture[] tiles = textures[tileSide];
				if (tiles != null)
				{
					int positionSelector = BakedCompositeTexture.GetTiledTexturesSelector(tiles, tileSide, vars.posX, vars.posY, vars.posZ);
					textureSubId = tiles[GameMath.Mod(positionSelector, tiles.Length)].TextureSubId;
					goto IL_0142;
				}
			}
			if (hasAlternates)
			{
				BakedCompositeTexture[] variants = textures[tileSide];
				if (variants != null)
				{
					textureSubId = variants[GameMath.Mod(randomSelector, variants.Length)].TextureSubId;
					goto IL_0142;
				}
			}
			textureSubId = fastBlockTextureSubidsByFace[tileSide];
			goto IL_0142;
			IL_0142:
			DrawBlockFace(vars, tileSide, blockFaceVertices[tileSide], textureAtlasPositionsByTextureSubId[textureSubId], allFlags | BlockFacing.ALLFACES[tileSide].NormalPackedFlags, colorMapDataValue, meshPools, blockHeight);
			verts += 4;
		}
	}

	public static void DrawBlockFace(TCTCache vars, int tileSide, FastVec3f[] quadOffsets, TextureAtlasPosition texPos, int flags, int colorMapDataValue, MeshData[] meshPools, float blockHeight = 1f)
	{
		float texHeight = ((tileSide <= 3) ? blockHeight : 1f);
		MeshData obj = meshPools[texPos.atlasNumber];
		int lastelement = obj.VerticesCount;
		int[] currentLightRGBByCorner = vars.CurrentLightRGBByCorner;
		float y2 = texPos.y2;
		float y3 = y2 + (texPos.y1 - y2) * texHeight;
		float x = vars.finalX;
		float y4 = vars.finalY;
		float z = vars.finalZ;
		FastVec3f tmpv = quadOffsets[7];
		obj.AddVertexWithFlags(x + tmpv.X, y4 + tmpv.Y * blockHeight, z + tmpv.Z, texPos.x2, y2, currentLightRGBByCorner[3], flags);
		tmpv = quadOffsets[5];
		obj.AddVertexWithFlags(x + tmpv.X, y4 + tmpv.Y * blockHeight, z + tmpv.Z, texPos.x2, y3, currentLightRGBByCorner[1], flags);
		tmpv = quadOffsets[4];
		obj.AddVertexWithFlags(x + tmpv.X, y4 + tmpv.Y * blockHeight, z + tmpv.Z, texPos.x1, y3, currentLightRGBByCorner[0], flags);
		tmpv = quadOffsets[6];
		obj.AddVertexWithFlags(x + tmpv.X, y4 + tmpv.Y * blockHeight, z + tmpv.Z, texPos.x1, y2, currentLightRGBByCorner[2], flags);
		obj.CustomInts.Add4(colorMapDataValue);
		obj.AddQuadIndices(lastelement);
		vars.UpdateChunkMinMax(x, y4, z);
		vars.UpdateChunkMinMax(x + 1f, y4 + blockHeight, z + 1f);
	}
}
