using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskStayInRange : AiTaskBaseTargetable
{
	protected Vec3d targetPos;

	private readonly Vec3d ownPos = new Vec3d();

	protected float moveSpeed = 0.02f;

	protected float searchRange = 25f;

	protected float targetRange = 15f;

	protected float rangeTolerance = 2f;

	protected bool stopNow;

	protected bool active;

	protected float currentFollowTime;

	protected long finishedMs;

	protected long lastSearchTotalMs;

	protected long lastHurtByTargetTotalMs;

	protected bool lastPathfindOk;

	protected int searchWaitMs = 4000;

	protected Vec3d lastGoalReachedPos;

	protected Dictionary<long, int> futilityCounters;

	protected bool RecentlyHurt => entity.World.ElapsedMilliseconds - lastHurtByTargetTotalMs < 10000;

	public AiTaskStayInRange(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		moveSpeed = taskConfig["movespeed"].AsFloat(0.02f);
		searchRange = taskConfig["searchRange"].AsFloat(25f);
		targetRange = taskConfig["targetRange"].AsFloat(15f);
		rangeTolerance = taskConfig["targetRangeTolerance"].AsFloat(2f);
		searchWaitMs = taskConfig["searchWaitMs"].AsInt(4000);
	}

	protected override void SetDefaultValues()
	{
		base.SetDefaultValues();
		ExecutionChance = 0.1;
	}

	public override bool ShouldExecute()
	{
		if (base.noEntityCodes && (attackedByEntity == null || !retaliateAttacks))
		{
			return false;
		}
		if (!PreconditionsSatisfied())
		{
			return false;
		}
		if (targetEntity != null)
		{
			float num = entity.Pos.SquareDistanceTo(targetEntity.Pos);
			bool toofar = num > (targetRange + rangeTolerance) * (targetRange + rangeTolerance);
			bool toonear = num < (targetRange - rangeTolerance) * (targetRange - rangeTolerance);
			if (toonear && AiTaskTurretMode.RequireShortRangeAttacks(entity, targetEntity))
			{
				return false;
			}
			if (toofar || toonear)
			{
				return true;
			}
		}
		if (WhenInEmotionStates == null && base.rand.NextDouble() > 0.5)
		{
			return false;
		}
		if (lastSearchTotalMs + searchWaitMs > entity.World.ElapsedMilliseconds)
		{
			return false;
		}
		if (cooldownUntilMs > entity.World.ElapsedMilliseconds && !base.RecentlyAttacked)
		{
			return false;
		}
		if (base.rand.NextDouble() > ExecutionChance && (WhenInEmotionStates == null || !IsInEmotionState(WhenInEmotionStates)) && !base.RecentlyAttacked)
		{
			return false;
		}
		lastSearchTotalMs = entity.World.ElapsedMilliseconds;
		if (!base.RecentlyAttacked)
		{
			attackedByEntity = null;
		}
		if (ShouldRetaliateForRange(searchRange))
		{
			targetEntity = attackedByEntity;
			targetPos = targetEntity.Pos.XYZ;
			return true;
		}
		ownPos.SetWithDimension(entity.Pos);
		targetEntity = partitionUtil.GetNearestEntity(ownPos, searchRange, (Entity e) => IsTargetableEntity(e, searchRange), EnumEntitySearchType.Creatures);
		if (targetEntity != null)
		{
			targetPos = targetEntity.Pos.XYZ;
			double num2 = entity.Pos.SquareDistanceTo(targetPos);
			bool toofar2 = num2 > (double)((targetRange + rangeTolerance) * (targetRange + rangeTolerance));
			return num2 < (double)((targetRange - rangeTolerance) * (targetRange - rangeTolerance)) || toofar2;
		}
		return false;
	}

	public override void StartExecute()
	{
		base.StartExecute();
		stopNow = false;
		active = true;
		currentFollowTime = 0f;
	}

	public override bool CanContinueExecute()
	{
		return true;
	}

	public override bool ContinueExecute(float dt)
	{
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		if (pathTraverser.Active)
		{
			return true;
		}
		float num = entity.Pos.SquareDistanceTo(targetEntity.Pos);
		bool toofar = num > (targetRange + rangeTolerance) * (targetRange + rangeTolerance);
		bool toonear = num < (targetRange - rangeTolerance) * (targetRange - rangeTolerance);
		bool canWalk = false;
		if (toofar)
		{
			canWalk = WalkTowards(-1);
		}
		else if (toonear)
		{
			canWalk = WalkTowards(1);
		}
		if (canWalk)
		{
			return toofar || toonear;
		}
		return false;
	}

	private bool WalkTowards(int sign)
	{
		Vec3d selfpos = entity.Pos.XYZ;
		Vec3d dir = selfpos.SubCopy(targetEntity.Pos.X, selfpos.Y, targetEntity.Pos.Z).Normalize();
		Vec3d nextPos = selfpos + sign * dir;
		Vec3d testPos = new Vec3d((double)(int)nextPos.X + 0.5, (int)nextPos.Y, (double)(int)nextPos.Z + 0.5);
		if (canStepTowards(nextPos))
		{
			pathTraverser.WalkTowards(nextPos, moveSpeed, 0.3f, OnGoalReached, OnStuck);
			return true;
		}
		int rnds = 1 - entity.World.Rand.Next(2) * 2;
		Vec3d ldir = dir.RotatedCopy((float)rnds * ((float)Math.PI / 2f));
		nextPos = selfpos + ldir;
		testPos.Set((double)(int)nextPos.X + 0.5, (int)nextPos.Y, (double)(int)nextPos.Z + 0.5);
		if (canStepTowards(testPos))
		{
			pathTraverser.WalkTowards(nextPos, moveSpeed, 0.3f, OnGoalReached, OnStuck);
			return true;
		}
		Vec3d rdir = dir.RotatedCopy((float)(-rnds) * ((float)Math.PI / 2f));
		nextPos = selfpos + rdir;
		testPos.Set((double)(int)nextPos.X + 0.5, (int)nextPos.Y, (double)(int)nextPos.Z + 0.5);
		if (canStepTowards(testPos))
		{
			pathTraverser.WalkTowards(nextPos, moveSpeed, 0.3f, OnGoalReached, OnStuck);
			return true;
		}
		return false;
	}

	private bool canStepTowards(Vec3d nextPos)
	{
		bool hereCollide = world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, nextPos, alsoCheckTouch: false);
		if (hereCollide && !world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, collTmpVec.Set(nextPos).Add(0.0, Math.Min(1f, stepHeight), 0.0), alsoCheckTouch: false))
		{
			return true;
		}
		if (hereCollide)
		{
			return false;
		}
		if (isLiquidAt(nextPos))
		{
			return false;
		}
		bool belowCollide = world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, collTmpVec.Set(nextPos).Add(0.0, -1.1, 0.0), alsoCheckTouch: false);
		if (belowCollide)
		{
			nextPos.Y -= 1.0;
			return true;
		}
		if (isLiquidAt(collTmpVec))
		{
			return false;
		}
		bool below2Collide = world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, collTmpVec.Set(nextPos).Add(0.0, -2.1, 0.0), alsoCheckTouch: false);
		if (!belowCollide && below2Collide && entity.Pos.Y - base.TargetEntity.Pos.Y >= 1.0)
		{
			nextPos.Y -= 2.0;
			return true;
		}
		if (isLiquidAt(collTmpVec))
		{
			return false;
		}
		bool below3Collide = world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, collTmpVec.Set(nextPos).Add(0.0, -3.1, 0.0), alsoCheckTouch: false);
		if (!belowCollide && !below2Collide && below3Collide && entity.Pos.Y - base.TargetEntity.Pos.Y >= 2.0)
		{
			nextPos.Y -= 3.0;
			return true;
		}
		return false;
	}

	protected bool isLiquidAt(Vec3d pos)
	{
		return entity.World.BlockAccessor.GetBlockRaw((int)pos.X, (int)pos.Y, (int)pos.Z, 2).IsLiquid();
	}

	private void OnStuck()
	{
	}

	private void OnGoalReached()
	{
	}

	public override void FinishExecute(bool cancelled)
	{
		base.FinishExecute(cancelled);
		finishedMs = entity.World.ElapsedMilliseconds;
		active = false;
	}

	public override void OnEntityHurt(DamageSource source, float damage)
	{
		base.OnEntityHurt(source, damage);
		if (targetEntity == source.GetCauseEntity() || !active)
		{
			lastHurtByTargetTotalMs = entity.World.ElapsedMilliseconds;
			if (targetEntity != null)
			{
				targetEntity.Pos.DistanceTo(entity.Pos);
			}
		}
	}
}
