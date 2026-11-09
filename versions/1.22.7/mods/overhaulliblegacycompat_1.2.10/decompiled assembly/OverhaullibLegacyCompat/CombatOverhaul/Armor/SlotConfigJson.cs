using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200015A RID: 346
	[NullableContext(1)]
	[Nullable(0)]
	public class SlotConfigJson
	{
		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x00052AD7 File Offset: 0x00050CD7
		// (set) Token: 0x060010EF RID: 4335 RVA: 0x00052ADF File Offset: 0x00050CDF
		public string[] CanHoldItemTags { get; set; } = Array.Empty<string>();

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x00052AE8 File Offset: 0x00050CE8
		// (set) Token: 0x060010F1 RID: 4337 RVA: 0x00052AF0 File Offset: 0x00050CF0
		public string[] CanHoldBlockTags { get; set; } = Array.Empty<string>();

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x00052AF9 File Offset: 0x00050CF9
		// (set) Token: 0x060010F3 RID: 4339 RVA: 0x00052B01 File Offset: 0x00050D01
		public string[][] CanHoldItemTagsCondition { get; set; } = Array.Empty<string[]>();

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x00052B0A File Offset: 0x00050D0A
		// (set) Token: 0x060010F5 RID: 4341 RVA: 0x00052B12 File Offset: 0x00050D12
		public string[][] CanHoldBlockTagsCondition { get; set; } = Array.Empty<string[]>();

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x00052B1B File Offset: 0x00050D1B
		// (set) Token: 0x060010F7 RID: 4343 RVA: 0x00052B23 File Offset: 0x00050D23
		public string[] CanHoldWildcards { get; set; } = Array.Empty<string>();

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060010F8 RID: 4344 RVA: 0x00052B2C File Offset: 0x00050D2C
		// (set) Token: 0x060010F9 RID: 4345 RVA: 0x00052B34 File Offset: 0x00050D34
		public string[] CanHoldItemAttributes { get; set; } = Array.Empty<string>();

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x00052B3D File Offset: 0x00050D3D
		// (set) Token: 0x060010FB RID: 4347 RVA: 0x00052B45 File Offset: 0x00050D45
		[Nullable(2)]
		public string SlotColor { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x00052B4E File Offset: 0x00050D4E
		// (set) Token: 0x060010FD RID: 4349 RVA: 0x00052B56 File Offset: 0x00050D56
		[Nullable(2)]
		public string SlotsIcon { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060010FE RID: 4350 RVA: 0x00052B5F File Offset: 0x00050D5F
		// (set) Token: 0x060010FF RID: 4351 RVA: 0x00052B67 File Offset: 0x00050D67
		public string SlotBackpackCategory { get; set; } = "";

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001100 RID: 4352 RVA: 0x00052B70 File Offset: 0x00050D70
		// (set) Token: 0x06001101 RID: 4353 RVA: 0x00052B78 File Offset: 0x00050D78
		public float CategoryOrderPriority { get; set; } = 1f;

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001102 RID: 4354 RVA: 0x00052B81 File Offset: 0x00050D81
		// (set) Token: 0x06001103 RID: 4355 RVA: 0x00052B89 File Offset: 0x00050D89
		public int SlotsNumber { get; set; }

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001104 RID: 4356 RVA: 0x00052B92 File Offset: 0x00050D92
		// (set) Token: 0x06001105 RID: 4357 RVA: 0x00052B9A File Offset: 0x00050D9A
		public int StoredItemLightLevelOffset { get; set; }

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x00052BA3 File Offset: 0x00050DA3
		// (set) Token: 0x06001107 RID: 4359 RVA: 0x00052BAB File Offset: 0x00050DAB
		public string SlotVariant { get; set; } = "bag_slot";

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001108 RID: 4360 RVA: 0x00052BB4 File Offset: 0x00050DB4
		// (set) Token: 0x06001109 RID: 4361 RVA: 0x00052BBC File Offset: 0x00050DBC
		public string SlotStateVariant { get; set; } = "bag_slot_state";

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x0600110A RID: 4362 RVA: 0x00052BC5 File Offset: 0x00050DC5
		// (set) Token: 0x0600110B RID: 4363 RVA: 0x00052BCD File Offset: 0x00050DCD
		public string EmptyStateCode { get; set; } = "empty";

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x0600110C RID: 4364 RVA: 0x00052BD6 File Offset: 0x00050DD6
		// (set) Token: 0x0600110D RID: 4365 RVA: 0x00052BDE File Offset: 0x00050DDE
		public string FullStateCode { get; set; } = "full";

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x0600110E RID: 4366 RVA: 0x00052BE7 File Offset: 0x00050DE7
		// (set) Token: 0x0600110F RID: 4367 RVA: 0x00052BEF File Offset: 0x00050DEF
		public string SlotMetalVariant { get; set; } = "bag_slot_metal";

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x00052BF8 File Offset: 0x00050DF8
		// (set) Token: 0x06001111 RID: 4369 RVA: 0x00052C00 File Offset: 0x00050E00
		public string SlotLeatherVariant { get; set; } = "bag_slot_leather";

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001112 RID: 4370 RVA: 0x00052C09 File Offset: 0x00050E09
		// (set) Token: 0x06001113 RID: 4371 RVA: 0x00052C11 File Offset: 0x00050E11
		public string SlotWoodVariant { get; set; } = "bag_slot_wood";

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001114 RID: 4372 RVA: 0x00052C1A File Offset: 0x00050E1A
		// (set) Token: 0x06001115 RID: 4373 RVA: 0x00052C22 File Offset: 0x00050E22
		public bool HandleHotkey { get; set; }

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x00052C2B File Offset: 0x00050E2B
		// (set) Token: 0x06001117 RID: 4375 RVA: 0x00052C33 File Offset: 0x00050E33
		public bool DisplayInToolDialog { get; set; }

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001118 RID: 4376 RVA: 0x00052C3C File Offset: 0x00050E3C
		// (set) Token: 0x06001119 RID: 4377 RVA: 0x00052C44 File Offset: 0x00050E44
		public bool SetVariants { get; set; }

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x0600111A RID: 4378 RVA: 0x00052C4D File Offset: 0x00050E4D
		// (set) Token: 0x0600111B RID: 4379 RVA: 0x00052C55 File Offset: 0x00050E55
		public bool SetMaterialVariants { get; set; }

		// Token: 0x0600111C RID: 4380 RVA: 0x00052C60 File Offset: 0x00050E60
		public SlotConfig ToConfig()
		{
			if (this.CanHoldItemTagsCondition.Length == 0 && this.CanHoldItemTags.Length != 0)
			{
				this.CanHoldItemTagsCondition = new string[][]
				{
					this.CanHoldItemTags
				};
			}
			if (this.CanHoldBlockTagsCondition.Length == 0 && this.CanHoldBlockTags.Length != 0)
			{
				this.CanHoldBlockTagsCondition = new string[][]
				{
					this.CanHoldBlockTags
				};
			}
			return new SlotConfig(this.CanHoldItemTagsCondition, this.CanHoldBlockTagsCondition)
			{
				CanHoldWildcards = this.CanHoldWildcards,
				CanHoldItemAttributes = this.CanHoldItemAttributes,
				SlotColor = this.SlotColor,
				SlotsIcon = this.SlotsIcon,
				BackpackCategoryCode = this.SlotBackpackCategory,
				OrderPriority = this.CategoryOrderPriority,
				SlotsNumber = this.SlotsNumber,
				StoredItemLightLevelOffset = this.StoredItemLightLevelOffset,
				SlotVariant = this.SlotVariant,
				SlotStateVariant = this.SlotStateVariant,
				EmptyStateCode = this.EmptyStateCode,
				FullStateCode = this.FullStateCode,
				SlotMetalVariant = this.SlotMetalVariant,
				SlotLeatherVariant = this.SlotLeatherVariant,
				SlotWoodVariant = this.SlotWoodVariant,
				SetVariants = this.SetVariants,
				SetMaterialVariants = this.SetMaterialVariants,
				HandleHotkey = this.HandleHotkey,
				DisplayInToolDialog = this.DisplayInToolDialog
			};
		}
	}
}
