using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskStayInRangeR : AiTaskBaseTargetableR
{
	private AiTaskStayInRangeConfig Config => GetConfig<AiTaskStayInRangeConfig>();

	public new Entity? TargetEntity
	{
		get
		{
			return targetEntity;
		}
		set
		{
			targetEntity = value;
		}
	}

	public AiTaskStayInRangeR(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		baseConfig = AiTaskBaseR.LoadConfig<AiTaskStayInRangeConfig>(entity, taskConfig, aiConfig);
	}

	public override bool ShouldExecute()
	{
		if (targetEntity != null)
		{
			return CheckIfOutOfRange();
		}
		if (!PreconditionsSatisficed() && (!Config.RetaliateUnconditionally || !base.RecentlyAttacked))
		{
			return false;
		}
		if (!base.RecentlyAttacked)
		{
			attackedByEntity = null;
		}
		if (!CheckAndResetSearchCooldown())
		{
			return false;
		}
		if (Config.RetaliateAttacks && attackedByEntity != null && attackedByEntity.Alive && attackedByEntity.IsInteractable && CanSense(attackedByEntity, GetSeekingRange()) && !entity.ToleratesDamageFrom(attackedByEntity))
		{
			targetEntity = attackedByEntity;
			return true;
		}
		SearchForTarget();
		if (targetEntity != null)
		{
			return CheckIfOutOfRange();
		}
		return false;
	}

	public override bool ContinueExecute(float dt)
	{
		if (!ContinueExecuteChecks(dt))
		{
			return false;
		}
		if (pathTraverser.Active)
		{
			return true;
		}
		CheckIfOutOfRange(out var tooFar, out var tooNear);
		bool canWalk = false;
		if (tooFar)
		{
			canWalk = WalkTowards(-1);
		}
		else if (tooNear)
		{
			canWalk = WalkTowards(1);
		}
		if (canWalk)
		{
			return tooFar || tooNear;
		}
		return false;
	}

	public override void FinishExecute(bool cancelled)
	{
		base.FinishExecute(cancelled);
		targetEntity = null;
	}

	protected virtual bool CheckIfOutOfRange()
	{
		bool tooFar;
		bool tooNear;
		return CheckIfOutOfRange(out tooFar, out tooNear);
	}

	protected virtual bool CheckIfOutOfRange(out bool tooFar, out bool tooNear)
	{
		tooFar = false;
		tooNear = false;
		if (targetEntity == null)
		{
			return false;
		}
		double distanceSquared = entity.Pos.SquareDistanceTo(targetEntity.Pos.XYZ);
		tooFar = distanceSquared >= (double)(Config.TargetRangeMin * Config.TargetRangeMin);
		tooNear = distanceSquared <= (double)(Config.TargetRangeMax * Config.TargetRangeMax);
		return tooNear | tooFar;
	}

	protected virtual bool WalkTowards(int sign)
	{
		_ = entity.World.BlockAccessor;
		Vec3d selfpos = entity.Pos.XYZ;
		Vec3d dir = selfpos.SubCopy(targetEntity.Pos.X, selfpos.Y, targetEntity.Pos.Z).Normalize();
		Vec3d nextPos = selfpos + sign * dir;
		Vec3d testPos = new Vec3d((double)(int)nextPos.X + 0.5, (int)nextPos.Y, (double)(int)nextPos.Z + 0.5);
		if (CanStepTowards(nextPos))
		{
			pathTraverser.WalkTowards(nextPos, Config.MoveSpeed, 0.3f, OnGoalReached, OnStuck, Config.AiCreatureType);
			return true;
		}
		int rnds = 1 - entity.World.Rand.Next(2) * 2;
		Vec3d ldir = dir.RotatedCopy((float)rnds * ((float)Math.PI / 2f));
		nextPos = selfpos + ldir;
		testPos.Set((double)(int)nextPos.X + 0.5, (int)nextPos.Y, (double)(int)nextPos.Z + 0.5);
		if (CanStepTowards(testPos))
		{
			pathTraverser.WalkTowards(nextPos, Config.MoveSpeed, 0.3f, OnGoalReached, OnStuck, Config.AiCreatureType);
			return true;
		}
		Vec3d rdir = dir.RotatedCopy((float)(-rnds) * ((float)Math.PI / 2f));
		nextPos = selfpos + rdir;
		testPos.Set((double)(int)nextPos.X + 0.5, (int)nextPos.Y, (double)(int)nextPos.Z + 0.5);
		if (CanStepTowards(testPos))
		{
			pathTraverser.WalkTowards(nextPos, Config.MoveSpeed, 0.3f, OnGoalReached, OnStuck, Config.AiCreatureType);
			return true;
		}
		return false;
	}

	protected virtual bool CanStepTowards(Vec3d nextPos)
	{
		if (targetEntity == null)
		{
			return false;
		}
		Vec3d collTmpVec = new Vec3d();
		bool hereCollide = world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, nextPos, alsoCheckTouch: false);
		if (hereCollide && !world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, collTmpVec.Set(nextPos).Add(0.0, Math.Min(1f, stepHeight), 0.0), alsoCheckTouch: false))
		{
			return true;
		}
		if (hereCollide)
		{
			return false;
		}
		if (IsLiquidAt(nextPos) && !Config.CanStepInLiquid)
		{
			return false;
		}
		if (world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, collTmpVec.Set(nextPos).Add(0.0, -1.1, 0.0), alsoCheckTouch: false))
		{
			nextPos.Y -= 1.0;
			return true;
		}
		if (IsLiquidAt(collTmpVec) && !Config.CanStepInLiquid)
		{
			return false;
		}
		bool below2Collide = world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, collTmpVec.Set(nextPos).Add(0.0, -2.1, 0.0), alsoCheckTouch: false);
		if (below2Collide && entity.Pos.Y - targetEntity.Pos.Y >= 1.0)
		{
			nextPos.Y -= 2.0;
			return true;
		}
		if (IsLiquidAt(collTmpVec) && !Config.CanStepInLiquid)
		{
			return false;
		}
		bool below3Collide = world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, collTmpVec.Set(nextPos).Add(0.0, -3.1, 0.0), alsoCheckTouch: false);
		if (!below2Collide && below3Collide && entity.Pos.Y - targetEntity.Pos.Y >= 2.0)
		{
			nextPos.Y -= 3.0;
			return true;
		}
		return false;
	}

	protected virtual bool IsLiquidAt(Vec3d pos)
	{
		BlockPos blockPos = new BlockPos(0);
		blockPos.SetAndCorrectDimension(pos);
		return entity.World.BlockAccessor.GetBlock(blockPos).IsLiquid();
	}

	protected virtual void OnStuck()
	{
	}

	protected virtual void OnGoalReached()
	{
	}
}
