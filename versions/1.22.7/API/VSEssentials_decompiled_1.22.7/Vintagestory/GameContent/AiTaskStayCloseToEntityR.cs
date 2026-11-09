using System;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskStayCloseToEntityR : AiTaskBaseTargetableR
{
	protected bool stuck;

	protected FastVec3d targetOffset;

	protected FastVec3d initialTargetPos;

	protected float executingTimeSec;

	protected int stuckCounter;

	private readonly BlockPos blockPosBuffer = new BlockPos(0);

	public virtual float TeleportMaxRange => Config.TeleportMaxRange;

	public virtual int AllowTeleportCount { get; set; }

	private AiTaskStayCloseToEntityConfig Config => GetConfig<AiTaskStayCloseToEntityConfig>();

	public AiTaskStayCloseToEntityR(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		baseConfig = AiTaskBaseR.LoadConfig<AiTaskStayCloseToEntityConfig>(entity, taskConfig, aiConfig);
	}

	public override bool ShouldExecute()
	{
		if (stuckCounter > 3)
		{
			stuckCounter = 0;
			cooldownUntilMs = entity.World.ElapsedMilliseconds + 60000;
		}
		if (!PreconditionsSatisficed())
		{
			return false;
		}
		if (!SearchForTarget())
		{
			return false;
		}
		if (targetEntity != null)
		{
			float num = entity.Pos.SquareDistanceTo(targetEntity.Pos);
			float minDistance = MinDistanceToTarget(Config.MinRangeToTrigger);
			if (num <= minDistance * minDistance)
			{
				return false;
			}
		}
		return true;
	}

	public override void StartExecute()
	{
		if (targetEntity != null)
		{
			base.StartExecute();
			executingTimeSec = 0f;
			initialTargetPos.Set(targetEntity.Pos.X, targetEntity.Pos.Y, targetEntity.Pos.Z);
			pathTraverser.NavigateTo_Async(targetEntity.Pos.XYZ, Config.MoveSpeed, MinDistanceToTarget(Config.ExtraMinDistanceToTarget), OnGoalReached, OnStuck, OnNoPath, 1000, 1, Config.AiCreatureType);
			targetOffset.Set(entity.World.Rand.NextDouble() * (double)Config.RandomTargetOffset - (double)(Config.RandomTargetOffset / 2f), 0.0, entity.World.Rand.NextDouble() * (double)Config.RandomTargetOffset - (double)(Config.RandomTargetOffset / 2f));
			stuck = false;
		}
	}

	public override bool ContinueExecute(float dt)
	{
		if (!base.ContinueExecute(dt))
		{
			return false;
		}
		if (targetEntity == null)
		{
			return false;
		}
		if (initialTargetPos.Distance(targetEntity.Pos.XYZ) > (double)Config.MinDistanceToRetarget)
		{
			initialTargetPos.Set(targetEntity.Pos.X, targetEntity.Pos.Y, targetEntity.Pos.Z);
			pathTraverser.Retarget();
		}
		double x = targetEntity.Pos.X + targetOffset.X;
		double y = targetEntity.Pos.Y;
		double z = targetEntity.Pos.Z + targetOffset.Z;
		pathTraverser.CurrentTarget.X = x;
		pathTraverser.CurrentTarget.Y = y;
		pathTraverser.CurrentTarget.Z = z;
		float distance = entity.Pos.SquareDistanceTo(x, y, z);
		float minDistance = MinDistanceToTarget();
		if (distance < minDistance * minDistance)
		{
			pathTraverser.Stop();
			return false;
		}
		if ((Config.AllowTeleport || AllowTeleportCount > 0) && executingTimeSec > Config.TeleportDelaySec && (distance > Config.TeleportAfterRange * Config.TeleportAfterRange || stuck) && distance < Config.TeleportMaxRange * Config.TeleportMaxRange && base.Rand.NextDouble() < (double)Config.TeleportChance)
		{
			TryTeleport();
		}
		executingTimeSec += dt;
		if (stuck || !pathTraverser.Active)
		{
			return executingTimeSec < Config.MinTimeBeforeGiveUpSec;
		}
		return true;
	}

	public override void FinishExecute(bool cancelled)
	{
		if (stuck)
		{
			stuckCounter++;
		}
		else
		{
			stuckCounter = 0;
		}
		base.FinishExecute(cancelled);
	}

	public override bool CanContinueExecute()
	{
		return pathTraverser.Ready;
	}

	protected virtual bool FindDecentTeleportPos(out FastVec3d teleportPosition)
	{
		teleportPosition = default(FastVec3d);
		if (targetEntity == null)
		{
			return false;
		}
		IBlockAccessor blockAccessor = entity.World.BlockAccessor;
		for (int teleportRangeStep = (int)Config.MinTeleportDistanceToTarget * 5; teleportRangeStep < (int)(Config.MaxTeleportDistanceToTarget + 1f) * 5; teleportRangeStep++)
		{
			float range = GameMath.Clamp((float)teleportRangeStep / 5f, Config.MinTeleportDistanceToTarget, Config.MaxTeleportDistanceToTarget);
			double xOffset = base.Rand.NextDouble() * 2.0 * (double)range - (double)range;
			double zOffset = base.Rand.NextDouble() * 2.0 * (double)range - (double)range;
			for (int yOffsetSeed = 0; yOffsetSeed < 8; yOffsetSeed++)
			{
				int yOffset = (1 - yOffsetSeed % 2 * 2) * (int)Math.Ceiling((float)yOffsetSeed / 2f);
				teleportPosition.Set(targetEntity.Pos.X + xOffset, targetEntity.Pos.Y + (double)yOffset, targetEntity.Pos.Z + zOffset);
				blockPosBuffer.Set((int)teleportPosition.X, (int)teleportPosition.Y, (int)teleportPosition.Z);
				Block aboveBlock = blockAccessor.GetBlock(blockPosBuffer);
				Cuboidf[] boxes = aboveBlock.GetCollisionBoxes(blockAccessor, blockPosBuffer);
				if (boxes != null && boxes.Length != 0)
				{
					continue;
				}
				JsonObject attributes = aboveBlock.Attributes;
				if (attributes != null && attributes["insideDamage"].AsInt() > 0)
				{
					continue;
				}
				blockPosBuffer.Set((int)teleportPosition.X, (int)teleportPosition.Y - 1, (int)teleportPosition.Z);
				Block belowBlock = blockAccessor.GetBlock(blockPosBuffer);
				boxes = belowBlock.GetCollisionBoxes(blockAccessor, blockPosBuffer);
				if (boxes == null || boxes.Length == 0)
				{
					continue;
				}
				JsonObject attributes2 = belowBlock.Attributes;
				if (attributes2 == null || attributes2["insideDamage"].AsInt() <= 0)
				{
					teleportPosition.Y = (float)((int)teleportPosition.Y - 1) + boxes.Max((Cuboidf cuboid) => cuboid.Y2);
					return true;
				}
			}
		}
		return false;
	}

	protected virtual void TryTeleport()
	{
		if ((Config.AllowTeleport || AllowTeleportCount > 0) && targetEntity != null && FindDecentTeleportPos(out var pos))
		{
			entity.TeleportToDouble(pos.X, pos.Y, pos.Z, delegate
			{
				initialTargetPos.Set(targetEntity.Pos.X, targetEntity.Pos.Y, targetEntity.Pos.Z);
				pathTraverser.Retarget();
				AllowTeleportCount = Math.Max(0, AllowTeleportCount - 1);
			});
		}
	}

	protected virtual void OnStuck()
	{
		stuck = true;
	}

	protected virtual void OnNoPath()
	{
	}

	protected virtual void OnGoalReached()
	{
		stopTask = true;
	}
}
