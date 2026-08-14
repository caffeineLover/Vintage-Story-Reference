using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace RPVoiceChat.GameContent.Renderers
{
	// Token: 0x020000BC RID: 188
	public class RotatingMechPartRenderer : IRenderer, IDisposable
	{
		// Token: 0x060006B2 RID: 1714 RVA: 0x00025C86 File Offset: 0x00023E86
		public RotatingMechPartRenderer(BlockEntity blockEntity, ICoreClientAPI capi, AssetLocation shapeLoc, float baseRotYDeg)
		{
			this.blockEntity = blockEntity;
			this.capi = capi;
			this.shapeLoc = shapeLoc;
			this.baseRotYDeg = baseRotYDeg;
			this.BuildMesh();
			capi.Event.RegisterRenderer(this, 1, "rpvc-rotatingmechpart");
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00025CC3 File Offset: 0x00023EC3
		public double RenderOrder
		{
			get
			{
				return 0.55;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x00025CCE File Offset: 0x00023ECE
		public int RenderRange
		{
			get
			{
				return 80;
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00025CD4 File Offset: 0x00023ED4
		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			if (stage == 1 && this.meshRef != null)
			{
				BlockEntity blockEntity = this.blockEntity;
				if (((blockEntity != null) ? blockEntity.Api : null) != null)
				{
					BEBehaviorMPConsumer consumer = this.blockEntity.GetBehavior<BEBehaviorMPConsumer>();
					if (consumer == null)
					{
						return;
					}
					Vec3d camPos = this.capi.World.Player.Entity.CameraPos;
					float angle = consumer.AngleRad % 6.2831855f;
					int[] array;
					if ((array = consumer.AxisSign) == null)
					{
						(array = new int[3])[2] = 1;
					}
					int[] axis = array;
					IStandardShaderProgram standardShaderProgram = this.capi.Render.PreparedStandardShader(this.blockEntity.Pos.X, this.blockEntity.Pos.Y, this.blockEntity.Pos.Z, null);
					standardShaderProgram.ProjectionMatrix = this.capi.Render.CurrentProjectionMatrix;
					standardShaderProgram.ViewMatrix = this.capi.Render.CameraMatrixOriginf;
					standardShaderProgram.ModelMatrix = new Matrixf().Identity().Translate((float)((double)this.blockEntity.Pos.X - camPos.X) + 0.5f, (float)((double)this.blockEntity.Pos.Y - camPos.Y) + 0.5f, (float)((double)this.blockEntity.Pos.Z - camPos.Z) + 0.5f).RotateX(angle * (float)axis[0]).RotateY(angle * (float)axis[1]).RotateZ(angle * (float)axis[2]).Translate(-0.5f, -0.5f, -0.5f).Values;
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
					this.capi.Render.RenderMesh(this.meshRef);
					standardShaderProgram.Stop();
					return;
				}
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00025ED8 File Offset: 0x000240D8
		private void BuildMesh()
		{
			BlockEntity blockEntity = this.blockEntity;
			if (((blockEntity != null) ? blockEntity.Block : null) == null)
			{
				return;
			}
			Shape shape = Shape.TryGet(this.capi, this.shapeLoc);
			if (shape == null)
			{
				return;
			}
			MeshData meshData;
			this.capi.Tesselator.TesselateShape(this.blockEntity.Block, shape, ref meshData, new Vec3f(0f, this.baseRotYDeg, 0f), default(int?), null);
			if (meshData != null && meshData.VerticesCount > 0)
			{
				this.meshRef = this.capi.Render.UploadMesh(meshData);
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00025F70 File Offset: 0x00024170
		public void Dispose()
		{
			MeshRef meshRef = this.meshRef;
			if (meshRef != null)
			{
				meshRef.Dispose();
			}
			this.meshRef = null;
			ICoreClientAPI coreClientAPI = this.capi;
			if (coreClientAPI != null)
			{
				coreClientAPI.Event.UnregisterRenderer(this, 1);
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x040002EE RID: 750
		private readonly ICoreClientAPI capi;

		// Token: 0x040002EF RID: 751
		private readonly BlockEntity blockEntity;

		// Token: 0x040002F0 RID: 752
		private readonly AssetLocation shapeLoc;

		// Token: 0x040002F1 RID: 753
		private readonly float baseRotYDeg;

		// Token: 0x040002F2 RID: 754
		private MeshRef meshRef;
	}
}
