using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul
{
	// Token: 0x02000018 RID: 24
	[NullableContext(1)]
	[Nullable(0)]
	internal static class NightVisionDeviceUtil
	{
		// Token: 0x06000156 RID: 342 RVA: 0x00007590 File Offset: 0x00005790
		public static int GetBestNightVisionSlotIndex(IInventory inventory)
		{
			if (inventory.Count == 0)
			{
				return -1;
			}
			if (NightVisionDeviceUtil.IsFueledNightVisionSlot(NightVisionDeviceUtil.GetSlot(inventory, 12)))
			{
				return 12;
			}
			for (int index = 0; index < inventory.Count; index++)
			{
				if (index != 12 && NightVisionDeviceUtil.IsFueledNightVisionSlot(inventory[index]))
				{
					return index;
				}
			}
			if (NightVisionDeviceUtil.IsNightVisionSlot(NightVisionDeviceUtil.GetSlot(inventory, 12)))
			{
				return 12;
			}
			for (int index2 = 0; index2 < inventory.Count; index2++)
			{
				if (index2 != 12 && NightVisionDeviceUtil.IsNightVisionSlot(inventory[index2]))
				{
					return index2;
				}
			}
			return -1;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00007618 File Offset: 0x00005818
		public static int GetNightVisionSlotIndexOrDefault(IInventory inventory)
		{
			int index = NightVisionDeviceUtil.GetBestNightVisionSlotIndex(inventory);
			if (index >= 0)
			{
				return index;
			}
			if (inventory.Count <= 12)
			{
				return Math.Max(0, inventory.Count - 1);
			}
			return 12;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007650 File Offset: 0x00005850
		[return: Nullable(2)]
		public static ItemSlot GetBestNightVisionSlot(IInventory inventory)
		{
			int index = NightVisionDeviceUtil.GetBestNightVisionSlotIndex(inventory);
			return NightVisionDeviceUtil.GetSlot(inventory, index);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000766B File Offset: 0x0000586B
		[NullableContext(2)]
		public static bool IsNightVisionSlot(ItemSlot slot)
		{
			object obj;
			if (slot == null)
			{
				obj = null;
			}
			else
			{
				ItemStack itemstack = slot.Itemstack;
				obj = ((itemstack != null) ? itemstack.Collectible : null);
			}
			return obj is ItemNightvisiondevice;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007690 File Offset: 0x00005890
		[NullableContext(2)]
		public static bool IsFueledNightVisionSlot(ItemSlot slot)
		{
			ItemStack stack = (slot != null) ? slot.Itemstack : null;
			ItemNightvisiondevice device = ((stack != null) ? stack.Collectible : null) as ItemNightvisiondevice;
			return device != null && device.GetFuelHours(stack) > 0.0;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000076D3 File Offset: 0x000058D3
		[return: Nullable(2)]
		private static ItemSlot GetSlot(IInventory inventory, int index)
		{
			if (index < 0 || index >= inventory.Count)
			{
				return null;
			}
			return inventory[index];
		}

		// Token: 0x0400009E RID: 158
		public const int VanillaNightVisionSlot = 12;
	}
}
