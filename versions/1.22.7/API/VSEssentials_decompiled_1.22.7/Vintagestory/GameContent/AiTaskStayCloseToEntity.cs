using System;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskStayCloseToEntity : AiTaskBase
{
	protected Entity targetEntity;

	protected float moveSpeed = 0.03f;

	protected float range = 8f;

	protected float maxDistance = 3f;

	protected string entityCode;

	protected bool stuck;

	protected bool onlyIfLowerId;

	protected bool allowTeleport;

	protected float teleportAfterRange;

	protected int teleportToRange;

	public int TeleportMaxRange;

	public float minSeekSeconds = 3f;

	protected Vec3d targetOffset = new Vec3d();

	protected Vec3d initialTargetPos;

	public int allowTeleportCount;

	private float executingSeconds;

	private int stuckCounter;

	private long cooldownUntilTotalMs;

	public AiTaskStayCloseToEntity(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		moveSpeed = taskConfig["movespeed"].AsFloat(0.03f);
		range = taskConfig["searchRange"].AsFloat(8f);
		maxDistance = taskConfig["maxDistance"].AsFloat(3f);
		minSeekSeconds = taskConfig["minSeekSeconds"].AsFloat(3f);
		onlyIfLowerId = taskConfig["onlyIfLowerId"].AsBool();
		entityCode = taskConfig["entityCode"].AsString();
		allowTeleport = taskConfig["allowTeleport"].AsBool();
		teleportAfterRange = taskConfig["teleportAfterRange"].AsFloat(30f);
		teleportToRange = taskConfig["teleportToRange"].AsInt(1);
		TeleportMaxRange = taskConfig["teleportMaxRange"].AsInt(int.MaxValue);
	}

	protected override void SetDefaultValues()
	{
		base.SetDefaultValues();
		ExecutionChance = 0.01;
	}

	public override bool ShouldExecute()
	{
		if (!PreconditionsSatisfied())
		{
			return false;
		}
		if (base.rand.NextDouble() > ExecutionChance)
		{
			return false;
		}
		if (stuckCounter > 3)
		{
			stuckCounter = 0;
			cooldownUntilTotalMs = entity.World.ElapsedMilliseconds + 60000;
		}
		if (entity.World.ElapsedMilliseconds < cooldownUntilMs)
		{
			return false;
		}
		if (targetEntity == null || !targetEntity.Alive)
		{
			if (onlyIfLowerId)
			{
				targetEntity = entity.World.GetNearestEntity(entity.Pos.XYZ, range, 2f, (Entity e) => e.EntityId < entity.EntityId && e.Code.Path.Equals(entityCode));
			}
			else
			{
				targetEntity = entity.World.GetNearestEntity(entity.Pos.XYZ, range, 2f, (Entity e) => e.Code.Path.Equals(entityCode));
			}
		}
		if (targetEntity != null && (!targetEntity.Alive || targetEntity.ShouldDespawn))
		{
			targetEntity = null;
		}
		if (targetEntity == null)
		{
			return false;
		}
		double x = targetEntity.Pos.X;
		double y = targetEntity.Pos.Y;
		double z = targetEntity.Pos.Z;
		return (double)entity.Pos.SquareDistanceTo(x, y, z) > (double)(maxDistance * maxDistance);
	}

	public override void StartExecute()
	{
		base.StartExecute();
		executingSeconds = 0f;
		initialTargetPos = targetEntity.Pos.XYZ;
		if (targetEntity.Pos.DistanceTo(entity.Pos) > (double)TeleportMaxRange)
		{
			stuck = true;
			return;
		}
		float size = targetEntity.SelectionBox.XSize;
		pathTraverser.NavigateTo_Async(targetEntity.Pos.XYZ, moveSpeed, size + 0.2f, OnGoalReached, OnStuck, OnNoPath, 1000, 1);
		targetOffset.Set(entity.World.Rand.NextDouble() * 2.0 - 1.0, 0.0, entity.World.Rand.NextDouble() * 2.0 - 1.0);
		stuck = false;
	}

	public override bool CanContinueExecute()
	{
		return pathTraverser.Ready;
	}

	public override bool ContinueExecute(float dt)
	{
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		if (initialTargetPos.DistanceTo(targetEntity.Pos.XYZ) > 3f)
		{
			initialTargetPos = targetEntity.Pos.XYZ;
			pathTraverser.Retarget();
		}
		double x = targetEntity.Pos.X + targetOffset.X;
		double y = targetEntity.Pos.Y;
		double z = targetEntity.Pos.Z + targetOffset.Z;
		pathTraverser.CurrentTarget.X = x;
		pathTraverser.CurrentTarget.Y = y;
		pathTraverser.CurrentTarget.Z = z;
		float dist = entity.Pos.SquareDistanceTo(x, y, z);
		if (dist < 9f)
		{
			pathTraverser.Stop();
			return false;
		}
		if ((allowTeleport || allowTeleportCount > 0) && executingSeconds > 4f && (dist > teleportAfterRange * teleportAfterRange || stuck) && entity.World.Rand.NextDouble() < 0.05)
		{
			tryTeleport();
		}
		executingSeconds += dt;
		if (stuck || !pathTraverser.Active)
		{
			return executingSeconds < minSeekSeconds;
		}
		return true;
	}

	private Vec3d findDecentTeleportPos()
	{
		IBlockAccessor ba = entity.World.BlockAccessor;
		Random rnd = entity.World.Rand;
		Vec3d pos = new Vec3d();
		BlockPos bpos = new BlockPos(targetEntity.Pos.Dimension);
		for (int i = teleportToRange; i < teleportToRange + 30; i++)
		{
			float range = GameMath.Clamp((float)i / 5f, 2f, 4.5f);
			double rndx = rnd.NextDouble() * 2.0 * (double)range - (double)range;
			double rndz = rnd.NextDouble() * 2.0 * (double)range - (double)range;
			for (int j = 0; j < 8; j++)
			{
				int dy = (1 - j % 2 * 2) * (int)Math.Ceiling((float)j / 2f);
				pos.Set(targetEntity.Pos.X + rndx, targetEntity.Pos.Y + (double)dy, targetEntity.Pos.Z + rndz);
				bpos.Set((int)pos.X, (int)pos.Y, (int)pos.Z);
				Block aboveBlock = ba.GetBlock(bpos);
				Cuboidf[] boxes = aboveBlock.GetCollisionBoxes(ba, bpos);
				if (boxes != null && boxes.Length != 0)
				{
					continue;
				}
				JsonObject attributes = aboveBlock.Attributes;
				if (attributes != null && attributes["insideDamage"].AsInt() > 0)
				{
					continue;
				}
				bpos.Set((int)pos.X, (int)pos.Y - 1, (int)pos.Z);
				Block belowBlock = ba.GetBlock(bpos);
				boxes = belowBlock.GetCollisionBoxes(ba, bpos);
				if (boxes == null || boxes.Length == 0)
				{
					continue;
				}
				JsonObject attributes2 = belowBlock.Attributes;
				if (attributes2 == null || attributes2["insideDamage"].AsInt() <= 0)
				{
					pos.Y = (float)((int)pos.Y - 1) + boxes.Max((Cuboidf c) => c.Y2);
					return pos;
				}
			}
		}
		return null;
	}

	protected void tryTeleport()
	{
		if ((!allowTeleport && allowTeleportCount <= 0) || targetEntity == null)
		{
			return;
		}
		Vec3d pos = findDecentTeleportPos();
		if (pos != null)
		{
			entity.TeleportToDouble(pos.X, pos.Y, pos.Z, delegate
			{
				initialTargetPos = targetEntity.Pos.XYZ;
				pathTraverser.Retarget();
				allowTeleportCount = Math.Max(0, allowTeleportCount - 1);
			});
		}
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

	protected void OnStuck()
	{
		stuck = true;
	}

	public void OnNoPath()
	{
	}

	protected virtual void OnGoalReached()
	{
	}
}
