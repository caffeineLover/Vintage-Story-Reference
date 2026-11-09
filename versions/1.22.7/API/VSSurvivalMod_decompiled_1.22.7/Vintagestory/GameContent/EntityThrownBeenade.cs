using System;
using System.IO;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class EntityThrownBeenade : Entity, IProjectile
{
	private bool beforeCollided;

	private bool stuck;

	private long msLaunch;

	private Vec3d motionBeforeCollide = new Vec3d();

	public Entity FiredBy;

	public float Damage;

	public ItemStack ProjectileStack;

	public override bool IsInteractable => false;

	Entity IProjectile.FiredBy
	{
		get
		{
			return FiredBy;
		}
		set
		{
			FiredBy = value;
		}
	}

	float IProjectile.Damage
	{
		get
		{
			return Damage;
		}
		set
		{
			Damage = value;
		}
	}

	int IProjectile.DamageTier { get; set; }

	EnumDamageType IProjectile.DamageType { get; set; }

	bool IProjectile.IgnoreInvFrames { get; set; }

	ItemStack IProjectile.ProjectileStack
	{
		get
		{
			return ProjectileStack;
		}
		set
		{
			ProjectileStack = value;
		}
	}

	ItemStack IProjectile.WeaponStack { get; set; }

	float IProjectile.DropOnImpactChance { get; set; }

	bool IProjectile.DamageStackOnImpact { get; set; }

	bool IProjectile.Collectible { get; set; }

	bool IProjectile.EntityHit { get; }

	float IProjectile.Weight { get; set; }

	bool IProjectile.Stuck
	{
		get
		{
			return stuck;
		}
		set
		{
			stuck = value;
		}
	}

	void IProjectile.PreInitialize()
	{
	}

	void IProjectile.SetFromConfig(IProjectileJsonConfig config)
	{
		Damage = config.Damage;
	}

	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		base.Initialize(properties, api, InChunkIndex3d);
		msLaunch = World.ElapsedMilliseconds;
		if (ProjectileStack?.Collectible != null)
		{
			ProjectileStack.ResolveBlockOrItem(World);
		}
	}

	public override void OnGameTick(float dt)
	{
		base.OnGameTick(dt);
		if (ShouldDespawn)
		{
			return;
		}
		EntityPos pos = base.Pos;
		stuck = base.Collided;
		pos.Pitch = (float)Math.PI / 2f;
		pos.Roll = 0f;
		pos.Yaw = (float)Math.PI / 2f;
		if (stuck)
		{
			if (!beforeCollided && World.Side == EnumAppSide.Server)
			{
				GameMath.Clamp((float)motionBeforeCollide.Length() * 4f, 0f, 1f);
				if (CollidedHorizontally || CollidedVertically)
				{
					OnImpact();
					return;
				}
				WatchedAttributes.MarkAllDirty();
			}
			beforeCollided = true;
			return;
		}
		if (Damage > 0f && World.Side == EnumAppSide.Server)
		{
			Entity entity = World.GetNearestEntity(base.Pos.XYZ, 5f, 5f, (Entity e) => e.EntityId != EntityId && (FiredBy == null || e.EntityId != FiredBy.EntityId || World.ElapsedMilliseconds - msLaunch >= 500) && e.IsInteractable && e.SelectionBox.ToDouble().Translate(e.Pos.X, e.Pos.Y, e.Pos.Z).ShortestDistanceFrom(base.Pos.X, base.Pos.Y, base.Pos.Z) < 0.5);
			if (entity != null)
			{
				DamageSource damageSource = new DamageSource
				{
					Source = EnumDamageSource.Entity,
					SourceEntity = this,
					CauseEntity = FiredBy,
					Type = EnumDamageType.BluntAttack
				};
				if (entity.ShouldReceiveDamage(damageSource, Damage))
				{
					entity.ReceiveDamage(damageSource, Damage);
				}
				OnImpact();
				return;
			}
		}
		beforeCollided = false;
		motionBeforeCollide.Set(pos.Motion.X, pos.Motion.Y, pos.Motion.Z);
	}

	public void OnImpact()
	{
		World.PlaySoundAt(new AssetLocation("sounds/effect/toolbreak"), this, null, randomizePitch: false);
		World.SpawnCubeParticles(base.Pos.XYZ.OffsetCopy(0.0, 0.2, 0.0), ProjectileStack, 0.8f, 20);
		Die();
		EntityProperties type = World.GetEntityType(new AssetLocation("beemob"));
		Entity entity = World.ClassRegistry.CreateEntity(type);
		if (entity != null)
		{
			entity.Pos.X = base.Pos.X + 0.5;
			entity.Pos.Y = base.Pos.Y + 0.5;
			entity.Pos.Z = base.Pos.Z + 0.5;
			entity.Pos.Yaw = (float)World.Rand.NextDouble() * 2f * (float)Math.PI;
			entity.Attributes.SetString("origin", "brokenbeenade");
			World.SpawnEntity(entity);
		}
	}

	public override bool CanCollect(Entity byEntity)
	{
		return false;
	}

	public override void OnCollideWithLiquid()
	{
		base.OnCollideWithLiquid();
	}

	public override void ToBytes(BinaryWriter writer, bool forClient)
	{
		base.ToBytes(writer, forClient);
		writer.Write(beforeCollided);
		ProjectileStack.ToBytes(writer);
	}

	public override void FromBytes(BinaryReader reader, bool fromServer)
	{
		base.FromBytes(reader, fromServer);
		beforeCollided = reader.ReadBoolean();
		ProjectileStack = ((World == null) ? new ItemStack(reader) : new ItemStack(reader, World));
	}
}
