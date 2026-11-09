using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Integration.Transpilers;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A7 RID: 423
	[NullableContext(1)]
	[Nullable(0)]
	internal class AnimatableShapeRenderer
	{
		// Token: 0x06001451 RID: 5201 RVA: 0x00064363 File Offset: 0x00062563
		public AnimatableShapeRenderer(ICoreClientAPI api, AnimatableShape shape)
		{
			this._clientApi = api;
			this._shape = shape;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0006437C File Offset: 0x0006257C
		public void Render(IShaderProgram shaderProgram, ItemRenderInfo itemStackRenderInfo, IRenderAPI render, ItemStack itemStack, Vec4f lightrgbs, Matrixf itemModelMat, Entity entity, float dt, [Nullable(2)] MultiTextureMeshRef meshOverride = null)
		{
			AnimatableShapeRenderer.RenderAnimatableShape(shaderProgram, this._clientApi.World, this._shape, itemStackRenderInfo, render, itemStack, entity, lightrgbs, itemModelMat, meshOverride);
			AnimatableShapeRenderer.SpawnParticles(itemModelMat, itemStack, dt, ref this._timeAccumulation, this._clientApi, entity);
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x000643C8 File Offset: 0x000625C8
		private static void RenderAnimatableShape(IShaderProgram shaderProgram, IWorldAccessor world, AnimatableShape shape, ItemRenderInfo itemStackRenderInfo, IRenderAPI render, ItemStack itemStack, Entity entity, Vec4f lightrgbs, Matrixf itemModelMat, [Nullable(2)] MultiTextureMeshRef meshOverride = null)
		{
			string textureSampleName = "tex";
			shaderProgram.Use();
			AnimatorBase animator = shape.GetAnimator(entity.EntityId);
			if (animator == null)
			{
				shaderProgram.Stop();
				return;
			}
			AnimatableShapeRenderer.FillShaderValues(shaderProgram, itemStackRenderInfo, render, itemStack, lightrgbs, itemModelMat, world, animator);
			if (!itemStackRenderInfo.CullFaces)
			{
				render.GlDisableCullFace();
			}
			render.RenderMultiTextureMesh(meshOverride ?? shape.MeshRef, textureSampleName, 0);
			if (!itemStackRenderInfo.CullFaces)
			{
				render.GlEnableCullFace();
			}
			shaderProgram.Uniform("damageEffect", 0f);
			shaderProgram.Stop();
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x00064454 File Offset: 0x00062654
		private static void SpawnParticles(Matrixf itemModelMat, ItemStack itemStack, float dt, ref float timeAccumulation, ICoreClientAPI api, Entity entity)
		{
			CollectibleObject collectible = itemStack.Collectible;
			if (((collectible != null) ? collectible.ParticleProperties : null) == null)
			{
				return;
			}
			float windStrength = Math.Max(0f, 1f - (float)api.World.BlockAccessor.GetDistanceToRainFall(entity.Pos.AsBlockPos, 4, 1) / 5f);
			AdvancedParticleProperties[] particleProperties = itemStack.Collectible.ParticleProperties;
			if (itemStack.Collectible == null || api.IsGamePaused)
			{
				return;
			}
			EntityPlayer entityPlayer = api.World.Player.Entity;
			Vec4f vec4f = itemModelMat.TransformVector(new Vec4f(itemStack.Collectible.TopMiddlePos.X, itemStack.Collectible.TopMiddlePos.Y, itemStack.Collectible.TopMiddlePos.Z, 1f));
			timeAccumulation += dt;
			if (particleProperties != null && particleProperties.Length != 0 && timeAccumulation > 0.05f)
			{
				timeAccumulation %= 0.025f;
				foreach (AdvancedParticleProperties advancedParticleProperties in particleProperties)
				{
					advancedParticleProperties.WindAffectednesAtPos = windStrength;
					advancedParticleProperties.WindAffectednes = windStrength;
					advancedParticleProperties.basePos.X = (double)vec4f.X + entity.Pos.X + (0.0 - (entity.Pos.X - entityPlayer.CameraPos.X));
					advancedParticleProperties.basePos.Y = (double)vec4f.Y + entity.Pos.Y + (0.0 - (entity.Pos.Y - entityPlayer.CameraPos.Y));
					advancedParticleProperties.basePos.Z = (double)vec4f.Z + entity.Pos.Z + (0.0 - (entity.Pos.Z - entityPlayer.CameraPos.Z));
					entity.World.SpawnParticles(advancedParticleProperties, null);
				}
			}
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x00064654 File Offset: 0x00062854
		private static void FillShaderValues(IShaderProgram shaderProgram, ItemRenderInfo itemStackRenderInfo, IRenderAPI render, ItemStack itemStack, Vec4f lightrgbs, Matrixf itemModelMatrix, IWorldAccessor world, AnimatorBase animator)
		{
			AnimatableShapeRenderer.FillShaderValues(shaderProgram, itemStackRenderInfo, render, itemStack, lightrgbs, itemModelMatrix, world);
			float[] array;
			if ((array = AnimatableShapeRenderer._elementTransformScratch) == null)
			{
				array = (AnimatableShapeRenderer._elementTransformScratch = new float[GlobalConstants.MaxAnimatedElements * 12]);
			}
			float[] elementTransforms = array;
			float[] transformationMatrices = animator.TransformationMatrices;
			int writeIndex = 0;
			bool allZero = true;
			int index = 0;
			while (index < transformationMatrices.Length && writeIndex < elementTransforms.Length)
			{
				if (index % 4 != 3)
				{
					float value = transformationMatrices[index];
					elementTransforms[writeIndex++] = value;
					allZero &= (value == 0f);
				}
				index++;
			}
			if (allZero)
			{
				for (int index2 = 0; index2 < writeIndex; index2 += 4)
				{
					elementTransforms[index2] = 1f;
				}
			}
			int animatedElementsCount = Math.Min(writeIndex / 12, GlobalConstants.MaxAnimatedElements);
			shaderProgram.UniformMatrices4x3("elementTransforms", animatedElementsCount, elementTransforms);
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x00064710 File Offset: 0x00062910
		private static void FillShaderValues(IShaderProgram shaderProgram, ItemRenderInfo itemStackRenderInfo, IRenderAPI render, ItemStack itemStack, Vec4f lightrgbs, Matrixf itemModelMatrix, IWorldAccessor world)
		{
			shaderProgram.Uniform("dontWarpVertices", 0);
			shaderProgram.Uniform("addRenderFlags", 0);
			shaderProgram.Uniform("normalShaded", 1);
			shaderProgram.Uniform("rgbaTint", ColorUtil.WhiteArgbVec);
			shaderProgram.Uniform("alphaTest", itemStackRenderInfo.AlphaTest);
			shaderProgram.Uniform("damageEffect", itemStackRenderInfo.DamageEffect);
			shaderProgram.Uniform("overlayOpacity", itemStackRenderInfo.OverlayOpacity);
			if (itemStackRenderInfo.OverlayTexture != null && itemStackRenderInfo.OverlayOpacity > 0f)
			{
				shaderProgram.Uniform("tex2dOverlay", itemStackRenderInfo.OverlayTexture.TextureId);
				shaderProgram.Uniform("overlayTextureSize", new Vec2f((float)itemStackRenderInfo.OverlayTexture.Width, (float)itemStackRenderInfo.OverlayTexture.Height));
				shaderProgram.Uniform("baseTextureSize", new Vec2f((float)itemStackRenderInfo.TextureSize.Width, (float)itemStackRenderInfo.TextureSize.Height));
				TextureAtlasPosition textureAtlasPosition = render.GetTextureAtlasPosition(itemStack);
				shaderProgram.Uniform("baseUvOrigin", new Vec2f(textureAtlasPosition.x1, textureAtlasPosition.y1));
			}
			int num3 = (int)itemStack.Collectible.GetTemperature(world, itemStack);
			float[] incandescenceColorAsColor4f = ColorUtil.GetIncandescenceColorAsColor4f(num3);
			int num2 = GameMath.Clamp((num3 - 500) / 3, 0, 255);
			shaderProgram.Uniform("extraGlow", num2);
			shaderProgram.Uniform("rgbaAmbientIn", render.AmbientColor);
			shaderProgram.Uniform("rgbaLightIn", lightrgbs);
			shaderProgram.Uniform("rgbaGlowIn", new Vec4f(incandescenceColorAsColor4f[0], incandescenceColorAsColor4f[1], incandescenceColorAsColor4f[2], (float)num2 / 255f));
			shaderProgram.Uniform("rgbaFogIn", render.FogColor);
			shaderProgram.Uniform("fogMinIn", render.FogMin);
			shaderProgram.Uniform("fogDensityIn", render.FogDensity);
			shaderProgram.Uniform("normalShaded", (itemStackRenderInfo.NormalShaded > false) ? 1 : 0);
			shaderProgram.UniformMatrix("projectionMatrix", render.CurrentProjectionMatrix);
			shaderProgram.UniformMatrix("viewMatrix", render.CameraMatrixOriginf);
			shaderProgram.UniformMatrix("modelMatrix", itemModelMatrix.Values);
			shaderProgram.Uniform("depthOffset", PlayerRenderingPatches.FpHandsOffset);
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x00064929 File Offset: 0x00062B29
		private static float GetDepthOffset(IWorldAccessor world)
		{
			ICoreClientAPI coreClientAPI = world.Api as ICoreClientAPI;
			if (coreClientAPI == null || !coreClientAPI.Settings.Bool["immersiveFpMode"])
			{
				return -0.3f;
			}
			return 0f;
		}

		// Token: 0x04000A05 RID: 2565
		private float _timeAccumulation;

		// Token: 0x04000A06 RID: 2566
		private readonly ICoreClientAPI _clientApi;

		// Token: 0x04000A07 RID: 2567
		private readonly AnimatableShape _shape;

		// Token: 0x04000A08 RID: 2568
		[Nullable(2)]
		[ThreadStatic]
		private static float[] _elementTransformScratch;
	}
}
