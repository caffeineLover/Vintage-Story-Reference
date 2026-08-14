using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskFlyWander : AiTaskBase
{
	private bool stayNearSpawn;

	private float radius;

	private float height;

	private float minDistance;

	protected double desiredYPos;

	protected float moveSpeed = 0.04f;

	public Vec3d SpawnPos;

	public Vec3d targetPos;

	private float targetTolerangeRange;

	public AiTaskFlyWander(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		stayNearSpawn = taskConfig["stayNearSpawn"].AsBool();
		radius = taskConfig["radius"].AsFloat(10f);
		height = taskConfig["height"].AsFloat(5f);
		minDistance = taskConfig["minDistance"].AsFloat(10f);
		moveSpeed = taskConfig["moveSpeed"].AsFloat(0.04f);
		targetTolerangeRange = taskConfig["targetTolerangeRange"].AsFloat(15f);
	}

	public override void OnEntityLoaded()
	{
		loadOrCreateSpawnPos();
	}

	public override void OnEntitySpawn()
	{
		loadOrCreateSpawnPos();
	}

	public void loadOrCreateSpawnPos()
	{
		if (entity.WatchedAttributes.HasAttribute("spawnPosX"))
		{
			SpawnPos = new Vec3d(entity.WatchedAttributes.GetDouble("spawnPosX"), entity.WatchedAttributes.GetDouble("spawnPosY"), entity.WatchedAttributes.GetDouble("spawnPosZ"));
			return;
		}
		SpawnPos = entity.Pos.XYZ;
		entity.WatchedAttributes.SetDouble("spawnPosX", SpawnPos.X);
		entity.WatchedAttributes.SetDouble("spawnPosY", SpawnPos.Y);
		entity.WatchedAttributes.SetDouble("spawnPosZ", SpawnPos.Z);
	}

	public override bool ShouldExecute()
	{
		if (cooldownUntilMs > entity.World.ElapsedMilliseconds)
		{
			return false;
		}
		return true;
	}

	public override void StartExecute()
	{
		base.StartExecute();
		Vec3d fromPos = (stayNearSpawn ? SpawnPos : entity.Pos.XYZ);
		double rndx = 0.0;
		double rndz = 0.0;
		for (int i = 0; i < 10; i++)
		{
			float yaw = (float)world.Rand.NextDouble() * ((float)Math.PI * 2f);
			rndx = (double)radius * Math.Sin(yaw);
			rndz = (double)radius * Math.Cos(yaw);
			if (fromPos.AddCopy(rndx, 0.0, rndz).HorizontalSquareDistanceTo(entity.Pos.XYZ) > minDistance)
			{
				break;
			}
		}
		targetPos = fromPos.AddCopy(rndx, 0.0, rndz);
	}

	public override bool ContinueExecute(float dt)
	{
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		if (entity.OnGround || entity.World.Rand.NextDouble() < 0.03)
		{
			ReadjustFlyHeight();
		}
		double yMot = GameMath.Clamp(desiredYPos - entity.Pos.Y, -0.2, 0.2);
		double dx = targetPos.X - entity.Pos.X;
		double dz = targetPos.Z - entity.Pos.Z;
		float targetYaw = (float)Math.Atan2(dx, dz);
		entity.Pos.Yaw += GameMath.AngleRadDistance(entity.Pos.Yaw, targetYaw) * dt;
		double cosYaw = Math.Cos(entity.Pos.Yaw);
		double sinYaw = Math.Sin(entity.Pos.Yaw);
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
		if (entity.Alive)
		{
			return dx * dx + dz * dz > (double)(targetTolerangeRange * targetTolerangeRange);
		}
		return false;
	}

	protected void ReadjustFlyHeight()
	{
		int terrainYPos = entity.World.BlockAccessor.GetTerrainMapheightAt(entity.Pos.AsBlockPos);
		int tries = 10;
		while (tries-- > 0 && entity.World.BlockAccessor.GetBlockRaw((int)entity.Pos.X, terrainYPos, (int)entity.Pos.Z, 2).IsLiquid())
		{
			terrainYPos++;
		}
		desiredYPos = (float)terrainYPos + height;
	}
}
