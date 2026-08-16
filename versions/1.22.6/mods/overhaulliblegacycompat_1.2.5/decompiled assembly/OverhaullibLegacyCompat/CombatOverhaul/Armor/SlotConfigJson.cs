using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200014E RID: 334
	[NullableContext(1)]
	[Nullable(0)]
	public class SlotConfigJson
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x000511CF File Offset: 0x0004F3CF
		// (set) Token: 0x0600107B RID: 4219 RVA: 0x000511D7 File Offset: 0x0004F3D7
		public string[] CanHoldItemTags { get; set; } = Array.Empty<string>();

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x000511E0 File Offset: 0x0004F3E0
		// (set) Token: 0x0600107D RID: 4221 RVA: 0x000511E8 File Offset: 0x0004F3E8
		public string[] CanHoldBlockTags { get; set; } = Array.Empty<string>();

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x000511F1 File Offset: 0x0004F3F1
		// (set) Token: 0x0600107F RID: 4223 RVA: 0x000511F9 File Offset: 0x0004F3F9
		public string[][] CanHoldItemTagsCondition { get; set; } = Array.Empty<string[]>();

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x00051202 File Offset: 0x0004F402
		// (set) Token: 0x06001081 RID: 4225 RVA: 0x0005120A File Offset: 0x0004F40A
		public string[][] CanHoldBlockTagsCondition { get; set; } = Array.Empty<string[]>();

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001082 RID: 4226 RVA: 0x00051213 File Offset: 0x0004F413
		// (set) Token: 0x06001083 RID: 4227 RVA: 0x0005121B File Offset: 0x0004F41B
		public string[] CanHoldWildcards { get; set; } = Array.Empty<string>();

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001084 RID: 4228 RVA: 0x00051224 File Offset: 0x0004F424
		// (set) Token: 0x06001085 RID: 4229 RVA: 0x0005122C File Offset: 0x0004F42C
		public string[] CanHoldItemAttributes { get; set; } = Array.Empty<string>();

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001086 RID: 4230 RVA: 0x00051235 File Offset: 0x0004F435
		// (set) Token: 0x06001087 RID: 4231 RVA: 0x0005123D File Offset: 0x0004F43D
		[Nullable(2)]
		public string SlotColor { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001088 RID: 4232 RVA: 0x00051246 File Offset: 0x0004F446
		// (set) Token: 0x06001089 RID: 4233 RVA: 0x0005124E File Offset: 0x0004F44E
		[Nullable(2)]
		public string SlotsIcon { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x00051257 File Offset: 0x0004F457
		// (set) Token: 0x0600108B RID: 4235 RVA: 0x0005125F File Offset: 0x0004F45F
		public string SlotBackpackCategory { get; set; } = "";

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x00051268 File Offset: 0x0004F468
		// (set) Token: 0x0600108D RID: 4237 RVA: 0x00051270 File Offset: 0x0004F470
		public float CategoryOrderPriority { get; set; } = 1f;

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x0600108E RID: 4238 RVA: 0x00051279 File Offset: 0x0004F479
		// (set) Token: 0x0600108F RID: 4239 RVA: 0x00051281 File Offset: 0x0004F481
		public int SlotsNumber { get; set; }

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001090 RID: 4240 RVA: 0x0005128A File Offset: 0x0004F48A
		// (set) Token: 0x06001091 RID: 4241 RVA: 0x00051292 File Offset: 0x0004F492
		public int StoredItemLightLevelOffset { get; set; }

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x0005129B File Offset: 0x0004F49B
		// (set) Token: 0x06001093 RID: 4243 RVA: 0x000512A3 File Offset: 0x0004F4A3
		public string SlotVariant { get; set; } = "bag_slot";

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x000512AC File Offset: 0x0004F4AC
		// (set) Token: 0x06001095 RID: 4245 RVA: 0x000512B4 File Offset: 0x0004F4B4
		public string SlotStateVariant { get; set; } = "bag_slot_state";

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001096 RID: 4246 RVA: 0x000512BD File Offset: 0x0004F4BD
		// (set) Token: 0x06001097 RID: 4247 RVA: 0x000512C5 File Offset: 0x0004F4C5
		public string EmptyStateCode { get; set; } = "empty";

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x000512CE File Offset: 0x0004F4CE
		// (set) Token: 0x06001099 RID: 4249 RVA: 0x000512D6 File Offset: 0x0004F4D6
		public string FullStateCode { get; set; } = "full";

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x0600109A RID: 4250 RVA: 0x000512DF File Offset: 0x0004F4DF
		// (set) Token: 0x0600109B RID: 4251 RVA: 0x000512E7 File Offset: 0x0004F4E7
		public string SlotMetalVariant { get; set; } = "bag_slot_metal";

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600109C RID: 4252 RVA: 0x000512F0 File Offset: 0x0004F4F0
		// (set) Token: 0x0600109D RID: 4253 RVA: 0x000512F8 File Offset: 0x0004F4F8
		public string SlotLeatherVariant { get; set; } = "bag_slot_leather";

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x00051301 File Offset: 0x0004F501
		// (set) Token: 0x0600109F RID: 4255 RVA: 0x00051309 File Offset: 0x0004F509
		public string SlotWoodVariant { get; set; } = "bag_slot_wood";

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x00051312 File Offset: 0x0004F512
		// (set) Token: 0x060010A1 RID: 4257 RVA: 0x0005131A File Offset: 0x0004F51A
		public bool HandleHotkey { get; set; }

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x00051323 File Offset: 0x0004F523
		// (set) Token: 0x060010A3 RID: 4259 RVA: 0x0005132B File Offset: 0x0004F52B
		public bool DisplayInToolDialog { get; set; }

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060010A4 RID: 4260 RVA: 0x00051334 File Offset: 0x0004F534
		// (set) Token: 0x060010A5 RID: 4261 RVA: 0x0005133C File Offset: 0x0004F53C
		public bool SetVariants { get; set; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x00051345 File Offset: 0x0004F545
		// (set) Token: 0x060010A7 RID: 4263 RVA: 0x0005134D File Offset: 0x0004F54D
		public bool SetMaterialVariants { get; set; }

		// Token: 0x060010A8 RID: 4264 RVA: 0x00051358 File Offset: 0x0004F558
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
