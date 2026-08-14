using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public sealed class AiTaskLookAtEntityConversable : AiTaskBaseR
{
	private float minTurnAnglePerSec;

	private float maxTurnAnglePerSec;

	private float curTurnRadPerSec;

	private readonly Entity target;

	public AiTaskLookAtEntityConversable(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		world.Logger.Error("This AI task 'AiTaskLookAtEntityConversable' can only be created from code.");
		throw new InvalidOperationException("This AI task can only be created from code.");
	}

	public AiTaskLookAtEntityConversable(EntityAgent entity, Entity target)
		: base(entity)
	{
		this.target = target;
	}

	public override bool ShouldExecute()
	{
		return false;
	}

	public override void StartExecute()
	{
		if (entity.Properties.Server?.Attributes != null)
		{
			minTurnAnglePerSec = entity.Properties.Server.Attributes.GetTreeAttribute("pathfinder").GetFloat("minTurnAnglePerSec", 250f);
			maxTurnAnglePerSec = entity.Properties.Server.Attributes.GetTreeAttribute("pathfinder").GetFloat("maxTurnAnglePerSec", 450f);
		}
		else
		{
			minTurnAnglePerSec = 250f;
			maxTurnAnglePerSec = 450f;
		}
		curTurnRadPerSec = minTurnAnglePerSec + (float)base.Rand.NextDouble() * (maxTurnAnglePerSec - minTurnAnglePerSec);
		curTurnRadPerSec *= (float)Math.PI / 180f;
	}

	public override bool ContinueExecute(float dt)
	{
		FastVec3f targetVec = default(FastVec3f);
		targetVec.Set((float)(target.Pos.X - entity.Pos.X), (float)(target.Pos.Y - entity.Pos.Y), (float)(target.Pos.Z - entity.Pos.Z));
		float desiredYaw = (float)Math.Atan2(targetVec.X, targetVec.Z);
		float yawDistance = GameMath.AngleRadDistance(entity.Pos.Yaw, desiredYaw);
		entity.Pos.Yaw += GameMath.Clamp(yawDistance, (0f - curTurnRadPerSec) * dt, curTurnRadPerSec * dt);
		entity.Pos.Yaw %= (float)Math.PI * 2f;
		return (double)Math.Abs(yawDistance) > 0.01;
	}
}
