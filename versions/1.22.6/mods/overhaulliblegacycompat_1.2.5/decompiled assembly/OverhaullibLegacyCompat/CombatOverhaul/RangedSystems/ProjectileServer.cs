using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Colliders;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.WeaponBuffs;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000BC RID: 188
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ProjectileServer
	{
		// Token: 0x060009A0 RID: 2464 RVA: 0x00033FC4 File Offset: 0x000321C4
		public ProjectileServer(ProjectileEntity projectile, ProjectileStats projectileStats, ProjectileSpawnStats spawnStats, ICoreAPI api, Action<Guid> clearCallback, ItemStack projectileStack, [Nullable(2)] ItemSlot weaponSlot = null) : this(projectile, projectileStats, spawnStats, api, clearCallback, projectileStack, weaponSlot, true)
		{
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00033FE4 File Offset: 0x000321E4
		public ProjectileServer(ProjectileEntity projectile, ProjectileStats projectileStats, ProjectileSpawnStats spawnStats, ICoreAPI api, Action<Guid> clearCallback, ItemStack projectileStack, [Nullable(2)] ItemSlot weaponSlot, bool applyBuffs)
		{
			this._stats = projectileStats;
			this._spawnStats = spawnStats;
			this._api = api;
			this._shooter = this._api.World.GetEntityById(spawnStats.ProducerEntityId);
			this._weaponSlot = weaponSlot;
			this._applyBuffs = applyBuffs;
			ProjectileSystemServer serverProjectileSystem = this._api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).ServerProjectileSystem;
			if (serverProjectileSystem == null)
			{
				throw new Exception();
			}
			this._system = serverProjectileSystem;
			this._settings = this._api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).Settings;
			this._entity = projectile;
			this._entity.ClearCallback = clearCallback;
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x00034090 File Offset: 0x00032290
		// (set) Token: 0x060009A3 RID: 2467 RVA: 0x00034098 File Offset: 0x00032298
		public int PacketVersion { get; set; }

		// Token: 0x060009A4 RID: 2468 RVA: 0x000340A4 File Offset: 0x000322A4
		public void OnCollision(ProjectileCollisionPacket packet)
		{
			Entity receiver = this._api.World.GetEntityById(packet.ReceiverEntity);
			if (receiver == null)
			{
				return;
			}
			if (packet.CollisionPoint.Length < 3)
			{
				return;
			}
			float initialPenetrationStrength = this._entity.PenetrationStrength;
			this._entity.PenetrationStrength = Math.Max(0f, this._entity.PenetrationStrength - packet.PenetrationStrengthLoss);
			Vector3d collisionPoint;
			collisionPoint..ctor(packet.CollisionPoint[0], packet.CollisionPoint[1], packet.CollisionPoint[2]);
			if (this._entity.PenetrationStrength == 0f)
			{
				this._entity.ServerPos.SetPos(new Vec3d(collisionPoint.X, collisionPoint.Y, collisionPoint.Z));
				this._entity.ServerPos.Motion.X = receiver.ServerPos.Motion.X;
				this._entity.ServerPos.Motion.Y = receiver.ServerPos.Motion.Y;
				this._entity.ServerPos.Motion.Z = receiver.ServerPos.Motion.Z;
			}
			else
			{
				this._entity.ServerPos.SetPos(new Vec3d(collisionPoint.X, collisionPoint.Y, collisionPoint.Z));
				float speedReduction = this._entity.PenetrationStrength / initialPenetrationStrength;
				this._entity.ServerPos.Motion.X *= (double)speedReduction;
				this._entity.ServerPos.Motion.Y *= (double)speedReduction;
				this._entity.ServerPos.Motion.Z *= (double)speedReduction;
			}
			this.DealCollisionDamage(receiver, collisionPoint, packet.Collider, packet.RelativeSpeed);
			this._entity.OnCollisionWithEntity(receiver, packet.Collider);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00034290 File Offset: 0x00032490
		public void OnLateCollision(ProjectileCollisionPacket packet)
		{
			Entity receiver = this._api.World.GetEntityById(packet.ReceiverEntity);
			if (receiver == null)
			{
				return;
			}
			if (packet.CollisionPoint.Length < 3)
			{
				return;
			}
			Vector3d collisionPoint;
			collisionPoint..ctor(packet.CollisionPoint[0], packet.CollisionPoint[1], packet.CollisionPoint[2]);
			this.DealCollisionDamage(receiver, collisionPoint, packet.Collider, packet.RelativeSpeed);
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x000342F7 File Offset: 0x000324F7
		public void TryCollide()
		{
			this._system.TryCollide(this._entity);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0003430A File Offset: 0x0003250A
		private void DealCollisionDamage(Entity receiver, Vector3d collisionPoint, string collider, double relativeSpeed)
		{
			if (this.Attack(this._shooter, receiver, collisionPoint, collider, relativeSpeed))
			{
				this.PlaySound(this._shooter);
			}
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0003432C File Offset: 0x0003252C
		private bool Attack(Entity attacker, Entity target, Vector3d position, string collider, double relativeSpeed)
		{
			if (relativeSpeed < (double)this._stats.SpeedThreshold)
			{
				return false;
			}
			if (!target.Alive)
			{
				return false;
			}
			string targetName = target.GetName();
			string projectileName = this._entity.GetName();
			string damageTierStat = "rangedDamageTierBonus" + this._stats.DamageStats.DamageType.ToString();
			float statValue = attacker.Stats.GetBlended(damageTierStat) - 1f;
			float damage = this._stats.DamageStats.Damage * this._spawnStats.DamageMultiplier;
			if (this._settings.RangedWeaponsDamageSupport)
			{
				float rangedWeaponsDamageStat = Math.Max(0f, attacker.Stats.GetBlended("rangedWeaponsDamage"));
				damage *= rangedWeaponsDamageStat;
			}
			int damageTierBonus = this._stats.DamageTierBonus + (int)statValue;
			DamageData damageData = new DamageData(Enum.Parse<EnumDamageType>(this._stats.DamageStats.DamageType), Math.Max(1, this._spawnStats.DamageTier + damageTierBonus), 0);
			if (!ProjectileServer.CheckPermissions(attacker, target) && damageData.DamageType != 6)
			{
				return false;
			}
			DirectionalTypedDamageSource damageSource = new DirectionalTypedDamageSource
			{
				Source = 8,
				SourceEntity = this._entity,
				CauseEntity = attacker,
				Type = damageData.DamageType,
				Position = position,
				Collider = collider,
				DamageTypeData = damageData,
				DamageTier = damageData.Tier,
				KnockbackStrength = this._stats.Knockback,
				Weapon = this._entity.WeaponStack,
				IgnoreInvFrames = this._entity.IgnoreInvFrames
			};
			this._system.OnDealDamage(target, damageSource, this._entity.WeaponStack, this._entity.ProjectileStack, ref damage, this._applyBuffs);
			bool damageReceived = target.ReceiveDamage(damageSource, damage);
			PlayerDamageModelBehavior behavior = target.GetBehavior<PlayerDamageModelBehavior>();
			DamageBlockEventArgs block = (behavior != null) ? behavior.GetLastDamageBlock(damageSource) : null;
			this._system.EmitRangedDamageResolved(target, damageSource, this._entity.WeaponStack, this._entity.ProjectileStack, damage, damageReceived, block);
			if (damageReceived && this._applyBuffs)
			{
				if (WeaponBuffSystem.ConsumeInternal(this._entity.WeaponStack, WeaponBuffConsumptionTrigger.RangedHit) > 0)
				{
					ItemSlot weaponSlot = this._weaponSlot;
					if (weaponSlot != null)
					{
						weaponSlot.MarkDirty();
					}
				}
				WeaponBuffSystem.ConsumeInternal(this._entity.ProjectileStack, WeaponBuffConsumptionTrigger.ProjectileHit);
			}
			bool received = damageReceived || damage <= 0f;
			if (this._settings.PrintRangeHits && collider != "")
			{
				CollidersEntityBehavior colliders = target.GetBehavior<CollidersEntityBehavior>();
				ColliderTypes ColliderType = ColliderTypes.Torso;
				ColliderTypes colliderType;
				if (colliders != null && colliders.CollidersTypes.TryGetValue(collider, ref colliderType))
				{
					ColliderType = colliderType;
				}
				float damageReceivedValue = damageReceived ? target.WatchedAttributes.GetFloat("onHurt", 0f) : 0f;
				string text = "combatoverhaul:damagelog-dealt-damage-with-projectile";
				object[] array = new object[4];
				int num = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("combatoverhaul:entity-damage-zone-");
				defaultInterpolatedStringHandler.AppendFormatted<ColliderTypes>(ColliderType);
				array[num] = Lang.Get(defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<object>());
				array[1] = targetName;
				int num2 = 2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(0, 1);
				defaultInterpolatedStringHandler2.AppendFormatted<float>(damageReceivedValue, "F2");
				array[num2] = defaultInterpolatedStringHandler2.ToStringAndClear();
				array[3] = projectileName;
				string damageLogMessage = Lang.Get(text, array);
				EntityPlayer entityPlayer = attacker as EntityPlayer;
				IServerPlayer serverPlayer = ((entityPlayer != null) ? entityPlayer.Player : null) as IServerPlayer;
				if (serverPlayer != null)
				{
					serverPlayer.SendMessage(GlobalConstants.DamageLogChatGroup, damageLogMessage, 4, null);
				}
			}
			return received;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00034698 File Offset: 0x00032898
		private static bool CheckPermissions(Entity attacker, Entity target)
		{
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
			return true;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0003470C File Offset: 0x0003290C
		private void PlaySound(Entity attacker)
		{
			ItemStack projectileStack = this._entity.ProjectileStack;
			ProjectileStats projectileStats;
			if (projectileStack == null)
			{
				projectileStats = null;
			}
			else
			{
				Item item = projectileStack.Item;
				if (item == null)
				{
					projectileStats = null;
				}
				else
				{
					ProjectileBehavior collectibleBehavior = item.GetCollectibleBehavior<ProjectileBehavior>(true);
					projectileStats = ((collectibleBehavior != null) ? collectibleBehavior.Stats : null);
				}
			}
			ProjectileStats stats = projectileStats;
			if (stats != null)
			{
				EntityPlayer player = attacker as EntityPlayer;
				if (player != null && !(stats.HitSound == ""))
				{
					this._api.World.PlaySoundFor(new AssetLocation(stats.HitSound), player.Player, false, 32f, 1f);
					return;
				}
			}
		}

		// Token: 0x04000464 RID: 1124
		public const string DamageTierPlayerStatPrefix = "rangedDamageTierBonus";

		// Token: 0x04000465 RID: 1125
		private readonly ProjectileStats _stats;

		// Token: 0x04000466 RID: 1126
		private readonly ProjectileSpawnStats _spawnStats;

		// Token: 0x04000467 RID: 1127
		internal readonly ProjectileEntity _entity;

		// Token: 0x04000468 RID: 1128
		private readonly Entity _shooter;

		// Token: 0x04000469 RID: 1129
		private readonly ICoreAPI _api;

		// Token: 0x0400046A RID: 1130
		private readonly ProjectileSystemServer _system;

		// Token: 0x0400046B RID: 1131
		private readonly Settings _settings;

		// Token: 0x0400046C RID: 1132
		[Nullable(2)]
		private readonly ItemSlot _weaponSlot;

		// Token: 0x0400046D RID: 1133
		private readonly bool _applyBuffs;
	}
}
