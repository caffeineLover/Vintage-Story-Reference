using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

internal class SystemRenderNightSky : ClientSystem
{
	private MeshRef nightSkyBox;

	private int textureId;

	private int frameSeed;

	private float[] modelMatrix = Mat4f.Create();

	private BitmapRef[] bmps;

	public override string Name => "rens";

	public SystemRenderNightSky(ClientMain game)
		: base(game)
	{
		MeshData modeldata = CubeMeshUtil.GetCubeOnlyScaleXyz(75f, 75f, new Vec3f());
		modeldata.Uv = null;
		modeldata.Rgba = null;
		nightSkyBox = game.Platform.UploadMesh(modeldata);
		game.eventManager.RegisterRenderer(OnRenderFrame3D, EnumRenderStage.Opaque, Name, 0.1);
	}

	public override void OnBlockTexturesLoaded()
	{
		bmps = new BitmapRef[6];
		TyronThreadPool.QueueTask((Action)LoadBitMaps, "NightSky.LoadBitMaps");
	}

	private void LoadBitMaps()
	{
		bmps[0] = game.Platform.CreateBitmapFromPng(game.AssetManager.Get("textures/environment/stars-ft.png"));
		bmps[1] = game.Platform.CreateBitmapFromPng(game.AssetManager.Get("textures/environment/stars-bg.png"));
		bmps[2] = game.Platform.CreateBitmapFromPng(game.AssetManager.Get("textures/environment/stars-up.png"));
		bmps[3] = game.Platform.CreateBitmapFromPng(game.AssetManager.Get("textures/environment/stars-dn.png"));
		bmps[4] = game.Platform.CreateBitmapFromPng(game.AssetManager.Get("textures/environment/stars-lf.png"));
		bmps[5] = game.Platform.CreateBitmapFromPng(game.AssetManager.Get("textures/environment/stars-rt.png"));
		game.EnqueueGameLaunchTask(FinishBitMaps, "nightsky");
	}

	private void FinishBitMaps()
	{
		textureId = game.Platform.Load3DTextureCube(bmps);
		BitmapRef[] array = bmps;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Dispose();
		}
	}

	public void OnRenderFrame3D(float deltaTime)
	{
		float daylightAdjusted = 1.25f * GameMath.Max(game.GameWorldCalendar.DayLightStrength - game.GameWorldCalendar.MoonLightStrength / 2f, 0.05f);
		EntityPos pos = game.EntityPlayer.Pos;
		float space = (float)GameMath.Clamp((pos.Y - (double)game.SeaLevel - 1000.0) / 30000.0, 0.0, 1.0);
		daylightAdjusted = Math.Max(0f, daylightAdjusted * (1f - space));
		if (game.Width != 0 && !((double)daylightAdjusted > 0.99))
		{
			game.GlMatrixModeModelView();
			game.Platform.GlDisableCullFace();
			game.Platform.GlDisableDepthTest();
			ShaderProgramNightsky prog = ShaderPrograms.Nightsky;
			prog.Use();
			prog.CtexCube = textureId;
			prog.DayLight = daylightAdjusted;
			prog.RgbaFog = game.AmbientManager.BlendedFogColor;
			prog.HorizonFog = game.AmbientManager.BlendedCloudDensity;
			prog.PlayerToSealevelOffset = (float)pos.Y - (float)game.SeaLevel;
			prog.DitherSeed = (frameSeed = (frameSeed + 1) % (game.Width * game.Height));
			prog.HorizontalResolution = game.Width;
			prog.ProjectionMatrix = game.CurrentProjectionMatrix;
			prog.FogDensityIn = game.AmbientManager.BlendedFogDensity;
			prog.FogMinIn = game.AmbientManager.BlendedFogMin;
			double totalDays = game.GameWorldCalendar.TotalDays;
			float yearRel = game.GameWorldCalendar.YearRel;
			float siderealRotationAngle = (float)GameMath.Mod(totalDays - (double)yearRel, 1.0) * ((float)Math.PI * 2f);
			float latitude = (float)game.GameWorldCalendar.OnGetLatitude(pos.Z);
			float theta = (float)Math.Acos(GameMath.Sin(latitude * ((float)Math.PI / 2f)) * GameMath.Sin(0.40910518f) + GameMath.Cos(latitude * ((float)Math.PI / 2f)) * GameMath.Cos(0.40910518f));
			Mat4f.Identity(modelMatrix);
			Mat4f.Rotate(modelMatrix, modelMatrix, siderealRotationAngle, new float[3]
			{
				0f,
				0f - GameMath.Sin(theta),
				GameMath.Cos(theta)
			});
			prog.ModelMatrix = modelMatrix;
			game.GlPushMatrix();
			MatrixToolsd.MatFollowPlayer(game.MvMatrix.Top);
			prog.ViewMatrix = game.CurrentModelViewMatrix;
			game.Platform.RenderMesh(nightSkyBox);
			game.GlPopMatrix();
			prog.Stop();
			game.Platform.GlEnableDepthTest();
			game.Platform.UnBindTextureCubeMap();
		}
	}

	public override void Dispose(ClientMain game)
	{
		game.Platform.DeleteMesh(nightSkyBox);
		game.Platform.GLDeleteTexture(textureId);
	}

	public override EnumClientSystemType GetSystemType()
	{
		return EnumClientSystemType.Render;
	}
}
