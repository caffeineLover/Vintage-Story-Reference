using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000139 RID: 313
	[NullableContext(1)]
	[Nullable(0)]
	public class AdditionalSlotsBehavior : CollectibleBehavior, IEnableAdditionalSlots
	{
		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x0004AD17 File Offset: 0x00048F17
		// (set) Token: 0x06000F2A RID: 3882 RVA: 0x0004AD1F File Offset: 0x00048F1F
		public AdditionalSlotsConfig Config { get; set; } = new AdditionalSlotsConfig();

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x0004AD28 File Offset: 0x00048F28
		// (set) Token: 0x06000F2C RID: 3884 RVA: 0x0004AD30 File Offset: 0x00048F30
		public Dictionary<string, SlotConfig> SlotConfigs { get; set; } = new Dictionary<string, SlotConfig>();

		// Token: 0x06000F2D RID: 3885 RVA: 0x0004AD39 File Offset: 0x00048F39
		public AdditionalSlotsBehavior(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0004AD58 File Offset: 0x00048F58
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			this.Config = properties.AsObject<AdditionalSlotsConfig>(null);
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0004AD70 File Offset: 0x00048F70
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

		// Token: 0x06000F30 RID: 3888 RVA: 0x0004AE04 File Offset: 0x00049004
		[return: Nullable(2)]
		public SlotConfig GetConfig(ItemStack stack, IInventory inventory, string slotType)
		{
			if (!this.SlotConfigs.ContainsKey(slotType))
			{
				return null;
			}
			return this.SlotConfigs[slotType];
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0004AE22 File Offset: 0x00049022
		[return: Nullable(2)]
		public string GetIcon(ItemStack stack, IInventory inventory, string slotType)
		{
			if (!this.Config.Icons.ContainsKey(slotType))
			{
				return null;
			}
			return this.Config.Icons[slotType];
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0004AE4A File Offset: 0x0004904A
		public bool GetIfEnabled(ItemStack stack, IInventory inventory, string slotType)
		{
			return MemoryExtensions.Contains<string>(this.Config.EnabledSlots, slotType);
		}
	}
}
