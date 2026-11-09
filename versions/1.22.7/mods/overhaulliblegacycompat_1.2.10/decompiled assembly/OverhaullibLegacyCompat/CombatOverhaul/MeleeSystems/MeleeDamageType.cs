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
	// Token: 0x020000FB RID: 251
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeDamageType : IHasLineCollider
	{
		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x0003F39F File Offset: 0x0003D59F
		// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x0003F3A7 File Offset: 0x0003D5A7
		public LineSegmentCollider RelativeCollider { get; set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x0003F3B0 File Offset: 0x0003D5B0
		// (set) Token: 0x06000CE3 RID: 3299 RVA: 0x0003F3B8 File Offset: 0x0003D5B8
		public LineSegmentCollider InWorldCollider { get; set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0003F3C1 File Offset: 0x0003D5C1
		// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x0003F3C9 File Offset: 0x0003D5C9
		public LineSegmentCollider PreviousInWorldCollider { get; set; }

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0003F3D4 File Offset: 0x0003D5D4
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

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0003F49C File Offset: 0x0003D69C
		public bool TryAttack(IPlayer attacker, Entity target, out string collider, out Vector3d collisionPoint, out MeleeDamagePacket packet, bool mainHand, double maximumParameter, ItemStackMeleeWeaponStats stats)
		{
			double parameter;
			ColliderTypes colliderType;
			bool collided = this.Collide(target, out collider, out collisionPoint, out parameter, out colliderType);
			packet = new MeleeDamagePacket();
			return maximumParameter >= parameter && collided && this.Attack(attacker.Entity, target, collisionPoint, collider, out packet, mainHand, colliderType, stats, AttackDirection.Top);
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0003F4EC File Offset: 0x0003D6EC
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

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0003F598 File Offset: 0x0003D798
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

		// Token: 0x06000CEA RID: 3306 RVA: 0x0003F608 File Offset: 0x0003D808
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

		// Token: 0x06000CEB RID: 3307 RVA: 0x0003F6E4 File Offset: 0x0003D8E4
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

		// Token: 0x06000CEC RID: 3308 RVA: 0x0003F9A8 File Offset: 0x0003DBA8
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

		// Token: 0x06000CED RID: 3309 RVA: 0x0003FB84 File Offset: 0x0003DD84
		private DamageData ResolveDamageTypeData(Entity attacker, bool mainHand, AttackDirection attackDirection, int damageTier)
		{
			EnumDamageType effectiveDamageType = this.DamageTypeData.DamageType;
			if (effectiveDamageType == 4 && MeleeDamageType.IsSingleDaggerTopOrSideAttack(attacker, mainHand, attackDirection))
			{
				effectiveDamageType = 3;
			}
			return new DamageData(effectiveDamageType, damageTier, this.DamageTypeData.ArmorPiercingTier);
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0003FBC0 File Offset: 0x0003DDC0
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

		// Token: 0x06000CEF RID: 3311 RVA: 0x0003FC24 File Offset: 0x0003DE24
		private static bool IsDagger(ItemSlot slot)
		{
			return CollectibleClassifier.IsDagger(slot);
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0003FC2C File Offset: 0x0003DE2C
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

		// Token: 0x04000633 RID: 1587
		public readonly float Damage;

		// Token: 0x04000634 RID: 1588
		public readonly DamageData DamageTypeData;

		// Token: 0x04000635 RID: 1589
		public readonly float Knockback;

		// Token: 0x04000636 RID: 1590
		public readonly int DurabilityDamage;

		// Token: 0x04000637 RID: 1591
		public readonly int StaggerTimeMs;

		// Token: 0x04000638 RID: 1592
		public readonly int StaggerTier;

		// Token: 0x04000639 RID: 1593
		public readonly float Radius;

		// Token: 0x0400063A RID: 1594
		public readonly int PushTier;

		// Token: 0x0400063B RID: 1595
		public const string DamageTierPlayerStatPrefix = "meleeDamageTierBonus";

		// Token: 0x0400063C RID: 1596
		[Nullable(2)]
		[ThreadStatic]
		private static Cuboidf _collisionBoxScratch;
	}
}
