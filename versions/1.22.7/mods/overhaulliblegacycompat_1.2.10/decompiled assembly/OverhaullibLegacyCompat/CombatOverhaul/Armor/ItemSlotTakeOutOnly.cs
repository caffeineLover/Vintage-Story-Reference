using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000152 RID: 338
	[NullableContext(1)]
	[Nullable(0)]
	public class ItemSlotTakeOutOnly : ItemSlotBagContent, IHasSlotBackpackCategory
	{
		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060010A9 RID: 4265 RVA: 0x000519C8 File Offset: 0x0004FBC8
		// (set) Token: 0x060010AA RID: 4266 RVA: 0x000519D0 File Offset: 0x0004FBD0
		public string ToolBagId { get; set; }

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060010AB RID: 4267 RVA: 0x000519D9 File Offset: 0x0004FBD9
		// (set) Token: 0x060010AC RID: 4268 RVA: 0x000519E1 File Offset: 0x0004FBE1
		public int ToolBagIndex { get; set; }

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x000519EA File Offset: 0x0004FBEA
		// (set) Token: 0x060010AE RID: 4270 RVA: 0x000519F2 File Offset: 0x0004FBF2
		public bool CanHoldNow { get; set; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x000519FB File Offset: 0x0004FBFB
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x00051A03 File Offset: 0x0004FC03
		public bool MainHand { get; set; } = true;

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x00051A0C File Offset: 0x0004FC0C
		// (set) Token: 0x060010B2 RID: 4274 RVA: 0x00051A14 File Offset: 0x0004FC14
		public string BackpackCategoryCode { get; set; } = "takeout";

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x00051A1D File Offset: 0x0004FC1D
		// (set) Token: 0x060010B4 RID: 4276 RVA: 0x00051A25 File Offset: 0x0004FC25
		public float OrderPriority { get; set; } = 0.1f;

		// Token: 0x060010B5 RID: 4277 RVA: 0x00051A30 File Offset: 0x0004FC30
		public ItemSlotTakeOutOnly(InventoryBase inventory, int BagIndex, int SlotIndex, EnumItemStorageFlags storageType, ItemStack sourceBag, [Nullable(2)] string color = null) : base(inventory, BagIndex, SlotIndex, storageType)
		{
			this.HexBackgroundColor = color;
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

		// Token: 0x060010B6 RID: 4278 RVA: 0x00051AA2 File Offset: 0x0004FCA2
		public override bool CanTakeFrom(ItemSlot sourceSlot, EnumMergePriority priority = 0)
		{
			return this.CanHoldNow;
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x00051AAA File Offset: 0x0004FCAA
		public override bool CanHold(ItemSlot sourceSlot)
		{
			return this.CanHoldNow;
		}
	}
}
