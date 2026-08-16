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
	// Token: 0x020000EC RID: 236
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeleeAttack
	{
		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x0003D674 File Offset: 0x0003B874
		public MeleeDamageType[] DamageTypes { get; }

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000C1A RID: 3098 RVA: 0x0003D67C File Offset: 0x0003B87C
		// (set) Token: 0x06000C1B RID: 3099 RVA: 0x0003D684 File Offset: 0x0003B884
		public MeleeDamagePacket[] LastDamagePackets { get; private set; } = Array.Empty<MeleeDamagePacket>();

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x0003D68D File Offset: 0x0003B88D
		// (set) Token: 0x06000C1D RID: 3101 RVA: 0x0003D695 File Offset: 0x0003B895
		public bool StopOnTerrainHit { get; set; }

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x0003D69E File Offset: 0x0003B89E
		// (set) Token: 0x06000C1F RID: 3103 RVA: 0x0003D6A6 File Offset: 0x0003B8A6
		public bool StopOnEntityHit { get; set; }

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x0003D6AF File Offset: 0x0003B8AF
		// (set) Token: 0x06000C21 RID: 3105 RVA: 0x0003D6B7 File Offset: 0x0003B8B7
		public bool CollideWithTerrain { get; set; }

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x0003D6C0 File Offset: 0x0003B8C0
		// (set) Token: 0x06000C23 RID: 3107 RVA: 0x0003D6C8 File Offset: 0x0003B8C8
		public bool HitOnlyOneEntity { get; set; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x0003D6D1 File Offset: 0x0003B8D1
		// (set) Token: 0x06000C25 RID: 3109 RVA: 0x0003D6D9 File Offset: 0x0003B8D9
		public float MaxReach { get; set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x0003D6E2 File Offset: 0x0003B8E2
		// (set) Token: 0x06000C27 RID: 3111 RVA: 0x0003D6EA File Offset: 0x0003B8EA
		[Nullable(2)]
		public ImpaleAttackStats Impale { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x06000C28 RID: 3112 RVA: 0x0003D6F4 File Offset: 0x0003B8F4
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

		// Token: 0x06000C29 RID: 3113 RVA: 0x0003D83C File Offset: 0x0003BA3C
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

		// Token: 0x06000C2A RID: 3114 RVA: 0x0003D890 File Offset: 0x0003BA90
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

		// Token: 0x06000C2B RID: 3115 RVA: 0x0003D8E4 File Offset: 0x0003BAE4
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

		// Token: 0x06000C2C RID: 3116 RVA: 0x0003D8FC File Offset: 0x0003BAFC
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

		// Token: 0x06000C2D RID: 3117 RVA: 0x0003D91C File Offset: 0x0003BB1C
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

		// Token: 0x06000C2E RID: 3118 RVA: 0x0003D96B File Offset: 0x0003BB6B
		public void PrepareColliders(IPlayer player, ItemSlot slot, bool mainHand)
		{
			LineSegmentCollider.Transform(this._lineColliders, player.Entity, slot, this._api, mainHand);
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x0003D987 File Offset: 0x0003BB87
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

		// Token: 0x06000C30 RID: 3120 RVA: 0x0003D9C0 File Offset: 0x0003BBC0
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

		// Token: 0x06000C31 RID: 3121 RVA: 0x0003DA38 File Offset: 0x0003BC38
		public void RenderDebugColliders(IPlayer player, ItemSlot slot, bool rightHand = true)
		{
			LineSegmentCollider.Transform(this._lineColliders, player.Entity, slot, this._api, rightHand);
			MeleeDamageType[] damageTypes = this.DamageTypes;
			for (int i = 0; i < damageTypes.Length; i++)
			{
				damageTypes[i].InWorldCollider.Render(this._api, player.Entity, -1);
			}
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x0003DA94 File Offset: 0x0003BC94
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

		// Token: 0x06000C33 RID: 3123 RVA: 0x0003DB98 File Offset: 0x0003BD98
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

		// Token: 0x06000C34 RID: 3124 RVA: 0x0003DC40 File Offset: 0x0003BE40
		public void AddAttackedEntities(MeleeAttack attack, long currentEntityId)
		{
			foreach (long id in attack._attackedEntities[currentEntityId])
			{
				this._attackedEntities[currentEntityId].Add(id);
			}
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x0003DCA8 File Offset: 0x0003BEA8
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

		// Token: 0x06000C36 RID: 3126 RVA: 0x0003DD54 File Offset: 0x0003BF54
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

		// Token: 0x06000C37 RID: 3127 RVA: 0x0003DFC8 File Offset: 0x0003C1C8
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

		// Token: 0x040005E0 RID: 1504
		private readonly ICoreClientAPI _api;

		// Token: 0x040005E1 RID: 1505
		private readonly IHasLineCollider[] _lineColliders;

		// Token: 0x040005E2 RID: 1506
		private readonly Dictionary<long, HashSet<long>> _attackedEntities = new Dictionary<long, HashSet<long>>();

		// Token: 0x040005E3 RID: 1507
		private readonly MeleeSystemClient _meleeSystem;

		// Token: 0x040005E4 RID: 1508
		private readonly CombatOverhaulSystem _combatOverhaulSystem;

		// Token: 0x040005E5 RID: 1509
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

		// Token: 0x040005E6 RID: 1510
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

		// Token: 0x040005E7 RID: 1511
		private readonly List<MeleeDamagePacket> _damagePacketsScratch = new List<MeleeDamagePacket>();

		// Token: 0x040005E8 RID: 1512
		private readonly List<MeleeCollisionPacket> _collisionPacketsScratch = new List<MeleeCollisionPacket>();

		// Token: 0x040005E9 RID: 1513
		private bool _hitPlayer;
	}
}
