using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public class TCTCache : IGeometryTester
{
	public const long DARK = 789516L;

	public FastVec3f[][] blockFaceVertices = CubeFaceVertices.blockFaceVertices;

	public int lx;

	public int ly;

	public int lz;

	public int posX;

	public int posY;

	public int posZ;

	public int dimension;

	public int extIndex3d;

	public int index3d;

	public float finalX;

	public float finalY;

	public float finalZ;

	public float xMin;

	public float xMax;

	public float yMin;

	public float yMax;

	public float zMin;

	public float zMax;

	public int drawFaceFlags;

	public int blockId;

	public Block block;

	public ShapeTesselatorManager shapes;

	public float[] preRotationMatrix;

	public bool isDecorTesselate;

	public int textureSubId;

	public float textureVOffset;

	public int decorSubPosition;

	public int decorRotationData;

	public ColorMapData ColorMapData;

	public int VertexFlags;

	public EnumChunkRenderPass RenderPass;

	public BlockFacing OverlayTextureFace;

	public TextureAtlasPosition OverlayTexturePos;

	public int OverlayColorMapData;

	public Block OverlayBlock;

	public float occ;

	public float halfoccInverted;

	private readonly int[] neighbourLightRGBS;

	public readonly int[] CurrentLightRGBByCorner;

	public ChunkTesselator tct;

	public TextureAtlasPosition[] textureAtlasPositionsByTextureSubId;

	public int[] fastBlockTextureSubidsByFace;

	public bool aoAndSmoothShadows;

	public const int chunkSize = 32;

	public const int extChunkSize = 34;

	public const int extMovey = 1156;

	internal Dictionary<BlockPos, BlockEntity> blockEntitiesOfChunk = new Dictionary<BlockPos, BlockEntity>();

	private readonly BlockPos tmpPos = new BlockPos(0);

	public ushort[] rainHeightMap;

	public int OceanityFlagTL;

	public int OceanityFlagTR;

	public int OceanityFlagBL;

	public int OceanityFlagBR;

	public bool IsDecorOnJson => OverlayTextureFace != null;

	public TCTCache(ChunkTesselator tct)
	{
		this.tct = tct;
		blockFaceVertices = CubeFaceVertices.blockFaceVertices;
		occ = 0.67f;
		halfoccInverted = 0.0196875f;
		neighbourLightRGBS = new int[9];
		CurrentLightRGBByCorner = new int[4];
	}

	internal void Start(ClientMain game)
	{
		shapes = game.TesselatorManager;
	}

	internal void SetDimension(int dim)
	{
		dimension = dim;
		tmpPos.SetDimension(dim);
	}

	internal long CalcBlockFaceLight(int tileSide, int extNeibIndex3d)
	{
		int extIndex3d = this.extIndex3d;
		int[] CurrentLightRGBByCorner = this.CurrentLightRGBByCorner;
		if (!aoAndSmoothShadows || !block.SideAo[tileSide])
		{
			int rgb = tct.currentChunkRgbsExt[extNeibIndex3d];
			if (block.DrawType == EnumDrawType.JSON && !block.SideAo[tileSide])
			{
				int absorption = (int)(GameMath.Clamp(((float?)tct.currentChunkBlocksExt[extNeibIndex3d]?.LightAbsorption / 32f).GetValueOrDefault(), 0f, 1f) * 255f);
				int num = tct.currentChunkRgbsExt[extIndex3d];
				int newSunLight = Math.Max((num >> 24) & 0xFF, ((rgb >> 24) & 0xFF) - absorption);
				int hsv = ColorUtil.Rgb2HSv(rgb);
				int oldV = hsv & 0xFF;
				int v = Math.Max(0, oldV - absorption);
				if (v != oldV)
				{
					hsv = (hsv & 0xFFFF00) | v;
					rgb = ColorUtil.Hsv2Rgb(hsv);
				}
				int newR = Math.Max((byte)(num >> 16), (byte)(rgb >> 16));
				int newG = Math.Max((byte)(num >> 8), (byte)(rgb >> 8));
				int newB = Math.Max((byte)num, (byte)rgb);
				rgb = (newSunLight << 24) | (newR << 16) | (newG << 8) | newB;
			}
			CurrentLightRGBByCorner[0] = (CurrentLightRGBByCorner[1] = (CurrentLightRGBByCorner[2] = (CurrentLightRGBByCorner[3] = rgb)));
			return rgb * 4;
		}
		int[] neighbourLightRGBS = this.neighbourLightRGBS;
		int[] currentChunkRgbsExt = tct.currentChunkRgbsExt;
		Block[] currentChunkBlocksExt = tct.currentChunkBlocksExt;
		Block[] currentChunkFluidBlocksExt = tct.currentChunkFluidBlocksExt;
		Vec3iAndFacingFlags[] vNeighbors = CubeFaceVertices.blockFaceVerticesCentered[tileSide];
		int blockRGB = currentChunkRgbsExt[extNeibIndex3d];
		bool thisIsALeaf = block.BlockMaterial == EnumBlockMaterial.Leaves;
		Block blockFront = currentChunkFluidBlocksExt[extNeibIndex3d];
		bool ao;
		if (blockFront.LightAbsorption > 0)
		{
			ao = true;
		}
		else
		{
			BlockFacing frontFacing = BlockFacing.ALLFACES[tileSide];
			blockFront = currentChunkBlocksExt[extNeibIndex3d];
			ao = blockFront.DoEmitSideAo(this, frontFacing.Opposite);
		}
		float frontAoTR = (ao ? occ : 1f);
		float frontAoTL = frontAoTR;
		float frontAoBR = frontAoTR;
		float frontAoBL = frontAoTR;
		int neighbourLighter = 0;
		int frontCornersLighter = 0;
		int i = 0;
		while (i < 8)
		{
			neighbourLighter <<= 1;
			Vec3iAndFacingFlags neibOffset = vNeighbors[i];
			int dirExtIndex3d = extIndex3d + neibOffset.extIndexOffset;
			Block nblock = currentChunkFluidBlocksExt[dirExtIndex3d];
			if (nblock.LightAbsorption > 0)
			{
				ao = false;
				neighbourLighter |= 1;
				if (i <= 3)
				{
					neighbourLighter <<= 1;
					neighbourLighter |= 1;
				}
				else
				{
					frontCornersLighter <<= 1;
					if (!blockFront.DoEmitSideAoByFlag(this, vNeighbors[8], neibOffset.FacingFlags) || (blockFront.ForFluidsLayer && blockFront.LightAbsorption > 0))
					{
						frontCornersLighter |= 1;
					}
				}
			}
			else
			{
				nblock = currentChunkBlocksExt[dirExtIndex3d];
				if (i <= 3)
				{
					ao = nblock.DoEmitSideAoByFlag(this, neibOffset, neibOffset.OppositeFlagsUpperOrLeft) || (thisIsALeaf && nblock.BlockMaterial == EnumBlockMaterial.Leaves);
					if (!ao)
					{
						neighbourLighter |= 1;
					}
					neighbourLighter <<= 1;
					if (!nblock.DoEmitSideAoByFlag(this, neibOffset, neibOffset.OppositeFlagsLowerOrRight) && (!thisIsALeaf || nblock.BlockMaterial != EnumBlockMaterial.Leaves))
					{
						neighbourLighter |= 1;
						ao = false;
					}
				}
				else
				{
					frontCornersLighter <<= 1;
					ao = nblock.DoEmitSideAoByFlag(this, neibOffset, neibOffset.OppositeFlags) || (thisIsALeaf && nblock.BlockMaterial == EnumBlockMaterial.Leaves);
					if (!ao)
					{
						neighbourLighter |= 1;
					}
					bool frontSideAo = blockFront.DoEmitSideAoByFlag(this, vNeighbors[8], neibOffset.FacingFlags);
					if (!frontSideAo || (blockFront.ForFluidsLayer && blockFront.LightAbsorption > 0))
					{
						frontCornersLighter |= 1;
						if (!frontSideAo)
						{
							switch (i)
							{
							case 4:
								frontAoTL = 1f;
								break;
							case 5:
								frontAoTR = 1f;
								break;
							case 6:
								frontAoBL = 1f;
								break;
							case 7:
								frontAoBR = 1f;
								break;
							}
						}
					}
				}
			}
			i++;
			neighbourLightRGBS[i] = (ao ? blockRGB : currentChunkRgbsExt[dirExtIndex3d]);
		}
		int doBottomRight = 8 * (neighbourLighter & 1);
		int doBottomLeft = 7 * ((neighbourLighter >>= 1) & 1);
		int doTopRight = 6 * ((neighbourLighter >>= 1) & 1);
		int doTopLeft = 5 * ((neighbourLighter >>= 1) & 1);
		int doRightLower = 4 * ((neighbourLighter >>= 1) & 1);
		int doRightUpper = 4 * ((neighbourLighter >>= 1) & 1);
		int doLeftLower = 3 * ((neighbourLighter >>= 1) & 1);
		int doLeftUpper = 3 * ((neighbourLighter >>= 1) & 1);
		int doBottomLHS = 2 * ((neighbourLighter >>= 1) & 1);
		int doBottomRHS = 2 * ((neighbourLighter >>= 1) & 1);
		int doTopLHS = (neighbourLighter >>= 1) & 1;
		int doTopRHS = neighbourLighter >> 1;
		if (tileSide >= 4)
		{
			int num2 = doTopRHS;
			doTopRHS = doTopLHS;
			doTopLHS = num2;
			int num3 = doBottomRHS;
			doBottomRHS = doBottomLHS;
			doBottomLHS = num3;
		}
		ushort sunLight = (ushort)((blockRGB >> 24) & 0xFF);
		ushort r = (ushort)((blockRGB >> 16) & 0xFF);
		ushort g = (ushort)((blockRGB >> 8) & 0xFF);
		ushort b = (ushort)(blockRGB & 0xFF);
		return (long)(CurrentLightRGBByCorner[0] = CornerAoRGB(doTopLHS, doLeftUpper, doTopLeft, frontCornersLighter & 1, frontAoTL, sunLight, r, g, b)) + (long)(CurrentLightRGBByCorner[1] = CornerAoRGB(doTopRHS, doRightUpper, doTopRight, (frontCornersLighter >> 1) & 1, frontAoTR, sunLight, r, g, b)) + (CurrentLightRGBByCorner[2] = CornerAoRGB(doBottomLHS, doLeftLower, doBottomLeft, (frontCornersLighter >> 2) & 1, frontAoBL, sunLight, r, g, b)) + (CurrentLightRGBByCorner[3] = CornerAoRGB(doBottomRHS, doRightLower, doBottomRight, (frontCornersLighter >> 3) & 1, frontAoBR, sunLight, r, g, b));
	}

	private int CornerAoRGB(int ndir1, int ndir2, int ndirbetween, int frontCorner, float frontAo, ushort s, ushort r, ushort g, ushort b)
	{
		float cornerAO;
		if (ndir1 + ndir2 == 0 || frontCorner + ndirbetween == 0)
		{
			float brightnessloss = halfoccInverted * (float)GameMath.Clamp(block.LightAbsorption, 0, 32);
			cornerAO = Math.Min(occ, 1f - brightnessloss);
		}
		else
		{
			cornerAO = ((ndir1 * ndir2 * ndirbetween == 0) ? occ : frontAo);
			int facesconsidered = 1;
			if (ndir1 > 0)
			{
				int blockRGB = neighbourLightRGBS[ndir1];
				s += (ushort)((blockRGB >> 24) & 0xFF);
				r += (ushort)((blockRGB >> 16) & 0xFF);
				g += (ushort)((blockRGB >> 8) & 0xFF);
				b += (ushort)(blockRGB & 0xFF);
				facesconsidered++;
			}
			if (ndir2 > 0)
			{
				int blockRGB = neighbourLightRGBS[ndir2];
				s += (ushort)((blockRGB >> 24) & 0xFF);
				r += (ushort)((blockRGB >> 16) & 0xFF);
				g += (ushort)((blockRGB >> 8) & 0xFF);
				b += (ushort)(blockRGB & 0xFF);
				facesconsidered++;
			}
			if (ndirbetween > 0)
			{
				int blockRGB = neighbourLightRGBS[ndirbetween];
				s += (ushort)((blockRGB >> 24) & 0xFF);
				r += (ushort)((blockRGB >> 16) & 0xFF);
				g += (ushort)((blockRGB >> 8) & 0xFF);
				b += (ushort)(blockRGB & 0xFF);
				facesconsidered++;
			}
			cornerAO /= (float)facesconsidered;
		}
		return ((int)((float)(int)s * cornerAO) << 24) | ((int)((float)(int)r * cornerAO) << 16) | ((int)((float)(int)g * cornerAO) << 8) | (int)((float)(int)b * cornerAO);
	}

	public BlockEntity GetCurrentBlockEntityOnSide(BlockFacing side)
	{
		tmpPos.Set(posX, posY, posZ).Offset(side);
		return tct.game.BlockAccessor.GetBlockEntity(tmpPos);
	}

	public BlockEntity GetCurrentBlockEntityOnSide(Vec3iAndFacingFlags neibOffset)
	{
		tmpPos.Set(posX + neibOffset.X, posY + neibOffset.Y, posZ + neibOffset.Z);
		return tct.game.BlockAccessor.GetBlockEntity(tmpPos);
	}

	public void UpdateChunkMinMax(float x, float y, float z)
	{
		if (x < xMin)
		{
			xMin = x;
		}
		else if (x > xMax)
		{
			xMax = x;
		}
		if (y < yMin)
		{
			yMin = y;
		}
		else if (y > yMax)
		{
			yMax = y;
		}
		if (z < zMin)
		{
			zMin = z;
		}
		else if (z > zMax)
		{
			zMax = z;
		}
	}
}
