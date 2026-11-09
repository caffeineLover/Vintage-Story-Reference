using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000050 RID: 80
	public class VenomOverlayRenderer : IRenderer, IDisposable
	{
		// Token: 0x060002F4 RID: 756 RVA: 0x00022D48 File Offset: 0x00020F48
		public VenomOverlayRenderer(ICoreClientAPI capi)
		{
			this.capi = capi;
			this.venomCounter = 20f;
			MeshData quadMesh = QuadMeshUtil.GetCustomQuadModelData(-1f, -1f, 0f, 2f, 2f);
			quadMesh.Rgba = null;
			this.quadRef = capi.Render.UploadMesh(quadMesh);
			capi.Event.ReloadShader += new ActionBoolReturn(this.LoadShader);
			capi.Event.RegisterRenderer(this, 10, "poison");
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00022DCF File Offset: 0x00020FCF
		public double RenderOrder
		{
			get
			{
				return 1.2;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00022DDA File Offset: 0x00020FDA
		public int RenderRange
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00022DDD File Offset: 0x00020FDD
		public void Dispose()
		{
			this.capi.Render.DeleteMesh(this.quadRef);
			this.overlayShaderProg.Dispose();
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00022E00 File Offset: 0x00021000
		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			string venomed = VenomState.Venomed;
			string player = this.capi.World.Player.Entity.GetName();
			if (venomed != player)
			{
				return;
			}
			this.venomCounter -= 1f * deltaTime;
			this.capi.World.SetCameraShake(0.2f);
			IShaderProgram currentActiveShader = this.capi.Render.CurrentActiveShader;
			currentActiveShader.Stop();
			this.overlayShaderProg.Use();
			this.capi.Render.GlToggleBlend(true, 0);
			float elapsedTime = (float)this.capi.World.ElapsedMilliseconds / 1000f;
			this.overlayShaderProg.Uniform("time", elapsedTime);
			this.capi.Render.RenderMesh(this.quadRef);
			this.overlayShaderProg.Stop();
			currentActiveShader.Use();
			if (this.venomCounter < 0f && (float)VenomOverlayRenderer.Rnd.Next(1000) < 1f)
			{
				this.capi.World.SetCameraShake(0f);
				VenomState.Venomed = "";
				this.venomCounter = 20f;
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00022F30 File Offset: 0x00021130
		public bool LoadShader()
		{
			this.overlayShaderProg = this.capi.Shader.NewShaderProgram();
			this.overlayShaderProg.VertexShader = this.capi.Shader.NewShader(35633);
			this.overlayShaderProg.FragmentShader = this.capi.Shader.NewShader(35632);
			this.capi.Shader.RegisterFileShaderProgram("poison", this.overlayShaderProg);
			this.overlayShaderProg.Compile();
			return true;
		}

		// Token: 0x04000132 RID: 306
		private readonly MeshRef quadRef;

		// Token: 0x04000133 RID: 307
		private readonly ICoreClientAPI capi;

		// Token: 0x04000134 RID: 308
		public IShaderProgram overlayShaderProg;

		// Token: 0x04000135 RID: 309
		private float venomCounter;

		// Token: 0x04000136 RID: 310
		protected static readonly Random Rnd = new Random();
	}
}
