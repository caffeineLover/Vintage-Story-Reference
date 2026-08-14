using System;
using System.Collections.Generic;
using System.IO;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public abstract class EntityProjectileBase : Entity, IProjectile
{
	protected bool beforeCollided;

	protected long msLaunch;

	protected long msCollide;

	protected Vec3d motionBeforeCollide = new Vec3d();

	protected CollisionTester collTester = new CollisionTester();

	protected Cuboidf? collisionTestBox;

	protected EntityPartitioning? entityPartitioning;

	protected List<long> entitiesHit = new List<long>();

	protected long firedByMountEntityId;

	protected AssetLocation impactSound = new AssetLocation("sounds/arrow-impact");

	protected AssetLocation hitNotifySound = new AssetLocation("sounds/player/projectilehit");

	protected long collectDelayMs = 1000L;

	protected double maxStuckSpeed = 0.2;

	protected double minAttackSpeed = 0.01;

	protected long selfHitDelay = 500L;

	protected float collisionCheckRadius = 5f;

	protected float knockbackFactor = 10f;

	public Entity? FiredBy { get; set; }

	public float Damage { get; set; }

	public int DamageTier { get; set; }

	public EnumDamageType DamageType { get; set; } = EnumDamageType.PiercingAttack;

	public bool IgnoreInvFrames { get; set; }

	public ItemStack? ProjectileStack { get; set; }

	public ItemStack? WeaponStack { get; set; }

	public float DropOnImpactChance { get; set; }

	public bool DamageStackOnImpact { get; set; }

	public bool Collectible
	{
		get
		{
			return Attributes.GetBool("collectible");
		}
		set
		{
			Attributes.SetBool("collectible", value);
		}
	}

	public bool EntityHit { get; protected set; }

	public float Weight { get; set; } = 0.1f;

	public bool Stuck { get; set; }

	public override bool ApplyGravity => !Stuck;

	public override bool IsInteractable => false;

	public virtual void SetFromConfig(IProjectileJsonConfig config)
	{
		Damage = config.Damage;
		DamageTier = config.DamageTier;
		DamageType = config.DamageType;
		IgnoreInvFrames = config.IgnoreInvFrames;
		DropOnImpactChance = config.DropOnImpactChance;
		DamageStackOnImpact = config.DamageStackOnImpact;
		Collectible = config.Collectible;
		Weight = config.Weight;
	}

	public virtual void PreInitialize()
	{
	}

	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		base.Initialize(properties, api, InChunkIndex3d);
		if (Api.Side == EnumAppSide.Server && FiredBy != null)
		{
			WatchedAttributes.SetLong("firedBy", FiredBy.EntityId);
		}
		if (Api.Side == EnumAppSide.Client)
		{
			FiredBy = Api.World.GetEntityById(WatchedAttributes.GetLong("firedBy", 0L));
		}
		msLaunch = World.ElapsedMilliseconds;
		if (FiredBy is EntityAgent firedByAgent && firedByAgent.MountedOn?.Entity != null)
		{
			firedByMountEntityId = firedByAgent.MountedOn.Entity.EntityId;
		}
		EntityBehaviorPassivePhysics physicsBehavior = GetBehavior<EntityBehaviorPassivePhysics>();
		if (physicsBehavior != null)
		{
			physicsBehavior.OnPhysicsTickCallback = OnPhysicsTickCallback;
			physicsBehavior.CollisionYExtra = 0f;
		}
		entityPartitioning = api.ModLoader.GetModSystem<EntityPartitioning>();
	}

	public override bool CanCollect(Entity byEntity)
	{
		if (Collectible && Alive && World.ElapsedMilliseconds - msLaunch > collectDelayMs)
		{
			return base.Pos.Motion.Length() < maxStuckSpeed;
		}
		return false;
	}

	public override void ToBytes(BinaryWriter writer, bool forClient)
	{
		base.ToBytes(writer, forClient);
		writer.Write(beforeCollided);
		ProjectileStack?.ToBytes(writer);
	}

	public override void FromBytes(BinaryReader reader, bool isSync)
	{
		base.FromBytes(reader, isSync);
		beforeCollided = reader.ReadBoolean();
		try
		{
			if (reader.BaseStream.Position < reader.BaseStream.Length)
			{
				ProjectileStack = new ItemStack(reader);
			}
		}
		catch (Exception)
		{
		}
	}

	public override ItemStack? OnCollected(Entity byEntity)
	{
		ProjectileStack?.ResolveBlockOrItem(World);
		return ProjectileStack;
	}

	protected virtual void OnPhysicsTickCallback(float dtFac)
	{
		if (ShouldDespawn || !Alive || World.ElapsedMilliseconds <= msCollide + 500)
		{
			return;
		}
		EntityPos pos = base.Pos;
		if (!(pos.Motion.LengthSq() < maxStuckSpeed * maxStuckSpeed))
		{
			Cuboidd collisionBox = CollisionBox.ToDouble().Translate(pos.X, pos.Y, pos.Z);
			AdjustCollisionBox(collisionBox, pos.Motion, dtFac);
			Cuboidd targetCollisionBox = new Cuboidd();
			entityPartitioning?.WalkEntities(pos.XYZ, collisionCheckRadius, (Entity target) => TryHitTarget(target, collisionBox, targetCollisionBox), EnumEntitySearchType.Creatures);
		}
	}

	protected virtual bool TryAttackEntity(double impactSpeed)
	{
		if (World is IClientWorldAccessor || World.ElapsedMilliseconds <= msCollide + 250)
		{
			return false;
		}
		if (impactSpeed <= minAttackSpeed)
		{
			return false;
		}
		EntityPos pos = base.Pos;
		Cuboidd collisionBox = CollisionBox.ToDouble().Translate(pos.X, pos.Y, pos.Z);
		AdjustCollisionBox(collisionBox, pos.Motion, 1.5f);
		Cuboidd targetCollisionBox = new Cuboidd();
		Entity target = World.GetNearestEntity(base.Pos.XYZ, collisionCheckRadius, collisionCheckRadius, delegate(Entity entity)
		{
			if (!CanHitTarget(entity))
			{
				return false;
			}
			if (entitiesHit.Contains(entity.EntityId))
			{
				return false;
			}
			targetCollisionBox.SetAndTranslate(entity.CollisionBox, entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
			return targetCollisionBox.IntersectsOrTouches(collisionBox);
		});
		if (target != null)
		{
			entitiesHit.Add(target.EntityId);
			ImpactOnEntity(target);
			return true;
		}
		return false;
	}

	protected virtual void ImpactOnEntity(Entity target)
	{
		if (!Alive)
		{
			return;
		}
		if (CanDealDamage(target) && World.Side == EnumAppSide.Server)
		{
			World.PlaySoundAt(impactSound, this, null, randomizePitch: false, 24f);
			bool dealtDamage = DealDamage(target);
			DamageProjectile(target);
			EntityPlayer player = FiredBy as EntityPlayer;
			if (player != null && dealtDamage)
			{
				World.PlaySoundFor(hitNotifySound, player.Player, randomizePitch: false, 24f);
			}
		}
		EntityHit = true;
		msCollide = World.ElapsedMilliseconds;
		base.Pos.Motion.Set(0.0, 0.0, 0.0);
	}

	protected virtual bool CanDealDamage(Entity target)
	{
		IServerPlayer fromPlayer = (FiredBy as EntityPlayer)?.Player as IServerPlayer;
		bool targetIsPlayer = target is EntityPlayer;
		bool targetIsCreature = target is EntityAgent;
		bool canDamage = true;
		ICoreServerAPI serverApi = World.Api as ICoreServerAPI;
		if (fromPlayer != null && serverApi != null)
		{
			if (targetIsPlayer && (!serverApi.Server.Config.AllowPvP || !fromPlayer.HasPrivilege("attackplayers")))
			{
				canDamage = false;
			}
			if (targetIsCreature && !fromPlayer.HasPrivilege("attackcreatures"))
			{
				canDamage = false;
			}
		}
		return canDamage;
	}

	protected virtual void DamageProjectile(Entity target)
	{
		int leftDurability = 1;
		if (DamageStackOnImpact && ProjectileStack != null)
		{
			ProjectileStack.Collectible.DamageItem(target.World, target, new DummySlot(ProjectileStack));
			leftDurability = ((ProjectileStack == null) ? 1 : ProjectileStack.Collectible.GetRemainingDurability(ProjectileStack));
		}
		if (leftDurability <= 0 || World.Rand.NextDouble() >= (double)DropOnImpactChance)
		{
			Die();
		}
	}

	protected virtual bool DealDamage(Entity target)
	{
		bool fromPlayer = FiredBy is EntityPlayer;
		float damage = Damage;
		ApplyModifiers(ref damage, target);
		return target.ReceiveDamage(new DamageSource
		{
			Source = (fromPlayer ? EnumDamageSource.Player : EnumDamageSource.Entity),
			SourceEntity = this,
			CauseEntity = FiredBy,
			Type = DamageType,
			DamageTier = DamageTier,
			IgnoreInvFrames = IgnoreInvFrames,
			KnockbackStrength = Weight * (float)base.Pos.Motion.Length() * knockbackFactor
		}, damage);
	}

	protected virtual void ApplyModifiers(ref float damage, Entity target)
	{
		if (FiredBy != null)
		{
			damage *= FiredBy.Stats.GetBlended("rangedWeaponsDamage");
			JsonObject attributes = target.Properties.Attributes;
			if (attributes != null && attributes["isMechanical"].AsBool())
			{
				damage *= FiredBy.Stats.GetBlended("mechanicalsDamage");
			}
		}
	}

	protected virtual void AdjustCollisionBox(Cuboidd collisionBox, Vec3d velocity, float dt)
	{
		if (velocity.X < 0.0)
		{
			collisionBox.X1 += velocity.X * (double)dt;
		}
		else
		{
			collisionBox.X2 += velocity.X * (double)dt;
		}
		if (velocity.Y < 0.0)
		{
			collisionBox.Y1 += velocity.Y * (double)dt;
		}
		else
		{
			collisionBox.Y2 += velocity.Y * (double)dt;
		}
		if (velocity.Z < 0.0)
		{
			collisionBox.Z1 += velocity.Z * (double)dt;
		}
		else
		{
			collisionBox.Z2 += velocity.Z * (double)dt;
		}
	}

	protected virtual bool TryHitTarget(Entity target, Cuboidd projectileCollisionBox, Cuboidd targetCollisionBoxBuffer)
	{
		if (!CanHitTarget(target))
		{
			return true;
		}
		if (entitiesHit.Contains(target.EntityId))
		{
			return true;
		}
		targetCollisionBoxBuffer.SetAndTranslate(target.CollisionBox, target.Pos.X, target.Pos.Y, target.Pos.Z);
		if (targetCollisionBoxBuffer.IntersectsOrTouches(projectileCollisionBox))
		{
			ImpactOnEntity(target);
			return false;
		}
		return true;
	}

	protected virtual bool CanHitTarget(Entity target)
	{
		if (target.EntityId != EntityId && target.IsInteractable && (FiredBy == null || target.EntityId != FiredBy.EntityId || World.ElapsedMilliseconds - msLaunch >= selfHitDelay))
		{
			if (target.EntityId == firedByMountEntityId)
			{
				return World.ElapsedMilliseconds - msLaunch >= selfHitDelay;
			}
			return true;
		}
		return false;
	}

	public static void SpawnProjectile(Entity entity, EntityAgent byEntity, double speed = 1.0, double dispersionFactor = 1.0, double verticalOffset = 0.0, double horizontalOffset = 0.0, double forwardOffset = 0.0, double parallaxDistance = 0.0)
	{
		var (position, direction) = GetProjectileDirection(byEntity, dispersionFactor, verticalOffset, horizontalOffset, forwardOffset, parallaxDistance);
		entity.Pos.SetPosWithDimension(position);
		entity.Pos.Motion.Set(direction * speed);
		entity.World = byEntity.World;
		(entity as IProjectile)?.PreInitialize();
		byEntity.World?.SpawnPriorityEntity(entity);
	}

	public static (FastVec3d position, FastVec3d direction) GetProjectileDirection(EntityAgent byEntity, double dispersionFactor, double verticalOffset = 0.0, double horizontalOffset = 0.0, double forwardOffset = 0.0, double parallaxDistance = 0.0)
	{
		float dispersion = Math.Max(0.001f, 1f - byEntity.Attributes.GetFloat("aimingAccuracy"));
		double pitch = byEntity.WatchedAttributes.GetDouble("aimingRandPitch", 1.0) * (double)dispersion * dispersionFactor;
		double yaw = byEntity.WatchedAttributes.GetDouble("aimingRandYaw", 1.0) * (double)dispersion * dispersionFactor;
		FastVec3d offset = new FastVec3d(0.0 - (double)GameMath.Cos(byEntity.Pos.Yaw) * horizontalOffset, verticalOffset, (double)GameMath.Sin(byEntity.Pos.Yaw) * horizontalOffset);
		FastVec3d position = byEntity.Pos.BehindCopy(forwardOffset).XYZFast.Add(byEntity.LocalEyePos) + offset;
		if (horizontalOffset != 0.0 && parallaxDistance != 0.0)
		{
			yaw = ParallaxCorrection(yaw, parallaxDistance, horizontalOffset);
		}
		FastVec3d direction = (position.AheadCopy(1.0, (double)byEntity.Pos.Pitch + pitch, (double)byEntity.Pos.Yaw + yaw) - position).Normalize();
		return (position: position, direction: direction);
	}

	public static double ParallaxCorrection(double yaw, double distance, double offset)
	{
		FastVec3d fastVec3d = new FastVec3d(Math.Cos(yaw), 0.0, Math.Sin(yaw)) * distance;
		FastVec3d start = new FastVec3d(Math.Sin(yaw), 0.0, 0.0 - Math.Cos(yaw)) * offset;
		FastVec3d correctedDirection = (fastVec3d - start).Normalize();
		return Math.Atan2(correctedDirection.Z, correctedDirection.X);
	}
}
