using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public class BleedingJsonTesselator : IBlockTesselator, IJsonTesselatorHooks
{
	private const float UpwardNormalThreshold = 0.5f;

	private readonly JsonTesselator jsonTesselator;

	private readonly BleedingJsonMeshHelper helper;

	private int currentShapeRotationSteps;

	private float[] floatpool = new float[3];

	public BleedingJsonTesselator(JsonTesselator jsonTesselator)
	{
		this.jsonTesselator = jsonTesselator;
		helper = new BleedingJsonMeshHelper
		{
			tess = this,
			jsonTess = jsonTesselator
		};
	}

	public void Tesselate(TCTCache vars)
	{
		helper.vars = vars;
		currentShapeRotationSteps = 0;
		BlockPos pos = new BlockPos(0);
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
			if (JsonTesselator.NotSurrounded(vars, vars.extIndex3d))
			{
				DoMeshWithBleeding(vars, vars.block.Lod0Mesh, 0);
			}
			else
			{
				doNotRenderAtLod2 = true;
			}
		}
		if (jsonTesselator.SetUpLightRGBs(vars) == 4934475)
		{
			DoMeshWithBleeding(vars, lod1Mesh, 0);
			return;
		}
		if (vars.block.Lod2Mesh == null)
		{
			DoMeshWithBleeding(vars, lod1Mesh, (!doNotRenderAtLod2) ? 1 : 2);
			return;
		}
		DoMeshWithBleeding(vars, lod1Mesh, 2);
		DoMeshWithBleeding(vars, vars.block.Lod2Mesh, 3);
	}

	private void DoMeshWithBleeding(TCTCache vars, MeshData sourceMesh, int lodLevel)
	{
		if (sourceMesh == null || sourceMesh.VerticesCount == 0)
		{
			return;
		}
		MeshData[] altModels = (((lodLevel + 1) / 2 == 1) ? vars.shapes.altblockModelDatasLod1[vars.blockId] : ((lodLevel == 0) ? vars.shapes.altblockModelDatasLod0[vars.blockId] : vars.shapes.altblockModelDatasLod2[vars.blockId]));
		currentShapeRotationSteps = 0;
		if (altModels != null)
		{
			int rnd = GameMath.MurmurHash3Mod(vars.posX, (vars.block.RandomizeAxes == EnumRandomizeAxes.XYZ) ? vars.posY : 0, vars.posZ, altModels.Length);
			sourceMesh = altModels[rnd];
			CompositeShape[] bakedAlternates = vars.block.Shape?.BakedAlternates;
			if (bakedAlternates != null && rnd < bakedAlternates.Length)
			{
				float rotateY = bakedAlternates[rnd].rotateY;
				currentShapeRotationSteps = ((int)Math.Round(rotateY / 90f) % 4 + 4) % 4;
			}
		}
		BlockPos pos = new BlockPos(0);
		pos.SetDimension(vars.dimension);
		pos.Set(vars.posX, vars.posY, vars.posZ);
		int[] lightRGBs = jsonTesselator.jsonLightRGB;
		vars.block.OnJsonTesselation(ref sourceMesh, ref lightRGBs, pos, vars.tct.currentChunkBlocksExt, vars.extIndex3d);
		float[] tfMatrix = null;
		if (vars.preRotationMatrix != null)
		{
			tfMatrix = vars.preRotationMatrix;
		}
		else if (vars.block.RandomizeRotations || vars.block.RandomSizeAdjust != 0f)
		{
			tfMatrix = (vars.block.RandomizeRotations ? TesselationMetaData.randomRotMatrices[GameMath.MurmurHash3Mod(vars.posX, (vars.block.RandomizeAxes == EnumRandomizeAxes.XYZ) ? vars.posY : 0, vars.posZ, TesselationMetaData.randomRotations.Length)] : JsonTesselator.reusableIdentityMatrix);
		}
		jsonTesselator.AddJsonModelDataToMesh(sourceMesh, lodLevel, vars, helper, tfMatrix, this, currentShapeRotationSteps);
	}

	public bool TryGetCustomTexture(TCTCache vars, int localFace, int worldFace, float[] tfMatrix, MeshData sourceMesh, int faceIndex, int shapeRotationSteps, out CustomTextureResult result)
	{
		result = default(CustomTextureResult);
		if (worldFace != BlockFacing.UP.Index)
		{
			return false;
		}
		if (localFace < 0 && sourceMesh.Flags != null)
		{
			int vertIndex = faceIndex * sourceMesh.VerticesPerFace;
			if (vertIndex < sourceMesh.FlagsCount)
			{
				VertexFlags.UnpackNormal(sourceMesh.Flags[vertIndex], floatpool);
				if (floatpool[1] <= 0.5f)
				{
					return false;
				}
				localFace = BlockFacing.UP.Index;
			}
		}
		if (localFace < 0 || localFace >= 6)
		{
			return false;
		}
		int effectiveRotation = shapeRotationSteps;
		if (effectiveRotation == 0 && tfMatrix != null && tfMatrix != JsonTesselator.reusableIdentityMatrix)
		{
			effectiveRotation = GetYAxisRotationSteps(tfMatrix);
		}
		int baseTextureSubId = BleedingTextureHelper.GetFaceTextureSubIdWithFallback(vars, localFace);
		BleedTextureResult bleedResult = BleedingTextureHelper.ComputeBleedForFace(vars, baseTextureSubId, worldFace, effectiveRotation);
		if (!bleedResult.HasBleed || bleedResult.BlendedTexPos == null)
		{
			return false;
		}
		int vertPerFace = sourceMesh.VerticesPerFace;
		int faceUVStart = faceIndex * vertPerFace * 2;
		float[] sourceMeshUV = sourceMesh.Uv;
		float minU = sourceMeshUV[faceUVStart];
		float maxU = minU;
		float minV = sourceMeshUV[faceUVStart + 1];
		float maxV = minV;
		for (int v = 1; v < vertPerFace; v++)
		{
			float u = sourceMeshUV[faceUVStart + v * 2];
			float vCoord = sourceMeshUV[faceUVStart + v * 2 + 1];
			if (u < minU)
			{
				minU = u;
			}
			if (u > maxU)
			{
				maxU = u;
			}
			if (vCoord < minV)
			{
				minV = vCoord;
			}
			if (vCoord > maxV)
			{
				maxV = vCoord;
			}
		}
		result = new CustomTextureResult
		{
			TexturePosition = bleedResult.BlendedTexPos,
			AtlasNumber = bleedResult.BlendedTexPos.atlasNumber,
			OrigMinU = minU,
			OrigMaxU = maxU,
			OrigMinV = minV,
			OrigMaxV = maxV
		};
		return true;
	}

	private static int GetYAxisRotationSteps(float[] tfMatrix)
	{
		if (tfMatrix == null)
		{
			return 0;
		}
		float cosTheta = tfMatrix[0];
		float sinTheta = tfMatrix[8];
		if (cosTheta > 0.5f)
		{
			return 0;
		}
		if (cosTheta < -0.5f)
		{
			return 2;
		}
		if (sinTheta > 0.5f)
		{
			return 1;
		}
		return 3;
	}
}
