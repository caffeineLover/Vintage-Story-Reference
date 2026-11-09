using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Armor;
using CombatOverhaul.RangedSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Common;

namespace Firearms
{
	// Token: 0x02000027 RID: 39
	[NullableContext(1)]
	[Nullable(0)]
	public class PistolServer : MuzzleloaderServer, IRangedWeaponIsLoaded
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x00009902 File Offset: 0x00007B02
		public PistolServer(ICoreServerAPI api, Item item) : base(api, item)
		{
			this.PistolStats = item.Attributes.AsObject<PistolStats>(null);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00009920 File Offset: 0x00007B20
		public override bool Shoot(IServerPlayer player, ItemSlot slot, ShotPacket packet, Entity shooter)
		{
			if (!base.Shoot(player, slot, packet, shooter))
			{
				return false;
			}
			ItemSlot replaceWith = this.FindReplacementSlot(player);
			if (replaceWith == null)
			{
				return true;
			}
			this.ReplaceWeapon(player, replaceWith, slot);
			return true;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00009953 File Offset: 0x00007B53
		public bool GetIsLoaded(EntityPlayer player, ItemSlot slot)
		{
			this.Inventory.Read(slot, "magazine");
			bool result = this.Inventory.Items.Count > 0;
			this.Inventory.Clear();
			return result;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00009984 File Offset: 0x00007B84
		protected virtual void ReplaceWeapon(IServerPlayer player, ItemSlot from, ItemSlot to)
		{
			DummySlot temporarySlot = new DummySlot();
			if (to.TryPutInto(player.Entity.Api.World, temporarySlot, 1) == 0)
			{
				return;
			}
			to.MarkDirty();
			if (from.TryPutInto(player.Entity.Api.World, to, 1) == 0)
			{
				temporarySlot.TryPutInto(player.Entity.Api.World, from, 1);
				return;
			}
			from.MarkDirty();
			if (player.Entity.TryGiveItemStack(temporarySlot.Itemstack))
			{
				return;
			}
			player.Entity.Api.World.SpawnItemEntity(temporarySlot.Itemstack, player.Entity.Pos.XYZ, player.Entity.Pos.Motion);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009A44 File Offset: 0x00007C44
		[return: Nullable(2)]
		protected virtual ItemSlot FindReplacementSlot(IServerPlayer player)
		{
			InventoryPlayerBackpacks inventory = this.GetBackpackInventory(player);
			if (inventory == null)
			{
				return null;
			}
			foreach (ItemSlot slot in inventory)
			{
				ItemSlotBagContentWithWildcardMatch bagSlot = slot as ItemSlotBagContentWithWildcardMatch;
				if (bagSlot != null && this.MatchBagSlot(bagSlot))
				{
					IRangedWeaponIsLoaded loadedWeapon = bagSlot.Itemstack.Collectible as IRangedWeaponIsLoaded;
					if (loadedWeapon != null && loadedWeapon.GetIsLoaded(player.Entity, bagSlot))
					{
						return slot;
					}
				}
			}
			foreach (ItemSlot slot2 in inventory)
			{
				ItemSlotBagContentWithWildcardMatch bagSlot2 = slot2 as ItemSlotBagContentWithWildcardMatch;
				if (bagSlot2 != null && this.MatchBagSlot(bagSlot2))
				{
					return slot2;
				}
			}
			return null;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009B24 File Offset: 0x00007D24
		protected virtual bool MatchBagSlot(ItemSlotBagContentWithWildcardMatch bagSlot)
		{
			if (bagSlot.Empty)
			{
				return false;
			}
			if (this.PistolStats.ReplacementBackpackCategoryWildcards.Length != 0 && !Enumerable.Any<string>(this.PistolStats.ReplacementBackpackCategoryWildcards, (string wildcard) => WildcardUtil.Match(wildcard, bagSlot.BackpackCategoryCode)))
			{
				return false;
			}
			if (this.PistolStats.ReplacementItemWildcards.Length != 0)
			{
				PistolServer.<>c__DisplayClass6_1 CS$<>8__locals2 = new PistolServer.<>c__DisplayClass6_1();
				PistolServer.<>c__DisplayClass6_1 CS$<>8__locals3 = CS$<>8__locals2;
				CollectibleObject collectible = bagSlot.Itemstack.Collectible;
				string text;
				if (collectible == null)
				{
					text = null;
				}
				else
				{
					AssetLocation code = collectible.Code;
					text = ((code != null) ? code.ToString() : null);
				}
				CS$<>8__locals3.itemCode = (text ?? "");
				if (!Enumerable.Any<string>(this.PistolStats.ReplacementItemWildcards, (string wildcard) => WildcardUtil.Match(wildcard, CS$<>8__locals2.itemCode)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00009BE7 File Offset: 0x00007DE7
		[return: Nullable(2)]
		protected virtual InventoryPlayerBackpacks GetBackpackInventory(IServerPlayer player)
		{
			return player.InventoryManager.GetOwnInventory("backpack") as InventoryPlayerBackpacks;
		}

		// Token: 0x040000E7 RID: 231
		protected readonly PistolStats PistolStats;
	}
}
