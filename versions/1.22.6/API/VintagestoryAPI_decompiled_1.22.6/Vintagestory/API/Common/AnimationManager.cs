using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common;

public class AnimationManager : IAnimationManager, IDisposable
{
	protected ICoreAPI api;

	protected ICoreClientAPI capi;

	[ThreadStatic]
	private static FastMemoryStream reusableStream;

	/// <summary>
	/// The list of currently active animations that should be playing
	/// </summary>
	public Dictionary<string, AnimationMetaData> ActiveAnimationsByAnimCode = new Dictionary<string, AnimationMetaData>(StringComparer.OrdinalIgnoreCase);

	public List<AnimFrameCallback> Triggers;

	/// <summary>
	/// The entity attached to this Animation Manager.
	/// </summary>
	protected Entity entity;

	public bool RunWhilePaused;

	private Dictionary<string, ILoadedSound> loopingSounds;

	/// <summary>
	/// Are the animations dirty in this AnimationManager?
	/// </summary>
	public bool AnimationsDirty { get; set; }

	/// <summary>
	/// The animator for the animation manager.
	/// </summary>
	public IAnimator Animator { get; set; }

	/// <summary>
	/// The entity head controller for this animator.
	/// </summary>
	public IHeadController HeadController { get; set; }

	Dictionary<string, AnimationMetaData> IAnimationManager.ActiveAnimationsByAnimCode => ActiveAnimationsByAnimCode;

	public bool AdjustCollisionBoxToAnimation { get; set; }

	public event StartAnimationDelegate OnStartAnimation;

	public event StartAnimationDelegate OnAnimationReceived;

	public event Action<string> OnAnimationStopped;

	/// <summary>
	/// Initializes the Animation Manager.
	/// </summary>
	/// <param name="api">The Core API.</param>
	/// <param name="entity">The entity this manager is attached to.</param>
	public virtual void Init(ICoreAPI api, Entity entity)
	{
		this.api = api;
		this.entity = entity;
		capi = api as ICoreClientAPI;
	}

	public IAnimator LoadAnimator(ICoreAPI api, Entity entity, Shape entityShape, RunningAnimation[] copyOverAnims, bool requirePosesOnServer, string[] disableElements, params string[] requireJointsForElements)
	{
		Init(entity.Api, entity);
		if (entityShape == null)
		{
			return null;
		}
		JsonObject attributes = entity.Properties.Attributes;
		if (attributes != null && attributes["requireJointsForElements"].Exists)
		{
			requireJointsForElements = requireJointsForElements.Append<string>(entity.Properties.Attributes["requireJointsForElements"].AsArray<string>());
		}
		entityShape.InitForAnimations(api.Logger, entity.Properties.Client.ShapeForEntity.Base.ToString(), disableElements, requireJointsForElements);
		IAnimator animator = (Animator = ((api.Side == EnumAppSide.Client) ? ClientAnimator.CreateForEntity(entity, entityShape.Animations, entityShape.Elements, entityShape.JointsById) : ServerAnimator.CreateForEntity(entity, entityShape.Animations, entityShape.Elements, entityShape.JointsById, requirePosesOnServer)));
		CopyOverAnimStates(copyOverAnims, animator);
		return animator;
	}

	public void CopyOverAnimStates(RunningAnimation[] copyOverAnims, IAnimator animator)
	{
		if (copyOverAnims == null || animator == null)
		{
			return;
		}
		foreach (RunningAnimation sourceAnim in copyOverAnims)
		{
			if (sourceAnim != null && sourceAnim.Active)
			{
				ActiveAnimationsByAnimCode.TryGetValue(sourceAnim.Animation.Code, out var meta);
				if (meta != null)
				{
					meta.StartFrameOnce = sourceAnim.CurrentFrame;
				}
			}
		}
	}

	public virtual bool IsAnimationActive(params string[] anims)
	{
		foreach (string val in anims)
		{
			if (ActiveAnimationsByAnimCode.ContainsKey(val))
			{
				return true;
			}
		}
		return false;
	}

	public virtual RunningAnimation GetAnimationState(string anim)
	{
		return Animator.GetAnimationState(anim);
	}

	/// <summary>
	/// If given animation is running, will set its progress to the first animation frame
	/// </summary>
	/// <param name="animCode"></param>
	public virtual void ResetAnimation(string animCode)
	{
		if (animCode != null)
		{
			RunningAnimation state = Animator?.GetAnimationState(animCode);
			if (state != null)
			{
				state.CurrentFrame = 0f;
				state.Iterations = 0;
			}
		}
	}

	/// <summary>
	/// As StartAnimation, except that it does not attempt to start the animation if the named animation is non-existent for this entity
	/// </summary>
	/// <param name="animdata"></param>
	public virtual bool TryStartAnimation(AnimationMetaData animdata)
	{
		if (((AnimatorBase)Animator).GetAnimationState(animdata.Animation) == null)
		{
			return false;
		}
		return StartAnimation(animdata);
	}

	/// <summary>
	/// Client: Starts given animation
	/// Server: Sends all active anims to all connected clients then purges the ActiveAnimationsByAnimCode list
	/// </summary>
	/// <param name="animdata"></param>
	public virtual bool StartAnimation(AnimationMetaData animdata)
	{
		if (animdata == null)
		{
			throw new Exception("Can't play null animdata");
		}
		if (this.OnStartAnimation != null)
		{
			EnumHandling handling = EnumHandling.PassThrough;
			bool preventDefault = false;
			bool started = false;
			Delegate[] invocationList = this.OnStartAnimation.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				StartAnimationDelegate dele = (StartAnimationDelegate)invocationList[i];
				started = dele(ref animdata, ref handling);
				if (handling == EnumHandling.PreventSubsequent)
				{
					return started;
				}
				if (animdata == null)
				{
					throw new Exception($"A StartAnimationDelegate {dele.Method.DeclaringType?.FullName}.{dele.Method.Name} changed the animation data to null. If the intention was to cancel the animation, use EnumHandling.PreventSubsequent instead.");
				}
				preventDefault = handling == EnumHandling.PreventDefault;
			}
			if (preventDefault)
			{
				return started;
			}
		}
		if (ActiveAnimationsByAnimCode.TryGetValue(animdata.Animation, out var activeAnimdata) && activeAnimdata == animdata)
		{
			return false;
		}
		if (animdata.Code == null)
		{
			throw new Exception("anim meta data code cannot be null!");
		}
		AnimationsDirty = true;
		ActiveAnimationsByAnimCode[animdata.Animation] = animdata;
		entity?.UpdateDebugAttributes();
		return true;
	}

	/// <summary>
	/// Start a new animation defined in the entity config file. If it's not defined, it won't play.
	/// Use StartAnimation(AnimationMetaData animdata) to circumvent the entity config anim data.
	/// </summary>
	/// <param name="configCode">Anim config code, not the animation code!</param>
	public virtual bool StartAnimation(string configCode)
	{
		if (configCode == null)
		{
			return false;
		}
		if (entity.Properties.Client.AnimationsByMetaCode.TryGetValue(configCode, out var animdata))
		{
			StartAnimation(animdata);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Stops given animation
	/// </summary>
	/// <param name="code"></param>
	public virtual void StopAnimation(string code)
	{
		if (code == null || entity == null)
		{
			return;
		}
		if (entity.World.Side == EnumAppSide.Server)
		{
			AnimationsDirty = true;
		}
		if (!ActiveAnimationsByAnimCode.Remove(code) && ActiveAnimationsByAnimCode.Count > 0)
		{
			foreach (KeyValuePair<string, AnimationMetaData> val in ActiveAnimationsByAnimCode)
			{
				if (val.Value.Code == code)
				{
					ActiveAnimationsByAnimCode.Remove(val.Key);
					break;
				}
			}
		}
		if (entity.World.EntityDebugMode)
		{
			entity.UpdateDebugAttributes();
		}
	}

	public virtual void StopAllAnimations()
	{
		if (entity != null)
		{
			if (entity.World.Side == EnumAppSide.Server)
			{
				AnimationsDirty = true;
			}
			ActiveAnimationsByAnimCode.Clear();
		}
	}

	/// <summary>
	/// The event fired when the manager recieves the server animations.
	/// </summary>
	/// <param name="activeAnimations"></param>
	/// <param name="activeAnimationsCount"></param>
	/// <param name="activeAnimationSpeeds"></param>
	public virtual void OnReceivedServerAnimations(int[] activeAnimations, int activeAnimationsCount, float[] activeAnimationSpeeds)
	{
		HashSet<string> toKeep = new HashSet<string>();
		string active = "";
		for (int i = 0; i < activeAnimationsCount; i++)
		{
			uint crc32 = (uint)activeAnimations[i];
			if (entity.Properties.Client.AnimationsByCrc32.TryGetValue(crc32, out var animmetadata))
			{
				toKeep.Add(animmetadata.Animation);
				if (!ActiveAnimationsByAnimCode.ContainsKey(animmetadata.Code))
				{
					animmetadata.AnimationSpeed = activeAnimationSpeeds[i];
					onReceivedServerAnimation(animmetadata);
				}
			}
			else
			{
				if (!entity.Properties.Client.LoadedShapeForEntity.AnimationsByCrc32.TryGetValue(crc32, out var anim))
				{
					continue;
				}
				toKeep.Add(anim.Code);
				if (!ActiveAnimationsByAnimCode.ContainsKey(anim.Code))
				{
					string code = ((anim.Code == null) ? anim.Name.ToLowerInvariant() : anim.Code);
					active = active + ", " + code;
					entity.Properties.Client.AnimationsByMetaCode.TryGetValue(code, out var animmeta);
					if (animmeta == null)
					{
						animmeta = new AnimationMetaData
						{
							Code = code,
							Animation = code,
							CodeCrc32 = anim.CodeCrc32
						};
					}
					animmeta.AnimationSpeed = activeAnimationSpeeds[i];
					onReceivedServerAnimation(animmeta);
				}
			}
		}
		if (entity.EntityId == (entity.World as IClientWorldAccessor).Player.Entity.EntityId)
		{
			return;
		}
		string[] keys = ActiveAnimationsByAnimCode.Keys.ToArray();
		foreach (string key in keys)
		{
			AnimationMetaData animMeta = ActiveAnimationsByAnimCode[key];
			if (!toKeep.Contains(key) && !animMeta.ClientSide && (!entity.Properties.Client.AnimationsByMetaCode.TryGetValue(key, out var animmetadata2) || animmetadata2.TriggeredBy == null || !animmetadata2.WasStartedFromTrigger))
			{
				ActiveAnimationsByAnimCode.Remove(key);
			}
		}
	}

	protected virtual void onReceivedServerAnimation(AnimationMetaData animmetadata)
	{
		EnumHandling handling = EnumHandling.PassThrough;
		this.OnAnimationReceived?.Invoke(ref animmetadata, ref handling);
		if (handling == EnumHandling.PassThrough)
		{
			ActiveAnimationsByAnimCode[animmetadata.Animation] = animmetadata;
		}
	}

	/// <summary>
	/// Serializes the animations to be stored in the SaveGame
	/// </summary>
	/// <param name="tree"></param>
	/// <param name="forClient"></param>
	public virtual void ToAttributes(ITreeAttribute tree, bool forClient)
	{
		if (Animator != null)
		{
			ITreeAttribute animtree = new TreeAttribute();
			tree["activeAnims"] = animtree;
			SerializeActiveAnimations(forClient, delegate(string code, FastMemoryStream ms)
			{
				animtree[code] = new ByteArrayAttribute(ms);
			});
		}
	}

	/// <summary>
	/// For performance, serializes the animations to be stored directly to the provided stream
	/// </summary>
	/// <param name="stream"></param>
	/// <param name="forClient"></param>
	public virtual void ToAttributeBytes(BinaryWriter stream, bool forClient)
	{
		if (Animator != null)
		{
			StreamedTreeAttribute streamedAnimTree = new StreamedTreeAttribute(stream);
			streamedAnimTree.WithKey("activeAnims");
			SerializeActiveAnimations(forClient, delegate(string code, FastMemoryStream ms)
			{
				streamedAnimTree[code] = new StreamedByteArrayAttribute(ms);
			});
			streamedAnimTree.EndKey();
		}
	}

	protected virtual void SerializeActiveAnimations(bool forClient, Action<string, FastMemoryStream> output)
	{
		if (ActiveAnimationsByAnimCode.Count == 0)
		{
			return;
		}
		using FastMemoryStream ms = reusableStream ?? (reusableStream = new FastMemoryStream());
		using BinaryWriter writer = new BinaryWriter(ms);
		foreach (KeyValuePair<string, AnimationMetaData> val in ActiveAnimationsByAnimCode)
		{
			if (val.Value.Code == null)
			{
				val.Value.Code = val.Key;
			}
			if (forClient || !(val.Value.Code != "die"))
			{
				RunningAnimation anim = Animator.GetAnimationState(val.Value.Animation);
				if (anim != null)
				{
					val.Value.StartFrameOnce = anim.CurrentFrame;
				}
				ms.Reset();
				val.Value.ToBytes(writer);
				val.Value.StartFrameOnce = 0f;
				output(val.Key, ms);
			}
		}
	}

	/// <summary>
	/// Loads the entity from a stored byte array from the SaveGame
	/// </summary>
	/// <param name="tree"></param>
	/// <param name="version"></param>
	public virtual void FromAttributes(ITreeAttribute tree, string version)
	{
		if (!(tree["activeAnims"] is ITreeAttribute animtree))
		{
			return;
		}
		foreach (KeyValuePair<string, IAttribute> val in animtree)
		{
			using MemoryStream ms = new MemoryStream((val.Value as ByteArrayAttribute).value);
			using BinaryReader reader = new BinaryReader(ms);
			ActiveAnimationsByAnimCode[val.Key] = AnimationMetaData.FromBytes(reader, version);
		}
	}

	/// <summary>
	/// The event fired at each server tick.
	/// </summary>
	/// <param name="dt"></param>
	public virtual void OnServerTick(float dt)
	{
		if (Animator != null)
		{
			Animator.CalculateMatrices = !entity.Alive || entity.requirePosesOnServer;
			Animator.OnFrame(ActiveAnimationsByAnimCode, dt);
			AdjustCollisionBoxToAnimation = ActiveAnimationsByAnimCode.Any((KeyValuePair<string, AnimationMetaData> anim) => anim.Value.AdjustCollisionBox);
		}
		runTriggers();
	}

	/// <summary>
	/// The event fired each time the client ticks.
	/// </summary>
	/// <param name="dt"></param>
	public virtual void OnClientFrame(float dt)
	{
		if ((!RunWhilePaused && capi.IsGamePaused) || Animator == null)
		{
			return;
		}
		if (HeadController != null)
		{
			HeadController.OnFrame(dt);
		}
		if (entity.IsRendered || entity.IsShadowRendered || !entity.Alive)
		{
			Animator.OnFrame(ActiveAnimationsByAnimCode, dt);
			AdjustCollisionBoxToAnimation = ActiveAnimationsByAnimCode.Any((KeyValuePair<string, AnimationMetaData> anim) => anim.Value.AdjustCollisionBox);
			runTriggers();
		}
		if (loopingSounds == null)
		{
			return;
		}
		foreach (KeyValuePair<string, ILoadedSound> val in loopingSounds)
		{
			if (!ActiveAnimationsByAnimCode.ContainsKey(val.Key))
			{
				val.Value.Stop();
				val.Value.Dispose();
				loopingSounds.Remove(val.Key);
				break;
			}
		}
	}

	public virtual void RegisterFrameCallback(AnimFrameCallback trigger)
	{
		if (Triggers == null)
		{
			Triggers = new List<AnimFrameCallback>();
		}
		Triggers.Add(trigger);
	}

	private void runTriggers()
	{
		List<AnimFrameCallback> Triggers = this.Triggers;
		if (Triggers == null)
		{
			return;
		}
		for (int i = 0; i < Triggers.Count; i++)
		{
			AnimFrameCallback trigger = Triggers[i];
			if (ActiveAnimationsByAnimCode.ContainsKey(trigger.Animation))
			{
				RunningAnimation state = Animator.GetAnimationState(trigger.Animation);
				if (state != null && state.CurrentFrame >= trigger.Frame)
				{
					Triggers.RemoveAt(i);
					trigger.Callback();
					i--;
				}
			}
		}
	}

	/// <summary>
	/// Disposes of the animation manager.
	/// </summary>
	public void Dispose()
	{
		if (loopingSounds == null)
		{
			return;
		}
		foreach (KeyValuePair<string, ILoadedSound> val in loopingSounds)
		{
			val.Value.Stop();
			val.Value.Dispose();
		}
	}

	public virtual void TriggerAnimationStopped(string code)
	{
		this.OnAnimationStopped?.Invoke(code);
	}

	public void ShouldPlaySound(string animationMetaCode, AnimationSound sound)
	{
		ShouldPlaySound(entity.World, entity.Pos.XYZFloat, entity.Pos.Dimension, loopingSounds, animationMetaCode, sound);
	}

	public static void ShouldPlaySound(IWorldAccessor world, Vec3f pos, int dimension, Dictionary<string, ILoadedSound> loopingSounds, string animationMetaCode, AnimationSound sound, Entity entity = null)
	{
		if (sound.Chance < 1f && world.Rand.NextSingle() >= sound.Chance)
		{
			return;
		}
		if (sound.Looping)
		{
			if (world is IClientWorldAccessor cworld)
			{
				if (loopingSounds == null)
				{
					loopingSounds = new Dictionary<string, ILoadedSound>();
				}
				if (!loopingSounds.TryGetValue(animationMetaCode, out var lsound))
				{
					lsound = cworld.LoadSound(new SoundParams
					{
						Location = sound.Attributes.Location,
						DisposeOnFinish = false,
						Range = sound.Attributes.Range,
						Position = pos,
						ShouldLoop = true,
						SoundType = sound.Attributes.Type,
						Pitch = sound.Attributes.Pitch.nextFloat(1f, world.Rand),
						Volume = sound.Attributes.Volume.nextFloat(1f, world.Rand)
					});
					loopingSounds.Add(animationMetaCode, lsound);
				}
				lsound.Start();
			}
		}
		else if (entity != null)
		{
			world.PlaySoundAt(sound.Attributes, entity);
		}
		else
		{
			world.PlaySoundAt(sound.Attributes, pos.X, pos.Y, pos.Z, dimension);
		}
	}
}
