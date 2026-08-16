using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000136 RID: 310
	[NullableContext(1)]
	[Nullable(0)]
	public class WearableWithStatsBehavior : CollectibleBehavior, IAffectsPlayerStats
	{
		// Token: 0x06000F16 RID: 3862 RVA: 0x0004AA2D File Offset: 0x00048C2D
		public WearableWithStatsBehavior(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x0004AA41 File Offset: 0x00048C41
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x0004AA49 File Offset: 0x00048C49
		public Dictionary<string, float> Stats { get; set; } = new Dictionary<string, float>();

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x0004AA52 File Offset: 0x00048C52
		// (set) Token: 0x06000F1A RID: 3866 RVA: 0x0004AA5A File Offset: 0x00048C5A
		public bool StatsChanged { get; set; }

		// Token: 0x06000F1B RID: 3867 RVA: 0x0004AA64 File Offset: 0x00048C64
		public Dictionary<string, float> PlayerStats(ItemSlot slot, EntityPlayer player)
		{
			float penaltyMultiplier = QuenchableStatUtil.GetArmorPenaltyMultiplier((slot != null) ? slot.Itemstack : null);
			if (Math.Abs(penaltyMultiplier - 1f) < 0.0001f)
			{
				return this.Stats;
			}
			Dictionary<string, float> adjusted = new Dictionary<string, float>(this.Stats.Count);
			foreach (KeyValuePair<string, float> keyValuePair in this.Stats)
			{
				string text;
				float num;
				keyValuePair.Deconstruct(ref text, ref num);
				string stat = text;
				float value = num;
				adjusted[stat] = (WearableWithStatsBehavior.IsPenaltyStat(stat) ? (value * penaltyMultiplier) : value);
			}
			return adjusted;
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0004AB18 File Offset: 0x00048D18
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			WearableStatsJson stats = properties.AsObject<WearableStatsJson>(null);
			this.Stats = stats.PlayerStats;
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0004AB40 File Offset: 0x00048D40
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			if (Enumerable.Any<float>(this.Stats.Values, (float value) => value != 0f))
			{
				dsc.AppendLine(Lang.Get("combatoverhaul:stat-stats", Array.Empty<object>()));
				foreach (KeyValuePair<string, float> keyValuePair in this.Stats)
				{
					string text;
					float num;
					keyValuePair.Deconstruct(ref text, ref num);
					string stat = text;
					float value2 = num;
					if (value2 != 0f)
					{
						StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(5, 2, dsc);
						appendInterpolatedStringHandler.AppendLiteral("  ");
						appendInterpolatedStringHandler.AppendFormatted(Lang.Get("combatoverhaul:stat-" + stat, Array.Empty<object>()));
						appendInterpolatedStringHandler.AppendLiteral(": ");
						appendInterpolatedStringHandler.AppendFormatted<float>(value2 * 100f, "F1");
						appendInterpolatedStringHandler.AppendLiteral("%");
						dsc.AppendLine(ref appendInterpolatedStringHandler);
					}
				}
				dsc.AppendLine();
			}
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0004AC68 File Offset: 0x00048E68
		private static bool IsPenaltyStat(string statCode)
		{
			return string.Equals(statCode, "walkspeed", 5) || string.Equals(statCode, "manipulationSpeed", 5) || string.Equals(statCode, "steadyAim", 5) || string.Equals(statCode, "healingeffectivness", 5) || string.Equals(statCode, "hungerrate", 5);
		}
	}
}
