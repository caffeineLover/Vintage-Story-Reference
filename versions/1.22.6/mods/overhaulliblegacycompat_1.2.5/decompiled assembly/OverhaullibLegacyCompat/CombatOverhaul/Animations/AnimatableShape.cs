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
	// Token: 0x02000198 RID: 408
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnimatableShape : ITexPositionSource, IDisposable
	{
		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x0006215C File Offset: 0x0006035C
		// (set) Token: 0x060013C5 RID: 5061 RVA: 0x00062163 File Offset: 0x00060363
		public static bool HadRenderErrorLastCall { get; private set; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060013C6 RID: 5062 RVA: 0x0006216B File Offset: 0x0006036B
		// (set) Token: 0x060013C7 RID: 5063 RVA: 0x00062173 File Offset: 0x00060373
		public Shape Shape { get; private set; }

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x0006217C File Offset: 0x0006037C
		// (set) Token: 0x060013C9 RID: 5065 RVA: 0x00062184 File Offset: 0x00060384
		public MultiTextureMeshRef MeshRef { get; private set; }

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x0006218D File Offset: 0x0006038D
		// (set) Token: 0x060013CB RID: 5067 RVA: 0x00062195 File Offset: 0x00060395
		public ITextureAtlasAPI Atlas { get; private set; }

		// Token: 0x060013CC RID: 5068 RVA: 0x000621A0 File Offset: 0x000603A0
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

		// Token: 0x060013CD RID: 5069 RVA: 0x00062220 File Offset: 0x00060420
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

		// Token: 0x060013CE RID: 5070 RVA: 0x000622B4 File Offset: 0x000604B4
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

		// Token: 0x060013CF RID: 5071 RVA: 0x00062328 File Offset: 0x00060528
		public void Dispose()
		{
			this.MeshRef.Dispose();
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00062338 File Offset: 0x00060538
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

		// Token: 0x060013D1 RID: 5073 RVA: 0x000623B0 File Offset: 0x000605B0
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

		// Token: 0x060013D2 RID: 5074 RVA: 0x00062444 File Offset: 0x00060644
		private static MeshData InitializeMeshData(ICoreClientAPI clientApi, string cacheDictKey, Shape shape, ITexPositionSource texSource)
		{
			shape.ResolveReferences(clientApi.World.Logger, cacheDictKey);
			AnimatableShape.CacheInvTransforms(shape.Elements);
			shape.ResolveAndFindJoints(clientApi.Logger, cacheDictKey, Array.Empty<string>());
			MeshData meshData;
			clientApi.Tesselator.TesselateShapeWithJointIds("collectible", shape, ref meshData, texSource, null, default(int?), null);
			return meshData.Clone();
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x000624A8 File Offset: 0x000606A8
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

		// Token: 0x060013D4 RID: 5076 RVA: 0x00062524 File Offset: 0x00060724
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

		// Token: 0x060013D5 RID: 5077 RVA: 0x00062558 File Offset: 0x00060758
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

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060013D6 RID: 5078 RVA: 0x000627AC File Offset: 0x000609AC
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

		// Token: 0x170004EA RID: 1258
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

		// Token: 0x060013D8 RID: 5080 RVA: 0x0006282C File Offset: 0x00060A2C
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

		// Token: 0x040009D6 RID: 2518
		private readonly ICoreClientAPI _clientApi;

		// Token: 0x040009D7 RID: 2519
		private readonly AnimatableShapeRenderer _renderer;

		// Token: 0x040009D8 RID: 2520
		private readonly Dictionary<long, AnimatorBase> _animators = new Dictionary<long, AnimatorBase>();

		// Token: 0x040009D9 RID: 2521
		private readonly Dictionary<long, string> _cacheKeys = new Dictionary<long, string>();

		// Token: 0x040009DA RID: 2522
		private readonly Item _item;

		// Token: 0x040009DB RID: 2523
		private readonly string _cachePrefix;

		// Token: 0x040009DC RID: 2524
		private static bool _reportedGenerateFramesError;
	}
}
