using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class AiTaskFleeEntity : AiTaskBaseTargetable
{
	private Vec3d targetPos = new Vec3d();

	private float targetYaw;

	private float moveSpeed = 0.02f;

	private float seekingRange = 25f;

	private float fleeingDistance = 31f;

	private float minDayLight = -1f;

	private float fleeDurationMs = 5000f;

	private float instafleeOnDamageChance;

	private bool cancelOnHurt;

	private long fleeStartMs;

	private bool stuck;

	private bool lowStabilityAttracted;

	private bool ignoreDeepDayLight;

	private bool cancelNow;

	private float nowFleeingDistance;

	private bool instafleenow;

	private bool intelligentFlee;

	protected EnumAICreatureType? creatureType;

	private long lastFailedPathSearchTotalMs = -9999L;

	private readonly Vec3d ownPos = new Vec3d();

	private int rndnum;

	private Vec3d tmpTargetPos = new Vec3d();

	public override bool AggressiveTargeting => false;

	public AiTaskFleeEntity(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		moveSpeed = taskConfig["movespeed"].AsFloat(0.02f);
		seekingRange = taskConfig["seekingRange"].AsFloat(25f);
		minDayLight = taskConfig["minDayLight"].AsFloat(-1f);
		cancelOnHurt = taskConfig["cancelOnHurt"].AsBool();
		intelligentFlee = taskConfig["intelligentFlee"].AsBool();
		ignoreDeepDayLight = taskConfig["ignoreDeepDayLight"].AsBool();
		fleeingDistance = taskConfig["fleeingDistance"].AsFloat(seekingRange + 15f);
		fleeDurationMs = taskConfig["fleeDurationMs"].AsInt(9000);
		instafleeOnDamageChance = taskConfig["instafleeOnDamageChance"].AsFloat();
		lowStabilityAttracted = entity.World.Config.GetString("temporalStability").ToBool(defaultValue: true) && (entity.Properties.Attributes?["spawnCloserDuringLowStability"].AsBool() ?? false);
		if (taskConfig["aiCreatureType"].Exists)
		{
			creatureType = (EnumAICreatureType)taskConfig["aiCreatureType"].AsInt();
		}
		rndnum = entity.World.Rand.Next(2000);
	}

	protected override void SetDefaultValues()
	{
		base.SetDefaultValues();
		ExecutionChance = 0.1;
		TargetOnlyInteractableEntities = false;
	}

	public override bool ShouldExecute()
	{
		soundChance = Math.Min(1.01f, soundChance + 0.002f);
		if (instafleenow)
		{
			return TryInstaFlee();
		}
		if (base.rand.NextDouble() > ExecutionChance)
		{
			return false;
		}
		if (intelliFleePaused())
		{
			return false;
		}
		if (base.noEntityCodes && (attackedByEntity == null || !retaliateAttacks))
		{
			return false;
		}
		if (!PreconditionsSatisfied())
		{
			return false;
		}
		if (minDayLight > 0f)
		{
			if (entity.Attributes.GetBool("ignoreDaylightFlee"))
			{
				return false;
			}
			if (ignoreDeepDayLight && entity.Pos.Y < (double)(world.SeaLevel - 2))
			{
				return false;
			}
			if ((float)entity.World.BlockAccessor.GetLightLevel((int)entity.Pos.X, (int)entity.Pos.Y, (int)entity.Pos.Z, EnumLightLevelType.TimeOfDaySunLight) / (float)entity.World.SunBrightness < minDayLight)
			{
				return false;
			}
		}
		int generation = GetOwnGeneration();
		float fearReductionFactor = ((WhenInEmotionStates != null) ? 1f : Math.Max(0f, (tamingGenerations - (float)generation) / tamingGenerations));
		ownPos.SetWithDimension(entity.Pos);
		float hereRange = fearReductionFactor * seekingRange;
		entity.World.FrameProfiler.Mark("task-fleeentity-shouldexecute-init");
		if (lowStabilityAttracted)
		{
			targetEntity = partitionUtil.GetNearestEntity(ownPos, hereRange, delegate(Entity entity)
			{
				if (!(entity is EntityAgent))
				{
					return false;
				}
				if (!IsTargetableEntity(entity, hereRange))
				{
					return false;
				}
				return !(entity is EntityPlayer) || entity.WatchedAttributes.GetDouble("temporalStability", 1.0) > 0.25;
			}, EnumEntitySearchType.Creatures) as EntityAgent;
		}
		else if (EntityTags.IsEmpty)
		{
			if (targetEntityFirstLetters.Length == 0)
			{
				targetEntity = partitionUtil.GetNearestEntity(ownPos, hereRange, (Entity entity) => IsTargetableEntityNoTagsAll(entity, hereRange) && entity is EntityAgent, EnumEntitySearchType.Creatures) as EntityAgent;
			}
			else
			{
				targetEntity = partitionUtil.GetNearestEntity(ownPos, hereRange, (Entity entity) => IsTargetableEntityNoTagsNoAll(entity, hereRange) && entity is EntityAgent, EnumEntitySearchType.Creatures) as EntityAgent;
			}
		}
		else
		{
			targetEntity = partitionUtil.GetNearestEntity(ownPos, hereRange, (Entity entity) => IsTargetableEntity(entity, hereRange) && entity is EntityAgent, EnumEntitySearchType.Creatures) as EntityAgent;
		}
		nowFleeingDistance = fleeingDistance;
		entity.World.FrameProfiler.Mark("task-fleeentity-shouldexecute-entitysearch");
		if (targetEntity != null)
		{
			if (entity.ToleratesDamageFrom(targetEntity))
			{
				nowFleeingDistance /= 2f;
			}
			float yaw = (float)Math.Atan2(targetEntity.Pos.X - entity.Pos.X, targetEntity.Pos.Z - entity.Pos.Z);
			updateTargetPosFleeMode(targetPos, yaw);
			return true;
		}
		return false;
	}

	protected bool intelliFleePaused()
	{
		if (intelligentFlee)
		{
			return entity.World.ElapsedMilliseconds - lastFailedPathSearchTotalMs < 9000 + rndnum;
		}
		return false;
	}

	private bool TryInstaFlee()
	{
		if (targetEntity == null || entity.Pos.DistanceTo(targetEntity.Pos) > (double)seekingRange)
		{
			float cosYaw = GameMath.Cos(entity.Pos.Yaw);
			float sinYaw = GameMath.Sin(entity.Pos.Yaw);
			double offset = 200.0;
			targetPos = new Vec3d(entity.Pos.X + (double)sinYaw * offset, entity.Pos.Y, entity.Pos.Z + (double)cosYaw * offset);
			targetYaw = entity.Pos.Yaw;
			targetEntity = null;
		}
		else
		{
			nowFleeingDistance = (float)entity.Pos.DistanceTo(targetEntity.Pos) + 15f;
			if (entity.ToleratesDamageFrom(targetEntity))
			{
				nowFleeingDistance /= 2.5f;
			}
			updateTargetPosFleeMode(targetPos, entity.Pos.Yaw);
		}
		instafleenow = false;
		return true;
	}

	public override void StartExecute()
	{
		base.StartExecute();
		cancelNow = false;
		soundChance = Math.Max(0.025f, soundChance - 0.2f);
		if (intelligentFlee)
		{
			int searchDepth = 3500;
			if (world.Rand.NextDouble() < 0.05)
			{
				searchDepth = 10000;
			}
			pathTraverser.Flee_Async(targetEntity.Pos.XYZ, fleeingDistance, moveSpeed, OnGoalReached, OnStuck, OnINoPath, searchDepth, creatureType);
		}
		else
		{
			float size = targetEntity?.SelectionBox.XSize ?? 0f;
			pathTraverser.WalkTowards(targetPos, moveSpeed, size + 0.2f, OnGoalReached, OnStuck);
		}
		fleeStartMs = entity.World.ElapsedMilliseconds;
		stuck = false;
	}

	private void OnINoPath()
	{
		lastFailedPathSearchTotalMs = entity.World.ElapsedMilliseconds;
		entity.Attributes.SetLong("lastFailedPathSearchTotalMs", lastFailedPathSearchTotalMs);
		stuck = true;
	}

	public override bool CanContinueExecute()
	{
		if (intelligentFlee && pathTraverser.Ready)
		{
			entity.Attributes.SetLong("lastGoodPathSearchTotalMs", entity.World.ElapsedMilliseconds);
		}
		if (intelligentFlee)
		{
			return pathTraverser.Ready;
		}
		return true;
	}

	public override bool ContinueExecute(float dt)
	{
		if (!intelligentFlee && world.Rand.NextDouble() < 0.2)
		{
			float yaw = ((targetEntity == null) ? (0f - targetYaw) : ((float)Math.Atan2(targetEntity.Pos.X - entity.Pos.X, targetEntity.Pos.Z - entity.Pos.Z)));
			updateTargetPosFleeMode(tmpTargetPos.Set(targetPos), yaw);
			pathTraverser.CurrentTarget.X = tmpTargetPos.X;
			pathTraverser.CurrentTarget.Y = tmpTargetPos.Y;
			pathTraverser.CurrentTarget.Z = tmpTargetPos.Z;
			pathTraverser.Retarget();
		}
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		if (targetEntity != null && entity.Pos.SquareDistanceTo(targetEntity.Pos) > nowFleeingDistance * nowFleeingDistance)
		{
			return false;
		}
		if (targetEntity == null && entity.World.ElapsedMilliseconds - fleeStartMs > 5000)
		{
			return false;
		}
		if (world.Rand.NextDouble() < 0.25)
		{
			float sunlight = (float)entity.World.BlockAccessor.GetLightLevel((int)entity.Pos.X, (int)entity.Pos.Y, (int)entity.Pos.Z, EnumLightLevelType.TimeOfDaySunLight) / (float)entity.World.SunBrightness;
			if (((ignoreDeepDayLight && entity.Pos.Y < (double)(world.SeaLevel - 2)) || sunlight < minDayLight) && !entity.Attributes.GetBool("ignoreDaylightFlee"))
			{
				return false;
			}
		}
		if (!stuck && (targetEntity == null || targetEntity.Alive) && (float)(entity.World.ElapsedMilliseconds - fleeStartMs) < fleeDurationMs && !cancelNow)
		{
			return pathTraverser.Active;
		}
		return false;
	}

	public override void OnEntityHurt(DamageSource source, float damage)
	{
		base.OnEntityHurt(source, damage);
		if (cancelOnHurt)
		{
			cancelNow = true;
		}
		if (source.Type != EnumDamageType.Heal && entity.World.Rand.NextDouble() < (double)instafleeOnDamageChance)
		{
			instafleenow = true;
			targetEntity = source.GetCauseEntity();
		}
	}

	public void InstaFleeFrom(Entity fromEntity)
	{
		instafleenow = true;
		targetEntity = fromEntity;
	}

	public override void FinishExecute(bool cancelled)
	{
		pathTraverser.Stop();
		base.FinishExecute(cancelled);
	}

	private void OnStuck()
	{
		stuck = true;
	}

	private void OnGoalReached()
	{
		if (!intelligentFlee)
		{
			pathTraverser.Retarget();
		}
	}
}
