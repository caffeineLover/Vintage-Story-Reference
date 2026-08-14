using System;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskFlyCircle : AiTaskTargetableAt
{
	[JsonProperty]
	protected bool stayNearSpawn;

	[JsonProperty]
	protected float minRadius = 10f;

	[JsonProperty]
	protected float maxRadius = 20f;

	[JsonProperty]
	protected float height = 5f;

	protected double desiredYPos;

	[JsonProperty]
	protected float moveSpeed = 0.04f;

	protected float direction = 1f;

	protected float directionChangeCoolDown = 60f;

	protected double desiredRadius;

	public AiTaskFlyCircle(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		direction = ((taskConfig["direction"].AsString("left") == "left") ? 1 : (-1));
	}

	public override bool ShouldExecute()
	{
		return true;
	}

	public override void StartExecute()
	{
		desiredRadius = minRadius + (float)world.Rand.NextDouble() * (maxRadius - minRadius);
		if (stayNearSpawn)
		{
			CenterPos = SpawnPos;
		}
		else
		{
			float randomYaw = (float)world.Rand.NextDouble() * ((float)Math.PI * 2f);
			double randomX = desiredRadius * Math.Sin(randomYaw);
			double randomZ = desiredRadius * Math.Cos(randomYaw);
			CenterPos = entity.Pos.XYZ.Add(randomX, 0.0, randomZ);
		}
		base.StartExecute();
	}

	public override bool ContinueExecute(float dt)
	{
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		if ((int)CenterPos.Y / 32768 != entity.Pos.Dimension)
		{
			return false;
		}
		if (entity.OnGround || entity.World.Rand.NextDouble() < 0.03)
		{
			UpdateFlyHeight();
		}
		double yMot = GameMath.Clamp(desiredYPos - entity.Pos.Y, -0.33, 0.33);
		double num = entity.Pos.X - CenterPos.X;
		double dz = entity.Pos.Z - CenterPos.Z;
		double currentRadius = Math.Sqrt(num * num + dz * dz);
		double offs = desiredRadius - currentRadius;
		float targetYaw = (float)Math.Atan2(num, dz) + (float)Math.PI / 2f + 0.1f * direction;
		if (offs < -1.0)
		{
			targetYaw += GameMath.Clamp((float)(0.0 - offs) / 13f, 0f, (float)Math.PI / 2f);
		}
		if (offs > 1.0)
		{
			targetYaw -= GameMath.Clamp((float)offs / 13f, 0f, (float)Math.PI / 2f);
		}
		entity.Pos.Yaw = targetYaw;
		float bla = (float)GameMath.Clamp(offs / 20.0, -1.0, 1.0);
		double cosYaw = Math.Cos(entity.Pos.Yaw - bla);
		double sinYaw = Math.Sin(entity.Pos.Yaw - bla);
		entity.Controls.WalkVector.Set(sinYaw, yMot, cosYaw);
		entity.Controls.WalkVector.Mul(moveSpeed);
		if (yMot < 0.0)
		{
			entity.Controls.WalkVector.Mul(0.5);
		}
		if (entity.Swimming)
		{
			entity.Controls.WalkVector.Y = 2f * moveSpeed;
			entity.Controls.FlyVector.Y = 2f * moveSpeed;
		}
		double speed = entity.Pos.Motion.Length();
		if (speed > 0.01)
		{
			entity.Pos.Roll = (float)Math.Asin(GameMath.Clamp((0.0 - entity.Pos.Motion.Y) / speed, -1.0, 1.0));
		}
		directionChangeCoolDown = Math.Max(0f, directionChangeCoolDown - dt);
		if (entity.CollidedHorizontally && directionChangeCoolDown <= 0f)
		{
			directionChangeCoolDown = 2f;
			direction *= -1f;
		}
		return entity.Alive;
	}

	protected void UpdateFlyHeight()
	{
		int terrainYPos = entity.World.BlockAccessor.GetTerrainMapheightAt(entity.Pos.AsBlockPos);
		int tries = 10;
		int dim = 32768 * entity.Pos.Dimension;
		while (tries-- > 0 && entity.World.BlockAccessor.GetBlockRaw((int)entity.Pos.X, terrainYPos + dim, (int)entity.Pos.Z, 2).IsLiquid())
		{
			terrainYPos++;
		}
		desiredYPos = (float)terrainYPos + height;
	}
}
