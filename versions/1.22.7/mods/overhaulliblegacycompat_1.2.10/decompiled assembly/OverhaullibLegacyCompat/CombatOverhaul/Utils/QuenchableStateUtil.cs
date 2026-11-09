using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Armor;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Utils
{
	// Token: 0x0200002B RID: 43
	[NullableContext(1)]
	[Nullable(0)]
	public static class QuenchableStateUtil
	{
		// Token: 0x060002EA RID: 746 RVA: 0x0000CBAC File Offset: 0x0000ADAC
		public static string GetKind([Nullable(2)] ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null)
			{
				return "";
			}
			JsonObject itemAttributes = stack.ItemAttributes;
			string explicitKind = (itemAttributes != null) ? itemAttributes["quenchBuffKind"].AsString(null) : null;
			if (!string.IsNullOrEmpty(explicitKind))
			{
				return explicitKind;
			}
			AssetLocation code2 = stack.Collectible.Code;
			string code = ((code2 != null) ? code2.ToShortString() : null) ?? "";
			if (QuenchableStateUtil.LooksLikeArmorComponent(code))
			{
				return "armor";
			}
			if (QuenchableStateUtil.LooksLikeWeaponOrWeaponPart(code))
			{
				return "weapon";
			}
			if (stack.Collectible.GetTool(new DummySlot(stack)) != null)
			{
				return "tool";
			}
			return "";
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000CC58 File Offset: 0x0000AE58
		[NullableContext(2)]
		public static bool IsArmorQuenchComponent(ItemStack stack)
		{
			return QuenchableStateUtil.GetKind(stack) == "armor";
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000CC6C File Offset: 0x0000AE6C
		[NullableContext(2)]
		public static bool IsArmorOrArmorComponent(ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null)
			{
				return false;
			}
			if (QuenchableStateUtil.IsArmorQuenchComponent(stack))
			{
				return true;
			}
			AssetLocation code2 = stack.Collectible.Code;
			string code = ((code2 != null) ? code2.ToShortString() : null) ?? "";
			return code.StartsWith("armory:armor-", 4) || code.StartsWith("game:armor-", 4) || stack.Collectible.GetCollectibleBehavior<ArmorBehavior>(true) != null;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000CCE4 File Offset: 0x0000AEE4
		[NullableContext(2)]
		public static int GetArmorQuenchState(ItemStack stack)
		{
			QuenchableStateUtil.MigrateLegacyArmorQuenchState(stack);
			if (((stack != null) ? stack.Attributes : null) == null)
			{
				return 0;
			}
			int @int = stack.Attributes.GetInt("armorQuenchLevel", 0);
			int quenchIteration = stack.Attributes.GetInt("quenchIteration", 0);
			return Math.Max(@int, quenchIteration);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000CD30 File Offset: 0x0000AF30
		[NullableContext(2)]
		public static bool HasAnyQuenchState(ItemStack stack)
		{
			if (((stack != null) ? stack.Attributes : null) == null)
			{
				return false;
			}
			if (QuenchableStateUtil.IsArmorOrArmorComponent(stack))
			{
				QuenchableStateUtil.MigrateLegacyArmorQuenchState(stack);
				if (QuenchableStateUtil.GetArmorQuenchState(stack) > 0 || QuenchableStateUtil.HasDirectArmorQuench(stack))
				{
					return true;
				}
			}
			return stack.Attributes.GetFloat("powervalue", 0f) > 0f || stack.Attributes.GetFloat("durationbonus", 0f) > 0f || stack.Attributes.GetInt("quenchIteration", 0) > 0 || QuenchableStateUtil.HasLegacyBuffStat(stack, new string[]
			{
				"attackpower",
				"miningspeed",
				"maxdurability",
				"power",
				"durability"
			});
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000CDF0 File Offset: 0x0000AFF0
		public static string GetArmorQuenchMode([Nullable(2)] ItemStack stack)
		{
			QuenchableStateUtil.MigrateLegacyArmorQuenchState(stack);
			if (((stack != null) ? stack.Attributes : null) == null)
			{
				return "";
			}
			return stack.Attributes.GetString("armorQuenchMode", "");
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000CE24 File Offset: 0x0000B024
		public static void ApplyArmorQuenchState(ItemStack stack, int state)
		{
			if (state <= 0)
			{
				stack.Attributes.RemoveAttribute("armorQuenchLevel");
				stack.Attributes.RemoveAttribute("armorQuenchMode");
				stack.Attributes.RemoveAttribute("armorDirectQuenched");
				stack.Attributes.RemoveAttribute("durationbonus");
				stack.Attributes.RemoveAttribute("powervalue");
				stack.Attributes.RemoveAttribute("quenchIteration");
				stack.Attributes.RemoveAttribute("temperIteration");
				stack.Attributes.RemoveAttribute("clayCovered");
				QuenchableStateUtil.RemoveBuffs(stack, new string[]
				{
					"attackpower",
					"miningspeed",
					"maxdurability"
				});
				QuenchableStateUtil.SanitizeArmorQuenchBuffs(stack);
				return;
			}
			stack.Attributes.SetInt("armorQuenchLevel", 1);
			stack.Attributes.SetInt("quenchIteration", 1);
			stack.Attributes.SetInt("temperIteration", 0);
			stack.Attributes.SetFloat("powervalue", 0f);
			stack.Attributes.SetFloat("durationbonus", 0.1f);
			stack.Attributes.SetBool("clayCovered", false);
			QuenchableStateUtil.RemoveBuffs(stack, new string[]
			{
				"attackpower",
				"miningspeed"
			});
			QuenchableStateUtil.SanitizeArmorQuenchBuffs(stack);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000CF74 File Offset: 0x0000B174
		public static void ApplyInheritedArmorQuenchState(ItemStack stack, bool directQuenched, int clayQuenchIteration, int clayTemperIteration)
		{
			if (!directQuenched && clayQuenchIteration <= 0)
			{
				QuenchableStateUtil.ApplyArmorQuenchState(stack, 0);
				return;
			}
			stack.Attributes.SetInt("armorQuenchLevel", 1);
			if (directQuenched)
			{
				stack.Attributes.SetBool("armorDirectQuenched", true);
			}
			else
			{
				stack.Attributes.RemoveAttribute("armorDirectQuenched");
			}
			if (clayQuenchIteration > 0)
			{
				stack.Attributes.SetString("armorQuenchMode", "clay");
				stack.Attributes.SetInt("quenchIteration", clayQuenchIteration);
				stack.Attributes.SetInt("temperIteration", Math.Clamp(clayTemperIteration, 0, clayQuenchIteration));
			}
			else
			{
				stack.Attributes.SetString("armorQuenchMode", "direct");
				stack.Attributes.SetInt("quenchIteration", 0);
				stack.Attributes.SetInt("temperIteration", 0);
			}
			stack.Attributes.SetFloat("powervalue", 0f);
			stack.Attributes.SetFloat("durationbonus", 0.1f);
			stack.Attributes.SetBool("clayCovered", false);
			QuenchableStateUtil.RemoveBuffs(stack, new string[]
			{
				"attackpower",
				"miningspeed"
			});
			QuenchableStateUtil.SanitizeArmorQuenchBuffs(stack);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000D0A0 File Offset: 0x0000B2A0
		public static void ApplyDirectArmorQuench(ItemStack stack)
		{
			int clayQuenchIteration = Math.Max(0, stack.Attributes.GetInt("quenchIteration", 0));
			int clayTemperIteration = Math.Clamp(stack.Attributes.GetInt("temperIteration", 0), 0, clayQuenchIteration);
			stack.Attributes.SetInt("armorQuenchLevel", 1);
			stack.Attributes.SetBool("armorDirectQuenched", true);
			stack.Attributes.SetString("armorQuenchMode", (clayQuenchIteration > 0) ? "clay" : "direct");
			stack.Attributes.SetInt("quenchIteration", clayQuenchIteration);
			stack.Attributes.SetInt("temperIteration", clayTemperIteration);
			stack.Attributes.SetFloat("powervalue", 0f);
			stack.Attributes.SetFloat("durationbonus", 0.1f);
			stack.Attributes.SetBool("clayCovered", false);
			QuenchableStateUtil.RemoveBuffs(stack, new string[]
			{
				"attackpower",
				"miningspeed"
			});
			QuenchableStateUtil.SanitizeArmorQuenchBuffs(stack);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000D1A0 File Offset: 0x0000B3A0
		public static void ApplyClayArmorQuench(ItemStack stack)
		{
			int quenchIteration = stack.Attributes.GetInt("quenchIteration", 0) + 1;
			stack.Attributes.SetInt("armorQuenchLevel", 1);
			stack.Attributes.SetString("armorQuenchMode", "clay");
			stack.Attributes.SetInt("quenchIteration", quenchIteration);
			stack.Attributes.SetInt("temperIteration", Math.Min(stack.Attributes.GetInt("temperIteration", 0), quenchIteration));
			stack.Attributes.SetBool("clayCovered", false);
			QuenchableStateUtil.RemoveBuffs(stack, new string[]
			{
				"attackpower",
				"miningspeed"
			});
			QuenchableStateUtil.SanitizeArmorQuenchBuffs(stack);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000D252 File Offset: 0x0000B452
		[NullableContext(2)]
		public static bool HasDirectArmorQuench(ItemStack stack)
		{
			QuenchableStateUtil.MigrateLegacyArmorQuenchState(stack);
			return ((stack != null) ? stack.Attributes : null) != null && stack.Attributes.GetBool("armorDirectQuenched", false);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000D27C File Offset: 0x0000B47C
		[NullableContext(2)]
		public static void NormalizeGenericBuffs(ItemStack stack)
		{
			QuenchableStateUtil.MigrateLegacyArmorQuenchState(stack);
			string kind = QuenchableStateUtil.GetKind(stack);
			if (kind == "armor")
			{
				QuenchableStateUtil.RemoveBuffs(stack, new string[]
				{
					"attackpower",
					"miningspeed",
					"maxdurability"
				});
				QuenchableStateUtil.SanitizeArmorQuenchBuffs(stack);
				return;
			}
			if (kind == "weapon")
			{
				QuenchableStateUtil.RemoveBuffs(stack, new string[]
				{
					"miningspeed"
				});
				return;
			}
			if (kind == "tool")
			{
				QuenchableStateUtil.RemoveBuffs(stack, new string[]
				{
					"attackpower"
				});
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000D314 File Offset: 0x0000B514
		[NullableContext(2)]
		public static void SanitizeArmorQuenchBuffs(ItemStack stack)
		{
			QuenchableStateUtil.MigrateLegacyArmorQuenchState(stack);
			if (((stack != null) ? stack.Attributes : null) == null || !QuenchableStateUtil.IsArmorOrArmorComponent(stack) || !QuenchableStateUtil.IsFerrous(stack))
			{
				return;
			}
			stack.Attributes.SetFloat("powervalue", 0f);
			ITreeAttribute buffs = stack.Attributes.GetTreeAttribute("buffs");
			if (buffs == null)
			{
				return;
			}
			List<string> toRemove = new List<string>();
			foreach (KeyValuePair<string, IAttribute> entry in buffs)
			{
				ITreeAttribute buff = entry.Value as ITreeAttribute;
				if (buff != null && QuenchableStateUtil.ShouldStripArmorQuenchStat(buff.GetString("statcode", "")))
				{
					toRemove.Add(entry.Key);
				}
			}
			foreach (string key in toRemove)
			{
				buffs.RemoveAttribute(key);
			}
			if (buffs.Count == 0)
			{
				stack.Attributes.RemoveAttribute("buffs");
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000D43C File Offset: 0x0000B63C
		[NullableContext(2)]
		public static void MigrateLegacyArmorQuenchState(ItemStack stack)
		{
			if (((stack != null) ? stack.Attributes : null) == null || !QuenchableStateUtil.IsArmorOrArmorComponent(stack) || !QuenchableStateUtil.IsFerrous(stack))
			{
				return;
			}
			ITreeAttribute attrs = stack.Attributes;
			string mode = attrs.GetString("armorQuenchMode", "");
			bool hasDirect = attrs.GetBool("armorDirectQuenched", false);
			int armorLevel = attrs.GetInt("armorQuenchLevel", 0);
			int quenchIteration = attrs.GetInt("quenchIteration", 0);
			int temperIteration = attrs.GetInt("temperIteration", 0);
			bool hasLegacyPower = attrs.GetFloat("powervalue", 0f) > 0f || QuenchableStateUtil.HasLegacyBuffStat(stack, new string[]
			{
				"attackpower",
				"power",
				"tier",
				"protection",
				"armor"
			});
			bool hasLegacyDurability = attrs.GetFloat("durationbonus", 0f) > 0f || QuenchableStateUtil.HasLegacyBuffStat(stack, new string[]
			{
				"maxdurability"
			});
			bool changed = false;
			if (hasLegacyPower && !hasDirect)
			{
				attrs.SetBool("armorDirectQuenched", true);
				hasDirect = true;
				changed = true;
			}
			if (hasLegacyDurability && quenchIteration <= 0)
			{
				quenchIteration = 1;
				attrs.SetInt("quenchIteration", quenchIteration);
				changed = true;
			}
			if (hasLegacyDurability && string.IsNullOrEmpty(mode))
			{
				attrs.SetString("armorQuenchMode", "clay");
				mode = "clay";
				changed = true;
			}
			if (hasDirect && string.IsNullOrEmpty(mode))
			{
				attrs.SetString("armorQuenchMode", "direct");
				changed = true;
			}
			int clampedTemperIteration = Math.Clamp(temperIteration, 0, Math.Max(0, quenchIteration));
			if (clampedTemperIteration != temperIteration)
			{
				attrs.SetInt("temperIteration", clampedTemperIteration);
				changed = true;
			}
			if ((hasDirect || quenchIteration > 0 || hasLegacyPower || hasLegacyDurability) && armorLevel <= 0)
			{
				attrs.SetInt("armorQuenchLevel", 1);
				changed = true;
			}
			if (changed)
			{
				attrs.SetFloat("powervalue", 0f);
				attrs.SetBool("clayCovered", false);
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000D620 File Offset: 0x0000B820
		public static void RemoveBuffs([Nullable(2)] ItemStack stack, params string[] statCodes)
		{
			if (((stack != null) ? stack.Attributes : null) == null || statCodes.Length == 0)
			{
				return;
			}
			ITreeAttribute buffs = stack.Attributes.GetTreeAttribute("buffs");
			if (buffs == null)
			{
				return;
			}
			HashSet<string> removeCodes = new HashSet<string>(statCodes, StringComparer.OrdinalIgnoreCase);
			List<string> toRemove = new List<string>();
			foreach (KeyValuePair<string, IAttribute> entry in buffs)
			{
				string key = entry.Key;
				ITreeAttribute buff = entry.Value as ITreeAttribute;
				if (buff != null)
				{
					string statCode = buff.GetString("statcode", "");
					if (removeCodes.Contains(statCode))
					{
						toRemove.Add(key);
					}
				}
			}
			foreach (string key2 in toRemove)
			{
				buffs.RemoveAttribute(key2);
			}
			if (buffs.Count == 0)
			{
				stack.Attributes.RemoveAttribute("buffs");
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000D734 File Offset: 0x0000B934
		[NullableContext(2)]
		public static bool IsFerrous(ItemStack stack)
		{
			string text;
			if (stack == null)
			{
				text = null;
			}
			else
			{
				CollectibleObject collectible = stack.Collectible;
				if (collectible == null)
				{
					text = null;
				}
				else
				{
					AssetLocation code2 = collectible.Code;
					text = ((code2 != null) ? code2.Path : null);
				}
			}
			string code = text ?? "";
			return code.Contains("-iron", 4) || code.Contains("-meteoriciron", 4) || code.Contains("-steel", 4) || QuenchableStateUtil.LooksLikeSteelBackedPlatedArmor(code);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000D7A1 File Offset: 0x0000B9A1
		private static bool LooksLikeSteelBackedPlatedArmor(string code)
		{
			return code.StartsWith("armor-", 4) && (code.Contains("-plate-gold", 4) || code.Contains("-plate-silver", 4));
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000D7D0 File Offset: 0x0000B9D0
		private static bool ShouldStripArmorQuenchStat(string statCode)
		{
			return !string.IsNullOrWhiteSpace(statCode) && (string.Equals(statCode, "attackpower", 5) || string.Equals(statCode, "miningspeed", 5) || string.Equals(statCode, "maxdurability", 5) || statCode.Contains("protection", 5) || statCode.Contains("armor", 5) || statCode.Contains("tier", 5) || statCode.Contains("power", 5));
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000D84C File Offset: 0x0000BA4C
		private static bool HasLegacyBuffStat(ItemStack stack, params string[] needles)
		{
			ITreeAttribute buffs = stack.Attributes.GetTreeAttribute("buffs");
			if (buffs == null)
			{
				return false;
			}
			foreach (KeyValuePair<string, IAttribute> entry in buffs)
			{
				ITreeAttribute buff = entry.Value as ITreeAttribute;
				if (buff != null)
				{
					string statCode = buff.GetString("statcode", "");
					if (!string.IsNullOrEmpty(statCode))
					{
						foreach (string needle in needles)
						{
							if (!string.IsNullOrEmpty(needle) && (string.Equals(statCode, needle, 5) || statCode.Contains(needle, 5)))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000D918 File Offset: 0x0000BB18
		private static bool LooksLikeArmorComponent(string code)
		{
			return (code.StartsWith("armory:part-", 4) && code.Contains("plate", 4)) || code == "game:metalplate-iron" || code == "game:metalplate-meteoriciron" || code == "game:metalplate-steel" || code == "game:metalchain-iron" || code == "game:metalchain-meteoriciron" || code == "game:metalchain-steel" || code == "game:metalscale-iron" || code == "game:metalscale-meteoriciron" || code == "game:metalscale-steel";
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000D9B8 File Offset: 0x0000BBB8
		private static bool LooksLikeWeaponOrWeaponPart(string code)
		{
			string path = code.Contains(':') ? code.Split(':', 2, 0)[1] : code;
			return path.Contains("blade", 4) || path.Contains("spear", 4) || path.Contains("sword", 4) || path.Contains("sabre", 4) || path.Contains("javelin", 4) || path.Contains("halberd", 4) || path.Contains("mace", 4) || path.Contains("poleaxe", 4) || path.Contains("longaxe", 4) || path.Contains("pike", 4) || path.Contains("dagger", 4);
		}

		// Token: 0x0400011A RID: 282
		public const string KindAttribute = "quenchBuffKind";

		// Token: 0x0400011B RID: 283
		public const string ArmorKind = "armor";

		// Token: 0x0400011C RID: 284
		public const string WeaponKind = "weapon";

		// Token: 0x0400011D RID: 285
		public const string ToolKind = "tool";

		// Token: 0x0400011E RID: 286
		public const string ArmorQuenchModeAttribute = "armorQuenchMode";

		// Token: 0x0400011F RID: 287
		public const string ArmorQuenchModeDirect = "direct";

		// Token: 0x04000120 RID: 288
		public const string ArmorQuenchModeClay = "clay";

		// Token: 0x04000121 RID: 289
		public const string ArmorDirectQuenchedAttribute = "armorDirectQuenched";
	}
}
