using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Colliders;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Implementations;
using CombatOverhaul.Inputs;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F0 RID: 240
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeDamageType : IHasLineCollider
	{
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x0003E3AF File Offset: 0x0003C5AF
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x0003E3B7 File Offset: 0x0003C5B7
		public LineSegmentCollider RelativeCollider { get; set; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x0003E3C0 File Offset: 0x0003C5C0
		// (set) Token: 0x06000C87 RID: 3207 RVA: 0x0003E3C8 File Offset: 0x0003C5C8
		public LineSegmentCollider InWorldCollider { get; set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x0003E3D1 File Offset: 0x0003C5D1
		// (set) Token: 0x06000C89 RID: 3209 RVA: 0x0003E3D9 File Offset: 0x0003C5D9
		public LineSegmentCollider PreviousInWorldCollider { get; set; }

		// Token: 0x06000C8A RID: 3210 RVA: 0x0003E3E4 File Offset: 0x0003C5E4
		public MeleeDamageType(MeleeDamageTypeJson stats)
		{
			this.Damage = stats.Damage.Damage;
			this.DamageTypeData = new DamageData(Enum.Parse<EnumDamageType>(stats.Damage.DamageType), stats.Damage.Tier, stats.Damage.ArmorPiercingTier);
			this.Knockback = stats.Knockback;
			this.RelativeCollider = new LineSegmentCollider(stats.Collider);
			this.InWorldCollider = this.RelativeCollider;
			this.PreviousInWorldCollider = this.RelativeCollider;
			this.DurabilityDamage = stats.DurabilityDamage;
			this.StaggerTimeMs = stats.StaggerTimeMs;
			this.StaggerTier = stats.StaggerTier;
			this.Radius = stats.Radius;
			this.PushTier = stats.PushTier;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0003E4AC File Offset: 0x0003C6AC
		public bool TryAttack(IPlayer attacker, Entity target, out string collider, out Vector3d collisionPoint, out MeleeDamagePacket packet, bool mainHand, double maximumParameter, ItemStackMeleeWeaponStats stats)
		{
			double parameter;
			ColliderTypes colliderType;
			bool collided = this.Collide(target, out collider, out collisionPoint, out parameter, out colliderType);
			packet = new MeleeDamagePacket();
			return maximumParameter >= parameter && collided && this.Attack(attacker.Entity, target, collisionPoint, collider, out packet, mainHand, colliderType, stats, AttackDirection.Top);
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0003E4FC File Offset: 0x0003C6FC
		[return: TupleElementNames(new string[]
		{
			"collided",
			"attacked"
		})]
		[return: Nullable(0)]
		public ValueTuple<bool, bool> TryAttack(IPlayer attacker, Entity target, out string collider, out Vector3d collisionPoint, out MeleeDamagePacket packet, out MeleeCollisionPacket collisionPacket, bool mainHand, double maximumParameter, ItemStackMeleeWeaponStats stats)
		{
			double parameter;
			ColliderTypes colliderType;
			bool collided = this.Collide(target, out collider, out collisionPoint, out parameter, out colliderType);
			packet = new MeleeDamagePacket();
			collisionPacket = new MeleeCollisionPacket
			{
				PushTier = this.PushTier,
				Collider = collider,
				ColliderType = (int)colliderType,
				AttackerEntityId = attacker.Entity.EntityId,
				TargetEntityId = target.EntityId,
				MainHand = mainHand
			};
			if (maximumParameter < parameter)
			{
				return new ValueTuple<bool, bool>(false, false);
			}
			if (!collided)
			{
				return new ValueTuple<bool, bool>(false, false);
			}
			bool received = this.Attack(attacker.Entity, target, collisionPoint, collider, out packet, mainHand, colliderType, stats, AttackDirection.Top);
			return new ValueTuple<bool, bool>(true, received);
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0003E5A8 File Offset: 0x0003C7A8
		public bool Collide(IPlayer attacker, Entity target, out string collider, out Vector3d collisionPoint, out MeleeCollisionPacket packet, bool mainHand, double maximumParameter, ItemStackMeleeWeaponStats stats, out ColliderTypes colliderType)
		{
			double parameter;
			bool collided = this.Collide(target, out collider, out collisionPoint, out parameter, out colliderType);
			packet = new MeleeCollisionPacket
			{
				PushTier = this.PushTier,
				Collider = collider,
				ColliderType = (int)colliderType,
				AttackerEntityId = attacker.Entity.EntityId,
				TargetEntityId = target.EntityId,
				MainHand = mainHand
			};
			return maximumParameter >= parameter && collided;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0003E618 File Offset: 0x0003C818
		public bool Collide(Entity target, out string collider, out Vector3d collisionPoint, out double parameter, out ColliderTypes colliderType)
		{
			parameter = 1.0;
			colliderType = ColliderTypes.Torso;
			collisionPoint = Vector3.Zero;
			CollidersEntityBehavior colliders = target.GetBehavior<CollidersEntityBehavior>();
			if (colliders != null && colliders.HasOBBCollider)
			{
				bool flag = colliders.Collide(this.InWorldCollider.Position, this.PreviousInWorldCollider.Position, this.InWorldCollider.Direction, this.PreviousInWorldCollider.Direction, this.Radius, out collider, out parameter, out collisionPoint);
				if (flag)
				{
					colliders.CollidersTypes.TryGetValue(collider, ref colliderType);
				}
				return flag;
			}
			collider = "";
			Cuboidf collisionBox = MeleeDamageType.GetCollisionBox(target);
			if (!this.InWorldCollider.RoughIntersect(collisionBox))
			{
				return false;
			}
			Vector3d? point = this.InWorldCollider.IntersectCuboid(collisionBox, out parameter);
			if (point == null)
			{
				return false;
			}
			collisionPoint = point.Value;
			return true;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0003E6F4 File Offset: 0x0003C8F4
		public bool Attack(Entity attacker, Entity target, Vector3d position, string collider, out MeleeDamagePacket packet, bool mainHand, ColliderTypes colliderType, ItemStackMeleeWeaponStats stats, AttackDirection attackDirection = AttackDirection.Top)
		{
			packet = new MeleeDamagePacket();
			ICoreServerAPI serverApi = attacker.Api as ICoreServerAPI;
			if (serverApi != null)
			{
				EntityPlayer playerAttacker = attacker as EntityPlayer;
				if (playerAttacker != null)
				{
					if (target is EntityPlayer && (!serverApi.Server.Config.AllowPvP || !playerAttacker.Player.HasPrivilege("attackplayers")))
					{
						return false;
					}
					if (!(target is EntityPlayer) && !playerAttacker.Player.HasPrivilege("attackcreatures"))
					{
						return false;
					}
				}
			}
			float damage = this.Damage * attacker.Stats.GetBlended("meleeWeaponsDamage");
			JsonObject attributes = target.Properties.Attributes;
			if (attributes != null && attributes["isMechanical"].AsBool(false))
			{
				damage *= attacker.Stats.GetBlended("mechanicalsDamage");
			}
			damage += stats.DamageBonus;
			damage *= stats.DamageMultiplier;
			string damageTierStat = "meleeDamageTierBonus" + this.DamageTypeData.DamageType.ToString();
			float statValue = attacker.Stats.GetBlended(damageTierStat) - 1f;
			int damageTier = this.DamageTypeData.Tier + stats.DamageTierBonus + (int)statValue;
			damageTier = GameMath.Max(damageTier, 0);
			DamageData damageTypeData = this.ResolveDamageTypeData(attacker, mainHand, attackDirection, damageTier);
			bool received = target.ReceiveDamage(new DirectionalTypedDamageSource
			{
				Source = ((attacker is EntityPlayer) ? 1 : 8),
				SourceEntity = attacker,
				CauseEntity = attacker,
				DamageTypeData = damageTypeData,
				Position = position,
				Collider = collider,
				KnockbackStrength = this.Knockback * stats.KnockbackMultiplier,
				IgnoreInvFrames = true,
				Type = this.DamageTypeData.DamageType
			}, damage) || this.Damage > 0f;
			packet = new MeleeDamagePacket
			{
				DamageType = damageTypeData.DamageType.ToString(),
				Tier = damageTypeData.Tier,
				ArmorPiercingTier = damageTypeData.ArmorPiercingTier + stats.ArmorPiercingBonus,
				Damage = damage,
				Knockback = this.Knockback * stats.KnockbackMultiplier,
				Position = new double[]
				{
					position.X,
					position.Y,
					position.Z
				},
				Collider = collider,
				ColliderType = (int)colliderType,
				AttackerEntityId = attacker.EntityId,
				TargetEntityId = target.EntityId,
				DurabilityDamage = this.DurabilityDamage,
				MainHand = mainHand,
				StaggerTimeMs = this.StaggerTimeMs,
				StaggerTier = this.StaggerTier,
				AttackDirection = (int)attackDirection
			};
			return received;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0003E9B8 File Offset: 0x0003CBB8
		public MeleeDamagePacket CreateDamagePacket(Entity attacker, Entity target, Vector3d position, string collider, bool mainHand, ColliderTypes colliderType, ItemStackMeleeWeaponStats stats, float damageMultiplier = 1f, float knockbackMultiplier = 1f, int durabilityDamageOverride = -1, AttackDirection attackDirection = AttackDirection.Top)
		{
			float damage = this.Damage * attacker.Stats.GetBlended("meleeWeaponsDamage");
			JsonObject attributes = target.Properties.Attributes;
			if (attributes != null && attributes["isMechanical"].AsBool(false))
			{
				damage *= attacker.Stats.GetBlended("mechanicalsDamage");
			}
			damage += stats.DamageBonus;
			damage *= stats.DamageMultiplier;
			damage *= damageMultiplier;
			string damageTierStat = "meleeDamageTierBonus" + this.DamageTypeData.DamageType.ToString();
			float statValue = attacker.Stats.GetBlended(damageTierStat) - 1f;
			int damageTier = this.DamageTypeData.Tier + stats.DamageTierBonus + (int)statValue;
			damageTier = GameMath.Max(damageTier, 0);
			DamageData damageTypeData = this.ResolveDamageTypeData(attacker, mainHand, attackDirection, damageTier);
			return new MeleeDamagePacket
			{
				DamageType = damageTypeData.DamageType.ToString(),
				Tier = damageTypeData.Tier,
				ArmorPiercingTier = damageTypeData.ArmorPiercingTier + stats.ArmorPiercingBonus,
				Damage = damage,
				Knockback = this.Knockback * stats.KnockbackMultiplier * knockbackMultiplier,
				Position = new double[]
				{
					position.X,
					position.Y,
					position.Z
				},
				Collider = collider,
				ColliderType = (int)colliderType,
				AttackerEntityId = attacker.EntityId,
				TargetEntityId = target.EntityId,
				DurabilityDamage = ((durabilityDamageOverride >= 0) ? durabilityDamageOverride : this.DurabilityDamage),
				MainHand = mainHand,
				StaggerTimeMs = this.StaggerTimeMs,
				StaggerTier = this.StaggerTier,
				AttackDirection = (int)attackDirection
			};
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0003EB94 File Offset: 0x0003CD94
		private DamageData ResolveDamageTypeData(Entity attacker, bool mainHand, AttackDirection attackDirection, int damageTier)
		{
			EnumDamageType effectiveDamageType = this.DamageTypeData.DamageType;
			if (effectiveDamageType == 4 && MeleeDamageType.IsSingleDaggerTopOrSideAttack(attacker, mainHand, attackDirection))
			{
				effectiveDamageType = 3;
			}
			return new DamageData(effectiveDamageType, damageTier, this.DamageTypeData.ArmorPiercingTier);
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0003EBD0 File Offset: 0x0003CDD0
		private static bool IsSingleDaggerTopOrSideAttack(Entity attacker, bool mainHand, AttackDirection attackDirection)
		{
			EntityAgent agent = attacker as EntityAgent;
			if (agent == null)
			{
				return false;
			}
			ItemSlot slot = mainHand ? agent.RightHandItemSlot : agent.LeftHandItemSlot;
			ItemSlot otherSlot = mainHand ? agent.LeftHandItemSlot : agent.RightHandItemSlot;
			return MeleeDamageType.IsDagger(slot) && !MeleeDamageType.IsDagger(otherSlot) && (attackDirection == AttackDirection.Top || attackDirection == AttackDirection.Left || attackDirection == AttackDirection.Right || attackDirection == AttackDirection.TopLeft || attackDirection == AttackDirection.TopRight);
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0003EC34 File Offset: 0x0003CE34
		private static bool IsDagger(ItemSlot slot)
		{
			return CollectibleClassifier.IsDagger(slot);
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0003EC3C File Offset: 0x0003CE3C
		private static Cuboidf GetCollisionBox(Entity entity)
		{
			Cuboidf source = entity.CollisionBox;
			Cuboidf cuboidf;
			if ((cuboidf = MeleeDamageType._collisionBoxScratch) == null)
			{
				cuboidf = (MeleeDamageType._collisionBoxScratch = new Cuboidf());
			}
			EntityPos position = entity.Pos;
			Cuboidf cuboidf2 = cuboidf;
			cuboidf2.X1 = source.X1 + (float)position.X;
			cuboidf2.Y1 = source.Y1 + (float)position.Y;
			cuboidf2.Z1 = source.Z1 + (float)position.Z;
			cuboidf2.X2 = source.X2 + (float)position.X;
			cuboidf2.Y2 = source.Y2 + (float)position.Y;
			cuboidf2.Z2 = source.Z2 + (float)position.Z;
			return cuboidf2;
		}

		// Token: 0x04000611 RID: 1553
		public readonly float Damage;

		// Token: 0x04000612 RID: 1554
		public readonly DamageData DamageTypeData;

		// Token: 0x04000613 RID: 1555
		public readonly float Knockback;

		// Token: 0x04000614 RID: 1556
		public readonly int DurabilityDamage;

		// Token: 0x04000615 RID: 1557
		public readonly int StaggerTimeMs;

		// Token: 0x04000616 RID: 1558
		public readonly int StaggerTier;

		// Token: 0x04000617 RID: 1559
		public readonly float Radius;

		// Token: 0x04000618 RID: 1560
		public readonly int PushTier;

		// Token: 0x04000619 RID: 1561
		public const string DamageTierPlayerStatPrefix = "meleeDamageTierBonus";

		// Token: 0x0400061A RID: 1562
		[Nullable(2)]
		[ThreadStatic]
		private static Cuboidf _collisionBoxScratch;
	}
}
