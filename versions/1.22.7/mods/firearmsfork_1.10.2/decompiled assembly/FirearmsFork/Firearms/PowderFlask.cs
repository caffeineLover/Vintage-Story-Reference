using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Firearms
{
	// Token: 0x02000011 RID: 17
	[NullableContext(1)]
	[Nullable(0)]
	public class PowderFlask : Item
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000099 RID: 153 RVA: 0x0000441F File Offset: 0x0000261F
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00004427 File Offset: 0x00002627
		public PowderFlaskStats Stats { get; set; } = new PowderFlaskStats();

		// Token: 0x0600009B RID: 155 RVA: 0x00004430 File Offset: 0x00002630
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this.Stats = this.Attributes.AsObject<PowderFlaskStats>(null);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000444B File Offset: 0x0000264B
		public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe)
		{
			base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
			if (outputSlot.Itemstack == null)
			{
				return;
			}
			this.SetDurability(outputSlot.Itemstack, 0);
			outputSlot.MarkDirty();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004472 File Offset: 0x00002672
		public override void DamageItem(IWorldAccessor world, Entity byEntity, ItemSlot itemSlot, int amount = 1, bool destroyOnZeroDurability = true)
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004474 File Offset: 0x00002674
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
			if (handling != 0)
			{
				return;
			}
			if (this.GetRemainingDurability(slot.Itemstack) == this.GetMaxDurability(slot.Itemstack))
			{
				return;
			}
			handling = 1;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000044AC File Offset: 0x000026AC
		public override bool OnHeldInteractStep(float secondsPassed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSelection, EntitySelection entitySel)
		{
			base.OnHeldInteractStep(secondsPassed, slot, byEntity, blockSelection, entitySel);
			if (this.GetRemainingDurability(slot.Itemstack) >= this.GetMaxDurability(slot.Itemstack))
			{
				return false;
			}
			ItemSlot powderSlot = null;
			EntityPlayer player = byEntity as EntityPlayer;
			foreach (ItemSlot hotbarSlot in player.Player.InventoryManager.GetOwnInventory("hotbar"))
			{
				AssetLocation assetLocation;
				if (hotbarSlot == null)
				{
					assetLocation = null;
				}
				else
				{
					ItemStack itemstack = hotbarSlot.Itemstack;
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
				if (assetLocation != null && WildcardUtil.Match(this.Stats.PowderWildcard, hotbarSlot.Itemstack.Item.Code.ToString()))
				{
					powderSlot = hotbarSlot;
					break;
				}
			}
			if (powderSlot == null)
			{
				foreach (ItemSlot backpackSlot in player.Player.InventoryManager.GetOwnInventory("backpack"))
				{
					AssetLocation assetLocation2;
					if (backpackSlot == null)
					{
						assetLocation2 = null;
					}
					else
					{
						ItemStack itemstack2 = backpackSlot.Itemstack;
						if (itemstack2 == null)
						{
							assetLocation2 = null;
						}
						else
						{
							Item item2 = itemstack2.Item;
							assetLocation2 = ((item2 != null) ? item2.Code : null);
						}
					}
					if (assetLocation2 != null && WildcardUtil.Match(this.Stats.PowderWildcard, backpackSlot.Itemstack.Item.Code.ToString()))
					{
						powderSlot = backpackSlot;
						break;
					}
				}
			}
			bool flag;
			if (powderSlot == null)
			{
				flag = (null != null);
			}
			else
			{
				ItemStack itemstack3 = powderSlot.Itemstack;
				flag = (((itemstack3 != null) ? itemstack3.Item : null) != null);
			}
			if (!flag)
			{
				return false;
			}
			powderSlot.TakeOut(1);
			this.SetDurability(slot.Itemstack, (int)GameMath.Clamp((float)this.GetRemainingDurability(slot.Itemstack) + this.Stats.DurabilityPerPowderItem, 0f, (float)this.GetMaxDurability(slot.Itemstack)));
			powderSlot.MarkDirty();
			slot.MarkDirty();
			AssetLocation sound = new AssetLocation(this.Stats.RefillSound);
			byEntity.Api.World.PlaySoundAt(sound, (byEntity as EntityPlayer).Player, (byEntity as EntityPlayer).Player, false, 16f, 1f);
			return false;
		}
	}
}
