using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000145 RID: 325
	[NullableContext(1)]
	[Nullable(0)]
	public class AdditionalSlotsBehavior : CollectibleBehavior, IEnableAdditionalSlots
	{
		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x0004C617 File Offset: 0x0004A817
		// (set) Token: 0x06000F9E RID: 3998 RVA: 0x0004C61F File Offset: 0x0004A81F
		public AdditionalSlotsConfig Config { get; set; } = new AdditionalSlotsConfig();

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x0004C628 File Offset: 0x0004A828
		// (set) Token: 0x06000FA0 RID: 4000 RVA: 0x0004C630 File Offset: 0x0004A830
		public Dictionary<string, SlotConfig> SlotConfigs { get; set; } = new Dictionary<string, SlotConfig>();

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0004C639 File Offset: 0x0004A839
		public AdditionalSlotsBehavior(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0004C658 File Offset: 0x0004A858
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			this.Config = properties.AsObject<AdditionalSlotsConfig>(null);
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0004C670 File Offset: 0x0004A870
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			foreach (KeyValuePair<string, SlotConfigJson> keyValuePair in this.Config.Configs)
			{
				string text;
				SlotConfigJson slotConfigJson;
				keyValuePair.Deconstruct(ref text, ref slotConfigJson);
				string code = text;
				SlotConfig config = slotConfigJson.ToConfig();
				config.Resolve(api);
				this.SlotConfigs.Add(code, config);
			}
			this.Config.Configs = new Dictionary<string, SlotConfigJson>();
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0004C704 File Offset: 0x0004A904
		[return: Nullable(2)]
		public SlotConfig GetConfig(ItemStack stack, IInventory inventory, string slotType)
		{
			if (!this.SlotConfigs.ContainsKey(slotType))
			{
				return null;
			}
			return this.SlotConfigs[slotType];
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0004C722 File Offset: 0x0004A922
		[return: Nullable(2)]
		public string GetIcon(ItemStack stack, IInventory inventory, string slotType)
		{
			if (!this.Config.Icons.ContainsKey(slotType))
			{
				return null;
			}
			return this.Config.Icons[slotType];
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0004C74A File Offset: 0x0004A94A
		public bool GetIfEnabled(ItemStack stack, IInventory inventory, string slotType)
		{
			return MemoryExtensions.Contains<string>(this.Config.EnabledSlots, slotType);
		}
	}
}
