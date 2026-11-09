using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Renderers
{
	// Token: 0x020000F6 RID: 246
	public class WarningBeaconBonfireRenderer : IRenderer, IDisposable
	{
		// Token: 0x060008D0 RID: 2256 RVA: 0x0003143C File Offset: 0x0002F63C
		public WarningBeaconBonfireRenderer(BlockEntityLucerne lucerne, ICoreClientAPI capi)
		{
			this.lucerne = lucerne;
			this.capi = capi;
			this.lastShowAshes = lucerne.ShowAshes;
			capi.Event.RegisterRenderer(this, 1, "warningbeaconbonfire");
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x00031477 File Offset: 0x0002F677
		public double RenderOrder
		{
			get
			{
				return 0.6;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x00031482 File Offset: 0x0002F682
		public int RenderRange
		{
			get
			{
				return 80;
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00031488 File Offset: 0x0002F688
		public void UpdateState()
		{
			bool showAshes = this.lucerne.ShowAshes;
			if (showAshes != this.lastShowAshes)
			{
				this.lastShowAshes = showAshes;
				this.meshBuilt = false;
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000314B8 File Offset: 0x0002F6B8
		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			if (stage != 1 || this.lucerne == null)
			{
				return;
			}
			if (!this.meshBuilt)
			{
				this.BuildMeshes();
			}
			MeshRef current = this.lucerne.ShowAshes ? this.meshAshes : this.meshBonfire;
			if (current == null)
			{
				return;
			}
			BlockPos structureCenterWorldPos = this.lucerne.GetStructureCenterWorldPos();
			this.origin = new Vec3d((double)this.lucerne.Pos.X, (double)this.lucerne.Pos.Y, (double)this.lucerne.Pos.Z);
			float dx = (float)((double)structureCenterWorldPos.X - this.origin.X);
			float dy = (float)((double)structureCenterWorldPos.Y - this.origin.Y);
			float dz = (float)((double)structureCenterWorldPos.Z - this.origin.Z);
			Vec3d camPos = this.capi.World.Player.Entity.CameraPos;
			IStandardShaderProgram standardShaderProgram = this.capi.Render.PreparedStandardShader((int)this.origin.X, (int)this.origin.Y, (int)this.origin.Z, null);
			standardShaderProgram.ProjectionMatrix = this.capi.Render.CurrentProjectionMatrix;
			standardShaderProgram.ViewMatrix = this.capi.Render.CameraMatrixOriginf;
			standardShaderProgram.ModelMatrix = new Matrixf().Identity().Translate((float)(this.origin.X - camPos.X) + dx, (float)(this.origin.Y - camPos.Y) + dy, (float)(this.origin.Z - camPos.Z) + dz).Values;
			IBlockTextureAtlasAPI blockTextureAtlas = this.capi.BlockTextureAtlas;
			int? num;
			if (blockTextureAtlas == null)
			{
				num = default(int?);
			}
			else
			{
				LoadedTexture loadedTexture = blockTextureAtlas.AtlasTextures[0];
				num = ((loadedTexture != null) ? new int?(loadedTexture.TextureId) : default(int?));
			}
			int? num2 = num;
			standardShaderProgram.Tex2D = num2.GetValueOrDefault();
			standardShaderProgram.Use();
			this.capi.Render.RenderMesh(current);
			standardShaderProgram.Stop();
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000316CC File Offset: 0x0002F8CC
		private void BuildMeshes()
		{
			MeshRef meshRef = this.meshBonfire;
			if (meshRef != null)
			{
				meshRef.Dispose();
			}
			MeshRef meshRef2 = this.meshAshes;
			if (meshRef2 != null)
			{
				meshRef2.Dispose();
			}
			this.meshBonfire = null;
			this.meshAshes = null;
			Block block = this.lucerne.Block;
			if (block == null)
			{
				return;
			}
			AssetLocation locBonfire = new AssetLocation(RPVoiceChatMod.modID, "shapes/block/warningbeacon/bonfire.json");
			AssetLocation locAshes = new AssetLocation(RPVoiceChatMod.modID, "shapes/block/warningbeacon/bonfire_ashes.json");
			Shape shapeBonfire = Shape.TryGet(this.capi, locBonfire);
			Shape shapeAshes = Shape.TryGet(this.capi, locAshes);
			if (shapeBonfire != null)
			{
				MeshData meshDataBonfire;
				this.capi.Tesselator.TesselateShape(block, shapeBonfire, ref meshDataBonfire, null, default(int?), null);
				if (meshDataBonfire != null && meshDataBonfire.VerticesCount > 0)
				{
					this.meshBonfire = this.capi.Render.UploadMesh(meshDataBonfire);
				}
			}
			if (shapeAshes != null)
			{
				MeshData meshDataAshes;
				this.capi.Tesselator.TesselateShape(block, shapeAshes, ref meshDataAshes, null, default(int?), null);
				if (meshDataAshes != null && meshDataAshes.VerticesCount > 0)
				{
					this.meshAshes = this.capi.Render.UploadMesh(meshDataAshes);
				}
			}
			this.meshBuilt = true;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000317F0 File Offset: 0x0002F9F0
		public void Dispose()
		{
			MeshRef meshRef = this.meshBonfire;
			if (meshRef != null)
			{
				meshRef.Dispose();
			}
			MeshRef meshRef2 = this.meshAshes;
			if (meshRef2 != null)
			{
				meshRef2.Dispose();
			}
			this.meshBonfire = null;
			this.meshAshes = null;
			ICoreClientAPI coreClientAPI = this.capi;
			if (coreClientAPI != null)
			{
				coreClientAPI.Event.UnregisterRenderer(this, 1);
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x040003D2 RID: 978
		private const string ShapeBonfire = "shapes/block/warningbeacon/bonfire.json";

		// Token: 0x040003D3 RID: 979
		private const string ShapeBonfireAshes = "shapes/block/warningbeacon/bonfire_ashes.json";

		// Token: 0x040003D4 RID: 980
		private readonly ICoreClientAPI capi;

		// Token: 0x040003D5 RID: 981
		private readonly BlockEntityLucerne lucerne;

		// Token: 0x040003D6 RID: 982
		private MeshRef meshBonfire;

		// Token: 0x040003D7 RID: 983
		private MeshRef meshAshes;

		// Token: 0x040003D8 RID: 984
		private Vec3d origin;

		// Token: 0x040003D9 RID: 985
		private bool lastShowAshes = true;

		// Token: 0x040003DA RID: 986
		private bool meshBuilt;
	}
}
