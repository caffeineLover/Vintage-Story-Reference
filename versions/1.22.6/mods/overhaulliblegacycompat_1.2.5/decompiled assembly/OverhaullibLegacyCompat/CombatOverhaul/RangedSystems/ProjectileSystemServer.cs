using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Colliders;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Integration;
using CombatOverhaul.Utils;
using CombatOverhaul.WeaponBuffs;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C8 RID: 200
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ProjectileSystemServer
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000A84 RID: 2692 RVA: 0x000373FC File Offset: 0x000355FC
		// (remove) Token: 0x06000A85 RID: 2693 RVA: 0x00037434 File Offset: 0x00035634
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event ProjectileSystemServer.RangedDamageDelegate OnDealRangedDamage;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000A86 RID: 2694 RVA: 0x0003746C File Offset: 0x0003566C
		// (remove) Token: 0x06000A87 RID: 2695 RVA: 0x000374A4 File Offset: 0x000356A4
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event ProjectileSystemServer.RangedDamageResolvedDelegate OnRangedDamageResolved;

		// Token: 0x06000A88 RID: 2696 RVA: 0x000374DC File Offset: 0x000356DC
		public ProjectileSystemServer(ICoreServerAPI api)
		{
			this._api = api;
			this._serverChannel = api.Network.RegisterChannel("CombatOverhaul:projectiles").RegisterMessageType<ProjectileCollisionPacket>().SetMessageHandler<ProjectileCollisionPacket>(new NetworkClientMessageHandler<ProjectileCollisionPacket>(this.HandleCollision)).RegisterMessageType<ProjectileCollisionCheckRequest>();
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00037560 File Offset: 0x00035760
		public void Spawn(Guid id, ProjectileStats projectileStats, ProjectileSpawnStats spawnStats, ItemStack projectileStack, [Nullable(2)] ItemStack weaponStack, Entity shooter)
		{
			this.SpawnInternal(id, projectileStats, spawnStats, projectileStack, weaponStack, null, shooter, shooter, false);
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00037580 File Offset: 0x00035780
		public void Spawn(Guid id, ProjectileStats projectileStats, ProjectileSpawnStats spawnStats, ItemStack projectileStack, [Nullable(2)] ItemStack weaponStack, Entity shooter, Entity target)
		{
			this.SpawnInternal(id, projectileStats, spawnStats, projectileStack, weaponStack, null, shooter, target, false);
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x000375A0 File Offset: 0x000357A0
		public void SpawnFromWeaponSlot(Guid id, ProjectileStats projectileStats, ProjectileSpawnStats spawnStats, ItemStack projectileStack, ItemSlot weaponSlot, Entity shooter)
		{
			this.SpawnInternal(id, projectileStats, spawnStats, projectileStack, weaponSlot.Itemstack, weaponSlot, shooter, shooter, true);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x000375C8 File Offset: 0x000357C8
		public void SpawnFromWeaponSlot(Guid id, ProjectileStats projectileStats, ProjectileSpawnStats spawnStats, ItemStack projectileStack, ItemSlot weaponSlot, Entity shooter, Entity target)
		{
			this.SpawnInternal(id, projectileStats, spawnStats, projectileStack, weaponSlot.Itemstack, weaponSlot, shooter, target, true);
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x000375F0 File Offset: 0x000357F0
		private void SpawnInternal(Guid id, ProjectileStats projectileStats, ProjectileSpawnStats spawnStats, ItemStack projectileStack, [Nullable(2)] ItemStack weaponStack, [Nullable(2)] ItemSlot weaponSlot, Entity shooter, Entity target, bool applyBuffs)
		{
			EntityPlayer entityPlayer;
			if ((entityPlayer = (shooter as EntityPlayer)) == null && (entityPlayer = (target as EntityPlayer)) == null)
			{
				entityPlayer = (this._api.World.GetNearestEntity(target.Pos.XYZ, 300f, 300f, (Entity entity) => entity is EntityPlayer) as EntityPlayer);
			}
			EntityPlayer owner = entityPlayer;
			if (owner == null)
			{
				return;
			}
			if (applyBuffs)
			{
				WeaponBuffSystem.ModifyProjectileSpawn(projectileStats, ref spawnStats, projectileStack, weaponStack, shooter, target);
			}
			ProjectileEntity projectile;
			ProjectileSystemServer.SpawnProjectile(id, projectileStack, weaponStack, projectileStats, spawnStats, this._api, shooter, owner, out projectile);
			if (projectile != null)
			{
				this._projectiles.Add(id, new ProjectileServer(projectile, projectileStats, spawnStats, this._api, new Action<Guid>(this.ClearId), projectileStack, weaponSlot, applyBuffs));
				projectile.ServerProjectile = this._projectiles[id];
				if (applyBuffs && WeaponBuffSystem.ConsumeInternal(weaponStack, WeaponBuffConsumptionTrigger.ProjectileSpawn) > 0 && weaponSlot != null)
				{
					weaponSlot.MarkDirty();
				}
				if (applyBuffs)
				{
					WeaponBuffSystem.ConsumeInternal(projectileStack, WeaponBuffConsumptionTrigger.ProjectileSpawn);
				}
			}
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x000376F8 File Offset: 0x000358F8
		public void TryCollide(ProjectileEntity projectile)
		{
			double num = projectile.ServerPos.X - projectile.PreviousPosition.X;
			double deltaY = projectile.ServerPos.Y - projectile.PreviousPosition.Y;
			double deltaZ = projectile.ServerPos.Z - projectile.PreviousPosition.Z;
			if (num * num + deltaY * deltaY + deltaZ * deltaZ <= 1E-12)
			{
				return;
			}
			bool isFirearmsProjectile = ProjectileSystemServer.IsFirearmsProjectile(projectile);
			long[] candidateEntityIds = isFirearmsProjectile ? this.GetProjectileCollisionCandidateEntityIds(projectile) : Array.Empty<long>();
			if (isFirearmsProjectile && candidateEntityIds.Length == 0)
			{
				return;
			}
			bool checkAABBOnly = ProjectileSystemServer.CheckAABBOnly(this._api, projectile);
			long[] ignoredEntities = (projectile.CollidedWith.Count == 0) ? Array.Empty<long>() : projectile.CollidedWith.ToArray();
			Entity shooter = this._api.World.GetEntityById(projectile.ShooterId);
			bool gateAgainstShooterBackHits = shooter is EntityPlayer;
			Vector3d shotDirection = gateAgainstShooterBackHits ? ProjectileSystemServer.GetProjectileShotDirection(projectile) : Vector3d.Zero;
			Vec3d shooterPosition = gateAgainstShooterBackHits ? ProjectileSystemServer.GetProjectileShooterPosition(projectile, shooter) : new Vec3d();
			ProjectileCollisionCheckRequest projectileCollisionCheckRequest = new ProjectileCollisionCheckRequest();
			projectileCollisionCheckRequest.ProjectileId = projectile.ProjectileId;
			projectileCollisionCheckRequest.ProjectileEntityId = projectile.EntityId;
			projectileCollisionCheckRequest.CurrentPosition = new double[]
			{
				projectile.ServerPos.X,
				projectile.ServerPos.Y,
				projectile.ServerPos.Z
			};
			projectileCollisionCheckRequest.PreviousPosition = new double[]
			{
				projectile.PreviousPosition.X,
				projectile.PreviousPosition.Y,
				projectile.PreviousPosition.Z
			};
			projectileCollisionCheckRequest.Velocity = new double[]
			{
				projectile.PreviousVelocity.X,
				projectile.PreviousVelocity.Y,
				projectile.PreviousVelocity.Z
			};
			ProjectileCollisionCheckRequest projectileCollisionCheckRequest2 = projectileCollisionCheckRequest;
			double[] shooterPosition2;
			if (!gateAgainstShooterBackHits)
			{
				shooterPosition2 = Array.Empty<double>();
			}
			else
			{
				double[] array = new double[3];
				array[0] = shooterPosition.X;
				array[1] = shooterPosition.Y;
				shooterPosition2 = array;
				array[2] = shooterPosition.Z;
			}
			projectileCollisionCheckRequest2.ShooterPosition = shooterPosition2;
			ProjectileCollisionCheckRequest projectileCollisionCheckRequest3 = projectileCollisionCheckRequest;
			double[] shotDirection2;
			if (!gateAgainstShooterBackHits || shotDirection.LengthSquared <= 1E-08)
			{
				shotDirection2 = Array.Empty<double>();
			}
			else
			{
				double[] array2 = new double[3];
				array2[0] = shotDirection.X;
				array2[1] = shotDirection.Y;
				shotDirection2 = array2;
				array2[2] = shotDirection.Z;
			}
			projectileCollisionCheckRequest3.ShotDirection = shotDirection2;
			projectileCollisionCheckRequest.Radius = projectile.ColliderRadius;
			projectileCollisionCheckRequest.PenetrationDistance = projectile.PenetrationDistance + 5f;
			projectileCollisionCheckRequest.PenetrationStrength = projectile.PenetrationStrength;
			projectileCollisionCheckRequest.CollideWithShooter = false;
			projectileCollisionCheckRequest.IgnoreEntities = ignoredEntities;
			projectileCollisionCheckRequest.CandidateEntityIds = candidateEntityIds;
			ProjectileCollisionCheckRequest projectileCollisionCheckRequest4 = projectileCollisionCheckRequest;
			ProjectileServer serverProjectile = projectile.ServerProjectile;
			projectileCollisionCheckRequest4.PacketVersion = ((serverProjectile != null) ? serverProjectile.PacketVersion : 0);
			projectileCollisionCheckRequest.ShooterId = projectile.ShooterId;
			projectileCollisionCheckRequest.CheckAABBOnly = checkAABBOnly;
			projectileCollisionCheckRequest.RequireColliderWhenAvailable = (!checkAABBOnly && ProjectileSystemServer.IsBodyPartRangedProjectile(projectile, isFirearmsProjectile));
			ProjectileCollisionCheckRequest packet = projectileCollisionCheckRequest;
			EntityPlayer entityPlayer = this._api.World.GetEntityById(projectile.OwnerId) as EntityPlayer;
			IServerPlayer player = ((entityPlayer != null) ? entityPlayer.Player : null) as IServerPlayer;
			if (player != null)
			{
				this._serverChannel.SendPacket<ProjectileCollisionCheckRequest>(packet, new IServerPlayer[]
				{
					player
				});
			}
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00037A1A File Offset: 0x00035C1A
		public void OnDealDamage(Entity target, DamageSource damageSource, [Nullable(2)] ItemStack weaponStack, ref float damage)
		{
			this.OnDealDamage(target, damageSource, weaponStack, null, ref damage);
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00037A28 File Offset: 0x00035C28
		public void OnDealDamage(Entity target, DamageSource damageSource, [Nullable(2)] ItemStack weaponStack, [Nullable(2)] ItemStack projectileStack, ref float damage)
		{
			this.OnDealDamage(target, damageSource, weaponStack, projectileStack, ref damage, true);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00037A38 File Offset: 0x00035C38
		internal void OnDealDamage(Entity target, DamageSource damageSource, [Nullable(2)] ItemStack weaponStack, [Nullable(2)] ItemStack projectileStack, ref float damage, bool applyBuffs)
		{
			ProjectileSystemServer.RangedDamageDelegate onDealRangedDamage = this.OnDealRangedDamage;
			if (onDealRangedDamage != null)
			{
				onDealRangedDamage(target, damageSource, weaponStack, ref damage);
			}
			if (applyBuffs)
			{
				WeaponBuffSystem.ModifyRangedDamage(target, damageSource, weaponStack, projectileStack, ref damage);
			}
			damage = GrindingWheelCompat.ApplyBuffableDamage(weaponStack, target, damage, damageSource);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00037A6E File Offset: 0x00035C6E
		[NullableContext(2)]
		internal void EmitRangedDamageResolved([Nullable(1)] Entity target, [Nullable(1)] DamageSource damageSource, ItemStack weaponStack, ItemStack projectileStack, float damage, bool damageReceived, DamageBlockEventArgs block)
		{
			ProjectileSystemServer.RangedDamageResolvedDelegate onRangedDamageResolved = this.OnRangedDamageResolved;
			if (onRangedDamageResolved == null)
			{
				return;
			}
			onRangedDamageResolved(new RangedDamageResolvedEventArgs(target, damageSource, weaponStack, projectileStack, damage, damageReceived, block));
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00037A90 File Offset: 0x00035C90
		private long[] GetProjectileCollisionCandidateEntityIds(ProjectileEntity projectile)
		{
			Vector3d currentPosition;
			currentPosition..ctor(projectile.ServerPos.X, projectile.ServerPos.Y, projectile.ServerPos.Z);
			Vector3d previousPosition;
			previousPosition..ctor(projectile.PreviousPosition.X, projectile.PreviousPosition.Y, projectile.PreviousPosition.Z);
			Vector3d segment = currentPosition - previousPosition;
			if (segment.LengthSquared <= 0.0)
			{
				return Array.Empty<long>();
			}
			Vec3d midPoint = new Vec3d(segment.X / 2.0 + previousPosition.X, segment.Y / 2.0 + previousPosition.Y, segment.Z / 2.0 + previousPosition.Z);
			double searchRadius = segment.Length / 2.0 + (double)projectile.ColliderRadius + 4.0;
			float candidateRadius = projectile.ColliderRadius + 4f;
			this._projectileCandidateScratch.Clear();
			foreach (Entity entity in this._api.World.GetEntitiesAround(midPoint, (float)searchRadius, (float)searchRadius, null))
			{
				Vector3d point;
				float num;
				if (ProjectileSystemServer.CanProjectileHit(entity) && !ProjectileSystemServer.ShouldIgnoreProjectileCandidate(projectile, entity) && ProjectileSystemServer.CheckEntityAabb(entity, currentPosition, previousPosition, candidateRadius, 0f, out point, out num, true))
				{
					double distance = (point - previousPosition).LengthSquared;
					this.AddProjectileCandidate(entity.EntityId, distance);
				}
			}
			if (this._projectileCandidateScratch.Count == 0)
			{
				return Array.Empty<long>();
			}
			int count = this._projectileCandidateScratch.Count;
			long[] result = new long[count];
			for (int index = 0; index < count; index++)
			{
				result[index] = this._projectileCandidateScratch[index].Item1;
			}
			return result;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00037C6C File Offset: 0x00035E6C
		private void AddProjectileCandidate(long entityId, double distance)
		{
			int insertIndex = this._projectileCandidateScratch.Count;
			while (insertIndex > 0 && distance < this._projectileCandidateScratch[insertIndex - 1].Item2)
			{
				insertIndex--;
			}
			if (insertIndex >= 16)
			{
				return;
			}
			this._projectileCandidateScratch.Insert(insertIndex, new ValueTuple<long, double>(entityId, distance));
			if (this._projectileCandidateScratch.Count > 16)
			{
				this._projectileCandidateScratch.RemoveAt(this._projectileCandidateScratch.Count - 1);
			}
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00037CE5 File Offset: 0x00035EE5
		private static bool ShouldIgnoreProjectileCandidate(ProjectileEntity projectile, Entity entity)
		{
			return entity.EntityId == projectile.EntityId || entity.EntityId == projectile.ShooterId || projectile.CollidedWith.Contains(entity.EntityId);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00037D1D File Offset: 0x00035F1D
		private static bool CheckAABBOnly(ICoreAPI api, ProjectileEntity projectile)
		{
			return !(api.World.GetEntityById(projectile.ShooterId) is EntityPlayer);
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00037D3C File Offset: 0x00035F3C
		private static bool IsFirearmsProjectile(ProjectileEntity projectile)
		{
			AssetLocation code = projectile.Code;
			if (((code != null) ? code.Domain : null) == "maltiezfirearms")
			{
				return true;
			}
			ItemStack weaponStack = projectile.WeaponStack;
			if (!ProjectileSystemServer.IsFirearmsCollectible((weaponStack != null) ? weaponStack.Collectible : null))
			{
				ItemStack projectileStack = projectile.ProjectileStack;
				return ProjectileSystemServer.IsFirearmsCollectible((projectileStack != null) ? projectileStack.Collectible : null);
			}
			return true;
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00037D9B File Offset: 0x00035F9B
		[NullableContext(2)]
		private static bool IsFirearmsCollectible(CollectibleObject collectible)
		{
			if (collectible == null)
			{
				return false;
			}
			AssetLocation code = collectible.Code;
			return ((code != null) ? code.Domain : null) == "maltiezfirearms" || CollectibleClassifier.IsFirearm(collectible);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00037DC8 File Offset: 0x00035FC8
		private static bool IsBodyPartRangedProjectile(ProjectileEntity projectile, bool isFirearmsProjectile)
		{
			if (isFirearmsProjectile)
			{
				return true;
			}
			ItemStack weaponStack = projectile.WeaponStack;
			return CollectibleClassifier.IsBow(weaponStack) || CollectibleClassifier.IsCrossbow(weaponStack);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00037DF1 File Offset: 0x00035FF1
		private static bool CanProjectileHit(Entity entity)
		{
			return entity.Alive && (entity.IsCreature || entity.GetBehavior<EntityBehaviorHealth>() != null);
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00037E10 File Offset: 0x00036010
		private static bool CheckEntityAabb(Entity target, Vector3d currentPosition, Vector3d previousPosition, float radius, float penetrationResistance, out Vector3d point, out float penetrationStrengthLoss, bool useServerPosition = false)
		{
			return ProjectileSystemServer.CheckCollisionBox(ProjectileSystemServer.GetCollisionBox(target, useServerPosition), currentPosition, previousPosition, radius, penetrationResistance, out point, out penetrationStrengthLoss);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00037E28 File Offset: 0x00036028
		private static bool CheckCollisionBox(CuboidAABBCollider collisionBox, Vector3d currentPosition, Vector3d previousPosition, float radius, float penetrationResistance, out Vector3d point, out float penetrationStrengthLoss)
		{
			point = default(Vector3d);
			penetrationStrengthLoss = 0f;
			if (!collisionBox.Collide(currentPosition, previousPosition, (double)radius, out point))
			{
				return false;
			}
			penetrationStrengthLoss = penetrationResistance;
			return true;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00037E54 File Offset: 0x00036054
		private static CuboidAABBCollider GetCollisionBox(Entity entity, bool useServerPosition)
		{
			Cuboidf collisionBox = entity.CollisionBox;
			EntityPos position = useServerPosition ? entity.ServerPos : entity.Pos;
			return new CuboidAABBCollider(new Vector3d((double)collisionBox.X1 + position.X, (double)collisionBox.Y1 + position.Y, (double)collisionBox.Z1 + position.Z), new Vector3d((double)collisionBox.X2 + position.X, (double)collisionBox.Y2 + position.Y, (double)collisionBox.Z2 + position.Z));
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00037EE0 File Offset: 0x000360E0
		private static void SpawnProjectile(Guid id, ItemStack projectileStack, [Nullable(2)] ItemStack weaponStack, ProjectileStats stats, ProjectileSpawnStats spawnStats, ICoreAPI api, Entity shooter, Entity owner, [Nullable(2)] out ProjectileEntity projectile)
		{
			AssetLocation entityTypeAsset = new AssetLocation(stats.EntityCode);
			EntityProperties entityType2 = api.World.GetEntityType(entityTypeAsset);
			if (entityType2 == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(41, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[Overhaul lib] Unable to create entity '");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(entityTypeAsset);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			EntityProperties entityType = entityType2;
			Entity entity2 = api.ClassRegistry.CreateEntity(entityType);
			if (entity2 == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(41, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("[Overhaul lib] Unable to create entity '");
				defaultInterpolatedStringHandler2.AppendFormatted<AssetLocation>(entityTypeAsset);
				defaultInterpolatedStringHandler2.AppendLiteral("'");
				throw new InvalidOperationException(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			Entity entity = entity2;
			entity.ServerPos.SetPos(new Vec3d(spawnStats.Position.X, spawnStats.Position.Y, spawnStats.Position.Z));
			entity.ServerPos.Motion.Set(new Vec3d(spawnStats.Velocity.X, spawnStats.Velocity.Y, spawnStats.Velocity.Z));
			entity.Pos.SetFrom(entity.ServerPos);
			entity.World = api.World;
			projectile = (entity as ProjectileEntity);
			if (projectile != null)
			{
				projectile.ProjectileId = id;
				projectile.ProjectileStack = projectileStack;
				projectile.WeaponStack = weaponStack;
				projectile.DropOnImpactChance = stats.DropChance;
				projectile.ColliderRadius = stats.CollisionRadius;
				projectile.PenetrationDistance = stats.PenetrationDistance;
				projectile.PenetrationStrength = Math.Max(0f, stats.PenetrationBonus + (float)spawnStats.DamageTier);
				projectile.DurabilityDamageOnImpact = stats.DurabilityDamage;
				projectile.ShooterId = shooter.EntityId;
				projectile.OwnerId = owner.EntityId;
				projectile.ShooterPosition.Set(shooter.ServerPos.X, shooter.ServerPos.Y, shooter.ServerPos.Z);
				projectile.CanBeCollected = stats.CanBeCollected;
				projectile.IgnoreInvFrames = true;
				projectile.SetRotation();
			}
			else
			{
				IProjectile vanillaProjectile = entity as IProjectile;
				if (vanillaProjectile != null)
				{
					vanillaProjectile.FiredBy = shooter;
					vanillaProjectile.Damage = stats.DamageStats.Damage * spawnStats.DamageMultiplier;
					vanillaProjectile.DamageTier = spawnStats.DamageTier + stats.DamageTierBonus;
					vanillaProjectile.ProjectileStack = projectileStack;
					vanillaProjectile.WeaponStack = weaponStack;
					vanillaProjectile.DropOnImpactChance = stats.DropChance;
					vanillaProjectile.DamageStackOnImpact = (stats.DurabilityDamage > 0);
					vanillaProjectile.Weight = entity.Properties.Weight;
					vanillaProjectile.IgnoreInvFrames = true;
					vanillaProjectile.PreInitialize();
				}
			}
			api.World.SpawnEntity(entity);
			float gravityFactorMultiplier = (spawnStats.GravityFactorMultiplier <= 0f) ? 1f : spawnStats.GravityFactorMultiplier;
			if (Math.Abs(gravityFactorMultiplier - 1f) > 0.0001f)
			{
				EntityBehaviorPassivePhysics physics = entity.GetBehavior<EntityBehaviorPassivePhysics>();
				if (physics != null)
				{
					physics.GravityPerSecond *= (double)gravityFactorMultiplier;
				}
			}
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00038200 File Offset: 0x00036400
		private void ClearId(Guid id)
		{
			this.PruneLateProjectiles();
			ProjectileServer projectileServer;
			if (this._projectiles.TryGetValue(id, ref projectileServer) && ProjectileSystemServer.IsFirearmsProjectile(projectileServer._entity))
			{
				this.TrimLateProjectileCacheForInsert();
				this._lateProjectiles[id] = new ProjectileSystemServer.LateProjectileCollision(projectileServer, projectileServer.PacketVersion, this._api.World.ElapsedMilliseconds + 1500L);
			}
			this._projectiles.Remove(id);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00038274 File Offset: 0x00036474
		private void HandleCollision(IServerPlayer player, ProjectileCollisionPacket packet)
		{
			this.PruneLateProjectiles();
			ProjectileServer projectileServer;
			if (this._projectiles.TryGetValue(packet.Id, ref projectileServer))
			{
				string rejectionReason;
				if (!this.IsValidCollisionPacket(player, projectileServer, packet, projectileServer.PacketVersion, out rejectionReason))
				{
					this.RecordFirearmsCollisionRejection(projectileServer, rejectionReason);
					return;
				}
				ProjectileServer projectileServer2 = projectileServer;
				int packetVersion = projectileServer2.PacketVersion;
				projectileServer2.PacketVersion = packetVersion + 1;
				projectileServer._entity.CollidedWith.Add(packet.ReceiverEntity);
				projectileServer._entity.Stuck = false;
				projectileServer._entity.CollidedVertically = false;
				projectileServer._entity.CollidedHorizontally = false;
				projectileServer.OnCollision(packet);
				return;
			}
			else
			{
				ProjectileSystemServer.LateProjectileCollision lateCollision;
				if (!this._lateProjectiles.TryGetValue(packet.Id, ref lateCollision))
				{
					return;
				}
				if (this._api.World.ElapsedMilliseconds > lateCollision.ExpiresAtMs)
				{
					this.RecordFirearmsCollisionRejection(lateCollision.Projectile, "expired-late-hit");
					this._lateProjectiles.Remove(packet.Id);
					return;
				}
				string lateRejectionReason;
				if (!this.IsValidCollisionPacket(player, lateCollision.Projectile, packet, lateCollision.PacketVersion, out lateRejectionReason))
				{
					this.RecordFirearmsCollisionRejection(lateCollision.Projectile, lateRejectionReason);
					return;
				}
				this._lateProjectiles.Remove(packet.Id);
				lateCollision.Projectile.OnLateCollision(packet);
				return;
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x000383A4 File Offset: 0x000365A4
		private void PruneLateProjectiles()
		{
			if (this._lateProjectiles.Count == 0)
			{
				return;
			}
			long now = this._api.World.ElapsedMilliseconds;
			this._lateProjectileRemovalScratch.Clear();
			foreach (KeyValuePair<Guid, ProjectileSystemServer.LateProjectileCollision> keyValuePair in this._lateProjectiles)
			{
				Guid guid;
				ProjectileSystemServer.LateProjectileCollision lateProjectileCollision;
				keyValuePair.Deconstruct(ref guid, ref lateProjectileCollision);
				Guid id = guid;
				ProjectileSystemServer.LateProjectileCollision lateCollision = lateProjectileCollision;
				if (now > lateCollision.ExpiresAtMs)
				{
					this._lateProjectileRemovalScratch.Add(id);
				}
			}
			foreach (Guid id2 in this._lateProjectileRemovalScratch)
			{
				this._lateProjectiles.Remove(id2);
			}
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0003848C File Offset: 0x0003668C
		private void TrimLateProjectileCacheForInsert()
		{
			int overflow = this._lateProjectiles.Count - 4096 + 1;
			if (overflow <= 0)
			{
				return;
			}
			this._lateProjectileRemovalScratch.Clear();
			for (int removeIndex = 0; removeIndex < overflow; removeIndex++)
			{
				Guid oldestId = default(Guid);
				long oldestExpiresAt = long.MaxValue;
				foreach (KeyValuePair<Guid, ProjectileSystemServer.LateProjectileCollision> keyValuePair in this._lateProjectiles)
				{
					Guid guid;
					ProjectileSystemServer.LateProjectileCollision lateProjectileCollision;
					keyValuePair.Deconstruct(ref guid, ref lateProjectileCollision);
					Guid id = guid;
					ProjectileSystemServer.LateProjectileCollision lateCollision = lateProjectileCollision;
					if (!this._lateProjectileRemovalScratch.Contains(id) && lateCollision.ExpiresAtMs < oldestExpiresAt)
					{
						oldestId = id;
						oldestExpiresAt = lateCollision.ExpiresAtMs;
					}
				}
				if (oldestExpiresAt == 9223372036854775807L)
				{
					break;
				}
				this._lateProjectileRemovalScratch.Add(oldestId);
			}
			foreach (Guid id2 in this._lateProjectileRemovalScratch)
			{
				this._lateProjectiles.Remove(id2);
			}
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x000385BC File Offset: 0x000367BC
		private void RecordFirearmsCollisionRejection(ProjectileServer projectileServer, string reason)
		{
			if (!ProjectileSystemServer.IsFirearmsProjectile(projectileServer._entity))
			{
				return;
			}
			int count;
			this._firearmsCollisionRejectionCounts.TryGetValue(reason, ref count);
			this._firearmsCollisionRejectionCounts[reason] = count + 1;
			this.FlushFirearmsCollisionRejectionDiagnostics();
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x000385FC File Offset: 0x000367FC
		private void FlushFirearmsCollisionRejectionDiagnostics()
		{
			long now = this._api.World.ElapsedMilliseconds;
			if (this._nextFirearmsCollisionDiagnosticsAtMs == 0L)
			{
				this._nextFirearmsCollisionDiagnosticsAtMs = now + 10000L;
				return;
			}
			if (now < this._nextFirearmsCollisionDiagnosticsAtMs || this._firearmsCollisionRejectionCounts.Count == 0)
			{
				return;
			}
			string counts = string.Join(", ", Enumerable.Select<KeyValuePair<string, int>, string>(Enumerable.OrderBy<KeyValuePair<string, int>, string>(this._firearmsCollisionRejectionCounts, (KeyValuePair<string, int> entry) => entry.Key), delegate(KeyValuePair<string, int> entry)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted(entry.Key);
				defaultInterpolatedStringHandler.AppendLiteral("=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(entry.Value);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}));
			LoggerUtil.Verbose(this._api, typeof(ProjectileSystemServer), "Firearms projectile collision rejection counts: " + counts);
			this._firearmsCollisionRejectionCounts.Clear();
			this._nextFirearmsCollisionDiagnosticsAtMs = now + 10000L;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x000386DC File Offset: 0x000368DC
		private bool IsValidCollisionPacket(IServerPlayer player, ProjectileServer projectileServer, ProjectileCollisionPacket packet, int expectedPacketVersion, out string rejectionReason)
		{
			rejectionReason = "";
			if (packet.PacketVersion != expectedPacketVersion)
			{
				rejectionReason = "wrong-packet-version";
				return false;
			}
			if (!ProjectileSystemServer.IsPacketSenderOwner(player, projectileServer))
			{
				rejectionReason = "wrong-owner";
				return false;
			}
			Entity target = this._api.World.GetEntityById(packet.ReceiverEntity);
			if (target == null || !ProjectileSystemServer.CanProjectileHit(target))
			{
				rejectionReason = "invalid-target";
				return false;
			}
			if (target.EntityId == projectileServer._entity.EntityId || target.EntityId == projectileServer._entity.ShooterId)
			{
				rejectionReason = "self-or-shooter";
				return false;
			}
			if (this.IsCollisionBehindShooter(projectileServer._entity, packet))
			{
				rejectionReason = "behind-shooter";
				return false;
			}
			return true;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00038790 File Offset: 0x00036990
		private bool IsCollisionBehindShooter(ProjectileEntity projectile, ProjectileCollisionPacket packet)
		{
			if (packet.CollisionPoint == null || packet.CollisionPoint.Length < 3)
			{
				return false;
			}
			if (!(this._api.World.GetEntityById(projectile.ShooterId) is EntityPlayer))
			{
				return false;
			}
			Vector3d shotDirection = ProjectileSystemServer.GetProjectileShotDirection(projectile);
			if (shotDirection.LengthSquared <= 1E-08)
			{
				return false;
			}
			Vec3d shooterPositionVanilla = ProjectileSystemServer.GetProjectileShooterPosition(projectile, null);
			Vector3d shooterPosition;
			shooterPosition..ctor(shooterPositionVanilla.X, shooterPositionVanilla.Y, shooterPositionVanilla.Z);
			Vector3d collisionPoint;
			collisionPoint..ctor(packet.CollisionPoint[0], packet.CollisionPoint[1], packet.CollisionPoint[2]);
			Vector3d horizontalShotDirection;
			horizontalShotDirection..ctor(shotDirection.X, 0.0, shotDirection.Z);
			if (horizontalShotDirection.LengthSquared <= 1E-08)
			{
				horizontalShotDirection = shotDirection;
			}
			horizontalShotDirection = horizontalShotDirection.Normalized();
			Vector3d toPoint;
			toPoint..ctor(collisionPoint.X - shooterPosition.X, 0.0, collisionPoint.Z - shooterPosition.Z);
			if (toPoint.LengthSquared <= 1E-08)
			{
				toPoint = collisionPoint - shooterPosition;
			}
			return Vector3d.Dot(toPoint, horizontalShotDirection) < -0.25;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x000388C0 File Offset: 0x00036AC0
		private static Vector3d GetProjectileShotDirection(ProjectileEntity projectile)
		{
			Vector3d velocity;
			velocity..ctor(projectile.PreviousVelocity.X, projectile.PreviousVelocity.Y, projectile.PreviousVelocity.Z);
			if (velocity.LengthSquared <= 1E-08)
			{
				velocity..ctor(projectile.ServerPos.Motion.X, projectile.ServerPos.Motion.Y, projectile.ServerPos.Motion.Z);
			}
			if (velocity.LengthSquared <= 1E-08)
			{
				velocity..ctor(projectile.ServerPos.X - projectile.PreviousPosition.X, projectile.ServerPos.Y - projectile.PreviousPosition.Y, projectile.ServerPos.Z - projectile.PreviousPosition.Z);
			}
			if (velocity.LengthSquared > 1E-08)
			{
				return velocity.Normalized();
			}
			return Vector3d.Zero;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x000389BC File Offset: 0x00036BBC
		private static Vec3d GetProjectileShooterPosition(ProjectileEntity projectile, [Nullable(2)] Entity shooter)
		{
			if (projectile.ShooterPosition.LengthSq() > 1E-08)
			{
				return projectile.ShooterPosition;
			}
			if (shooter != null)
			{
				return new Vec3d(shooter.ServerPos.X, shooter.ServerPos.Y, shooter.ServerPos.Z);
			}
			return new Vec3d(projectile.PreviousPosition.X, projectile.PreviousPosition.Y, projectile.PreviousPosition.Z);
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00038A38 File Offset: 0x00036C38
		private static bool IsPacketSenderOwner(IServerPlayer player, ProjectileServer projectileServer)
		{
			EntityPlayer entity = player.Entity;
			long? num = (entity != null) ? new long?(entity.EntityId) : default(long?);
			long ownerId = projectileServer._entity.OwnerId;
			return num.GetValueOrDefault() == ownerId & num != null;
		}

		// Token: 0x040004E9 RID: 1257
		public const string NetworkChannelId = "CombatOverhaul:projectiles";

		// Token: 0x040004EA RID: 1258
		public const float PenetrationDistanceOffset = 5f;

		// Token: 0x040004EB RID: 1259
		private readonly ICoreServerAPI _api;

		// Token: 0x040004EC RID: 1260
		private readonly IServerNetworkChannel _serverChannel;

		// Token: 0x040004ED RID: 1261
		private readonly Dictionary<Guid, ProjectileServer> _projectiles = new Dictionary<Guid, ProjectileServer>();

		// Token: 0x040004EE RID: 1262
		private readonly Dictionary<Guid, ProjectileSystemServer.LateProjectileCollision> _lateProjectiles = new Dictionary<Guid, ProjectileSystemServer.LateProjectileCollision>();

		// Token: 0x040004EF RID: 1263
		private readonly Dictionary<string, int> _firearmsCollisionRejectionCounts = new Dictionary<string, int>();

		// Token: 0x040004F0 RID: 1264
		[TupleElementNames(new string[]
		{
			"EntityId",
			"Distance"
		})]
		[Nullable(new byte[]
		{
			1,
			0
		})]
		private readonly List<ValueTuple<long, double>> _projectileCandidateScratch = new List<ValueTuple<long, double>>(16);

		// Token: 0x040004F1 RID: 1265
		private readonly List<Guid> _lateProjectileRemovalScratch = new List<Guid>();

		// Token: 0x040004F2 RID: 1266
		private const float _nearestPlayerSearchRange = 300f;

		// Token: 0x040004F3 RID: 1267
		private const float FirearmsProjectileCandidatePadding = 4f;

		// Token: 0x040004F4 RID: 1268
		private const int MaxFirearmsProjectileCollisionCandidates = 16;

		// Token: 0x040004F5 RID: 1269
		private const int LateCollisionGraceMs = 1500;

		// Token: 0x040004F6 RID: 1270
		private const int MaxLateProjectileCacheSize = 4096;

		// Token: 0x040004F7 RID: 1271
		private const int FirearmsCollisionDiagnosticsIntervalMs = 10000;

		// Token: 0x040004F8 RID: 1272
		private const double BehindShooterTolerance = -0.25;

		// Token: 0x040004F9 RID: 1273
		private long _nextFirearmsCollisionDiagnosticsAtMs;

		// Token: 0x0200024A RID: 586
		// (Invoke) Token: 0x0600166C RID: 5740
		[NullableContext(0)]
		public delegate void RangedDamageDelegate(Entity target, DamageSource damageSource, [Nullable(2)] ItemStack weaponStack, ref float damage);

		// Token: 0x0200024B RID: 587
		// (Invoke) Token: 0x06001670 RID: 5744
		[NullableContext(0)]
		public delegate void RangedDamageResolvedDelegate(RangedDamageResolvedEventArgs args);

		// Token: 0x0200024C RID: 588
		[Nullable(0)]
		private sealed class LateProjectileCollision
		{
			// Token: 0x06001673 RID: 5747 RVA: 0x0006A1C5 File Offset: 0x000683C5
			public LateProjectileCollision(ProjectileServer projectile, int packetVersion, long expiresAtMs)
			{
				this.Projectile = projectile;
				this.PacketVersion = packetVersion;
				this.ExpiresAtMs = expiresAtMs;
			}

			// Token: 0x17000524 RID: 1316
			// (get) Token: 0x06001674 RID: 5748 RVA: 0x0006A1E2 File Offset: 0x000683E2
			public ProjectileServer Projectile { get; }

			// Token: 0x17000525 RID: 1317
			// (get) Token: 0x06001675 RID: 5749 RVA: 0x0006A1EA File Offset: 0x000683EA
			public int PacketVersion { get; }

			// Token: 0x17000526 RID: 1318
			// (get) Token: 0x06001676 RID: 5750 RVA: 0x0006A1F2 File Offset: 0x000683F2
			public long ExpiresAtMs { get; }
		}
	}
}
