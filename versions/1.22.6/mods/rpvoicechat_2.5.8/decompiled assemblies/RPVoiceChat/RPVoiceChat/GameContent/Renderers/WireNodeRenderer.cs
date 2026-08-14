using System;
using System.Collections.Generic;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.GameContent.Renderers
{
	// Token: 0x020000BF RID: 191
	public class WireNodeRenderer : IRenderer, IDisposable
	{
		// Token: 0x060006C4 RID: 1732 RVA: 0x00026814 File Offset: 0x00024A14
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

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00026870 File Offset: 0x00024A70
		public double RenderOrder
		{
			get
			{
				return 0.5;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x0002687B File Offset: 0x00024A7B
		public int RenderRange
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0002687F File Offset: 0x00024A7F
		public void MarkNeedsRebuild()
		{
			this.needsRebuild = true;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00026888 File Offset: 0x00024A88
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

		// Token: 0x060006C9 RID: 1737 RVA: 0x000269F8 File Offset: 0x00024BF8
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

		// Token: 0x060006CA RID: 1738 RVA: 0x00026D10 File Offset: 0x00024F10
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

		// Token: 0x040002FF RID: 767
		private readonly ICoreClientAPI capi;

		// Token: 0x04000300 RID: 768
		private readonly BEWireNode node;

		// Token: 0x04000301 RID: 769
		private AssetLocation wireTextureAsset = new AssetLocation("game:block/metal/plate/copper");

		// Token: 0x04000302 RID: 770
		private MeshRef meshRef;

		// Token: 0x04000303 RID: 771
		private Vec3d meshOrigin;

		// Token: 0x04000304 RID: 772
		private bool needsRebuild = true;
	}
}
