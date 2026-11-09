using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Colliders;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D2 RID: 210
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ProjectileSystemClient
	{
		// Token: 0x06000AD0 RID: 2768 RVA: 0x000376A0 File Offset: 0x000358A0
		public ProjectileSystemClient(ICoreClientAPI api, EntityPartitioning entityPartitioning)
		{
			this._clientChannel = api.Network.RegisterChannel("CombatOverhaul:projectiles").RegisterMessageType<ProjectileCollisionPacket>().RegisterMessageType<ProjectileCollisionCheckRequest>().SetMessageHandler<ProjectileCollisionCheckRequest>(new NetworkServerMessageHandler<ProjectileCollisionCheckRequest>(this.HandleRequest));
			this._api = api;
			this._entityPartitioning = entityPartitioning;
			this._combatOverhaulSystem = this._api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00037714 File Offset: 0x00035914
		public void Collide(Guid id, Entity target, Vector3d point, Vector3d velocity, double relativeSpeed, string collider, ProjectileCollisionCheckRequest packet, float penetrationStrenghLoss)
		{
			ProjectileCollisionPacket newPacket = new ProjectileCollisionPacket
			{
				Id = id,
				CollisionPoint = new double[]
				{
					point.X,
					point.Y,
					point.Z
				},
				AfterCollisionVelocity = new double[]
				{
					velocity.X,
					velocity.Y,
					velocity.Z
				},
				RelativeSpeed = relativeSpeed,
				ReceiverEntity = target.EntityId,
				Collider = collider,
				PacketVersion = packet.PacketVersion,
				PenetrationStrengthLoss = penetrationStrenghLoss
			};
			this._clientChannel.SendPacket<ProjectileCollisionPacket>(newPacket);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x000377C0 File Offset: 0x000359C0
		private void HandleRequest(ProjectileCollisionCheckRequest packet)
		{
			Vector3d currentPosition;
			currentPosition..ctor(packet.CurrentPosition[0], packet.CurrentPosition[1], packet.CurrentPosition[2]);
			Vector3d previousPosition;
			previousPosition..ctor(packet.PreviousPosition[0], packet.PreviousPosition[1], packet.PreviousPosition[2]);
			Vector3d velocity;
			velocity..ctor(packet.Velocity[0], packet.Velocity[1], packet.Velocity[2]);
			Vector3d segment = currentPosition - previousPosition;
			Vec3d midPoint = new Vec3d(segment.X / 2.0 + previousPosition.X, segment.Y / 2.0 + previousPosition.Y, segment.Z / 2.0 + previousPosition.Z);
			double searchRadius = Math.Max((double)(this._combatOverhaulSystem.Settings.CollisionRadius + packet.Radius), segment.Length / 2.0 + (double)this._combatOverhaulSystem.Settings.CollisionRadius + (double)packet.Radius);
			long[] candidateEntityIds = packet.CandidateEntityIds ?? Array.Empty<long>();
			if (candidateEntityIds.Length != 0)
			{
				foreach (long entityId in candidateEntityIds)
				{
					Entity entity = this._api.World.GetEntityById(entityId);
					if (entity != null && ProjectileSystemClient.CanProjectileHit(entity) && this.Collide(entity, packet, currentPosition, previousPosition, velocity))
					{
						return;
					}
				}
				return;
			}
			if (this.TryCollidePartitionedEntities(midPoint, searchRadius, packet, currentPosition, previousPosition, velocity))
			{
				return;
			}
			foreach (Entity entity2 in this._api.World.GetEntitiesAround(midPoint, (float)searchRadius, (float)searchRadius, null))
			{
				if (ProjectileSystemClient.CanProjectileHit(entity2) && this.Collide(entity2, packet, currentPosition, previousPosition, velocity))
				{
					return;
				}
			}
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0003798C File Offset: 0x00035B8C
		private bool TryCollidePartitionedEntities(Vec3d midPoint, double searchRadius, ProjectileCollisionCheckRequest packet, Vector3d currentPosition, Vector3d previousPosition, Vector3d velocity)
		{
			bool collided = false;
			this._entityPartitioning.WalkEntities(midPoint, searchRadius, delegate(Entity entity)
			{
				if (!ProjectileSystemClient.CanProjectileHit(entity))
				{
					return true;
				}
				if (this.Collide(entity, packet, currentPosition, previousPosition, velocity))
				{
					collided = true;
					return false;
				}
				return true;
			}, 0);
			return collided;
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x000379EC File Offset: 0x00035BEC
		private void RecordFirearmsCollisionMiss(string reason)
		{
			int count;
			this._firearmsCollisionMissCounts.TryGetValue(reason, ref count);
			this._firearmsCollisionMissCounts[reason] = count + 1;
			this.FlushFirearmsCollisionMissDiagnostics();
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00037A20 File Offset: 0x00035C20
		private void FlushFirearmsCollisionMissDiagnostics()
		{
			long now = this._api.World.ElapsedMilliseconds;
			if (this._nextFirearmsCollisionDiagnosticsAtMs == 0L)
			{
				this._nextFirearmsCollisionDiagnosticsAtMs = now + 10000L;
				return;
			}
			if (now < this._nextFirearmsCollisionDiagnosticsAtMs || this._firearmsCollisionMissCounts.Count == 0)
			{
				return;
			}
			string counts = string.Join(", ", Enumerable.Select<KeyValuePair<string, int>, string>(Enumerable.OrderBy<KeyValuePair<string, int>, string>(this._firearmsCollisionMissCounts, (KeyValuePair<string, int> entry) => entry.Key), delegate(KeyValuePair<string, int> entry)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted(entry.Key);
				defaultInterpolatedStringHandler.AppendLiteral("=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(entry.Value);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}));
			LoggerUtil.Verbose(this._api, typeof(ProjectileSystemClient), "Firearms projectile CO collider miss counts: " + counts);
			this._firearmsCollisionMissCounts.Clear();
			this._nextFirearmsCollisionDiagnosticsAtMs = now + 10000L;
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00037AFE File Offset: 0x00035CFE
		private static bool CanProjectileHit(Entity entity)
		{
			return entity.Alive && (entity.IsCreature || entity.GetBehavior<EntityBehaviorHealth>() != null);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00037B20 File Offset: 0x00035D20
		private bool Collide(Entity target, ProjectileCollisionCheckRequest packet, Vector3d currentPosition, Vector3d previousPosition, Vector3d velocity)
		{
			if (target.EntityId == packet.ProjectileEntityId)
			{
				return false;
			}
			if (!packet.CollideWithShooter && packet.ShooterId == target.EntityId)
			{
				return false;
			}
			if (Array.IndexOf<long>(packet.IgnoreEntities, target.EntityId) >= 0)
			{
				return false;
			}
			string collider;
			Vector3d point;
			float penetrationStrengthLoss;
			string collisionMode;
			string missReason;
			if (!this.CheckCollision(target, out collider, out point, currentPosition, previousPosition, packet.Radius, packet.PenetrationDistance, packet.PenetrationStrength, out penetrationStrengthLoss, packet.CheckAABBOnly, packet.RequireColliderWhenAvailable, out collisionMode, out missReason))
			{
				if (packet.RequireColliderWhenAvailable && collisionMode == "CO" && missReason.Length > 0)
				{
					this.RecordFirearmsCollisionMiss(missReason);
				}
				return false;
			}
			if (ProjectileSystemClient.IsCollisionBehindShooter(packet, point))
			{
				return false;
			}
			Vector3d targetVelocity;
			targetVelocity..ctor((double)((float)target.Pos.Motion.X), (double)((float)target.Pos.Motion.Y), (double)((float)target.Pos.Motion.Z));
			double relativeSpeed = (targetVelocity - velocity).Length;
			this.Collide(packet.ProjectileId, target, point, targetVelocity, relativeSpeed, collider, packet, penetrationStrengthLoss);
			return true;
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00037C3C File Offset: 0x00035E3C
		private static bool IsCollisionBehindShooter(ProjectileCollisionCheckRequest packet, Vector3d point)
		{
			if (packet.ShooterPosition == null || packet.ShooterPosition.Length < 3)
			{
				return false;
			}
			if (packet.ShotDirection == null || packet.ShotDirection.Length < 3)
			{
				return false;
			}
			Vector3d shooterPosition;
			shooterPosition..ctor(packet.ShooterPosition[0], packet.ShooterPosition[1], packet.ShooterPosition[2]);
			Vector3d shotDirection;
			shotDirection..ctor(packet.ShotDirection[0], 0.0, packet.ShotDirection[2]);
			if (shotDirection.LengthSquared <= 1E-08)
			{
				shotDirection..ctor(packet.ShotDirection[0], packet.ShotDirection[1], packet.ShotDirection[2]);
			}
			if (shotDirection.LengthSquared <= 1E-08)
			{
				return false;
			}
			shotDirection = shotDirection.Normalized();
			Vector3d toPoint;
			toPoint..ctor(point.X - shooterPosition.X, 0.0, point.Z - shooterPosition.Z);
			if (toPoint.LengthSquared <= 1E-08)
			{
				toPoint = point - shooterPosition;
			}
			return Vector3d.Dot(toPoint, shotDirection) < -0.25;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00037D58 File Offset: 0x00035F58
		private unsafe bool CheckCollision(Entity target, out string collider, out Vector3d point, Vector3d currentPosition, Vector3d previousPosition, float radius, float penetrationDistance, float penetrationSrength, out float penetrationStrengthLoss, bool checkAABBOnly, bool requireColliderWhenAvailable, out string collisionMode, out string missReason)
		{
			CollidersEntityBehavior colliders = target.GetBehavior<CollidersEntityBehavior>();
			EntityDamageModelBehavior damageModel = target.GetBehavior<EntityDamageModelBehavior>();
			PlayerDamageModelBehavior playerDamageModel = target.GetBehavior<PlayerDamageModelBehavior>();
			collider = "";
			point = default(Vector3d);
			penetrationStrengthLoss = 0f;
			bool hasUsableDetailedColliders = colliders != null && colliders.HasUsableDetailedColliders;
			collisionMode = ((colliders == null) ? "AABBNoColliders" : (checkAABBOnly ? "AABBOnly" : (hasUsableDetailedColliders ? "CO" : "AABBNoUsableColliders")));
			missReason = "";
			float defaultColliderPenetrationResistance = this._combatOverhaulSystem.Settings.DefaultColliderPenetrationResistance;
			bool requireDetailedCollider = requireColliderWhenAvailable && hasUsableDetailedColliders && !checkAABBOnly;
			Vector3d aabbPoint;
			float aabbPenetrationStrengthLoss;
			bool aabbHit = ProjectileSystemClient.CheckEntityAabb(target, currentPosition, previousPosition, radius, defaultColliderPenetrationResistance, out aabbPoint, out aabbPenetrationStrengthLoss);
			if (colliders != null && !checkAABBOnly && hasUsableDetailedColliders)
			{
				List<ValueTuple<string, double, Vector3d>> intersections;
				bool result;
				try
				{
					result = colliders.Collide(currentPosition, previousPosition, radius, penetrationDistance, out intersections);
				}
				catch (Exception exception)
				{
					missReason = "co-collide-exception:" + exception.GetType().Name;
					if (requireDetailedCollider)
					{
						return false;
					}
					if (!aabbHit)
					{
						return false;
					}
					point = aabbPoint;
					penetrationStrengthLoss = aabbPenetrationStrengthLoss;
					return true;
				}
				if (!result || intersections.Count == 0)
				{
					if (requireDetailedCollider)
					{
						missReason = (result ? "co-no-intersections" : "co-miss");
						return false;
					}
					if (aabbHit)
					{
						point = aabbPoint;
						penetrationStrengthLoss = aabbPenetrationStrengthLoss;
						return true;
					}
					return colliders.HasOBBCollider && colliders.Colliders.Count > 0 && ProjectileSystemClient.CheckCollisionBox(colliders.BoundingBox, currentPosition, previousPosition, radius, colliders.DefaultPenetrationResistance, out point, out penetrationStrengthLoss);
				}
				else
				{
					penetrationStrengthLoss = colliders.DefaultPenetrationResistance;
					if (damageModel == null && playerDamageModel == null)
					{
						collider = intersections[0].Item1;
						point = intersections[0].Item3;
						return true;
					}
					if (damageModel != null)
					{
						bool selectedHit = false;
						float maxDamageMultiplier = 0f;
						penetrationStrengthLoss = 0f;
						ColliderTypes selectedColliderType = ColliderTypes.Torso;
						Span<ColliderTypes> encounteredColliderTypes = new Span<ColliderTypes>(stackalloc byte[(UIntPtr)64], 16);
						int encounteredColliderTypeCount = 0;
						foreach (ValueTuple<string, double, Vector3d> valueTuple in intersections)
						{
							string key = valueTuple.Item1;
							Vector3d intersectionPoint = valueTuple.Item3;
							ColliderTypes colliderType;
							bool knownColliderType = colliders.CollidersTypes.TryGetValue(key, ref colliderType);
							if (!knownColliderType)
							{
								colliderType = ColliderTypes.Torso;
							}
							if (knownColliderType && colliderType == ColliderTypes.Resistant && colliders.ResistantCollidersStopProjectiles)
							{
								if (!selectedHit)
								{
									collider = key;
									point = intersectionPoint;
									selectedColliderType = colliderType;
									selectedHit = true;
								}
								penetrationStrengthLoss = penetrationSrength;
								break;
							}
							float multiplier;
							float damageMultiplier = damageModel.DamageMultipliers.TryGetValue(colliderType, ref multiplier) ? multiplier : 1f;
							if (!selectedHit || damageMultiplier >= maxDamageMultiplier)
							{
								maxDamageMultiplier = damageMultiplier;
								collider = ((knownColliderType || requireDetailedCollider) ? key : "");
								point = intersectionPoint;
								selectedColliderType = colliderType;
								selectedHit = true;
							}
							float penetrationResistance = colliders.DefaultPenetrationResistance;
							if (colliders.PenetrationResistances.ContainsKey(key))
							{
								penetrationResistance = colliders.PenetrationResistances[key];
							}
							if (!ProjectileSystemClient.Contains<ColliderTypes>(encounteredColliderTypes, encounteredColliderTypeCount, colliderType))
							{
								penetrationStrengthLoss += penetrationResistance;
								ProjectileSystemClient.AddEncountered<ColliderTypes>(encounteredColliderTypes, ref encounteredColliderTypeCount, colliderType);
							}
							if (penetrationStrengthLoss > penetrationSrength)
							{
								break;
							}
						}
						if (!selectedHit)
						{
							if (requireDetailedCollider)
							{
								missReason = "co-no-selected-collider";
								return false;
							}
							if (aabbHit)
							{
								collider = "";
								point = aabbPoint;
								penetrationStrengthLoss = aabbPenetrationStrengthLoss;
							}
							else
							{
								collider = "";
								point = intersections[0].Item3;
								penetrationStrengthLoss = colliders.DefaultPenetrationResistance;
							}
						}
						else if (selectedColliderType == ColliderTypes.Resistant && aabbHit && !requireDetailedCollider)
						{
							collider = "";
							point = aabbPoint;
							penetrationStrengthLoss = aabbPenetrationStrengthLoss;
						}
						return true;
					}
					if (playerDamageModel != null)
					{
						bool selectedHit2 = false;
						float maxDamageMultiplier2 = 0f;
						penetrationStrengthLoss = 0f;
						Span<PlayerBodyPart> encounteredColliderTypes2 = new Span<PlayerBodyPart>(stackalloc byte[(UIntPtr)64], 16);
						int encounteredColliderTypeCount2 = 0;
						foreach (ValueTuple<string, double, Vector3d> valueTuple2 in intersections)
						{
							string key2 = valueTuple2.Item1;
							Vector3d intersectionPoint2 = valueTuple2.Item3;
							PlayerBodyPart bodyType;
							if (playerDamageModel.CollidersToBodyParts.TryGetValue(key2, ref bodyType))
							{
								float damageMultiplier2 = ProjectileSystemClient.GetPlayerBodyPartMultiplier(playerDamageModel, bodyType);
								if (!selectedHit2 || damageMultiplier2 >= maxDamageMultiplier2)
								{
									maxDamageMultiplier2 = damageMultiplier2;
									collider = key2;
									point = intersectionPoint2;
									selectedHit2 = true;
								}
								float penetrationResistance2 = colliders.DefaultPenetrationResistance;
								if (colliders.PenetrationResistances.ContainsKey(key2))
								{
									penetrationResistance2 = colliders.PenetrationResistances[key2];
								}
								if (!ProjectileSystemClient.Contains<PlayerBodyPart>(encounteredColliderTypes2, encounteredColliderTypeCount2, bodyType))
								{
									penetrationStrengthLoss += penetrationResistance2;
									ProjectileSystemClient.AddEncountered<PlayerBodyPart>(encounteredColliderTypes2, ref encounteredColliderTypeCount2, bodyType);
								}
								if (penetrationStrengthLoss > penetrationSrength)
								{
									break;
								}
							}
						}
						if (!selectedHit2)
						{
							if (requireDetailedCollider)
							{
								missReason = "co-no-mapped-player-body-part";
								return false;
							}
							if (aabbHit)
							{
								collider = "";
								point = aabbPoint;
								penetrationStrengthLoss = aabbPenetrationStrengthLoss;
							}
							else
							{
								collider = "";
								point = intersections[0].Item3;
								penetrationStrengthLoss = colliders.DefaultPenetrationResistance;
							}
						}
					}
					return true;
				}
				bool result2;
				return result2;
			}
			if (!aabbHit)
			{
				missReason = "aabb-miss";
				return false;
			}
			point = aabbPoint;
			penetrationStrengthLoss = aabbPenetrationStrengthLoss;
			return true;
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00038280 File Offset: 0x00036480
		private unsafe static bool Contains<[Nullable(2)] T>([Nullable(new byte[]
		{
			0,
			1
		})] Span<T> values, int count, T value)
		{
			for (int index = 0; index < count; index++)
			{
				if (EqualityComparer<T>.Default.Equals(*values[index], value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x000382B8 File Offset: 0x000364B8
		private unsafe static void AddEncountered<[Nullable(2)] T>([Nullable(new byte[]
		{
			0,
			1
		})] Span<T> values, ref int count, T value)
		{
			if (count >= values.Length)
			{
				return;
			}
			int num = count;
			count = num + 1;
			*values[num] = value;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x000382E8 File Offset: 0x000364E8
		private static float GetPlayerBodyPartMultiplier(PlayerDamageModelBehavior playerDamageModel, PlayerBodyPart bodyType)
		{
			foreach (DamageZoneStats damageZone in playerDamageModel.DamageModel.DamageZones)
			{
				if (damageZone.ZoneType == bodyType)
				{
					return damageZone.DamageMultiplier;
				}
			}
			return 1f;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0003832C File Offset: 0x0003652C
		private static bool CheckEntityAabb(Entity target, Vector3d currentPosition, Vector3d previousPosition, float radius, float penetrationResistance, out Vector3d point, out float penetrationStrengthLoss)
		{
			return ProjectileSystemClient.CheckCollisionBox(ProjectileSystemClient.GetCollisionBox(target), currentPosition, previousPosition, radius, penetrationResistance, out point, out penetrationStrengthLoss);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x00038342 File Offset: 0x00036542
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

		// Token: 0x06000ADF RID: 2783 RVA: 0x0003836C File Offset: 0x0003656C
		private static CuboidAABBCollider GetCollisionBox(Entity entity)
		{
			Cuboidf collisionBox = entity.CollisionBox;
			EntityPos position = entity.Pos;
			return new CuboidAABBCollider(new Vector3d((double)collisionBox.X1 + position.X, (double)collisionBox.Y1 + position.Y, (double)collisionBox.Z1 + position.Z), new Vector3d((double)collisionBox.X2 + position.X, (double)collisionBox.Y2 + position.Y, (double)collisionBox.Z2 + position.Z));
		}

		// Token: 0x04000500 RID: 1280
		public const string NetworkChannelId = "CombatOverhaul:projectiles";

		// Token: 0x04000501 RID: 1281
		private readonly ICoreClientAPI _api;

		// Token: 0x04000502 RID: 1282
		private readonly IClientNetworkChannel _clientChannel;

		// Token: 0x04000503 RID: 1283
		private readonly EntityPartitioning _entityPartitioning;

		// Token: 0x04000504 RID: 1284
		private readonly CombatOverhaulSystem _combatOverhaulSystem;

		// Token: 0x04000505 RID: 1285
		private readonly Dictionary<string, int> _firearmsCollisionMissCounts = new Dictionary<string, int>();

		// Token: 0x04000506 RID: 1286
		private long _nextFirearmsCollisionDiagnosticsAtMs;

		// Token: 0x04000507 RID: 1287
		private const int FirearmsCollisionDiagnosticsIntervalMs = 10000;

		// Token: 0x04000508 RID: 1288
		private const double BehindShooterTolerance = -0.25;
	}
}
