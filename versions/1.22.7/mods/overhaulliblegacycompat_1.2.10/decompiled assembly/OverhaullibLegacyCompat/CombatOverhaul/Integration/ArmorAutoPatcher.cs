using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Armor;
using CombatOverhaul.Utils;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000035 RID: 53
	[NullableContext(1)]
	[Nullable(0)]
	public static class ArmorAutoPatcher
	{
		// Token: 0x060003A8 RID: 936 RVA: 0x00010C64 File Offset: 0x0000EE64
		public static void Patch(ICoreAPI api)
		{
			foreach (Item item in api.World.Items)
			{
				if (item.Attributes != null && ArmorAutoPatcher.IsVanillaArmor(item))
				{
					try
					{
						if (!ArmorAutoPatcher.IsCOArmor(item))
						{
							ArmorAutoPatcher.Patch(item, api);
						}
						ArmorAutoPatcher.EnsureWearableArmorBehavior(item, api);
					}
					catch (Exception exception)
					{
						Type typeFromHandle = typeof(ArmorAutoPatcher);
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Error while patching armor values for '");
						defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(item.Code);
						defaultInterpolatedStringHandler.AppendLiteral("':\n");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
						LoggerUtil.Error(api, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00010D3C File Offset: 0x0000EF3C
		public static void Patch(Item item, ICoreAPI api)
		{
			JsonObject properties = ArmorAutoPatcher.GenerateProperties(item, api);
			ArmorBehavior behavior = new ArmorBehavior(item);
			behavior.Initialize(properties);
			item.CollectibleBehaviors = Enumerable.ToArray<CollectibleBehavior>(Enumerable.Append<CollectibleBehavior>(item.CollectibleBehaviors, behavior));
			ArmorAutoPatcher.RemoveVanillaStats(item);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00010D7C File Offset: 0x0000EF7C
		private static void EnsureWearableArmorBehavior(Item item, ICoreAPI api)
		{
			if (item is ItemWearableArmor)
			{
				return;
			}
			CollectibleBehavior[] behaviors = item.CollectibleBehaviors ?? Array.Empty<CollectibleBehavior>();
			if (Enumerable.Any<CollectibleBehavior>(behaviors, (CollectibleBehavior behavior) => behavior is WearableArmorBehavior))
			{
				return;
			}
			WearableArmorBehavior behavior2 = new WearableArmorBehavior(item);
			behavior2.Initialize(new JsonObject(new JObject()));
			behavior2.OnLoaded(api);
			CollectibleBehavior[] array = behaviors;
			Predicate<CollectibleBehavior> predicate;
			if ((predicate = ArmorAutoPatcher.<>O.<0>__IsVanillaWearableBehavior) == null)
			{
				predicate = (ArmorAutoPatcher.<>O.<0>__IsVanillaWearableBehavior = new Predicate<CollectibleBehavior>(ArmorAutoPatcher.IsVanillaWearableBehavior));
			}
			int wearableIndex = Array.FindIndex<CollectibleBehavior>(array, predicate);
			if (wearableIndex < 0)
			{
				item.CollectibleBehaviors = Enumerable.ToArray<CollectibleBehavior>(Enumerable.Append<CollectibleBehavior>(behaviors, behavior2));
				return;
			}
			item.CollectibleBehaviors = Enumerable.ToArray<CollectibleBehavior>(Enumerable.Concat<CollectibleBehavior>(Enumerable.Append<CollectibleBehavior>(Enumerable.Take<CollectibleBehavior>(behaviors, wearableIndex), behavior2), Enumerable.Skip<CollectibleBehavior>(behaviors, wearableIndex)));
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00010E45 File Offset: 0x0000F045
		private static bool IsVanillaWearableBehavior(CollectibleBehavior behavior)
		{
			return behavior is CollectibleBehaviorWearable || behavior.GetType().Name == "CollectibleBehaviorWearable";
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00010E68 File Offset: 0x0000F068
		private static bool IsVanillaArmor(Item item)
		{
			string armorPart = ArmorAutoPatcher.VanillaArmorPart(item);
			return ArmorAutoPatcher._vanillaArmorParts.Contains(armorPart);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00010E88 File Offset: 0x0000F088
		private static string VanillaArmorPart(Item item)
		{
			string clothesCategory = null;
			if (item.Attributes.KeyExists("clothesCategory"))
			{
				clothesCategory = item.Attributes["clothesCategory"].AsString(null);
			}
			if (item.Attributes.KeyExists("clothescategory"))
			{
				clothesCategory = item.Attributes["clothescategory"].AsString(null);
			}
			string categoryCode = null;
			if (item.Attributes.KeyExists("attachableToEntity") && item.Attributes["attachableToEntity"].KeyExists("categoryCode"))
			{
				categoryCode = item.Attributes["attachableToEntity"]["categoryCode"].AsString(null);
			}
			string result;
			if ((result = clothesCategory) == null)
			{
				result = (categoryCode ?? "");
			}
			return result;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00010F4C File Offset: 0x0000F14C
		private static bool IsCOArmor(Item item)
		{
			bool collectibleInterface = item.GetCollectibleInterface<IArmor>() != null;
			IModularArmor modularArmor = item.GetCollectibleInterface<IModularArmor>();
			return collectibleInterface || modularArmor != null;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00010F70 File Offset: 0x0000F170
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"relativeProtection",
			"flatDamageReduction",
			"protectionTier"
		})]
		private static ValueTuple<float, float, int> VanillaArmorStats([Nullable(1)] Item item)
		{
			if (!item.Attributes.KeyExists("protectionModifiers"))
			{
				return new ValueTuple<float, float, int>(0f, 0f, 0);
			}
			float num = item.Attributes["protectionModifiers"]["relativeProtection"].AsFloat(0f);
			float flatDamageReduction = item.Attributes["protectionModifiers"]["flatDamageReduction"].AsFloat(0f);
			int protectionTier = item.Attributes["protectionModifiers"]["protectionTier"].AsInt(0);
			return new ValueTuple<float, float, int>(num, flatDamageReduction, protectionTier);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00011014 File Offset: 0x0000F214
		private static int ProtectionTierFromVanillaStats(float relativeProtection, float flatDamageReduction, int protectionTier)
		{
			float relativeProtectionFraction = (relativeProtection - 0.4f) / 0.57000005f;
			return (int)MathF.Floor(2f + 12f * relativeProtectionFraction * ArmorAutoPatcher.MultiplierFromVanillaProtectionTier(protectionTier));
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001104C File Offset: 0x0000F24C
		private static float MultiplierFromVanillaProtectionTier(int protectionTier)
		{
			float tierFraction = (float)protectionTier / 4f;
			return 0.2f + 0.8f * tierFraction;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00011070 File Offset: 0x0000F270
		private static JsonObject GenerateProperties(Item item, ICoreAPI api)
		{
			string armorPart = ArmorAutoPatcher.VanillaArmorPart(item);
			ValueTuple<float, float, int> valueTuple = ArmorAutoPatcher.VanillaArmorStats(item);
			float relativeProtection = valueTuple.Item1;
			float flatDamageReduction = valueTuple.Item2;
			int protectionTier = valueTuple.Item3;
			int newProtectionTier = ArmorAutoPatcher.ProtectionTierFromVanillaStats(relativeProtection, flatDamageReduction, protectionTier);
			string layers = "\"Layers\":[\"Outer\", \"Middle\"]";
			string zones = "\"Zones\":" + ArmorAutoPatcher._damageZoneFromArmorPart[armorPart];
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(67, 3);
			defaultInterpolatedStringHandler.AppendLiteral("\"Resists\":{\"PiercingAttack\": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(newProtectionTier);
			defaultInterpolatedStringHandler.AppendLiteral(", \"SlashingAttack\": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(newProtectionTier);
			defaultInterpolatedStringHandler.AppendLiteral(", \"BluntAttack\": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>((int)((float)newProtectionTier * 0.5f));
			defaultInterpolatedStringHandler.AppendLiteral("}");
			string resists = defaultInterpolatedStringHandler.ToStringAndClear();
			string stats = "\"PlayerStats\":{\"walkspeed\": -0.05, \"manipulationSpeed\": -0.05, \"steadyAim\": -0.05, \"healingeffectivness\": -0.05, \"hungerrate\": 0.05}";
			string text = "{";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(3, 4);
			defaultInterpolatedStringHandler2.AppendFormatted(layers);
			defaultInterpolatedStringHandler2.AppendLiteral(",");
			defaultInterpolatedStringHandler2.AppendFormatted(zones);
			defaultInterpolatedStringHandler2.AppendLiteral(",");
			defaultInterpolatedStringHandler2.AppendFormatted(resists);
			defaultInterpolatedStringHandler2.AppendLiteral(",");
			defaultInterpolatedStringHandler2.AppendFormatted(stats);
			string properties = text + defaultInterpolatedStringHandler2.ToStringAndClear() + "}";
			JsonObject propertiesJson;
			try
			{
				propertiesJson = JsonObject.FromJson(properties);
			}
			catch (Exception)
			{
				LoggerUtil.Warn(api, typeof(ArmorAutoPatcher), "Failed parsing:\n" + properties);
				throw;
			}
			return propertiesJson;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x000111D8 File Offset: 0x0000F3D8
		private static void RemoveVanillaStats(Item item)
		{
			JObject jobject = item.Attributes.Token as JObject;
			if (jobject != null)
			{
				jobject.Remove("protectionModifiers");
			}
			JObject jobject2 = item.Attributes.Token as JObject;
			if (jobject2 != null)
			{
				jobject2.Remove("statModifiers");
			}
			ItemWearable armor = item as ItemWearable;
			if (armor != null)
			{
				if (armor.StatModifers != null)
				{
					armor.StatModifers.rangedWeaponsAcc = 0f;
					armor.StatModifers.rangedWeaponsSpeed = 0f;
					armor.StatModifers.walkSpeed = 0f;
					armor.StatModifers.healingeffectivness = 0f;
					armor.StatModifers.hungerrate = 0f;
					armor.StatModifers.canEat = true;
				}
				if (armor.ProtectionModifiers != null)
				{
					armor.ProtectionModifiers.RelativeProtection = 0f;
					armor.ProtectionModifiers.PerTierRelativeProtectionLoss = new float[0];
					armor.ProtectionModifiers.FlatDamageReduction = 0f;
					armor.ProtectionModifiers.PerTierFlatDamageReductionLoss = new float[0];
					armor.ProtectionModifiers.ProtectionTier = 0;
					armor.ProtectionModifiers.HighDamageTierResistant = false;
				}
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x000112FC File Offset: 0x0000F4FC
		// Note: this type is marked as 'beforefieldinit'.
		static ArmorAutoPatcher()
		{
			List<string> list = new List<string>();
			list.Add("armorhead");
			list.Add("armorbody");
			list.Add("armorlegs");
			ArmorAutoPatcher._vanillaArmorParts = list;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("armorhead", "[\"Head\", \"Neck\", \"Face\"]");
			dictionary.Add("armorbody", "[\"Torso\", \"Arms\", \"Hands\"]");
			dictionary.Add("armorlegs", "[\"Legs\", \"Feet\"]");
			ArmorAutoPatcher._damageZoneFromArmorPart = dictionary;
		}

		// Token: 0x04000145 RID: 325
		private static List<string> _vanillaArmorParts;

		// Token: 0x04000146 RID: 326
		private static Dictionary<string, string> _damageZoneFromArmorPart;

		// Token: 0x04000147 RID: 327
		private const int _minProtectionTier = 2;

		// Token: 0x04000148 RID: 328
		private const int _maxProtectionTier = 14;

		// Token: 0x04000149 RID: 329
		private const float _minVanillaRelativeProtection = 0.4f;

		// Token: 0x0400014A RID: 330
		private const float _maxVanillaRelativeProtection = 0.97f;

		// Token: 0x0400014B RID: 331
		private const int _minVanillaProtectionTier = 0;

		// Token: 0x0400014C RID: 332
		private const int _maxVanillaProtectionTier = 4;

		// Token: 0x0400014D RID: 333
		private const float _minProtectionTierMultiplier = 0.2f;

		// Token: 0x0400014E RID: 334
		private const float _maxProtectionTierMultiplier = 1f;

		// Token: 0x020001D4 RID: 468
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000AE7 RID: 2791
			[Nullable(0)]
			public static Predicate<CollectibleBehavior> <0>__IsVanillaWearableBehavior;
		}
	}
}
