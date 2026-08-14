using System;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Renderers
{
	// Token: 0x020000BD RID: 189
	public class WarningBeaconBonfireRenderer : IRenderer, IDisposable
	{
		// Token: 0x060006B8 RID: 1720 RVA: 0x00025FA8 File Offset: 0x000241A8
		public WarningBeaconBonfireRenderer(BlockEntityLucerne lucerne, ICoreClientAPI capi)
		{
			this.lucerne = lucerne;
			this.capi = capi;
			this.lastShowAshes = lucerne.ShowAshes;
			capi.Event.RegisterRenderer(this, 1, "warningbeaconbonfire");
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x00025FE3 File Offset: 0x000241E3
		public double RenderOrder
		{
			get
			{
				return 0.6;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00025FEE File Offset: 0x000241EE
		public int RenderRange
		{
			get
			{
				return 80;
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00025FF4 File Offset: 0x000241F4
		public void UpdateState()
		{
			bool showAshes = this.lucerne.ShowAshes;
			if (showAshes != this.lastShowAshes)
			{
				this.lastShowAshes = showAshes;
				this.meshBuilt = false;
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00026024 File Offset: 0x00024224
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

		// Token: 0x060006BD RID: 1725 RVA: 0x00026238 File Offset: 0x00024438
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

		// Token: 0x060006BE RID: 1726 RVA: 0x0002635C File Offset: 0x0002455C
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

		// Token: 0x040002F3 RID: 755
		private const string ShapeBonfire = "shapes/block/warningbeacon/bonfire.json";

		// Token: 0x040002F4 RID: 756
		private const string ShapeBonfireAshes = "shapes/block/warningbeacon/bonfire_ashes.json";

		// Token: 0x040002F5 RID: 757
		private readonly ICoreClientAPI capi;

		// Token: 0x040002F6 RID: 758
		private readonly BlockEntityLucerne lucerne;

		// Token: 0x040002F7 RID: 759
		private MeshRef meshBonfire;

		// Token: 0x040002F8 RID: 760
		private MeshRef meshAshes;

		// Token: 0x040002F9 RID: 761
		private Vec3d origin;

		// Token: 0x040002FA RID: 762
		private bool lastShowAshes = true;

		// Token: 0x040002FB RID: 763
		private bool meshBuilt;
	}
}
