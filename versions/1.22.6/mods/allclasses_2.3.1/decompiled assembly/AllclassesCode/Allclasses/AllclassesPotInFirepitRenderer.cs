using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace Allclasses
{
	// Token: 0x02000008 RID: 8
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AllclassesPotInFirepitRenderer : IInFirepitRenderer, IRenderer, IDisposable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00003061 File Offset: 0x00001261
		public double RenderOrder
		{
			get
			{
				return 0.5;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000306C File Offset: 0x0000126C
		public int RenderRange
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003070 File Offset: 0x00001270
		public AllclassesPotInFirepitRenderer([Nullable(2)] ICoreClientAPI capi, ItemStack stack, BlockPos pos, bool isOutputSlot)
		{
			this.capi = capi;
			this.pos = pos;
			this.isOutputSlot = isOutputSlot;
			if (capi == null)
			{
				return;
			}
			MeshData mesh;
			capi.Tesselator.TesselateBlock(stack.Block, ref mesh);
			this.potRef = capi.Render.UploadMultiTextureMesh(mesh);
			if (isOutputSlot)
			{
				this.TryCreateMealMesh(stack);
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000030DC File Offset: 0x000012DC
		private bool TryCreateMealMesh(ItemStack stack)
		{
			if (this.capi == null)
			{
				return false;
			}
			BlockCookedContainer cookedBlock = this.capi.World.GetBlock(stack.Collectible.CodeWithVariant("type", "cooked")) as BlockCookedContainer;
			if (cookedBlock == null)
			{
				return false;
			}
			MealMeshCache meshCache = this.capi.ModLoader.GetModSystem<MealMeshCache>(true);
			this.potWithFoodRef = meshCache.GetOrCreateMealInContainerMeshRef(cookedBlock, cookedBlock.GetCookingRecipe(this.capi.World, stack), cookedBlock.GetNonEmptyContents(this.capi.World, stack), new Vec3f(0f, 0.15625f, 0f));
			return this.potWithFoodRef != null;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003184 File Offset: 0x00001384
		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			if (this.capi == null || this.disposed)
			{
				return;
			}
			MultiTextureMeshRef meshRef = this.potWithFoodRef ?? this.potRef;
			if (meshRef == null)
			{
				return;
			}
			IRenderAPI render = this.capi.Render;
			Vec3d cameraPos = this.capi.World.Player.Entity.CameraPos;
			render.GlDisableCullFace();
			render.GlToggleBlend(true, 0);
			IStandardShaderProgram standardShaderProgram = render.PreparedStandardShader(this.pos.X, this.pos.Y, this.pos.Z, null);
			standardShaderProgram.DontWarpVertices = 0;
			standardShaderProgram.AddRenderFlags = 0;
			standardShaderProgram.RgbaAmbientIn = render.AmbientColor;
			standardShaderProgram.RgbaFogIn = render.FogColor;
			standardShaderProgram.FogMinIn = render.FogMin;
			standardShaderProgram.FogDensityIn = render.FogDensity;
			standardShaderProgram.RgbaTint = ColorUtil.WhiteArgbVec;
			standardShaderProgram.NormalShaded = 1;
			standardShaderProgram.ExtraGodray = 0f;
			standardShaderProgram.SsaoAttn = 0f;
			standardShaderProgram.AlphaTest = 0.05f;
			standardShaderProgram.OverlayOpacity = 0f;
			standardShaderProgram.ModelMatrix = this.modelMat.Identity().Translate((double)this.pos.X - cameraPos.X + 0.001, (double)this.pos.Y - cameraPos.Y + 0.0625, (double)this.pos.Z - cameraPos.Z - 0.001).Values;
			standardShaderProgram.ViewMatrix = render.CameraMatrixOriginf;
			standardShaderProgram.ProjectionMatrix = render.CurrentProjectionMatrix;
			if (!this.TryRenderMesh(render, meshRef))
			{
				MultiTextureMeshRef fallbackRef = (meshRef == this.potWithFoodRef) ? this.potRef : null;
				if (fallbackRef != null)
				{
					this.TryRenderMesh(render, fallbackRef);
				}
			}
			standardShaderProgram.Stop();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003348 File Offset: 0x00001548
		private bool TryRenderMesh(IRenderAPI render, MultiTextureMeshRef meshRef)
		{
			bool result;
			try
			{
				render.RenderMultiTextureMesh(meshRef, "tex", 0);
				result = true;
			}
			catch (ArgumentException ex) when (ex.Message.Contains("disposed mesh", 5))
			{
				if (meshRef == this.potWithFoodRef)
				{
					this.potWithFoodRef = null;
				}
				if (meshRef == this.potRef)
				{
					this.potRef = null;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000033C0 File Offset: 0x000015C0
		public void OnUpdate(float temperature)
		{
			this.temp = temperature;
			float volume = this.isOutputSlot ? 0f : GameMath.Clamp((this.temp - 50f) / 50f, 0f, 1f);
			this.SetCookingSoundVolume(volume);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000340C File Offset: 0x0000160C
		public void OnCookingComplete()
		{
			this.SetCookingSoundVolume(0f);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000341C File Offset: 0x0000161C
		public void Dispose()
		{
			this.disposed = true;
			MultiTextureMeshRef multiTextureMeshRef = this.potRef;
			if (multiTextureMeshRef != null)
			{
				multiTextureMeshRef.Dispose();
			}
			this.potRef = null;
			this.potWithFoodRef = null;
			ILoadedSound loadedSound = this.cookingSound;
			if (loadedSound != null)
			{
				loadedSound.Stop();
			}
			ILoadedSound loadedSound2 = this.cookingSound;
			if (loadedSound2 != null)
			{
				loadedSound2.Dispose();
			}
			this.cookingSound = null;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003478 File Offset: 0x00001678
		private void SetCookingSoundVolume(float volume)
		{
			if (this.capi == null)
			{
				return;
			}
			if (volume > 0f)
			{
				if (this.cookingSound == null)
				{
					this.cookingSound = this.capi.World.LoadSound(new SoundParams
					{
						Location = new AssetLocation("sounds/effect/cooking.ogg"),
						ShouldLoop = true,
						Position = this.pos.ToVec3f().Add(0.5f, 0.25f, 0.5f),
						DisposeOnFinish = false,
						Range = 10f,
						ReferenceDistance = 3f,
						Volume = volume
					});
					this.cookingSound.Start();
					return;
				}
				this.cookingSound.SetVolume(volume);
				return;
			}
			else
			{
				if (this.cookingSound == null)
				{
					return;
				}
				this.cookingSound.Stop();
				this.cookingSound.Dispose();
				this.cookingSound = null;
				return;
			}
		}

		// Token: 0x0400000B RID: 11
		[Nullable(2)]
		private readonly ICoreClientAPI capi;

		// Token: 0x0400000C RID: 12
		private readonly BlockPos pos;

		// Token: 0x0400000D RID: 13
		private readonly Matrixf modelMat = new Matrixf();

		// Token: 0x0400000E RID: 14
		private readonly bool isOutputSlot;

		// Token: 0x0400000F RID: 15
		[Nullable(2)]
		private MultiTextureMeshRef potRef;

		// Token: 0x04000010 RID: 16
		[Nullable(2)]
		private MultiTextureMeshRef potWithFoodRef;

		// Token: 0x04000011 RID: 17
		[Nullable(2)]
		private ILoadedSound cookingSound;

		// Token: 0x04000012 RID: 18
		private bool disposed;

		// Token: 0x04000013 RID: 19
		private float temp;
	}
}
