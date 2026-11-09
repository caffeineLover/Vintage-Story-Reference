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
	// Token: 0x02000022 RID: 34
	[NullableContext(1)]
	[Nullable(0)]
	public class MuzzleloaderServer : RangeWeaponServer
	{
		// Token: 0x0600018C RID: 396 RVA: 0x00008D44 File Offset: 0x00006F44
		public MuzzleloaderServer(ICoreServerAPI api, Item item) : base(api, item)
		{
			this.Stats = item.Attributes.AsObject<MuzzleloaderStats>(null);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008D74 File Offset: 0x00006F74
		public override bool Reload(IServerPlayer player, ItemSlot slot, [Nullable(2)] ItemSlot ammoSlot, ReloadPacket packet)
		{
			base.Reload(player, slot, ammoSlot, packet);
			MuzzleloaderLoadingStage currentStage = MuzzleloaderServer.GetLoadingStage<MuzzleloaderLoadingStage>(packet);
			MuzzleloaderLoadingStage storedStage = MuzzleloaderServer.GetLoadingStage<MuzzleloaderLoadingStage>(slot);
			this.Inventory.Read(slot, "magazine");
			int storedMagazineCount = this.Inventory.Items.Count;
			this.Inventory.Clear();
			if (!this.ValidReloadTransition(currentStage, storedStage, storedMagazineCount, packet))
			{
				return false;
			}
			int num;
			if (currentStage != MuzzleloaderLoadingStage.Loading)
			{
				if (currentStage != MuzzleloaderLoadingStage.Priming)
				{
					num = 1;
				}
				else
				{
					num = this.Stats.PrimePowderConsumption;
				}
			}
			else
			{
				num = this.Stats.LoadPowderConsumption;
			}
			int powderNeeded = num;
			if (packet.Amount > 0 && (ammoSlot == null || packet.Data.Length < 8))
			{
				return false;
			}
			ItemSlot flask = this.GetFlask(player, powderNeeded);
			ItemSlot wadding = this.GetWadding(player);
			if (powderNeeded > 0)
			{
				bool flag;
				if (flask == null)
				{
					flag = (null != null);
				}
				else
				{
					ItemStack itemstack = flask.Itemstack;
					flag = (((itemstack != null) ? itemstack.Item : null) != null);
				}
				if (!flag)
				{
					return false;
				}
			}
			if (this.Stats.WaddingUsedPerReload > 0)
			{
				bool flag2;
				if (wadding == null)
				{
					flag2 = (null != null);
				}
				else
				{
					ItemStack itemstack2 = wadding.Itemstack;
					flag2 = (((itemstack2 != null) ? itemstack2.Item : null) != null);
				}
				if (!flag2 && currentStage == MuzzleloaderLoadingStage.Loading)
				{
					return false;
				}
			}
			if (ammoSlot != null)
			{
				this.Inventory.Read(slot, "magazine");
				int expectedMagazineCount = MuzzleloaderServer.GetExpectedMagazineCount(packet);
				if (expectedMagazineCount >= 0 && this.Inventory.Items.Count != expectedMagazineCount)
				{
					this.Inventory.Clear();
					return false;
				}
				if (this.Inventory.Items.Count >= this.Stats.MagazineSize)
				{
					this.Inventory.Clear();
					return false;
				}
				ItemStack itemstack3 = ammoSlot.Itemstack;
				AssetLocation assetLocation;
				if (itemstack3 == null)
				{
					assetLocation = null;
				}
				else
				{
					Item item = itemstack3.Item;
					assetLocation = ((item != null) ? item.Code : null);
				}
				if (!(assetLocation != null) || ammoSlot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) == null || !WildcardUtil.Match(this.Stats.BulletWildcard, ammoSlot.Itemstack.Item.Code.ToString()) || !FirearmsAmmoUtility.HasEnoughBulletItems(ammoSlot, this.Stats.BulletLoadedPerReload, this.Stats.BulletsLoadedPerBulletItem))
				{
					this.Inventory.Clear();
					return false;
				}
				FirearmsAmmoUtility.TryConsumeAndLoadBullets(this.Inventory, ammoSlot, this.Stats.BulletLoadedPerReload, this.Stats.BulletsLoadedPerBulletItem);
				ammoSlot.MarkDirty();
				this.Inventory.Write(slot);
				this.Inventory.Clear();
			}
			if (flask != null && powderNeeded > 0)
			{
				flask.Itemstack.Attributes.SetInt("durability", flask.Itemstack.Collectible.GetRemainingDurability(flask.Itemstack) - powderNeeded);
				flask.MarkDirty();
			}
			if (wadding != null && this.Stats.WaddingUsedPerReload > 0 && currentStage == MuzzleloaderLoadingStage.Loading)
			{
				wadding.TakeOut(this.Stats.WaddingUsedPerReload);
				wadding.MarkDirty();
			}
			MuzzleloaderServer.SetLoadingStage<MuzzleloaderLoadingStage>(slot, currentStage);
			return true;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00009040 File Offset: 0x00007240
		public override bool Shoot(IServerPlayer player, ItemSlot slot, ShotPacket packet, Entity shooter)
		{
			base.Shoot(player, slot, packet, shooter);
			if (MuzzleloaderServer.GetLoadingStage<MuzzleloaderLoadingStage>(slot) != this.LastStage)
			{
				return false;
			}
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
					ProjectileSpawnStats projectileSpawnStats = default(ProjectileSpawnStats);
					projectileSpawnStats.ProducerEntityId = player.Entity.EntityId;
					projectileSpawnStats.DamageMultiplier = this.Stats.BulletDamageMultiplier * stackStats.DamageMultiplier;
					projectileSpawnStats.DamageStrength = this.Stats.BulletDamageStrength + (float)stackStats.DamageTierBonus;
					projectileSpawnStats.Position = new Vector3d(packet.Position[0], packet.Position[1], packet.Position[2]);
					projectileSpawnStats.Velocity = base.GetDirectionWithDispersion(packet.Velocity, new float[]
					{
						this.Stats.DispersionMOA[0] * stackStats.DispersionMultiplier,
						this.Stats.DispersionMOA[1] * stackStats.DispersionMultiplier
					}) * (double)this.Stats.BulletVelocity * (double)stackStats.ProjectileSpeed;
					ProjectileSpawnStats spawnStats = projectileSpawnStats;
					this.ProjectileSystem.SpawnFromWeaponSlot(packet.ProjectileId[count], stats, spawnStats, ammo, slot, shooter);
					count++;
				}
			}
			slot.Itemstack.Item.DamageItem(player.Entity.World, player.Entity, slot, 1 + additionalDurabilityCost, true);
			slot.MarkDirty();
			this.Inventory.Write(slot);
			int ammoLeft = this.Inventory.Items.Count;
			this.Inventory.Clear();
			MuzzleloaderServer.SetLoadingStage<MuzzleloaderLoadingStage>(slot, (ammoLeft == 0) ? MuzzleloaderLoadingStage.Unloaded : this.LastStage);
			return true;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000092A4 File Offset: 0x000074A4
		protected virtual bool ValidReloadTransition(MuzzleloaderLoadingStage requestedStage, MuzzleloaderLoadingStage storedStage, int storedMagazineCount, ReloadPacket packet)
		{
			bool result;
			if (requestedStage != MuzzleloaderLoadingStage.Loading)
			{
				result = (requestedStage == MuzzleloaderLoadingStage.Priming && (packet.Amount == 0 && storedStage == MuzzleloaderLoadingStage.Loading) && storedMagazineCount > 0);
			}
			else
			{
				result = (packet.Amount > 0 && storedStage != MuzzleloaderLoadingStage.Priming && storedMagazineCount < this.Stats.MagazineSize);
			}
			return result;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000092F8 File Offset: 0x000074F8
		[NullableContext(0)]
		protected static TStage GetLoadingStage<TStage>([Nullable(1)] ReloadPacket packet) where TStage : struct, Enum
		{
			int stage = BitConverter.ToInt32(packet.Data, 0);
			return (TStage)((object)Enum.ToObject(typeof(TStage), stage));
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00009327 File Offset: 0x00007527
		protected static int GetExpectedMagazineCount(ReloadPacket packet)
		{
			if (packet.Data.Length < 8)
			{
				return -1;
			}
			return BitConverter.ToInt32(packet.Data, 4);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00009344 File Offset: 0x00007544
		[NullableContext(0)]
		protected static TStage GetLoadingStage<TStage>([Nullable(1)] ItemSlot slot) where TStage : struct, Enum
		{
			ItemStack itemstack = slot.Itemstack;
			int stage = (itemstack != null) ? itemstack.Attributes.GetAsInt("CombatOverhaul:loading-stage", 0) : 0;
			return (TStage)((object)Enum.ToObject(typeof(TStage), stage));
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00009384 File Offset: 0x00007584
		[NullableContext(0)]
		protected static void SetLoadingStage<TStage>([Nullable(1)] ItemSlot slot, TStage stage) where TStage : struct, Enum
		{
			ItemStack itemstack = slot.Itemstack;
			if (itemstack != null)
			{
				itemstack.Attributes.SetInt("CombatOverhaul:loading-stage", (int)Enum.ToObject(typeof(TStage), stage));
			}
			slot.MarkDirty();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000093C4 File Offset: 0x000075C4
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

		// Token: 0x06000195 RID: 405 RVA: 0x0000940C File Offset: 0x0000760C
		[return: Nullable(2)]
		protected ItemSlot GetWadding(IServerPlayer player)
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
				if (WildcardUtil.Match(this.Stats.WaddingWildcard, slot.Itemstack.Item.Code.ToString()) && slot.Itemstack.StackSize >= this.Stats.WaddingUsedPerReload)
				{
					waddingSlot = slot;
					return false;
				}
				return true;
			});
			return waddingSlot;
		}

		// Token: 0x040000D7 RID: 215
		protected readonly MuzzleloaderStats Stats;

		// Token: 0x040000D8 RID: 216
		protected readonly ItemInventoryBuffer Inventory = new ItemInventoryBuffer();

		// Token: 0x040000D9 RID: 217
		protected const string InventoryId = "magazine";

		// Token: 0x040000DA RID: 218
		protected const string LoadingStageAttribute = "CombatOverhaul:loading-stage";

		// Token: 0x040000DB RID: 219
		protected readonly MuzzleloaderLoadingStage LastStage = MuzzleloaderLoadingStage.Priming;
	}
}
