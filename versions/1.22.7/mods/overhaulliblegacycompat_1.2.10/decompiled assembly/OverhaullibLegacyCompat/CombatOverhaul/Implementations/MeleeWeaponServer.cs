using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000082 RID: 130
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeWeaponServer : RangeWeaponServer
	{
		// Token: 0x060006AE RID: 1710 RVA: 0x0002A1C0 File Offset: 0x000283C0
		public MeleeWeaponServer(ICoreServerAPI api, Item item) : base(api, item)
		{
			ProjectileSystemServer serverProjectileSystem = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true).ServerProjectileSystem;
			if (serverProjectileSystem == null)
			{
				throw new Exception();
			}
			this.ProjectileSystem = serverProjectileSystem;
			try
			{
				this.Stats = item.Attributes.AsObject<MeleeWeaponStats>(null);
			}
			catch (Exception exception)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(54, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error while getting stats for item '");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(item.Code);
				defaultInterpolatedStringHandler.AppendLiteral("' on server side: ");
				defaultInterpolatedStringHandler.AppendFormatted(exception.Message);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0002A264 File Offset: 0x00028464
		public override bool Shoot(IServerPlayer player, ItemSlot slot, ShotPacket packet, Entity shooter)
		{
			GeneralUtils.MarkItemStack(slot);
			if (((slot != null) ? slot.Itemstack : null) == null || slot.Itemstack.StackSize < 1)
			{
				return false;
			}
			ProjectileBehavior collectibleBehavior = slot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true);
			ProjectileStats stats = (collectibleBehavior != null) ? collectibleBehavior.GetStats(slot.Itemstack) : null;
			ItemStackMeleeWeaponStats stackStats = ItemStackMeleeWeaponStats.FromItemStack(slot.Itemstack);
			if (stats == null)
			{
				return false;
			}
			Vector3d playerVelocity;
			playerVelocity..ctor(player.Entity.ServerPos.Motion.X, player.Entity.ServerPos.Motion.Y, player.Entity.ServerPos.Motion.Z);
			ProjectileSpawnStats projectileSpawnStats = default(ProjectileSpawnStats);
			projectileSpawnStats.ProducerEntityId = player.Entity.EntityId;
			projectileSpawnStats.DamageMultiplier = 1f * stackStats.ThrownDamageMultiplier;
			ThrowWeaponStats throwAttack = this.Stats.ThrowAttack;
			projectileSpawnStats.DamageTier = ((throwAttack != null) ? throwAttack.DamageTier : stackStats.ThrownDamageTierBonus);
			projectileSpawnStats.Position = new Vector3d(packet.Position[0], packet.Position[1], packet.Position[2]);
			Vector3d vector3d = Vector3d.Normalize(new Vector3d(packet.Velocity[0], packet.Velocity[1], packet.Velocity[2]));
			ThrowWeaponStats throwAttack2 = this.Stats.ThrowAttack;
			projectileSpawnStats.Velocity = vector3d * (double)((throwAttack2 != null) ? throwAttack2.Velocity : 1f) * (double)stackStats.ThrownProjectileSpeedMultiplier + playerVelocity;
			ProjectileSpawnStats spawnStats = projectileSpawnStats;
			AssetLocation projectileCode = slot.Itemstack.Item.Code.Clone();
			ItemStack weaponStack = slot.Itemstack.Clone();
			ItemStack projectileStack = slot.TakeOut(1);
			this.ProjectileSystem.Spawn(packet.ProjectileId[0], stats, spawnStats, projectileStack, weaponStack, shooter);
			this.SwapToNewProjectile(player, slot, projectileCode);
			slot.MarkDirty();
			return true;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0002A444 File Offset: 0x00028644
		protected virtual void SwapToNewProjectile(IServerPlayer player, ItemSlot slot, AssetLocation projectileCode)
		{
			if (slot2.Itemstack == null || slot2.Itemstack.StackSize == 0)
			{
				ItemSlot replacementSlot = null;
				this.WalkInventory(player.Entity, delegate(ItemSlot slot)
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
					if (slot.Itemstack.Item.Code.ToString() == projectileCode.ToString())
					{
						replacementSlot = slot;
						return false;
					}
					return true;
				});
				if (replacementSlot == null)
				{
					string projectilePath = projectileCode.ToShortString();
					while (projectilePath.Contains('-'))
					{
						int delimiterIndex = projectilePath.LastIndexOf('-');
						projectilePath = projectilePath.Substring(0, delimiterIndex);
						string wildcard = projectilePath + "-*";
						this.WalkInventory(player.Entity, delegate(ItemSlot slot)
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
							if (WildcardUtil.Match(wildcard, slot.Itemstack.Item.Code.ToString()))
							{
								replacementSlot = slot;
								return false;
							}
							return true;
						});
						if (replacementSlot != null)
						{
							break;
						}
					}
				}
				if (replacementSlot == null && CollectibleClassifier.IsDaggerCode(projectileCode))
				{
					this.WalkInventory(player.Entity, delegate(ItemSlot slot)
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
						if (slot == player.Entity.LeftHandItemSlot)
						{
							return true;
						}
						if (!this.IsDaggerStack(slot.Itemstack))
						{
							return true;
						}
						replacementSlot = slot;
						return false;
					});
				}
				if (replacementSlot != null)
				{
					slot2.TryFlipWith(replacementSlot);
					replacementSlot.MarkDirty();
					return;
				}
				if (slot2 == player.Entity.RightHandItemSlot)
				{
					ItemSlot leftHandItemSlot = player.Entity.LeftHandItemSlot;
					if (this.IsDaggerStack((leftHandItemSlot != null) ? leftHandItemSlot.Itemstack : null))
					{
						ItemSlot offhandSlot = player.Entity.LeftHandItemSlot;
						slot2.TryFlipWith(offhandSlot);
						offhandSlot.MarkDirty();
					}
				}
			}
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0002A5D3 File Offset: 0x000287D3
		[NullableContext(2)]
		protected virtual bool IsDaggerStack(ItemStack stack)
		{
			return CollectibleClassifier.IsDagger(stack);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0002A5DC File Offset: 0x000287DC
		protected virtual void WalkInventory(EntityPlayer player, Func<ItemSlot, bool> selector)
		{
			MeleeWeaponServer.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.selector = selector;
			IPlayer player2 = player.Player;
			IPlayerInventoryManager inventoryManager = (player2 != null) ? player2.InventoryManager : null;
			if (inventoryManager == null)
			{
				return;
			}
			CS$<>8__locals1.scannedSlots = new HashSet<ItemSlot>();
			if (!MeleeWeaponServer.<WalkInventory>g__ScanInventory|5_0(inventoryManager.GetOwnInventory("hotbar"), ref CS$<>8__locals1))
			{
				return;
			}
			if (!MeleeWeaponServer.<WalkInventory>g__ScanInventory|5_0(inventoryManager.GetOwnInventory("backpack"), ref CS$<>8__locals1))
			{
				return;
			}
			EntityBehaviorPlayerInventory behavior = player.GetBehavior<EntityBehaviorPlayerInventory>();
			if (!MeleeWeaponServer.<WalkInventory>g__ScanInventory|5_0((behavior != null) ? behavior.Inventory : null, ref CS$<>8__locals1))
			{
				return;
			}
			if (inventoryManager.Inventories == null)
			{
				return;
			}
			foreach (KeyValuePair<string, IInventory> keyValuePair in inventoryManager.Inventories)
			{
				string text;
				IInventory inventory;
				keyValuePair.Deconstruct(ref text, ref inventory);
				if (!MeleeWeaponServer.<WalkInventory>g__ScanInventory|5_0(inventory, ref CS$<>8__locals1))
				{
					break;
				}
			}
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0002A6BC File Offset: 0x000288BC
		[NullableContext(2)]
		[CompilerGenerated]
		internal static bool <WalkInventory>g__ScanInventory|5_0(IInventory inventory, ref MeleeWeaponServer.<>c__DisplayClass5_0 A_1)
		{
			if (inventory == null || inventory is InventoryPlayerCreative)
			{
				return true;
			}
			foreach (ItemSlot slot in inventory)
			{
				if (A_1.scannedSlots.Add(slot) && !A_1.selector.Invoke(slot))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040002E9 RID: 745
		protected readonly MeleeWeaponStats Stats;
	}
}
