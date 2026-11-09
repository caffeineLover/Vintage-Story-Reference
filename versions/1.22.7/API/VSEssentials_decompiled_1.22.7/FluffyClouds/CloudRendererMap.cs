using System;
using System.Threading;
using OpenTK.Graphics.OpenGL;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace FluffyClouds;

public class CloudRendererMap : CloudRendererBase, IRenderer, IDisposable
{
	private int TextureData1;

	private int TextureData2;

	private short[] TextureDataBuffer1;

	private short[] TextureDataBuffer2;

	private int Framebuffer;

	public int TextureMap;

	public int TextureCol;

	public Vec3f offset = new Vec3f();

	private MeshRef quad;

	private IShaderProgram prog;

	private int programId;

	private Matrixf matrix = new Matrixf();

	private float time;

	public CloudTilesState committedState = new CloudTilesState();

	public CloudTilesState mainThreadState = new CloudTilesState();

	public CloudTilesState offThreadState = new CloudTilesState();

	public CloudTile[] Tiles;

	private CloudTile[] tempTiles;

	private bool newStateRready;

	private object cloudStateLock = new object();

	internal float blendedCloudDensity;

	internal float blendedGlobalCloudBrightness;

	public int QuantityCloudTiles = 25;

	private long windChangeTimer;

	private float cloudSpeedX;

	private float cloudSpeedZ;

	private float targetCloudSpeedX;

	private float targetCloudSpeedZ;

	private Random rand;

	private bool renderCloudMap;

	public WeatherSystemClient weatherSys;

	private Thread cloudTileUpdThread;

	private bool isShuttingDown;

	private ICoreClientAPI capi;

	private ModSystem mod;

	private int cloudTileBlendSpeed = 32;

	private WeatherDataReaderPreLoad wreaderpreload;

	private bool isFirstTick = true;

	private bool requireTileRebuild;

	public bool instantTileBlend;

	private int accum = 20;

	public double RenderOrder => 0.3;

	public int RenderRange => 9999;

	public CloudRendererMap(ModSystem mod, ICoreClientAPI capi)
	{
		ArgumentNullException.ThrowIfNull(capi, "capi");
		WeatherSystemClient weatherSys = (WeatherSystemClient)capi.ModLoader.GetModSystem<WeatherSystemBase>();
		this.capi = capi;
		this.mod = mod;
		this.weatherSys = weatherSys;
		wreaderpreload = weatherSys.getWeatherDataReaderPreLoad();
		rand = new Random(capi.World.Seed);
		quad = capi.Render.UploadMesh(QuadMeshUtil.GetQuad());
		capi.Event.ReloadShader += LoadShader;
		LoadShader();
		double time = capi.World.Calendar.TotalHours * 60.0;
		windOffsetX += 2.0 * time;
		windOffsetZ += 0.10000000149011612 * time;
		mainThreadState.WindTileOffsetX += (int)(windOffsetX / (double)base.CloudTileSize);
		windOffsetX %= base.CloudTileSize;
		mainThreadState.WindTileOffsetZ += (int)(windOffsetZ / (double)base.CloudTileSize);
		windOffsetZ %= base.CloudTileSize;
		offThreadState.Set(mainThreadState);
		committedState.Set(mainThreadState);
		InitCloudTiles(8 * capi.World.Player.WorldData.DesiredViewDistance);
		capi.Settings.AddWatcher<int>("viewDistance", OnViewDistanceChanged);
		capi.Settings.AddWatcher("cloudRenderMode", delegate(int val)
		{
			renderCloudMap = val == 1;
		});
		renderCloudMap = capi.Settings.Int.Get("cloudRenderMode", 0) == 1;
		capi.Event.LeaveWorld += delegate
		{
			isShuttingDown = true;
		};
		cloudTileUpdThread = new Thread((ThreadStart)delegate
		{
			while (!isShuttingDown)
			{
				if (!newStateRready)
				{
					int num = (int)windOffsetX / base.CloudTileSize;
					int num2 = (int)windOffsetZ / base.CloudTileSize;
					int x = offThreadState.CenterTilePos.X;
					int z = offThreadState.CenterTilePos.Z;
					offThreadState.Set(mainThreadState);
					offThreadState.WindTileOffsetX += num;
					offThreadState.WindTileOffsetZ += num2;
					int num3 = num + x - offThreadState.CenterTilePos.X;
					int num4 = num2 + z - offThreadState.CenterTilePos.Z;
					if (num3 != 0 || num4 != 0)
					{
						MoveCloudTilesOffThread(num3, num4);
					}
					UpdateCloudTilesOffThread(instantTileBlend ? 32767 : cloudTileBlendSpeed);
					instantTileBlend = false;
					newStateRready = true;
				}
				Thread.Sleep(40);
			}
		});
		cloudTileUpdThread.IsBackground = true;
	}

	public bool LoadShader()
	{
		prog = capi.Shader.NewShaderProgram();
		prog.VertexShader = capi.Shader.NewShader(EnumShaderType.VertexShader);
		prog.FragmentShader = capi.Shader.NewShader(EnumShaderType.FragmentShader);
		prog.AssetDomain = mod.Mod.Info.ModID;
		capi.Shader.RegisterFileShaderProgram("cloudmap", prog);
		bool result = prog.Compile();
		programId = prog.ProgramId;
		return result;
	}

	private void FreeGlResources()
	{
		GL.DeleteTexture(TextureData1);
		GL.DeleteTexture(TextureData2);
		GL.DeleteTexture(TextureMap);
		GL.DeleteTexture(TextureCol);
		GL.DeleteFramebuffer(Framebuffer);
	}

	private void OnViewDistanceChanged(int newValue)
	{
		requireTileRebuild = true;
	}

	public void CloudTick(float deltaTime)
	{
		blendedCloudDensity = capi.Ambient.BlendedCloudDensity;
		blendedGlobalCloudBrightness = capi.Ambient.BlendedCloudBrightness;
		if (isFirstTick)
		{
			weatherSys.ProcessWeatherUpdates();
			UpdateCloudTilesOffThread(32767);
			cloudTileUpdThread.Start();
			isFirstTick = false;
		}
		deltaTime *= capi.World.Calendar.SpeedOfTime / 60f;
		deltaTime = Math.Min(deltaTime, 1f);
		if (deltaTime > 0f)
		{
			if (windChangeTimer - capi.ElapsedMilliseconds < 0)
			{
				windChangeTimer = capi.ElapsedMilliseconds + rand.Next(20000, 120000);
				targetCloudSpeedX = (float)rand.NextDouble() * 5f;
				targetCloudSpeedZ = (float)rand.NextDouble() * 0.5f;
			}
			float windspeedx = 3f * (float)weatherSys.WeatherDataAtPlayer.GetWindSpeed(capi.World.Player.Entity.Pos.Y);
			cloudSpeedX += (targetCloudSpeedX + windspeedx - cloudSpeedX) * deltaTime;
			cloudSpeedZ += (targetCloudSpeedZ - cloudSpeedZ) * deltaTime;
		}
		lock (cloudStateLock)
		{
			if (deltaTime > 0f)
			{
				windOffsetX += cloudSpeedX * deltaTime;
				windOffsetZ += cloudSpeedZ * deltaTime;
			}
			mainThreadState.CenterTilePos.X = (int)capi.World.Player.Entity.Pos.X / base.CloudTileSize;
			mainThreadState.CenterTilePos.Z = (int)capi.World.Player.Entity.Pos.Z / base.CloudTileSize;
		}
		if (newStateRready)
		{
			int dx = offThreadState.WindTileOffsetX - committedState.WindTileOffsetX;
			int dz = offThreadState.WindTileOffsetZ - committedState.WindTileOffsetZ;
			committedState.Set(offThreadState);
			mainThreadState.WindTileOffsetX = committedState.WindTileOffsetX;
			mainThreadState.WindTileOffsetZ = committedState.WindTileOffsetZ;
			windOffsetX -= dx * base.CloudTileSize;
			windOffsetZ -= dz * base.CloudTileSize;
			weatherSys.ProcessWeatherUpdates();
			if (requireTileRebuild)
			{
				InitCloudTiles(8 * capi.World.Player.WorldData.DesiredViewDistance);
				UpdateCloudTiles();
				requireTileRebuild = false;
				instantTileBlend = true;
			}
			WriteTexture();
			newStateRready = false;
		}
		capi.World.FrameProfiler.Mark("gt-clouds");
	}

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
	{
		if (renderCloudMap)
		{
			if (!capi.IsGamePaused)
			{
				CloudTick(deltaTime);
				time = (time + deltaTime) % 86400f;
			}
			offset.X = (float)((double)(committedState.CenterTilePos.X * base.CloudTileSize) - capi.World.Player.Entity.Pos.X + windOffsetX);
			offset.Y = (float)((double)(weatherSys.CloudLevelRel * (float)capi.World.BlockAccessor.MapSizeY) + 0.5 - capi.World.Player.Entity.CameraPos.Y);
			offset.Z = (float)((double)(committedState.CenterTilePos.Z * base.CloudTileSize) - capi.World.Player.Entity.Pos.Z + windOffsetZ);
			Vec2f offsetCentre = new Vec2f((float)(((double)committedState.CenterTilePos.X * (double)base.CloudTileSize - capi.World.DefaultSpawnPosition.X + windOffsetX) / (double)base.CloudTileSize), (float)(((double)committedState.CenterTilePos.Z * (double)base.CloudTileSize - capi.World.DefaultSpawnPosition.Z + windOffsetZ) / (double)base.CloudTileSize));
			matrix.Set(capi.Render.CameraMatrixOriginf);
			int fb = default(int);
			GL.GetInteger((GetPName)36006, ref fb);
			int[] vp = new int[4];
			GL.GetInteger((GetPName)2978, vp);
			prog.Use();
			prog.Uniform("dayLight", Math.Max(0f, capi.World.Calendar.DayLightStrength - capi.World.Calendar.MoonLightStrength * 0.95f));
			prog.Uniform("globalCloudBrightness", capi.Ambient.BlendedCloudBrightness);
			prog.Uniform("time", time);
			prog.Uniform("rgbaFogIn", capi.Ambient.BlendedFogColor);
			prog.Uniform("fogMinIn", capi.Ambient.BlendedFogMin);
			prog.Uniform("fogDensityIn", capi.Ambient.BlendedFogDensity);
			prog.Uniform("sunPosition", capi.World.Calendar.SunPositionNormalized);
			prog.Uniform("nightVisionStrength", capi.Render.ShaderUniforms.NightVisionStrength);
			prog.Uniform("alpha", GameMath.Clamp(1f - 1.5f * Math.Max(0f, capi.Render.ShaderUniforms.GlitchStrength - 0.1f), 0f, 1f));
			prog.Uniform("width", (float)CloudTileLength);
			prog.Uniform("mapOffset", offset);
			prog.Uniform("mapOffsetCentre", offsetCentre);
			prog.BindTexture2D("mapData1", TextureData1, 8);
			prog.BindTexture2D("mapData2", TextureData2, 9);
			prog.UniformMatrix("viewMatrix", matrix.Values);
			prog.Uniform("pointLightQuantity", capi.Render.ShaderUniforms.PointLightsCount);
			if (capi.Render.ShaderUniforms.PointLightsCount > 0)
			{
				GL.Uniform3(GL.GetUniformLocation(programId, "pointLights"), capi.Render.ShaderUniforms.PointLightsCount, capi.Render.ShaderUniforms.PointLights3);
				GL.Uniform3(GL.GetUniformLocation(programId, "pointLightColors"), capi.Render.ShaderUniforms.PointLightsCount, capi.Render.ShaderUniforms.PointLightColors3);
			}
			GL.BindFramebuffer((FramebufferTarget)36160, Framebuffer);
			GL.Viewport(0, 0, CloudTileLength, CloudTileLength);
			GL.Disable((EnableCap)3042);
			GL.Disable((EnableCap)2929);
			capi.Render.RenderMesh(quad);
			GL.Enable((EnableCap)2929);
			GL.Enable((EnableCap)3042);
			GL.BindFramebuffer((FramebufferTarget)36160, fb);
			GL.Viewport(vp[0], vp[1], vp[2], vp[3]);
			prog.Stop();
		}
	}

	private void WriteTexture()
	{
		for (int i = 0; i < CloudTileLength * CloudTileLength; i++)
		{
			CloudTile tile = Tiles[i];
			int half = CloudTileLength / 2;
			int j = (tile.GridZOffset + half) * CloudTileLength + tile.GridXOffset + half;
			TextureDataBuffer1[j * 4] = tile.ThinCloudMode;
			TextureDataBuffer1[j * 4 + 1] = tile.SelfThickness;
			TextureDataBuffer1[j * 4 + 2] = tile.CloudOpaqueness;
			TextureDataBuffer1[j * 4 + 3] = tile.Brightness;
			TextureDataBuffer2[j * 4] = tile.UndulatingCloudMode;
		}
		GL.BindTexture((TextureTarget)3553, TextureData1);
		GL.TexSubImage2D<short>((TextureTarget)3553, 0, 0, 0, CloudTileLength, CloudTileLength, (PixelFormat)6408, (PixelType)5122, TextureDataBuffer1);
		GL.BindTexture((TextureTarget)3553, TextureData2);
		GL.TexSubImage2D<short>((TextureTarget)3553, 0, 0, 0, CloudTileLength, CloudTileLength, (PixelFormat)6408, (PixelType)5122, TextureDataBuffer2);
	}

	private int makeTexture(int width, PixelInternalFormat internalFormat, PixelFormat format, PixelType type)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		int texture = GL.GenTexture();
		GL.BindTexture((TextureTarget)3553, texture);
		GL.TexImage2D((TextureTarget)3553, 0, internalFormat, width, width, 0, format, type, (IntPtr)0);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10241, 9728);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10240, 9728);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10242, 33071);
		GL.TexParameter((TextureTarget)3553, (TextureParameterName)10243, 33071);
		return texture;
	}

	public void InitCloudTiles(int viewDistance)
	{
		CloudTileLength = GameMath.Clamp(viewDistance / base.CloudTileSize, 20, 200);
		QuantityCloudTiles = CloudTileLength * CloudTileLength;
		Tiles = new CloudTile[QuantityCloudTiles];
		tempTiles = new CloudTile[QuantityCloudTiles];
		int seed = rand.Next();
		for (int x = 0; x < CloudTileLength; x++)
		{
			for (int z = 0; z < CloudTileLength; z++)
			{
				Tiles[x * CloudTileLength + z] = new CloudTile
				{
					GridXOffset = (short)(x - CloudTileLength / 2),
					GridZOffset = (short)(z - CloudTileLength / 2),
					brightnessRand = new LCGRandom(seed)
				};
			}
		}
		FreeGlResources();
		TextureData1 = makeTexture(CloudTileLength, (PixelInternalFormat)32859, (PixelFormat)6408, (PixelType)5122);
		TextureData2 = makeTexture(CloudTileLength, (PixelInternalFormat)32859, (PixelFormat)6408, (PixelType)5122);
		TextureDataBuffer1 = new short[CloudTileLength * CloudTileLength * 4];
		TextureDataBuffer2 = new short[CloudTileLength * CloudTileLength * 4];
		int fb = default(int);
		GL.GetInteger((GetPName)36006, ref fb);
		TextureMap = makeTexture(CloudTileLength, (PixelInternalFormat)34836, (PixelFormat)6408, (PixelType)5126);
		TextureCol = makeTexture(CloudTileLength, (PixelInternalFormat)34836, (PixelFormat)6408, (PixelType)5126);
		Framebuffer = GL.GenFramebuffer();
		GL.BindFramebuffer((FramebufferTarget)36160, Framebuffer);
		GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36064, (TextureTarget)3553, TextureMap, 0);
		GL.FramebufferTexture2D((FramebufferTarget)36160, (FramebufferAttachment)36065, (TextureTarget)3553, TextureCol, 0);
		DrawBuffersEnum[] buffers = (DrawBuffersEnum[])(object)new DrawBuffersEnum[2]
		{
			(DrawBuffersEnum)36064,
			(DrawBuffersEnum)36065
		};
		GL.DrawBuffers(buffers.Length, buffers);
		GL.BindTexture((TextureTarget)3553, 0);
		GL.BindFramebuffer((FramebufferTarget)36160, fb);
	}

	public void UpdateCloudTilesOffThread(int changeSpeed)
	{
		bool reloadRainNoiseValues = false;
		accum++;
		if (accum > 10)
		{
			accum = 0;
			reloadRainNoiseValues = true;
		}
		int cnt = CloudTileLength * CloudTileLength;
		int prevTopLeftRegX = -9999;
		int prevTopLeftRegZ = -9999;
		Vec3i tileOffset = new Vec3i(offThreadState.TileOffsetX - offThreadState.WindTileOffsetX, 0, offThreadState.TileOffsetZ - offThreadState.WindTileOffsetZ);
		Vec3i tileCenterPos = offThreadState.CenterTilePos;
		for (int i = 0; i < cnt; i++)
		{
			CloudTile cloudTile = Tiles[i];
			int tileXPos = tileCenterPos.X + cloudTile.GridXOffset;
			int tileZPos = tileCenterPos.Z + cloudTile.GridZOffset;
			cloudTile.brightnessRand.InitPositionSeed(tileXPos - offThreadState.WindTileOffsetX, tileZPos - offThreadState.WindTileOffsetZ);
			Vec3d cloudTilePos = new Vec3d(tileXPos * base.CloudTileSize, capi.World.SeaLevel, tileZPos * base.CloudTileSize);
			int regSize = capi.World.BlockAccessor.RegionSize;
			int topLeftRegX = (int)Math.Round(cloudTilePos.X / (double)regSize) - 1;
			int topLeftRegZ = (int)Math.Round(cloudTilePos.Z / (double)regSize) - 1;
			if (topLeftRegX != prevTopLeftRegX || topLeftRegZ != prevTopLeftRegZ)
			{
				prevTopLeftRegX = topLeftRegX;
				prevTopLeftRegZ = topLeftRegZ;
				wreaderpreload.LoadAdjacentSims(cloudTilePos);
				wreaderpreload.EnsureCloudTileCacheIsFresh(tileOffset);
			}
			if (reloadRainNoiseValues || !cloudTile.rainValuesSet)
			{
				wreaderpreload.LoadLerp(cloudTilePos);
				cloudTile.lerpRainCloudOverlay = wreaderpreload.lerpRainCloudOverlay;
				cloudTile.lerpRainOverlay = wreaderpreload.lerpRainOverlay;
				cloudTile.rainValuesSet = true;
			}
			else
			{
				wreaderpreload.LoadLerp(cloudTilePos, useArgValues: true, cloudTile.lerpRainCloudOverlay, cloudTile.lerpRainOverlay);
			}
			int cloudTileX = (int)cloudTilePos.X / base.CloudTileSize;
			int cloudTileZ = (int)cloudTilePos.Z / base.CloudTileSize;
			double density = GameMath.Clamp(wreaderpreload.GetBlendedCloudThicknessAt(cloudTileX, cloudTileZ), 0.0, 1.0);
			double bright = wreaderpreload.GetBlendedCloudBrightness(1f) * (double)(0.85f + cloudTile.brightnessRand.NextFloat() * 0.15f);
			cloudTile.TargetBrightnes = (short)(GameMath.Clamp(bright, 0.0, 1.0) * 32767.0);
			cloudTile.TargetThickness = (short)GameMath.Clamp(density * 32767.0, 0.0, 32767.0);
			cloudTile.TargetThinCloudMode = (short)GameMath.Clamp(wreaderpreload.GetBlendedThinCloudModeness() * 32767.0, 0.0, 32767.0);
			cloudTile.TargetCloudOpaquenes = (short)GameMath.Clamp(wreaderpreload.GetBlendedCloudOpaqueness() * 32767.0, 0.0, 32767.0);
			cloudTile.TargetUndulatingCloudMode = (short)GameMath.Clamp(wreaderpreload.GetBlendedUndulatingCloudModeness() * 32767.0, 0.0, 32767.0);
			cloudTile.Brightness = LerpTileValue(cloudTile.TargetBrightnes, cloudTile.Brightness, changeSpeed);
			cloudTile.SelfThickness = LerpTileValue(cloudTile.TargetThickness, cloudTile.SelfThickness, changeSpeed);
			cloudTile.ThinCloudMode = LerpTileValue(cloudTile.TargetThinCloudMode, cloudTile.ThinCloudMode, changeSpeed);
			cloudTile.CloudOpaqueness = LerpTileValue(cloudTile.TargetCloudOpaquenes, cloudTile.CloudOpaqueness, changeSpeed);
			cloudTile.UndulatingCloudMode = LerpTileValue(cloudTile.TargetUndulatingCloudMode, cloudTile.UndulatingCloudMode, changeSpeed);
		}
	}

	private short LerpTileValue(int target, int current, int changeSpeed)
	{
		float changeVal = GameMath.Clamp(target - current, -changeSpeed, changeSpeed);
		return (short)GameMath.Clamp((float)current + changeVal, 0f, 32767f);
	}

	public void MoveCloudTilesOffThread(int dx, int dz)
	{
		for (int x = 0; x < CloudTileLength; x++)
		{
			for (int z = 0; z < CloudTileLength; z++)
			{
				int newX = GameMath.Mod(x + dx, CloudTileLength);
				int newZ = GameMath.Mod(z + dz, CloudTileLength);
				CloudTile tile = Tiles[x * CloudTileLength + z];
				tile.GridXOffset = (short)(newX - CloudTileLength / 2);
				tile.GridZOffset = (short)(newZ - CloudTileLength / 2);
				tempTiles[newX * CloudTileLength + newZ] = tile;
			}
		}
		CloudTile[] flip = Tiles;
		Tiles = tempTiles;
		tempTiles = flip;
	}

	public void Dispose()
	{
		FreeGlResources();
		capi.Render.DeleteMesh(quad);
	}
}
