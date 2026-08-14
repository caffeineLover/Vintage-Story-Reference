using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskWanderR : AiTaskBaseR
{
	protected int failedWanders;

	protected long lastTimeInRangeMs;

	protected bool spawnPositionSet;

	protected Vec3d mainTarget = new Vec3d();

	protected Vec3d spawnPosition = new Vec3d();

	protected bool needsToTeleport;

	private readonly Vec4d bestTarget = new Vec4d();

	private readonly Vec4d currentTarget = new Vec4d();

	private readonly BlockPos blockPosBuffer = new BlockPos(0);

	private AiTaskWanderConfig Config => GetConfig<AiTaskWanderConfig>();

	protected float WanderRangeMul
	{
		get
		{
			return entity.Attributes.GetFloat("wanderRangeMul", 1f);
		}
		set
		{
			entity.Attributes.SetFloat("wanderRangeMul", value);
		}
	}

	protected int FailedConsecutivePathfinds
	{
		get
		{
			return entity.Attributes.GetInt("failedConsecutivePathfinds");
		}
		set
		{
			entity.Attributes.SetInt("failedConsecutivePathfinds", value);
		}
	}

	public AiTaskWanderR(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		baseConfig = AiTaskBaseR.LoadConfig<AiTaskWanderConfig>(entity, taskConfig, aiConfig);
		spawnPosition = new Vec3d(entity.Attributes.GetDouble("spawnX"), entity.Attributes.GetDouble("spawnY"), entity.Attributes.GetDouble("spawnZ"));
	}

	public override bool ShouldExecute()
	{
		if (!PreconditionsSatisficed())
		{
			return false;
		}
		failedWanders = 0;
		needsToTeleport = false;
		if (!Config.StayCloseToSpawn)
		{
			return LoadNextWanderTarget();
		}
		long currentTimeMs = entity.World.ElapsedMilliseconds;
		if ((double)entity.Pos.XYZ.SquareDistanceTo(spawnPosition.X, spawnPosition.Y, spawnPosition.Z) <= Config.MaxDistanceToSpawn * Config.MaxDistanceToSpawn)
		{
			lastTimeInRangeMs = currentTimeMs;
			return false;
		}
		if (currentTimeMs - lastTimeInRangeMs > Config.TeleportToSpawnTimeout && entity.World.GetNearestEntity(entity.Pos.XYZ, Config.NoPlayersRange, Config.NoPlayersRange, (Entity target) => target is EntityPlayer) == null)
		{
			needsToTeleport = true;
		}
		mainTarget = spawnPosition;
		return true;
	}

	public override void StartExecute()
	{
		base.StartExecute();
		if (needsToTeleport)
		{
			entity.TeleportTo((int)spawnPosition.X, (int)spawnPosition.Y, (int)spawnPosition.Z);
			stopTask = true;
		}
		else
		{
			pathTraverser.WalkTowards(mainTarget, Config.MoveSpeed, Config.MinDistanceToTarget, OnGoalReached, OnStuck);
		}
	}

	public override bool ContinueExecute(float dt)
	{
		if (!base.ContinueExecute(dt))
		{
			return false;
		}
		if (entity.Controls.IsClimbing && entity.Properties.CanClimbAnywhere && entity.ClimbingIntoFace != null)
		{
			BlockFacing climbingIntoFace = entity.ClimbingIntoFace;
			if (Math.Sign(climbingIntoFace.Normali.X) == Math.Sign(pathTraverser.CurrentTarget.X - entity.Pos.X))
			{
				pathTraverser.CurrentTarget.X = entity.Pos.X;
			}
			if (Math.Sign(climbingIntoFace.Normali.Y) == Math.Sign(pathTraverser.CurrentTarget.Y - entity.Pos.Y))
			{
				pathTraverser.CurrentTarget.Y = entity.Pos.Y;
			}
			if (Math.Sign(climbingIntoFace.Normali.Z) == Math.Sign(pathTraverser.CurrentTarget.Z - entity.Pos.Z))
			{
				pathTraverser.CurrentTarget.Z = entity.Pos.Z;
			}
		}
		if ((double)mainTarget.HorizontalSquareDistanceTo(entity.Pos.X, entity.Pos.Z) < 0.5)
		{
			pathTraverser.Stop();
			return false;
		}
		return true;
	}

	public override void FinishExecute(bool cancelled)
	{
		base.FinishExecute(cancelled);
		if (cancelled)
		{
			pathTraverser.Stop();
		}
	}

	public override void OnEntityLoaded()
	{
		if (!entity.Attributes.HasAttribute("spawnX"))
		{
			OnEntitySpawn();
		}
	}

	public override void OnEntitySpawn()
	{
		entity.Attributes.SetDouble("spawnX", entity.Pos.X);
		entity.Attributes.SetDouble("spawnY", entity.Pos.Y);
		entity.Attributes.SetDouble("spawnZ", entity.Pos.Z);
		spawnPosition.Set(entity.Pos.XYZ);
	}

	protected override bool CheckExecutionChance()
	{
		return base.Rand.NextDouble() <= (double)((failedWanders > 0) ? (1f - Config.ExecutionChance * 4f * (float)failedWanders) : Config.ExecutionChance);
	}

	protected virtual bool LoadNextWanderTarget()
	{
		EnumHabitat habitat = entity.Properties.Habitat;
		bool targetFound = false;
		if (FailedConsecutivePathfinds > 10)
		{
			WanderRangeMul = Math.Max(0.1f, WanderRangeMul * 0.9f);
		}
		else
		{
			WanderRangeMul = Math.Min(1f, WanderRangeMul * 1.1f);
			if (Config.DoRandomWanderRangeChanges && base.Rand.NextDouble() < 0.05)
			{
				WanderRangeMul = Math.Min(1f, WanderRangeMul * 1.5f);
			}
		}
		float wRangeMul = WanderRangeMul;
		if (Config.DoRandomWanderRangeChanges && base.Rand.NextDouble() < 0.05)
		{
			wRangeMul *= 3f;
		}
		for (int check = 0; check < Config.MaxBlocksChecked; check++)
		{
			double dx = Config.WanderRangeHorizontal.nextFloat() * (float)(base.Rand.Next(2) * 2 - 1) * wRangeMul;
			double dy = Config.WanderRangeVertical.nextFloat() * (float)(base.Rand.Next(2) * 2 - 1) * wRangeMul;
			double dz = Config.WanderRangeHorizontal.nextFloat() * (float)(base.Rand.Next(2) * 2 - 1) * wRangeMul;
			currentTarget.X = entity.Pos.X + dx;
			currentTarget.Y = entity.Pos.InternalY + dy;
			currentTarget.Z = entity.Pos.Z + dz;
			currentTarget.W = 1.0;
			if (Config.StayCloseToSpawn)
			{
				double distToEdge = (double)currentTarget.SquareDistanceTo(spawnPosition) / (Config.MaxDistanceToSpawn * Config.MaxDistanceToSpawn);
				currentTarget.W = 1.0 - distToEdge;
			}
			switch (habitat)
			{
			case EnumHabitat.Air:
			{
				int rainMapY = world.BlockAccessor.GetRainMapHeightAt((int)currentTarget.X, (int)currentTarget.Z);
				currentTarget.Y = Math.Min(currentTarget.Y, (float)rainMapY + Config.MaxHeight);
				if (entity.World.BlockAccessor.GetBlockRaw((int)currentTarget.X, (int)currentTarget.Y, (int)currentTarget.Z, 2).IsLiquid())
				{
					currentTarget.W = 0.0;
				}
				break;
			}
			case EnumHabitat.Land:
			{
				currentTarget.Y = MoveDownToFloor((int)currentTarget.X, currentTarget.Y, (int)currentTarget.Z);
				if (currentTarget.Y < 0.0)
				{
					currentTarget.W = 0.0;
					break;
				}
				if (entity.World.BlockAccessor.GetBlockRaw((int)currentTarget.X, (int)currentTarget.Y, (int)currentTarget.Z, 2).IsLiquid())
				{
					currentTarget.W /= 2.0;
				}
				bool stop = false;
				bool willFall = false;
				float angleHor = (float)Math.Atan2(dx, dz) + (float)Math.PI / 2f;
				Vec3d target1BlockAhead = new Vec3d(currentTarget.X, currentTarget.Y, currentTarget.Z).Ahead(1.0, 0f, angleHor);
				Vec3d startAhead = new Vec3d(entity.Pos.X, entity.Pos.Y, entity.Pos.Z).Ahead(1.0, 0f, angleHor);
				int prevY = (int)startAhead.Y;
				GameMath.BresenHamPlotLine2d((int)startAhead.X, (int)startAhead.Z, (int)target1BlockAhead.X, (int)target1BlockAhead.Z, delegate(int x, int z)
				{
					if (!stop)
					{
						double num = MoveDownToFloor(x, prevY, z);
						if (num < 0.0 || (double)prevY - num > 4.0)
						{
							willFall = true;
							stop = true;
						}
						if (num - (double)prevY > 2.0)
						{
							stop = true;
						}
						prevY = (int)num;
					}
				});
				if (willFall)
				{
					currentTarget.W = 0.0;
				}
				break;
			}
			case EnumHabitat.Sea:
				if (!entity.World.BlockAccessor.GetBlockRaw((int)currentTarget.X, (int)currentTarget.Y, (int)currentTarget.Z, 2).IsLiquid())
				{
					currentTarget.W = 0.0;
				}
				break;
			case EnumHabitat.Underwater:
				if (!entity.World.BlockAccessor.GetBlockRaw((int)currentTarget.X, (int)currentTarget.Y, (int)currentTarget.Z, 2).IsLiquid())
				{
					currentTarget.W = 0.0;
				}
				else
				{
					currentTarget.W = 1.0 / (Math.Abs(dy) + 1.0);
				}
				break;
			}
			if (currentTarget.W > 0.0)
			{
				for (int i = 0; i < BlockFacing.HORIZONTALS.Length; i++)
				{
					if (entity.World.BlockAccessor.IsSideSolid((int)currentTarget.X + BlockFacing.HORIZONTALS[i].Normali.X, (int)currentTarget.Y, (int)currentTarget.Z + BlockFacing.HORIZONTALS[i].Normali.Z, BlockFacing.HORIZONTALS[i].Opposite))
					{
						currentTarget.W *= 0.5;
					}
				}
			}
			if (!Config.IgnoreLightLevel && currentTarget.W != 0.0)
			{
				blockPosBuffer.Set((int)currentTarget.X, (int)currentTarget.Y, (int)currentTarget.Z);
				int lightDiff = Math.Abs(Config.PreferredLightLevel - entity.World.BlockAccessor.GetLightLevel(blockPosBuffer, Config.PreferredLightLevelType));
				currentTarget.W /= Math.Max(1, lightDiff);
			}
			if (!targetFound || currentTarget.W > bestTarget.W)
			{
				targetFound = true;
				bestTarget.Set(currentTarget.X, currentTarget.Y, currentTarget.Z, currentTarget.W);
				if (currentTarget.W >= 1.0)
				{
					break;
				}
			}
		}
		if (bestTarget.W > 0.0)
		{
			FailedConsecutivePathfinds = Math.Max(FailedConsecutivePathfinds - 3, 0);
			mainTarget.Set(bestTarget.X, bestTarget.Y, bestTarget.Z);
			return true;
		}
		FailedConsecutivePathfinds++;
		return false;
	}

	protected virtual double MoveDownToFloor(int x, double y, int z)
	{
		int tries = 5;
		while (tries-- > 0)
		{
			if (world.BlockAccessor.IsSideSolid(x, (int)y, z, BlockFacing.UP))
			{
				return y + 1.0;
			}
			y -= 1.0;
		}
		return -1.0;
	}

	protected virtual void OnStuck()
	{
		stopTask = true;
		failedWanders++;
	}

	protected virtual void OnGoalReached()
	{
		stopTask = true;
		failedWanders = 0;
	}
}
