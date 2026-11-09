using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Colliders;
using CombatOverhaul.Implementations;
using CombatOverhaul.Inputs;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F7 RID: 247
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeleeAttack
	{
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x0003E664 File Offset: 0x0003C864
		public MeleeDamageType[] DamageTypes { get; }

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x0003E66C File Offset: 0x0003C86C
		// (set) Token: 0x06000C77 RID: 3191 RVA: 0x0003E674 File Offset: 0x0003C874
		public MeleeDamagePacket[] LastDamagePackets { get; private set; } = Array.Empty<MeleeDamagePacket>();

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x0003E67D File Offset: 0x0003C87D
		// (set) Token: 0x06000C79 RID: 3193 RVA: 0x0003E685 File Offset: 0x0003C885
		public bool StopOnTerrainHit { get; set; }

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x0003E68E File Offset: 0x0003C88E
		// (set) Token: 0x06000C7B RID: 3195 RVA: 0x0003E696 File Offset: 0x0003C896
		public bool StopOnEntityHit { get; set; }

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x0003E69F File Offset: 0x0003C89F
		// (set) Token: 0x06000C7D RID: 3197 RVA: 0x0003E6A7 File Offset: 0x0003C8A7
		public bool CollideWithTerrain { get; set; }

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x0003E6B0 File Offset: 0x0003C8B0
		// (set) Token: 0x06000C7F RID: 3199 RVA: 0x0003E6B8 File Offset: 0x0003C8B8
		public bool HitOnlyOneEntity { get; set; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x0003E6C1 File Offset: 0x0003C8C1
		// (set) Token: 0x06000C81 RID: 3201 RVA: 0x0003E6C9 File Offset: 0x0003C8C9
		public float MaxReach { get; set; }

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x0003E6D2 File Offset: 0x0003C8D2
		// (set) Token: 0x06000C83 RID: 3203 RVA: 0x0003E6DA File Offset: 0x0003C8DA
		[Nullable(2)]
		public ImpaleAttackStats Impale { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x06000C84 RID: 3204 RVA: 0x0003E6E4 File Offset: 0x0003C8E4
		public MeleeAttack(ICoreClientAPI api, MeleeAttackStats stats)
		{
			this._api = api;
			this.StopOnTerrainHit = stats2.StopOnTerrainHit;
			this.StopOnEntityHit = stats2.StopOnEntityHit;
			this.CollideWithTerrain = stats2.CollideWithTerrain;
			this.HitOnlyOneEntity = stats2.HitOnlyOneEntity;
			this.MaxReach = stats2.MaxReach;
			this.Impale = stats2.Impale;
			this.DamageTypes = Enumerable.ToArray<MeleeDamageType>(Enumerable.Select<MeleeDamageTypeJson, MeleeDamageType>(stats2.DamageTypes, (MeleeDamageTypeJson stats) => stats.ToDamageType()));
			this._lineColliders = new IHasLineCollider[this.DamageTypes.Length];
			for (int index = 0; index < this.DamageTypes.Length; index++)
			{
				this._lineColliders[index] = this.DamageTypes[index];
			}
			MeleeSystemClient clientMeleeSystem = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).ClientMeleeSystem;
			if (clientMeleeSystem == null)
			{
				throw new Exception();
			}
			this._meleeSystem = clientMeleeSystem;
			this._combatOverhaulSystem = this._api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0003E82C File Offset: 0x0003CA2C
		public void Start(IPlayer player)
		{
			long entityId = player.Entity.EntityId;
			HashSet<long> value;
			if (this._attackedEntities.TryGetValue(entityId, ref value))
			{
				value.Clear();
			}
			else
			{
				this._attackedEntities[entityId] = new HashSet<long>();
			}
			this._hitPlayer = false;
			LineSegmentCollider.ResetPreviousColliders(this._lineColliders);
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0003E880 File Offset: 0x0003CA80
		[Obsolete]
		public void Start(IPlayer player, ItemStackMeleeWeaponStats stats)
		{
			long entityId = player.Entity.EntityId;
			HashSet<long> value;
			if (this._attackedEntities.TryGetValue(entityId, ref value))
			{
				value.Clear();
			}
			else
			{
				this._attackedEntities[entityId] = new HashSet<long>();
			}
			this._hitPlayer = false;
			LineSegmentCollider.ResetPreviousColliders(this._lineColliders);
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0003E8D4 File Offset: 0x0003CAD4
		public void Attack(IPlayer player, ItemSlot slot, bool mainHand, [TupleElementNames(new string[]
		{
			"block",
			"point"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] out IEnumerable<ValueTuple<Block, Vector3d>> terrainCollisions, [TupleElementNames(new string[]
		{
			"entity",
			"point"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] out IEnumerable<ValueTuple<Entity, Vector3d>> entitiesCollisions)
		{
			this.Attack(player, slot, mainHand, out terrainCollisions, out entitiesCollisions, new ItemStackMeleeWeaponStats(), AttackDirection.Top);
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0003E8EC File Offset: 0x0003CAEC
		public bool Attack(IPlayer player, ItemSlot slot, bool mainHand, [TupleElementNames(new string[]
		{
			"block",
			"point"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] out IEnumerable<ValueTuple<Block, Vector3d>> terrainCollisions, [TupleElementNames(new string[]
		{
			"entity",
			"point"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] out IEnumerable<ValueTuple<Entity, Vector3d>> entitiesCollisions, ItemStackMeleeWeaponStats stats, AttackDirection attackDirection = AttackDirection.Top)
		{
			return this.Attack(player, slot, mainHand, out terrainCollisions, out entitiesCollisions, stats, attackDirection, true);
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0003E90C File Offset: 0x0003CB0C
		public bool Attack(IPlayer player, ItemSlot slot, bool mainHand, [TupleElementNames(new string[]
		{
			"block",
			"point"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] out IEnumerable<ValueTuple<Block, Vector3d>> terrainCollisions, [TupleElementNames(new string[]
		{
			"entity",
			"point"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] out IEnumerable<ValueTuple<Entity, Vector3d>> entitiesCollisions, ItemStackMeleeWeaponStats stats, AttackDirection attackDirection = AttackDirection.Top, bool collideWithTerrain = true)
		{
			terrainCollisions = Array.Empty<ValueTuple<Block, Vector3d>>();
			entitiesCollisions = Array.Empty<ValueTuple<Entity, Vector3d>>();
			this.PrepareColliders(player, slot, mainHand);
			double parameter = 1.0;
			if (collideWithTerrain)
			{
				this.TryCollideWithTerrain(out terrainCollisions, out parameter);
			}
			return this.TryAttackEntities(player, slot, out entitiesCollisions, mainHand, parameter, stats, attackDirection);
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0003E95B File Offset: 0x0003CB5B
		public void PrepareColliders(IPlayer player, ItemSlot slot, bool mainHand)
		{
			LineSegmentCollider.Transform(this._lineColliders, player.Entity, slot, this._api, mainHand);
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0003E977 File Offset: 0x0003CB77
		public bool TryCollideWithTerrain([TupleElementNames(new string[]
		{
			"block",
			"point"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] out IEnumerable<ValueTuple<Block, Vector3d>> terrainCollisions, out double parameter)
		{
			if (!this.CollideWithTerrain)
			{
				this._terrainCollisionsScratch.Clear();
				parameter = 1.0;
				terrainCollisions = this._terrainCollisionsScratch;
				return false;
			}
			bool result = this.CheckTerrainCollision(out parameter);
			terrainCollisions = this._terrainCollisionsScratch;
			return result;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0003E9B0 File Offset: 0x0003CBB0
		public bool TryAttackEntities(IPlayer player, ItemSlot slot, [TupleElementNames(new string[]
		{
			"entity",
			"point"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] out IEnumerable<ValueTuple<Entity, Vector3d>> entitiesCollisions, bool mainHand, double maximumParameter, ItemStackMeleeWeaponStats stats, AttackDirection attackDirection = AttackDirection.Top)
		{
			List<MeleeDamagePacket> damagePackets;
			List<MeleeCollisionPacket> collisions;
			entitiesCollisions = this.CollideWithEntities(player, slot, out damagePackets, out collisions, mainHand, maximumParameter, stats, attackDirection);
			this.LastDamagePackets = ((damagePackets.Count == 0) ? Array.Empty<MeleeDamagePacket>() : damagePackets.ToArray());
			if (this.LastDamagePackets.Length != 0)
			{
				this._meleeSystem.SendPackets(this.LastDamagePackets);
			}
			if (collisions.Count > 0)
			{
				this._meleeSystem.SendPackets(collisions);
			}
			return this.LastDamagePackets.Length != 0;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0003EA28 File Offset: 0x0003CC28
		public void RenderDebugColliders(IPlayer player, ItemSlot slot, bool rightHand = true)
		{
			LineSegmentCollider.Transform(this._lineColliders, player.Entity, slot, this._api, rightHand);
			MeleeDamageType[] damageTypes = this.DamageTypes;
			for (int i = 0; i < damageTypes.Length; i++)
			{
				damageTypes[i].InWorldCollider.Render(this._api, player.Entity, -1);
			}
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0003EA84 File Offset: 0x0003CC84
		public void MergeAttackedEntities(MeleeAttack attack)
		{
			foreach (long entityId in this._attackedEntities.Keys)
			{
				foreach (long id in this._attackedEntities[entityId])
				{
					attack._attackedEntities[entityId].Add(id);
				}
				foreach (long id2 in attack._attackedEntities[entityId])
				{
					this._attackedEntities[entityId].Add(id2);
				}
			}
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0003EB88 File Offset: 0x0003CD88
		public void AddAttackedEntities(MeleeAttack attack)
		{
			foreach (long entityId in this._attackedEntities.Keys)
			{
				foreach (long id in attack._attackedEntities[entityId])
				{
					this._attackedEntities[entityId].Add(id);
				}
			}
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0003EC30 File Offset: 0x0003CE30
		public void AddAttackedEntities(MeleeAttack attack, long currentEntityId)
		{
			foreach (long id in attack._attackedEntities[currentEntityId])
			{
				this._attackedEntities[currentEntityId].Add(id);
			}
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0003EC98 File Offset: 0x0003CE98
		private bool CheckTerrainCollision(out double parameter)
		{
			this._terrainCollisionsScratch.Clear();
			parameter = 1.0;
			MeleeDamageType[] damageTypes = this.DamageTypes;
			for (int i = 0; i < damageTypes.Length; i++)
			{
				ValueTuple<Block, Vector3d, double>? result = damageTypes[i].InWorldCollider.IntersectTerrain(this._api);
				if (result != null)
				{
					this._terrainCollisionsScratch.Add(new ValueTuple<Block, Vector3d>(result.Value.Item1, result.Value.Item2));
					if (result.Value.Item3 < parameter)
					{
						parameter = result.Value.Item3;
					}
				}
			}
			return this._terrainCollisionsScratch.Count > 0;
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0003ED44 File Offset: 0x0003CF44
		[return: TupleElementNames(new string[]
		{
			"entity",
			"point"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private IEnumerable<ValueTuple<Entity, Vector3d>> CollideWithEntities(IPlayer player, ItemSlot slot, out List<MeleeDamagePacket> packets, out List<MeleeCollisionPacket> collisions, bool mainHand, double maximumParameter, ItemStackMeleeWeaponStats stats, AttackDirection attackDirection)
		{
			long entityId = player.Entity.EntityId;
			IMountableSeat mountedOn2 = player.Entity.MountedOn;
			long? num;
			if (mountedOn2 == null)
			{
				num = default(long?);
			}
			else
			{
				Entity entity2 = mountedOn2.Entity;
				num = ((entity2 != null) ? new long?(entity2.EntityId) : default(long?));
			}
			long? num2 = num;
			long mountedOn = num2.GetValueOrDefault();
			HashSet<long> attackedEntities;
			if (!this._attackedEntities.TryGetValue(entityId, ref attackedEntities))
			{
				attackedEntities = new HashSet<long>();
				this._attackedEntities.Add(entityId, attackedEntities);
			}
			this._entityCollisionsScratch.Clear();
			this._damagePacketsScratch.Clear();
			this._collisionPacketsScratch.Clear();
			if (attackedEntities.Count > 0 && this.HitOnlyOneEntity)
			{
				packets = this._damagePacketsScratch;
				collisions = this._collisionPacketsScratch;
				return this._entityCollisionsScratch;
			}
			Entity[] entities = this._api.World.GetEntitiesAround(player.Entity.Pos.XYZ, this._combatOverhaulSystem.Settings.CollisionRadius + this.MaxReach, this._combatOverhaulSystem.Settings.CollisionRadius + this.MaxReach, null);
			foreach (MeleeDamageType damageType in this.DamageTypes)
			{
				bool attackedAtLeastOnce = false;
				foreach (Entity entity in entities)
				{
					string collider;
					Vector3d point;
					double parameter;
					ColliderTypes colliderType;
					if (entity.IsCreature && entity.Alive && entity.EntityId != entityId && entity.EntityId != mountedOn && (!(entity is EntityPlayer) || !this._hitPlayer) && damageType.Collide(entity, out collider, out point, out parameter, out colliderType) && maximumParameter >= parameter)
					{
						this._entityCollisionsScratch.Add(new ValueTuple<Entity, Vector3d>(entity, point));
						MeleeDamagePacket packet;
						if (!attackedEntities.Contains(entity.EntityId) && damageType.Attack(player.Entity, entity, point, collider, out packet, mainHand, colliderType, stats, attackDirection))
						{
							attackedAtLeastOnce = true;
							this.ApplyImpaleStats(packet, slot);
							this._damagePacketsScratch.Add(packet);
							attackedEntities.Add(entity.EntityId);
							if (this.StopOnEntityHit)
							{
								break;
							}
							if (entity is EntityPlayer)
							{
								this._hitPlayer = true;
							}
						}
					}
				}
				if (attackedAtLeastOnce && this.StopOnEntityHit)
				{
					break;
				}
			}
			packets = this._damagePacketsScratch;
			collisions = this._collisionPacketsScratch;
			return this._entityCollisionsScratch;
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0003EFB8 File Offset: 0x0003D1B8
		private void ApplyImpaleStats(MeleeDamagePacket packet, ItemSlot slot)
		{
			ImpaleAttackStats impale = this.Impale;
			if (impale == null || !impale.Enabled)
			{
				return;
			}
			if (!ImpaleAccess.IsDevImpaleSpear(slot))
			{
				return;
			}
			if (!packet.DamageType.Equals(4.ToString(), 5))
			{
				return;
			}
			packet.ImpaleEnabled = true;
			packet.ImpaleTargetFilter = this.Impale.TargetFilter;
			packet.ImpaleMaxTargetVolume = this.Impale.MaxTargetVolume;
			packet.ImpaleHoldOffset = this.Impale.HoldOffset;
			packet.ImpaleTargetOffset = this.Impale.TargetOffset;
			packet.ImpaleThrowVelocity = this.Impale.ThrowVelocity;
			packet.ImpaleThrowUpBias = this.Impale.ThrowUpBias;
		}

		// Token: 0x04000602 RID: 1538
		private readonly ICoreClientAPI _api;

		// Token: 0x04000603 RID: 1539
		private readonly IHasLineCollider[] _lineColliders;

		// Token: 0x04000604 RID: 1540
		private readonly Dictionary<long, HashSet<long>> _attackedEntities = new Dictionary<long, HashSet<long>>();

		// Token: 0x04000605 RID: 1541
		private readonly MeleeSystemClient _meleeSystem;

		// Token: 0x04000606 RID: 1542
		private readonly CombatOverhaulSystem _combatOverhaulSystem;

		// Token: 0x04000607 RID: 1543
		[TupleElementNames(new string[]
		{
			"block",
			"point"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private readonly List<ValueTuple<Block, Vector3d>> _terrainCollisionsScratch = new List<ValueTuple<Block, Vector3d>>();

		// Token: 0x04000608 RID: 1544
		[TupleElementNames(new string[]
		{
			"entity",
			"point"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private readonly List<ValueTuple<Entity, Vector3d>> _entityCollisionsScratch = new List<ValueTuple<Entity, Vector3d>>();

		// Token: 0x04000609 RID: 1545
		private readonly List<MeleeDamagePacket> _damagePacketsScratch = new List<MeleeDamagePacket>();

		// Token: 0x0400060A RID: 1546
		private readonly List<MeleeCollisionPacket> _collisionPacketsScratch = new List<MeleeCollisionPacket>();

		// Token: 0x0400060B RID: 1547
		private bool _hitPlayer;
	}
}
