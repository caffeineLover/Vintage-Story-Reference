using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000145 RID: 325
	[NullableContext(1)]
	[Nullable(0)]
	public class ItemSlotBagContentWithWildcardMatch : ItemSlotBagContent, IHasSlotBackpackCategory
	{
		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x0004FE92 File Offset: 0x0004E092
		// (set) Token: 0x06001027 RID: 4135 RVA: 0x0004FE9A File Offset: 0x0004E09A
		public ItemStack SourceBag { get; set; }

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001028 RID: 4136 RVA: 0x0004FEA3 File Offset: 0x0004E0A3
		// (set) Token: 0x06001029 RID: 4137 RVA: 0x0004FEAB File Offset: 0x0004E0AB
		public SlotConfig Config { get; set; } = new SlotConfig(Array.Empty<string[]>(), Array.Empty<string[]>());

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x0600102A RID: 4138 RVA: 0x0004FEB4 File Offset: 0x0004E0B4
		public string BackpackCategoryCode
		{
			get
			{
				return this.Config.BackpackCategoryCode;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x0004FEC1 File Offset: 0x0004E0C1
		public float OrderPriority
		{
			get
			{
				return this.Config.OrderPriority;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x0004FECE File Offset: 0x0004E0CE
		// (set) Token: 0x0600102D RID: 4141 RVA: 0x0004FED6 File Offset: 0x0004E0D6
		public string ToolBagId { get; set; }

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x0600102E RID: 4142 RVA: 0x0004FEDF File Offset: 0x0004E0DF
		// (set) Token: 0x0600102F RID: 4143 RVA: 0x0004FEE7 File Offset: 0x0004E0E7
		public int ToolBagIndex { get; set; }

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001030 RID: 4144 RVA: 0x0004FEF0 File Offset: 0x0004E0F0
		// (set) Token: 0x06001031 RID: 4145 RVA: 0x0004FEF8 File Offset: 0x0004E0F8
		public bool MainHand { get; set; } = true;

		// Token: 0x06001032 RID: 4146 RVA: 0x0004FF04 File Offset: 0x0004E104
		public ItemSlotBagContentWithWildcardMatch(InventoryBase inventory, int BagIndex, int slotIndex, EnumItemStorageFlags storageType, ItemStack sourceBag, [Nullable(2)] string color = null) : base(inventory, BagIndex, slotIndex, storageType)
		{
			this.HexBackgroundColor = color;
			this.SourceBag = sourceBag;
			Item item = sourceBag.Item;
			string text;
			if (item == null)
			{
				text = null;
			}
			else
			{
				AssetLocation code = item.Code;
				text = ((code != null) ? code.ToString() : null);
			}
			this.ToolBagId = (text ?? "");
			this.ToolBagIndex = BagIndex;
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0004FF7D File Offset: 0x0004E17D
		public override bool CanTakeFrom(ItemSlot sourceSlot, EnumMergePriority priority = 0)
		{
			return this.CanHold(sourceSlot) && base.CanTakeFrom(sourceSlot, priority);
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0004FF94 File Offset: 0x0004E194
		public override bool CanHold(ItemSlot sourceSlot)
		{
			AssetLocation assetLocation;
			if (sourceSlot == null)
			{
				assetLocation = null;
			}
			else
			{
				ItemStack itemstack = sourceSlot.Itemstack;
				if (itemstack == null)
				{
					assetLocation = null;
				}
				else
				{
					CollectibleObject collectible = itemstack.Collectible;
					assetLocation = ((collectible != null) ? collectible.Code : null);
				}
			}
			if (assetLocation != null)
			{
				bool flag = WildcardUtil.Match(this.Config.CanHoldWildcards, sourceSlot.Itemstack.Collectible.Code.Path);
				bool matchWithDomain = WildcardUtil.Match(this.Config.CanHoldWildcards, sourceSlot.Itemstack.Collectible.Code.ToString());
				bool matchWithTags = false;
				ItemStack itemstack2 = sourceSlot.Itemstack;
				if (((itemstack2 != null) ? itemstack2.Item : null) != null && this.Config.CanHoldItemTags.Length != 0)
				{
					matchWithTags = ItemTagRule.ContainsAllFromAtLeastOne(sourceSlot.Itemstack.Item.Tags, this.Config.CanHoldItemTags);
				}
				ItemStack itemstack3 = sourceSlot.Itemstack;
				if (((itemstack3 != null) ? itemstack3.Block : null) != null && this.Config.CanHoldBlockTags.Length != 0 && !matchWithTags)
				{
					matchWithTags = BlockTagRule.ContainsAllFromAtLeastOne(sourceSlot.Itemstack.Block.Tags, this.Config.CanHoldBlockTags);
				}
				bool matchWithAttributes = this.Config.MatchesItemAttributes(sourceSlot.Itemstack);
				return flag || matchWithDomain || matchWithTags || matchWithAttributes;
			}
			return false;
		}
	}
}
