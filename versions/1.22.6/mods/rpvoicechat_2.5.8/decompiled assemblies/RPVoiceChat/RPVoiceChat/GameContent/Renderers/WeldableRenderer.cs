using System;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Renderers
{
	// Token: 0x020000BE RID: 190
	public class WeldableRenderer : IRenderer, IDisposable
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x000263B7 File Offset: 0x000245B7
		public double RenderOrder
		{
			get
			{
				return 0.5;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x000263C2 File Offset: 0x000245C2
		public int RenderRange
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x000263C6 File Offset: 0x000245C6
		public WeldableRenderer(ICoreClientAPI capi, BEWeldable BEWeldable)
		{
			if (capi == null)
			{
				return;
			}
			this.capi = capi;
			this.BEWeldable = BEWeldable;
			capi.Event.RegisterRenderer(this, 1, null);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000263F9 File Offset: 0x000245F9
		public virtual void Dispose()
		{
			if (this.capi == null)
			{
				return;
			}
			this.capi.Event.UnregisterRenderer(this, 1);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00026418 File Offset: 0x00024618
		public virtual void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			if (Enumerable.All<MeshRef>(this.BEWeldable.PartMeshRefs, (MeshRef x) => x == null))
			{
				return;
			}
			int firstNonNull = Array.FindIndex<MeshRef>(this.BEWeldable.PartMeshRefs, (MeshRef x) => x != null);
			IRenderAPI rpi = this.capi.Render;
			Vec3d camPos = this.capi.World.Player.Entity.CameraPos;
			int num = (int)this.BEWeldable.Inv[firstNonNull + 1].Itemstack.Collectible.GetTemperature(this.capi.World, this.BEWeldable.Inv[firstNonNull + 1].Itemstack);
			Vec4f lightrgbs = this.capi.World.BlockAccessor.GetLightRGBs(this.BEWeldable.Pos.X, this.BEWeldable.Pos.Y, this.BEWeldable.Pos.Z);
			float[] glowColor = ColorUtil.GetIncandescenceColorAsColor4f(num);
			int extraGlow = GameMath.Clamp((num - 550) / 2, 0, 255);
			rpi.GlDisableCullFace();
			rpi.GlToggleBlend(true, 0);
			IStandardShaderProgram prog = rpi.PreparedStandardShader(this.BEWeldable.Pos.X, this.BEWeldable.Pos.Y, this.BEWeldable.Pos.Z, null);
			prog.Tex2D = this.capi.BlockTextureAtlas.AtlasTextures[0].TextureId;
			prog.ModelMatrix = this.ModelMat.Identity().Translate((double)this.BEWeldable.Pos.X - camPos.X, (double)this.BEWeldable.Pos.Y - camPos.Y, (double)this.BEWeldable.Pos.Z - camPos.Z).Values;
			prog.RgbaLightIn = lightrgbs;
			prog.RgbaGlowIn = new Vec4f(glowColor[0], glowColor[1], glowColor[2], (float)extraGlow / 255f);
			prog.ExtraGlow = extraGlow;
			prog.ViewMatrix = rpi.CameraMatrixOriginf;
			prog.ProjectionMatrix = rpi.CurrentProjectionMatrix;
			for (int i = 0; i < this.BEWeldable.PartMeshRefs.Length; i++)
			{
				if (this.BEWeldable.PartMeshRefs[i] != null && !this.BEWeldable.PartMeshRefs[i].Disposed)
				{
					int num2 = (int)this.BEWeldable.Inv[i + 1].Itemstack.Collectible.GetTemperature(this.capi.World, this.BEWeldable.Inv[i + 1].Itemstack);
					lightrgbs = this.capi.World.BlockAccessor.GetLightRGBs(this.BEWeldable.Pos.X, this.BEWeldable.Pos.Y, this.BEWeldable.Pos.Z);
					glowColor = ColorUtil.GetIncandescenceColorAsColor4f(num2);
					extraGlow = GameMath.Clamp((num2 - 550) / 2, 0, 255);
					prog.RgbaLightIn = lightrgbs;
					prog.RgbaGlowIn = new Vec4f(glowColor[0], glowColor[1], glowColor[2], (float)extraGlow / 255f);
					prog.ExtraGlow = extraGlow;
					rpi.RenderMesh(this.BEWeldable.PartMeshRefs[i]);
				}
			}
			for (int j = 0; j < this.BEWeldable.FluxMeshRefs.Length; j++)
			{
				if (this.BEWeldable.FluxMeshRefs[j] != null && !this.BEWeldable.FluxMeshRefs[j].Disposed)
				{
					prog.ExtraGlow = 0;
					rpi.RenderMesh(this.BEWeldable.FluxMeshRefs[j]);
				}
			}
			prog.Stop();
		}

		// Token: 0x040002FC RID: 764
		protected BEWeldable BEWeldable;

		// Token: 0x040002FD RID: 765
		protected ICoreClientAPI capi;

		// Token: 0x040002FE RID: 766
		public Matrixf ModelMat = new Matrixf();
	}
}
