using System;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class AiTaskButterflyFeedOnFlowers : AiTaskBase
{
	public Vec3d MainTarget;

	private int taskState;

	private float moveSpeed = 0.03f;

	private float targetDistance = 0.07f;

	private bool awaitReached = true;

	private BlockPos tmpPos = new BlockPos(0);

	private double feedTime;

	[JsonProperty]
	[Obsolete("Use ExecutionChance instead")]
	private double searchFrequency
	{
		set
		{
			ExecutionChance = value;
		}
	}

	public AiTaskButterflyFeedOnFlowers(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		(entity.Api as ICoreServerAPI).Event.DidBreakBlock += Event_DidBreakBlock;
		targetDistance = taskConfig["targetDistance"].AsFloat(0.07f);
		moveSpeed = taskConfig["movespeed"].AsFloat(0.03f);
		awaitReached = taskConfig["awaitReached"].AsBool(defaultValue: true);
	}

	protected override void SetDefaultValues()
	{
		base.SetDefaultValues();
		ExecutionChance = 0.07;
	}

	public override void OnEntityDespawn(EntityDespawnData reason)
	{
		(entity.Api as ICoreServerAPI).Event.DidBreakBlock -= Event_DidBreakBlock;
	}

	private void Event_DidBreakBlock(IServerPlayer byPlayer, int oldblockId, BlockSelection blockSel)
	{
		if (tmpPos != null && blockSel.Position.Equals(tmpPos))
		{
			taskState = 4;
		}
	}

	public override bool ShouldExecute()
	{
		if (base.rand.NextDouble() > ExecutionChance)
		{
			return false;
		}
		double dx = base.rand.NextDouble() * 4.0 - 2.0;
		double dz = base.rand.NextDouble() * 4.0 - 2.0;
		tmpPos.Set((int)(entity.Pos.X + dx), 0, (int)(entity.Pos.Z + dz));
		tmpPos.SetDimension(entity.Pos.Dimension);
		tmpPos.Y = entity.World.BlockAccessor.GetTerrainMapheightAt(tmpPos) + 1;
		Block block = entity.World.BlockAccessor.GetBlock(tmpPos);
		if (block.Attributes == null)
		{
			return false;
		}
		if (block.Attributes["butterflyFeed"].AsBool())
		{
			if (entity.World.BlockAccessor.GetBlock(tmpPos, 2).BlockId != 0)
			{
				return false;
			}
			double topPos = block.Attributes["sitHeight"].AsDouble(block.TopMiddlePos.Y);
			EnumWindBitMode windMode = (EnumWindBitMode)block.Attributes["butteflyWindMode"].AsInt(2);
			entity.WatchedAttributes.SetInt("windMode", (int)windMode);
			MainTarget = tmpPos.ToVec3d().Add(block.TopMiddlePos.X, topPos, block.TopMiddlePos.Z);
			return true;
		}
		return false;
	}

	public override void StartExecute()
	{
		base.StartExecute();
		taskState = 0;
		pathTraverser.WalkTowards(MainTarget, moveSpeed, targetDistance, OnGoalReached, OnStuck);
		feedTime = 3.0 + base.rand.NextDouble() * 10.0;
	}

	public override bool ContinueExecute(float dt)
	{
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		if (taskState == 1)
		{
			entity.Pos.Motion.Set(0.0, 0.0, 0.0);
			entity.AnimManager.StartAnimation("feed");
			taskState = 2;
		}
		if (taskState == 2)
		{
			feedTime -= dt;
		}
		if (feedTime <= 0.0)
		{
			return false;
		}
		return taskState < 3;
	}

	public override void FinishExecute(bool cancelled)
	{
		base.FinishExecute(cancelled);
		if (cancelled)
		{
			pathTraverser.Stop();
		}
		entity.StopAnimation("feed");
	}

	private void OnStuck()
	{
		taskState = 3;
	}

	private void OnGoalReached()
	{
		taskState = 1;
	}
}
