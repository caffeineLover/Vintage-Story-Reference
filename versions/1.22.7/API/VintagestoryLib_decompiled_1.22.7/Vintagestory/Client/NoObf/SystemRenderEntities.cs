using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public class SystemRenderEntities : ClientSystem
{
	public override string Name => "ree";

	public SystemRenderEntities(ClientMain game)
		: base(game)
	{
		game.eventManager.RegisterRenderer(OnBeforeRender, EnumRenderStage.Before, Name, 0.4);
		game.eventManager.RegisterRenderer(OnRenderOpaque3D, EnumRenderStage.Opaque, Name, 0.4);
		game.eventManager.RegisterRenderer(OnRenderOIT, EnumRenderStage.OIT, Name, 0.4);
		game.eventManager.RegisterRenderer(OnRenderAfterOIT, EnumRenderStage.AfterOIT, Name, 0.7);
		game.eventManager.RegisterRenderer(OnRenderFrame2D, EnumRenderStage.Ortho, Name, 0.4);
		game.eventManager.RegisterRenderer(OnRenderFrameShadows, EnumRenderStage.ShadowFar, Name, 0.4);
		game.eventManager.RegisterRenderer(OnRenderFrameShadows, EnumRenderStage.ShadowNear, Name, 0.4);
	}

	private void OnBeforeRender(float dt)
	{
		int viewDistSq = ClientSettings.ViewDistance * ClientSettings.ViewDistance;
		Vec3d plrPos = game.EntityPlayer.Pos.XYZ;
		int plrDim = game.EntityPlayer.Pos.Dimension;
		foreach (KeyValuePair<long, EntityRenderer> val in game.EntityRenderers)
		{
			Entity entity = val.Value.entity;
			if (game.frustumCuller.SphereInFrustum((float)entity.Pos.X, (float)entity.Pos.InternalY, (float)entity.Pos.Z, entity.FrustumSphereRadius) && entity.Pos.Dimension == plrDim && (entity.AllowOutsideLoadedRange || (plrPos.HorizontalSquareDistanceTo(entity.Pos.X, entity.Pos.Z) < (float)viewDistSq && (entity == game.EntityPlayer || game.WorldMap.IsChunkRendered(entity.Pos)))))
			{
				entity.IsRendered = true;
				val.Value.BeforeRender(dt);
			}
			else
			{
				entity.IsRendered = false;
			}
			game.api.World.FrameProfiler.Mark("esr-beforeanim");
			try
			{
				entity.AnimManager?.OnClientFrame(dt);
			}
			catch (Exception)
			{
				game.Logger.Error("Animations error for entity " + entity.Code.ToShortString() + " at " + entity.Pos.AsBlockPos);
				throw;
			}
			game.api.World.FrameProfiler.Mark("esr-afteranim");
		}
	}

	public void OnRenderOpaque3D(float deltaTime)
	{
		RuntimeStats.renderedEntities = 0;
		game.GlMatrixModeModelView();
		game.Platform.GlDisableCullFace();
		game.Platform.GlToggleBlend(on: true);
		game.Platform.GlEnableDepthTest();
		foreach (KeyValuePair<long, EntityRenderer> val in game.EntityRenderers)
		{
			if (val.Value.entity.IsRendered)
			{
				val.Value.DoRender3DOpaque(deltaTime, isShadowPass: false);
				RuntimeStats.renderedEntities++;
			}
		}
		ScreenManager.FrameProfiler.Mark("ree-op");
		ShaderProgramEntityanimated prog = ShaderPrograms.Entityanimated;
		prog.Use();
		prog.RgbaAmbientIn = game.api.renderapi.AmbientColor;
		prog.RgbaFogIn = game.api.renderapi.FogColor;
		prog.FogMinIn = game.api.renderapi.FogMin;
		prog.FogDensityIn = game.api.renderapi.FogDensity;
		prog.ProjectionMatrix = game.CurrentProjectionMatrix;
		prog.EntityTex2D = game.EntityAtlasManager.AtlasTextures[0].TextureId;
		prog.AlphaTest = 0.05f;
		prog.LightPosition = game.shUniforms.LightPosition3D;
		game.Platform.GlDisableCullFace();
		game.GlMatrixModeModelView();
		game.GlPushMatrix();
		game.GlLoadMatrix(game.MainCamera.CameraMatrixOrigin);
		game.Platform.GlToggleBlend(on: true);
		foreach (KeyValuePair<long, EntityRenderer> val2 in game.EntityRenderers)
		{
			if (val2.Value.entity.IsRendered)
			{
				val2.Value.DoRender3DOpaqueBatched(deltaTime, isShadowPass: false);
			}
		}
		game.GlPopMatrix();
		prog.Stop();
		ScreenManager.FrameProfiler.Mark("ree-op-b");
		game.Platform.GlToggleBlend(on: false);
	}

	private void OnRenderOIT(float dt)
	{
	}

	private void OnRenderAfterOIT(float dt)
	{
		game.GlMatrixModeModelView();
		game.Platform.GlDisableCullFace();
		game.Platform.GlToggleBlend(on: true);
		game.Platform.GlEnableDepthTest();
		foreach (KeyValuePair<long, EntityRenderer> val in game.EntityRenderers)
		{
			if (val.Value.entity.IsRendered)
			{
				val.Value.DoRender3DAfterOIT(dt, isShadowPass: false);
			}
		}
	}

	private void OnRenderFrameShadows(float dt)
	{
		int plrDim = game.EntityPlayer.Pos.Dimension;
		ShaderProgramChunkshadowmap prog = (ShaderProgramChunkshadowmap)ShaderProgramBase.CurrentShaderProgram;
		if (game.api.Render.UseSSBOs)
		{
			prog.Stop();
			prog = (ShaderProgramChunkshadowmap)game.api.Shader.GetProgram(42);
			prog.Use();
		}
		foreach (KeyValuePair<long, EntityRenderer> val in game.EntityRenderers)
		{
			Entity entity = val.Value.entity;
			if (game.frustumCuller.SphereInFrustum((float)entity.Pos.X, (float)entity.Pos.InternalY, (float)entity.Pos.Z, 3.0) && (entity == game.EntityPlayer || (game.WorldMap.IsValidPos((int)entity.Pos.X, (int)entity.Pos.InternalY, (int)entity.Pos.Z) && game.WorldMap.IsChunkRendered(entity.Pos))) && entity.Pos.Dimension == plrDim)
			{
				entity.IsShadowRendered = true;
				val.Value.DoRender3DOpaque(dt, isShadowPass: true);
			}
			else
			{
				entity.IsShadowRendered = false;
			}
		}
		prog.Stop();
		ShaderProgramShadowmapentityanimated proge = ShaderPrograms.Shadowmapentityanimated;
		proge.Use();
		proge.ProjectionMatrix = game.CurrentProjectionMatrix;
		proge.EntityTex2D = game.EntityAtlasManager.AtlasTextures[0].TextureId;
		_ = game.api.Render;
		_ = game.api.World.Player.Entity;
		foreach (KeyValuePair<long, EntityRenderer> val2 in game.EntityRenderers)
		{
			if (val2.Value.entity.IsShadowRendered)
			{
				val2.Value.DoRender3DOpaqueBatched(dt, isShadowPass: true);
			}
		}
		proge.Stop();
		prog.Use();
		prog.MvpMatrix = game.shadowMvpMatrix;
	}

	private void OnRenderFrame2D(float dt)
	{
		foreach (KeyValuePair<long, EntityRenderer> val in game.EntityRenderers)
		{
			Entity entity = val.Value.entity;
			EntityRenderer renderer = val.Value;
			if (entity.IsRendered)
			{
				renderer.DoRender2D(dt);
			}
		}
		ScreenManager.FrameProfiler.Mark("ree2d-d");
	}

	public override void Dispose(ClientMain game)
	{
		foreach (KeyValuePair<long, EntityRenderer> entityRenderer in game.EntityRenderers)
		{
			entityRenderer.Value.Dispose();
		}
	}

	public override EnumClientSystemType GetSystemType()
	{
		return EnumClientSystemType.Render;
	}
}
