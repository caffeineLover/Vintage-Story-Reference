using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems;
using Vintagestory.API.Common;

namespace Firearms
{
	// Token: 0x02000014 RID: 20
	[NullableContext(1)]
	[Nullable(0)]
	internal static class FirearmsAmmoUtility
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00004874 File Offset: 0x00002A74
		public static int BulletItemsRequired(int bulletsLoaded, int bulletsLoadedPerBulletItem)
		{
			if (bulletsLoaded <= 0)
			{
				return 0;
			}
			int loadedPerItem = Math.Max(1, bulletsLoadedPerBulletItem);
			return (bulletsLoaded + loadedPerItem - 1) / loadedPerItem;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004898 File Offset: 0x00002A98
		public static bool HasEnoughBulletItems(ItemSlot ammoSlot, int bulletsLoaded, int bulletsLoadedPerBulletItem)
		{
			int bulletItemsRequired = FirearmsAmmoUtility.BulletItemsRequired(bulletsLoaded, bulletsLoadedPerBulletItem);
			ItemStack itemstack = ammoSlot.Itemstack;
			return itemstack != null && itemstack.StackSize >= bulletItemsRequired;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000048C4 File Offset: 0x00002AC4
		public static bool TryConsumeAndLoadBullets(ItemInventoryBuffer inventory, ItemSlot ammoSlot, int bulletsLoaded, int bulletsLoadedPerBulletItem)
		{
			if (!FirearmsAmmoUtility.TryLoadBulletsWithoutConsuming(inventory, ammoSlot, bulletsLoaded, bulletsLoadedPerBulletItem))
			{
				return false;
			}
			int bulletItemsRequired = FirearmsAmmoUtility.BulletItemsRequired(bulletsLoaded, bulletsLoadedPerBulletItem);
			if (bulletItemsRequired > 0)
			{
				ammoSlot.TakeOut(bulletItemsRequired);
			}
			return true;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000048F4 File Offset: 0x00002AF4
		public static bool TryLoadBulletsWithoutConsuming(ItemInventoryBuffer inventory, ItemSlot ammoSlot, int bulletsLoaded, int bulletsLoadedPerBulletItem)
		{
			if (ammoSlot.Itemstack == null || !FirearmsAmmoUtility.HasEnoughBulletItems(ammoSlot, bulletsLoaded, bulletsLoadedPerBulletItem))
			{
				return false;
			}
			ItemStack ammoTemplate = ammoSlot.Itemstack.Clone();
			ammoTemplate.StackSize = 1;
			for (int count = 0; count < bulletsLoaded; count++)
			{
				FirearmsAmmoUtility.InventoryItemStackAdd(inventory, ammoTemplate);
			}
			return true;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000493C File Offset: 0x00002B3C
		public static void ApplyProjectileDisappearance(ItemStack ammo, ProjectileStats stats)
		{
			if (!FirearmsAmmoUtility.ProjectileDisappearanceEnabled)
			{
				stats.CanBeCollected = true;
				stats.DropChance = 1f;
				return;
			}
			stats.CanBeCollected = false;
			stats.DropChance = 0f;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000496A File Offset: 0x00002B6A
		private static void InventoryItemStackAdd(ItemInventoryBuffer inventory, ItemStack ammoTemplate)
		{
			inventory.Items.Add(ammoTemplate.Clone());
		}

		// Token: 0x04000052 RID: 82
		public static readonly bool RecoilEnabled = true;

		// Token: 0x04000053 RID: 83
		public static readonly bool ProjectileDisappearanceEnabled = true;
	}
}
