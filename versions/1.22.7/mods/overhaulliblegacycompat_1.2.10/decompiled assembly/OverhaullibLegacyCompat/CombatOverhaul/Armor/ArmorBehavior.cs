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
	// Token: 0x02000139 RID: 313
	[NullableContext(1)]
	[Nullable(0)]
	public class ArmorBehavior : CollectibleBehavior, IArmor, IModularArmor, IAffectsPlayerStats
	{
		// Token: 0x06000F0D RID: 3853 RVA: 0x00049144 File Offset: 0x00047344
		public ArmorBehavior(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0004917B File Offset: 0x0004737B
		// (set) Token: 0x06000F0F RID: 3855 RVA: 0x00049183 File Offset: 0x00047383
		public ArmorType ArmorType { get; protected set; } = new ArmorType(ArmorLayers.None, DamageZone.None);

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x0004918C File Offset: 0x0004738C
		// (set) Token: 0x06000F11 RID: 3857 RVA: 0x00049194 File Offset: 0x00047394
		public DamageResistData Resists { get; protected set; } = new DamageResistData();

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x0004919D File Offset: 0x0004739D
		// (set) Token: 0x06000F13 RID: 3859 RVA: 0x000491A5 File Offset: 0x000473A5
		public Dictionary<ArmorType, DamageResistData> ResistsByType { get; protected set; } = new Dictionary<ArmorType, DamageResistData>();

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000F14 RID: 3860 RVA: 0x000491AE File Offset: 0x000473AE
		// (set) Token: 0x06000F15 RID: 3861 RVA: 0x000491B6 File Offset: 0x000473B6
		public Dictionary<string, float> Stats { get; protected set; } = new Dictionary<string, float>();

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x000491BF File Offset: 0x000473BF
		// (set) Token: 0x06000F17 RID: 3863 RVA: 0x000491C7 File Offset: 0x000473C7
		public bool StatsChanged { get; set; }

		// Token: 0x06000F18 RID: 3864 RVA: 0x000491D0 File Offset: 0x000473D0
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

		// Token: 0x06000F19 RID: 3865 RVA: 0x00049284 File Offset: 0x00047484
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

		// Token: 0x06000F1A RID: 3866 RVA: 0x00049508 File Offset: 0x00047708
		public void Initialize(ArmorType armorType, DamageResistData resists, Dictionary<string, float> stats)
		{
			JsonObject properties = new JsonObject(new JObject());
			base.Initialize(properties);
			this.Stats = stats;
			this.ArmorType = armorType;
			this.Resists = resists;
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0004953C File Offset: 0x0004773C
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

		// Token: 0x06000F1C RID: 3868 RVA: 0x00049A9C File Offset: 0x00047C9C
		public DamageResistData GetResists(ItemSlot slot, ArmorType type)
		{
			if (this.ResistsByType.ContainsKey(type))
			{
				return QuenchableStatUtil.ApplyArmorProtection(slot.Itemstack, this.ResistsByType[type]);
			}
			return QuenchableStatUtil.ApplyArmorProtection(slot.Itemstack, this.Resists);
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00049AD8 File Offset: 0x00047CD8
		private static bool IsPenaltyStat(string statCode)
		{
			return string.Equals(statCode, "walkspeed", 5) || string.Equals(statCode, "manipulationSpeed", 5) || string.Equals(statCode, "steadyAim", 5) || string.Equals(statCode, "healingeffectivness", 5) || string.Equals(statCode, "hungerrate", 5);
		}

		// Token: 0x0200028D RID: 653
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000D5F RID: 3423
			[Nullable(0)]
			public static Func<float[], DirectionConstrain> <0>__FromArray;

			// Token: 0x04000D60 RID: 3424
			[Nullable(0)]
			public static Func<string, ArmorLayers> <1>__Parse;

			// Token: 0x04000D61 RID: 3425
			[Nullable(0)]
			public static Func<string, DamageZone> <2>__Parse;
		}
	}
}
