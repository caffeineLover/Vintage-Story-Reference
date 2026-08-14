using System;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskWander : AiTaskBase
{
	public Vec3d MainTarget;

	private bool done;

	private float moveSpeed = 0.03f;

	private float maxHeight = 7f;

	private float? preferredLightLevel;

	private float targetDistance = 0.12f;

	private NatFloat wanderRangeHorizontal = NatFloat.createStrongerInvexp(3f, 40f);

	private NatFloat wanderRangeVertical = NatFloat.createStrongerInvexp(3f, 10f);

	public bool StayCloseToSpawn;

	public Vec3d SpawnPosition;

	public double MaxDistanceToSpawn;

	private long lastTimeInRangeMs;

	private int failedWanders;

	private bool needsToTele;

	private float tryStartAnimAgain = 0.1f;

	[JsonProperty]
	[Obsolete("Use ExecutionChance instead")]
	private double wanderChance
	{
		get
		{
			return ExecutionChance;
		}
		set
		{
			ExecutionChance = value;
		}
	}

	public float WanderRangeMul
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

	public int FailedConsecutivePathfinds
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

	public AiTaskWander(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		float wanderRangeMin = 3f;
		float wanderRangeMax = 30f;
		if (taskConfig["maxDistanceToSpawn"].Exists)
		{
			StayCloseToSpawn = true;
			MaxDistanceToSpawn = taskConfig["maxDistanceToSpawn"].AsDouble(10.0);
			SpawnPosition = new Vec3d(entity.Attributes.GetDouble("spawnX"), entity.Attributes.GetDouble("spawnY"), entity.Attributes.GetDouble("spawnZ"));
			BlockPos importOffset = entity.WatchedAttributes.GetBlockPos("importOffset");
			if (importOffset != null)
			{
				SpawnPosition.Add(importOffset);
			}
		}
		targetDistance = taskConfig["targetDistance"].AsFloat(0.12f);
		moveSpeed = taskConfig["movespeed"].AsFloat(0.03f);
		wanderRangeMin = taskConfig["wanderRangeMin"].AsFloat(3f);
		wanderRangeMax = taskConfig["wanderRangeMax"].AsFloat(30f);
		wanderRangeHorizontal = NatFloat.createInvexp(wanderRangeMin, wanderRangeMax);
		maxHeight = taskConfig["maxHeight"].AsFloat(7f);
		preferredLightLevel = taskConfig["preferredLightLevel"].AsFloat(-99f);
		if (preferredLightLevel < 0f)
		{
			preferredLightLevel = null;
		}
	}

	protected override void SetDefaultValues()
	{
		base.SetDefaultValues();
		ExecutionChance = 0.02;
	}

	public override void OnEntityLoaded()
	{
		if (StayCloseToSpawn && (SpawnPosition == null || (SpawnPosition.X == 0.0 && SpawnPosition.Z == 0.0) || !entity.Attributes.HasAttribute("spawnX")))
		{
			OnEntitySpawn();
		}
	}

	public override void OnEntitySpawn()
	{
		entity.Attributes.SetDouble("spawnX", entity.Pos.X);
		entity.Attributes.SetDouble("spawnY", entity.Pos.Y);
		entity.Attributes.SetDouble("spawnZ", entity.Pos.Z);
		SpawnPosition = entity.Pos.XYZ;
	}

	public Vec3d loadNextWanderTarget()
	{
		EnumHabitat habitat = entity.Properties.Habitat;
		int tries = 9;
		Vec4d bestTarget = null;
		Vec4d curTarget = new Vec4d();
		BlockPos tmpPos = new BlockPos(entity.Pos.Dimension);
		if (FailedConsecutivePathfinds > 10)
		{
			WanderRangeMul = Math.Max(0.1f, WanderRangeMul * 0.9f);
		}
		else
		{
			WanderRangeMul = Math.Min(1f, WanderRangeMul * 1.1f);
			if (base.rand.NextDouble() < 0.05)
			{
				WanderRangeMul = Math.Min(1f, WanderRangeMul * 1.5f);
			}
		}
		float wRangeMul = WanderRangeMul;
		if (base.rand.NextDouble() < 0.05)
		{
			wRangeMul *= 3f;
		}
		while (tries-- > 0)
		{
			double dx = wanderRangeHorizontal.nextFloat() * (float)(base.rand.Next(2) * 2 - 1) * wRangeMul;
			double dy = wanderRangeVertical.nextFloat() * (float)(base.rand.Next(2) * 2 - 1) * wRangeMul;
			double dz = wanderRangeHorizontal.nextFloat() * (float)(base.rand.Next(2) * 2 - 1) * wRangeMul;
			curTarget.X = entity.Pos.X + dx;
			curTarget.Y = entity.Pos.InternalY + dy;
			curTarget.Z = entity.Pos.Z + dz;
			curTarget.W = 1.0;
			if (StayCloseToSpawn)
			{
				double distToEdge = (double)curTarget.SquareDistanceTo(SpawnPosition) / (MaxDistanceToSpawn * MaxDistanceToSpawn);
				curTarget.W = 1.0 - distToEdge;
			}
			switch (habitat)
			{
			case EnumHabitat.Air:
			{
				int rainMapY = world.BlockAccessor.GetRainMapHeightAt((int)curTarget.X, (int)curTarget.Z);
				curTarget.Y = Math.Min(curTarget.Y, (float)rainMapY + maxHeight);
				if (entity.World.BlockAccessor.GetBlockRaw((int)curTarget.X, (int)curTarget.Y, (int)curTarget.Z, 2).IsLiquid())
				{
					curTarget.W = 0.0;
				}
				break;
			}
			case EnumHabitat.Land:
			{
				curTarget.Y = moveDownToFloor((int)curTarget.X, curTarget.Y, (int)curTarget.Z);
				if (curTarget.Y < 0.0)
				{
					curTarget.W = 0.0;
					break;
				}
				if (entity.World.BlockAccessor.GetBlockRaw((int)curTarget.X, (int)curTarget.Y, (int)curTarget.Z, 2).IsLiquid())
				{
					curTarget.W /= 2.0;
				}
				bool stop = false;
				bool willFall = false;
				float angleHor = (float)Math.Atan2(dx, dz) + (float)Math.PI / 2f;
				Vec3d target1BlockAhead = curTarget.XYZ.Ahead(1.0, 0f, angleHor);
				Vec3d startAhead = entity.Pos.XYZ.Ahead(1.0, 0f, angleHor);
				int prevY = (int)startAhead.Y;
				GameMath.BresenHamPlotLine2d((int)startAhead.X, (int)startAhead.Z, (int)target1BlockAhead.X, (int)target1BlockAhead.Z, delegate(int x, int z)
				{
					if (!stop)
					{
						double num = moveDownToFloor(x, prevY, z);
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
					curTarget.W = 0.0;
				}
				break;
			}
			case EnumHabitat.Sea:
				if (!entity.World.BlockAccessor.GetBlockRaw((int)curTarget.X, (int)curTarget.Y, (int)curTarget.Z, 2).IsLiquid())
				{
					curTarget.W = 0.0;
				}
				break;
			case EnumHabitat.Underwater:
				if (!entity.World.BlockAccessor.GetBlockRaw((int)curTarget.X, (int)curTarget.Y, (int)curTarget.Z, 2).IsLiquid())
				{
					curTarget.W = 0.0;
				}
				else
				{
					curTarget.W = 1.0 / (Math.Abs(dy) + 1.0);
				}
				break;
			}
			if (curTarget.W > 0.0)
			{
				for (int i = 0; i < BlockFacing.HORIZONTALS.Length; i++)
				{
					BlockFacing face = BlockFacing.HORIZONTALS[i];
					if (entity.World.BlockAccessor.IsSideSolid((int)curTarget.X + face.Normali.X, (int)curTarget.Y, (int)curTarget.Z + face.Normali.Z, face.Opposite))
					{
						curTarget.W *= 0.5;
					}
				}
			}
			if (preferredLightLevel.HasValue && curTarget.W != 0.0)
			{
				tmpPos.Set((int)curTarget.X, (int)curTarget.Y, (int)curTarget.Z);
				int lightdiff = Math.Abs((int)preferredLightLevel.Value - entity.World.BlockAccessor.GetLightLevel(tmpPos, EnumLightLevelType.MaxLight));
				curTarget.W /= Math.Max(1, lightdiff);
			}
			if (bestTarget == null || curTarget.W > bestTarget.W)
			{
				bestTarget = new Vec4d(curTarget.X, curTarget.Y, curTarget.Z, curTarget.W);
				if (curTarget.W >= 1.0)
				{
					break;
				}
			}
		}
		if (bestTarget.W > 0.0)
		{
			FailedConsecutivePathfinds = Math.Max(FailedConsecutivePathfinds - 3, 0);
			return bestTarget.XYZ;
		}
		FailedConsecutivePathfinds++;
		return null;
	}

	private double moveDownToFloor(int x, double y, int z)
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

	public override bool ShouldExecute()
	{
		if (!PreconditionsSatisfied())
		{
			return false;
		}
		if (base.rand.NextDouble() > ((failedWanders > 0) ? (1.0 - ExecutionChance * 4.0 * (double)failedWanders) : ExecutionChance))
		{
			failedWanders = 0;
			return false;
		}
		needsToTele = false;
		if (StayCloseToSpawn)
		{
			double num = entity.Pos.XYZ.SquareDistanceTo(SpawnPosition);
			long ellapsedMs = entity.World.ElapsedMilliseconds;
			if (num > MaxDistanceToSpawn * MaxDistanceToSpawn)
			{
				if (ellapsedMs - lastTimeInRangeMs > 120000 && entity.World.GetNearestEntity(entity.Pos.XYZ, 15f, 15f, (Entity e) => e is EntityPlayer) == null)
				{
					needsToTele = true;
				}
				MainTarget = SpawnPosition.Clone();
				return true;
			}
			lastTimeInRangeMs = ellapsedMs;
		}
		MainTarget = loadNextWanderTarget();
		return MainTarget != null;
	}

	public override void StartExecute()
	{
		base.StartExecute();
		if (needsToTele && StayCloseToSpawn)
		{
			entity.TeleportTo(SpawnPosition);
			done = true;
		}
		else
		{
			done = false;
			pathTraverser.WalkTowards(MainTarget, moveSpeed, targetDistance, OnGoalReached, OnStuck);
			tryStartAnimAgain = 0.1f;
		}
	}

	public override bool ContinueExecute(float dt)
	{
		base.ContinueExecute(dt);
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		if (animMeta != null && tryStartAnimAgain > 0f && (tryStartAnimAgain -= dt) <= 0f)
		{
			entity.AnimManager.StartAnimation(animMeta);
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
		if ((double)MainTarget.HorizontalSquareDistanceTo(entity.Pos.X, entity.Pos.Z) < 0.5)
		{
			pathTraverser.Stop();
			return false;
		}
		return !done;
	}

	public override void FinishExecute(bool cancelled)
	{
		base.FinishExecute(cancelled);
		if (cancelled)
		{
			pathTraverser.Stop();
		}
	}

	private void OnStuck()
	{
		done = true;
		failedWanders++;
	}

	private void OnGoalReached()
	{
		done = true;
		failedWanders = 0;
	}
}
