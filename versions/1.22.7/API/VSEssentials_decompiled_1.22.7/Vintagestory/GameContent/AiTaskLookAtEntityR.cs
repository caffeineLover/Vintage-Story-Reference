using System;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskLookAtEntityR : AiTaskBaseTargetableR
{
	protected float minTurnAnglePerSec;

	protected float maxTurnAnglePerSec;

	protected float currentTurnRadPerSec;

	protected const float yawChangeRateToStop = 0.01f;

	private AiTaskLookAtEntityConfig Config => GetConfig<AiTaskLookAtEntityConfig>();

	public AiTaskLookAtEntityR(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		baseConfig = AiTaskBaseR.LoadConfig<AiTaskLookAtEntityConfig>(entity, taskConfig, aiConfig);
	}

	public override bool ShouldExecute()
	{
		if (!PreconditionsSatisficed())
		{
			return false;
		}
		return SearchForTarget();
	}

	public override void StartExecute()
	{
		base.StartExecute();
		ITreeAttribute pathfinder = entity?.Properties.Server?.Attributes?.GetTreeAttribute("pathfinder");
		if (pathfinder != null)
		{
			minTurnAnglePerSec = pathfinder.GetFloat("minTurnAnglePerSec", Config.DefaultMinTurnAngleDegPerSec);
			maxTurnAnglePerSec = pathfinder.GetFloat("maxTurnAnglePerSec", Config.DefaultMaxTurnAngleDegPerSec);
		}
		else
		{
			minTurnAnglePerSec = Config.DefaultMinTurnAngleDegPerSec;
			maxTurnAnglePerSec = Config.DefaultMaxTurnAngleDegPerSec;
		}
		currentTurnRadPerSec = minTurnAnglePerSec + (float)base.Rand.NextDouble() * (maxTurnAnglePerSec - minTurnAnglePerSec);
		currentTurnRadPerSec *= (float)Math.PI / 180f;
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
		FastVec3f targetVec = default(FastVec3f);
		targetVec.Set((float)(targetEntity.Pos.X - entity.Pos.X), (float)(targetEntity.Pos.Y - entity.Pos.Y), (float)(targetEntity.Pos.Z - entity.Pos.Z));
		float desiredYaw = (float)Math.Atan2(targetVec.X, targetVec.Z);
		if (Config.MaxTurnAngleRad < (float)Math.PI)
		{
			desiredYaw = GameMath.Clamp(desiredYaw, Config.SpawnAngleRad - Config.MaxTurnAngleRad, Config.SpawnAngleRad + Config.MaxTurnAngleRad);
		}
		float yawDistance = GameMath.AngleRadDistance(entity.Pos.Yaw, desiredYaw);
		entity.Pos.Yaw += GameMath.Clamp(yawDistance, (0f - currentTurnRadPerSec) * dt * GlobalConstants.OverallSpeedMultiplier, currentTurnRadPerSec * dt * GlobalConstants.OverallSpeedMultiplier);
		entity.Pos.Yaw %= (float)Math.PI * 2f;
		return Math.Abs(yawDistance) > 0.01f;
	}
}
