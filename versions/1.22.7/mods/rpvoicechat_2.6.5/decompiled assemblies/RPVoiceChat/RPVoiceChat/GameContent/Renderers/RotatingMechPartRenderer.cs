using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace RPVoiceChat.GameContent.Renderers
{
	// Token: 0x020000F5 RID: 245
	public class RotatingMechPartRenderer : IRenderer, IDisposable
	{
		// Token: 0x060008CA RID: 2250 RVA: 0x0003111C File Offset: 0x0002F31C
		public RotatingMechPartRenderer(BlockEntity blockEntity, ICoreClientAPI capi, AssetLocation shapeLoc, float baseRotYDeg)
		{
			this.blockEntity = blockEntity;
			this.capi = capi;
			this.shapeLoc = shapeLoc;
			this.baseRotYDeg = baseRotYDeg;
			this.BuildMesh();
			capi.Event.RegisterRenderer(this, 1, "rpvc-rotatingmechpart");
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00031159 File Offset: 0x0002F359
		public double RenderOrder
		{
			get
			{
				return 0.55;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x00031164 File Offset: 0x0002F364
		public int RenderRange
		{
			get
			{
				return 80;
			}
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00031168 File Offset: 0x0002F368
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

		// Token: 0x060008CE RID: 2254 RVA: 0x0003136C File Offset: 0x0002F56C
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

		// Token: 0x060008CF RID: 2255 RVA: 0x00031404 File Offset: 0x0002F604
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

		// Token: 0x040003CD RID: 973
		private readonly ICoreClientAPI capi;

		// Token: 0x040003CE RID: 974
		private readonly BlockEntity blockEntity;

		// Token: 0x040003CF RID: 975
		private readonly AssetLocation shapeLoc;

		// Token: 0x040003D0 RID: 976
		private readonly float baseRotYDeg;

		// Token: 0x040003D1 RID: 977
		private MeshRef meshRef;
	}
}
