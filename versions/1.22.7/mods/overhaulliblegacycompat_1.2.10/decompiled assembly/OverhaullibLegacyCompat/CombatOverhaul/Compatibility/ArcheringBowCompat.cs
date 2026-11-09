using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace CombatOverhaul.Compatibility
{
	// Token: 0x0200015E RID: 350
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ArcheringBowCompat
	{
		// Token: 0x0600115F RID: 4447 RVA: 0x0005342D File Offset: 0x0005162D
		public ArcheringBowCompat(ICoreAPI api)
		{
			this._api = api;
			this.Enabled = 0;
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x00053443 File Offset: 0x00051643
		public bool Enabled { get; }

		// Token: 0x06001161 RID: 4449 RVA: 0x0005344C File Offset: 0x0005164C
		[return: Nullable(2)]
		public ArcheringBowAimingSettings GetSettings(Item item)
		{
			if (!this.Enabled || item.Code == null)
			{
				return null;
			}
			object config = this.GetConfig();
			if (config == null)
			{
				return null;
			}
			IDictionary bowSettings = ArcheringBowCompat.GetValue(config, "BowSettings") as IDictionary;
			if (bowSettings == null)
			{
				return null;
			}
			ValueTuple<object, string> valueTuple = ArcheringBowCompat.ResolveBowStats(item.Code, bowSettings);
			object bowStats = valueTuple.Item1;
			string matchedKey = valueTuple.Item2;
			if (bowStats == null)
			{
				return null;
			}
			IDictionary arrowBreakChances = ArcheringBowCompat.GetValue(config, "ArrowBreakChances") as IDictionary;
			return new ArcheringBowAimingSettings
			{
				MatchedKey = matchedKey,
				Damage = Math.Max(0f, ArcheringBowCompat.GetFloat(bowStats, "Damage", 0f)),
				DefaultDamage = ArcheringBowCompat.ResolveDefaultDamage(item.Code, matchedKey),
				DamageMult = Math.Max(0f, ArcheringBowCompat.GetFloat(config, "DamageMult", 1f)),
				DrawTime = Math.Max(0.05f, ArcheringBowCompat.GetFloat(bowStats, "DrawTime", 1.2f)),
				DrawWeight = Math.Max(0.05f, ArcheringBowCompat.GetFloat(bowStats, "DrawWeight", 1.5f)),
				SwayAmplitude = Math.Max(0f, ArcheringBowCompat.GetFloat(config, "SwayAmplitude", 1f)),
				SwaySpeed = Math.Max(0f, ArcheringBowCompat.GetFloat(config, "SwaySpeed", 1f)),
				ChargeAmplitude = Math.Max(0f, ArcheringBowCompat.GetFloat(config, "ChargeAmplitude", 1f)),
				RunPenaltyMultiplier = Math.Max(1f, ArcheringBowCompat.GetFloat(config, "RunPenaltyMultiplier", 9.5f)),
				HoldBreathMultiplier = Math.Max(0f, ArcheringBowCompat.GetFloat(config, "HoldBreathMultiplier", 0.5f)),
				BreathRecoveryMultiplier = Math.Max(0f, ArcheringBowCompat.GetFloat(config, "BreathRecoveryMultiplier", 1.67f)),
				BreathDuration = Math.Max(0.1f, ArcheringBowCompat.GetFloat(config, "BreathDuration", 2f)),
				BreathRecoveryDuration = Math.Max(0.1f, ArcheringBowCompat.GetFloat(config, "BreathRecoveryDuration", 5f)),
				DrawWeightDropPower = ArcheringBowCompat.GetFloat(config, "DrawWeightDropPower", 0.65f),
				DrawWeightChargePower = ArcheringBowCompat.GetFloat(config, "DrawWeightChargePower", 0.85f),
				EnableSwaying = ArcheringBowCompat.GetBool(config, "EnableSwaying", true),
				EnableBowDamageTweaks = ArcheringBowCompat.GetBool(config, "EnableBowDamageTweaks", true),
				EnableDrawTimeTweaks = ArcheringBowCompat.GetBool(config, "EnableDrawTimeTweaks", true),
				EnableDrawTimeArrowDamageTweaks = ArcheringBowCompat.GetBool(config, "EnableDrawTimeArrowDamageTweaks", true),
				EnableHoldBreath = ArcheringBowCompat.GetBool(config, "EnableHoldBreath", true),
				EnableDenockingWithLeftClick = ArcheringBowCompat.GetBool(config, "EnableDenockingWithLeftClick", true),
				EnableArcheringChargedReticle = ArcheringBowCompat.GetBool(config, "EnableArcheringChargedReticle", true),
				EnableSpeedIncreasesWithDrawWeight = ArcheringBowCompat.GetBool(config, "EnableSpeedIncreasesWithDrawWeight", true),
				EnableArrowBreakTweaks = ArcheringBowCompat.GetBool(config, "EnableArrowBreakTweaks", true),
				DisableVanillaReticle = ArcheringBowCompat.GetBool(config, "DisableVanillaReticle", true),
				ArrowBreakChances = ArcheringBowCompat.CopyFloatRules(arrowBreakChances)
			};
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x00053750 File Offset: 0x00051950
		public float GetVelocityMultiplier(Item item)
		{
			ArcheringBowAimingSettings settings = this.GetSettings(item);
			if (settings == null || !settings.EnableSpeedIncreasesWithDrawWeight)
			{
				return 1f;
			}
			return settings.DrawWeightSpeedMultiplier;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00053784 File Offset: 0x00051984
		public float GetGravityFactorMultiplier(Item item)
		{
			ArcheringBowAimingSettings settings = this.GetSettings(item);
			if (settings == null || settings.EnableSpeedIncreasesWithDrawWeight)
			{
				return 1f;
			}
			float speedMultiplier = settings.DrawWeightSpeedMultiplier;
			return Math.Max(0.01f, 1f / (speedMultiplier * speedMultiplier));
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x000537C4 File Offset: 0x000519C4
		public float GetBowDamageMultiplier(Item item)
		{
			ArcheringBowAimingSettings settings = this.GetSettings(item);
			if (settings == null || !settings.EnableBowDamageTweaks)
			{
				return 1f;
			}
			float multiplier = settings.DamageMult;
			if (settings.Damage > 0f && settings.DefaultDamage > 0f)
			{
				multiplier *= settings.Damage / settings.DefaultDamage;
			}
			return Math.Max(0f, multiplier);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0005382C File Offset: 0x00051A2C
		public float GetDrawTimeArrowDamageMultiplier(Item item)
		{
			ArcheringBowAimingSettings settings = this.GetSettings(item);
			if (settings == null || !settings.EnableDrawTimeArrowDamageTweaks)
			{
				return 1f;
			}
			return Math.Max(0f, settings.DrawTime / 0.65f);
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00053870 File Offset: 0x00051A70
		public void ApplyArrowBreakChance(Item bowItem, Item projectileItem, ProjectileStats stats)
		{
			ArcheringBowAimingSettings settings = this.GetSettings(bowItem);
			if (settings == null || !settings.EnableArrowBreakTweaks || projectileItem.Code == null)
			{
				return;
			}
			float? breakChance = ArcheringBowCompat.ResolveFloatRule(projectileItem.Code, settings.ArrowBreakChances);
			if (breakChance == null)
			{
				return;
			}
			stats.DropChance = Math.Clamp(1f - breakChance.Value, 0f, 1f);
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x000538E4 File Offset: 0x00051AE4
		[NullableContext(2)]
		private object GetConfig()
		{
			object system = this.GetModSystem();
			if (system == null)
			{
				return null;
			}
			return ArcheringBowCompat.GetValue(system, "Config");
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00053908 File Offset: 0x00051B08
		[NullableContext(2)]
		private object GetModSystem()
		{
			if (this._modSystem != null)
			{
				return this._modSystem;
			}
			if (this._modSystemType == null)
			{
				IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
				Func<Assembly, IEnumerable<Type>> func;
				if ((func = ArcheringBowCompat.<>O.<0>__GetLoadableTypes) == null)
				{
					func = (ArcheringBowCompat.<>O.<0>__GetLoadableTypes = new Func<Assembly, IEnumerable<Type>>(ArcheringBowCompat.GetLoadableTypes));
				}
				this._modSystemType = Enumerable.FirstOrDefault<Type>(Enumerable.SelectMany<Assembly, Type>(assemblies, func), (Type type) => type.FullName == "Archering.ArcheringModSystem");
			}
			if (this._modSystemType == null)
			{
				this.WarnMissingSystem("Archering is loaded, but its mod system type was not found.");
				return null;
			}
			MethodInfo method = Enumerable.FirstOrDefault<MethodInfo>(Enumerable.OrderBy<MethodInfo, int>(Enumerable.Where<MethodInfo>(this._api.ModLoader.GetType().GetMethods(20), (MethodInfo methodInfo) => methodInfo.Name == "GetModSystem" && methodInfo.IsGenericMethodDefinition), (MethodInfo methodInfo) => methodInfo.GetParameters().Length), delegate(MethodInfo methodInfo)
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				return parameters.Length == 0 || (parameters.Length == 1 && parameters[0].ParameterType == typeof(bool));
			});
			if (method == null)
			{
				this.WarnMissingSystem("Archering is loaded, but ModLoader.GetModSystem<T>() was not found.");
				return null;
			}
			try
			{
				MethodInfo genericMethod = method.MakeGenericMethod(new Type[]
				{
					this._modSystemType
				});
				object[] array;
				if (genericMethod.GetParameters().Length != 0)
				{
					(array = new object[1])[0] = false;
				}
				else
				{
					array = null;
				}
				object[] arguments = array;
				this._modSystem = genericMethod.Invoke(this._api.ModLoader, arguments);
			}
			catch (Exception exception)
			{
				this.WarnMissingSystem("Could not read Archering mod system: " + exception.GetBaseException().Message);
			}
			return this._modSystem;
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00053AB8 File Offset: 0x00051CB8
		private void WarnMissingSystem(string message)
		{
			if (this._warnedMissingSystem)
			{
				return;
			}
			this._warnedMissingSystem = true;
			LoggerUtil.Warn(this._api, typeof(ArcheringBowCompat), message);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00053AE0 File Offset: 0x00051CE0
		private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
		{
			IEnumerable<Type> result;
			try
			{
				result = assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				result = Enumerable.OfType<Type>(ex.Types);
			}
			catch
			{
				result = Array.Empty<Type>();
			}
			return result;
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00053B2C File Offset: 0x00051D2C
		[return: TupleElementNames(new string[]
		{
			"stats",
			"matchedKey"
		})]
		[return: Nullable(new byte[]
		{
			0,
			2,
			1
		})]
		private static ValueTuple<object, string> ResolveBowStats(AssetLocation bowCode, IDictionary bowSettings)
		{
			string bowPath = bowCode.Path;
			string fullCode = bowCode.ToString();
			foreach (object obj in bowSettings)
			{
				DictionaryEntry entry = (DictionaryEntry)obj;
				string key = entry.Key as string;
				if (key != null && (key.Equals(bowPath, 5) || key.Equals(fullCode, 5)))
				{
					return new ValueTuple<object, string>(entry.Value, key);
				}
			}
			foreach (object obj2 in bowSettings)
			{
				DictionaryEntry entry2 = (DictionaryEntry)obj2;
				string key2 = entry2.Key as string;
				if (key2 != null && (key2.Contains('*') || key2.Contains('?')) && ArcheringBowCompat.MatchesRule(key2, bowCode))
				{
					return new ValueTuple<object, string>(entry2.Value, key2);
				}
			}
			if (!bowSettings.Contains("bow-generic"))
			{
				return new ValueTuple<object, string>(null, "");
			}
			return new ValueTuple<object, string>(bowSettings["bow-generic"], "bow-generic");
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x00053C7C File Offset: 0x00051E7C
		private static float ResolveDefaultDamage(AssetLocation bowCode, string matchedKey)
		{
			float exactDamage;
			if (ArcheringBowCompat.DefaultBowDamageByKey.TryGetValue(matchedKey, ref exactDamage))
			{
				return exactDamage;
			}
			foreach (KeyValuePair<string, float> keyValuePair in ArcheringBowCompat.DefaultBowDamageByKey)
			{
				string text;
				float num;
				keyValuePair.Deconstruct(ref text, ref num);
				string key = text;
				float damage = num;
				if (ArcheringBowCompat.MatchesRule(key, bowCode))
				{
					return damage;
				}
			}
			return 0f;
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00053D00 File Offset: 0x00051F00
		private static Dictionary<string, float> CopyFloatRules([Nullable(2)] IDictionary source)
		{
			Dictionary<string, float> result = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);
			if (source == null)
			{
				return result;
			}
			foreach (object obj in source)
			{
				DictionaryEntry entry = (DictionaryEntry)obj;
				string key = entry.Key as string;
				if (key != null)
				{
					result[key] = ArcheringBowCompat.ToFloat(entry.Value, 0f);
				}
			}
			return result;
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00053D8C File Offset: 0x00051F8C
		private static float? ResolveFloatRule(AssetLocation code, IReadOnlyDictionary<string, float> rules)
		{
			string path = code.Path;
			string full = code.ToString();
			string text;
			float num;
			foreach (KeyValuePair<string, float> keyValuePair in rules)
			{
				keyValuePair.Deconstruct(ref text, ref num);
				string key = text;
				float value = num;
				if (key.Equals(path, 5) || key.Equals(full, 5))
				{
					return new float?(value);
				}
			}
			foreach (KeyValuePair<string, float> keyValuePair in rules)
			{
				keyValuePair.Deconstruct(ref text, ref num);
				string key2 = text;
				float value2 = num;
				if ((key2.Contains('*') || key2.Contains('?')) && ArcheringBowCompat.MatchesRule(key2, code))
				{
					return new float?(value2);
				}
			}
			return default(float?);
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00053E8C File Offset: 0x0005208C
		private static bool MatchesRule(string key, AssetLocation code)
		{
			if (key.Contains(':'))
			{
				return WildcardUtil.Match(new AssetLocation(key), code);
			}
			return WildcardUtil.Match(key, code.Path) || WildcardUtil.Match(key, code.ToString());
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00053EC4 File Offset: 0x000520C4
		[return: Nullable(2)]
		private static object GetValue(object instance, string name)
		{
			Type type = instance.GetType();
			FieldInfo field = type.GetField(name, 52);
			object result;
			if ((result = ((field != null) ? field.GetValue(instance) : null)) == null)
			{
				PropertyInfo property = type.GetProperty(name, 52);
				if (property == null)
				{
					return null;
				}
				result = property.GetValue(instance);
			}
			return result;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00053F07 File Offset: 0x00052107
		private static float GetFloat(object instance, string name, float fallback)
		{
			return ArcheringBowCompat.ToFloat(ArcheringBowCompat.GetValue(instance, name), fallback);
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00053F18 File Offset: 0x00052118
		[NullableContext(2)]
		private static float ToFloat(object value, float fallback)
		{
			float result;
			if (value is float)
			{
				float floatValue = (float)value;
				result = floatValue;
			}
			else if (value is double)
			{
				double doubleValue = (double)value;
				result = (float)doubleValue;
			}
			else if (value is int)
			{
				int intValue = (int)value;
				result = (float)intValue;
			}
			else if (value is long)
			{
				long longValue = (long)value;
				result = (float)longValue;
			}
			else
			{
				result = fallback;
			}
			return result;
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00053F88 File Offset: 0x00052188
		private static bool GetBool(object instance, string name, bool fallback)
		{
			object value = ArcheringBowCompat.GetValue(instance, name);
			if (value is bool)
			{
				return (bool)value;
			}
			return fallback;
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00053FB4 File Offset: 0x000521B4
		// Note: this type is marked as 'beforefieldinit'.
		static ArcheringBowCompat()
		{
			Dictionary<string, float> dictionary = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);
			dictionary["bow-crude"] = 2f;
			dictionary["bow-simple"] = 5f;
			dictionary["bow-long"] = 15.9f;
			dictionary["bow-recurve"] = 10.8f;
			dictionary["bowsbykanahaku:hornybow-nomad*"] = 15.9f;
			dictionary["bowsbykanahaku:hornybow-yagermaster*"] = 10.8f;
			dictionary["bowsbykanahaku:expandedbow-ranger*"] = 15.9f;
			dictionary["bowsbykanahaku:expandedbow-yager*"] = 10.8f;
			ArcheringBowCompat.DefaultBowDamageByKey = dictionary;
		}

		// Token: 0x04000803 RID: 2051
		private const bool CompatibilityEnabled = false;

		// Token: 0x04000804 RID: 2052
		private const string ModId = "archering";

		// Token: 0x04000805 RID: 2053
		private const string ModSystemTypeName = "Archering.ArcheringModSystem";

		// Token: 0x04000806 RID: 2054
		private const float VanillaDrawTime = 0.65f;

		// Token: 0x04000807 RID: 2055
		private static readonly Dictionary<string, float> DefaultBowDamageByKey;

		// Token: 0x04000808 RID: 2056
		private readonly ICoreAPI _api;

		// Token: 0x04000809 RID: 2057
		private bool _warnedMissingSystem;

		// Token: 0x0400080A RID: 2058
		[Nullable(2)]
		private Type _modSystemType;

		// Token: 0x0400080B RID: 2059
		[Nullable(2)]
		private object _modSystem;

		// Token: 0x020002BE RID: 702
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000DE9 RID: 3561
			[Nullable(new byte[]
			{
				0,
				0,
				1,
				0
			})]
			public static Func<Assembly, IEnumerable<Type>> <0>__GetLoadableTypes;
		}
	}
}
