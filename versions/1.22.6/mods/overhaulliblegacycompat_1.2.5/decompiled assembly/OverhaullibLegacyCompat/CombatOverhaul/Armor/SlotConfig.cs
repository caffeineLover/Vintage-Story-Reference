using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200014F RID: 335
	[NullableContext(1)]
	[Nullable(0)]
	public class SlotConfig : IHasSlotBackpackCategory
	{
		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x00051560 File Offset: 0x0004F760
		// (set) Token: 0x060010AB RID: 4267 RVA: 0x00051568 File Offset: 0x0004F768
		public ItemTagRule[] CanHoldItemTags { get; set; } = Array.Empty<ItemTagRule>();

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x00051571 File Offset: 0x0004F771
		// (set) Token: 0x060010AD RID: 4269 RVA: 0x00051579 File Offset: 0x0004F779
		public BlockTagRule[] CanHoldBlockTags { get; set; } = Array.Empty<BlockTagRule>();

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060010AE RID: 4270 RVA: 0x00051582 File Offset: 0x0004F782
		// (set) Token: 0x060010AF RID: 4271 RVA: 0x0005158A File Offset: 0x0004F78A
		public string[] CanHoldWildcards { get; set; } = Array.Empty<string>();

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x00051593 File Offset: 0x0004F793
		// (set) Token: 0x060010B1 RID: 4273 RVA: 0x0005159B File Offset: 0x0004F79B
		public string[] CanHoldItemAttributes { get; set; } = Array.Empty<string>();

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x000515A4 File Offset: 0x0004F7A4
		// (set) Token: 0x060010B3 RID: 4275 RVA: 0x000515AC File Offset: 0x0004F7AC
		[Nullable(2)]
		public string SlotColor { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x000515B5 File Offset: 0x0004F7B5
		// (set) Token: 0x060010B5 RID: 4277 RVA: 0x000515BD File Offset: 0x0004F7BD
		[Nullable(2)]
		public string SlotsIcon { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x000515C6 File Offset: 0x0004F7C6
		// (set) Token: 0x060010B7 RID: 4279 RVA: 0x000515CE File Offset: 0x0004F7CE
		public string BackpackCategoryCode { get; set; } = "";

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x000515D7 File Offset: 0x0004F7D7
		// (set) Token: 0x060010B9 RID: 4281 RVA: 0x000515DF File Offset: 0x0004F7DF
		public float OrderPriority { get; set; } = 1f;

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x000515E8 File Offset: 0x0004F7E8
		// (set) Token: 0x060010BB RID: 4283 RVA: 0x000515F0 File Offset: 0x0004F7F0
		public int SlotsNumber { get; set; }

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x000515F9 File Offset: 0x0004F7F9
		// (set) Token: 0x060010BD RID: 4285 RVA: 0x00051601 File Offset: 0x0004F801
		public int StoredItemLightLevelOffset { get; set; }

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x0005160A File Offset: 0x0004F80A
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x00051612 File Offset: 0x0004F812
		public bool HandleHotkey { get; set; }

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x0005161B File Offset: 0x0004F81B
		// (set) Token: 0x060010C1 RID: 4289 RVA: 0x00051623 File Offset: 0x0004F823
		public bool DisplayInToolDialog { get; set; }

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x0005162C File Offset: 0x0004F82C
		// (set) Token: 0x060010C3 RID: 4291 RVA: 0x00051634 File Offset: 0x0004F834
		public string SlotVariant { get; set; } = "bag_slot";

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x0005163D File Offset: 0x0004F83D
		// (set) Token: 0x060010C5 RID: 4293 RVA: 0x00051645 File Offset: 0x0004F845
		public string SlotStateVariant { get; set; } = "bag_slot_state";

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x0005164E File Offset: 0x0004F84E
		// (set) Token: 0x060010C7 RID: 4295 RVA: 0x00051656 File Offset: 0x0004F856
		public string EmptyStateCode { get; set; } = "empty";

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x0005165F File Offset: 0x0004F85F
		// (set) Token: 0x060010C9 RID: 4297 RVA: 0x00051667 File Offset: 0x0004F867
		public string FullStateCode { get; set; } = "full";

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x00051670 File Offset: 0x0004F870
		// (set) Token: 0x060010CB RID: 4299 RVA: 0x00051678 File Offset: 0x0004F878
		public string SlotMetalVariant { get; set; } = "bag_slot_metal";

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x00051681 File Offset: 0x0004F881
		// (set) Token: 0x060010CD RID: 4301 RVA: 0x00051689 File Offset: 0x0004F889
		public string SlotLeatherVariant { get; set; } = "bag_slot_leather";

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060010CE RID: 4302 RVA: 0x00051692 File Offset: 0x0004F892
		// (set) Token: 0x060010CF RID: 4303 RVA: 0x0005169A File Offset: 0x0004F89A
		public string SlotWoodVariant { get; set; } = "bag_slot_wood";

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x000516A3 File Offset: 0x0004F8A3
		// (set) Token: 0x060010D1 RID: 4305 RVA: 0x000516AB File Offset: 0x0004F8AB
		public bool SetVariants { get; set; }

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060010D2 RID: 4306 RVA: 0x000516B4 File Offset: 0x0004F8B4
		// (set) Token: 0x060010D3 RID: 4307 RVA: 0x000516BC File Offset: 0x0004F8BC
		public bool SetMaterialVariants { get; set; }

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060010D4 RID: 4308 RVA: 0x000516C5 File Offset: 0x0004F8C5
		// (set) Token: 0x060010D5 RID: 4309 RVA: 0x000516CD File Offset: 0x0004F8CD
		protected string[][] CanHoldItemTagsNames { get; set; }

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x000516D6 File Offset: 0x0004F8D6
		// (set) Token: 0x060010D7 RID: 4311 RVA: 0x000516DE File Offset: 0x0004F8DE
		protected string[][] CanHoldBlockTagsNames { get; set; }

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060010D8 RID: 4312 RVA: 0x000516E7 File Offset: 0x0004F8E7
		// (set) Token: 0x060010D9 RID: 4313 RVA: 0x000516EF File Offset: 0x0004F8EF
		protected bool Resolved { get; set; }

		// Token: 0x060010DA RID: 4314 RVA: 0x000516F8 File Offset: 0x0004F8F8
		public SlotConfig(string[][] itemTags, string[][] blockTags)
		{
			this.CanHoldItemTagsNames = itemTags;
			this.CanHoldBlockTagsNames = blockTags;
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x000517A8 File Offset: 0x0004F9A8
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

		// Token: 0x060010DC RID: 4316 RVA: 0x00051874 File Offset: 0x0004FA74
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
