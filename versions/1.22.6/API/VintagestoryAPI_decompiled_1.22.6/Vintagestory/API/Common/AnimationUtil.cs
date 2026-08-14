using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common;

public class AnimationUtil : IRenderer, IDisposable
{
	public AnimatorBase animator;

	public AnimatableRenderer renderer;

	public Dictionary<string, AnimationMetaData> activeAnimationsByAnimCode = new Dictionary<string, AnimationMetaData>();

	protected ICoreAPI api;

	protected bool stopRenderTriggered;

	protected ICoreClientAPI capi;

	protected Vec3d position;

	protected Vec3f soundPosition;

	protected int dimension;

	private Dictionary<string, ILoadedSound> loopingSounds;

	[Obsolete]
	protected virtual int RenderTextureId => capi.BlockTextureAtlas.AtlasTextures[0].TextureId;

	public double RenderOrder => 1.0;

	public int RenderRange => 99;

	public AnimationUtil(ICoreAPI api, Vec3d position, int dimension = 0)
	{
		this.api = api;
		this.position = position;
		this.dimension = dimension;
		soundPosition = position.ToVec3f().Add(0.5f, 0.5f, 0.5f);
		capi = api as ICoreClientAPI;
		(api as ICoreClientAPI)?.Event.RegisterRenderer(this, EnumRenderStage.Opaque, "beanimutil");
	}

	public virtual void InitializeShapeAndAnimator(string cacheDictKey, Shape shape, ITexPositionSource texSource, Vec3f rotation, out MeshData meshdata)
	{
		Dictionary<string, ShapeElement> elementsByName = shape.CollectAndResolveReferences(api.World.Logger, cacheDictKey);
		shape.CacheInvTransforms();
		shape.ResolveAndFindJoints(api.World.Logger, cacheDictKey, elementsByName);
		TesselationMetaData meta = new TesselationMetaData
		{
			TexSource = texSource,
			WithJointIds = true,
			WithDamageEffect = true,
			TypeForLogging = cacheDictKey,
			Rotation = rotation
		};
		capi.Tesselator.TesselateShape(meta, shape, out var mesh);
		meshdata = mesh;
		InitializeAnimator(cacheDictKey, meshdata, shape, rotation);
	}

	public virtual void InitializeAnimator(string cacheDictKey, MeshData meshdata, Shape shape, Vec3f rotation, EnumRenderStage renderStage = EnumRenderStage.Opaque)
	{
		if (meshdata == null)
		{
			throw new ArgumentException("meshdata cannot be null");
		}
		animator = GetAnimator(api, cacheDictKey, shape, ShouldPlaySound);
		renderer?.Dispose();
		renderer = new AnimatableRenderer(capi, position, rotation, animator, activeAnimationsByAnimCode, meshdata, renderStage);
	}

	public virtual void InitializeAnimatorServer(string cacheDictKey, Shape blockShape)
	{
		animator = GetAnimator(api, cacheDictKey, blockShape);
	}

	public void AnimationTickServer(float deltaTime)
	{
		if (animator != null && (activeAnimationsByAnimCode.Count > 0 || animator.ActiveAnimationCount > 0))
		{
			animator.OnFrame(activeAnimationsByAnimCode, deltaTime);
		}
	}

	public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
	{
		if (animator == null || renderer == null)
		{
			return;
		}
		ICoreClientAPI obj = api as ICoreClientAPI;
		if (obj == null || !obj.IsGamePaused)
		{
			if (activeAnimationsByAnimCode.Count > 0 || animator.ActiveAnimationCount > 0)
			{
				animator.OnFrame(activeAnimationsByAnimCode, deltaTime);
			}
			if (activeAnimationsByAnimCode.Count == 0 && animator.ActiveAnimationCount == 0 && renderer.ShouldRender && !stopRenderTriggered)
			{
				stopRenderTriggered = true;
				OnAnimationsStateChange(animsNowActive: false);
			}
		}
	}

	public virtual bool StartAnimation(AnimationMetaData meta)
	{
		if (!activeAnimationsByAnimCode.ContainsKey(meta.Code))
		{
			stopRenderTriggered = false;
			activeAnimationsByAnimCode[meta.Code] = meta;
			OnAnimationsStateChange(animsNowActive: true);
			return true;
		}
		return false;
	}

	protected virtual void OnAnimationsStateChange(bool animsNowActive)
	{
	}

	public void StopAnimation(string code)
	{
		activeAnimationsByAnimCode.Remove(code);
	}

	public static AnimatorBase GetAnimator(ICoreAPI api, string cacheDictKey, Shape shape, Action<string, AnimationSound> onShouldPlaySoundListener = null)
	{
		if (shape == null)
		{
			return null;
		}
		cacheDictKey = "animutil-" + cacheDictKey;
		Dictionary<string, AnimCacheEntry> animCache = null;
		api.ObjectCache.TryGetValue("animUtil-animCache", out var animCacheObj);
		animCache = animCacheObj as Dictionary<string, AnimCacheEntry>;
		if (animCache == null)
		{
			animCache = (Dictionary<string, AnimCacheEntry>)(api.ObjectCache["animUtil-animCache"] = new Dictionary<string, AnimCacheEntry>());
		}
		AnimatorBase animator;
		if (animCache.TryGetValue(cacheDictKey, out var cacheObj))
		{
			animator = ((api.Side == EnumAppSide.Client) ? new ClientAnimator(() => 1.0, cacheObj.RootPoses, cacheObj.Animations, cacheObj.RootElems, shape.JointsById, null, onShouldPlaySoundListener) : new ServerAnimator(() => 1.0, cacheObj.RootPoses, cacheObj.Animations, cacheObj.RootElems, shape.JointsById));
		}
		else
		{
			Animation[] Animations = shape.Animations;
			if (Animations != null)
			{
				for (int i = 0; i < Animations.Length; i++)
				{
					Animations[i].GenerateAllFrames(shape.Elements, shape.JointsById);
				}
			}
			animator = ((api.Side == EnumAppSide.Client) ? new ClientAnimator(() => 1.0, shape.Animations, shape.Elements, shape.JointsById, null, onShouldPlaySoundListener) : new ServerAnimator(() => 1.0, shape.Animations, shape.Elements, shape.JointsById));
			animCache[cacheDictKey] = new AnimCacheEntry
			{
				Animations = shape.Animations,
				RootElems = (animator as ClientAnimator).RootElements,
				RootPoses = (animator as ClientAnimator).RootPoses
			};
		}
		return animator;
	}

	public void Dispose()
	{
		renderer?.Dispose();
		(api as ICoreClientAPI)?.Event.UnregisterRenderer(this, EnumRenderStage.Opaque);
	}

	public void ShouldPlaySound(string animationMetaCode, AnimationSound sound)
	{
		AnimationManager.ShouldPlaySound(api.World, soundPosition, dimension, loopingSounds, animationMetaCode, sound);
	}
}
