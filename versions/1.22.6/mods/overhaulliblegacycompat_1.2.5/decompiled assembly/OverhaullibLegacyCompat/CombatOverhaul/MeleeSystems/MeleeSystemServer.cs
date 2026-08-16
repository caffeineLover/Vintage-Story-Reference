using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Colliders;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Implementations;
using CombatOverhaul.Integration;
using CombatOverhaul.Utils;
using CombatOverhaul.WeaponBuffs;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000FB RID: 251
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeleeSystemServer : MeleeSystem
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000CB3 RID: 3251 RVA: 0x0003EFD8 File Offset: 0x0003D1D8
		// (remove) Token: 0x06000CB4 RID: 3252 RVA: 0x0003F010 File Offset: 0x0003D210
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event MeleeSystemServer.MeleeDamageDelegate OnDealMeleeDamage;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000CB5 RID: 3253 RVA: 0x0003F048 File Offset: 0x0003D248
		// (remove) Token: 0x06000CB6 RID: 3254 RVA: 0x0003F080 File Offset: 0x0003D280
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event MeleeSystemServer.MeleeDamageResolvedDelegate OnMeleeDamageResolved;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000CB7 RID: 3255 RVA: 0x0003F0B8 File Offset: 0x0003D2B8
		// (remove) Token: 0x06000CB8 RID: 3256 RVA: 0x0003F0F0 File Offset: 0x0003D2F0
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event MeleeSystemServer.MeleeAttackDelegate OnMeleeAttackStart;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000CB9 RID: 3257 RVA: 0x0003F128 File Offset: 0x0003D328
		// (remove) Token: 0x06000CBA RID: 3258 RVA: 0x0003F160 File Offset: 0x0003D360
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event MeleeSystemServer.MeleeAttackDelegate OnMeleeAttackEnd;

		// Token: 0x06000CBB RID: 3259 RVA: 0x0003F198 File Offset: 0x0003D398
		public MeleeSystemServer(ICoreServerAPI api)
		{
			this._api = api;
			api.Network.RegisterChannel("CombatOverhaul:damage-packets").RegisterMessageType<MeleeAttackPacket>().RegisterMessageType<MeleePushPacket>().RegisterMessageType<MeleeAttackStatusPacket>().SetMessageHandler<MeleeAttackPacket>(new NetworkClientMessageHandler<MeleeAttackPacket>(this.HandlePacket)).SetMessageHandler<MeleePushPacket>(new NetworkClientMessageHandler<MeleePushPacket>(this.HandlePacket)).SetMessageHandler<MeleeAttackStatusPacket>(new NetworkClientMessageHandler<MeleeAttackStatusPacket>(this.HandlePacket));
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0003F208 File Offset: 0x0003D408
		private void HandlePacket(IServerPlayer player, MeleeAttackPacket packet)
		{
			if (packet.MeleeAttackDamagePackets == null)
			{
				return;
			}
			foreach (MeleeDamagePacket damagePacket in packet.MeleeAttackDamagePackets)
			{
				if (damagePacket != null)
				{
					this.Attack(player, damagePacket);
				}
			}
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0003F244 File Offset: 0x0003D444
		private void HandlePacket(IServerPlayer player, MeleePushPacket packet)
		{
			foreach (MeleeCollisionPacket collisionPacket in packet.MeleeAttackDamagePackets)
			{
				this.Push(collisionPacket);
			}
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0003F274 File Offset: 0x0003D474
		private void HandlePacket(IServerPlayer player, MeleeAttackStatusPacket packet)
		{
			ItemSlot weaponSlot = packet.MainHand ? player.Entity.ActiveHandItemSlot : player.Entity.LeftHandItemSlot;
			MeleeAttackStatus status = packet.Status;
			if (status != MeleeAttackStatus.Start)
			{
				if (status != MeleeAttackStatus.End)
				{
					return;
				}
				MeleeSystemServer.MeleeAttackDelegate onMeleeAttackEnd = this.OnMeleeAttackEnd;
				if (onMeleeAttackEnd == null)
				{
					return;
				}
				onMeleeAttackEnd(player.Entity, weaponSlot);
				return;
			}
			else
			{
				MeleeSystemServer.MeleeAttackDelegate onMeleeAttackStart = this.OnMeleeAttackStart;
				if (onMeleeAttackStart == null)
				{
					return;
				}
				onMeleeAttackStart(player.Entity, weaponSlot);
				return;
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0003F2E4 File Offset: 0x0003D4E4
		private void Attack(IServerPlayer player, MeleeDamagePacket packet)
		{
			Entity target;
			ItemSlot slot;
			EnumDamageType damageType;
			Vector3d position;
			if (!this.TryValidateAttackPacket(player, packet, out target, out slot, out damageType, out position))
			{
				return;
			}
			Entity attacker = player.Entity;
			string targetName = target.GetName();
			if (damageType != 6)
			{
				if (target is EntityPlayer && (!this._api.Server.Config.AllowPvP || !player.HasPrivilege("attackplayers")))
				{
					return;
				}
				if (target is EntityAgent && !player.HasPrivilege("attackcreatures"))
				{
					return;
				}
			}
			DirectionalTypedDamageSource damageSource = new DirectionalTypedDamageSource
			{
				Source = ((attacker is EntityPlayer) ? 1 : 8),
				SourceEntity = attacker,
				CauseEntity = attacker,
				DamageTypeData = new DamageData(damageType, packet.Tier, packet.ArmorPiercingTier),
				Position = position,
				Collider = packet.Collider,
				KnockbackStrength = packet.Knockback,
				DamageTier = packet.Tier,
				Type = damageType,
				Weapon = ((slot != null) ? slot.Itemstack : null),
				IgnoreInvFrames = true
			};
			bool damageReceived = this.DealDamage(target, damageSource, slot, packet.Damage);
			if (damageReceived)
			{
				WeaponBuffSystem weaponBuffSystem = WeaponBuffSystem.Current;
				if (weaponBuffSystem != null)
				{
					weaponBuffSystem.Consume(slot, WeaponBuffConsumptionTrigger.MeleeHit);
				}
			}
			ImpaleSystemServer serverImpaleSystem = this._api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).ServerImpaleSystem;
			if (serverImpaleSystem != null)
			{
				serverImpaleSystem.TryAttach(packet, attacker, target, slot, damageReceived);
			}
			if (packet.StaggerTimeMs > 0)
			{
				StaggerBehavior behavior = target.GetBehavior<StaggerBehavior>();
				if (behavior != null)
				{
					behavior.TriggerStagger(TimeSpan.FromMilliseconds((long)packet.StaggerTimeMs), packet.StaggerTier);
				}
			}
			this.DealDurabilityDamage(slot, packet, attacker);
			this.PrintLog(attacker, damageReceived, target, packet, targetName);
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0003F480 File Offset: 0x0003D680
		private bool TryValidateAttackPacket(IServerPlayer player, MeleeDamagePacket packet, out Entity target, [Nullable(2)] out ItemSlot slot, out EnumDamageType damageType, out Vector3d position)
		{
			target = null;
			slot = null;
			damageType = 0;
			position = default(Vector3d);
			if (packet.AttackerEntityId != player.Entity.EntityId)
			{
				this.LogRejectedAttackPacket(player, packet, "wrong-attacker", "");
				return false;
			}
			Entity packetTarget = this._api.World.GetEntityById(packet.TargetEntityId);
			if (packetTarget == null || !packetTarget.Alive)
			{
				this.LogRejectedAttackPacket(player, packet, "invalid-target", "");
				return false;
			}
			if (!Enum.TryParse<EnumDamageType>(packet.DamageType, ref damageType))
			{
				this.LogRejectedAttackPacket(player, packet, "invalid-damage-type", "");
				return false;
			}
			if (!MeleeSystemServer.TryGetPacketPosition(packet, out position))
			{
				this.LogRejectedAttackPacket(player, packet, "invalid-position", "");
				return false;
			}
			slot = MeleeSystemServer.GetWeaponSlot(player.Entity, packet.MainHand);
			ItemSlot itemSlot = slot;
			if (((itemSlot != null) ? itemSlot.Itemstack : null) == null)
			{
				this.LogRejectedAttackPacket(player, packet, "missing-weapon", "");
				return false;
			}
			MeleeSystemServer.MeleeAttackLimits limits;
			if (!this.TryGetAttackLimits(player.Entity, packetTarget, slot, out limits))
			{
				this.LogRejectedAttackPacket(player, packet, "missing-weapon-stats", "");
				return false;
			}
			if (!this.IsTargetWithinReach(player.Entity, packetTarget, limits.MaxReach))
			{
				this.LogRejectedAttackPacket(player, packet, "out-of-range", "");
				return false;
			}
			if (!limits.DamageTypes.Contains(damageType))
			{
				this.LogRejectedAttackPacket(player, packet, "unconfigured-damage-type", "");
				return false;
			}
			if (this._api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).Settings.MeleeWeaponStopOnTerrainHit && limits.RequiresTerrainClearance(damageType) && this.IsTerrainObstructingHit(player.Entity, position))
			{
				this.LogRejectedAttackPacket(player, packet, "terrain-obstructed", "");
				return false;
			}
			float allowedMaxDamage = MeleeSystemServer.GetAllowedMaxDamage(limits.MaxDamage, slot.Itemstack, packetTarget);
			if (packet.Damage < 0f || !float.IsFinite(packet.Damage) || packet.Damage > allowedMaxDamage)
			{
				string reason = "damage-too-high";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(26, 2);
				defaultInterpolatedStringHandler.AppendLiteral("maxDamage=");
				defaultInterpolatedStringHandler.AppendFormatted<float>(limits.MaxDamage);
				defaultInterpolatedStringHandler.AppendLiteral(", allowedDamage=");
				defaultInterpolatedStringHandler.AppendFormatted<float>(allowedMaxDamage);
				this.LogRejectedAttackPacket(player, packet, reason, defaultInterpolatedStringHandler.ToStringAndClear());
				return false;
			}
			if (packet.Tier < 0 || packet.Tier > limits.MaxTier)
			{
				this.LogRejectedAttackPacket(player, packet, "tier-too-high", "");
				return false;
			}
			if (packet.ArmorPiercingTier < 0 || packet.ArmorPiercingTier > limits.MaxArmorPiercingTier)
			{
				this.LogRejectedAttackPacket(player, packet, "armor-piercing-too-high", "");
				return false;
			}
			if (packet.Knockback < limits.MinKnockback - 0.01f || packet.Knockback > limits.MaxKnockback + 0.01f)
			{
				this.LogRejectedAttackPacket(player, packet, "knockback-out-of-range", "");
				return false;
			}
			if (packet.DurabilityDamage < 0 || packet.DurabilityDamage > limits.MaxDurabilityDamage)
			{
				this.LogRejectedAttackPacket(player, packet, "durability-too-high", "");
				return false;
			}
			if (packet.StaggerTimeMs < 0 || packet.StaggerTimeMs > limits.MaxStaggerTimeMs || packet.StaggerTier < 0 || packet.StaggerTier > limits.MaxStaggerTier)
			{
				this.LogRejectedAttackPacket(player, packet, "stagger-too-high", "");
				return false;
			}
			target = packetTarget;
			return true;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0003F7C8 File Offset: 0x0003D9C8
		private static bool TryGetPacketPosition(MeleeDamagePacket packet, out Vector3d position)
		{
			position = default(Vector3d);
			if (packet.Position == null || packet.Position.Length < 3)
			{
				return false;
			}
			double x = packet.Position[0];
			double y = packet.Position[1];
			double z = packet.Position[2];
			if (!double.IsFinite(x) || !double.IsFinite(y) || !double.IsFinite(z))
			{
				return false;
			}
			position = new Vector3d(x, y, z);
			return true;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0003F835 File Offset: 0x0003DA35
		[return: Nullable(2)]
		private static ItemSlot GetWeaponSlot(EntityPlayer player, bool mainHand)
		{
			if (!mainHand)
			{
				return player.LeftHandItemSlot;
			}
			ItemSlot activeHandItemSlot = player.ActiveHandItemSlot;
			if (((activeHandItemSlot != null) ? activeHandItemSlot.Itemstack : null) == null)
			{
				return player.RightHandItemSlot;
			}
			return player.ActiveHandItemSlot;
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0003F864 File Offset: 0x0003DA64
		private bool TryGetAttackLimits(EntityPlayer attacker, Entity target, ItemSlot slot, out MeleeSystemServer.MeleeAttackLimits limits)
		{
			limits = default(MeleeSystemServer.MeleeAttackLimits);
			if (slot.Itemstack == null)
			{
				return false;
			}
			ItemStackMeleeWeaponStats stackStats = ItemStackMeleeWeaponStats.FromItemStack(slot.Itemstack);
			MeleeSystemServer.MeleeAttackLimitsBuilder builder = new MeleeSystemServer.MeleeAttackLimitsBuilder(attacker, target, slot, stackStats);
			try
			{
				JsonObject itemAttributes = slot.Itemstack.ItemAttributes;
				MeleeWeaponStats meleeStats = (itemAttributes != null) ? itemAttributes.AsObject<MeleeWeaponStats>(null) : null;
				if (meleeStats != null)
				{
					MeleeSystemServer.AddMeleeWeaponStats(builder, meleeStats);
				}
			}
			catch
			{
			}
			try
			{
				JsonObject itemAttributes2 = slot.Itemstack.ItemAttributes;
				StanceBasedMeleeWeaponStats stanceStats = (itemAttributes2 != null) ? itemAttributes2.AsObject<StanceBasedMeleeWeaponStats>(null) : null;
				if (stanceStats != null)
				{
					MeleeSystemServer.AddStanceBasedMeleeWeaponStats(builder, stanceStats);
				}
			}
			catch
			{
			}
			try
			{
				JsonObject itemAttributes3 = slot.Itemstack.ItemAttributes;
				MeleeWeaponModeCollectionStats modeStats = (itemAttributes3 != null) ? itemAttributes3.AsObject<MeleeWeaponModeCollectionStats>(null) : null;
				if (modeStats != null)
				{
					foreach (MeleeWeaponModeStats mode in modeStats.Modes.Values)
					{
						MeleeSystemServer.AddMeleeWeaponStats(builder, mode);
					}
				}
			}
			catch
			{
			}
			MeleeSystemServer.AddServerMeleeAttacks(attacker, target, slot, builder);
			return builder.TryBuild(out limits);
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0003F994 File Offset: 0x0003DB94
		private static void AddServerMeleeAttacks(EntityPlayer attacker, Entity target, ItemSlot slot, MeleeSystemServer.MeleeAttackLimitsBuilder builder)
		{
			ItemStack itemstack = slot.Itemstack;
			CollectibleObject collectible = (itemstack != null) ? itemstack.Collectible : null;
			if (collectible == null)
			{
				return;
			}
			IHasServerMeleeAttacks itemProvider = collectible as IHasServerMeleeAttacks;
			if (itemProvider != null)
			{
				MeleeSystemServer.AddServerMeleeAttacks(attacker, target, slot, builder, itemProvider);
			}
			CollectibleBehavior[] array = collectible.CollectibleBehaviors ?? Array.Empty<CollectibleBehavior>();
			for (int i = 0; i < array.Length; i++)
			{
				IHasServerMeleeAttacks behaviorProvider = array[i] as IHasServerMeleeAttacks;
				if (behaviorProvider != null)
				{
					MeleeSystemServer.AddServerMeleeAttacks(attacker, target, slot, builder, behaviorProvider);
				}
			}
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0003FA04 File Offset: 0x0003DC04
		private static void AddServerMeleeAttacks(EntityPlayer attacker, Entity target, ItemSlot slot, MeleeSystemServer.MeleeAttackLimitsBuilder builder, IHasServerMeleeAttacks provider)
		{
			try
			{
				foreach (ServerMeleeAttackStats attack in provider.GetServerMeleeAttacks(attacker, target, slot))
				{
					builder.Add(attack.Attack, attack.StackStats);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0003FA74 File Offset: 0x0003DC74
		private static void AddMeleeWeaponStats(MeleeSystemServer.MeleeAttackLimitsBuilder builder, MeleeWeaponStats stats)
		{
			MeleeSystemServer.AddStance(builder, stats.OneHandedStance);
			MeleeSystemServer.AddStance(builder, stats.TwoHandedStance);
			MeleeSystemServer.AddStance(builder, stats.OffHandStance);
			foreach (StanceStats stance in stats.MainHandDualWieldStances.Values)
			{
				MeleeSystemServer.AddStance(builder, stance);
			}
			foreach (StanceStats stance2 in stats.OffHandDualWieldStances.Values)
			{
				MeleeSystemServer.AddStance(builder, stance2);
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0003FB38 File Offset: 0x0003DD38
		private static void AddStance(MeleeSystemServer.MeleeAttackLimitsBuilder builder, [Nullable(2)] StanceStats stance)
		{
			if (stance == null)
			{
				return;
			}
			builder.Add(stance.Attack);
			builder.Add(stance.Riposte);
			builder.Add(stance.BlockBash);
			builder.Add(stance.HandleAttack);
			Dictionary<string, MeleeAttackStats> directionalAttacks = stance.DirectionalAttacks;
			IEnumerable<MeleeAttackStats> enumerable = (directionalAttacks != null) ? directionalAttacks.Values : null;
			foreach (MeleeAttackStats attack in (enumerable ?? Enumerable.Empty<MeleeAttackStats>()))
			{
				builder.Add(attack);
			}
			Dictionary<string, MeleeAttackStats> directionalBlockBashes = stance.DirectionalBlockBashes;
			enumerable = ((directionalBlockBashes != null) ? directionalBlockBashes.Values : null);
			foreach (MeleeAttackStats attack2 in (enumerable ?? Enumerable.Empty<MeleeAttackStats>()))
			{
				builder.Add(attack2);
			}
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0003FC24 File Offset: 0x0003DE24
		private static void AddStanceBasedMeleeWeaponStats(MeleeSystemServer.MeleeAttackLimitsBuilder builder, StanceBasedMeleeWeaponStats stats)
		{
			MeleeSystemServer.AddGrip(builder, stats.OneHanded);
			MeleeSystemServer.AddGrip(builder, stats.TwoHanded);
			MeleeSystemServer.AddGrip(builder, stats.OffHand);
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0003FC4C File Offset: 0x0003DE4C
		private static void AddGrip(MeleeSystemServer.MeleeAttackLimitsBuilder builder, [Nullable(2)] StanceBasedMeleeWeaponGripStats grip)
		{
			if (grip == null)
			{
				return;
			}
			builder.Add(grip.DefaultLeftClickAttack);
			builder.Add(grip.DefaultRightClickAttack);
			foreach (StanceBasedMeleeWeaponAttackStats attack in grip.StanceToStanceLeftClickAttacks.Values)
			{
				builder.Add(attack);
			}
			foreach (StanceBasedMeleeWeaponAttackStats attack2 in grip.StanceToStanceRightClickAttacks.Values)
			{
				builder.Add(attack2);
			}
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0003FD08 File Offset: 0x0003DF08
		private bool IsTargetWithinReach(EntityPlayer attacker, Entity target, float maxReach)
		{
			CombatOverhaulSystem system = this._api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			double allowedDistance = (double)(maxReach + system.Settings.CollisionRadius) + 1.0;
			Vector3d vector3d = new Vector3d(attacker.Pos.X, attacker.Pos.Y, attacker.Pos.Z);
			Vector3d closestTargetPoint = MeleeSystemServer.GetClosestPointOnTargetAabb(vector3d, target);
			return Vector3d.Distance(vector3d, closestTargetPoint) <= allowedDistance;
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0003FD7C File Offset: 0x0003DF7C
		private bool IsTerrainObstructingHit(EntityPlayer attacker, Vector3d hitPosition)
		{
			foreach (Vector3d origin in MeleeSystemServer.GetTerrainValidationOrigins(attacker))
			{
				if (!this.IsTerrainObstructingSegment(origin, hitPosition))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0003FDD4 File Offset: 0x0003DFD4
		private bool IsTerrainObstructingSegment(Vector3d origin, Vector3d hitPosition)
		{
			Vector3d segment = hitPosition - origin;
			double length = segment.Length;
			if (length <= 0.3)
			{
				return false;
			}
			Vector3d direction = segment / length;
			Vector3d start = origin + direction * 0.25;
			Vector3d end = hitPosition - direction * 0.05;
			Vector3d vector3d = end - start;
			Vector3d vector3d2;
			BlockFacing blockFacing;
			Block block;
			BlockPos blockPos;
			return vector3d.LengthSquared > 1E-06 && CuboidAABBCollider.CollideWithTerrain(this._api.World.BlockAccessor, end, start, 0.03, out vector3d, out vector3d2, out blockFacing, out block, out blockPos);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0003FE7D File Offset: 0x0003E07D
		private static IEnumerable<Vector3d> GetTerrainValidationOrigins(EntityPlayer attacker)
		{
			MeleeSystemServer.<GetTerrainValidationOrigins>d__43 <GetTerrainValidationOrigins>d__ = new MeleeSystemServer.<GetTerrainValidationOrigins>d__43(-2);
			<GetTerrainValidationOrigins>d__.<>3__attacker = attacker;
			return <GetTerrainValidationOrigins>d__;
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0003FE90 File Offset: 0x0003E090
		private static Vector3d GetClosestPointOnTargetAabb(Vector3d point, Entity target)
		{
			Cuboidf collisionBox = target.CollisionBox;
			EntityPos position = target.Pos;
			double minX = (double)Math.Min(collisionBox.X1, collisionBox.X2) + position.X;
			double minY = (double)Math.Min(collisionBox.Y1, collisionBox.Y2) + position.Y;
			double minZ = (double)Math.Min(collisionBox.Z1, collisionBox.Z2) + position.Z;
			double maxX = (double)Math.Max(collisionBox.X1, collisionBox.X2) + position.X;
			double maxY = (double)Math.Max(collisionBox.Y1, collisionBox.Y2) + position.Y;
			double maxZ = (double)Math.Max(collisionBox.Z1, collisionBox.Z2) + position.Z;
			return new Vector3d(Math.Clamp(point.X, minX, maxX), Math.Clamp(point.Y, minY, maxY), Math.Clamp(point.Z, minZ, maxZ));
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0003FF7C File Offset: 0x0003E17C
		private static float GetAllowedMaxDamage(float maxDamage, [Nullable(2)] ItemStack weaponStack, Entity target)
		{
			float effectiveMaxDamage = maxDamage;
			float buffableMaxDamage = GrindingWheelCompat.ApplyBuffableDamage(weaponStack, target, maxDamage, null, false);
			if (float.IsFinite(buffableMaxDamage))
			{
				effectiveMaxDamage = MathF.Max(effectiveMaxDamage, buffableMaxDamage);
			}
			return effectiveMaxDamage + MathF.Max(0.05f, MathF.Abs(effectiveMaxDamage) * 0.2f);
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0003FFC0 File Offset: 0x0003E1C0
		private void LogRejectedAttackPacket(IServerPlayer player, MeleeDamagePacket packet, string reason, string details = "")
		{
			if (this._rejectedAttackPacketLogs >= 20)
			{
				return;
			}
			this._rejectedAttackPacketLogs++;
			string detailsText = string.IsNullOrEmpty(details) ? "" : (", " + details);
			ICoreAPI api = this._api;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(86, 8);
			defaultInterpolatedStringHandler.AppendLiteral("Rejected melee attack packet from '");
			defaultInterpolatedStringHandler.AppendFormatted(player.PlayerName);
			defaultInterpolatedStringHandler.AppendLiteral("' (");
			defaultInterpolatedStringHandler.AppendFormatted(player.PlayerUID);
			defaultInterpolatedStringHandler.AppendLiteral("): reason=");
			defaultInterpolatedStringHandler.AppendFormatted(reason);
			defaultInterpolatedStringHandler.AppendLiteral(", attacker=");
			defaultInterpolatedStringHandler.AppendFormatted<long>(packet.AttackerEntityId);
			defaultInterpolatedStringHandler.AppendLiteral(", target=");
			defaultInterpolatedStringHandler.AppendFormatted<long>(packet.TargetEntityId);
			defaultInterpolatedStringHandler.AppendLiteral(", damage=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(packet.Damage);
			defaultInterpolatedStringHandler.AppendLiteral(", type='");
			defaultInterpolatedStringHandler.AppendFormatted(packet.DamageType);
			defaultInterpolatedStringHandler.AppendLiteral("'");
			defaultInterpolatedStringHandler.AppendFormatted(detailsText);
			LoggerUtil.Warn(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x000400DE File Offset: 0x0003E2DE
		private void Push(MeleeCollisionPacket packet)
		{
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x000400E0 File Offset: 0x0003E2E0
		private bool DealDamage(Entity target, DamageSource damageSource, [Nullable(2)] ItemSlot slot, float damage)
		{
			MeleeSystemServer.MeleeDamageDelegate onDealMeleeDamage = this.OnDealMeleeDamage;
			if (onDealMeleeDamage != null)
			{
				onDealMeleeDamage(target, damageSource, slot, ref damage);
			}
			WeaponBuffSystem.ModifyMeleeDamage(target, damageSource, slot, ref damage);
			damage = GrindingWheelCompat.ApplyBuffableDamage((slot != null) ? slot.Itemstack : null, target, damage, damageSource);
			bool damageReceived = target.ReceiveDamage(damageSource, damage);
			if (this.OnMeleeDamageResolved != null)
			{
				PlayerDamageModelBehavior behavior = target.GetBehavior<PlayerDamageModelBehavior>();
				DamageBlockEventArgs block = (behavior != null) ? behavior.GetLastDamageBlock(damageSource) : null;
				this.OnMeleeDamageResolved(new MeleeDamageResolvedEventArgs(target, damageSource, slot, damage, damageReceived, block));
			}
			return damageReceived;
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x00040164 File Offset: 0x0003E364
		[NullableContext(2)]
		private void DealDurabilityDamage(ItemSlot slot, [Nullable(1)] MeleeDamagePacket packet, Entity attacker)
		{
			if (packet.DurabilityDamage <= 0)
			{
				return;
			}
			bool flag;
			if (slot == null)
			{
				flag = (null != null);
			}
			else
			{
				ItemStack itemstack = slot.Itemstack;
				flag = (((itemstack != null) ? itemstack.Collectible : null) != null);
			}
			if (flag && attacker != null)
			{
				slot.Itemstack.Collectible.DamageItem(attacker.Api.World, attacker, slot, packet.DurabilityDamage, true);
				slot.MarkDirty();
			}
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x000401C4 File Offset: 0x0003E3C4
		private void PrintLog([Nullable(2)] Entity attacker, bool damageReceived, Entity target, MeleeDamagePacket packet, string targetName)
		{
			if (this._api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).Settings.PrintMeleeHits)
			{
				float damage = damageReceived ? target.WatchedAttributes.GetFloat("onHurt", 0f) : 0f;
				string text = "combatoverhaul:damagelog-dealt-damage";
				object[] array = new object[3];
				int num = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("combatoverhaul:entity-damage-zone-");
				defaultInterpolatedStringHandler.AppendFormatted<ColliderTypes>((ColliderTypes)packet.ColliderType);
				array[num] = Lang.Get(defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<object>());
				array[1] = targetName;
				int num2 = 2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(0, 1);
				defaultInterpolatedStringHandler2.AppendFormatted<float>(damage, "F2");
				array[num2] = defaultInterpolatedStringHandler2.ToStringAndClear();
				string damageLogMessage = Lang.Get(text, array);
				EntityPlayer entityPlayer = attacker as EntityPlayer;
				IServerPlayer serverPlayer = ((entityPlayer != null) ? entityPlayer.Player : null) as IServerPlayer;
				if (serverPlayer == null)
				{
					return;
				}
				serverPlayer.SendMessage(GlobalConstants.DamageLogChatGroup, damageLogMessage, 4, null);
			}
		}

		// Token: 0x04000634 RID: 1588
		private readonly ICoreServerAPI _api;

		// Token: 0x04000635 RID: 1589
		private const double _reachTolerance = 1.0;

		// Token: 0x04000636 RID: 1590
		private const float _damageTolerance = 0.05f;

		// Token: 0x04000637 RID: 1591
		private const float _damageRelativeTolerance = 0.2f;

		// Token: 0x04000638 RID: 1592
		private const float _knockbackTolerance = 0.01f;

		// Token: 0x04000639 RID: 1593
		private const int _maxRejectedAttackPacketLogs = 20;

		// Token: 0x0400063A RID: 1594
		private const double TerrainObstructionRadius = 0.03;

		// Token: 0x0400063B RID: 1595
		private const double TerrainObstructionStartPadding = 0.25;

		// Token: 0x0400063C RID: 1596
		private const double TerrainObstructionEndPadding = 0.05;

		// Token: 0x0400063D RID: 1597
		private int _rejectedAttackPacketLogs;

		// Token: 0x02000255 RID: 597
		// (Invoke) Token: 0x0600169F RID: 5791
		[NullableContext(0)]
		public delegate void MeleeDamageDelegate(Entity target, DamageSource damageSource, [Nullable(2)] ItemSlot slot, ref float damage);

		// Token: 0x02000256 RID: 598
		// (Invoke) Token: 0x060016A3 RID: 5795
		[NullableContext(0)]
		public delegate void MeleeDamageResolvedDelegate(MeleeDamageResolvedEventArgs args);

		// Token: 0x02000257 RID: 599
		// (Invoke) Token: 0x060016A7 RID: 5799
		[NullableContext(0)]
		public delegate void MeleeAttackDelegate(Entity attacker, ItemSlot weaponSlot);

		// Token: 0x02000258 RID: 600
		[Nullable(0)]
		private readonly struct MeleeAttackLimits
		{
			// Token: 0x060016AA RID: 5802 RVA: 0x0006A384 File Offset: 0x00068584
			public MeleeAttackLimits(float maxReach, float maxDamage, float minKnockback, float maxKnockback, int maxTier, int maxArmorPiercingTier, int maxDurabilityDamage, int maxStaggerTimeMs, int maxStaggerTier, HashSet<EnumDamageType> damageTypes, HashSet<EnumDamageType> terrainBypassDamageTypes)
			{
				this.MaxReach = maxReach;
				this.MaxDamage = maxDamage;
				this.MinKnockback = minKnockback;
				this.MaxKnockback = maxKnockback;
				this.MaxTier = maxTier;
				this.MaxArmorPiercingTier = maxArmorPiercingTier;
				this.MaxDurabilityDamage = maxDurabilityDamage;
				this.MaxStaggerTimeMs = maxStaggerTimeMs;
				this.MaxStaggerTier = maxStaggerTier;
				this.DamageTypes = damageTypes;
				this.TerrainBypassDamageTypes = terrainBypassDamageTypes;
			}

			// Token: 0x060016AB RID: 5803 RVA: 0x0006A3E6 File Offset: 0x000685E6
			public bool RequiresTerrainClearance(EnumDamageType damageType)
			{
				return !this.TerrainBypassDamageTypes.Contains(damageType);
			}

			// Token: 0x04000C71 RID: 3185
			public readonly float MaxReach;

			// Token: 0x04000C72 RID: 3186
			public readonly float MaxDamage;

			// Token: 0x04000C73 RID: 3187
			public readonly float MinKnockback;

			// Token: 0x04000C74 RID: 3188
			public readonly float MaxKnockback;

			// Token: 0x04000C75 RID: 3189
			public readonly int MaxTier;

			// Token: 0x04000C76 RID: 3190
			public readonly int MaxArmorPiercingTier;

			// Token: 0x04000C77 RID: 3191
			public readonly int MaxDurabilityDamage;

			// Token: 0x04000C78 RID: 3192
			public readonly int MaxStaggerTimeMs;

			// Token: 0x04000C79 RID: 3193
			public readonly int MaxStaggerTier;

			// Token: 0x04000C7A RID: 3194
			public readonly HashSet<EnumDamageType> DamageTypes;

			// Token: 0x04000C7B RID: 3195
			public readonly HashSet<EnumDamageType> TerrainBypassDamageTypes;
		}

		// Token: 0x02000259 RID: 601
		[Nullable(0)]
		private sealed class MeleeAttackLimitsBuilder
		{
			// Token: 0x060016AC RID: 5804 RVA: 0x0006A3F8 File Offset: 0x000685F8
			public MeleeAttackLimitsBuilder(EntityPlayer attacker, Entity target, ItemSlot slot, ItemStackMeleeWeaponStats stackStats)
			{
				this._attacker = attacker;
				this._target = target;
				this._slot = slot;
				this._defaultStackStats = stackStats;
				this._meleeDamageMultiplier = attacker.Stats.GetBlended("meleeWeaponsDamage");
				JsonObject attributes = target.Properties.Attributes;
				this._mechanicalsDamageMultiplier = ((attributes != null && attributes["isMechanical"].AsBool(false)) ? attacker.Stats.GetBlended("mechanicalsDamage") : 1f);
				this._isDagger = MeleeSystemServer.MeleeAttackLimitsBuilder.IsDagger(slot);
			}

			// Token: 0x060016AD RID: 5805 RVA: 0x0006A4A1 File Offset: 0x000686A1
			[NullableContext(2)]
			public void Add(MeleeAttackStats attack)
			{
				this.Add(attack, this._defaultStackStats);
			}

			// Token: 0x060016AE RID: 5806 RVA: 0x0006A4B0 File Offset: 0x000686B0
			[NullableContext(2)]
			public void Add(MeleeAttackStats attack, ItemStackMeleeWeaponStats stackStats)
			{
				if (attack == null)
				{
					return;
				}
				this._hasAttack = true;
				this._maxReach = Math.Max(this._maxReach, attack.MaxReach);
				foreach (MeleeDamageTypeJson damageType in attack.DamageTypes ?? Array.Empty<MeleeDamageTypeJson>())
				{
					this.Add(damageType, stackStats, attack.CollideWithTerrain);
				}
			}

			// Token: 0x060016AF RID: 5807 RVA: 0x0006A510 File Offset: 0x00068710
			public bool TryBuild(out MeleeSystemServer.MeleeAttackLimits limits)
			{
				limits = default(MeleeSystemServer.MeleeAttackLimits);
				if (!this._hasAttack || this._damageTypes.Count == 0)
				{
					return false;
				}
				limits = new MeleeSystemServer.MeleeAttackLimits(this._maxReach, this._maxDamage, this._minKnockback, this._maxKnockback, this._maxTier, this._maxArmorPiercingTier, this._maxDurabilityDamage, this._maxStaggerTimeMs, this._maxStaggerTier, this._damageTypes, this._terrainBypassDamageTypes);
				return true;
			}

			// Token: 0x060016B0 RID: 5808 RVA: 0x0006A589 File Offset: 0x00068789
			private void Add(MeleeDamageTypeJson damageType, ItemStackMeleeWeaponStats stackStats)
			{
				this.Add(damageType, stackStats, true);
			}

			// Token: 0x060016B1 RID: 5809 RVA: 0x0006A594 File Offset: 0x00068794
			private void Add(MeleeDamageTypeJson damageType, ItemStackMeleeWeaponStats stackStats, bool collidesWithTerrain)
			{
				EnumDamageType configuredDamageType;
				if (!Enum.TryParse<EnumDamageType>(damageType.Damage.DamageType, ref configuredDamageType))
				{
					return;
				}
				this._damageTypes.Add(configuredDamageType);
				if (!collidesWithTerrain)
				{
					this._terrainBypassDamageTypes.Add(configuredDamageType);
				}
				if (configuredDamageType == 4 && this._isDagger)
				{
					this._damageTypes.Add(3);
					if (!collidesWithTerrain)
					{
						this._terrainBypassDamageTypes.Add(3);
					}
				}
				float damage = damageType.Damage.Damage * this._meleeDamageMultiplier * this._mechanicalsDamageMultiplier;
				damage += stackStats.DamageBonus;
				damage *= stackStats.DamageMultiplier;
				this._maxDamage = Math.Max(this._maxDamage, damage);
				string damageTierStat = "meleeDamageTierBonus" + configuredDamageType.ToString();
				float statValue = this._attacker.Stats.GetBlended(damageTierStat) - 1f;
				int damageTier = GameMath.Max(damageType.Damage.Tier + stackStats.DamageTierBonus + (int)statValue, 0);
				this._maxTier = Math.Max(this._maxTier, damageTier);
				this._maxArmorPiercingTier = Math.Max(this._maxArmorPiercingTier, damageType.Damage.ArmorPiercingTier + stackStats.ArmorPiercingBonus);
				this._maxDurabilityDamage = Math.Max(this._maxDurabilityDamage, damageType.DurabilityDamage);
				this._maxStaggerTimeMs = Math.Max(this._maxStaggerTimeMs, damageType.StaggerTimeMs);
				this._maxStaggerTier = Math.Max(this._maxStaggerTier, damageType.StaggerTier);
				float knockback = damageType.Knockback * stackStats.KnockbackMultiplier;
				this._minKnockback = Math.Min(this._minKnockback, knockback);
				this._maxKnockback = Math.Max(this._maxKnockback, knockback);
			}

			// Token: 0x060016B2 RID: 5810 RVA: 0x0006A737 File Offset: 0x00068937
			private static bool IsDagger(ItemSlot slot)
			{
				return CollectibleClassifier.IsDagger(slot);
			}

			// Token: 0x04000C7C RID: 3196
			private readonly EntityPlayer _attacker;

			// Token: 0x04000C7D RID: 3197
			private readonly Entity _target;

			// Token: 0x04000C7E RID: 3198
			private readonly ItemSlot _slot;

			// Token: 0x04000C7F RID: 3199
			private readonly ItemStackMeleeWeaponStats _defaultStackStats;

			// Token: 0x04000C80 RID: 3200
			private readonly float _meleeDamageMultiplier;

			// Token: 0x04000C81 RID: 3201
			private readonly float _mechanicalsDamageMultiplier;

			// Token: 0x04000C82 RID: 3202
			private readonly bool _isDagger;

			// Token: 0x04000C83 RID: 3203
			private readonly HashSet<EnumDamageType> _damageTypes = new HashSet<EnumDamageType>();

			// Token: 0x04000C84 RID: 3204
			private readonly HashSet<EnumDamageType> _terrainBypassDamageTypes = new HashSet<EnumDamageType>();

			// Token: 0x04000C85 RID: 3205
			private bool _hasAttack;

			// Token: 0x04000C86 RID: 3206
			private float _maxReach;

			// Token: 0x04000C87 RID: 3207
			private float _maxDamage;

			// Token: 0x04000C88 RID: 3208
			private float _minKnockback;

			// Token: 0x04000C89 RID: 3209
			private float _maxKnockback;

			// Token: 0x04000C8A RID: 3210
			private int _maxTier;

			// Token: 0x04000C8B RID: 3211
			private int _maxArmorPiercingTier;

			// Token: 0x04000C8C RID: 3212
			private int _maxDurabilityDamage;

			// Token: 0x04000C8D RID: 3213
			private int _maxStaggerTimeMs;

			// Token: 0x04000C8E RID: 3214
			private int _maxStaggerTier;
		}
	}
}
