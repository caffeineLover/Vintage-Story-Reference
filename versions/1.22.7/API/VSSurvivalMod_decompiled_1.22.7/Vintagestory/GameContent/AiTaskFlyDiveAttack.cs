using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskFlyDiveAttack : AiTaskBaseTargetable
{
	[JsonProperty]
	protected float damage = 2f;

	[JsonProperty]
	protected EnumDamageType damageType = EnumDamageType.BluntAttack;

	[JsonProperty]
	protected int damageTier;

	protected float knockbackStrength = 1f;

	protected long lastCheckOrAttackMs;

	[JsonProperty]
	protected float seekingRangeVer = 25f;

	[JsonProperty]
	protected float seekingRangeHor = 25f;

	[JsonProperty]
	protected float damageRange = 5f;

	[JsonProperty]
	protected float moveSpeed = 0.04f;

	protected TimeSpan attemptToExecuteCooldownMs = TimeSpan.FromMilliseconds(1500L);

	protected TimeSpan targetRetentionTime = TimeSpan.FromSeconds(30L);

	protected const float minVerticalDistance = 9f;

	protected const float minHorizontalDistance = 20f;

	protected const float sensePlayerRange = 15f;

	protected float diveRange = 20f;

	protected float requireMinRange = 30f;

	[JsonProperty]
	protected float diveHeight = 30f;

	[JsonProperty]
	protected float timeSwitchProbability = 0.5f;

	[JsonProperty]
	protected long globalAttackCooldownMs = 3000L;

	protected HashSet<long> didDamageEntity = new HashSet<long>();

	protected EntityPos targetPos = new EntityPos();

	protected EntityBehaviorHealth? healthBehavior;

	protected float damageAccum;

	protected bool diving;

	protected bool impacted;

	protected double diveDistance = 1.0;

	protected bool shouldUseTimeSwitchThisTime;

	public bool Enabled { get; set; } = true;

	protected int CurrentDimension => entity.Pos.Dimension;

	protected int TargetDimension => targetEntity?.Pos.Dimension ?? CurrentDimension;

	public AiTaskFlyDiveAttack(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		healthBehavior = entity.GetBehavior<EntityBehaviorHealth>();
		knockbackStrength = taskConfig["knockbackStrength"].AsFloat(GameMath.Sqrt(damage / 2f));
		attemptToExecuteCooldownMs = TimeSpan.FromMilliseconds(taskConfig["attemptToExecuteCooldownMs"].AsInt(1500));
		targetRetentionTime = TimeSpan.FromSeconds(taskConfig["targetRetentionTimeSec"].AsInt(30));
	}

	public override bool ShouldExecute()
	{
		if (!Enabled)
		{
			return false;
		}
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
		if (targetEntity != null && entity.Pos.Y - targetEntity.Pos.Y > 9.0)
		{
			return entity.Pos.HorDistanceTo(targetEntity.Pos) > 20.0;
		}
		return false;
	}

	public override void StartExecute()
	{
		didDamageEntity.Clear();
		targetPos.SetFrom(targetEntity.Pos);
		diving = false;
		impacted = false;
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
		updateTargetPosition();
		if (impacted)
		{
			entity.GetBehavior<EntityBehaviorTaskAI>()?.PathTraverser?.Stop();
			return onImpact();
		}
		if (!diving)
		{
			if (entity.Pos.Y - targetPos.Y < (double)diveHeight)
			{
				entity.Pos.Motion.Y = 0.15000000596046448;
				entity.Pos.Motion.X *= 0.8999999761581421;
				entity.Pos.Motion.Z *= 0.8999999761581421;
				followTargetOnFlyUp();
				return true;
			}
			entity.AnimManager.StopAnimation("fly-idle");
			entity.AnimManager.StopAnimation("fly-flapactive");
			entity.AnimManager.StopAnimation("fly-flapcruise");
			entity.AnimManager.StartAnimation("dive");
			diveDistance = distanceToTarget();
			diving = true;
		}
		followTarget();
		if (entity.Collided)
		{
			entity.AnimManager.StopAnimation("dive");
			entity.AnimManager.StartAnimation("slam");
			impacted = true;
			attackEntities();
			return onImpact();
		}
		damageAccum += dt;
		if (damageAccum > 0.2f)
		{
			attackEntities();
			damageAccum = 0f;
		}
		return true;
	}

	public override void FinishExecute(bool cancelled)
	{
		pathTraverser.Stop();
		entity.AnimManager.StartAnimation("fly-idle");
		entity.AnimManager.StopAnimation("slam");
		if (entity is EntityErel erel)
		{
			erel.LastAttackTime = entity.World.ElapsedMilliseconds;
			erel.StandingOnGround = false;
		}
		base.FinishExecute(cancelled);
	}

	protected bool checkGlobalAttackCooldown()
	{
		long lastAttack = ((EntityErel)entity).LastAttackTime;
		return entity.World.ElapsedMilliseconds - lastAttack > globalAttackCooldownMs;
	}

	protected void updateTargetPosition()
	{
		ArgumentNullException.ThrowIfNull(targetEntity, "targetEntity");
		if (targetEntity.Pos.Dimension == entity.Pos.Dimension)
		{
			targetPos.SetFrom(targetEntity.Pos);
		}
	}

	protected bool onImpact()
	{
		entity.Pos.Roll = 0f;
		entity.Pos.Motion.Set(0.0, 0.0, 0.0);
		RunningAnimation state = entity.AnimManager.GetAnimationState("slam");
		if (state != null && state.AnimProgress > 0.9f)
		{
			entity.AnimManager.StartAnimation("takeoff");
		}
		if (entity is EntityErel ee)
		{
			ee.StandingOnGround = true;
		}
		if (state != null)
		{
			return state.AnimProgress < 0.99f;
		}
		return true;
	}

	protected void followTargetOnFlyUp()
	{
		Vec3d targetVector = targetPos.XYZ - entity.Pos.XYZ;
		entity.Pos.Roll = -(float)Math.PI / 12f;
		entity.Pos.Yaw = (float)Math.Atan2(targetVector.X, targetVector.Z);
	}

	protected void followTarget()
	{
		Vec3d targetVector = targetPos.XYZ - entity.Pos.XYZ;
		Vec3d direction = targetVector.Normalize();
		entity.Pos.Motion.X = direction.X * (double)moveSpeed * 10.0;
		entity.Pos.Motion.Y = direction.Y * (double)moveSpeed * 10.0;
		entity.Pos.Motion.Z = direction.Z * (double)moveSpeed * 10.0;
		double speed = entity.Pos.Motion.Length();
		if (speed > 0.01)
		{
			entity.Pos.Roll = (float)Math.Asin(GameMath.Clamp((0.0 - direction.Y) / speed, -1.0, 1.0));
		}
		entity.Pos.Yaw = (float)Math.Atan2(targetVector.X, targetVector.Z);
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
}
