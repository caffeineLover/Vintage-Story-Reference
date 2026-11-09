using Vintagestory.API.Client;
using Vintagestory.API.Client.Tesselation;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public class SurfaceLayerTesselator : IBlockTesselator
{
	private const int caveArtPerBlock = 16;

	public const float decorFaceOffset = 0.001953125f;

	private float[] uv = new float[8];

	public void Tesselate(TCTCache vars)
	{
		int verts = 0;
		TextureAtlasPosition[] textureAtlasPositionsByTextureSubId = vars.textureAtlasPositionsByTextureSubId;
		int[] fastBlockTextureSubidsByFace = vars.fastBlockTextureSubidsByFace;
		bool hasAlternates = vars.block.HasAlternates;
		int colorMapDataValue = vars.ColorMapData.Value;
		int drawFaceFlags = vars.drawFaceFlags ^ 0x3F;
		int extIndex3d = vars.extIndex3d;
		int allFlags = vars.VertexFlags;
		FastVec3f[][] blockFaceVertices = vars.blockFaceVertices;
		BakedCompositeTexture[][] textures = null;
		int randomSelector = 0;
		if (hasAlternates)
		{
			textures = vars.block.FastTextureVariants;
			randomSelector = GameMath.MurmurHash3(vars.posX, vars.posY, vars.posZ);
		}
		int rotIndex = 0;
		if (vars.block.RandomizeRotations)
		{
			rotIndex = GameMath.MurmurHash3Mod(vars.posX, vars.posY, vars.posZ, 4);
		}
		Block targetBlock = vars.tct.currentChunkBlocksExt[vars.extIndex3d];
		int mainSide;
		int textureSubId;
		if ((targetBlock.DrawType == EnumDrawType.JSON || targetBlock.DrawType == EnumDrawType.JSONAndSnowLayer) && !targetBlock.CustomBlockLayerHandler)
		{
			for (mainSide = 0; mainSide < 6 && (drawFaceFlags & (1 << mainSide)) == 0; mainSide++)
			{
			}
			if (mainSide < 6 && !targetBlock.SideIsSolid(vars.tct.game.BlockAccessor, new BlockPos(vars.posX, vars.posY, vars.posZ), TileSideEnum.GetOpposite(mainSide)))
			{
				if (hasAlternates)
				{
					BakedCompositeTexture[] variants = textures[mainSide];
					if (variants != null)
					{
						textureSubId = variants[GameMath.Mod(randomSelector, variants.Length)].TextureSubId;
						goto IL_0161;
					}
				}
				textureSubId = fastBlockTextureSubidsByFace[mainSide];
				goto IL_0161;
			}
		}
		EnumChunkRenderPass renderPass = vars.RenderPass;
		if (targetBlock.RenderPass == EnumChunkRenderPass.Transparent && renderPass == EnumChunkRenderPass.Decor)
		{
			renderPass = EnumChunkRenderPass.OpaqueNoCull;
		}
		MeshData[] meshPools = vars.tct.GetPoolForPass(renderPass, (!vars.block.DoNotRenderAtLod2) ? 1 : 2);
		MeshData[] meshPoolsDark = vars.tct.GetPoolForPass(renderPass, 0);
		for (int tileSide = 0; tileSide < 6; tileSide++)
		{
			if ((drawFaceFlags & (1 << tileSide)) == 0)
			{
				continue;
			}
			int lightfaceSide = BlockFacing.ALLFACES[tileSide].Opposite.Index;
			long lighting = vars.CalcBlockFaceLight(lightfaceSide, extIndex3d + TileSideEnum.MoveIndex[lightfaceSide]);
			if (hasAlternates)
			{
				BakedCompositeTexture[] variants2 = textures[tileSide];
				if (variants2 != null)
				{
					textureSubId = variants2[GameMath.Mod(randomSelector, variants2.Length)].TextureSubId;
					goto IL_022f;
				}
			}
			textureSubId = fastBlockTextureSubidsByFace[tileSide];
			goto IL_022f;
			IL_022f:
			DrawBlockFace(vars, tileSide, blockFaceVertices[tileSide], textureAtlasPositionsByTextureSubId[textureSubId], allFlags | BlockFacing.ALLFACES[TileSideEnum.GetOpposite(tileSide)].NormalPackedFlags, colorMapDataValue, (lighting == 789516) ? meshPoolsDark : meshPools, 1f, rotIndex);
			verts += 4;
		}
		return;
		IL_0161:
		TextureAtlasPosition texPos = textureAtlasPositionsByTextureSubId[textureSubId];
		JsonTesselator.AddDecor(targetBlock, vars, BlockFacing.ALLFACES[mainSide], texPos, rotIndex);
	}

	public void DrawBlockFace(TCTCache vars, int tileSide, FastVec3f[] quadOffsets, TextureAtlasPosition texPos, int flags, int colorMapDataValue, MeshData[] meshPools, float blockHeight = 1f, int rotIndex = 0)
	{
		MeshData toreturn = meshPools[texPos.atlasNumber];
		int lastelement = toreturn.VerticesCount;
		int[] currentLightRGBByCorner = vars.CurrentLightRGBByCorner;
		float uvx1 = texPos.x1;
		float uvy1 = texPos.y1;
		float uvx2 = texPos.x2;
		float uvy2 = texPos.y2;
		if (rotIndex > 1)
		{
			uvx1 = texPos.x2;
			uvy1 = texPos.y2;
			uvx2 = texPos.x1;
			uvy2 = texPos.y1;
		}
		if (rotIndex == 1 || rotIndex == 3)
		{
			float uvwdt = uvx2 - uvx1;
			float uvhgt = uvy2 - uvy1;
			float uvbx = uvx1;
			float uvby = uvy1;
			uvx1 = uvbx + uvwdt;
			uvy1 = uvby;
			uvx2 = uvbx;
			uvy2 = uvby + uvhgt;
		}
		Vec3f normalf = BlockFacing.ALLFACES[tileSide].Normalf;
		float subFactor = 1f;
		int subpos = vars.decorSubPosition - 1;
		if (subpos >= 0)
		{
			float xx = subpos % 16;
			float yy = subpos / 16;
			subFactor += xx % 7f / 7f + yy % 4f / 2f;
			subFactor /= 8f;
		}
		float x = vars.finalX - normalf.X * 0.001953125f * subFactor;
		float y = vars.finalY - normalf.Y * 0.001953125f * subFactor;
		float z = vars.finalZ - normalf.Z * 0.001953125f * subFactor;
		float factor = 1.0001f;
		float dxTL = 0f;
		float dxBL = 0f;
		float dxTR = 0f;
		float dxBR = 0f;
		float dyT = 0f;
		float dyB = 0f;
		float dzTL = 0f;
		float dzBL = 0f;
		float dzTR = 0f;
		float dzBR = 0f;
		int uvIndex = (8 - vars.decorRotationData % 4 * 2) % 8;
		float[] uv = this.uv;
		if (subpos >= 0)
		{
			string path = vars.block.Code.Path;
			float xSize = (uvx2 - uvx1) / (float)GlobalConstants.CaveArtColsPerRow;
			float ySize = (uvy2 - uvy1) / (float)GlobalConstants.CaveArtColsPerRow;
			uvx1 += (float)(path[path.Length - 3] - 49) * xSize;
			uvy1 += (float)(path[path.Length - 1] - 49) * ySize;
			uvx2 = uvx1 + xSize;
			uvy2 = uvy1 + ySize;
			factor /= 16f;
			float xx2 = (float)(subpos % 16) * factor;
			float yy2 = (float)(subpos / 16) * factor;
			float nearOne = 0.9375f;
			factor *= 4f;
			float minLimit = factor;
			float maxLimit = 1f - factor;
			switch (tileSide)
			{
			case 0:
				x += xx2 - factor;
				y += nearOne - yy2 - factor;
				if (xx2 > maxLimit)
				{
					if (!CaveArtBlockOnSide(vars, tileSide, BlockFacing.EAST))
					{
						float excess13 = (xx2 - maxLimit) / 0.5f;
						cropRightSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess13, vars.decorRotationData);
						dxTL = (0f - factor) * 2f * excess13;
						dxBL = (0f - factor) * 2f * excess13;
					}
				}
				else if (xx2 < minLimit && !CaveArtBlockOnSide(vars, tileSide, BlockFacing.WEST))
				{
					float excess14 = (minLimit - xx2) / 0.5f;
					cropLeftSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess14, vars.decorRotationData);
					dxTR = factor * 2f * excess14;
					dxBR = factor * 2f * excess14;
				}
				if (nearOne - yy2 > maxLimit)
				{
					if (!CaveArtBlockOnSide(vars, tileSide, BlockFacing.UP))
					{
						float excess15 = (nearOne - yy2 - maxLimit) / 0.5f;
						cropTopSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess15, vars.decorRotationData);
						dyT = (0f - factor) * 2f * excess15;
					}
				}
				else if (nearOne - yy2 < minLimit && !CaveArtBlockOnSide(vars, tileSide, BlockFacing.DOWN))
				{
					float excess16 = (minLimit - nearOne + yy2) / 0.5f;
					cropBottomSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess16, vars.decorRotationData);
					dyB = factor * 2f * excess16;
				}
				break;
			case 1:
				z += xx2 - factor;
				y += nearOne - yy2 - factor;
				x += 0.5f;
				if (xx2 > maxLimit)
				{
					if (!CaveArtBlockOnSide(vars, tileSide, BlockFacing.SOUTH))
					{
						float excess9 = (xx2 - maxLimit) / 0.5f;
						cropRightSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess9, vars.decorRotationData);
						dzTL = (0f - factor) * 2f * excess9;
						dzBL = (0f - factor) * 2f * excess9;
					}
				}
				else if (xx2 < minLimit && !CaveArtBlockOnSide(vars, tileSide, BlockFacing.NORTH))
				{
					float excess10 = (minLimit - xx2) / 0.5f;
					cropLeftSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess10, vars.decorRotationData);
					dzTR = factor * 2f * excess10;
					dzBR = factor * 2f * excess10;
				}
				if (nearOne - yy2 > maxLimit)
				{
					if (!CaveArtBlockOnSide(vars, tileSide, BlockFacing.UP))
					{
						float excess11 = (nearOne - yy2 - maxLimit) / 0.5f;
						cropTopSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess11, vars.decorRotationData);
						dyT = (0f - factor) * 2f * excess11;
					}
				}
				else if (nearOne - yy2 < minLimit && !CaveArtBlockOnSide(vars, tileSide, BlockFacing.DOWN))
				{
					float excess12 = (minLimit - nearOne + yy2) / 0.5f;
					cropBottomSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess12, vars.decorRotationData);
					dyB = factor * 2f * excess12;
				}
				break;
			case 2:
				x += nearOne - xx2 - factor;
				y += nearOne - yy2 - factor;
				z += 0.5f;
				if (nearOne - xx2 > maxLimit)
				{
					if (!CaveArtBlockOnSide(vars, tileSide, BlockFacing.EAST))
					{
						float excess17 = (nearOne - xx2 - maxLimit) / 0.5f;
						cropLeftSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess17, vars.decorRotationData);
						dxTR = (0f - factor) * 2f * excess17;
						dxBR = (0f - factor) * 2f * excess17;
					}
				}
				else if (nearOne - xx2 < minLimit && !CaveArtBlockOnSide(vars, tileSide, BlockFacing.WEST))
				{
					float excess18 = (minLimit - nearOne + xx2) / 0.5f;
					cropRightSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess18, vars.decorRotationData);
					dxTL = factor * 2f * excess18;
					dxBL = factor * 2f * excess18;
				}
				if (nearOne - yy2 > maxLimit)
				{
					if (!CaveArtBlockOnSide(vars, tileSide, BlockFacing.UP))
					{
						float excess19 = (nearOne - yy2 - maxLimit) / 0.5f;
						cropTopSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess19, vars.decorRotationData);
						dyT = (0f - factor) * 2f * excess19;
					}
				}
				else if (nearOne - yy2 < minLimit && !CaveArtBlockOnSide(vars, tileSide, BlockFacing.DOWN))
				{
					float excess20 = (minLimit - nearOne + yy2) / 0.5f;
					cropBottomSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess20, vars.decorRotationData);
					dyB = factor * 2f * excess20;
				}
				break;
			case 3:
				z += nearOne - xx2 - factor;
				y += nearOne - yy2 - factor;
				if (nearOne - xx2 > maxLimit)
				{
					if (!CaveArtBlockOnSide(vars, tileSide, BlockFacing.SOUTH))
					{
						float excess21 = (nearOne - xx2 - maxLimit) / 0.5f;
						cropLeftSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess21, vars.decorRotationData);
						dzTR = (0f - factor) * 2f * excess21;
						dzBR = (0f - factor) * 2f * excess21;
					}
				}
				else if (nearOne - xx2 < minLimit && !CaveArtBlockOnSide(vars, tileSide, BlockFacing.NORTH))
				{
					float excess22 = (minLimit - nearOne + xx2) / 0.5f;
					cropRightSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess22, vars.decorRotationData);
					dzTL = factor * 2f * excess22;
					dzBL = factor * 2f * excess22;
				}
				if (nearOne - yy2 > maxLimit)
				{
					if (!CaveArtBlockOnSide(vars, tileSide, BlockFacing.UP))
					{
						float excess23 = (nearOne - yy2 - maxLimit) / 0.5f;
						cropTopSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess23, vars.decorRotationData);
						dyT = (0f - factor) * 2f * excess23;
					}
				}
				else if (nearOne - yy2 < minLimit && !CaveArtBlockOnSide(vars, tileSide, BlockFacing.DOWN))
				{
					float excess24 = (minLimit - nearOne + yy2) / 0.5f;
					cropBottomSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess24, vars.decorRotationData);
					dyB = factor * 2f * excess24;
				}
				break;
			case 4:
				x += xx2 - factor;
				z += nearOne - yy2 - factor;
				y += 0.5f;
				if (xx2 > maxLimit)
				{
					if (!CaveArtBlockOnSide(vars, tileSide, BlockFacing.EAST))
					{
						float excess5 = (xx2 - maxLimit) / 0.5f;
						cropRightSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess5, vars.decorRotationData);
						dxTL = (0f - factor) * 2f * excess5;
						dxBL = (0f - factor) * 2f * excess5;
					}
				}
				else if (xx2 < minLimit && !CaveArtBlockOnSide(vars, tileSide, BlockFacing.WEST))
				{
					float excess6 = (minLimit - xx2) / 0.5f;
					cropLeftSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess6, vars.decorRotationData);
					dxTR = factor * 2f * excess6;
					dxBR = factor * 2f * excess6;
				}
				if (nearOne - yy2 > maxLimit)
				{
					if (!CaveArtBlockOnSide(vars, tileSide, BlockFacing.SOUTH))
					{
						float excess7 = (nearOne - yy2 - maxLimit) / 0.5f;
						cropTopSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess7, vars.decorRotationData);
						dzTR = (0f - factor) * 2f * excess7;
						dzTL = (0f - factor) * 2f * excess7;
					}
				}
				else if (nearOne - yy2 < minLimit && !CaveArtBlockOnSide(vars, tileSide, BlockFacing.NORTH))
				{
					float excess8 = (minLimit - nearOne + yy2) / 0.5f;
					cropBottomSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess8, vars.decorRotationData);
					dzBR = factor * 2f * excess8;
					dzBL = factor * 2f * excess8;
				}
				break;
			case 5:
				x += xx2 - factor;
				z += yy2 - factor;
				if (xx2 > maxLimit)
				{
					if (!CaveArtBlockOnSide(vars, tileSide, BlockFacing.EAST))
					{
						float excess = (xx2 - maxLimit) / 0.5f;
						cropRightSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess, vars.decorRotationData);
						dxTL = (0f - factor) * 2f * excess;
						dxBL = (0f - factor) * 2f * excess;
					}
				}
				else if (xx2 < minLimit && !CaveArtBlockOnSide(vars, tileSide, BlockFacing.WEST))
				{
					float excess2 = (minLimit - xx2) / 0.5f;
					cropLeftSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess2, vars.decorRotationData);
					dxTR = factor * 2f * excess2;
					dxBR = factor * 2f * excess2;
				}
				if (yy2 > maxLimit)
				{
					if (!CaveArtBlockOnSide(vars, tileSide, BlockFacing.SOUTH))
					{
						float excess3 = (yy2 - maxLimit) / 0.5f;
						cropBottomSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess3, vars.decorRotationData);
						dzBR = (0f - factor) * 2f * excess3;
						dzBL = (0f - factor) * 2f * excess3;
					}
				}
				else if (yy2 < minLimit && !CaveArtBlockOnSide(vars, tileSide, BlockFacing.NORTH))
				{
					float excess4 = (minLimit - yy2) / 0.5f;
					cropTopSide(ref uvx1, ref uvx2, ref uvy1, ref uvy2, xSize, ySize, excess4, vars.decorRotationData);
					dzTR = factor * 2f * excess4;
					dzTL = factor * 2f * excess4;
				}
				break;
			}
			factor *= 2f;
		}
		if ((vars.decorRotationData & 4) > 0)
		{
			uv[0] = (uv[6] = uvx1);
			uv[2] = (uv[4] = uvx2);
		}
		else
		{
			uv[0] = (uv[6] = uvx2);
			uv[2] = (uv[4] = uvx1);
		}
		uv[1] = (uv[3] = uvy1);
		uv[5] = (uv[7] = uvy2);
		float seamPrevention = ((subpos >= 0) ? 0f : 0.001953125f);
		FastVec3f tmpv = quadOffsets[6] * factor;
		tmpv.Add(dxBL, dyB, dzBL);
		if (seamPrevention != 0f)
		{
			tmpv *= 1f + seamPrevention;
		}
		toreturn.AddVertexWithFlags(x + tmpv.X, y + tmpv.Y, z + tmpv.Z, uv[(uvIndex + 4) % 8], uv[(uvIndex + 5) % 8], currentLightRGBByCorner[(tileSide <= 3) ? 3 : 0], flags);
		tmpv = quadOffsets[4] * factor;
		tmpv.Add(dxTL, dyT, dzTL);
		if (seamPrevention != 0f)
		{
			tmpv *= 1f + seamPrevention;
		}
		toreturn.AddVertexWithFlags(x + tmpv.X, y + tmpv.Y, z + tmpv.Z, uv[(uvIndex + 2) % 8], uv[(uvIndex + 3) % 8], currentLightRGBByCorner[(tileSide <= 3) ? 1 : 2], flags);
		tmpv = quadOffsets[5] * factor;
		tmpv.Add(dxTR, dyT, dzTR);
		if (seamPrevention != 0f)
		{
			tmpv *= 1f + seamPrevention;
		}
		toreturn.AddVertexWithFlags(x + tmpv.X, y + tmpv.Y, z + tmpv.Z, uv[uvIndex], uv[uvIndex + 1], currentLightRGBByCorner[(tileSide > 3) ? 3 : 0], flags);
		tmpv = quadOffsets[7] * factor;
		tmpv.Add(dxBR, dyB, dzBR);
		if (seamPrevention != 0f)
		{
			tmpv *= 1f + seamPrevention;
		}
		toreturn.AddVertexWithFlags(x + tmpv.X, y + tmpv.Y, z + tmpv.Z, uv[(uvIndex + 6) % 8], uv[(uvIndex + 7) % 8], currentLightRGBByCorner[(tileSide > 3) ? 1 : 2], flags);
		toreturn.CustomInts.Add4(colorMapDataValue);
		toreturn.AddQuadIndices(lastelement);
	}

	private void cropRightSide(ref float uvx1, ref float uvx2, ref float uvy1, ref float uvy2, float xSize, float ySize, float excess, int rot)
	{
		switch (rot % 8)
		{
		case 0:
		case 6:
			uvx1 += xSize * excess;
			break;
		case 1:
		case 5:
			uvy1 += ySize * excess;
			break;
		case 2:
		case 4:
			uvx2 -= xSize * excess;
			break;
		case 3:
		case 7:
			uvy2 -= ySize * excess;
			break;
		}
	}

	private void cropLeftSide(ref float uvx1, ref float uvx2, ref float uvy1, ref float uvy2, float xSize, float ySize, float excess, int rot)
	{
		switch (rot % 8)
		{
		case 2:
		case 4:
			uvx1 += xSize * excess;
			break;
		case 3:
		case 7:
			uvy1 += ySize * excess;
			break;
		case 0:
		case 6:
			uvx2 -= xSize * excess;
			break;
		case 1:
		case 5:
			uvy2 -= ySize * excess;
			break;
		}
	}

	private void cropTopSide(ref float uvx1, ref float uvx2, ref float uvy1, ref float uvy2, float xSize, float ySize, float excess, int rot)
	{
		switch (rot % 8)
		{
		case 3:
		case 5:
			uvx1 += xSize * excess;
			break;
		case 0:
		case 4:
			uvy1 += ySize * excess;
			break;
		case 1:
		case 7:
			uvx2 -= xSize * excess;
			break;
		case 2:
		case 6:
			uvy2 -= ySize * excess;
			break;
		}
	}

	private void cropBottomSide(ref float uvx1, ref float uvx2, ref float uvy1, ref float uvy2, float xSize, float ySize, float excess, int rot)
	{
		switch (rot % 8)
		{
		case 1:
		case 7:
			uvx1 += xSize * excess;
			break;
		case 2:
		case 6:
			uvy1 += ySize * excess;
			break;
		case 3:
		case 5:
			uvx2 -= xSize * excess;
			break;
		case 0:
		case 4:
			uvy2 -= ySize * excess;
			break;
		}
	}

	private bool CaveArtBlockOnSide(TCTCache vars, int tileSide, BlockFacing neibDir)
	{
		int extIndex3d = vars.extIndex3d;
		EnumBlockMaterial baseMaterial = vars.tct.currentChunkBlocksExt[extIndex3d].BlockMaterial;
		switch (neibDir.Index)
		{
		case 0:
			extIndex3d -= 34;
			break;
		case 1:
			extIndex3d++;
			break;
		case 2:
			extIndex3d += 34;
			break;
		case 3:
			extIndex3d--;
			break;
		case 4:
			extIndex3d += 1156;
			break;
		case 5:
			extIndex3d -= 1156;
			break;
		}
		Block neib = vars.tct.currentChunkBlocksExt[extIndex3d];
		if (neib.SideSolid[tileSide])
		{
			return neib.BlockMaterial == baseMaterial;
		}
		return false;
	}
}
