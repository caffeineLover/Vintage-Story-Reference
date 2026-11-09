using System;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Renderers
{
	// Token: 0x020000F7 RID: 247
	public class WeldableRenderer : IRenderer, IDisposable
	{
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0003184B File Offset: 0x0002FA4B
		public double RenderOrder
		{
			get
			{
				return 0.5;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x00031856 File Offset: 0x0002FA56
		public int RenderRange
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0003185A File Offset: 0x0002FA5A
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

		// Token: 0x060008DA RID: 2266 RVA: 0x0003188D File Offset: 0x0002FA8D
		public virtual void Dispose()
		{
			if (this.capi == null)
			{
				return;
			}
			this.capi.Event.UnregisterRenderer(this, 1);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x000318AC File Offset: 0x0002FAAC
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

		// Token: 0x040003DB RID: 987
		protected BEWeldable BEWeldable;

		// Token: 0x040003DC RID: 988
		protected ICoreClientAPI capi;

		// Token: 0x040003DD RID: 989
		public Matrixf ModelMat = new Matrixf();
	}
}
