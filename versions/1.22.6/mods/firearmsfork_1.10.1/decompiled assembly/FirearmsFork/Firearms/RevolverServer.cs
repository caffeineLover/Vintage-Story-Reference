using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Implementations;
using CombatOverhaul.RangedSystems;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Firearms
{
	// Token: 0x0200002F RID: 47
	[NullableContext(1)]
	[Nullable(0)]
	public class RevolverServer : RangeWeaponServer
	{
		// Token: 0x06000263 RID: 611 RVA: 0x0000CB4F File Offset: 0x0000AD4F
		public RevolverServer(ICoreServerAPI api, Item item) : base(api, item)
		{
			this.Stats = item.Attributes.AsObject<RevolverStats>(null);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000CB78 File Offset: 0x0000AD78
		public override bool Reload(IServerPlayer player, ItemSlot slot, [Nullable(2)] ItemSlot ammoSlot, ReloadPacket packet)
		{
			base.Reload(player, slot, ammoSlot, packet);
			if (packet.Data.Length < 2)
			{
				return false;
			}
			int stage = (int)packet.Data[0];
			RevolverReadyState state = (RevolverReadyState)packet.Data[1];
			if (stage < 0 || stage >= this.Stats.LoadStages.Length)
			{
				return false;
			}
			if (packet.Amount > 0)
			{
				if (ammoSlot == null || packet.Data.Length < 4)
				{
					return false;
				}
				int expectedStage = (int)packet.Data[2];
				RevolverReadyState expectedReadyState = (RevolverReadyState)packet.Data[3];
				ItemStack itemstack = slot.Itemstack;
				int num = (itemstack != null) ? itemstack.Attributes.GetAsInt("CombatOverhaul:loading-stage", 0) : 0;
				ItemStack itemstack2 = slot.Itemstack;
				RevolverReadyState actualReadyState = (RevolverReadyState)((itemstack2 != null) ? itemstack2.Attributes.GetAsInt("CombatOverhaul:ready-stage", 0) : 0);
				if (num != expectedStage || actualReadyState != expectedReadyState)
				{
					return false;
				}
			}
			if (packet.Amount <= 0)
			{
				this.SetLoadStage(slot, stage);
				this.SetReadyState(slot, state);
				slot.MarkDirty();
				return true;
			}
			RevolverLoadStageStats currentStage = this.Stats.LoadStages[stage];
			int powderNeeded = currentStage.PowderConsumption;
			ItemSlot flask = (powderNeeded > 0) ? this.GetFlask(player, powderNeeded) : null;
			ItemSlot wadding = (currentStage.WaddingUsed > 0) ? this.GetWadding(player, currentStage.WaddingUsed) : null;
			if (powderNeeded > 0)
			{
				bool flag;
				if (flask == null)
				{
					flag = (null != null);
				}
				else
				{
					ItemStack itemstack3 = flask.Itemstack;
					flag = (((itemstack3 != null) ? itemstack3.Item : null) != null);
				}
				if (!flag)
				{
					return false;
				}
			}
			if (currentStage.WaddingUsed > 0)
			{
				bool flag2;
				if (wadding == null)
				{
					flag2 = (null != null);
				}
				else
				{
					ItemStack itemstack4 = wadding.Itemstack;
					flag2 = (((itemstack4 != null) ? itemstack4.Item : null) != null);
				}
				if (!flag2)
				{
					return false;
				}
			}
			if (ammoSlot != null)
			{
				this.Inventory.Read(slot, "magazine");
				ItemStack itemstack5 = ammoSlot.Itemstack;
				AssetLocation assetLocation;
				if (itemstack5 == null)
				{
					assetLocation = null;
				}
				else
				{
					Item item = itemstack5.Item;
					assetLocation = ((item != null) ? item.Code : null);
				}
				if (!(assetLocation != null) || ammoSlot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) == null || !WildcardUtil.Match(this.Stats.BulletWildcard, ammoSlot.Itemstack.Item.Code.ToString()) || !FirearmsAmmoUtility.HasEnoughBulletItems(ammoSlot, currentStage.BulletLoaded, this.Stats.BulletsLoadedPerBulletItem))
				{
					this.Inventory.Clear();
					return false;
				}
				FirearmsAmmoUtility.TryConsumeAndLoadBullets(this.Inventory, ammoSlot, currentStage.BulletLoaded, this.Stats.BulletsLoadedPerBulletItem);
				ammoSlot.MarkDirty();
				this.Inventory.Write(slot);
				this.Inventory.Clear();
			}
			if (flask != null && powderNeeded > 0)
			{
				flask.Itemstack.Attributes.SetInt("durability", flask.Itemstack.Collectible.GetRemainingDurability(flask.Itemstack) - powderNeeded);
				flask.MarkDirty();
			}
			if (wadding != null && currentStage.WaddingUsed > 0)
			{
				wadding.TakeOut(currentStage.WaddingUsed);
				wadding.MarkDirty();
			}
			this.SetLoadStage(slot, stage);
			this.SetReadyState(slot, state);
			slot.MarkDirty();
			return true;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000CE44 File Offset: 0x0000B044
		public override bool Shoot(IServerPlayer player, ItemSlot slot, ShotPacket packet, Entity shooter)
		{
			base.Shoot(player, slot, packet, shooter);
			if (packet.Data.Length < 2)
			{
				return false;
			}
			int stage = (int)packet.Data[0];
			RevolverReadyState state = (RevolverReadyState)packet.Data[1];
			RevolverLoadStageStats revolverLoadStageStats = this.Stats.LoadStages[stage];
			this.Inventory.Read(slot, "magazine");
			if (this.Inventory.Items.Count == 0)
			{
				return false;
			}
			int count = 0;
			int additionalDurabilityCost = 0;
			while (this.Inventory.Items.Count > 0 && count < packet.Amount)
			{
				ItemStack ammo = this.Inventory.Items[0];
				ammo.ResolveBlockOrItem(this.Api.World);
				this.Inventory.Items.RemoveAt(0);
				Item item = ammo.Item;
				ProjectileStats projectileStats;
				if (item == null)
				{
					projectileStats = null;
				}
				else
				{
					ProjectileBehavior collectibleBehavior = item.GetCollectibleBehavior<ProjectileBehavior>(true);
					projectileStats = ((collectibleBehavior != null) ? collectibleBehavior.GetStats(ammo) : null);
				}
				ProjectileStats stats = projectileStats;
				if (stats != null)
				{
					FirearmsAmmoUtility.ApplyProjectileDisappearance(ammo, stats);
				}
				if (stats != null)
				{
					ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(slot.Itemstack);
					additionalDurabilityCost = Math.Max(additionalDurabilityCost, stats.AdditionalDurabilityCost);
					float[] dispersion = new float[]
					{
						this.Stats.FiringStats.DispersionMOA[0] * stackStats.DispersionMultiplier,
						this.Stats.FiringStats.DispersionMOA[1] * stackStats.DispersionMultiplier
					};
					Vector3d velocity = base.GetDirectionWithDispersion(packet.Velocity, dispersion) * (double)this.Stats.FiringStats.BulletVelocity * (double)stackStats.ProjectileSpeed;
					ProjectileSpawnStats projectileSpawnStats = default(ProjectileSpawnStats);
					projectileSpawnStats.ProducerEntityId = player.Entity.EntityId;
					projectileSpawnStats.DamageMultiplier = this.Stats.FiringStats.BulletDamageMultiplier * stackStats.DamageMultiplier;
					projectileSpawnStats.DamageTier = this.Stats.FiringStats.BulletDamageTier + stackStats.DamageTierBonus;
					projectileSpawnStats.Position = new Vector3d(packet.Position[0], packet.Position[1], packet.Position[2]);
					projectileSpawnStats.Velocity = velocity;
					ProjectileSpawnStats spawnStats = projectileSpawnStats;
					this.ProjectileSystem.SpawnFromWeaponSlot(packet.ProjectileId[count], stats, spawnStats, ammo, slot, shooter);
					count++;
				}
			}
			this.Inventory.Write(slot);
			this.Inventory.Clear();
			slot.Itemstack.Item.DamageItem(player.Entity.World, player.Entity, slot, 1 + additionalDurabilityCost, true);
			this.SetLoadStage(slot, stage);
			this.SetReadyState(slot, state);
			slot.MarkDirty();
			return true;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000D0D7 File Offset: 0x0000B2D7
		protected void SetLoadStage(ItemSlot slot, int stage)
		{
			ItemStack itemstack = slot.Itemstack;
			if (itemstack == null)
			{
				return;
			}
			itemstack.Attributes.SetInt("CombatOverhaul:loading-stage", stage);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000D0F4 File Offset: 0x0000B2F4
		protected void SetReadyState(ItemSlot slot, RevolverReadyState state)
		{
			ItemStack itemstack = slot.Itemstack;
			if (itemstack == null)
			{
				return;
			}
			itemstack.Attributes.SetInt("CombatOverhaul:ready-stage", (int)state);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000D114 File Offset: 0x0000B314
		[return: Nullable(2)]
		protected ItemSlot GetFlask(IServerPlayer player, int powderNeeded)
		{
			ItemSlot flaskSlot = null;
			player.Entity.WalkInventory(delegate(ItemSlot slot)
			{
				AssetLocation assetLocation;
				if (slot == null)
				{
					assetLocation = null;
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					if (itemstack == null)
					{
						assetLocation = null;
					}
					else
					{
						Item item = itemstack.Item;
						assetLocation = ((item != null) ? item.Code : null);
					}
				}
				if (assetLocation == null)
				{
					return true;
				}
				if (WildcardUtil.Match(this.Stats.FlaskWildcard, slot.Itemstack.Item.Code.ToString()) && slot.Itemstack.Item.GetRemainingDurability(slot.Itemstack) >= powderNeeded)
				{
					flaskSlot = slot;
					return false;
				}
				return true;
			});
			return flaskSlot;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000D15C File Offset: 0x0000B35C
		[return: Nullable(2)]
		protected ItemSlot GetWadding(IServerPlayer player, int amount)
		{
			ItemSlot waddingSlot = null;
			player.Entity.WalkInventory(delegate(ItemSlot slot)
			{
				AssetLocation assetLocation;
				if (slot == null)
				{
					assetLocation = null;
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					if (itemstack == null)
					{
						assetLocation = null;
					}
					else
					{
						Item item = itemstack.Item;
						assetLocation = ((item != null) ? item.Code : null);
					}
				}
				if (assetLocation == null)
				{
					return true;
				}
				if (WildcardUtil.Match(this.Stats.WaddingWildcard, slot.Itemstack.Item.Code.ToString()) && slot.Itemstack.StackSize >= amount)
				{
					waddingSlot = slot;
					return false;
				}
				return true;
			});
			return waddingSlot;
		}

		// Token: 0x04000144 RID: 324
		protected readonly RevolverStats Stats;

		// Token: 0x04000145 RID: 325
		protected readonly ItemInventoryBuffer Inventory = new ItemInventoryBuffer();

		// Token: 0x04000146 RID: 326
		protected const string InventoryId = "magazine";

		// Token: 0x04000147 RID: 327
		protected const string LoadingStageAttribute = "CombatOverhaul:loading-stage";

		// Token: 0x04000148 RID: 328
		protected const string WeaponReadyStageAttribute = "CombatOverhaul:ready-stage";
	}
}
