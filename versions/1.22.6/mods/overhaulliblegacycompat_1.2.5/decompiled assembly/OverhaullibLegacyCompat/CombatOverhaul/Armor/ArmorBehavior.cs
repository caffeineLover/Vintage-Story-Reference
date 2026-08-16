using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Utils;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200012D RID: 301
	[NullableContext(1)]
	[Nullable(0)]
	public class ArmorBehavior : CollectibleBehavior, IArmor, IModularArmor, IAffectsPlayerStats
	{
		// Token: 0x06000E99 RID: 3737 RVA: 0x00047844 File Offset: 0x00045A44
		public ArmorBehavior(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x0004787B File Offset: 0x00045A7B
		// (set) Token: 0x06000E9B RID: 3739 RVA: 0x00047883 File Offset: 0x00045A83
		public ArmorType ArmorType { get; protected set; } = new ArmorType(ArmorLayers.None, DamageZone.None);

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x0004788C File Offset: 0x00045A8C
		// (set) Token: 0x06000E9D RID: 3741 RVA: 0x00047894 File Offset: 0x00045A94
		public DamageResistData Resists { get; protected set; } = new DamageResistData();

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x0004789D File Offset: 0x00045A9D
		// (set) Token: 0x06000E9F RID: 3743 RVA: 0x000478A5 File Offset: 0x00045AA5
		public Dictionary<ArmorType, DamageResistData> ResistsByType { get; protected set; } = new Dictionary<ArmorType, DamageResistData>();

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x000478AE File Offset: 0x00045AAE
		// (set) Token: 0x06000EA1 RID: 3745 RVA: 0x000478B6 File Offset: 0x00045AB6
		public Dictionary<string, float> Stats { get; protected set; } = new Dictionary<string, float>();

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x000478BF File Offset: 0x00045ABF
		// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x000478C7 File Offset: 0x00045AC7
		public bool StatsChanged { get; set; }

		// Token: 0x06000EA4 RID: 3748 RVA: 0x000478D0 File Offset: 0x00045AD0
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
				adjusted[stat] = (ArmorBehavior.IsPenaltyStat(stat) ? (value * penaltyMultiplier) : value);
			}
			return adjusted;
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00047984 File Offset: 0x00045B84
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			ArmorStatsJson stats = properties.AsObject<ArmorStatsJson>(null);
			this.Stats = stats.PlayerStats;
			if (!Enumerable.Any<string>(stats.Layers) || !Enumerable.Any<string>(stats.Zones))
			{
				return;
			}
			IEnumerable<float[]> anglesCoverage = stats.AnglesCoverage;
			Func<float[], DirectionConstrain> func;
			if ((func = ArmorBehavior.<>O.<0>__FromArray) == null)
			{
				func = (ArmorBehavior.<>O.<0>__FromArray = new Func<float[], DirectionConstrain>(DirectionConstrain.FromArray));
			}
			IEnumerable<DirectionConstrain> angleCoverage = Enumerable.Select<float[], DirectionConstrain>(anglesCoverage, func);
			IEnumerable<string> layers = stats.Layers;
			Func<string, ArmorLayers> func2;
			if ((func2 = ArmorBehavior.<>O.<1>__Parse) == null)
			{
				func2 = (ArmorBehavior.<>O.<1>__Parse = new Func<string, ArmorLayers>(Enum.Parse<ArmorLayers>));
			}
			ArmorLayers layers2 = Enumerable.Aggregate<ArmorLayers>(Enumerable.Select<string, ArmorLayers>(layers, func2), (ArmorLayers first, ArmorLayers second) => first | second);
			IEnumerable<string> zones = stats.Zones;
			Func<string, DamageZone> func3;
			if ((func3 = ArmorBehavior.<>O.<2>__Parse) == null)
			{
				func3 = (ArmorBehavior.<>O.<2>__Parse = new Func<string, DamageZone>(Enum.Parse<DamageZone>));
			}
			this.ArmorType = new ArmorType(layers2, Enumerable.Aggregate<DamageZone>(Enumerable.Select<string, DamageZone>(zones, func3), (DamageZone first, DamageZone second) => first | second));
			this.Resists = new DamageResistData(Enumerable.ToDictionary<KeyValuePair<string, float>, EnumDamageType, float>(stats.Resists, (KeyValuePair<string, float> entry) => Enum.Parse<EnumDamageType>(entry.Key), (KeyValuePair<string, float> entry) => entry.Value), Enumerable.ToDictionary<KeyValuePair<string, float>, EnumDamageType, float>(stats.FlatReduction, (KeyValuePair<string, float> entry) => Enum.Parse<EnumDamageType>(entry.Key), (KeyValuePair<string, float> entry) => entry.Value), angleCoverage);
			foreach (KeyValuePair<string, Dictionary<string, float>> keyValuePair in stats.ResistsByZone)
			{
				string text;
				Dictionary<string, float> dictionary;
				keyValuePair.Deconstruct(ref text, ref dictionary);
				string text2 = text;
				Dictionary<string, float> resists = dictionary;
				string[] array = text2.Split('-', 0);
				ArmorLayers layer = Enum.Parse<ArmorLayers>(array[0]);
				DamageZone zone = Enum.Parse<DamageZone>(array[1]);
				ArmorType typeEnum = new ArmorType(layer, zone);
				DamageResistData resistsData = new DamageResistData(Enumerable.ToDictionary<KeyValuePair<string, float>, EnumDamageType, float>(resists, (KeyValuePair<string, float> entry) => Enum.Parse<EnumDamageType>(entry.Key), (KeyValuePair<string, float> entry) => entry.Value), new Dictionary<EnumDamageType, float>(), angleCoverage);
				this.ResistsByType.Add(typeEnum, resistsData);
			}
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00047C08 File Offset: 0x00045E08
		public void Initialize(ArmorType armorType, DamageResistData resists, Dictionary<string, float> stats)
		{
			JsonObject properties = new JsonObject(new JObject());
			base.Initialize(properties);
			this.Stats = stats;
			this.ArmorType = armorType;
			this.Resists = resists;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00047C3C File Offset: 0x00045E3C
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			dsc.AppendLine(Lang.Get("combatoverhaul:armor-layers-info", new object[]
			{
				this.ArmorType.LayersToTranslatedString()
			}));
			dsc.AppendLine(Lang.Get("combatoverhaul:armor-zones-info", new object[]
			{
				this.ArmorType.ZonesToTranslatedString()
			}));
			DamageResistData displayResists = QuenchableStatUtil.ApplyArmorProtection(inSlot.Itemstack, this.Resists);
			EnumDamageType enumDamageType;
			float num;
			if (Enumerable.Any<float>(displayResists.Resists.Values, (float value) => value != 0f))
			{
				dsc.AppendLine(Lang.Get("combatoverhaul:armor-fraction-protection", Array.Empty<object>()));
				foreach (KeyValuePair<EnumDamageType, float> keyValuePair in Enumerable.Where<KeyValuePair<EnumDamageType, float>>(displayResists.Resists, (KeyValuePair<EnumDamageType, float> entry) => entry.Value > 0f))
				{
					keyValuePair.Deconstruct(ref enumDamageType, ref num);
					EnumDamageType type = enumDamageType;
					float level = num;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
					defaultInterpolatedStringHandler.AppendLiteral("combatoverhaul:damage-type-");
					defaultInterpolatedStringHandler.AppendFormatted<EnumDamageType>(type);
					string damageType = Lang.Get(defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<object>());
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 2, dsc);
					appendInterpolatedStringHandler.AppendLiteral("  ");
					appendInterpolatedStringHandler.AppendFormatted(damageType);
					appendInterpolatedStringHandler.AppendLiteral(": ");
					appendInterpolatedStringHandler.AppendFormatted<float>(level);
					dsc.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			if (Enumerable.Any<float>(displayResists.FlatDamageReduction.Values, (float value) => value != 0f))
			{
				dsc.AppendLine(Lang.Get("combatoverhaul:armor-flat-protection", Array.Empty<object>()));
				using (IEnumerator<KeyValuePair<EnumDamageType, float>> enumerator = Enumerable.Where<KeyValuePair<EnumDamageType, float>>(displayResists.FlatDamageReduction, (KeyValuePair<EnumDamageType, float> entry) => entry.Value > 0f).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<EnumDamageType, float> keyValuePair = enumerator.Current;
						keyValuePair.Deconstruct(ref enumDamageType, ref num);
						EnumDamageType type2 = enumDamageType;
						float level2 = num;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(27, 1);
						defaultInterpolatedStringHandler2.AppendLiteral("combatoverhaul:damage-type-");
						defaultInterpolatedStringHandler2.AppendFormatted<EnumDamageType>(type2);
						string damageType2 = Lang.Get(defaultInterpolatedStringHandler2.ToStringAndClear(), Array.Empty<object>());
						StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 2, dsc);
						appendInterpolatedStringHandler.AppendLiteral("  ");
						appendInterpolatedStringHandler.AppendFormatted(damageType2);
						appendInterpolatedStringHandler.AppendLiteral(": ");
						appendInterpolatedStringHandler.AppendFormatted<float>(level2);
						dsc.AppendLine(ref appendInterpolatedStringHandler);
					}
					goto IL_3E7;
				}
			}
			if (Enumerable.Any<KeyValuePair<ArmorType, DamageResistData>>(this.ResistsByType) && Enumerable.Any<KeyValuePair<EnumDamageType, float>>(Enumerable.First<DamageResistData>(this.ResistsByType.Values).Resists))
			{
				IEnumerable<KeyValuePair<EnumDamageType, float>> resists = QuenchableStatUtil.ApplyArmorProtection(inSlot.Itemstack, Enumerable.First<DamageResistData>(this.ResistsByType.Values)).Resists;
				dsc.AppendLine(Lang.Get("combatoverhaul:armor-fraction-protection", Array.Empty<object>()));
				foreach (KeyValuePair<EnumDamageType, float> keyValuePair in Enumerable.Where<KeyValuePair<EnumDamageType, float>>(resists, (KeyValuePair<EnumDamageType, float> entry) => entry.Value > 0f))
				{
					keyValuePair.Deconstruct(ref enumDamageType, ref num);
					EnumDamageType type3 = enumDamageType;
					float level3 = num;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(27, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("combatoverhaul:damage-type-");
					defaultInterpolatedStringHandler3.AppendFormatted<EnumDamageType>(type3);
					string damageType3 = Lang.Get(defaultInterpolatedStringHandler3.ToStringAndClear(), Array.Empty<object>());
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 2, dsc);
					appendInterpolatedStringHandler.AppendLiteral("  ");
					appendInterpolatedStringHandler.AppendFormatted(damageType3);
					appendInterpolatedStringHandler.AppendLiteral(": ");
					appendInterpolatedStringHandler.AppendFormatted<float>(level3);
					dsc.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			IL_3E7:
			if (Enumerable.Any<float>(this.Stats.Values, (float value) => value != 0f))
			{
				float penaltyMultiplier = QuenchableStatUtil.GetArmorPenaltyMultiplier((inSlot != null) ? inSlot.Itemstack : null);
				dsc.AppendLine(Lang.Get("combatoverhaul:stat-stats", Array.Empty<object>()));
				foreach (KeyValuePair<string, float> keyValuePair2 in this.Stats)
				{
					string text;
					keyValuePair2.Deconstruct(ref text, ref num);
					string stat = text;
					float value2 = num;
					float shownValue = ArmorBehavior.IsPenaltyStat(stat) ? (value2 * penaltyMultiplier) : value2;
					if (shownValue != 0f)
					{
						StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(5, 2, dsc);
						appendInterpolatedStringHandler.AppendLiteral("  ");
						appendInterpolatedStringHandler.AppendFormatted(Lang.Get("combatoverhaul:stat-" + stat, Array.Empty<object>()));
						appendInterpolatedStringHandler.AppendLiteral(": ");
						appendInterpolatedStringHandler.AppendFormatted<float>(shownValue * 100f, "F1");
						appendInterpolatedStringHandler.AppendLiteral("%");
						dsc.AppendLine(ref appendInterpolatedStringHandler);
					}
				}
			}
			dsc.AppendLine();
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0004819C File Offset: 0x0004639C
		public DamageResistData GetResists(ItemSlot slot, ArmorType type)
		{
			if (this.ResistsByType.ContainsKey(type))
			{
				return QuenchableStatUtil.ApplyArmorProtection(slot.Itemstack, this.ResistsByType[type]);
			}
			return QuenchableStatUtil.ApplyArmorProtection(slot.Itemstack, this.Resists);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x000481D8 File Offset: 0x000463D8
		private static bool IsPenaltyStat(string statCode)
		{
			return string.Equals(statCode, "walkspeed", 5) || string.Equals(statCode, "manipulationSpeed", 5) || string.Equals(statCode, "steadyAim", 5) || string.Equals(statCode, "healingeffectivness", 5) || string.Equals(statCode, "hungerrate", 5);
		}

		// Token: 0x0200027A RID: 634
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000D13 RID: 3347
			[Nullable(0)]
			public static Func<float[], DirectionConstrain> <0>__FromArray;

			// Token: 0x04000D14 RID: 3348
			[Nullable(0)]
			public static Func<string, ArmorLayers> <1>__Parse;

			// Token: 0x04000D15 RID: 3349
			[Nullable(0)]
			public static Func<string, DamageZone> <2>__Parse;
		}
	}
}
