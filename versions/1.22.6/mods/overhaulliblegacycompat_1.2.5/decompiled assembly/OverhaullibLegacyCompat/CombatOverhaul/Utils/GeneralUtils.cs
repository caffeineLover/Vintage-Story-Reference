using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Vanity;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.Common;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000024 RID: 36
	[NullableContext(1)]
	[Nullable(0)]
	public static class GeneralUtils
	{
		// Token: 0x06000296 RID: 662 RVA: 0x0000AD40 File Offset: 0x00008F40
		public static long MarkItemStack(ItemSlot slot)
		{
			ItemStack stack = slot.Itemstack;
			if (stack == null)
			{
				return 0L;
			}
			if (stack.Attributes == null)
			{
				stack.Attributes = new TreeAttribute();
			}
			long? id = stack.Attributes.TryGetLong("item-stack-uid");
			if (id == null)
			{
				long newId = DateTime.UtcNow.Ticks;
				stack.Attributes.SetLong("item-stack-uid", newId);
				slot.MarkDirty();
				id = new long?(newId);
			}
			return id.Value;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000ADBC File Offset: 0x00008FBC
		public static long GetItemMark(ItemSlot slot)
		{
			ItemStack stack = slot.Itemstack;
			if (stack == null)
			{
				return 0L;
			}
			if (stack.Attributes == null)
			{
				return 0L;
			}
			long? id = stack.Attributes.TryGetLong("item-stack-uid");
			if (id == null)
			{
				return 0L;
			}
			return id.Value;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000AE05 File Offset: 0x00009005
		[NullableContext(2)]
		public static InventoryPlayerBackpacks GetBackpackInventory(IPlayer player)
		{
			return ((player != null) ? player.InventoryManager.GetOwnInventory("backpack") : null) as InventoryPlayerBackpacks;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000AE22 File Offset: 0x00009022
		[NullableContext(2)]
		public static InventoryCharacter GetCharacterInventory(IPlayer player)
		{
			return ((player != null) ? player.InventoryManager.GetOwnInventory("character") : null) as InventoryCharacter;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000AE3F File Offset: 0x0000903F
		[NullableContext(2)]
		public static VanityInventory GetVanityInventory(IPlayer player)
		{
			return ((player != null) ? player.InventoryManager.GetOwnInventory("combatoverhaul:vanity") : null) as VanityInventory;
		}

		// Token: 0x04000103 RID: 259
		public const string ItemStackIdAttribute = "item-stack-uid";
	}
}
