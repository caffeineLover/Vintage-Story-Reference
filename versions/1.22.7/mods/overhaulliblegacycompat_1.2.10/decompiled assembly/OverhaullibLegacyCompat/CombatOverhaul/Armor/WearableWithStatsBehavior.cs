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
	// Token: 0x02000142 RID: 322
	[NullableContext(1)]
	[Nullable(0)]
	public class WearableWithStatsBehavior : CollectibleBehavior, IAffectsPlayerStats
	{
		// Token: 0x06000F8A RID: 3978 RVA: 0x0004C32D File Offset: 0x0004A52D
		public WearableWithStatsBehavior(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0004C341 File Offset: 0x0004A541
		// (set) Token: 0x06000F8C RID: 3980 RVA: 0x0004C349 File Offset: 0x0004A549
		public Dictionary<string, float> Stats { get; set; } = new Dictionary<string, float>();

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x0004C352 File Offset: 0x0004A552
		// (set) Token: 0x06000F8E RID: 3982 RVA: 0x0004C35A File Offset: 0x0004A55A
		public bool StatsChanged { get; set; }

		// Token: 0x06000F8F RID: 3983 RVA: 0x0004C364 File Offset: 0x0004A564
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

		// Token: 0x06000F90 RID: 3984 RVA: 0x0004C418 File Offset: 0x0004A618
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			WearableStatsJson stats = properties.AsObject<WearableStatsJson>(null);
			this.Stats = stats.PlayerStats;
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0004C440 File Offset: 0x0004A640
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

		// Token: 0x06000F92 RID: 3986 RVA: 0x0004C568 File Offset: 0x0004A768
		private static bool IsPenaltyStat(string statCode)
		{
			return string.Equals(statCode, "walkspeed", 5) || string.Equals(statCode, "manipulationSpeed", 5) || string.Equals(statCode, "steadyAim", 5) || string.Equals(statCode, "healingeffectivness", 5) || string.Equals(statCode, "hungerrate", 5);
		}
	}
}
