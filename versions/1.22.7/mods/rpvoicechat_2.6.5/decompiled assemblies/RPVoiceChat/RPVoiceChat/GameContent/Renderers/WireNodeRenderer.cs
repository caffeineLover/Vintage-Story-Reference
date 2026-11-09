using System;
using System.Collections.Generic;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Renderers
{
	// Token: 0x020000F8 RID: 248
	public class WireNodeRenderer : IRenderer, IDisposable
	{
		// Token: 0x060008DC RID: 2268 RVA: 0x00031CA8 File Offset: 0x0002FEA8
		public WireNodeRenderer(BEWireNode node, ICoreClientAPI capi)
		{
			this.node = node;
			this.capi = capi;
			node.OnConnectionsChanged += delegate()
			{
				this.MarkNeedsRebuild();
			};
			capi.Event.RegisterRenderer(this, 1, "wirenoderenderer");
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x00031D04 File Offset: 0x0002FF04
		public double RenderOrder
		{
			get
			{
				return 0.5;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x00031D0F File Offset: 0x0002FF0F
		public int RenderRange
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00031D13 File Offset: 0x0002FF13
		public void MarkNeedsRebuild()
		{
			this.needsRebuild = true;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00031D1C File Offset: 0x0002FF1C
		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			if (stage != 1 || this.node == null)
			{
				return;
			}
			if (this.needsRebuild)
			{
				this.RebuildMesh();
			}
			if (this.meshRef == null)
			{
				return;
			}
			Vec3d camPos = this.capi.World.Player.Entity.CameraPos;
			this.capi.Render.GLEnableDepthTest();
			this.capi.Render.GlEnableCullFace();
			IStandardShaderProgram standardShaderProgram = this.capi.Render.PreparedStandardShader(this.node.Position.X, this.node.Position.Y, this.node.Position.Z, null);
			standardShaderProgram.ProjectionMatrix = this.capi.Render.CurrentProjectionMatrix;
			standardShaderProgram.ViewMatrix = this.capi.Render.CameraMatrixOriginf;
			standardShaderProgram.ModelMatrix = new Matrixf().Identity().Translate((float)(this.meshOrigin.X - camPos.X), (float)(this.meshOrigin.Y - camPos.Y), (float)(this.meshOrigin.Z - camPos.Z)).Values;
			standardShaderProgram.Tex2D = this.capi.BlockTextureAtlas.AtlasTextures[0].TextureId;
			standardShaderProgram.Use();
			this.capi.Render.RenderMesh(this.meshRef);
			standardShaderProgram.Stop();
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00031E8C File Offset: 0x0003008C
		private void RebuildMesh()
		{
			MeshRef meshRef = this.meshRef;
			if (meshRef != null)
			{
				meshRef.Dispose();
			}
			this.meshRef = null;
			IReadOnlyList<WireConnection> connections = this.node.GetConnections();
			if (connections == null || connections.Count == 0)
			{
				return;
			}
			Vec3d origin = new Vec3d((double)this.node.Position.X, (double)this.node.Position.Y, (double)this.node.Position.Z);
			this.meshOrigin = origin;
			MeshData combinedMesh = new MeshData(4, 6, false, true, true, true);
			combinedMesh.SetMode(0);
			foreach (WireConnection wireConnection in connections)
			{
				BlockPos otherBlockPos = wireConnection.GetOtherBlockPos(this.node.Pos);
				if (!(otherBlockPos == null))
				{
					BEWireNode bewireNode = this.capi.World.BlockAccessor.GetBlockEntity(otherBlockPos) as BEWireNode;
					Vec3f startOffset = this.node.GetWireAttachmentOffsetFor(otherBlockPos);
					Vec3f otherOffset = ((bewireNode != null) ? bewireNode.GetWireAttachmentOffsetFor(this.node.Pos) : null) ?? new Vec3f(0.5f, 0.5f, 0.5f);
					Vec3f pos = startOffset;
					Vec3f endLocal = new Vec3f((float)((double)((float)otherBlockPos.X + otherOffset.X) - origin.X), (float)((double)((float)otherBlockPos.Y + otherOffset.Y) - origin.Y), (float)((double)((float)otherBlockPos.Z + otherOffset.Z) - origin.Z));
					MeshData wireMesh = WireMesh.MakeWireMesh(pos, endLocal, 0.025f);
					combinedMesh.AddMeshData(wireMesh);
				}
			}
			if (combinedMesh.VerticesCount > 0)
			{
				int subId;
				TextureAtlasPosition texPos;
				if (this.capi.BlockTextureAtlas.GetOrInsertTexture(this.wireTextureAsset, ref subId, ref texPos, null, 0f) && combinedMesh.Uv != null && combinedMesh.Uv.Length >= 2)
				{
					for (int idx = 0; idx < combinedMesh.Uv.Length - 1; idx += 2)
					{
						float u = combinedMesh.Uv[idx];
						float v = combinedMesh.Uv[idx + 1];
						u = (float)((double)u - Math.Floor((double)u));
						v = GameMath.Clamp(v, 0f, 1f);
						combinedMesh.Uv[idx] = GameMath.Lerp(texPos.x1, texPos.x2, u);
						combinedMesh.Uv[idx + 1] = GameMath.Lerp(texPos.y1, texPos.y2, v);
					}
				}
				int vcount = combinedMesh.VerticesCount;
				combinedMesh.Rgba = new byte[vcount * 4];
				for (int i = 0; i < vcount; i++)
				{
					combinedMesh.Rgba[i * 4] = byte.MaxValue;
					combinedMesh.Rgba[i * 4 + 1] = byte.MaxValue;
					combinedMesh.Rgba[i * 4 + 2] = byte.MaxValue;
					combinedMesh.Rgba[i * 4 + 3] = byte.MaxValue;
				}
				this.meshRef = this.capi.Render.UploadMesh(combinedMesh);
			}
			this.needsRebuild = false;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x000321A4 File Offset: 0x000303A4
		public void Dispose()
		{
			MeshRef meshRef = this.meshRef;
			if (meshRef != null)
			{
				meshRef.Dispose();
			}
			this.meshRef = null;
			this.capi.Event.UnregisterRenderer(this, 1);
			GC.SuppressFinalize(this);
		}

		// Token: 0x040003DE RID: 990
		private readonly ICoreClientAPI capi;

		// Token: 0x040003DF RID: 991
		private readonly BEWireNode node;

		// Token: 0x040003E0 RID: 992
		private AssetLocation wireTextureAsset = new AssetLocation("game:block/metal/plate/copper");

		// Token: 0x040003E1 RID: 993
		private MeshRef meshRef;

		// Token: 0x040003E2 RID: 994
		private Vec3d meshOrigin;

		// Token: 0x040003E3 RID: 995
		private bool needsRebuild = true;
	}
}
