using Vintagestory.API.Client;
using Vintagestory.API.Client.Tesselation;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public class BleedingCubeTesselator : IBlockTesselator
{
	private readonly float blockHeight;

	public BleedingCubeTesselator(float blockHeight)
	{
		this.blockHeight = blockHeight;
	}

	public void Tesselate(TCTCache vars)
	{
		int verts = 0;
		int colorMapDataValue = vars.ColorMapData.Value;
		int drawFaceFlags = vars.drawFaceFlags;
		int extIndex3d = vars.extIndex3d;
		int allFlags = vars.VertexFlags;
		FastVec3f[][] blockFaceVertices = vars.blockFaceVertices;
		MeshData[] meshPools = vars.tct.GetPoolForPass(vars.RenderPass, 1);
		BlockTextureAtlasManager atlasManager = vars.tct.game.BlockAtlasManager;
		int[] moveIndices = TileSideEnum.MoveIndex;
		for (int tileSide = 0; tileSide < moveIndices.Length; tileSide++)
		{
			if ((drawFaceFlags & (1 << tileSide)) != 0)
			{
				vars.CalcBlockFaceLight(tileSide, extIndex3d + moveIndices[tileSide]);
				int textureSubId = BleedingTextureHelper.GetFaceTextureSubId(vars, tileSide);
				if (textureSubId <= 0)
				{
					textureSubId = vars.fastBlockTextureSubidsByFace[tileSide];
				}
				BleedTextureResult bleedResult = BleedingTextureHelper.ComputeBleedForFace(vars, textureSubId, tileSide, -1);
				int finalTextureSubId = (bleedResult.HasBleed ? bleedResult.BlendedTextureSubId : textureSubId);
				TextureAtlasPosition[] textureAtlasPositionsByTextureSubId = atlasManager.TextureAtlasPositionsByTextureSubId;
				if (!BleedingTextureHelper.IsValidTextureSubId(textureAtlasPositionsByTextureSubId, finalTextureSubId))
				{
					finalTextureSubId = textureSubId;
				}
				TextureAtlasPosition texPosUsed = textureAtlasPositionsByTextureSubId[finalTextureSubId];
				CubeTesselator.DrawBlockFace(vars, tileSide, blockFaceVertices[tileSide], texPosUsed, allFlags | BlockFacing.ALLFACES[tileSide].NormalPackedFlags, colorMapDataValue, meshPools, blockHeight);
				verts += 4;
			}
		}
	}
}
