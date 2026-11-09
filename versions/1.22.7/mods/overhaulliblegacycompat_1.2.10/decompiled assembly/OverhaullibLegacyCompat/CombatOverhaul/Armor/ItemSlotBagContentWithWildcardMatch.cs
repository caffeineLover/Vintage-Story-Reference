using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000151 RID: 337
	[NullableContext(1)]
	[Nullable(0)]
	public class ItemSlotBagContentWithWildcardMatch : ItemSlotBagContent, IHasSlotBackpackCategory
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x0005179A File Offset: 0x0004F99A
		// (set) Token: 0x0600109B RID: 4251 RVA: 0x000517A2 File Offset: 0x0004F9A2
		public ItemStack SourceBag { get; set; }

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x0600109C RID: 4252 RVA: 0x000517AB File Offset: 0x0004F9AB
		// (set) Token: 0x0600109D RID: 4253 RVA: 0x000517B3 File Offset: 0x0004F9B3
		public SlotConfig Config { get; set; } = new SlotConfig(Array.Empty<string[]>(), Array.Empty<string[]>());

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x000517BC File Offset: 0x0004F9BC
		public string BackpackCategoryCode
		{
			get
			{
				return this.Config.BackpackCategoryCode;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x000517C9 File Offset: 0x0004F9C9
		public float OrderPriority
		{
			get
			{
				return this.Config.OrderPriority;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x000517D6 File Offset: 0x0004F9D6
		// (set) Token: 0x060010A1 RID: 4257 RVA: 0x000517DE File Offset: 0x0004F9DE
		public string ToolBagId { get; set; }

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x000517E7 File Offset: 0x0004F9E7
		// (set) Token: 0x060010A3 RID: 4259 RVA: 0x000517EF File Offset: 0x0004F9EF
		public int ToolBagIndex { get; set; }

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x000517F8 File Offset: 0x0004F9F8
		// (set) Token: 0x060010A5 RID: 4261 RVA: 0x00051800 File Offset: 0x0004FA00
		public bool MainHand { get; set; } = true;

		// Token: 0x060010A6 RID: 4262 RVA: 0x0005180C File Offset: 0x0004FA0C
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

		// Token: 0x060010A7 RID: 4263 RVA: 0x00051885 File Offset: 0x0004FA85
		public override bool CanTakeFrom(ItemSlot sourceSlot, EnumMergePriority priority = 0)
		{
			return this.CanHold(sourceSlot) && base.CanTakeFrom(sourceSlot, priority);
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0005189C File Offset: 0x0004FA9C
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
