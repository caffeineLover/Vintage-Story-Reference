using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200015B RID: 347
	[NullableContext(1)]
	[Nullable(0)]
	public class SlotConfig : IHasSlotBackpackCategory
	{
		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x0600111E RID: 4382 RVA: 0x00052E68 File Offset: 0x00051068
		// (set) Token: 0x0600111F RID: 4383 RVA: 0x00052E70 File Offset: 0x00051070
		public ItemTagRule[] CanHoldItemTags { get; set; } = Array.Empty<ItemTagRule>();

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001120 RID: 4384 RVA: 0x00052E79 File Offset: 0x00051079
		// (set) Token: 0x06001121 RID: 4385 RVA: 0x00052E81 File Offset: 0x00051081
		public BlockTagRule[] CanHoldBlockTags { get; set; } = Array.Empty<BlockTagRule>();

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06001122 RID: 4386 RVA: 0x00052E8A File Offset: 0x0005108A
		// (set) Token: 0x06001123 RID: 4387 RVA: 0x00052E92 File Offset: 0x00051092
		public string[] CanHoldWildcards { get; set; } = Array.Empty<string>();

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001124 RID: 4388 RVA: 0x00052E9B File Offset: 0x0005109B
		// (set) Token: 0x06001125 RID: 4389 RVA: 0x00052EA3 File Offset: 0x000510A3
		public string[] CanHoldItemAttributes { get; set; } = Array.Empty<string>();

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001126 RID: 4390 RVA: 0x00052EAC File Offset: 0x000510AC
		// (set) Token: 0x06001127 RID: 4391 RVA: 0x00052EB4 File Offset: 0x000510B4
		[Nullable(2)]
		public string SlotColor { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001128 RID: 4392 RVA: 0x00052EBD File Offset: 0x000510BD
		// (set) Token: 0x06001129 RID: 4393 RVA: 0x00052EC5 File Offset: 0x000510C5
		[Nullable(2)]
		public string SlotsIcon { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x0600112A RID: 4394 RVA: 0x00052ECE File Offset: 0x000510CE
		// (set) Token: 0x0600112B RID: 4395 RVA: 0x00052ED6 File Offset: 0x000510D6
		public string BackpackCategoryCode { get; set; } = "";

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x00052EDF File Offset: 0x000510DF
		// (set) Token: 0x0600112D RID: 4397 RVA: 0x00052EE7 File Offset: 0x000510E7
		public float OrderPriority { get; set; } = 1f;

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x00052EF0 File Offset: 0x000510F0
		// (set) Token: 0x0600112F RID: 4399 RVA: 0x00052EF8 File Offset: 0x000510F8
		public int SlotsNumber { get; set; }

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001130 RID: 4400 RVA: 0x00052F01 File Offset: 0x00051101
		// (set) Token: 0x06001131 RID: 4401 RVA: 0x00052F09 File Offset: 0x00051109
		public int StoredItemLightLevelOffset { get; set; }

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001132 RID: 4402 RVA: 0x00052F12 File Offset: 0x00051112
		// (set) Token: 0x06001133 RID: 4403 RVA: 0x00052F1A File Offset: 0x0005111A
		public bool HandleHotkey { get; set; }

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x00052F23 File Offset: 0x00051123
		// (set) Token: 0x06001135 RID: 4405 RVA: 0x00052F2B File Offset: 0x0005112B
		public bool DisplayInToolDialog { get; set; }

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001136 RID: 4406 RVA: 0x00052F34 File Offset: 0x00051134
		// (set) Token: 0x06001137 RID: 4407 RVA: 0x00052F3C File Offset: 0x0005113C
		public string SlotVariant { get; set; } = "bag_slot";

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001138 RID: 4408 RVA: 0x00052F45 File Offset: 0x00051145
		// (set) Token: 0x06001139 RID: 4409 RVA: 0x00052F4D File Offset: 0x0005114D
		public string SlotStateVariant { get; set; } = "bag_slot_state";

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x0600113A RID: 4410 RVA: 0x00052F56 File Offset: 0x00051156
		// (set) Token: 0x0600113B RID: 4411 RVA: 0x00052F5E File Offset: 0x0005115E
		public string EmptyStateCode { get; set; } = "empty";

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x0600113C RID: 4412 RVA: 0x00052F67 File Offset: 0x00051167
		// (set) Token: 0x0600113D RID: 4413 RVA: 0x00052F6F File Offset: 0x0005116F
		public string FullStateCode { get; set; } = "full";

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x0600113E RID: 4414 RVA: 0x00052F78 File Offset: 0x00051178
		// (set) Token: 0x0600113F RID: 4415 RVA: 0x00052F80 File Offset: 0x00051180
		public string SlotMetalVariant { get; set; } = "bag_slot_metal";

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x00052F89 File Offset: 0x00051189
		// (set) Token: 0x06001141 RID: 4417 RVA: 0x00052F91 File Offset: 0x00051191
		public string SlotLeatherVariant { get; set; } = "bag_slot_leather";

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x00052F9A File Offset: 0x0005119A
		// (set) Token: 0x06001143 RID: 4419 RVA: 0x00052FA2 File Offset: 0x000511A2
		public string SlotWoodVariant { get; set; } = "bag_slot_wood";

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x00052FAB File Offset: 0x000511AB
		// (set) Token: 0x06001145 RID: 4421 RVA: 0x00052FB3 File Offset: 0x000511B3
		public bool SetVariants { get; set; }

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x00052FBC File Offset: 0x000511BC
		// (set) Token: 0x06001147 RID: 4423 RVA: 0x00052FC4 File Offset: 0x000511C4
		public bool SetMaterialVariants { get; set; }

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x00052FCD File Offset: 0x000511CD
		// (set) Token: 0x06001149 RID: 4425 RVA: 0x00052FD5 File Offset: 0x000511D5
		protected string[][] CanHoldItemTagsNames { get; set; }

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x00052FDE File Offset: 0x000511DE
		// (set) Token: 0x0600114B RID: 4427 RVA: 0x00052FE6 File Offset: 0x000511E6
		protected string[][] CanHoldBlockTagsNames { get; set; }

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x00052FEF File Offset: 0x000511EF
		// (set) Token: 0x0600114D RID: 4429 RVA: 0x00052FF7 File Offset: 0x000511F7
		protected bool Resolved { get; set; }

		// Token: 0x0600114E RID: 4430 RVA: 0x00053000 File Offset: 0x00051200
		public SlotConfig(string[][] itemTags, string[][] blockTags)
		{
			this.CanHoldItemTagsNames = itemTags;
			this.CanHoldBlockTagsNames = blockTags;
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x000530B0 File Offset: 0x000512B0
		public void Resolve(ICoreAPI api)
		{
			if (this.Resolved)
			{
				return;
			}
			this.Resolved = true;
			this.CanHoldItemTags = Enumerable.ToArray<ItemTagRule>(Enumerable.Where<ItemTagRule>(Enumerable.Select<string[], ItemTagRule>(this.CanHoldItemTagsNames, (string[] tags) => new ItemTagRule(api, tags)), (ItemTagRule tags) => tags != ItemTagRule.Empty));
			this.CanHoldBlockTags = Enumerable.ToArray<BlockTagRule>(Enumerable.Where<BlockTagRule>(Enumerable.Select<string[], BlockTagRule>(this.CanHoldBlockTagsNames, (string[] tags) => new BlockTagRule(api, tags)), (BlockTagRule tags) => tags != BlockTagRule.Empty));
			this.CanHoldItemTagsNames = Array.Empty<string[]>();
			this.CanHoldBlockTagsNames = Array.Empty<string[]>();
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x0005317C File Offset: 0x0005137C
		[NullableContext(2)]
		public bool MatchesItemAttributes(ItemStack stack)
		{
			if (this.CanHoldItemAttributes.Length != 0)
			{
				ItemStack stack2 = stack;
				bool flag;
				if (stack2 == null)
				{
					flag = (null != null);
				}
				else
				{
					Item item = stack2.Item;
					flag = (((item != null) ? item.Attributes : null) != null);
				}
				if (flag)
				{
					return Enumerable.Any<string>(this.CanHoldItemAttributes, delegate(string attribute)
					{
						JsonObject attributes = stack.Item.Attributes;
						return attributes != null && attributes[attribute].AsBool(false);
					});
				}
			}
			return false;
		}
	}
}
