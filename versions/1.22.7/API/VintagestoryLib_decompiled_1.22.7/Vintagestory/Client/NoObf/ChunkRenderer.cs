using System;
using OpenTK.Graphics.OpenGL;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public class ChunkRenderer
{
	protected MeshDataPoolMasterManager masterPool;

	protected ClientPlatformAbstract platform;

	protected ClientMain game;

	protected ChunkCuller culler;

	protected CustomMeshDataPartFloat twoCustomFloats;

	protected CustomMeshDataPartInt twoCustomInts;

	protected CustomMeshDataPartInt oneCustomInt;

	protected CustomMeshDataPartShort twoShortsNormalised;

	protected Vec2f blockTextureSize = new Vec2f();

	public int[] textureIds;

	public int QuantityRenderingChunks;

	public MeshDataPoolManager[][] poolsByRenderPass;

	private float subPixelPaddingX;

	private float subPixelPaddingY;

	private float curRainFall;

	private float lastSetRainFall;

	private float accum;

	public ChunkRenderer(int[] textureIds, ClientMain game)
	{
		this.textureIds = textureIds;
		platform = game.Platform;
		this.game = game;
		culler = new ChunkCuller(game);
		game.api.eventapi.ReloadShader += Eventapi_ReloadShader;
		twoCustomFloats = new CustomMeshDataPartFloat
		{
			InterleaveOffsets = new int[1],
			InterleaveSizes = new int[1] { 2 },
			InterleaveStride = 8
		};
		twoCustomInts = new CustomMeshDataPartInt
		{
			InterleaveOffsets = new int[2] { 0, 4 },
			InterleaveSizes = new int[2] { 1, 1 },
			InterleaveStride = 8,
			Conversion = DataConversion.Integer
		};
		oneCustomInt = new CustomMeshDataPartInt
		{
			InterleaveOffsets = new int[1],
			InterleaveSizes = new int[1] { 1 },
			InterleaveStride = 4,
			Conversion = DataConversion.Integer
		};
		twoShortsNormalised = new CustomMeshDataPartShort
		{
			InterleaveOffsets = new int[1],
			InterleaveSizes = new int[1] { 2 },
			InterleaveStride = 4,
			Conversion = DataConversion.NormalizedFloat
		};
		masterPool = new MeshDataPoolMasterManager(game.api);
		masterPool.DelayedPoolLocationRemoval = true;
		Array passes = Enum.GetValues(typeof(EnumChunkRenderPass));
		poolsByRenderPass = new MeshDataPoolManager[passes.Length][];
		int MaxVertices = ClientSettings.ModelDataPoolMaxVertexSize;
		int MaxIndices = ClientSettings.ModelDataPoolMaxIndexSize;
		int MaxPartsPerPool = ClientSettings.ModelDataPoolMaxParts * 2;
		foreach (EnumChunkRenderPass pass in passes)
		{
			poolsByRenderPass[(int)pass] = new MeshDataPoolManager[textureIds.Length + 3];
			for (int i = 0; i < textureIds.Length; i++)
			{
				AddPoolsForAtlasAndPass(i, pass, MaxVertices, MaxIndices, MaxPartsPerPool);
			}
		}
		blockTextureSize.X = (float)game.textureSize / (float)game.BlockAtlasManager.Size.Width;
		blockTextureSize.Y = (float)game.textureSize / (float)game.BlockAtlasManager.Size.Height;
	}

	internal void RuntimeAddBlockTextureAtlas(int[] textureIds)
	{
		Array values = Enum.GetValues(typeof(EnumChunkRenderPass));
		int MaxVertices = ClientSettings.ModelDataPoolMaxVertexSize;
		int MaxIndices = ClientSettings.ModelDataPoolMaxIndexSize;
		int MaxPartsPerPool = ClientSettings.ModelDataPoolMaxParts * 2;
		int index = textureIds.Length - 1;
		foreach (EnumChunkRenderPass pass in values)
		{
			AddPoolsForAtlasAndPass(index, pass, MaxVertices, MaxIndices, MaxPartsPerPool);
		}
		this.textureIds = textureIds;
	}

	private void AddPoolsForAtlasAndPass(int atlas, EnumChunkRenderPass pass, int maxVertices, int maxIndices, int maxPartsPerPool)
	{
		switch (pass)
		{
		case EnumChunkRenderPass.Liquid:
			poolsByRenderPass[(int)pass][atlas] = new MeshDataPoolManager(masterPool, game.frustumCuller, game.api, maxVertices, maxIndices, maxPartsPerPool, twoCustomFloats, null, null, twoCustomInts);
			break;
		case EnumChunkRenderPass.TopSoil:
			poolsByRenderPass[(int)pass][atlas] = new MeshDataPoolManager(masterPool, game.frustumCuller, game.api, maxVertices, maxIndices, maxPartsPerPool, null, twoShortsNormalised, null, oneCustomInt);
			break;
		default:
			poolsByRenderPass[(int)pass][atlas] = new MeshDataPoolManager(masterPool, game.frustumCuller, game.api, maxVertices, maxIndices, maxPartsPerPool, null, null, null, oneCustomInt);
			break;
		}
	}

	private bool Eventapi_ReloadShader()
	{
		lastSetRainFall = -1f;
		return true;
	}

	internal void SwapVisibleBuffers()
	{
		ClientChunk.bufIndex = (ClientChunk.bufIndex + 1) % 2;
		ModelDataPoolLocation.VisibleBufIndex = ClientChunk.bufIndex;
	}

	public void OnSeperateThreadTick(float dt)
	{
		culler.CullInvisibleChunks();
	}

	public void OnRenderBefore(float dt)
	{
		game.Platform.LoadFrameBuffer(EnumFrameBuffer.LiquidDepth);
		game.Platform.ClearFrameBuffer(EnumFrameBuffer.LiquidDepth);
		subPixelPaddingX = game.BlockAtlasManager.SubPixelPaddingX;
		subPixelPaddingY = game.BlockAtlasManager.SubPixelPaddingY;
		Vec3d playerPos = game.EntityPlayer.CameraPos;
		game.GlPushMatrix();
		game.GlLoadMatrix(game.MainCamera.CameraMatrixOrigin);
		ShaderProgramChunkliquiddepth progLide = ShaderPrograms.Chunkliquiddepth;
		progLide.Use();
		progLide.ViewDistance = ClientSettings.ViewDistance;
		progLide.ProjectionMatrix = game.CurrentProjectionMatrix;
		progLide.ModelViewMatrix = game.CurrentModelViewMatrix;
		bool tempSSBOs = game.api.renderapi.useSSBOs;
		game.api.renderapi.useSSBOs = false;
		for (int i = 0; i < textureIds.Length; i++)
		{
			poolsByRenderPass[4][i].Render(playerPos, "origin");
		}
		game.api.renderapi.useSSBOs = tempSSBOs;
		progLide.Stop();
		ScreenManager.FrameProfiler.Mark("rend3D-ret-lide");
		game.GlPopMatrix();
		game.Platform.UnloadFrameBuffer(EnumFrameBuffer.LiquidDepth);
		game.Platform.LoadFrameBuffer(EnumFrameBuffer.Primary);
	}

	public void OnBeforeRenderOpaque(float dt)
	{
		masterPool.OnFrame(dt, game.CurrentModelViewMatrix, game.shadowMvpMatrix);
		RuntimeStats.renderedTriangles = 0;
		RuntimeStats.availableTriangles = 0;
		accum += dt;
		if (accum > 5f)
		{
			accum = 0f;
			ClimateCondition conds = game.BlockAccessor.GetClimateAt(game.EntityPlayer.Pos.AsBlockPos);
			float raininess = GameMath.Clamp((conds.Temperature + 1f) / 4f, 0f, 1f);
			curRainFall = conds.Rainfall * raininess;
		}
	}

	public void RenderShadow(float dt)
	{
		ShaderProgramChunkshadowmap prog = ShaderPrograms.Chunkshadowmap;
		prog.Uniform("subpixelPaddingX", subPixelPaddingX);
		prog.Uniform("subpixelPaddingY", subPixelPaddingY);
		Vec3d playerPos = game.EntityPlayer.CameraPos;
		ScreenManager.FrameProfiler.Mark("rend3D-rets-begin");
		platform.GlDepthMask(flag: true);
		platform.GlToggleBlend(on: false);
		platform.GlEnableDepthTest();
		platform.GlDisableCullFace();
		EnumFrustumCullMode cullMode = ((game.currentRenderStage == EnumRenderStage.ShadowFar) ? EnumFrustumCullMode.CullInstantShadowPassFar : EnumFrustumCullMode.CullInstantShadowPassNear);
		for (int i = 0; i < textureIds.Length; i++)
		{
			prog.Tex2d2D = textureIds[i];
			poolsByRenderPass[0][i].Render(playerPos, "origin", cullMode);
		}
		ScreenManager.FrameProfiler.Mark("rend3D-rets-op");
		for (int j = 0; j < textureIds.Length; j++)
		{
			prog.Tex2d2D = textureIds[j];
			poolsByRenderPass[5][j].Render(playerPos, "origin", cullMode);
		}
		ScreenManager.FrameProfiler.Mark("rend3D-rets-tpp");
		platform.GlDisableCullFace();
		for (int k = 0; k < textureIds.Length; k++)
		{
			prog.Tex2d2D = textureIds[k];
			poolsByRenderPass[2][k].Render(playerPos, "origin", cullMode);
		}
		for (int l = 0; l < textureIds.Length; l++)
		{
			prog.Tex2d2D = textureIds[l];
			poolsByRenderPass[1][l].Render(playerPos, "origin", cullMode);
		}
		platform.GlToggleBlend(on: true);
	}

	public void RenderOpaque(float dt)
	{
		Vec3d playerCamPos = game.EntityPlayer.CameraPos;
		ScreenManager.FrameProfiler.Mark("rend3D-ret-begin");
		platform.GlDepthMask(flag: true);
		platform.GlEnableDepthTest();
		platform.GlToggleBlend(on: true);
		platform.GlEnableCullFace();
		game.GlMatrixModeModelView();
		game.GlPushMatrix();
		game.GlLoadMatrix(game.MainCamera.CameraMatrixOrigin);
		ShaderProgramChunkopaque progOp = ShaderPrograms.Chunkopaque;
		progOp.Use();
		progOp.CameraUnderwater = game.shUniforms.CameraUnderwater;
		progOp.RgbaFogIn = game.AmbientManager.BlendedFogColor;
		progOp.RgbaAmbientIn = game.AmbientManager.BlendedAmbientColor;
		progOp.FogDensityIn = game.AmbientManager.BlendedFogDensity;
		progOp.FogMinIn = game.AmbientManager.BlendedFogMin;
		progOp.ProjectionMatrix = game.CurrentProjectionMatrix;
		progOp.AlphaTest = 0.001f;
		progOp.HaxyFade = 0;
		progOp.LiquidDepth2D = game.Platform.FrameBuffers[5].DepthTextureId;
		progOp.ModelViewMatrix = game.CurrentModelViewMatrix;
		progOp.Uniform("subpixelPaddingX", subPixelPaddingX);
		progOp.Uniform("subpixelPaddingY", subPixelPaddingY);
		progOp.SunPosition = game.GameWorldCalendar.SunPositionNormalized;
		progOp.DayLight = game.shUniforms.SkyDaylight;
		progOp.HorizonFog = game.AmbientManager.BlendedCloudDensity;
		for (int i = 0; i < textureIds.Length; i++)
		{
			progOp.TerrainTex2D = textureIds[i];
			progOp.TerrainTexLinear2D = textureIds[i];
			poolsByRenderPass[0][i].Render(playerCamPos, "origin");
		}
		ScreenManager.FrameProfiler.Mark("rend3D-ret-op");
		progOp.Stop();
		ShaderProgramChunktopsoil progTs = ShaderPrograms.Chunktopsoil;
		progTs.Use();
		progTs.RgbaFogIn = game.AmbientManager.BlendedFogColor;
		progTs.RgbaAmbientIn = game.AmbientManager.BlendedAmbientColor;
		progTs.FogDensityIn = game.AmbientManager.BlendedFogDensity;
		progTs.FogMinIn = game.AmbientManager.BlendedFogMin;
		progTs.ProjectionMatrix = game.CurrentProjectionMatrix;
		progTs.ModelViewMatrix = game.CurrentModelViewMatrix;
		progTs.BlockTextureSize = blockTextureSize;
		progTs.Uniform("subpixelPaddingX", subPixelPaddingX);
		progTs.Uniform("subpixelPaddingY", subPixelPaddingY);
		for (int j = 0; j < textureIds.Length; j++)
		{
			progTs.TerrainTex2D = textureIds[j];
			progTs.TerrainTexLinear2D = textureIds[j];
			poolsByRenderPass[5][j].Render(playerCamPos, "origin");
		}
		ScreenManager.FrameProfiler.Mark("rend3D-ret-tpp");
		progTs.Stop();
		platform.GlDisableCullFace();
		progOp.Use();
		progOp.RgbaFogIn = game.AmbientManager.BlendedFogColor;
		progOp.RgbaAmbientIn = game.AmbientManager.BlendedAmbientColor;
		progOp.FogDensityIn = game.AmbientManager.BlendedFogDensity;
		progOp.FogMinIn = game.AmbientManager.BlendedFogMin;
		progOp.ProjectionMatrix = game.CurrentProjectionMatrix;
		progOp.ModelViewMatrix = game.CurrentModelViewMatrix;
		progOp.AlphaTest = 0.25f;
		progOp.HaxyFade = 0;
		platform.GlToggleBlend(on: true);
		for (int k = 0; k < textureIds.Length; k++)
		{
			progOp.TerrainTex2D = textureIds[k];
			progOp.TerrainTexLinear2D = textureIds[k];
			poolsByRenderPass[2][k].Render(playerCamPos, "origin");
		}
		platform.GlToggleBlend(on: false);
		progOp.AlphaTest = 0.42f;
		progOp.HaxyFade = 1;
		for (int l = 0; l < textureIds.Length; l++)
		{
			progOp.TerrainTex2D = textureIds[l];
			progOp.TerrainTexLinear2D = textureIds[l];
			poolsByRenderPass[1][l].Render(playerCamPos, "origin");
		}
		platform.GlEnableCullFace();
		platform.GlToggleBlend(on: true);
		progOp.AlphaTest = 0.2f;
		progOp.HaxyFade = 0;
		for (int m = 0; m < textureIds.Length; m++)
		{
			progOp.TerrainTex2D = textureIds[m];
			progOp.TerrainTexLinear2D = textureIds[m];
			poolsByRenderPass[8][m].Render(playerCamPos, "origin");
		}
		platform.GlToggleBlend(on: false);
		progOp.Stop();
		ScreenManager.FrameProfiler.Mark("rend3D-ret-opnc");
		game.GlPopMatrix();
		if (game.unbindSamplers)
		{
			GL.BindSampler(0, 0);
			GL.BindSampler(1, 0);
			GL.BindSampler(2, 0);
			GL.BindSampler(3, 0);
			GL.BindSampler(4, 0);
			GL.BindSampler(5, 0);
			GL.BindSampler(6, 0);
			GL.BindSampler(7, 0);
			GL.BindSampler(8, 0);
		}
	}

	internal void RenderOIT(float deltaTime)
	{
		Vec3d playerPos = game.EntityPlayer.CameraPos;
		game.GlPushMatrix();
		game.GlLoadMatrix(game.MainCamera.CameraMatrixOrigin);
		game.GlPushMatrix();
		ShaderProgramChunkliquid progLi = ShaderPrograms.Chunkliquid;
		progLi.Use();
		progLi.WaterStillCounter = game.shUniforms.WaterStillCounter;
		progLi.WaterFlowCounter = game.shUniforms.WaterFlowCounter;
		progLi.WindWaveIntensity = game.shUniforms.WindWaveIntensity;
		progLi.SunPosRel = game.shUniforms.SunPosition3D;
		progLi.SunColor = game.Calendar.SunColor;
		progLi.ReflectColor = game.Calendar.ReflectColor;
		progLi.PlayerPosForFoam = game.shUniforms.PlayerPosForFoam;
		progLi.CameraUnderwater = game.shUniforms.CameraUnderwater;
		progLi.Uniform("subpixelPaddingX", subPixelPaddingX);
		progLi.Uniform("subpixelPaddingY", subPixelPaddingY);
		if (Math.Abs(lastSetRainFall - curRainFall) > 0.05f || curRainFall == 0f)
		{
			progLi.DropletIntensity = (lastSetRainFall = curRainFall);
		}
		FrameBufferRef framebuffer = game.api.Render.FrameBuffers[0];
		progLi.RgbaFogIn = game.AmbientManager.BlendedFogColor;
		progLi.RgbaAmbientIn = game.AmbientManager.BlendedAmbientColor;
		progLi.FogDensityIn = game.AmbientManager.BlendedFogDensity;
		progLi.FogMinIn = game.AmbientManager.BlendedFogMin;
		progLi.BlockTextureSize = blockTextureSize;
		progLi.TextureAtlasSize = game.BlockAtlasManager.SizeAsVec2f;
		progLi.ToShadowMapSpaceMatrixFar = game.toShadowMapSpaceMatrixFar;
		progLi.ToShadowMapSpaceMatrixNear = game.toShadowMapSpaceMatrixNear;
		progLi.ProjectionMatrix = game.CurrentProjectionMatrix;
		progLi.ModelViewMatrix = game.CurrentModelViewMatrix;
		progLi.PlayerViewVec = game.shUniforms.PlayerViewVector;
		progLi.DepthTex2D = framebuffer.DepthTextureId;
		progLi.Uniform("frameSize", framebuffer.Width, framebuffer.Height);
		progLi.SunSpecularIntensity = game.shUniforms.SunSpecularIntensity;
		bool tempSSBOs = game.api.renderapi.useSSBOs;
		game.api.renderapi.useSSBOs = false;
		for (int i = 0; i < textureIds.Length; i++)
		{
			progLi.TerrainTex2D = textureIds[i];
			poolsByRenderPass[4][i].Render(playerPos, "origin");
		}
		game.api.renderapi.useSSBOs = tempSSBOs;
		progLi.Stop();
		ScreenManager.FrameProfiler.Mark("rend3D-ret-lp");
		game.GlPopMatrix();
		ShaderProgramChunktransparent progTp = ShaderPrograms.Chunktransparent;
		progTp.Use();
		progTp.RgbaFogIn = game.AmbientManager.BlendedFogColor;
		progTp.RgbaAmbientIn = game.AmbientManager.BlendedAmbientColor;
		progTp.FogDensityIn = game.AmbientManager.BlendedFogDensity;
		progTp.FogMinIn = game.AmbientManager.BlendedFogMin;
		progTp.ProjectionMatrix = game.CurrentProjectionMatrix;
		progTp.ModelViewMatrix = game.CurrentModelViewMatrix;
		progTp.Uniform("subpixelPaddingX", subPixelPaddingX);
		progTp.Uniform("subpixelPaddingY", subPixelPaddingY);
		for (int j = 0; j < textureIds.Length; j++)
		{
			progTp.TerrainTex2D = textureIds[j];
			poolsByRenderPass[3][j].Render(playerPos, "origin");
			if (ClientSettings.RenderMetaBlocks)
			{
				poolsByRenderPass[6][j].Render(playerPos, "origin");
			}
		}
		progTp.Stop();
		game.GlPopMatrix();
		ScreenManager.FrameProfiler.Mark("rend3D-ret-tp");
	}

	internal void RenderAfterOIT(float deltaTime)
	{
		game.GlPushMatrix();
		game.GlLoadMatrix(game.MainCamera.CameraMatrixOrigin);
		Vec3d playerCamPos = game.EntityPlayer.CameraPos;
		ShaderProgramChunkopaque progOp = ShaderPrograms.Chunkopaque;
		platform.GlDisableCullFace();
		platform.GlToggleBlend(on: false);
		platform.GlEnableDepthTest();
		progOp.Use();
		progOp.RgbaFogIn = game.AmbientManager.BlendedFogColor;
		progOp.RgbaAmbientIn = game.AmbientManager.BlendedAmbientColor;
		progOp.FogDensityIn = game.AmbientManager.BlendedFogDensity;
		progOp.FogMinIn = game.AmbientManager.BlendedFogMin;
		progOp.ProjectionMatrix = game.CurrentProjectionMatrix;
		progOp.ModelViewMatrix = game.CurrentModelViewMatrix;
		progOp.AlphaTest = 0.42f;
		progOp.SunPosition = game.GameWorldCalendar.SunPositionNormalized;
		progOp.DayLight = game.shUniforms.SkyDaylight;
		progOp.HorizonFog = game.AmbientManager.BlendedCloudDensity;
		progOp.HaxyFade = 1;
		progOp.Uniform("subpixelPaddingX", subPixelPaddingX);
		progOp.Uniform("subpixelPaddingY", subPixelPaddingY);
		for (int i = 0; i < textureIds.Length; i++)
		{
			progOp.TerrainTex2D = textureIds[i];
			progOp.TerrainTexLinear2D = textureIds[i];
			poolsByRenderPass[7][i].Render(playerCamPos, "origin");
		}
		progOp.Stop();
		game.GlPopMatrix();
	}

	public void Dispose()
	{
		masterPool.DisposeAllPools(game.api);
	}

	public void AddTesselatedChunk(TesselatedChunk tesschunk, ClientChunk hostChunk)
	{
		Vec3i chunkOrigin = new Vec3i(tesschunk.positionX, tesschunk.positionYAndDimension % 32768, tesschunk.positionZ);
		int dimension = tesschunk.positionYAndDimension / 32768;
		Sphere boundingSphere = tesschunk.boundingSphere;
		tesschunk.AddCenterToPools(this, chunkOrigin, dimension, boundingSphere, hostChunk);
		tesschunk.AddEdgeToPools(this, chunkOrigin, dimension, boundingSphere, hostChunk);
		tesschunk.centerParts = null;
		tesschunk.edgeParts = null;
		tesschunk.chunk = null;
	}

	public void RemoveDataPoolLocations(ModelDataPoolLocation[] locations)
	{
		masterPool.RemoveDataPoolLocations(locations);
	}

	public void GetStats(out long usedVideoMemory, out long renderedTris, out long allocatedTris)
	{
		usedVideoMemory = 0L;
		renderedTris = 0L;
		allocatedTris = 0L;
		foreach (EnumChunkRenderPass pass in Enum.GetValues(typeof(EnumChunkRenderPass)))
		{
			for (int i = 0; i < textureIds.Length; i++)
			{
				poolsByRenderPass[(int)pass][i].GetStats(ref usedVideoMemory, ref renderedTris, ref allocatedTris);
			}
		}
	}

	public float CalcFragmentation()
	{
		return masterPool.CalcFragmentation();
	}

	public int QuantityModelDataPools()
	{
		return masterPool.QuantityModelDataPools();
	}

	internal void SetInterleaveStrides(MeshData modelDataLod0, EnumChunkRenderPass pass)
	{
		if (pass == EnumChunkRenderPass.Liquid)
		{
			modelDataLod0.CustomFloats.InterleaveStride = twoCustomFloats.InterleaveStride;
			modelDataLod0.CustomInts.InterleaveStride = twoCustomInts.InterleaveStride;
			return;
		}
		modelDataLod0.CustomInts.InterleaveStride = oneCustomInt.InterleaveStride;
		if (pass == EnumChunkRenderPass.TopSoil)
		{
			modelDataLod0.CustomShorts.InterleaveStride = twoShortsNormalised.InterleaveStride;
		}
	}
}
