using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class AiTaskSeekEntityR : AiTaskBaseTargetableR
{
	protected int pathSearchDepth = 3500;

	protected int pathDeepSearchDepth = 10000;

	protected float chanceOfDeepSearch = 0.05f;

	protected int updatePathDepth = 2000;

	protected int circlePathSearchDepth = 3500;

	protected float currentSeekingRange;

	protected float currentFollowTimeSec;

	protected float lastPathUpdateSecondsSec;

	protected EnumAttackPattern attackPattern;

	protected long finishedMs;

	protected bool jumpAnimationOn;

	protected long jumpedMs;

	protected readonly Dictionary<long, int> futilityCounters = new Dictionary<long, int>();

	protected readonly Vec3d targetPosition = new Vec3d();

	protected readonly Vec3d previousPosition = new Vec3d();

	protected bool updatedPathAfterLanding = true;

	protected Vec3d jumpHorizontalVelocity = new Vec3d();

	private readonly Vec3d posBuffer = new Vec3d();

	public override string Id => "seekentity";

	private AiTaskSeekEntityConfig Config => GetConfig<AiTaskSeekEntityConfig>();

	public AiTaskSeekEntityR(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		baseConfig = AiTaskBaseR.LoadConfig<AiTaskSeekEntityConfig>(entity, taskConfig, aiConfig);
	}

	public override bool ShouldExecute()
	{
		if (jumpAnimationOn && entity.World.ElapsedMilliseconds - finishedMs > Config.JumpAnimationTimeoutMs)
		{
			entity.AnimManager.StopAnimation("jump");
			jumpAnimationOn = false;
		}
		if (!PreconditionsSatisficed() && (!Config.RetaliateUnconditionally || !base.RecentlyAttacked))
		{
			return false;
		}
		SetSeekingRange();
		if (!base.RecentlyAttacked)
		{
			ClearAttacker();
		}
		if (ShouldRetaliate() && attackedByEntity != null)
		{
			targetEntity = attackedByEntity;
			targetPosition.SetWithDimension(attackedByEntity.Pos);
			AlarmHerd();
			return true;
		}
		if (!CheckAndResetSearchCooldown())
		{
			return false;
		}
		SearchForTarget();
		if (targetEntity == null)
		{
			return false;
		}
		AlarmHerd();
		targetPosition.SetWithDimension(targetEntity.Pos);
		if (entity.Pos.SquareDistanceTo(targetPosition) <= (double)MinDistanceToTarget(Config.ExtraTargetDistance))
		{
			return false;
		}
		return true;
	}

	public override void StartExecute()
	{
		if (targetEntity != null)
		{
			base.StartExecute();
			currentFollowTimeSec = 0f;
			attackPattern = EnumAttackPattern.DirectAttack;
			int searchDepth = pathSearchDepth;
			if (world.Rand.NextDouble() < (double)chanceOfDeepSearch)
			{
				searchDepth = pathDeepSearchDepth;
			}
			pathTraverser.NavigateTo_Async(targetEntity.Pos.XYZ, Config.MoveSpeed, MinDistanceToTarget(Config.ExtraTargetDistance), OnGoalReached, OnStuck, OnSeekUnable, searchDepth, 1, Config.AiCreatureType);
			previousPosition.SetWithDimension(entity.Pos);
		}
	}

	public override bool ContinueExecute(float dt)
	{
		if (targetEntity == null)
		{
			return false;
		}
		if (!base.ContinueExecute(dt))
		{
			return false;
		}
		currentFollowTimeSec += dt;
		lastPathUpdateSecondsSec += dt;
		if (Config.JumpAtTarget && !updatedPathAfterLanding && entity.OnGround && entity.Collided)
		{
			pathTraverser.NavigateTo(targetPosition, Config.MoveSpeed, MinDistanceToTarget(Config.ExtraTargetDistance), OnGoalReached, OnStuck, null, giveUpWhenNoPath: false, updatePathDepth, 1, Config.AiCreatureType);
			lastPathUpdateSecondsSec = 0f;
			updatedPathAfterLanding = true;
		}
		if (!Config.JumpAtTarget || entity.OnGround || updatedPathAfterLanding)
		{
			UpdatePath();
		}
		if (Config.JumpAtTarget && !updatedPathAfterLanding && !entity.OnGround)
		{
			entity.Pos.Motion.X = jumpHorizontalVelocity.X;
			entity.Pos.Motion.Z = jumpHorizontalVelocity.Z;
		}
		RestoreMainAnimation();
		if (attackPattern == EnumAttackPattern.DirectAttack)
		{
			pathTraverser.CurrentTarget.X = targetEntity.Pos.X;
			pathTraverser.CurrentTarget.Y = targetEntity.Pos.InternalY;
			pathTraverser.CurrentTarget.Z = targetEntity.Pos.Z;
		}
		if (PerformJump())
		{
			updatedPathAfterLanding = false;
			pathTraverser.Stop();
		}
		double distance = GetDistanceToTarget();
		EntityPlayer obj = targetEntity as EntityPlayer;
		bool inCreativeMode = obj != null && obj.Player?.WorldData.CurrentGameMode == EnumGameMode.Creative && !Config.TargetPlayerInAllGameModes;
		float minDistance = MinDistanceToTarget(Config.ExtraTargetDistance);
		bool pathTraverserActive = pathTraverser.Active || !updatedPathAfterLanding;
		if (targetEntity.Alive && !inCreativeMode && pathTraverserActive && currentFollowTimeSec < Config.MaxFollowTimeSec && distance < (double)currentSeekingRange)
		{
			if (!(distance > (double)minDistance))
			{
				if (targetEntity is EntityAgent entityAgent)
				{
					return entityAgent.ServerControls.TriesToMove;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override void FinishExecute(bool cancelled)
	{
		base.FinishExecute(cancelled);
		finishedMs = entity.World.ElapsedMilliseconds;
		pathTraverser.Stop();
		active = false;
	}

	public override bool CanContinueExecute()
	{
		return pathTraverser.Ready;
	}

	public override bool Notify(string key, object data)
	{
		if (key == "seekEntity")
		{
			targetEntity = (Entity)data;
			targetPosition.SetWithDimension(targetEntity.Pos);
			return true;
		}
		return false;
	}

	public override void OnEntityHurt(DamageSource source, float damage)
	{
		base.OnEntityHurt(source, damage);
		if (!active || targetEntity != source.GetCauseEntity() || targetEntity == null || targetEntity.Pos.DistanceTo(entity.Pos) <= (double)currentSeekingRange)
		{
			return;
		}
		if (Config.StopWhenAttackedByTargetOutsideOfSeekingRange)
		{
			stopTask = true;
		}
		if (Config.FleeWhenAttackedByTargetOutsideOfSeekingRange)
		{
			entity.GetBehavior<EntityBehaviorTaskAI>().TaskManager.AllTasks.ForEach(delegate(IAiTask t)
			{
				(t as AiTaskFleeEntity)?.InstaFleeFrom(targetEntity);
			});
		}
	}

	protected override bool CanSense(Entity target, double range)
	{
		if (!base.CanSense(target, range))
		{
			return false;
		}
		if (futilityCounters != null && futilityCounters.TryGetValue(target.EntityId, out var futilityCounter) && futilityCounter > 0)
		{
			futilityCounter -= 2;
			futilityCounters[target.EntityId] = futilityCounter;
			return false;
		}
		return true;
	}

	protected override bool SearchForTarget()
	{
		bool fullyTamed = (float)GetOwnGeneration() >= Config.TamingGenerations;
		posBuffer.SetWithDimension(entity.Pos);
		targetEntity = partitionUtil.GetNearestEntity(posBuffer, currentSeekingRange, (Entity potentialTarget) => (!(Config.IgnorePlayerIfFullyTamed && fullyTamed) || (!IsNonAttackingPlayer(potentialTarget) && !entity.ToleratesDamageFrom(attackedByEntity))) && IsTargetableEntity(potentialTarget, currentSeekingRange), Config.SearchType);
		return targetEntity != null;
	}

	protected virtual void OnSeekUnable()
	{
		if (targetPosition.DistanceTo(entity.Pos.XYZ) < currentSeekingRange && !TryCircleTarget())
		{
			OnCircleTargetUnable();
		}
	}

	protected virtual void OnCircleTargetUnable()
	{
		if (targetEntity != null && Config.FleeIfCantReach)
		{
			taskAiBehavior.TaskManager.GetTasks<AiTaskFleeEntity>().Foreach(delegate(AiTaskFleeEntity task)
			{
				task.InstaFleeFrom(targetEntity);
			});
		}
	}

	protected virtual bool TryCircleTarget()
	{
		attackPattern = EnumAttackPattern.CircleTarget;
		float angle = (float)Math.Atan2(entity.Pos.X - targetPosition.X, entity.Pos.Z - targetPosition.Z);
		for (int count = 0; count < 3; count++)
		{
			double value = (double)angle + 0.5 + world.Rand.NextDouble() / 2.0;
			double distance = 4.0 + world.Rand.NextDouble() * 6.0;
			double dx = GameMath.Sin(value) * distance;
			double dz = GameMath.Cos(value) * distance;
			targetPosition.Add(dx, 0.0, dz);
			int tries = 0;
			bool ok = false;
			BlockPos tmp = new BlockPos((int)targetPosition.X, (int)targetPosition.Y, (int)targetPosition.Z);
			int dy = 0;
			while (tries < 5)
			{
				if (world.BlockAccessor.GetBlockBelow(tmp, dy).SideSolid[BlockFacing.UP.Index] && !world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, new Vec3d((double)tmp.X + 0.5, tmp.Y - dy + 1, (double)tmp.Z + 0.5), alsoCheckTouch: false))
				{
					ok = true;
					targetPosition.Y -= dy;
					targetPosition.Y += 1.0;
					break;
				}
				if (world.BlockAccessor.GetBlockAbove(tmp, dy).SideSolid[BlockFacing.UP.Index] && !world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, new Vec3d((double)tmp.X + 0.5, tmp.Y + dy + 1, (double)tmp.Z + 0.5), alsoCheckTouch: false))
				{
					ok = true;
					targetPosition.Y += dy;
					targetPosition.Y += 1.0;
					break;
				}
				tries++;
				dy++;
			}
			if (ok)
			{
				pathTraverser.NavigateTo_Async(targetPosition.Clone(), Config.MoveSpeed, MinDistanceToTarget(Config.ExtraTargetDistance), OnGoalReached, OnStuck, OnCircleTargetUnable, circlePathSearchDepth, 1, Config.AiCreatureType);
				return true;
			}
		}
		return false;
	}

	protected virtual double GetDistanceToTarget()
	{
		if (targetEntity == null)
		{
			return double.MaxValue;
		}
		Cuboidd cuboidd = targetEntity.SelectionBox.ToDouble().Translate(targetEntity.Pos.X, targetEntity.Pos.InternalY, targetEntity.Pos.Z);
		posBuffer.SetWithDimension(entity.Pos);
		posBuffer.Add(0.0, entity.SelectionBox.Y2 / 2f, 0.0).Ahead(entity.SelectionBox.XSize / 2f, 0f, entity.Pos.Yaw);
		return cuboidd.ShortestDistanceFrom(posBuffer);
	}

	protected virtual void UpdatePath()
	{
		if (targetEntity != null && attackPattern == EnumAttackPattern.DirectAttack && lastPathUpdateSecondsSec >= Config.PathUpdateCooldownSec && targetPosition.SquareDistanceTo(targetEntity.Pos.X, targetEntity.Pos.InternalY, targetEntity.Pos.Z) >= Config.MinDistanceToUpdatePath * Config.MinDistanceToUpdatePath)
		{
			targetPosition.Set(targetEntity.Pos.X + targetEntity.Pos.Motion.X * (double)Config.MotionAnticipationFactor, targetEntity.Pos.InternalY, targetEntity.Pos.Z + targetEntity.Pos.Motion.Z * (double)Config.MotionAnticipationFactor);
			pathTraverser.NavigateTo(targetPosition, Config.MoveSpeed, MinDistanceToTarget(Config.ExtraTargetDistance), OnGoalReached, OnStuck, null, giveUpWhenNoPath: false, updatePathDepth, 1, Config.AiCreatureType);
			lastPathUpdateSecondsSec = 0f;
		}
	}

	protected virtual void RestoreMainAnimation()
	{
		if (Config.AnimationMeta == null)
		{
			return;
		}
		if (Config.JumpAtTarget && !entity.AnimManager.IsAnimationActive(Config.AnimationMeta.Code))
		{
			RunningAnimation animationState = entity.AnimManager.Animator.GetAnimationState(Config.JumpAnimationCode);
			if (animationState == null || !animationState.Active)
			{
				Config.AnimationMeta.EaseInSpeed = 1f;
				Config.AnimationMeta.EaseOutSpeed = 1f;
				entity.AnimManager.StartAnimation(Config.AnimationMeta);
			}
		}
		if (jumpAnimationOn && entity.World.ElapsedMilliseconds - finishedMs > Config.JumpAnimationTimeoutMs)
		{
			entity.AnimManager.StopAnimation(Config.JumpAnimationCode);
			Config.AnimationMeta.EaseInSpeed = 1f;
			Config.AnimationMeta.EaseOutSpeed = 1f;
			entity.AnimManager.StartAnimation(Config.AnimationMeta);
		}
	}

	protected virtual void PlayJumpAnimation()
	{
		if (Config.JumpAnimationCode != null)
		{
			string[] animationToStopForJump = Config.AnimationToStopForJump;
			foreach (string animation in animationToStopForJump)
			{
				entity.AnimManager.StopAnimation(animation);
				entity.AnimManager.StopAnimation(animation);
			}
			entity.AnimManager.StartAnimation(new AnimationMetaData
			{
				Animation = Config.JumpAnimationCode,
				Code = Config.JumpAnimationCode
			}.Init());
			jumpAnimationOn = true;
		}
	}

	protected virtual bool PerformJump()
	{
		if (targetEntity == null)
		{
			return false;
		}
		double distance = GetDistanceToTarget();
		EntityPlayer obj = targetEntity as EntityPlayer;
		if ((obj != null && obj.Player?.WorldData.CurrentGameMode == EnumGameMode.Creative && !Config.TargetPlayerInAllGameModes) || !Config.JumpAtTarget || base.Rand.NextDouble() > (double)Config.JumpChance)
		{
			return false;
		}
		bool recentlyJumped = entity.World.ElapsedMilliseconds - jumpedMs < Config.JumpCooldownMs;
		bool jumpedJustNow = false;
		if (distance >= (double)Config.DistanceToTargetToJump[0] && distance <= (double)Config.DistanceToTargetToJump[1] && !recentlyJumped && (double)Config.MaxHeightDifferenceToJump >= entity.Pos.Y - targetEntity.Pos.Y)
		{
			Vec3d horizontalMotion = new Vec3d(entity.Pos.Motion.X, 0.0, entity.Pos.Motion.Z);
			double horizontalSpeed = horizontalMotion.Length();
			double dx = (targetEntity.Pos.X - entity.Pos.X + targetEntity.Pos.Motion.X * (double)Config.JumpMotionAnticipationFactor) * (double)Config.JumpSpeedFactor * 0.032999999821186066 * 0.5;
			double dz = (targetEntity.Pos.Z - entity.Pos.Z + targetEntity.Pos.Motion.Z * (double)Config.JumpMotionAnticipationFactor) * (double)Config.JumpSpeedFactor * 0.032999999821186066 * 0.5;
			double dy = (double)Config.JumpHeightFactor * GameMath.Max(Config.MaxVerticalJumpSpeed, (targetEntity.Pos.Y - entity.Pos.Y) * 0.032999999821186066);
			horizontalMotion.Set(dx, 0.0, dz).Normalize().Mul(horizontalSpeed);
			jumpHorizontalVelocity.Set(dx + entity.Pos.Motion.X, 0.0, dz + entity.Pos.Motion.Z);
			entity.Pos.Motion.X = horizontalMotion.X;
			entity.Pos.Motion.Z = horizontalMotion.Z;
			entity.Pos.Motion.Add(dx, dy, dz);
			float yaw = (float)Math.Atan2(dx, dz);
			entity.Pos.Yaw = yaw;
			PlayJumpAnimation();
			jumpedMs = entity.World.ElapsedMilliseconds;
			finishedMs = entity.World.ElapsedMilliseconds;
			jumpedJustNow = true;
		}
		if (recentlyJumped && !entity.Collided && distance < (double)Config.DistanceToTargetToJump[0])
		{
			entity.Pos.Motion *= Config.AfterJumpSpeedReduction;
		}
		return jumpedJustNow;
	}

	protected virtual void AlarmHerd()
	{
		if (!Config.AlarmHerd || entity.HerdId == 0L)
		{
			return;
		}
		posBuffer.SetWithDimension(entity.Pos);
		entity.World.GetNearestEntity(posBuffer, Config.HerdAlarmRange, Config.HerdAlarmRange, delegate(Entity target)
		{
			if (target.EntityId != entity.EntityId && target is EntityAgent { Alive: not false } entityAgent && entityAgent.HerdId == entity.HerdId)
			{
				entityAgent.Notify("seekEntity", targetEntity);
			}
			return false;
		});
	}

	protected virtual void OnStuck()
	{
		stopTask = true;
	}

	protected virtual void OnGoalReached()
	{
		if (targetEntity == null || attackPattern != EnumAttackPattern.DirectAttack)
		{
			return;
		}
		if ((double)previousPosition.SquareDistanceTo(entity.Pos) < 0.001)
		{
			futilityCounters.TryGetValue(targetEntity.EntityId, out var futilityCounter);
			futilityCounter++;
			futilityCounters[targetEntity.EntityId] = futilityCounter;
			if (futilityCounter > 19)
			{
				return;
			}
		}
		previousPosition.SetWithDimension(entity.Pos);
		pathTraverser.Retarget();
	}

	protected virtual float SetSeekingRange()
	{
		currentSeekingRange = Config.SeekingRange;
		if (Config.BelowTemperatureThreshold > -99f && entity.World.BlockAccessor.GetClimateAt(entity.Pos.AsBlockPos, EnumGetClimateMode.ForSuppliedDate_TemperatureOnly, entity.World.Calendar.TotalDays).Temperature <= Config.BelowTemperatureThreshold)
		{
			currentSeekingRange = Config.BelowTemperatureSeekingRange;
		}
		if (Config.RetaliationSeekingRangeFactor != 1f && Config.RetaliateAttacks && attackedByEntity != null && attackedByEntity.Alive && attackedByEntity.IsInteractable && CanSense(attackedByEntity, currentSeekingRange * Config.RetaliationSeekingRangeFactor) && !entity.ToleratesDamageFrom(attackedByEntity))
		{
			currentSeekingRange *= Config.RetaliationSeekingRangeFactor;
		}
		currentSeekingRange *= GetFearReductionFactor();
		return currentSeekingRange;
	}
}
