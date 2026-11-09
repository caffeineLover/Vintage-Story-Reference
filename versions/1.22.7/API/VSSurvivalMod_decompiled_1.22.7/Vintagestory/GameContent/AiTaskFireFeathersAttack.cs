using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class AiTaskFireFeathersAttack : AiTaskFlyCircle
{
	public bool Enabled = true;

	protected float seekingRangeVer = 25f;

	protected float seekingRangeHor = 25f;

	protected int fireAfterMs;

	protected int durationMs;

	protected ProjectileConfig[] projectileConfigs;

	protected float accum;

	protected bool projectilesFired;

	protected float minVerticalDistance = 5f;

	protected float minHorizontalDistance = 10f;

	protected long globalAttackCooldownMs = 3000L;

	public AiTaskFireFeathersAttack(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		fireAfterMs = taskConfig["fireAfterMs"].AsInt(1000);
		durationMs = taskConfig["durationMs"].AsInt(1000);
		seekingRangeHor = taskConfig["seekingRangeHor"].AsFloat(25f);
		seekingRangeVer = taskConfig["seekingRangeVer"].AsFloat(25f);
		projectileConfigs = taskConfig["projectileConfigs"].AsObject<ProjectileConfig[]>(null, entity.Code.Domain);
		minVerticalDistance = taskConfig["minVerticalDistance"].AsFloat(5f);
		minHorizontalDistance = taskConfig["minHorizontalDistance"].AsFloat(10f);
		globalAttackCooldownMs = taskConfig["globalAttackCooldownMs"].AsInt(3000);
		ProjectileConfig[] array = projectileConfigs;
		foreach (ProjectileConfig projectileConfig in array)
		{
			projectileConfig.EntityType = entity.World.GetEntityType(projectileConfig.Code);
			if (projectileConfig.EntityType == null)
			{
				throw new Exception("No such projectile exists - " + projectileConfig.Code);
			}
			projectileConfig.CollectibleStack?.Resolve(entity.World, $"Projectile stack of {entity.Code}", printWarningOnError: true);
		}
	}

	public override bool ShouldExecute()
	{
		if (!Enabled)
		{
			return false;
		}
		CenterPos = SpawnPos;
		long ellapsedMs = entity.World.ElapsedMilliseconds;
		if (cooldownUntilMs > ellapsedMs)
		{
			return false;
		}
		cooldownUntilMs = entity.World.ElapsedMilliseconds + 1500;
		if (!PreconditionsSatisfied())
		{
			return false;
		}
		if (!checkGlobalAttackCooldown())
		{
			return false;
		}
		if (entity.World.ElapsedMilliseconds - attackedByEntityMs > 30000)
		{
			attackedByEntity = null;
		}
		if (ShouldRetaliateForRange(15f))
		{
			targetEntity = attackedByEntity;
		}
		else
		{
			targetEntity = entity.World.GetNearestEntity(CenterPos, seekingRangeHor, seekingRangeVer, (Entity e) => IsTargetableEntity(e, seekingRangeHor) && hasDirectContact(e, seekingRangeHor, seekingRangeVer));
		}
		if (targetEntity != null && entity.Pos.Y - targetEntity.Pos.Y > (double)minVerticalDistance)
		{
			return entity.Pos.HorDistanceTo(targetEntity.Pos) > (double)minHorizontalDistance;
		}
		return false;
	}

	public override void StartExecute()
	{
		base.StartExecute();
		accum = 0f;
		projectilesFired = false;
	}

	public override bool ContinueExecute(float dt)
	{
		if (!IsInValidDayTimeHours(initialRandomness: false))
		{
			return false;
		}
		accum += dt;
		if (accum * 1000f > (float)fireAfterMs)
		{
			if (!projectilesFired)
			{
				ProjectileConfig[] array = projectileConfigs;
				foreach (ProjectileConfig cfg in array)
				{
					cfg.LeftToFire = GameMath.RoundRandom(entity.World.Rand, cfg.Quantity.nextFloat());
				}
				world.PlaySoundAt("sounds/creature/erel/fire", entity, null, randomizePitch: false, 100f);
			}
			fireProjectiles();
			projectilesFired = true;
		}
		bool result = base.ContinueExecute(dt) && accum * 1000f < (float)durationMs;
		lookatTarget();
		return result;
	}

	public override void FinishExecute(bool cancelled)
	{
		(entity as EntityErel).LastAttackTime = entity.World.ElapsedMilliseconds;
		base.FinishExecute(cancelled);
	}

	protected void lookatTarget()
	{
		Vec3d targetVector = targetEntity.Pos.XYZ - entity.Pos.XYZ;
		entity.Pos.Yaw = (float)Math.Atan2(targetVector.X, targetVector.Z);
	}

	protected bool checkGlobalAttackCooldown()
	{
		long lastAttack = (entity as EntityErel).LastAttackTime;
		return entity.World.ElapsedMilliseconds - lastAttack > globalAttackCooldownMs;
	}

	protected void fireProjectiles()
	{
		IWorldAccessor world = entity.World;
		Random rnd = world.Rand;
		projectileConfigs = projectileConfigs.Shuffle(rnd);
		ProjectileConfig[] array = projectileConfigs;
		foreach (ProjectileConfig cfg in array)
		{
			if (cfg.LeftToFire > 0)
			{
				cfg.LeftToFire--;
				EntityProjectile entitypr = world.ClassRegistry.CreateEntity(cfg.EntityType) as EntityProjectile;
				entitypr.FiredBy = entity;
				entitypr.DamageType = cfg.DamageType;
				entitypr.Damage = cfg.Damage;
				entitypr.DamageTier = cfg.DamageTier;
				entitypr.ProjectileStack = cfg.CollectibleStack?.ResolvedItemstack?.Clone() ?? new ItemStack(world.GetItem(new AssetLocation("stone-granite")));
				entitypr.Collectible = cfg.CollectibleStack?.ResolvedItemstack != null;
				entitypr.World = world;
				Vec3d spawnpos = entity.Pos.XYZ.Add(rnd.NextDouble() * 6.0 - 3.0, rnd.NextDouble() * 5.0, rnd.NextDouble() * 6.0 - 3.0);
				Vec3d targetPos = targetEntity.Pos.XYZ.Add(0.0, targetEntity.LocalEyePos.Y, 0.0) + targetEntity.Pos.Motion * 8f;
				double dist = spawnpos.DistanceTo(targetPos);
				double distf = Math.Pow(dist, 0.2);
				Vec3d velocity = (targetPos - spawnpos).Normalize() * GameMath.Clamp(distf - 1.0, 0.10000000149011612, 1.0);
				velocity.Y += (dist - 10.0) / 150.0;
				velocity.X *= 1.0 + (rnd.NextDouble() - 0.5) / 3.0;
				velocity.Y *= 1.0 + (rnd.NextDouble() - 0.5) / 5.0;
				velocity.Z *= 1.0 + (rnd.NextDouble() - 0.5) / 3.0;
				entitypr.Pos.SetPosWithDimension(spawnpos);
				entitypr.Pos.Motion.Set(velocity);
				entitypr.SetInitialRotation();
				world.SpawnEntity(entitypr);
				break;
			}
		}
	}
}
