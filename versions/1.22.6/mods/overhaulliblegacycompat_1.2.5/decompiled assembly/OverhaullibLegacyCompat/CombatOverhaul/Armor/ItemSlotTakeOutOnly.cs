using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000146 RID: 326
	[NullableContext(1)]
	[Nullable(0)]
	public class ItemSlotTakeOutOnly : ItemSlotBagContent, IHasSlotBackpackCategory
	{
		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x000500C0 File Offset: 0x0004E2C0
		// (set) Token: 0x06001036 RID: 4150 RVA: 0x000500C8 File Offset: 0x0004E2C8
		public string ToolBagId { get; set; }

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x000500D1 File Offset: 0x0004E2D1
		// (set) Token: 0x06001038 RID: 4152 RVA: 0x000500D9 File Offset: 0x0004E2D9
		public int ToolBagIndex { get; set; }

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x000500E2 File Offset: 0x0004E2E2
		// (set) Token: 0x0600103A RID: 4154 RVA: 0x000500EA File Offset: 0x0004E2EA
		public bool CanHoldNow { get; set; }

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x000500F3 File Offset: 0x0004E2F3
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x000500FB File Offset: 0x0004E2FB
		public bool MainHand { get; set; } = true;

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x0600103D RID: 4157 RVA: 0x00050104 File Offset: 0x0004E304
		// (set) Token: 0x0600103E RID: 4158 RVA: 0x0005010C File Offset: 0x0004E30C
		public string BackpackCategoryCode { get; set; } = "takeout";

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x00050115 File Offset: 0x0004E315
		// (set) Token: 0x06001040 RID: 4160 RVA: 0x0005011D File Offset: 0x0004E31D
		public float OrderPriority { get; set; } = 0.1f;

		// Token: 0x06001041 RID: 4161 RVA: 0x00050128 File Offset: 0x0004E328
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

		// Token: 0x06001042 RID: 4162 RVA: 0x0005019A File Offset: 0x0004E39A
		public override bool CanTakeFrom(ItemSlot sourceSlot, EnumMergePriority priority = 0)
		{
			return this.CanHoldNow;
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x000501A2 File Offset: 0x0004E3A2
		public override bool CanHold(ItemSlot sourceSlot)
		{
			return this.CanHoldNow;
		}
	}
}
