using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x020001A6 RID: 422
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnimatableShape : ITexPositionSource, IDisposable
	{
		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x00063BDC File Offset: 0x00061DDC
		// (set) Token: 0x0600143D RID: 5181 RVA: 0x00063BE3 File Offset: 0x00061DE3
		public static bool HadRenderErrorLastCall { get; private set; }

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x00063BEB File Offset: 0x00061DEB
		// (set) Token: 0x0600143F RID: 5183 RVA: 0x00063BF3 File Offset: 0x00061DF3
		public Shape Shape { get; private set; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x00063BFC File Offset: 0x00061DFC
		// (set) Token: 0x06001441 RID: 5185 RVA: 0x00063C04 File Offset: 0x00061E04
		public MultiTextureMeshRef MeshRef { get; private set; }

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x00063C0D File Offset: 0x00061E0D
		// (set) Token: 0x06001443 RID: 5187 RVA: 0x00063C15 File Offset: 0x00061E15
		public ITextureAtlasAPI Atlas { get; private set; }

		// Token: 0x06001444 RID: 5188 RVA: 0x00063C20 File Offset: 0x00061E20
		[return: Nullable(2)]
		public static AnimatableShape Create(ICoreClientAPI api, string shapePath, Item item)
		{
			string cacheKey = "shapeEditorCollectibleMeshes-" + shapePath;
			AssetLocation shapeLocation = AssetLocation.Create(shapePath, item.Code.Domain);
			shapeLocation = shapeLocation.WithPathAppendixOnce(".json").WithPathPrefixOnce("shapes/");
			Shape currentShape = Shape.TryGet(api, shapeLocation);
			if (currentShape != null)
			{
				currentShape.ResolveReferences(api.Logger, cacheKey);
			}
			if (currentShape == null)
			{
				return null;
			}
			AnimatableShape animatableShape = new AnimatableShape(api, cacheKey, currentShape, item);
			animatableShape.Shape.ResolveReferences(api.Logger, "creating new animatable shape");
			return animatableShape;
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x00063CA0 File Offset: 0x00061EA0
		[NullableContext(2)]
		public AnimatorBase GetAnimator(long entityId)
		{
			if (this._animators.ContainsKey(entityId))
			{
				return this._animators[entityId];
			}
			this.RemoveAnimatorsForNonValidEntities();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted(this._cachePrefix);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted<long>(entityId);
			string cacheKey = defaultInterpolatedStringHandler.ToStringAndClear();
			AnimatorBase animator = AnimatableShape.GetAnimator(this._clientApi, cacheKey, this.Shape);
			if (animator == null)
			{
				return null;
			}
			this._animators.Add(entityId, animator);
			this._cacheKeys.Add(entityId, cacheKey);
			return animator;
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00063D34 File Offset: 0x00061F34
		public void Render(IShaderProgram shaderProgram, ItemRenderInfo itemStackRenderInfo, IRenderAPI render, ItemStack itemStack, Vec4f lightrgbs, Matrixf itemModelMat, Entity entity, float dt, [Nullable(2)] MultiTextureMeshRef meshOverride = null)
		{
			AnimatableShape.HadRenderErrorLastCall = false;
			try
			{
				this._renderer.Render(shaderProgram, itemStackRenderInfo, render, itemStack, lightrgbs, itemModelMat, entity, dt, meshOverride);
			}
			catch (Exception ex) when (ex is KeyNotFoundException || ex is InvalidOperationException)
			{
				AnimatableShape.HadRenderErrorLastCall = true;
			}
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x00063DA8 File Offset: 0x00061FA8
		public void Dispose()
		{
			this.MeshRef.Dispose();
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x00063DB8 File Offset: 0x00061FB8
		private AnimatableShape(ICoreClientAPI api, string cacheKey, Shape currentShape, Item item)
		{
			this._clientApi = api;
			this._cachePrefix = cacheKey;
			this.Shape = currentShape;
			this.Atlas = api.ItemTextureAtlas;
			this._item = item;
			MeshData meshData = AnimatableShape.InitializeMeshData(api, cacheKey, currentShape, this);
			this.MeshRef = AnimatableShape.InitializeMeshRef(api, meshData);
			this._renderer = new AnimatableShapeRenderer(api, this);
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x00063E30 File Offset: 0x00062030
		private void RemoveAnimatorsForNonValidEntities()
		{
			foreach (KeyValuePair<long, AnimatorBase> keyValuePair in this._animators)
			{
				long num;
				AnimatorBase animatorBase;
				keyValuePair.Deconstruct(ref num, ref animatorBase);
				long entityId = num;
				Entity entity = this._clientApi.World.GetEntityById(entityId);
				if (entity == null || !entity.Alive)
				{
					this._animators.Remove(entityId);
					this._cacheKeys.Remove(entityId);
				}
			}
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x00063EC4 File Offset: 0x000620C4
		private static MeshData InitializeMeshData(ICoreClientAPI clientApi, string cacheDictKey, Shape shape, ITexPositionSource texSource)
		{
			shape.ResolveReferences(clientApi.World.Logger, cacheDictKey);
			AnimatableShape.CacheInvTransforms(shape.Elements);
			shape.ResolveAndFindJoints(clientApi.Logger, cacheDictKey, Array.Empty<string>());
			MeshData meshData;
			clientApi.Tesselator.TesselateShapeWithJointIds("collectible", shape, ref meshData, texSource, null, default(int?), null);
			return meshData.Clone();
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x00063F28 File Offset: 0x00062128
		private static MultiTextureMeshRef InitializeMeshRef(ICoreClientAPI clientApi, MeshData meshData)
		{
			MultiTextureMeshRef meshRef = null;
			if (RuntimeEnv.MainThreadId == Environment.CurrentManagedThreadId)
			{
				meshRef = clientApi.Render.UploadMultiTextureMesh(meshData);
			}
			else
			{
				clientApi.Event.EnqueueMainThreadTask(delegate()
				{
					meshRef = clientApi.Render.UploadMultiTextureMesh(meshData);
				}, "uploadmesh");
			}
			return meshRef;
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x00063FA4 File Offset: 0x000621A4
		private static void CacheInvTransforms(ShapeElement[] elements)
		{
			if (elements == null)
			{
				return;
			}
			for (int i = 0; i < elements.Length; i++)
			{
				elements[i].CacheInverseTransformMatrix();
				AnimatableShape.CacheInvTransforms(elements[i].Children);
			}
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00063FD8 File Offset: 0x000621D8
		[return: Nullable(2)]
		private static AnimatorBase GetAnimator(ICoreClientAPI clientApi, string cacheDictKey, [Nullable(2)] Shape shape)
		{
			if (shape == null)
			{
				return null;
			}
			object animCacheObj;
			clientApi.ObjectCache.TryGetValue("proceduralAnimatorsCache", ref animCacheObj);
			Dictionary<string, AnimCacheEntry> animationCache = animCacheObj as Dictionary<string, AnimCacheEntry>;
			if (animationCache == null)
			{
				animationCache = (clientApi.ObjectCache["proceduralAnimatorsCache"] = new Dictionary<string, AnimCacheEntry>());
			}
			AnimCacheEntry cacheObj;
			AnimatorBase animator;
			if (animationCache.TryGetValue(cacheDictKey, ref cacheObj))
			{
				AnimatorBase animatorBase;
				if (clientApi.Side != 2)
				{
					animatorBase = new ServerAnimator(() => 1.0, cacheObj.RootPoses, cacheObj.Animations, cacheObj.RootElems, shape.JointsById, null, false);
				}
				else
				{
					animatorBase = new ClientItemAnimator(() => 1.0, cacheObj.RootPoses, cacheObj.Animations, cacheObj.RootElems, shape.JointsById, null, null);
				}
				animator = animatorBase;
			}
			else
			{
				int i = 0;
				while (shape.Animations != null && i < shape.Animations.Length)
				{
					try
					{
						shape.Animations[i].GenerateAllFrames(shape.Elements, shape.JointsById, true);
					}
					catch (Exception exception)
					{
						if (!AnimatableShape._reportedGenerateFramesError)
						{
							AnimatableShape._reportedGenerateFramesError = true;
							Type typeFromHandle = typeof(AnimatableShape);
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
							defaultInterpolatedStringHandler..ctor(48, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Error while generating animation frames for '");
							defaultInterpolatedStringHandler.AppendFormatted(cacheDictKey);
							defaultInterpolatedStringHandler.AppendLiteral("':\n");
							defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
							LoggerUtil.Warn(clientApi, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
					i++;
				}
				AnimatorBase animatorBase2;
				if (clientApi.Side != 2)
				{
					animatorBase2 = new ServerAnimator(() => 1.0, shape.Animations, shape.Elements, shape.JointsById, null, false);
				}
				else
				{
					animatorBase2 = new ClientItemAnimator(() => 1.0, shape.Animations, shape.Elements, shape.JointsById, null, null);
				}
				animator = animatorBase2;
				Dictionary<string, AnimCacheEntry> dictionary = animationCache;
				AnimCacheEntry animCacheEntry = new AnimCacheEntry();
				animCacheEntry.Animations = shape.Animations;
				ClientAnimator clientAnimator = animator as ClientAnimator;
				animCacheEntry.RootElems = ((clientAnimator != null) ? clientAnimator.RootElements : null);
				ClientAnimator clientAnimator2 = animator as ClientAnimator;
				animCacheEntry.RootPoses = ((clientAnimator2 != null) ? clientAnimator2.RootPoses : null);
				dictionary[cacheDictKey] = animCacheEntry;
			}
			return animator;
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x0600144E RID: 5198 RVA: 0x0006422C File Offset: 0x0006242C
		[Nullable(2)]
		public Size2i AtlasSize
		{
			[NullableContext(2)]
			get
			{
				ITextureAtlasAPI atlas = this.Atlas;
				if (atlas == null)
				{
					return null;
				}
				return atlas.Size;
			}
		}

		// Token: 0x1700050C RID: 1292
		[Nullable(2)]
		public TextureAtlasPosition this[string textureCode]
		{
			[return: Nullable(2)]
			get
			{
				AssetLocation texturePath = null;
				if (this._item.Textures.ContainsKey(textureCode))
				{
					texturePath = this._item.Textures[textureCode].Base;
				}
				else
				{
					Shape shape = this.Shape;
					if (shape != null)
					{
						shape.Textures.TryGetValue(textureCode, ref texturePath);
					}
				}
				if (texturePath == null)
				{
					texturePath = new AssetLocation(textureCode);
				}
				return this.GetOrCreateTexPos(texturePath);
			}
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x000642AC File Offset: 0x000624AC
		[return: Nullable(2)]
		private TextureAtlasPosition GetOrCreateTexPos(AssetLocation texturePath)
		{
			if (this.Atlas == null)
			{
				return null;
			}
			TextureAtlasPosition texturePosition = this.Atlas[texturePath];
			if (texturePosition == null)
			{
				if (this._clientApi.Assets.TryGet(texturePath.Clone().WithPathPrefixOnce("textures/").WithPathAppendixOnce(".png"), true) != null)
				{
					int num;
					this.Atlas.GetOrInsertTexture(texturePath, ref num, ref texturePosition, null, 0f);
				}
				else
				{
					ILogger logger = this._clientApi.World.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(56, 1);
					defaultInterpolatedStringHandler.AppendLiteral("[Animation Manager] texture ");
					defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(texturePath);
					defaultInterpolatedStringHandler.AppendLiteral(", not no such texture found.");
					logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			return texturePosition;
		}

		// Token: 0x040009FE RID: 2558
		private readonly ICoreClientAPI _clientApi;

		// Token: 0x040009FF RID: 2559
		private readonly AnimatableShapeRenderer _renderer;

		// Token: 0x04000A00 RID: 2560
		private readonly Dictionary<long, AnimatorBase> _animators = new Dictionary<long, AnimatorBase>();

		// Token: 0x04000A01 RID: 2561
		private readonly Dictionary<long, string> _cacheKeys = new Dictionary<long, string>();

		// Token: 0x04000A02 RID: 2562
		private readonly Item _item;

		// Token: 0x04000A03 RID: 2563
		private readonly string _cachePrefix;

		// Token: 0x04000A04 RID: 2564
		private static bool _reportedGenerateFramesError;
	}
}
