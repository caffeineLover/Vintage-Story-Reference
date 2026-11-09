using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Client.Tesselation;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public class JsonTesselator : IBlockTesselator
{
	private const string snowTexturesPath = "block/liquid/snow/";

	private const string iceTexturesPath = "block/liquid/ice/";

	private float decoru1;

	private float decoru2;

	private float decorv1;

	private float decorv2;

	private int decorVertexOffset;

	private int decorFace;

	private float decorSeamAdjustX1;

	private float decorSeamAdjustY1;

	private float decorSeamAdjustZ1;

	private float decorSeamAdjustX2;

	private float decorSeamAdjustY2;

	private float decorSeamAdjustZ2;

	private List<TextureAtlasPosition> snowTextures = new List<TextureAtlasPosition>();

	public const int DisableRandomsFlag = 1024;

	public const long Darkness = 4934475L;

	public TerrainMesherHelper helper = new TerrainMesherHelper();

	private readonly BlockPos pos = new BlockPos(0);

	internal int[] jsonLightRGB = new int[25];

	public static float[] reusableIdentityMatrix = Mat4f.Create();

	private float[] floatpool = new float[3];

	private int[] windFlagsMask = new int[64];

	private int[] windFlagsSet = new int[64];

	private static int[] faceCoordLookup = new int[6] { 2, 0, 2, 0, 1, 1 };

	private float[] tmpCoords = new float[3];

	private static int[][] axesByFacingLookup = new int[6][]
	{
		new int[2] { 0, 1 },
		new int[2] { 1, 2 },
		new int[2] { 0, 1 },
		new int[2] { 1, 2 },
		new int[2] { 0, 2 },
		new int[2] { 0, 2 }
	};

	private static int[][] indexesByFacingLookup = new int[6][]
	{
		new int[4] { 3, 2, 1, 0 },
		new int[4] { 3, 1, 2, 0 },
		new int[4] { 2, 3, 0, 1 },
		new int[4] { 2, 0, 3, 1 },
		new int[4] { 3, 2, 1, 0 },
		new int[4] { 1, 0, 3, 2 }
	};

	[ThreadStatic]
	private static float[] reusableFloatArray;

	[ThreadStatic]
	private static int[] reusableIntArray;

	public static void AddDecor(Block targetBlock, TCTCache vars, BlockFacing face, TextureAtlasPosition texPos, int rotIndex)
	{
		if (rotIndex > 0)
		{
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
			texPos = texPos.Clone();
			texPos.x1 = uvx1;
			texPos.y1 = uvy1;
			texPos.x2 = uvx2;
			texPos.y2 = uvy2;
		}
		vars.OverlayTextureFace = face;
		vars.OverlayTexturePos = texPos;
		vars.OverlayColorMapData = vars.ColorMapData.Value;
		vars.OverlayBlock = vars.block;
		vars.block = targetBlock;
		vars.blockId = targetBlock.BlockId;
		Vec3i delta = vars.OverlayTextureFace.Normali;
		vars.posX += delta.X;
		vars.posY += delta.Y;
		vars.posZ += delta.Z;
		vars.lx += delta.X;
		vars.ly += delta.Y;
		vars.lz += delta.Z;
		vars.finalX += delta.X;
		vars.finalY += delta.Y;
		vars.finalZ += delta.Z;
		try
		{
			vars.isDecorTesselate = false;
			vars.tct.jsonTesselator.Tesselate(vars);
		}
		finally
		{
			vars.OverlayTextureFace = null;
		}
	}

	public float FindDecorMaxPos(TCTCache vars, MeshData sourceMesh, float[] sourceMeshXYZ, float[] rotatedXyz, int sourceMeshXyzFacesCount, float xOff, float zOff, bool prevRotatedSource)
	{
		short[] renderPassesAndExtraBits = sourceMesh.RenderPassesAndExtraBits;
		bool haveRenderPasses = renderPassesAndExtraBits != null && renderPassesAndExtraBits.Length != 0;
		float decorMaxPos = 0f;
		decorFace = 0;
		if (vars.OverlayTextureFace != null)
		{
			decorFace = vars.OverlayTextureFace.Index;
			int centerFaceOffset = faceCoordLookup[decorFace];
			for (int i = 0; i < sourceMeshXyzFacesCount; i++)
			{
				bool rotatedSource = true;
				if (haveRenderPasses)
				{
					int renderpass = sourceMesh.RenderPassesAndExtraBits[i];
					if (renderpass >= 1024)
					{
						renderpass &= 0x3FF;
						rotatedSource = false;
					}
				}
				else
				{
					int renderpass = -1;
				}
				if (rotatedSource != prevRotatedSource)
				{
					prevRotatedSource = rotatedSource;
					sourceMeshXYZ = (rotatedSource ? rotatedXyz : sourceMesh.xyz);
				}
				float centerCoord = sourceMeshXYZ[i * 4 * 3 + centerFaceOffset];
				if (decorFace == 1 || decorFace == 2 || decorFace == 4)
				{
					centerCoord = 1f - centerCoord;
				}
				if (centerCoord > decorMaxPos)
				{
					decorMaxPos = centerCoord;
				}
			}
		}
		return decorMaxPos;
	}

	public bool SkipDecorFace(TCTCache vars, int i, float[] sourceMeshXYZ)
	{
		int centerIndex = i * 4 * 3 + faceCoordLookup[decorFace];
		float centerCoord = sourceMeshXYZ[centerIndex];
		if (decorFace == 1 || decorFace == 2 || decorFace == 4)
		{
			centerCoord = 1f - centerCoord;
		}
		return false;
	}

	public bool CalcAutoUVsForDecor(TCTCache vars, MeshData sourceMesh, int sourceVertexIndex, float[] tfMatrix, ref float currentXOff, ref float currentYOff, ref float currentZOff)
	{
		if (snowTextures.Count == 0)
		{
			PopulateSnowTextures(vars.tct.game);
		}
		int textureId = sourceMesh.TextureIds[sourceMesh.TextureIndices[sourceVertexIndex / 3 / sourceMesh.VerticesPerFace]];
		int sourceUVIndex = sourceVertexIndex / 3 * 2;
		float modelU1 = sourceMesh.Uv[sourceUVIndex++];
		float modelV1 = sourceMesh.Uv[sourceUVIndex++];
		sourceUVIndex += 2;
		float modelU2 = sourceMesh.Uv[sourceUVIndex++];
		float modelV2 = sourceMesh.Uv[sourceUVIndex++];
		foreach (TextureAtlasPosition tap in snowTextures)
		{
			if (tap.atlasTextureId == textureId && tap.ContainsUV(modelU1, modelV1) && tap.ContainsUV(modelU2, modelV2))
			{
				return false;
			}
		}
		decorVertexOffset = 0;
		float tmpu1 = vars.OverlayTexturePos.x1;
		float tmpu2 = vars.OverlayTexturePos.x2;
		float tmpv1 = vars.OverlayTexturePos.y1;
		float tmpv2 = vars.OverlayTexturePos.y2;
		float minOverlayU = Math.Min(tmpu1, tmpu2);
		float maxOverlayU = Math.Max(tmpu1, tmpu2);
		float minOverlayV = Math.Min(tmpv1, tmpv2);
		float maxOverlayV = Math.Max(tmpv1, tmpv2);
		float[] xyz = sourceMesh.xyz;
		FastVec3f topRight = new FastVec3f(xyz[sourceVertexIndex + 3], xyz[sourceVertexIndex + 4], xyz[sourceVertexIndex + 5]);
		FastVec3f sideA = new FastVec3f(xyz[sourceVertexIndex], xyz[sourceVertexIndex + 1], xyz[sourceVertexIndex + 2]) - topRight;
		FastVec3f sideB = new FastVec3f(xyz[sourceVertexIndex + 6], xyz[sourceVertexIndex + 7], xyz[sourceVertexIndex + 8]) - topRight;
		float uvWidth = tmpu2 - tmpu1;
		float uvHeight = tmpv2 - tmpv1;
		float widthInBlocks = GameMath.RoundTo(sideB.Length(), 64f);
		float heightInBlocks = GameMath.RoundTo(sideA.Length(), 64f);
		FastVec3f normal = sideB.Cross(sideA);
		float u1PosRel = 0f;
		float v1PosRel = 0f;
		if (Math.Abs(sideA.Y + sideB.Y) < 1f / 64f)
		{
			topRight += (sideA + sideB) * 0.5f;
			float num = GameMath.RoundTo(topRight.X, 64f);
			float dz = GameMath.RoundTo(topRight.Z, 64f);
			u1PosRel = num - widthInBlocks * 0.5f;
			v1PosRel = dz - heightInBlocks * 0.5f;
			decoru1 = tmpu1 + GameMath.RoundTo(u1PosRel, 32f) * uvWidth;
			decorv1 = tmpv1 + GameMath.RoundTo(v1PosRel, 32f) * uvHeight;
			decoru2 = decoru1 + widthInBlocks * uvWidth;
			decorv2 = decorv1 + heightInBlocks * uvHeight;
		}
		else
		{
			FastVec3f corner2 = topRight + sideA;
			FastVec3f corner3 = topRight + sideB;
			FastVec3f corner4 = corner2 + sideB;
			if (corner2.Y > topRight.Y)
			{
				FastVec3f.ExchangeValues(ref topRight, ref corner2);
			}
			if (corner4.Y > corner3.Y)
			{
				FastVec3f.ExchangeValues(ref corner3, ref corner4);
			}
			FastVec3f highest;
			FastVec3f secondHighest;
			if (corner3.Y > topRight.Y)
			{
				highest = corner3;
				secondHighest = ((!(topRight.Y > corner4.Y)) ? corner4 : topRight);
			}
			else
			{
				highest = topRight;
				secondHighest = ((!(corner2.Y > corner3.Y)) ? corner3 : corner2);
			}
			bool num2 = Math.Abs(sideA.X + sideB.X) < Math.Abs(sideA.Z + sideB.Z);
			FastVec3f fastVec3f = (highest + secondHighest) * 0.5f;
			_ = highest.Z;
			_ = secondHighest.Z;
			float dx = GameMath.RoundTo(fastVec3f.X, 64f);
			float dz2 = GameMath.RoundTo(fastVec3f.Z, 64f);
			if (num2)
			{
				decorVertexOffset++;
				float num3 = widthInBlocks;
				widthInBlocks = heightInBlocks;
				heightInBlocks = num3;
				u1PosRel = ((normal.X < 0f) ? (1f - dx) : (0f - widthInBlocks));
				v1PosRel = ((highest.Z < secondHighest.Z) ? highest.Z : secondHighest.Z);
				decoru1 = tmpu1 + GameMath.RoundTo(u1PosRel, 32f) * uvWidth;
				decorv1 = tmpv1 + GameMath.RoundTo(v1PosRel, 32f) * uvHeight;
			}
			else
			{
				v1PosRel = dz2;
				float halfHeight = Math.Abs(heightInBlocks) / 2f;
				if (v1PosRel >= halfHeight)
				{
					v1PosRel -= halfHeight;
				}
				else if (v1PosRel >= 0.125f)
				{
					v1PosRel -= 0.125f;
				}
				else if (v1PosRel >= 0.0625f)
				{
					v1PosRel -= 0.0625f;
				}
				u1PosRel = ((highest.X < secondHighest.X) ? highest.X : secondHighest.X);
				decoru1 = tmpu1 + GameMath.RoundTo(u1PosRel, 32f) * uvWidth;
				decorv1 = tmpv1 + GameMath.RoundTo(v1PosRel, 32f) * uvHeight;
			}
			decoru2 = decoru1 + widthInBlocks * uvWidth;
			decorv2 = decorv1 + heightInBlocks * uvHeight;
			if (normal.X > 0f)
			{
				decoru2 = tmpu2 - GameMath.RoundTo(dx, 32f) * uvWidth;
				decoru1 = decoru2 - widthInBlocks * uvWidth;
			}
			float excess = minOverlayU - Math.Min(decoru1, decoru2);
			if (excess > 0f && excess < 1f)
			{
				decoru1 += excess;
				decoru2 += excess;
			}
			excess = Math.Max(decoru1, decoru2) - maxOverlayU;
			if (excess > 0f && excess < 1f)
			{
				decoru1 -= excess;
				decoru2 -= excess;
			}
			excess = minOverlayV - Math.Min(decorv1, decorv2);
			if (excess > 0f && excess < 1f)
			{
				decorv1 += excess;
				decorv2 += excess;
			}
			excess = Math.Max(decorv1, decorv2) - maxOverlayV;
			if (excess > 0f && excess < 1f)
			{
				decorv1 -= excess;
				decorv2 -= excess;
			}
			if (num2 && normal.X > 0f)
			{
				float tmp = decoru1;
				decoru1 = decoru2;
				decoru2 = tmp;
				tmp = decorv1;
				decorv1 = decorv2;
				decorv2 = tmp;
			}
		}
		if (Math.Min(decoru1, decoru2) < minOverlayU || Math.Max(decoru1, decoru2) > maxOverlayU)
		{
			return false;
		}
		if (Math.Min(decorv1, decorv2) < minOverlayV || Math.Max(decorv1, decorv2) > maxOverlayV)
		{
			return false;
		}
		normal.Normalize();
		if (tfMatrix != null)
		{
			Mat4f.MulWithVec3(tfMatrix, normal, normal);
		}
		currentXOff += normal.X * 0.001953125f / 4f;
		currentYOff += normal.Y * 0.001953125f / 4f;
		currentZOff += normal.Z * 0.001953125f / 4f;
		sideA.Normalize();
		decorSeamAdjustX1 = sideA.X * 0.001953125f / 4f;
		decorSeamAdjustY1 = sideA.Y * 0.001953125f / 4f;
		decorSeamAdjustZ1 = sideA.Z * 0.001953125f / 4f;
		sideB.Normalize();
		decorSeamAdjustX2 = sideB.X * 0.001953125f / 4f;
		decorSeamAdjustY2 = sideB.Y * 0.001953125f / 4f;
		decorSeamAdjustZ2 = sideB.Z * 0.001953125f / 4f;
		currentXOff += decorSeamAdjustX1 - decorSeamAdjustX2;
		currentYOff += decorSeamAdjustY1 - decorSeamAdjustY2;
		currentZOff += decorSeamAdjustZ1 - decorSeamAdjustZ2;
		return true;
	}

	public void ApplyDecorUVs(float[] targetMeshUV, int targetUVIndex, int k, ref float currentXOff, ref float currentYOff, ref float currentZOff)
	{
		int decorK = (k - 1 + decorVertexOffset) % 4 + 1;
		targetMeshUV[targetUVIndex++] = ((decorK > 2) ? decoru1 : decoru2);
		targetMeshUV[targetUVIndex++] = ((decorK % 4 < 2) ? decorv2 : decorv1);
		switch (k)
		{
		case 4:
			currentXOff -= 2f * decorSeamAdjustX1;
			currentYOff -= 2f * decorSeamAdjustY1;
			currentZOff -= 2f * decorSeamAdjustZ1;
			break;
		case 3:
			currentXOff += 2f * decorSeamAdjustX2;
			currentYOff += 2f * decorSeamAdjustY2;
			currentZOff += 2f * decorSeamAdjustZ2;
			break;
		case 2:
			currentXOff += 2f * decorSeamAdjustX1;
			currentYOff += 2f * decorSeamAdjustY1;
			currentZOff += 2f * decorSeamAdjustZ1;
			break;
		}
	}

	private void PopulateSnowTextures(ClientMain game)
	{
		foreach (IAsset item in game.AssetManager.GetManyInCategory("textures", "block/liquid/snow/"))
		{
			AssetLocation loc = item.Location.CloneWithoutPrefixAndEnding("textures/".Length);
			TextureAtlasPosition tap = game.BlockAtlasManager[loc];
			if (tap != null)
			{
				snowTextures.Add(tap);
			}
		}
		foreach (IAsset item2 in game.AssetManager.GetManyInCategory("textures", "block/liquid/ice/"))
		{
			AssetLocation loc2 = item2.Location.CloneWithoutPrefixAndEnding("textures/".Length);
			TextureAtlasPosition tap2 = game.BlockAtlasManager[loc2];
			if (tap2 != null)
			{
				snowTextures.Add(tap2);
			}
		}
	}

	public JsonTesselator()
	{
		helper.tess = this;
	}

	public long SetUpLightRGBs(TCTCache vars)
	{
		int extIndex3d = vars.extIndex3d;
		int[] currentLightRGBByCorner = vars.CurrentLightRGBByCorner;
		int lightRGBindex = 0;
		long totalLight = 0L;
		int[] jsonLightRGB = this.jsonLightRGB;
		for (int tileSide = 0; tileSide < 6; tileSide++)
		{
			totalLight += vars.CalcBlockFaceLight(tileSide, extIndex3d + TileSideEnum.MoveIndex[tileSide]);
			jsonLightRGB[lightRGBindex++] = currentLightRGBByCorner[0];
			jsonLightRGB[lightRGBindex++] = currentLightRGBByCorner[1];
			jsonLightRGB[lightRGBindex++] = currentLightRGBByCorner[2];
			jsonLightRGB[lightRGBindex++] = currentLightRGBByCorner[3];
		}
		return totalLight + (jsonLightRGB[24] = vars.tct.currentChunkRgbsExt[extIndex3d]);
	}

	public void Tesselate(TCTCache vars)
	{
		int extIndex3d = vars.extIndex3d;
		long totalLight = SetUpLightRGBs(vars);
		helper.vars = vars;
		pos.SetDimension(vars.dimension);
		pos.Set(vars.posX, vars.posY, vars.posZ);
		if (!vars.isDecorTesselate)
		{
			Dictionary<BlockPos, BlockEntity> blockEntitiesOfChunk = vars.blockEntitiesOfChunk;
			if (blockEntitiesOfChunk != null && blockEntitiesOfChunk.TryGetValue(pos, out var be))
			{
				try
				{
					if (be != null && be.OnTesselation(helper, vars.tct.game.TesselatorManager.Tesselator))
					{
						return;
					}
				}
				catch (RetryTesselationException)
				{
					throw;
				}
				catch (Exception e)
				{
					vars.tct.game.Logger.Error("Exception thrown during OnTesselation() of block entity {0}@{1}/{2}/{3}. Block will probably not be rendered as intended.", be, vars.posX, vars.posY, vars.posZ);
					vars.tct.game.Logger.Error(e);
				}
			}
		}
		MeshData lod1Mesh = null;
		try
		{
			lod1Mesh = vars.shapes.GetDefaultBlockMesh(vars.block);
		}
		catch (Exception e2)
		{
			vars.tct.game.Logger.Error("Exception thrown during tesselation of block {0}@{1}/{2}/{3}. Block will be invisible.", vars.block.Code.ToShortString(), vars.posX, vars.posY, vars.posZ);
			vars.tct.game.Logger.Error(e2);
			return;
		}
		bool doNotRenderAtLod2 = vars.block.DoNotRenderAtLod2;
		if (vars.block.Lod0Shape != null)
		{
			if (NotSurrounded(vars, extIndex3d))
			{
				doMesh(vars, vars.block.Lod0Mesh, 0);
			}
			else
			{
				doNotRenderAtLod2 = true;
			}
		}
		if (totalLight == 4934475)
		{
			doMesh(vars, lod1Mesh, 0);
			return;
		}
		if (vars.block.Lod2Mesh == null)
		{
			doMesh(vars, lod1Mesh, (!doNotRenderAtLod2) ? 1 : 2);
			return;
		}
		doMesh(vars, lod1Mesh, 2);
		doMesh(vars, vars.block.Lod2Mesh, 3);
	}

	public static bool NotSurrounded(TCTCache vars, int extIndex3d)
	{
		if (vars.block.FaceCullMode != EnumFaceCullMode.CollapseMaterial)
		{
			return true;
		}
		for (int tileSide = 0; tileSide < 5; tileSide++)
		{
			Block nblock = vars.tct.currentChunkBlocksExt[extIndex3d + TileSideEnum.MoveIndex[tileSide]];
			if (nblock.BlockMaterial != vars.block.BlockMaterial && !nblock.SideOpaque[TileSideEnum.GetOpposite(tileSide)])
			{
				return true;
			}
		}
		return false;
	}

	public void doMesh(TCTCache vars, MeshData sourceMesh, int lodLevel)
	{
		if (sourceMesh.VerticesCount == 0)
		{
			return;
		}
		if (sourceMesh == null)
		{
			vars.block.DrawType = EnumDrawType.Cube;
			return;
		}
		MeshData[] altModels = (((lodLevel + 1) / 2 == 1) ? vars.shapes.altblockModelDatasLod1[vars.blockId] : ((lodLevel == 0) ? vars.shapes.altblockModelDatasLod0[vars.blockId] : vars.shapes.altblockModelDatasLod2[vars.blockId]));
		if (altModels != null)
		{
			int rnd = GameMath.MurmurHash3Mod(vars.posX, (vars.block.RandomizeAxes == EnumRandomizeAxes.XYZ) ? vars.posY : 0, vars.posZ, altModels.Length);
			sourceMesh = altModels[rnd];
		}
		vars.block.OnJsonTesselation(ref sourceMesh, ref jsonLightRGB, pos, vars.tct.currentChunkBlocksExt, vars.extIndex3d);
		if (vars.preRotationMatrix != null)
		{
			AddJsonModelDataToMesh(sourceMesh, lodLevel, vars, helper, vars.preRotationMatrix);
		}
		else if (vars.block.RandomizeRotations || vars.block.RandomSizeAdjust != 0f)
		{
			float[] matrix = (vars.block.RandomizeRotations ? TesselationMetaData.randomRotMatrices[GameMath.MurmurHash3Mod(vars.posX, (vars.block.RandomizeAxes == EnumRandomizeAxes.XYZ) ? vars.posY : 0, vars.posZ, TesselationMetaData.randomRotations.Length)] : reusableIdentityMatrix);
			AddJsonModelDataToMesh(sourceMesh, lodLevel, vars, helper, matrix);
		}
		else
		{
			AddJsonModelDataToMesh(sourceMesh, lodLevel, vars, helper, null);
		}
	}

	public void AddJsonModelDataToMesh(MeshData sourceMesh, int lodlevel, TCTCache vars, IMeshPoolSupplier poolSupplier, float[] tfMatrix)
	{
		AddJsonModelDataToMesh(sourceMesh, lodlevel, vars, poolSupplier, tfMatrix, null, 0);
	}

	public void AddJsonModelDataToMesh(MeshData sourceMesh, int lodlevel, TCTCache vars, IMeshPoolSupplier poolSupplier, float[] tfMatrix, IJsonTesselatorHooks hooks, int shapeRotationSteps)
	{
		if (sourceMesh.VerticesCount > windFlagsMask.Length)
		{
			windFlagsMask = new int[sourceMesh.VerticesCount];
			windFlagsSet = new int[sourceMesh.VerticesCount];
		}
		for (int i = 0; i < sourceMesh.VerticesCount; i++)
		{
			windFlagsMask[i] = -1;
			windFlagsSet[i] = 0;
		}
		Block fluidBlock = vars.tct.currentChunkFluidBlocksExt[vars.extIndex3d + vars.block.IceCheckOffset * 34 * 34];
		if (fluidBlock.BlockId != 0)
		{
			AdjustWindWaveForFluids(sourceMesh, fluidBlock);
		}
		float xOff = vars.finalX;
		float yOff = vars.finalY;
		float zOff = vars.finalZ;
		int baseRenderFlags = vars.VertexFlags;
		int drawFaceFlags = vars.drawFaceFlags;
		bool frostable = (vars.IsDecorOnJson ? vars.OverlayBlock.Frostable : vars.block.Frostable);
		byte colorLightA = 0;
		byte colorLightB = 0;
		byte colorLightG = 0;
		byte colorLightR = 0;
		float aoBright = 1f;
		byte temperature = vars.ColorMapData.Temperature;
		byte rainfall = vars.ColorMapData.Rainfall;
		int prevRenderPass = -2;
		int prevTextureId = -1;
		MeshData targetMesh = null;
		int colorTopLeft = 0;
		int colorTopRight = 0;
		int colorBottomLeft = 0;
		int colorBottomRight = 0;
		float[] tmpCoords = this.tmpCoords;
		float[] sourceMeshXYZ = sourceMesh.xyz;
		float[] sourceMeshUV = sourceMesh.Uv;
		int[] sourceMeshFlags = sourceMesh.Flags;
		int[] sourceMeshIndices = sourceMesh.Indices;
		float[] targetMeshXYZ = null;
		float[] targetMeshUV = null;
		byte[] targetMeshRgba = null;
		int[] targetMeshFlags = null;
		CustomMeshDataPartInt targetMeshCustomInts = null;
		float[] rotatedXyz = sourceMesh.xyz;
		int[] rotatedFlags = sourceMesh.Flags;
		if (poolSupplier == null)
		{
			poolSupplier = vars.tct;
		}
		int vertPerFace = sourceMesh.VerticesPerFace;
		int indPerFace = sourceMesh.IndicesPerFace;
		bool allowSSBOs = vars.tct.game.api.renderapi.useSSBOs;
		if ((allowSSBOs && vertPerFace != 4) || indPerFace != 6)
		{
			string warning = "Model " + vars.block.Code.ToShortString() + " does not have 4 vertices and 6 indices per face, will break chunk shader optimizations. Try disabling clientsetting: allowSSBOs.";
			vars.tct.game.Logger.Warning(warning);
		}
		if (tfMatrix != null)
		{
			int sourceMeshArrayCount = sourceMesh.VerticesCount * 3;
			rotatedXyz = NewFloatArray(sourceMeshArrayCount);
			float randomScalefactor = vars.block.RandomSizeAdjust;
			bool doRandomYScale = false;
			if (randomScalefactor != 0f)
			{
				if (randomScalefactor < 0f)
				{
					randomScalefactor = 0f - randomScalefactor;
				}
				else
				{
					doRandomYScale = true;
				}
				int elementHeight = (int)randomScalefactor;
				randomScalefactor = randomScalefactor % 1f * ((float)GameMath.MurmurHash3Mod(-vars.posX, 0, vars.posZ, 2000) / 1000f - 1f);
				yOff += (float)elementHeight * randomScalefactor;
			}
			for (int j = 0; j < sourceMeshArrayCount; j += 3)
			{
				if (randomScalefactor == 0f)
				{
					Mat4f.MulWithVec3_Position(tfMatrix, sourceMeshXYZ, rotatedXyz, j);
				}
				else if (doRandomYScale)
				{
					Mat4f.MulWithVec3_Position_AndScale(tfMatrix, sourceMeshXYZ, rotatedXyz, j, 1f + randomScalefactor);
				}
				else
				{
					Mat4f.MulWithVec3_Position_AndScaleXY(tfMatrix, sourceMeshXYZ, rotatedXyz, j, 1f + randomScalefactor);
				}
			}
			sourceMeshArrayCount = sourceMesh.FlagsCount;
			rotatedFlags = sourceMeshFlags;
			if (rotatedFlags != null && tfMatrix != reusableIdentityMatrix)
			{
				rotatedFlags = NewIntArray(sourceMeshArrayCount);
				for (int k = 0; k < sourceMeshArrayCount; k++)
				{
					int flagOrig = sourceMeshFlags[k];
					VertexFlags.UnpackNormal(flagOrig, floatpool);
					Mat4f.MulWithVec3(tfMatrix, floatpool, floatpool);
					float len = GameMath.RootSumOfSquares(floatpool[0], floatpool[1], floatpool[2]);
					rotatedFlags[k] = (flagOrig & -33546241) | VertexFlags.PackNormal(floatpool[0] / len, floatpool[1] / len, floatpool[2] / len);
				}
			}
		}
		int sourceMeshXyzFacesCount = sourceMesh.XyzFacesCount;
		short[] renderPassesAndExtraBits = sourceMesh.RenderPassesAndExtraBits;
		bool haveRenderPasses = renderPassesAndExtraBits != null && renderPassesAndExtraBits.Length != 0;
		bool prevRotatedSource = tfMatrix == null;
		FindDecorMaxPos(vars, sourceMesh, sourceMeshXYZ, rotatedXyz, sourceMeshXyzFacesCount, xOff, zOff, prevRotatedSource);
		for (int l = 0; l < sourceMeshXyzFacesCount; l++)
		{
			int textureid = ((!vars.IsDecorOnJson) ? sourceMesh.TextureIds[sourceMesh.TextureIndices[l * vertPerFace / sourceMesh.VerticesPerFace]] : vars.OverlayTexturePos.atlasTextureId);
			float currentXOff = xOff;
			float currentZOff = zOff;
			bool rotatedSource = true;
			int renderpass;
			if (haveRenderPasses)
			{
				renderpass = sourceMesh.RenderPassesAndExtraBits[l];
				if (renderpass >= 1024)
				{
					renderpass &= 0x3FF;
					rotatedSource = false;
					currentXOff = (int)(xOff + 0.5f);
					currentZOff = (int)(zOff + 0.5f);
				}
			}
			else
			{
				renderpass = -1;
			}
			if (rotatedSource != prevRotatedSource)
			{
				prevRotatedSource = rotatedSource;
				sourceMeshXYZ = (rotatedSource ? rotatedXyz : sourceMesh.xyz);
				sourceMeshFlags = (rotatedSource ? rotatedFlags : sourceMesh.Flags);
			}
			int localFace = sourceMesh.XyzFaces[l] - 1;
			int currentFace = localFace;
			if (currentFace >= 0)
			{
				if (tfMatrix != null && rotatedSource && tfMatrix != reusableIdentityMatrix)
				{
					currentFace = Mat4f.MulWithVec3_BlockFacing(tfMatrix, BlockFacing.ALLFACES[localFace].Normalf).Index;
				}
				if (((1 << currentFace) & drawFaceFlags) == 0)
				{
					int centerIndex = l * 4 * 3 + faceCoordLookup[currentFace];
					float centerCoord = sourceMeshXYZ[centerIndex];
					if (currentFace == 1 || currentFace == 2 || currentFace == 4)
					{
						centerCoord = 1f - centerCoord;
					}
					if (centerCoord <= 0.01f)
					{
						centerCoord = sourceMeshXYZ[centerIndex + 6];
						if (currentFace == 1 || currentFace == 2 || currentFace == 4)
						{
							centerCoord = 1f - centerCoord;
						}
						if (centerCoord <= 0.01f)
						{
							bool withinBlockBounds = true;
							for (int m = 0; m < 9; m++)
							{
								if (m == 3)
								{
									m += 3;
								}
								int ix = l * 4 * 3 + m;
								if (ix != centerIndex && ix != centerIndex + 6)
								{
									centerCoord = sourceMeshXYZ[ix];
									if (centerCoord < -0.0001f || centerCoord > 1.0001f)
									{
										withinBlockBounds = false;
										break;
									}
								}
							}
							if (withinBlockBounds)
							{
								continue;
							}
						}
					}
				}
			}
			bool isLiquid = renderpass == 4;
			bool istopsoil = renderpass == 5;
			bool useCustomTexture = false;
			CustomTextureResult customTexture = default(CustomTextureResult);
			if (hooks != null)
			{
				useCustomTexture = hooks.TryGetCustomTexture(vars, localFace, currentFace, tfMatrix, sourceMesh, l, shapeRotationSteps, out customTexture);
			}
			int effectiveAtlasNum = (useCustomTexture ? customTexture.AtlasNumber : (-1));
			if (prevRenderPass != renderpass || prevTextureId != textureid || (effectiveAtlasNum >= 0 && targetMesh != null))
			{
				if (effectiveAtlasNum >= 0)
				{
					MeshData[] meshPools = vars.tct.GetPoolForPass((renderpass >= 0) ? ((EnumChunkRenderPass)renderpass) : vars.RenderPass, lodlevel);
					if (effectiveAtlasNum < meshPools.Length)
					{
						targetMesh = meshPools[effectiveAtlasNum];
					}
					else
					{
						useCustomTexture = false;
						targetMesh = poolSupplier.GetMeshPoolForPass(textureid, (renderpass >= 0) ? ((EnumChunkRenderPass)renderpass) : vars.RenderPass, lodlevel);
					}
				}
				else
				{
					targetMesh = poolSupplier.GetMeshPoolForPass(textureid, (renderpass >= 0) ? ((EnumChunkRenderPass)renderpass) : vars.RenderPass, lodlevel);
				}
				if (targetMesh == null)
				{
					vars.tct.game.Logger.VerboseDebug("textureid " + textureid + " did not yet match an atlas pool: known atlasses count " + vars.tct.quantityAtlasses);
					throw new RetryTesselationException("textureid did not yet match an atlas pool");
				}
				targetMeshXYZ = targetMesh.xyz;
				targetMeshUV = targetMesh.Uv;
				targetMeshRgba = targetMesh.Rgba;
				targetMeshCustomInts = targetMesh.CustomInts;
				targetMeshFlags = targetMesh.Flags;
			}
			prevRenderPass = renderpass;
			prevTextureId = textureid;
			int colorMapDataCalculated = ColorMapData.FromValues(sourceMesh.SeasonColorMapIds[l], sourceMesh.ClimateColorMapIds[l], temperature, rainfall, (sourceMesh.FrostableBits != null) ? sourceMesh.FrostableBits[l] : frostable, vars.block.ExtraColorBits);
			if (vars.IsDecorOnJson)
			{
				if (SkipDecorFace(vars, l, sourceMeshXYZ))
				{
					continue;
				}
				colorMapDataCalculated = vars.OverlayColorMapData;
			}
			int[] axesByFacing = null;
			float textureVOffset = 0f;
			int[] jsonLightRGB = this.jsonLightRGB;
			if (currentFace < 0)
			{
				int num = jsonLightRGB[24];
				colorLightA = (byte)((float)(num & 0xFF) * aoBright);
				colorLightB = (byte)(num >> 8);
				colorLightG = (byte)(num >> 16);
				colorLightR = (byte)(num >> 24);
				aoBright = 1f;
			}
			else
			{
				axesByFacing = axesByFacingLookup[currentFace];
				int[] indexes = indexesByFacingLookup[currentFace];
				int baseIndex = currentFace * 4;
				colorTopLeft = jsonLightRGB[baseIndex + indexes[0]];
				colorTopRight = jsonLightRGB[baseIndex + indexes[1]];
				colorBottomLeft = jsonLightRGB[baseIndex + indexes[2]];
				colorBottomRight = jsonLightRGB[baseIndex + indexes[3]];
				if (vars.textureVOffset != 0f && ((1 << currentFace) & vars.block.alternatingVOffsetFaces) == 0)
				{
					textureVOffset = vars.textureVOffset / ((float)ClientSettings.MaxTextureAtlasHeight / 32f);
				}
			}
			int sourceVertexNum = l * vertPerFace;
			int targetVertexNum = targetMesh.VerticesCount;
			int lastelement = targetVertexNum - l * vertPerFace;
			int targetVertexIndex = targetVertexNum * 3;
			int sourceVertexIndex = sourceVertexNum * 3;
			int targetUVIndex = targetVertexNum * 2;
			int sourceUVIndex = sourceVertexNum * 2;
			int targetRGBAIndex = targetVertexNum * 4;
			float currentYOff = yOff;
			if (vars.IsDecorOnJson && !CalcAutoUVsForDecor(vars, sourceMesh, sourceVertexIndex, tfMatrix, ref currentXOff, ref currentYOff, ref currentZOff))
			{
				continue;
			}
			int k2 = vertPerFace;
			do
			{
				if (targetVertexNum >= targetMesh.VerticesMax)
				{
					targetMesh.VerticesCount = targetVertexNum;
					targetMesh.GrowVertexBuffer();
					targetMesh.GrowNormalsBuffer();
					targetMeshXYZ = targetMesh.xyz;
					targetMeshUV = targetMesh.Uv;
					targetMeshRgba = targetMesh.Rgba;
					targetMeshCustomInts = targetMesh.CustomInts;
					targetMeshFlags = targetMesh.Flags;
				}
				vars.UpdateChunkMinMax(targetMeshXYZ[targetVertexIndex++] = (tmpCoords[0] = sourceMeshXYZ[sourceVertexIndex++]) + currentXOff, targetMeshXYZ[targetVertexIndex++] = (tmpCoords[1] = sourceMeshXYZ[sourceVertexIndex++]) + currentYOff, targetMeshXYZ[targetVertexIndex++] = (tmpCoords[2] = sourceMeshXYZ[sourceVertexIndex++]) + currentZOff);
				if (useCustomTexture)
				{
					customTexture.RemapUV(sourceMeshUV[sourceUVIndex++], sourceMeshUV[sourceUVIndex++], out targetMeshUV[targetUVIndex], out targetMeshUV[targetUVIndex + 1]);
					targetUVIndex += 2;
				}
				else if (vars.IsDecorOnJson)
				{
					ApplyDecorUVs(targetMeshUV, targetUVIndex, k2, ref currentXOff, ref currentYOff, ref currentZOff);
					targetUVIndex += 2;
				}
				else
				{
					targetMeshUV[targetUVIndex++] = sourceMeshUV[sourceUVIndex++];
					targetMeshUV[targetUVIndex++] = sourceMeshUV[sourceUVIndex++] + textureVOffset;
				}
				float f = 1f;
				if (vars.block.DrawType == EnumDrawType.JSONAndWater && tmpCoords[1] < 1f)
				{
					f = Math.Max(0.6f, tmpCoords[1]) - 0.1f;
				}
				if (currentFace >= 0)
				{
					float lx = GameMath.Clamp(tmpCoords[axesByFacing[0]], 0f, 1f);
					float dy = GameMath.Clamp(tmpCoords[axesByFacing[1]], 0f, 1f);
					int num2 = GameMath.BiLerpRgbaColor(lx, dy, colorTopLeft, colorTopRight, colorBottomLeft, colorBottomRight);
					colorLightA = (byte)(num2 & 0xFF);
					colorLightB = (byte)(num2 >> 8);
					colorLightG = (byte)(num2 >> 16);
					colorLightR = (byte)((float)(int)(byte)(num2 >> 24) * aoBright);
				}
				if (isLiquid)
				{
					if (sourceMesh.CustomFloats == null)
					{
						targetMeshCustomInts.Add(colorMapDataCalculated);
						targetMeshCustomInts.Add(268435456);
						targetMesh.CustomFloats.Add(0f);
						targetMesh.CustomFloats.Add(0f);
					}
					else
					{
						targetMeshCustomInts.Add(colorMapDataCalculated);
						targetMeshCustomInts.Add(sourceMesh.CustomInts.Values[sourceVertexNum]);
						targetMesh.CustomFloats.Add(sourceMesh.CustomFloats.Values[2 * sourceVertexNum]);
						targetMesh.CustomFloats.Add(sourceMesh.CustomFloats.Values[2 * sourceVertexNum + 1]);
					}
				}
				else
				{
					targetMeshCustomInts.Add(colorMapDataCalculated);
					if (istopsoil)
					{
						targetMesh.CustomShorts.Add(sourceMesh.CustomShorts.Values[2 * sourceVertexNum]);
						targetMesh.CustomShorts.Add(sourceMesh.CustomShorts.Values[2 * sourceVertexNum + 1]);
					}
				}
				targetMeshRgba[targetRGBAIndex++] = (byte)((float)(int)colorLightA * f);
				targetMeshRgba[targetRGBAIndex++] = (byte)((float)(int)colorLightB * f);
				targetMeshRgba[targetRGBAIndex++] = (byte)((float)(int)colorLightG * f);
				targetMeshRgba[targetRGBAIndex++] = (byte)((float)(int)colorLightR * f);
				targetMeshFlags[targetVertexNum++] = (baseRenderFlags | sourceMeshFlags[sourceVertexNum] | windFlagsSet[sourceVertexNum]) & windFlagsMask[sourceVertexNum];
				sourceVertexNum++;
			}
			while (--k2 > 0);
			targetMesh.VerticesCount = targetVertexNum;
			if (indPerFace == 6)
			{
				int indexNum = l * indPerFace;
				targetMesh.AddIndices(allowSSBOs, lastelement + sourceMeshIndices[indexNum++], lastelement + sourceMeshIndices[indexNum++], lastelement + sourceMeshIndices[indexNum++], lastelement + sourceMeshIndices[indexNum++], lastelement + sourceMeshIndices[indexNum++], lastelement + sourceMeshIndices[indexNum]);
				continue;
			}
			int indexNum2 = l * indPerFace;
			for (int n = 0; n < indPerFace; n++)
			{
				targetMesh.AddIndex(lastelement + sourceMeshIndices[indexNum2++]);
			}
		}
	}

	private void AdjustWindWaveForFluids(MeshData sourceMesh, Block fluidBlock)
	{
		int clearFlags = -503316481;
		int verticesCount = sourceMesh.VerticesCount;
		int[] windFlagsMask = this.windFlagsMask;
		if (fluidBlock.SideSolid.Any)
		{
			for (int vertexNum = 0; vertexNum < verticesCount; vertexNum++)
			{
				windFlagsMask[vertexNum] = clearFlags;
			}
			return;
		}
		int[] windFlagsSet = this.windFlagsSet;
		int[] sourceMeshFlags = sourceMesh.Flags;
		for (int i = 0; i < verticesCount; i++)
		{
			int flags = sourceMeshFlags[i] & 0x1E000000;
			if (flags == 33554432 || flags == 100663296)
			{
				windFlagsSet[i] = 738197504;
				windFlagsMask[i] = -1073741825;
			}
		}
	}

	private static float[] NewFloatArray(int size)
	{
		if (reusableFloatArray == null || reusableFloatArray.Length < size)
		{
			reusableFloatArray = new float[size];
		}
		return reusableFloatArray;
	}

	private static int[] NewIntArray(int size)
	{
		if (reusableIntArray == null || reusableIntArray.Length < size)
		{
			reusableIntArray = new int[size];
		}
		return reusableIntArray;
	}
}
