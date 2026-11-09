using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.Client.NoObf;

public class SystemRenderSunMoon : ClientSystem
{
	private const float maxMoonProximityFactor = 0.2f;

	private MeshRef quadModel;

	private int suntextureId;

	private int[] moontextureIds;

	internal int ImageSize;

	public Matrixf ModelMat = new Matrixf();

	private int occlQueryId;

	private bool nowQuerying;

	private Matrix4 sunmat;

	private Matrix4 moonmat;

	private float targetSunSpec;

	private bool firstTickDone;

	public float sunScale = 0.04f;

	public float moonScale = 0.023100002f;

	private static Vec3f YAxis = new Vec3f(0f, 1f, 0f);

	public override string Name => "resm";

	public SystemRenderSunMoon(ClientMain game)
		: base(game)
	{
		suntextureId = -1;
		moontextureIds = new int[8];
		moontextureIds.Fill(-1);
		ImageSize = 256;
		MeshData meshdata = QuadMeshUtilExt.GetCustomQuadModelData(0f, 0f, 0f, ImageSize, ImageSize, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		meshdata.Flags = new int[4];
		quadModel = game.Platform.UploadMesh(meshdata);
		game.eventManager.RegisterRenderer(OnRenderFrame3D, EnumRenderStage.Opaque, Name, 0.3);
		game.eventManager.RegisterRenderer(OnRenderFrame3DPost, EnumRenderStage.Opaque, Name, 999.0);
		GL.GenQueries(1, ref occlQueryId);
	}

	private void OnRenderFrame3DPost(float obj)
	{
		ClientPlatformAbstract platform = game.Platform;
		platform.GlEnableDepthTest();
		platform.GlToggleBlend(on: true);
		platform.GlDisableCullFace();
		platform.GlDepthMask(flag: false);
		GL.ColorMask(false, false, false, false);
		Vec3f sunPos = game.Calendar.SunPosition;
		Quaternion quat = CreateLookRotation(new Vector3(sunPos.X, sunPos.Y, sunPos.Z));
		sunmat = Matrix4.CreateTranslation(-ImageSize / 2, -ImageSize, -ImageSize / 2) * Matrix4.CreateScale(sunScale, sunScale * 7f, sunScale) * Matrix4.CreateFromQuaternion(quat) * Matrix4.CreateTranslation(new Vector3(sunPos.X, sunPos.Y, sunPos.Z));
		ShaderProgramStandard prog = ShaderPrograms.Standard;
		prog.Use();
		prog.RgbaTint = ColorUtil.WhiteArgbVec;
		prog.RgbaAmbientIn = ColorUtil.WhiteRgbVec;
		prog.RgbaLightIn = new Vec4f(0f, 0f, 0f, (float)Math.Sin((double)game.ElapsedMilliseconds / 1000.0) / 2f + 0.5f);
		prog.RgbaFogIn = game.AmbientManager.BlendedFogColor;
		prog.ExtraGlow = 0;
		prog.FogMinIn = game.AmbientManager.BlendedFogMin;
		prog.FogDensityIn = game.AmbientManager.BlendedFogDensity;
		prog.DontWarpVertices = 0;
		prog.AddRenderFlags = 0;
		prog.ExtraZOffset = 0f;
		prog.NormalShaded = 0;
		prog.OverlayOpacity = 0f;
		prog.ExtraGodray = 1f / 3f;
		prog.UniformMatrix("modelMatrix", ref sunmat);
		prog.ViewMatrix = game.api.renderapi.CameraMatrixOriginf;
		prog.ProjectionMatrix = game.api.renderapi.CurrentProjectionMatrix;
		prog.Tex2D = suntextureId;
		if (firstTickDone)
		{
			int resultReady = default(int);
			GL.GetQueryObject(occlQueryId, (GetQueryObjectParam)34919, ref resultReady);
			if (resultReady > 0)
			{
				int samplesRendered = default(int);
				GL.GetQueryObject(occlQueryId, (GetQueryObjectParam)34918, ref samplesRendered);
				targetSunSpec = GameMath.Clamp((float)samplesRendered / 1500f, 0f, 1f);
				nowQuerying = false;
			}
		}
		firstTickDone = true;
		bool didBeginQuery = false;
		if (!nowQuerying)
		{
			GL.BeginQuery((QueryTarget)35092, occlQueryId);
			nowQuerying = true;
			didBeginQuery = true;
		}
		platform.RenderMesh(quadModel);
		prog.Stop();
		if (didBeginQuery)
		{
			GL.EndQuery((QueryTarget)35092);
		}
		platform.GlDepthMask(flag: true);
		GL.ColorMask(true, true, true, true);
	}

	public void OnRenderFrame3D(float dt)
	{
		ClientPlatformAbstract platform = game.Platform;
		if (suntextureId == -1)
		{
			suntextureId = game.GetOrLoadCachedTexture(new AssetLocation("environment/sun.png"));
			for (int i = 0; i < 8; i++)
			{
				moontextureIds[i] = game.GetOrLoadCachedTexture(new AssetLocation("environment/moon/" + i + ".png"));
			}
		}
		Vec3f moonPos = game.Calendar.MoonPosition;
		Vec3f sunPosRel = game.GameWorldCalendar.SunPositionNormalized;
		game.shUniforms.SunPosition3D = sunPosRel;
		Vec3f moonPosRel = moonPos.Clone().Normalize();
		float moonb = game.GameWorldCalendar.MoonLightStrength;
		float sunb = game.GameWorldCalendar.SunLightStrength;
		float t = GameMath.Clamp(50f * (moonb - sunb), 0f, 1f);
		game.shUniforms.LightPosition3D.Set(GameMath.Lerp(sunPosRel.X, moonPosRel.X, t), GameMath.Lerp(sunPosRel.Y, moonPosRel.Y, t), GameMath.Lerp(sunPosRel.Z, moonPosRel.Z, t));
		if (sunPosRel.Y < -0.05f)
		{
			double[] projMat = game.PMatrix.Top;
			double[] viewMat = game.api.renderapi.CameraMatrixOrigin;
			double[] modelViewMat = new double[16];
			Mat4d.Mul(modelViewMat, viewMat, ModelMat.ValuesAsDouble);
			Vec3d screenPos = MatrixToolsd.Project(new Vec3d((float)ImageSize / 4f, (float)(-ImageSize) / 4f, 0.0), projMat, modelViewMat, game.Width, game.Height);
			Vec3f centeredRelPos = new Vec3f((float)screenPos.X / (float)game.Width * 2f - 1f, (float)screenPos.Y / (float)game.Height * 2f - 1f, (float)screenPos.Z);
			game.shUniforms.SunPositionScreen = centeredRelPos;
		}
		platform.GlToggleBlend(on: true);
		platform.GlDisableCullFace();
		platform.GlDisableDepthTest();
		prepareSunMat();
		Vec3f suncol = game.Calendar.SunColor.Clone();
		float f = (suncol.R + suncol.G + suncol.B) / 3f;
		float colorLoss = GameMath.Clamp(GameMath.Max(game.AmbientManager.BlendedFlatFogDensity * 40f, game.AmbientManager.BlendedCloudDensity * game.AmbientManager.BlendedCloudDensity), 0f, 1f);
		suncol.R = colorLoss * f + (1f - colorLoss) * suncol.R;
		suncol.G = colorLoss * f + (1f - colorLoss) * suncol.G;
		suncol.B = colorLoss * f + (1f - colorLoss) * suncol.B;
		ShaderProgramStandard prog = ShaderPrograms.Standard;
		prog.Use();
		prog.Uniform("skyShaded", 1);
		Vec4f rgbatint = new Vec4f(1f, 1f, 1f, 1f);
		DefaultShaderUniforms shu = game.api.renderapi.ShaderUniforms;
		if (shu.FogSphereQuantity > 0)
		{
			for (int j = 0; j < shu.FogSphereQuantity; j++)
			{
				float num = shu.FogSpheres[j * 8];
				float y = shu.FogSpheres[j * 8 + 1];
				float z = shu.FogSpheres[j * 8 + 2];
				float rad = shu.FogSpheres[j * 8 + 3];
				float dense = shu.FogSpheres[j * 8 + 4];
				double d = Math.Sqrt(num * num + y * y + z * z);
				double fogAmount = (1.0 - d / (double)rad) * (double)rad * (double)dense;
				rgbatint.A = (float)GameMath.Clamp((double)rgbatint.A - fogAmount, 0.0, 1.0);
			}
		}
		prog.FadeFromSpheresFog = 1;
		prog.RgbaTint = rgbatint;
		prog.RgbaAmbientIn = ColorUtil.WhiteRgbVec;
		prog.RgbaLightIn = new Vec4f(suncol.R, suncol.G, suncol.B, 1f);
		prog.RgbaFogIn = game.AmbientManager.BlendedFogColor;
		prog.ExtraGlow = 0;
		prog.FogMinIn = game.AmbientManager.BlendedFogMin;
		prog.FogDensityIn = game.AmbientManager.BlendedFogDensity;
		prog.DontWarpVertices = 0;
		prog.AddRenderFlags = 0;
		prog.ExtraZOffset = 0f;
		prog.NormalShaded = 0;
		prog.OverlayOpacity = 0f;
		prog.ExtraGodray = 1f / 3f;
		prog.ShadowIntensity = 0f;
		prog.ApplySsao = 0;
		prog.AlphaTest = 0.01f;
		prog.UniformMatrix("modelMatrix", ref sunmat);
		prog.ViewMatrix = game.api.renderapi.CameraMatrixOriginf;
		prog.ProjectionMatrix = game.api.renderapi.CurrentProjectionMatrix;
		prog.Tex2D = suntextureId;
		platform.RenderMesh(quadModel);
		prog.Uniform("skyShaded", 0);
		prog.ExtraGodray = 0f;
		prog.ApplySsao = 1;
		prog.FadeFromSpheresFog = 0;
		prog.Stop();
		if (sunPosRel.Y >= -0.05f)
		{
			double[] projMat2 = game.PMatrix.Top;
			double[] viewMat2 = game.api.renderapi.CameraMatrixOrigin;
			double[] modelViewMat2 = new double[16];
			Mat4d.Mul(modelViewMat2, viewMat2, new double[16]
			{
				sunmat.M11, sunmat.M12, sunmat.M13, sunmat.M14, sunmat.M21, sunmat.M22, sunmat.M23, sunmat.M24, sunmat.M31, sunmat.M32,
				sunmat.M33, sunmat.M34, sunmat.M41, sunmat.M42, sunmat.M43, sunmat.M44
			});
			Vec3d screenPos2 = MatrixToolsd.Project(new Vec3d((float)ImageSize / 2f, (float)ImageSize / 2f, 0.0), projMat2, modelViewMat2, game.Width, game.Height);
			Vec3f centeredRelPos2 = new Vec3f((float)screenPos2.X / (float)game.Width * 2f - 1f, (float)screenPos2.Y / (float)game.Height * 2f - 1f, (float)screenPos2.Z);
			game.shUniforms.SunPositionScreen = centeredRelPos2;
		}
		game.shUniforms.SunSpecularIntensity = GameMath.Clamp(game.shUniforms.SunSpecularIntensity + (targetSunSpec - game.shUniforms.SunSpecularIntensity) * dt * 20f, 0f, 1f);
		prepareMoonMat(moonPos, moonPosRel);
		float angle = getAngleSunFromMoon(sunPosRel.Clone().Sub(moonPosRel), moonPosRel);
		ShaderProgramCelestialobject celestialobject = ShaderPrograms.Celestialobject;
		celestialobject.Use();
		celestialobject.Sky2D = game.skyTextureId;
		celestialobject.Glow2D = game.skyGlowTextureId;
		celestialobject.SunPosition = sunPosRel;
		celestialobject.Uniform("moonPosition", moonPosRel);
		celestialobject.Uniform("moonSunAngle", angle);
		celestialobject.DayLight = game.shUniforms.SkyDaylight;
		celestialobject.WeirdMathToMakeMoonLookNicer = 1;
		celestialobject.DitherSeed = (game.frameSeed + 1) % Math.Max(1, game.Width * game.Height);
		celestialobject.HorizontalResolution = game.Width;
		celestialobject.PlayerToSealevelOffset = (float)game.EntityPlayer.Pos.Y - (float)game.SeaLevel;
		celestialobject.RgbaFogIn = game.AmbientManager.BlendedFogColor;
		celestialobject.FogMinIn = game.AmbientManager.BlendedFogMin;
		celestialobject.FogDensityIn = game.AmbientManager.BlendedFogDensity;
		celestialobject.HorizonFog = game.AmbientManager.BlendedCloudDensity;
		celestialobject.ExtraGlow = 0;
		celestialobject.ExtraGodray = 0.5f;
		celestialobject.UniformMatrix("modelMatrix", ref moonmat);
		celestialobject.ViewMatrix = game.api.renderapi.CameraMatrixOriginf;
		celestialobject.ProjectionMatrix = game.api.renderapi.CurrentProjectionMatrix;
		celestialobject.Tex2D = moontextureIds[4];
		platform.RenderMesh(quadModel);
		celestialobject.WeirdMathToMakeMoonLookNicer = 0;
		celestialobject.Stop();
		platform.GlToggleBlend(on: false);
		platform.GlEnableDepthTest();
	}

	public void prepareSunMat()
	{
		Vec3f sunPos = game.Calendar.SunPosition;
		float sunPosY = sunPos.Y + (float)game.EntityPlayer.LocalEyePos.Y - ((float)game.EntityPlayer.Pos.Y - (float)game.SeaLevel) / 10000f;
		Quaternion quat = CreateLookRotation(new Vector3(sunPos.X, sunPosY, sunPos.Z));
		sunmat = Matrix4.CreateTranslation(-ImageSize / 2, -ImageSize / 2, -ImageSize / 2) * Matrix4.CreateScale(sunScale) * Matrix4.CreateFromQuaternion(quat) * Matrix4.CreateTranslation(new Vector3(sunPos.X, sunPosY, sunPos.Z));
	}

	public void prepareMoonMat(Vec3f moonPos, Vec3f moonPosNormalised)
	{
		float proximityFactor = GameMath.Clamp((game.Calendar.SunPositionNormalized.Dot(moonPosNormalised) - 0.99f) * 40f, 0f, 0.2f);
		float moonPosY = moonPos.Y + (float)game.EntityPlayer.LocalEyePos.Y - ((float)game.EntityPlayer.Pos.Y - (float)game.SeaLevel) / 10000f;
		bool waning = (int)game.Calendar.MoonPhaseExact > 4;
		Quaternion quat = CreateLookRotationMoon(new Vector3(moonPos.X, moonPosY, moonPos.Z), game.Calendar.SunPositionNormalized.Clone().Sub(moonPosNormalised), moonPosNormalised, waning, proximityFactor);
		moonmat = Matrix4.CreateTranslation(-ImageSize / 2, -ImageSize / 2, -ImageSize / 2) * Matrix4.CreateScale(moonScale * (1.1f + proximityFactor)) * Matrix4.CreateFromQuaternion(quat) * Matrix4.CreateTranslation(new Vector3(moonPos.X, moonPosY, moonPos.Z));
	}

	public static Quaternion CreateLookRotation(Vector3 direction)
	{
		Vector3 forwardVecXz = new Vector3(direction.X, 0f, direction.Z).Normalized();
		double rotY = Math.Atan2(forwardVecXz.X, forwardVecXz.Z);
		Quaternion quaternion = Quaternion.FromAxisAngle(Vector3.UnitY, (float)rotY);
		Vector3 forwardVecZy = new Vector3(z: new Vector2(direction.X, direction.Z).Length, x: 0f, y: direction.Y).Normalized();
		Quaternion xQuat = Quaternion.FromAxisAngle(angle: (float)Math.Atan2(forwardVecZy.Y, forwardVecZy.Z), axis: -Vector3.UnitX);
		return quaternion * xQuat;
	}

	public static Quaternion CreateLookRotationMoon(Vector3 direction, Vec3f sunVecRel, Vec3f moonVec, bool flip, float proximityFactor)
	{
		Vector3 forwardVecXz = new Vector3(direction.X, 0f, direction.Z).Normalized();
		double rotY = Math.Atan2(forwardVecXz.X, forwardVecXz.Z);
		Quaternion quaternion = Quaternion.FromAxisAngle(Vector3.UnitY, (float)rotY);
		Vector3 forwardVecZy = new Vector3(z: new Vector2(direction.X, direction.Z).Length, x: 0f, y: direction.Y).Normalized();
		Quaternion xQuat = Quaternion.FromAxisAngle(angle: (float)Math.Atan2(forwardVecZy.Y, forwardVecZy.Z), axis: -Vector3.UnitX);
		return quaternion * xQuat;
	}

	private float getAngleSunFromMoon(Vec3f sunVecRel, Vec3f moonVec)
	{
		sunVecRel.Sub(moonVec.Clone().Mul(moonVec.Dot(sunVecRel)));
		Vec3f axis = moonVec.Cross(YAxis);
		float scale = sunVecRel.Length() * axis.Length();
		if (scale == 0f)
		{
			return 0f;
		}
		return (float)Math.Acos(sunVecRel.Dot(axis) / scale);
	}

	public override void Dispose(ClientMain game)
	{
		game.Platform.DeleteMesh(quadModel);
		game.Platform.GLDeleteTexture(suntextureId);
		for (int i = 0; i < moontextureIds.Length; i++)
		{
			game.Platform.GLDeleteTexture(moontextureIds[i]);
		}
		GL.DeleteQuery(occlQueryId);
	}

	public override EnumClientSystemType GetSystemType()
	{
		return EnumClientSystemType.Render;
	}
}
