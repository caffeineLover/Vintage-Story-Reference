using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000039 RID: 57
	public class EntityGenericShapeRenderer : EntityShapeRenderer, ITexPositionSource
	{
		// Token: 0x06000266 RID: 614 RVA: 0x0001C598 File Offset: 0x0001A798
		public EntityGenericShapeRenderer(Entity entity, ICoreClientAPI api) : base(entity, api)
		{
			EntityGenericShapeRenderer <>4__this = this;
			api.Event.ReloadTextures += delegate()
			{
				<>4__this.capi = api;
			};
			this.modSystem = api.ModLoader.GetModSystem<PrimitiveSurvivalSystem>(true);
			this.offset = entity.Pos.X + entity.Pos.Y + entity.Pos.Z;
			this.strobeFrequency = this.entity.Properties.Attributes["strobeFrequency"].AsFloat(0f);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0001C64C File Offset: 0x0001A84C
		public override void DoRender3DOpaqueBatched(float dt, bool isShadowPass)
		{
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0001C650 File Offset: 0x0001A850
		public override void DoRender3DOpaque(float dt, bool isShadowPass)
		{
			base.DoRender3DOpaque(dt, isShadowPass);
			if (isShadowPass)
			{
				return;
			}
			IShaderProgram prevProg = this.capi.Render.CurrentActiveShader;
			if (prevProg != null)
			{
				prevProg.Stop();
			}
			IShaderProgram prog = this.modSystem.EntityGenericShaderProgram;
			prog.Use();
			Vec4f lightrgbs = this.capi.World.BlockAccessor.GetLightRGBs((int)(this.entity.Pos.X + (double)this.entity.SelectionBox.X1 - (double)this.entity.OriginSelectionBox.X1), (int)this.entity.Pos.Y, (int)(this.entity.Pos.Z + (double)this.entity.SelectionBox.Z1 - (double)this.entity.OriginSelectionBox.Z1));
			if (this.strobeFrequency > 0f)
			{
				this.time += (double)(dt * this.strobeFrequency);
				float timeoff = (float)Math.Abs(Math.Sin(this.time + this.offset));
				lightrgbs.R = lightrgbs.R * 0.2f + timeoff * 0.8f;
				lightrgbs.G = lightrgbs.G * 0.2f + timeoff * 0.8f;
				lightrgbs.B = lightrgbs.B * 0.2f + timeoff * 0.8f;
				lightrgbs.A = lightrgbs.A * 0.2f + timeoff * 0.8f;
				prog.Uniform("extraGlow", (int)timeoff);
			}
			else
			{
				prog.Uniform("extraGlow", this.entity.Properties.Client.GlowLevel);
			}
			IRenderAPI rapi = this.capi.Render;
			prog.Uniform("rgbaAmbientIn", rapi.AmbientColor);
			prog.Uniform("rgbaLightIn", lightrgbs);
			prog.Uniform("rgbaFogIn", rapi.FogColor);
			prog.Uniform("fogMinIn", rapi.FogMin);
			prog.Uniform("fogDensityIn", rapi.FogDensity);
			prog.BindTexture2D("entityTex", this.capi.EntityTextureAtlas.Positions[0].atlasTextureId, 0);
			prog.UniformMatrix("modelMatrix", this.ModelMat);
			prog.UniformMatrix("viewMatrix", this.capi.Render.CameraMatrixOriginf);
			prog.Uniform("addRenderFlags", this.AddRenderFlags);
			prog.Uniform("windWaveIntensity", (float)this.WindWaveIntensity);
			prog.Uniform("skipRenderJointId", -2);
			prog.Uniform("skipRenderJointId2", -2);
			prog.Uniform("entityId", (int)this.entity.EntityId);
			prog.Uniform("waterWaveCounter", this.capi.Render.ShaderUniforms.WaterWaveCounter);
			this.color[0] = (float)(this.entity.RenderColor >> 16 & 255) / 255f;
			this.color[1] = (float)(this.entity.RenderColor >> 8 & 255) / 255f;
			this.color[2] = (float)(this.entity.RenderColor & 255) / 255f;
			this.color[3] = (float)(this.entity.RenderColor >> 24 & 255) / 255f;
			prog.Uniform("renderColor", this.color);
			prog.UniformMatrix("projectionMatrix", this.capi.Render.CurrentProjectionMatrix);
			double @double = this.entity.WatchedAttributes.GetDouble("temporalStability", 1.0);
			double plrStab = this.capi.World.Player.Entity.WatchedAttributes.GetDouble("temporalStability", 1.0);
			double stabMin = Math.Min(@double, plrStab);
			float strength = (float)(this.glitchAffected ? Math.Max(0.0, 1.0 - 2.5 * stabMin) : 0.0);
			prog.Uniform("glitchEffectStrength", strength);
			prog.UBOs["Animation"].Update(this.entity.AnimManager.Animator.Matrices, 0, this.entity.AnimManager.Animator.MaxJointId * 16 * 4);
			if (this.meshRefOpaque != null)
			{
				this.capi.Render.RenderMultiTextureMesh(this.meshRefOpaque, "entityTex", 0);
			}
			prog.Stop();
		}

		// Token: 0x04000110 RID: 272
		private readonly PrimitiveSurvivalSystem modSystem;

		// Token: 0x04000111 RID: 273
		private double time;

		// Token: 0x04000112 RID: 274
		private readonly float strobeFrequency;

		// Token: 0x04000113 RID: 275
		private readonly double offset;
	}
}
