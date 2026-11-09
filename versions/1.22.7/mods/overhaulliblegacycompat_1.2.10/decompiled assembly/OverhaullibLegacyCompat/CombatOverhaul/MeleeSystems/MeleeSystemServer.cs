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
	// Token: 0x02000106 RID: 262
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MeleeSystemServer : MeleeSystem
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000D0F RID: 3343 RVA: 0x0003FFC8 File Offset: 0x0003E1C8
		// (remove) Token: 0x06000D10 RID: 3344 RVA: 0x00040000 File Offset: 0x0003E200
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event MeleeSystemServer.MeleeDamageDelegate OnDealMeleeDamage;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000D11 RID: 3345 RVA: 0x00040038 File Offset: 0x0003E238
		// (remove) Token: 0x06000D12 RID: 3346 RVA: 0x00040070 File Offset: 0x0003E270
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event MeleeSystemServer.MeleeDamageResolvedDelegate OnMeleeDamageResolved;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000D13 RID: 3347 RVA: 0x000400A8 File Offset: 0x0003E2A8
		// (remove) Token: 0x06000D14 RID: 3348 RVA: 0x000400E0 File Offset: 0x0003E2E0
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event MeleeSystemServer.MeleeAttackDelegate OnMeleeAttackStart;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000D15 RID: 3349 RVA: 0x00040118 File Offset: 0x0003E318
		// (remove) Token: 0x06000D16 RID: 3350 RVA: 0x00040150 File Offset: 0x0003E350
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event MeleeSystemServer.MeleeAttackDelegate OnMeleeAttackEnd;

		// Token: 0x06000D17 RID: 3351 RVA: 0x00040188 File Offset: 0x0003E388
		public MeleeSystemServer(ICoreServerAPI api)
		{
			this._api = api;
			api.Network.RegisterChannel("CombatOverhaul:damage-packets").RegisterMessageType<MeleeAttackPacket>().RegisterMessageType<MeleePushPacket>().RegisterMessageType<MeleeAttackStatusPacket>().SetMessageHandler<MeleeAttackPacket>(new NetworkClientMessageHandler<MeleeAttackPacket>(this.HandlePacket)).SetMessageHandler<MeleePushPacket>(new NetworkClientMessageHandler<MeleePushPacket>(this.HandlePacket)).SetMessageHandler<MeleeAttackStatusPacket>(new NetworkClientMessageHandler<MeleeAttackStatusPacket>(this.HandlePacket));
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x000401F8 File Offset: 0x0003E3F8
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

		// Token: 0x06000D19 RID: 3353 RVA: 0x00040234 File Offset: 0x0003E434
		private void HandlePacket(IServerPlayer player, MeleePushPacket packet)
		{
			foreach (MeleeCollisionPacket collisionPacket in packet.MeleeAttackDamagePackets)
			{
				this.Push(collisionPacket);
			}
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00040264 File Offset: 0x0003E464
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

		// Token: 0x06000D1B RID: 3355 RVA: 0x000402D4 File Offset: 0x0003E4D4
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

		// Token: 0x06000D1C RID: 3356 RVA: 0x00040470 File Offset: 0x0003E670
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

		// Token: 0x06000D1D RID: 3357 RVA: 0x000407B8 File Offset: 0x0003E9B8
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

		// Token: 0x06000D1E RID: 3358 RVA: 0x00040825 File Offset: 0x0003EA25
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

		// Token: 0x06000D1F RID: 3359 RVA: 0x00040854 File Offset: 0x0003EA54
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

		// Token: 0x06000D20 RID: 3360 RVA: 0x00040984 File Offset: 0x0003EB84
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

		// Token: 0x06000D21 RID: 3361 RVA: 0x000409F4 File Offset: 0x0003EBF4
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

		// Token: 0x06000D22 RID: 3362 RVA: 0x00040A64 File Offset: 0x0003EC64
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

		// Token: 0x06000D23 RID: 3363 RVA: 0x00040B28 File Offset: 0x0003ED28
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

		// Token: 0x06000D24 RID: 3364 RVA: 0x00040C14 File Offset: 0x0003EE14
		private static void AddStanceBasedMeleeWeaponStats(MeleeSystemServer.MeleeAttackLimitsBuilder builder, StanceBasedMeleeWeaponStats stats)
		{
			MeleeSystemServer.AddGrip(builder, stats.OneHanded);
			MeleeSystemServer.AddGrip(builder, stats.TwoHanded);
			MeleeSystemServer.AddGrip(builder, stats.OffHand);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00040C3C File Offset: 0x0003EE3C
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

		// Token: 0x06000D26 RID: 3366 RVA: 0x00040CF8 File Offset: 0x0003EEF8
		private bool IsTargetWithinReach(EntityPlayer attacker, Entity target, float maxReach)
		{
			CombatOverhaulSystem system = this._api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			double allowedDistance = (double)(maxReach + system.Settings.CollisionRadius) + 1.0;
			Vector3d vector3d = new Vector3d(attacker.Pos.X, attacker.Pos.Y, attacker.Pos.Z);
			Vector3d closestTargetPoint = MeleeSystemServer.GetClosestPointOnTargetAabb(vector3d, target);
			return Vector3d.Distance(vector3d, closestTargetPoint) <= allowedDistance;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00040D6C File Offset: 0x0003EF6C
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

		// Token: 0x06000D28 RID: 3368 RVA: 0x00040DC4 File Offset: 0x0003EFC4
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

		// Token: 0x06000D29 RID: 3369 RVA: 0x00040E6D File Offset: 0x0003F06D
		private static IEnumerable<Vector3d> GetTerrainValidationOrigins(EntityPlayer attacker)
		{
			MeleeSystemServer.<GetTerrainValidationOrigins>d__43 <GetTerrainValidationOrigins>d__ = new MeleeSystemServer.<GetTerrainValidationOrigins>d__43(-2);
			<GetTerrainValidationOrigins>d__.<>3__attacker = attacker;
			return <GetTerrainValidationOrigins>d__;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00040E80 File Offset: 0x0003F080
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

		// Token: 0x06000D2B RID: 3371 RVA: 0x00040F6C File Offset: 0x0003F16C
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

		// Token: 0x06000D2C RID: 3372 RVA: 0x00040FB0 File Offset: 0x0003F1B0
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

		// Token: 0x06000D2D RID: 3373 RVA: 0x000410CE File Offset: 0x0003F2CE
		private void Push(MeleeCollisionPacket packet)
		{
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x000410D0 File Offset: 0x0003F2D0
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

		// Token: 0x06000D2F RID: 3375 RVA: 0x00041154 File Offset: 0x0003F354
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

		// Token: 0x06000D30 RID: 3376 RVA: 0x000411B4 File Offset: 0x0003F3B4
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

		// Token: 0x04000656 RID: 1622
		private readonly ICoreServerAPI _api;

		// Token: 0x04000657 RID: 1623
		private const double _reachTolerance = 1.0;

		// Token: 0x04000658 RID: 1624
		private const float _damageTolerance = 0.05f;

		// Token: 0x04000659 RID: 1625
		private const float _damageRelativeTolerance = 0.2f;

		// Token: 0x0400065A RID: 1626
		private const float _knockbackTolerance = 0.01f;

		// Token: 0x0400065B RID: 1627
		private const int _maxRejectedAttackPacketLogs = 20;

		// Token: 0x0400065C RID: 1628
		private const double TerrainObstructionRadius = 0.03;

		// Token: 0x0400065D RID: 1629
		private const double TerrainObstructionStartPadding = 0.25;

		// Token: 0x0400065E RID: 1630
		private const double TerrainObstructionEndPadding = 0.05;

		// Token: 0x0400065F RID: 1631
		private int _rejectedAttackPacketLogs;

		// Token: 0x02000264 RID: 612
		// (Invoke) Token: 0x0600171A RID: 5914
		[NullableContext(0)]
		public delegate void MeleeDamageDelegate(Entity target, DamageSource damageSource, [Nullable(2)] ItemSlot slot, ref float damage);

		// Token: 0x02000265 RID: 613
		// (Invoke) Token: 0x0600171E RID: 5918
		[NullableContext(0)]
		public delegate void MeleeDamageResolvedDelegate(MeleeDamageResolvedEventArgs args);

		// Token: 0x02000266 RID: 614
		// (Invoke) Token: 0x06001722 RID: 5922
		[NullableContext(0)]
		public delegate void MeleeAttackDelegate(Entity attacker, ItemSlot weaponSlot);

		// Token: 0x02000267 RID: 615
		[Nullable(0)]
		private readonly struct MeleeAttackLimits
		{
			// Token: 0x06001725 RID: 5925 RVA: 0x0006BE48 File Offset: 0x0006A048
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

			// Token: 0x06001726 RID: 5926 RVA: 0x0006BEAA File Offset: 0x0006A0AA
			public bool RequiresTerrainClearance(EnumDamageType damageType)
			{
				return !this.TerrainBypassDamageTypes.Contains(damageType);
			}

			// Token: 0x04000C9B RID: 3227
			public readonly float MaxReach;

			// Token: 0x04000C9C RID: 3228
			public readonly float MaxDamage;

			// Token: 0x04000C9D RID: 3229
			public readonly float MinKnockback;

			// Token: 0x04000C9E RID: 3230
			public readonly float MaxKnockback;

			// Token: 0x04000C9F RID: 3231
			public readonly int MaxTier;

			// Token: 0x04000CA0 RID: 3232
			public readonly int MaxArmorPiercingTier;

			// Token: 0x04000CA1 RID: 3233
			public readonly int MaxDurabilityDamage;

			// Token: 0x04000CA2 RID: 3234
			public readonly int MaxStaggerTimeMs;

			// Token: 0x04000CA3 RID: 3235
			public readonly int MaxStaggerTier;

			// Token: 0x04000CA4 RID: 3236
			public readonly HashSet<EnumDamageType> DamageTypes;

			// Token: 0x04000CA5 RID: 3237
			public readonly HashSet<EnumDamageType> TerrainBypassDamageTypes;
		}

		// Token: 0x02000268 RID: 616
		[Nullable(0)]
		private sealed class MeleeAttackLimitsBuilder
		{
			// Token: 0x06001727 RID: 5927 RVA: 0x0006BEBC File Offset: 0x0006A0BC
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

			// Token: 0x06001728 RID: 5928 RVA: 0x0006BF65 File Offset: 0x0006A165
			[NullableContext(2)]
			public void Add(MeleeAttackStats attack)
			{
				this.Add(attack, this._defaultStackStats);
			}

			// Token: 0x06001729 RID: 5929 RVA: 0x0006BF74 File Offset: 0x0006A174
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

			// Token: 0x0600172A RID: 5930 RVA: 0x0006BFD4 File Offset: 0x0006A1D4
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

			// Token: 0x0600172B RID: 5931 RVA: 0x0006C04D File Offset: 0x0006A24D
			private void Add(MeleeDamageTypeJson damageType, ItemStackMeleeWeaponStats stackStats)
			{
				this.Add(damageType, stackStats, true);
			}

			// Token: 0x0600172C RID: 5932 RVA: 0x0006C058 File Offset: 0x0006A258
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

			// Token: 0x0600172D RID: 5933 RVA: 0x0006C1FB File Offset: 0x0006A3FB
			private static bool IsDagger(ItemSlot slot)
			{
				return CollectibleClassifier.IsDagger(slot);
			}

			// Token: 0x04000CA6 RID: 3238
			private readonly EntityPlayer _attacker;

			// Token: 0x04000CA7 RID: 3239
			private readonly Entity _target;

			// Token: 0x04000CA8 RID: 3240
			private readonly ItemSlot _slot;

			// Token: 0x04000CA9 RID: 3241
			private readonly ItemStackMeleeWeaponStats _defaultStackStats;

			// Token: 0x04000CAA RID: 3242
			private readonly float _meleeDamageMultiplier;

			// Token: 0x04000CAB RID: 3243
			private readonly float _mechanicalsDamageMultiplier;

			// Token: 0x04000CAC RID: 3244
			private readonly bool _isDagger;

			// Token: 0x04000CAD RID: 3245
			private readonly HashSet<EnumDamageType> _damageTypes = new HashSet<EnumDamageType>();

			// Token: 0x04000CAE RID: 3246
			private readonly HashSet<EnumDamageType> _terrainBypassDamageTypes = new HashSet<EnumDamageType>();

			// Token: 0x04000CAF RID: 3247
			private bool _hasAttack;

			// Token: 0x04000CB0 RID: 3248
			private float _maxReach;

			// Token: 0x04000CB1 RID: 3249
			private float _maxDamage;

			// Token: 0x04000CB2 RID: 3250
			private float _minKnockback;

			// Token: 0x04000CB3 RID: 3251
			private float _maxKnockback;

			// Token: 0x04000CB4 RID: 3252
			private int _maxTier;

			// Token: 0x04000CB5 RID: 3253
			private int _maxArmorPiercingTier;

			// Token: 0x04000CB6 RID: 3254
			private int _maxDurabilityDamage;

			// Token: 0x04000CB7 RID: 3255
			private int _maxStaggerTimeMs;

			// Token: 0x04000CB8 RID: 3256
			private int _maxStaggerTier;
		}
	}
}
