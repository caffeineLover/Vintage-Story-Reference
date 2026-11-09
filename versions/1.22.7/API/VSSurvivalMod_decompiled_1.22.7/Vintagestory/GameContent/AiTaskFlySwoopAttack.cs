using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskFlySwoopAttack : AiTaskBaseTargetable
{
	[JsonProperty]
	protected EnumDamageType damageType = EnumDamageType.BluntAttack;

	[JsonProperty]
	protected int damageTier;

	[JsonProperty]
	protected float damage = 2f;

	protected float knockbackStrength = 1f;

	[JsonProperty]
	protected float seekingRangeVer = 25f;

	[JsonProperty]
	protected float seekingRangeHor = 25f;

	[JsonProperty]
	protected float damageRange = 2f;

	[JsonProperty]
	protected float moveSpeed = 0.04f;

	protected TimeSpan attemptToExecuteCooldownMs = TimeSpan.FromMilliseconds(1500L);

	protected TimeSpan targetRetentionTime = TimeSpan.FromSeconds(30L);

	[JsonProperty]
	protected float firstTimeSwitchDistance = 0.9f;

	[JsonProperty]
	protected float secondTimeSwitchDistance = 0.25f;

	[JsonProperty]
	protected float secondTimeSwitchMinimumDistance = 0.1f;

	[JsonProperty]
	protected float timeSwitchHealthThreshold = 0.75f;

	[JsonProperty]
	protected float minVerticalDistance = 9f;

	[JsonProperty]
	protected float minHorizontalDistance = 25f;

	protected const float sensePlayerRange = 15f;

	protected const float pathRefreshCooldown = 1f;

	protected const float pathStopRefreshThreshold = 5f;

	[JsonProperty]
	protected bool timeSwitchAtTheStart;

	[JsonProperty]
	protected int pathLength = 35;

	[JsonProperty]
	protected float speedThresholdForDamage = 0.3f;

	[JsonProperty]
	protected float timeSwitchProbability = 0.5f;

	[JsonProperty]
	protected long globalAttackCooldownMs = 3000L;

	protected long lastCheckOrAttackMs;

	protected HashSet<long> didDamageEntity = new HashSet<long>();

	protected Vec3d beginAttackPos;

	protected List<Vec3d> swoopPath;

	protected EntityBehaviorHealth? healthBehavior;

	protected float pathRefreshAccum;

	protected float pathStopRefreshAccum;

	protected float damageAccum;

	protected double initialDistanceToTarget;

	protected int intendedDimension;

	protected NatFloat timeSwitchRandom;

	protected bool shouldUseTimeSwitchThisTime;

	protected int CurrentDimension => entity.Pos.Dimension;

	protected int TargetDimension => targetEntity?.Pos.Dimension ?? CurrentDimension;

	public AiTaskFlySwoopAttack(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		healthBehavior = entity.GetBehavior<EntityBehaviorHealth>();
		timeSwitchRandom = new NatFloat(0.5f, 0.5f, EnumDistribution.UNIFORM);
		knockbackStrength = taskConfig["knockbackStrength"].AsFloat(GameMath.Sqrt(damage / 2f));
		attemptToExecuteCooldownMs = TimeSpan.FromMilliseconds(taskConfig["attemptToExecuteCooldownMs"].AsInt(1500));
		targetRetentionTime = TimeSpan.FromSeconds(taskConfig["targetRetentionTimeSec"].AsInt(30));
	}

	public override bool ShouldExecute()
	{
		long elapsedMs = entity.World.ElapsedMilliseconds;
		if (cooldownUntilMs > elapsedMs)
		{
			return false;
		}
		cooldownUntilMs = entity.World.ElapsedMilliseconds + (long)attemptToExecuteCooldownMs.TotalMilliseconds;
		if (!PreconditionsSatisfied())
		{
			return false;
		}
		if (!checkGlobalAttackCooldown())
		{
			return false;
		}
		Vec3d pos = entity.Pos.XYZ.Add(0.0, entity.SelectionBox.Y2 / 2f, 0.0).Ahead(entity.SelectionBox.XSize / 2f, 0f, entity.Pos.Yaw);
		if (entity.World.ElapsedMilliseconds - attackedByEntityMs > (long)targetRetentionTime.TotalMilliseconds)
		{
			attackedByEntity = null;
		}
		if (ShouldRetaliateForRange(15f))
		{
			targetEntity = attackedByEntity;
		}
		else
		{
			targetEntity = entity.World.GetNearestEntity(pos, seekingRangeHor, seekingRangeVer, (Entity e) => IsTargetableEntity(e, seekingRangeHor) && hasDirectContact(e, seekingRangeHor, seekingRangeVer));
		}
		lastCheckOrAttackMs = entity.World.ElapsedMilliseconds;
		if (targetEntity == null || !(entity.Pos.Y - targetEntity.Pos.Y > (double)minVerticalDistance) || !(entity.Pos.HorDistanceTo(targetEntity.Pos) > (double)minHorizontalDistance))
		{
			return false;
		}
		beginAttackPos = entity.Pos.XYZ;
		swoopPath = new List<Vec3d>(getSwoopPath(targetEntity, pathLength, simplifiedOut: false));
		return pathClear(swoopPath);
	}

	public override void StartExecute()
	{
		didDamageEntity.Clear();
		swoopPath.Clear();
		swoopPath.AddRange(getSwoopPath(targetEntity, pathLength, simplifiedOut: true));
		pathTraverser.FollowRoute(swoopPath, moveSpeed, 8f, null, null);
		pathStopRefreshAccum = 0f;
		pathRefreshAccum = 0f;
		initialDistanceToTarget = distanceToTarget();
		intendedDimension = CurrentDimension;
		shouldUseTimeSwitchThisTime = timeSwitchRandom.nextFloat() < timeSwitchProbability;
		base.StartExecute();
	}

	public override bool ContinueExecute(float dt)
	{
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		if (timeoutExceeded())
		{
			return false;
		}
		pathStopRefreshAccum += dt;
		pathRefreshAccum += dt;
		if (pathRefreshAccum > 1f && pathStopRefreshAccum < 5f && targetEntity.Pos.Dimension == entity.Pos.Dimension)
		{
			refreshPath();
			pathRefreshAccum = 0f;
		}
		if (distanceToTarget() > (double)(Math.Max(seekingRangeHor, seekingRangeVer) * 2f))
		{
			return false;
		}
		damageAccum += dt;
		if (damageAccum > 0.2f && entity.Pos.Motion.Length() > (double)speedThresholdForDamage)
		{
			damageAccum = 0f;
			attackEntities();
		}
		double speed = entity.Pos.Motion.Length();
		if (speed > 0.01)
		{
			entity.Pos.Roll = (float)Math.Asin(GameMath.Clamp((0.0 - entity.Pos.Motion.Y) / speed, -1.0, 1.0));
		}
		if (!pathTraverser.Active)
		{
			return false;
		}
		return true;
	}

	public override void FinishExecute(bool cancelled)
	{
		pathTraverser.Stop();
		if (entity is EntityErel erel)
		{
			erel.LastAttackTime = entity.World.ElapsedMilliseconds;
		}
		base.FinishExecute(cancelled);
	}

	protected bool checkGlobalAttackCooldown()
	{
		long lastAttack = ((EntityErel)entity).LastAttackTime;
		return entity.World.ElapsedMilliseconds - lastAttack > globalAttackCooldownMs;
	}

	protected bool pathClear(List<Vec3d> swoopPath)
	{
		int skipPoints = 2;
		Vec3d tmppos = new Vec3d();
		for (int i = 0; i < swoopPath.Count; i += skipPoints)
		{
			tmppos.Set(swoopPath[i]);
			tmppos.Y -= 1.0;
			if (world.CollisionTester.IsColliding(entity.World.BlockAccessor, entity.CollisionBox, tmppos))
			{
				return false;
			}
		}
		return true;
	}

	protected virtual Vec3d[] getSwoopPath(Entity target, int its, bool simplifiedOut)
	{
		EntityPos targetEntityPos = target.Pos.Copy();
		targetEntityPos.Dimension = 0;
		EntityPos thisEntityPos = entity.Pos;
		thisEntityPos.Dimension = 0;
		Vec3d targetPos = targetEntityPos.XYZ.AddCopy(target.LocalEyePos);
		Vec3d start1 = entity.Pos.XYZ;
		Vec3d start2;
		Vec3d vec3d = (start2 = new Vec3d(start1.X, targetPos.Y + 10.0, start1.Z));
		Vec3d end2 = targetPos;
		Vec3d delta1 = vec3d - start1;
		Vec3d delta2 = end2 - start2;
		int outits = (simplifiedOut ? (its / 3) : its);
		Vec3d[] points = new Vec3d[its + outits];
		for (int i = 0; i < its; i++)
		{
			double p = (double)i / (double)its;
			Vec3d mid1 = start1 + p * delta1;
			Vec3d mid2 = start2 + p * delta2;
			points[i] = (1.0 - p) * mid1 + p * mid2;
		}
		start1 = points[its - 1];
		Vec3d offs = (targetEntityPos.XYZ - thisEntityPos.XYZ) * 1f;
		Vec3d vec3d2 = (start2 = new Vec3d(targetPos.X + offs.X, targetPos.Y, targetPos.Z + offs.Z));
		end2 = new Vec3d(targetPos.X + offs.X * 1.2999999523162842, targetPos.Y + (beginAttackPos.Y - targetPos.Y) * 0.5, targetPos.Z + offs.Z * 1.2999999523162842);
		delta1 = vec3d2 - start1;
		delta2 = end2 - start2;
		for (int j = 0; j < outits; j++)
		{
			double p2 = (double)j / (double)outits;
			Vec3d mid3 = start1 + p2 * delta1;
			Vec3d mid4 = start2 + p2 * delta2;
			points[its + j] = (1.0 - p2) * mid3 + p2 * mid4;
		}
		return points;
	}

	protected void attackEntities()
	{
		List<Entity> attackableEntities = new List<Entity>();
		entity.Api.ModLoader.GetModSystem<EntityPartitioning>().GetNearestEntity(entity.Pos.XYZ, damageRange + 1f, delegate(Entity e)
		{
			if (IsTargetableEntity(e, damageRange) && hasDirectContact(e, damageRange, damageRange) && !didDamageEntity.Contains(entity.EntityId))
			{
				attackableEntities.Add(e);
			}
			return false;
		}, EnumEntitySearchType.Creatures);
		foreach (Entity attackEntity in attackableEntities)
		{
			attackEntity.ReceiveDamage(new DamageSource
			{
				Source = EnumDamageSource.Entity,
				SourceEntity = entity,
				Type = damageType,
				DamageTier = damageTier,
				KnockbackStrength = knockbackStrength
			}, damage * GlobalConstants.CreatureDamageModifier);
			if (entity is IMeleeAttackListener imal)
			{
				imal.DidAttack(attackEntity);
			}
			didDamageEntity.Add(entity.EntityId);
		}
	}

	protected double distanceToTarget()
	{
		ArgumentNullException.ThrowIfNull(targetEntity, "targetEntity");
		double num = entity.Pos.X - targetEntity.Pos.X;
		double yDistance = entity.Pos.Y - targetEntity.Pos.Y;
		double zDistance = entity.Pos.Z - targetEntity.Pos.Z;
		return Math.Sqrt(num * num + yDistance * yDistance + zDistance * zDistance);
	}

	protected void updateSwoopPathDimension(int fromDimension, int toDimension)
	{
		double difference = (toDimension - fromDimension) * 32768;
		foreach (Vec3d item in swoopPath)
		{
			item.Y += difference;
		}
	}

	protected void refreshPath()
	{
		ArgumentNullException.ThrowIfNull(targetEntity, "targetEntity");
		Vec3d[] path = getSwoopPath(targetEntity, pathLength, simplifiedOut: true);
		if (pathClear(new List<Vec3d>(path)))
		{
			swoopPath.Clear();
			swoopPath.AddRange(path);
		}
	}
}
