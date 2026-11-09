using System;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class AiTaskShootAtEntityR : AiTaskBaseTargetableR
{
	protected float minTurnAnglePerSec;

	protected float maxTurnAnglePerSec;

	protected float currentTurnRadPerSec;

	protected bool alreadyThrown;

	protected long previousTargetId;

	protected float currentYawDispersion;

	protected float currentPitchDispersion;

	protected const string defaultRockType = "granite";

	protected readonly NatFloat randomFloat;

	private AiTaskShootAtEntityConfig Config => GetConfig<AiTaskShootAtEntityConfig>();

	public AiTaskShootAtEntityR(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		baseConfig = AiTaskBaseR.LoadConfig<AiTaskShootAtEntityConfig>(entity, taskConfig, aiConfig);
		randomFloat = new NatFloat(0f, 1f, Config.DispersionDistribution);
	}

	public override bool ShouldExecute()
	{
		if (!PreconditionsSatisficed() && (!Config.RetaliateUnconditionally || !base.RecentlyAttacked))
		{
			return false;
		}
		if (!CheckAndResetSearchCooldown())
		{
			return false;
		}
		return SearchForTarget();
	}

	public override void StartExecute()
	{
		if (targetEntity != null)
		{
			base.StartExecute();
			ITreeAttribute pathfinder = entity.Properties.Server?.Attributes?.GetTreeAttribute("pathfinder");
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
			alreadyThrown = false;
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
		AdjustYaw(dt);
		if (entity.World.ElapsedMilliseconds - executionStartTimeMs > Config.ThrowAtMs && !alreadyThrown)
		{
			SetOrAdjustDispersion();
			ShootProjectile();
			alreadyThrown = true;
		}
		return true;
	}

	protected override bool IsTargetableEntity(Entity target, float range)
	{
		if (!base.IsTargetableEntity(target, range))
		{
			return false;
		}
		if (!HasDirectContact(target, range, range * Config.VerticalRangeFactor))
		{
			return false;
		}
		if (!CanAimAt(target))
		{
			return false;
		}
		return true;
	}

	protected virtual float GetAimYaw(Entity targetEntity)
	{
		FastVec3f targetVec = default(FastVec3f);
		targetVec.Set((float)(targetEntity.Pos.X - entity.Pos.X), (float)(targetEntity.Pos.Y - entity.Pos.Y), (float)(targetEntity.Pos.Z - entity.Pos.Z));
		return MathF.Atan2(targetVec.X, targetVec.Z);
	}

	protected virtual bool CanAimAt(Entity target)
	{
		if (!Config.Immobile)
		{
			return true;
		}
		float aimYaw = GetAimYaw(target);
		if (aimYaw > Config.SpawnAngleRad - Config.MaxTurnAngleRad - Config.MaxThrowingAngleRad)
		{
			return aimYaw < Config.SpawnAngleRad + Config.MaxTurnAngleRad + Config.MaxThrowingAngleRad;
		}
		return false;
	}

	protected virtual AssetLocation ReplaceRockType(AssetLocation code)
	{
		if (!Config.ReplaceRockVariant)
		{
			return code;
		}
		AssetLocation assetLocation = code.Clone();
		string rockType = "granite";
		IMapChunk mapChunk = entity.World.BlockAccessor.GetMapChunkAtBlockPos(entity.Pos.AsBlockPos);
		if (mapChunk != null)
		{
			int localZ = (int)entity.Pos.Z % 32;
			int localX = (int)entity.Pos.X % 32;
			rockType = entity.World.Blocks[mapChunk.TopRockIdMap[localZ * 32 + localX]].Variant["rock"] ?? "granite";
		}
		assetLocation.Path = assetLocation.Path.Replace("{rock}", rockType);
		return assetLocation;
	}

	protected virtual void SetOrAdjustDispersion()
	{
		if (targetEntity != null)
		{
			if (targetEntity.EntityId == previousTargetId)
			{
				currentYawDispersion = MathF.Max(Config.YawDispersionDeg, currentYawDispersion - Config.DispersionReductionSpeedDeg);
				currentPitchDispersion = MathF.Max(Config.PitchDispersionDeg, currentPitchDispersion - Config.DispersionReductionSpeedDeg);
			}
			else
			{
				currentYawDispersion = MathF.Max(Config.MaxYawDispersionDeg, Config.YawDispersionDeg);
				currentPitchDispersion = MathF.Max(Config.MaxPitchDispersionDeg, Config.PitchDispersionDeg);
				previousTargetId = targetEntity.EntityId;
			}
		}
	}

	protected virtual void AdjustYaw(float dt)
	{
		if (targetEntity != null)
		{
			float desiredYaw = GetAimYaw(targetEntity);
			desiredYaw = GameMath.Clamp(desiredYaw, Config.SpawnAngleRad - Config.MaxTurnAngleRad, Config.SpawnAngleRad + Config.MaxTurnAngleRad);
			float yawDistance = GameMath.AngleRadDistance(entity.Pos.Yaw, desiredYaw);
			entity.Pos.Yaw += GameMath.Clamp(yawDistance, (0f - currentTurnRadPerSec) * dt, currentTurnRadPerSec * dt);
			entity.Pos.Yaw %= (float)Math.PI * 2f;
		}
	}

	protected virtual void ShootProjectile()
	{
		if (targetEntity != null)
		{
			CreateProjectile(out Entity projectileEntity, out IProjectile projectile, out Item projectileItem);
			projectile.FiredBy = entity;
			projectile.Damage = Config.ProjectileDamage;
			projectile.DamageTier = Config.ProjectileDamageTier;
			projectile.DamageType = Config.ProjectileDamageType;
			projectile.IgnoreInvFrames = Config.IgnoreInvFrames;
			projectile.ProjectileStack = new ItemStack(projectileItem);
			projectile.Collectible = Config.NonCollectible;
			projectile.DropOnImpactChance = Config.DropOnImpactChance;
			projectile.DamageStackOnImpact = Config.DamageStackOnImpact;
			SetProjectilePositionAndVelocity(projectileEntity, projectile, Config.ProjectileGravityFactor, Config.ProjectileSpeed);
			entity.World.SpawnPriorityEntity(projectileEntity);
			if (Config.ShootSound != null)
			{
				entity.World.PlaySoundAt(Config.ShootSound, entity, null, Config.RandomizePitch, Config.SoundRange, Config.SoundVolume);
			}
		}
	}

	protected virtual void CreateProjectile(out Entity projectileEntity, out IProjectile projectile, out Item projectileItem)
	{
		AssetLocation projectileCode = ReplaceRockType(Config.ProjectileCode);
		AssetLocation projectileItemCode = ReplaceRockType(Config.ProjectileItem);
		EntityProperties type = entity.World.GetEntityType(projectileCode);
		if (type == null)
		{
			throw new ArgumentException($"Error while running '{Config.Code}' AI task for entity '{entity.Code}': projectile entity with code '{projectileCode}' does not exist");
		}
		Entity createdEntity = entity.World.ClassRegistry.CreateEntity(type);
		if (createdEntity == null)
		{
			throw new ArgumentException($"Error while running '{Config.Code}' AI task for entity '{entity.Code}': unable to create entity with code '{projectileCode}'.");
		}
		if (!(createdEntity is IProjectile createdProjectile))
		{
			throw new ArgumentException($"Error while running '{Config.Code}' AI task for entity '{entity.Code}': projectile entity '{projectileCode}' should have 'IProjectile' interface.");
		}
		projectile = createdProjectile;
		Item createdProjectileItem = entity.World.GetItem(projectileItemCode);
		if (createdProjectileItem == null)
		{
			throw new ArgumentException($"Error while running '{Config.Code}' AI task for entity '{entity.Code}': projectile item '{projectileItemCode}' does not exist.");
		}
		projectileItem = createdProjectileItem;
		projectileEntity = createdEntity;
	}

	protected virtual void SetProjectilePositionAndVelocity(Entity projectileEntity, IProjectile projectile, double gravityFactor, double speed)
	{
		if (targetEntity == null)
		{
			return;
		}
		Vec3d pos = entity.Pos.XYZ.Add(0.0, entity.LocalEyePos.Y, 0.0);
		Vec3d targetPos = targetEntity.Pos.XYZ.Add(0.0, targetEntity.LocalEyePos.Y, 0.0);
		float blocksPerSecondToBlocksPerMinute = 1f / 60f;
		speed *= (double)blocksPerSecondToBlocksPerMinute;
		double gravityAcceleration = gravityFactor * (double)GlobalConstants.GravityPerSecond * (double)blocksPerSecondToBlocksPerMinute;
		double approximateTime = (targetPos - pos).Length() / speed;
		targetPos += targetEntity.Pos.Motion * approximateTime;
		FastVec3d start = new FastVec3d(pos.X, pos.Y, pos.Z);
		FastVec3d target = new FastVec3d(targetPos.X, targetPos.Y, targetPos.Z);
		FastVec3d velocity = new FastVec3d(0.0, 0.0, 0.0);
		bool solvedBallisticArc = false;
		for (int triesCount = 0; triesCount < 30; triesCount++)
		{
			solvedBallisticArc = SolveBallisticArc(out velocity, start, target, speed, gravityAcceleration);
			if (solvedBallisticArc)
			{
				break;
			}
			speed *= 1.100000023841858;
		}
		if (!solvedBallisticArc)
		{
			FallBackVelocity(out velocity, start, target);
		}
		velocity = ApplyDispersionToVelocity(velocity, currentYawDispersion, currentPitchDispersion);
		projectileEntity.Pos.SetPosWithDimension(entity.Pos.BehindCopy(0.21).XYZ.Add(0.0, entity.LocalEyePos.Y, 0.0));
		projectileEntity.Pos.Motion.Set(velocity.X, velocity.Y, velocity.Z);
		projectileEntity.World = entity.World;
		projectile.PreInitialize();
	}

	protected virtual bool SolveBallisticArc(out FastVec3d velocity, FastVec3d start, FastVec3d target, double speed, double acceleration)
	{
		Vector3d startVec = new Vector3d(start.X, start.Y, start.Z);
		Vector3d targetVec = new Vector3d(target.X, target.Y, target.Z);
		Vector3d velocityVec;
		bool result = SolveBallisticArc(out velocityVec, startVec, targetVec, speed, acceleration);
		velocity = new FastVec3d(velocityVec.X, velocityVec.Y, velocityVec.Z);
		return result;
	}

	protected virtual bool SolveBallisticArc(out Vector3d velocity, Vector3d start, Vector3d target, double speed, double acceleration)
	{
		velocity = Vector3d.Zero;
		Vector3d delta = target - start;
		Vector2d deltaXZ = new Vector2d(delta.X, delta.Z);
		double horizontalDist = deltaXZ.Length;
		double verticalDist = delta.Y;
		double speedSq = speed * speed;
		double discriminant = speedSq * speedSq - acceleration * (acceleration * horizontalDist * horizontalDist + 2.0 * verticalDist * speedSq);
		if (discriminant < 0.0)
		{
			return false;
		}
		double sqrtDisc = Math.Sqrt(discriminant);
		double angle = Math.Atan2(speedSq - sqrtDisc, acceleration * horizontalDist);
		double vy = speed * Math.Sin(angle);
		double horizontalSpeed = speed * Math.Cos(angle);
		Vector2d vector2d = Vector2d.Normalize(deltaXZ);
		double vx = vector2d.X * horizontalSpeed;
		double vz = vector2d.Y * horizontalSpeed;
		velocity = new Vector3d(vx, vy, vz);
		return true;
	}

	protected virtual void FallBackVelocity(out FastVec3d velocity, FastVec3d start, FastVec3d target)
	{
		if (targetEntity == null)
		{
			velocity = new FastVec3d(0.0, 0.0, 0.0);
			return;
		}
		Vec3d pos = new Vec3d(start.X, start.Y, start.Z);
		Vec3d targetPos = new Vec3d(target.X, target.Y, target.Z);
		double distance = Math.Pow(pos.SquareDistanceTo(targetPos), 0.1);
		Vec3d velocityTemp = (targetPos - pos).Normalize() * GameMath.Clamp(distance - 1.0, 0.10000000149011612, 1.0);
		velocity = new FastVec3d(velocityTemp.X, velocityTemp.Y, velocityTemp.Z);
	}

	protected virtual FastVec3d ApplyDispersionToVelocity(FastVec3d velocity, float yawDispersionDeg, float pitchDispersionDeg)
	{
		Vector3d directionVector = Vector3d.Normalize(new Vector3d(velocity.X, velocity.Y, velocity.Z));
		Vector2 dispersionVector = new Vector2(yawDispersionDeg, pitchDispersionDeg);
		Vector3d direction = GetDirectionWithDispersion(directionVector, dispersionVector);
		double speed = velocity.Length();
		return new FastVec3d(direction.X * speed, direction.Y * speed, direction.Z * speed);
	}

	protected virtual Vector3d GetDirectionWithDispersion(Vector3d direction, Vector2 dispersionDeg)
	{
		float randomPitch = randomFloat.nextFloat() * dispersionDeg.Y * ((float)Math.PI / 180f);
		float randomYaw = randomFloat.nextFloat() * dispersionDeg.X * ((float)Math.PI / 180f);
		Vector3 verticalAxis = new Vector3(0f, 0f, 1f);
		if ((verticalAxis - direction).Length < 1000000000.0 || (verticalAxis + direction).Length < 1000000000.0)
		{
			verticalAxis = new Vector3(0f, 1f, 0f);
		}
		Vector3d forwardAxis = Vector3d.Normalize(direction);
		Vector3d vector3d = Vector3d.Normalize(Vector3d.Cross(forwardAxis, verticalAxis));
		Vector3d pitchAxis = Vector3d.Normalize(Vector3d.Cross(vector3d, forwardAxis));
		Vector3d yawComponent = vector3d * Math.Tan(randomYaw);
		Vector3d pitchComponent = pitchAxis * Math.Tan(randomPitch);
		return Vector3d.Normalize(forwardAxis + yawComponent + pitchComponent);
	}
}
