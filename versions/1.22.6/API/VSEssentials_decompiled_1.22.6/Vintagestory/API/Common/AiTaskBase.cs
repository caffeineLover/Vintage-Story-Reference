using System;
using System.Linq;
using Newtonsoft.Json;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.Essentials;
using Vintagestory.GameContent;

namespace Vintagestory.API.Common;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public abstract class AiTaskBase : IAiTask
{
	[ThreadStatic]
	private static Random? randTL;

	public readonly EntityAgent entity;

	public readonly IWorldAccessor world;

	public AnimationMetaData? animMeta;

	[JsonProperty]
	protected float? priorityForCancel;

	[JsonProperty]
	public double ExecutionChance = 1.0;

	[JsonProperty]
	public TagSetFast TagsAppliedToEntity;

	protected TagSetFast tagsAppliedOnStart;

	[JsonProperty]
	public int MinCooldownMs;

	[JsonProperty]
	public int MaxCooldownMs = 100;

	[JsonProperty]
	protected double MinCooldownHours;

	[JsonProperty]
	protected double MaxCooldownHours;

	protected AssetLocation? finishSound;

	protected AssetLocation? sound;

	[JsonProperty]
	protected float soundRange = 16f;

	[JsonProperty]
	protected int soundStartMs;

	[JsonProperty]
	protected int soundRepeatMs;

	[JsonProperty]
	protected float soundChance = 1.01f;

	protected long lastSoundTotalMs;

	[JsonProperty]
	public bool? WhenSwimming;

	[JsonProperty]
	public string[]? WhenInEmotionStates;

	[JsonProperty]
	public string[]? WhenNotInEmotionStates;

	protected long cooldownUntilMs;

	protected int? minGeneration;

	protected int? maxGeneration;

	protected double cooldownUntilTotalHours;

	protected WaypointsTraverser pathTraverser;

	protected EntityBehaviorEmotionStates? bhEmo;

	[JsonProperty]
	public DayTimeFrame[]? duringDayTimeFrames;

	[JsonProperty]
	public float DayTimeFrameInaccuracy = 0.15f;

	[JsonProperty]
	public int[] EntityLightLevels = new int[2] { 0, 32 };

	[JsonProperty]
	public EnumLightLevelType EntityLightLevelType = EnumLightLevelType.MaxTimeOfDayLight;

	[JsonProperty]
	public float[]? TemperatureRange;

	protected double defaultTimeoutSec = 30.0;

	protected TimeSpan timeout;

	protected long ExecutionStartTimeMs;

	public Random rand => randTL ?? (randTL = new Random());

	[JsonProperty]
	public virtual string? Id { get; set; }

	[JsonProperty]
	public virtual float Priority { get; set; }

	public virtual float PriorityForCancel => priorityForCancel ?? Priority;

	[JsonProperty]
	public virtual int Slot { get; set; }

	[Obsolete("Use MinCooldownMs instead. Kept for now for json backwards compatibility.")]
	[JsonProperty]
	public int Mincooldown
	{
		get
		{
			return MinCooldownMs;
		}
		set
		{
			MinCooldownMs = value;
		}
	}

	[Obsolete("Use MaxCooldownMs instead. Kept for now for json backwards compatibility.")]
	[JsonProperty]
	public int Maxcooldown
	{
		get
		{
			return MaxCooldownMs;
		}
		set
		{
			MaxCooldownMs = value;
		}
	}

	[Obsolete("Use MinCooldownHours instead.")]
	protected double mincooldownHours
	{
		get
		{
			return MinCooldownHours;
		}
		set
		{
			MinCooldownHours = value;
		}
	}

	[Obsolete("Use MaxCooldownHours instead.")]
	protected double maxcooldownHours
	{
		get
		{
			return MaxCooldownHours;
		}
		set
		{
			MaxCooldownHours = value;
		}
	}

	[Obsolete("Use WhenInEmotionStates (plural, array) instead")]
	[JsonProperty]
	public string? WhenInEmotionState
	{
		get
		{
			string[]? whenInEmotionStates = WhenInEmotionStates;
			if (whenInEmotionStates == null)
			{
				return null;
			}
			return whenInEmotionStates[0];
		}
		set
		{
			WhenInEmotionStates = value?.Split("|");
		}
	}

	[Obsolete("Use WhenNotInEmotionStates (plural, array) instead")]
	[JsonProperty]
	public string? WhenNotInEmotionState
	{
		get
		{
			string[]? whenNotInEmotionStates = WhenNotInEmotionStates;
			if (whenNotInEmotionStates == null)
			{
				return null;
			}
			return whenNotInEmotionStates[0];
		}
		set
		{
			WhenNotInEmotionStates = value?.Split("|");
		}
	}

	public string ProfilerName { get; set; } = "";

	public AiTaskBase(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
	{
		this.entity = entity;
		world = entity.World;
		if (randTL == null)
		{
			randTL = new Random((int)entity.EntityId);
		}
		pathTraverser = entity.GetBehavior<EntityBehaviorTaskAI>().PathTraverser;
		if (pathTraverser == null)
		{
			throw new ArgumentNullException("No PathTraverser found for entity " + entity.Code);
		}
		bhEmo = entity.GetBehavior<EntityBehaviorEmotionStates>();
		SetDefaultValues();
		if (taskConfig.Exists)
		{
			taskConfig.Token.Populate(this);
		}
		Id = taskConfig["id"].AsString();
		int initialmincooldown = taskConfig["initialMinCoolDown"].AsInt(MinCooldownMs);
		int initialmaxcooldown = taskConfig["initialMaxCoolDown"].AsInt(MaxCooldownMs);
		cooldownUntilMs = entity.World.ElapsedMilliseconds + initialmincooldown + entity.World.Rand.Next(initialmaxcooldown - initialmincooldown);
		minGeneration = (taskConfig["minGeneration"].Exists ? new int?(taskConfig["minGeneration"].AsInt()) : ((int?)null));
		maxGeneration = (taskConfig["maxGeneration"].Exists ? new int?(taskConfig["maxGeneration"].AsInt()) : ((int?)null));
		timeout = TimeSpan.FromSeconds(taskConfig["timeoutSec"].AsDouble(defaultTimeoutSec));
		JsonObject animationCfg = taskConfig["animation"];
		if (animationCfg.Exists)
		{
			string code = animationCfg.AsString()?.ToLowerInvariant();
			JsonObject animationSpeedCfg = taskConfig["animationSpeed"];
			float speed = animationSpeedCfg.AsFloat(1f);
			AnimationMetaData cmeta = this.entity.Properties.Client.Animations.FirstOrDefault((AnimationMetaData a) => a.Code == code);
			if (cmeta != null)
			{
				if (animationSpeedCfg.Exists)
				{
					animMeta = cmeta.Clone();
					animMeta.AnimationSpeed = speed;
				}
				else
				{
					animMeta = cmeta;
				}
			}
			else
			{
				animMeta = new AnimationMetaData
				{
					Code = code,
					Animation = code,
					AnimationSpeed = speed
				}.Init();
				animMeta.EaseInSpeed = 1f;
				animMeta.EaseOutSpeed = 1f;
			}
		}
		JsonObject soundCfg = taskConfig["sound"];
		if (soundCfg.Exists)
		{
			sound = AssetLocation.Create(soundCfg.AsString(), entity.Code.Domain).WithPathPrefixOnce("sounds/");
		}
		JsonObject finishSoundCfg = taskConfig["finishSound"];
		if (finishSoundCfg.Exists)
		{
			finishSound = AssetLocation.Create(finishSoundCfg.AsString(), entity.Code.Domain).WithPathPrefixOnce("sounds/");
		}
		string taskCode = taskConfig["code"].AsString();
		if (TemperatureRange != null && TemperatureRange.Length != 2)
		{
			throw new ArgumentException($"Invalid 'temperatureRange' value in AI task '{taskCode}' for entity '{entity.Code}'");
		}
		if (EntityLightLevels.Length != 2)
		{
			throw new ArgumentException($"Invalid 'entityLightLevels' value in AI task '{taskCode}' for entity '{entity.Code}'");
		}
	}

	protected virtual void SetDefaultValues()
	{
	}

	protected virtual bool IsOnCooldown()
	{
		if (cooldownUntilMs <= entity.World.ElapsedMilliseconds)
		{
			return cooldownUntilTotalHours > entity.World.Calendar.TotalHours;
		}
		return true;
	}

	protected virtual void ResetCooldown()
	{
		cooldownUntilMs = entity.World.ElapsedMilliseconds + MinCooldownMs + entity.World.Rand.Next(MaxCooldownMs - MinCooldownMs);
		cooldownUntilTotalHours = entity.World.Calendar.TotalHours + MinCooldownHours + entity.World.Rand.NextDouble() * (MaxCooldownHours - MinCooldownHours);
	}

	protected virtual bool IsValidTemperature()
	{
		if (TemperatureRange == null)
		{
			return true;
		}
		float temperature = entity.World.BlockAccessor.GetClimateAt(entity.Pos.AsBlockPos, EnumGetClimateMode.ForSuppliedDate_TemperatureOnly, entity.World.Calendar.TotalDays).Temperature;
		if (TemperatureRange[0] <= temperature)
		{
			return temperature <= TemperatureRange[1];
		}
		return false;
	}

	protected virtual bool IsValidLightLevel()
	{
		if (EntityLightLevels[0] == 0 && EntityLightLevels[1] == 32)
		{
			return true;
		}
		int lightLevel = entity.World.BlockAccessor.GetLightLevel((int)entity.Pos.X, (int)entity.Pos.InternalY, (int)entity.Pos.Z, EntityLightLevelType);
		if (EntityLightLevels[0] <= lightLevel)
		{
			return lightLevel <= EntityLightLevels[1];
		}
		return false;
	}

	protected virtual bool PreconditionsSatisfied()
	{
		if (WhenSwimming.HasValue && WhenSwimming != entity.Swimming)
		{
			return false;
		}
		if (WhenInEmotionStates != null && !IsInEmotionState(WhenInEmotionStates))
		{
			return false;
		}
		if (WhenNotInEmotionStates != null && IsInEmotionState(WhenNotInEmotionStates))
		{
			return false;
		}
		if (minGeneration.HasValue && entity.WatchedAttributes.GetInt("generation") < minGeneration)
		{
			return false;
		}
		if (maxGeneration.HasValue && entity.WatchedAttributes.GetInt("generation") > maxGeneration)
		{
			return false;
		}
		if (!IsValidTemperature())
		{
			return false;
		}
		if (!IsValidLightLevel())
		{
			return false;
		}
		if (!IsInValidDayTimeHours(initialRandomness: true))
		{
			return false;
		}
		return true;
	}

	protected virtual bool IsInValidDayTimeHours(bool initialRandomness)
	{
		if (duringDayTimeFrames != null)
		{
			double hourOfDay = entity.World.Calendar.HourOfDay / entity.World.Calendar.HoursPerDay * 24f;
			if (initialRandomness)
			{
				hourOfDay += (double)DayTimeFrameInaccuracy * (entity.World.Rand.NextDouble() * 2.0 - 1.0);
			}
			for (int i = 0; i < duringDayTimeFrames.Length; i++)
			{
				if (duringDayTimeFrames[i].Matches(hourOfDay))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	protected virtual bool IsInEmotionState(string[] emotionStates)
	{
		if (bhEmo == null)
		{
			return false;
		}
		foreach (string state in emotionStates)
		{
			if (bhEmo.IsInEmotionState(state))
			{
				return true;
			}
		}
		return false;
	}

	protected virtual bool IsInEmotionState(string emotionState)
	{
		if (bhEmo == null)
		{
			return false;
		}
		if (emotionState == null)
		{
			return false;
		}
		return bhEmo.IsInEmotionState(emotionState);
	}

	public virtual void AfterInitialize()
	{
	}

	public abstract bool ShouldExecute();

	public virtual void StartExecute()
	{
		if (animMeta != null)
		{
			entity.AnimManager.StartAnimation(animMeta);
		}
		if (sound != null && entity.World.Rand.NextDouble() <= (double)soundChance)
		{
			if (soundStartMs > 0)
			{
				entity.World.RegisterCallback(delegate
				{
					entity.World.PlaySoundAt(sound, entity.Pos.X, entity.Pos.InternalY, entity.Pos.Z, null, randomizePitch: true, soundRange);
					lastSoundTotalMs = entity.World.ElapsedMilliseconds;
				}, soundStartMs);
			}
			else
			{
				entity.World.PlaySoundAt(sound, entity.Pos.X, entity.Pos.InternalY, entity.Pos.Z, null, randomizePitch: true, soundRange);
				lastSoundTotalMs = entity.World.ElapsedMilliseconds;
			}
		}
		ExecutionStartTimeMs = entity.World.ElapsedMilliseconds;
		tagsAppliedOnStart = TagsAppliedToEntity & ~entity.Tags;
		if (!tagsAppliedOnStart.IsEmpty)
		{
			entity.Tags |= tagsAppliedOnStart;
			entity.MarkTagsDirty();
		}
	}

	public virtual bool ContinueExecute(float dt)
	{
		if (sound != null && soundRepeatMs > 0 && entity.World.ElapsedMilliseconds > lastSoundTotalMs + soundRepeatMs)
		{
			entity.World.PlaySoundAt(sound, entity.Pos.X, entity.Pos.InternalY, entity.Pos.Z, null, randomizePitch: true, soundRange);
			lastSoundTotalMs = entity.World.ElapsedMilliseconds;
		}
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		return true;
	}

	public virtual void FinishExecute(bool cancelled)
	{
		ResetCooldown();
		if (animMeta != null && animMeta.Code != "attack" && animMeta.Code != "idle")
		{
			entity.AnimManager.StopAnimation(animMeta.Code);
		}
		if (finishSound != null)
		{
			entity.World.PlaySoundAt(finishSound, entity.Pos.X, entity.Pos.InternalY, entity.Pos.Z, null, randomizePitch: true, soundRange);
		}
		if (!tagsAppliedOnStart.IsEmpty)
		{
			entity.Tags &= ~tagsAppliedOnStart;
			entity.MarkTagsDirty();
		}
	}

	public virtual void OnStateChanged(EnumEntityState beforeState)
	{
		if (entity.State == EnumEntityState.Active)
		{
			IWorldAccessor World = entity.World;
			cooldownUntilMs = World.ElapsedMilliseconds + MinCooldownMs + World.Rand.Next(MaxCooldownMs - MinCooldownMs);
		}
	}

	public virtual bool Notify(string key, object data)
	{
		return false;
	}

	public virtual void OnEntityLoaded()
	{
	}

	public virtual void OnEntitySpawn()
	{
	}

	public virtual void OnEntityDespawn(EntityDespawnData reason)
	{
	}

	public virtual void OnEntityHurt(DamageSource source, float damage)
	{
	}

	public virtual void OnNoPath(Vec3d target)
	{
	}

	public virtual bool CanContinueExecute()
	{
		return true;
	}

	protected virtual bool timeoutExceeded()
	{
		return (double)(entity.World.ElapsedMilliseconds - ExecutionStartTimeMs) > timeout.TotalMilliseconds;
	}
}
