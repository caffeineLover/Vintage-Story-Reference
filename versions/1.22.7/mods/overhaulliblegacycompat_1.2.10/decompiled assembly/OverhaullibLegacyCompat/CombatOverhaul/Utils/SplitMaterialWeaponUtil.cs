using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace CombatOverhaul.Utils
{
	// Token: 0x0200002E RID: 46
	[NullableContext(1)]
	[Nullable(0)]
	public static class SplitMaterialWeaponUtil
	{
		// Token: 0x06000311 RID: 785 RVA: 0x0000DFCD File Offset: 0x0000C1CD
		public static void SetApi(ICoreAPI api)
		{
			SplitMaterialWeaponUtil._world = api.World;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000DFDC File Offset: 0x0000C1DC
		[NullableContext(2)]
		public static bool Normalize(ItemStack stack)
		{
			SplitMaterialInfo materials;
			if (((stack != null) ? stack.Collectible : null) == null || !SplitMaterialWeaponUtil.TryGetSplitMaterials(stack, out materials))
			{
				return false;
			}
			if (stack.Attributes == null)
			{
				stack.Attributes = new TreeAttribute();
			}
			bool changed = false;
			changed |= SplitMaterialWeaponUtil.SetString(stack.Attributes, "splitBodyMaterial", materials.BodyMaterial);
			changed |= SplitMaterialWeaponUtil.SetString(stack.Attributes, "splitBladeMaterial", materials.BladeMaterial);
			changed |= SplitMaterialWeaponUtil.SetString(stack.Attributes, "headMaterial", materials.BodyMaterial);
			changed |= SplitMaterialWeaponUtil.SetString(stack.Attributes, "bladeMaterial", materials.BladeMaterial);
			changed |= SplitMaterialWeaponUtil.SetString(stack.Attributes, "bodyTexture", SplitMaterialWeaponUtil.MaterialTexture(materials.BodyMaterial));
			changed |= SplitMaterialWeaponUtil.SetString(stack.Attributes, "bladeTexture", SplitMaterialWeaponUtil.MaterialTexture(materials.BladeMaterial));
			changed |= SplitMaterialWeaponUtil.SetString(stack.Attributes, "headTexture", SplitMaterialWeaponUtil.MaterialTexture(materials.BladeMaterial));
			int bladeTier;
			int baseTier;
			if (SplitMaterialWeaponUtil.TryGetMaterialTier(materials.BladeMaterial, out bladeTier) && SplitMaterialWeaponUtil.TryGetMaterialTier(SplitMaterialWeaponUtil.GetPrimaryMaterial(stack) ?? materials.BodyMaterial, out baseTier))
			{
				changed |= SplitMaterialWeaponUtil.SetInt(stack.Attributes, "damageTierBonus", bladeTier - baseTier);
				changed |= SplitMaterialWeaponUtil.SetInt(stack.Attributes, "thrownDamageTierBonus", bladeTier - baseTier);
			}
			changed |= SplitMaterialWeaponUtil.SetFloat(stack.Attributes, "damageMultiplier", SplitMaterialWeaponUtil.GetDamageMultiplier(materials.BladeMaterial));
			return changed | SplitMaterialWeaponUtil.SetFloat(stack.Attributes, "thrownDamageMultiplier", SplitMaterialWeaponUtil.GetDamageMultiplier(materials.BladeMaterial));
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000E179 File Offset: 0x0000C379
		[NullableContext(2)]
		public static bool TryGetSplitMaterials(ItemStack stack, out SplitMaterialInfo materials)
		{
			materials = default(SplitMaterialInfo);
			return ((stack != null) ? stack.Collectible : null) != null && (SplitMaterialWeaponUtil.TryReadMaterialsFromAttributes(stack, out materials) || SplitMaterialWeaponUtil.TryReadMaterialsFromToolsmithHead(stack, out materials) || SplitMaterialWeaponUtil.TryReadMaterialsFromVariants(stack, out materials));
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000E1B4 File Offset: 0x0000C3B4
		public static string GetHeldItemName([Nullable(2)] ItemStack stack, string baseName)
		{
			SplitMaterialWeaponUtil.Normalize(stack);
			SplitMaterialInfo materials;
			if (!SplitMaterialWeaponUtil.TryGetSplitMaterials(stack, out materials) || !materials.IsMixed)
			{
				return baseName;
			}
			string cleanName = SplitMaterialWeaponUtil.StripTrailingMaterialSuffix(baseName);
			return Lang.Get("combatoverhaul:split-material-item-name", new object[]
			{
				cleanName,
				SplitMaterialWeaponUtil.MaterialName(materials.BladeMaterial),
				SplitMaterialWeaponUtil.MaterialName(materials.BodyMaterial)
			});
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000E218 File Offset: 0x0000C418
		public static void AppendTooltip([Nullable(2)] ItemStack stack, StringBuilder dsc)
		{
			SplitMaterialWeaponUtil.Normalize(stack);
			SplitMaterialInfo materials;
			if (!SplitMaterialWeaponUtil.TryGetSplitMaterials(stack, out materials) || !materials.IsMixed)
			{
				return;
			}
			SplitMaterialWeaponUtil.AppendLineOnce(dsc, Lang.Get("combatoverhaul:split-material-description", new object[]
			{
				SplitMaterialWeaponUtil.MaterialName(materials.BladeMaterial),
				SplitMaterialWeaponUtil.MaterialName(materials.BodyMaterial)
			}));
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000E274 File Offset: 0x0000C474
		[NullableContext(2)]
		public static bool TryGetBodyMaxDurability(ItemStack stack, int currentMaxDurability, out int maxDurability)
		{
			maxDurability = currentMaxDurability;
			if (currentMaxDurability > 0)
			{
				bool flag;
				if (stack == null)
				{
					flag = (null != null);
				}
				else
				{
					CollectibleObject collectible = stack.Collectible;
					flag = (((collectible != null) ? collectible.Attributes : null) != null);
				}
				SplitMaterialInfo materials;
				int bodyMaxDurability;
				if (flag && SplitMaterialWeaponUtil.TryGetSplitMaterials(stack, out materials) && SplitMaterialWeaponUtil.TryGetBodyMaterialMaxDurability(stack, materials, out bodyMaxDurability))
				{
					maxDurability = SplitMaterialWeaponUtil.GetBladeDurabilityAdjustedValue(materials, bodyMaxDurability);
					return maxDurability != currentMaxDurability;
				}
			}
			return false;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000E2CC File Offset: 0x0000C4CC
		private static bool TryGetBodyMaterialMaxDurability(ItemStack stack, SplitMaterialInfo materials, out int maxDurability)
		{
			maxDurability = 0;
			Dictionary<string, int> durabilityByType = SplitMaterialWeaponUtil.ReadDurabilityByType(stack.Collectible);
			if (durabilityByType == null || durabilityByType.Count == 0)
			{
				return false;
			}
			string bodyPath = SplitMaterialWeaponUtil.BuildBodyMaterialPath(stack.Collectible.Code.Path, materials);
			string bodyCode = stack.Collectible.Code.Domain + ":" + bodyPath;
			foreach (KeyValuePair<string, int> keyValuePair in durabilityByType)
			{
				string text;
				int num;
				keyValuePair.Deconstruct(ref text, ref num);
				string pattern = text;
				int durability = num;
				if (WildcardUtil.Match(pattern, bodyPath) || WildcardUtil.Match(pattern, bodyCode))
				{
					maxDurability = durability;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000E398 File Offset: 0x0000C598
		private static int GetBladeDurabilityAdjustedValue(SplitMaterialInfo materials, int bodyMaxDurability)
		{
			if (!materials.BladeMaterial.Equals("springsteel", 5))
			{
				return bodyMaxDurability;
			}
			return Math.Max(1, (int)MathF.Round((float)bodyMaxDurability * 1.2f));
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000E3C4 File Offset: 0x0000C5C4
		[NullableContext(2)]
		public static void NormalizeDurabilityIfNeeded(ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null || stack.Attributes == null || !stack.Attributes.HasAttribute("durability"))
			{
				return;
			}
			int maxDurability = stack.Collectible.GetMaxDurability(stack);
			if (maxDurability <= 0)
			{
				return;
			}
			int currentDurability = stack.Collectible.GetRemainingDurability(stack);
			if (currentDurability <= 0 || currentDurability > maxDurability)
			{
				stack.Attributes.RemoveAttribute("durability");
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000E434 File Offset: 0x0000C634
		private static bool TryReadMaterialsFromAttributes(ItemStack stack, out SplitMaterialInfo materials)
		{
			materials = default(SplitMaterialInfo);
			if (stack.Attributes == null)
			{
				return false;
			}
			string body = SplitMaterialWeaponUtil.ReadString(stack.Attributes, new string[]
			{
				"splitBodyMaterial",
				"headMaterial",
				"bodyMaterial",
				"material"
			});
			string blade = SplitMaterialWeaponUtil.ReadString(stack.Attributes, new string[]
			{
				"splitBladeMaterial",
				"bladeMaterial",
				"edgeMaterial",
				"edge"
			});
			if (!SplitMaterialWeaponUtil.IsKnownMaterial(body) || !SplitMaterialWeaponUtil.IsKnownMaterial(blade))
			{
				return false;
			}
			materials = new SplitMaterialInfo(SplitMaterialWeaponUtil.NormalizeMaterial(body), SplitMaterialWeaponUtil.NormalizeMaterial(blade));
			return true;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000E4E0 File Offset: 0x0000C6E0
		private static bool TryReadMaterialsFromToolsmithHead(ItemStack stack, out SplitMaterialInfo materials)
		{
			materials = default(SplitMaterialInfo);
			if (stack.Attributes == null || !stack.Attributes.HasAttribute("tinkeredToolHead"))
			{
				return false;
			}
			ItemStack head = stack.Attributes.GetItemstack("tinkeredToolHead", null);
			if (head == null)
			{
				return false;
			}
			if (SplitMaterialWeaponUtil.TryReadMaterialsFromAttributes(head, out materials))
			{
				return true;
			}
			if (head.Collectible == null && SplitMaterialWeaponUtil._world != null)
			{
				head.ResolveBlockOrItem(SplitMaterialWeaponUtil._world);
			}
			return head.Collectible != null && (SplitMaterialWeaponUtil.TryReadMaterialsFromVariants(head, out materials) || SplitMaterialWeaponUtil.TryReadMaterialsFromCode(head.Collectible.Code.Path, out materials));
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000E578 File Offset: 0x0000C778
		private static bool TryReadMaterialsFromVariants(ItemStack stack, out SplitMaterialInfo materials)
		{
			materials = default(SplitMaterialInfo);
			CollectibleObject collectible = stack.Collectible;
			if (((collectible != null) ? collectible.Variant : null) == null)
			{
				return false;
			}
			string body = SplitMaterialWeaponUtil.ReadVariant(stack.Collectible, new string[]
			{
				"material",
				"head",
				"body",
				"metal"
			});
			string blade = SplitMaterialWeaponUtil.ReadVariant(stack.Collectible, new string[]
			{
				"edge",
				"edge1",
				"blade"
			});
			if (!SplitMaterialWeaponUtil.IsKnownMaterial(body) || !SplitMaterialWeaponUtil.IsKnownMaterial(blade))
			{
				return false;
			}
			materials = new SplitMaterialInfo(SplitMaterialWeaponUtil.NormalizeMaterial(body), SplitMaterialWeaponUtil.NormalizeMaterial(blade));
			return true;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000E628 File Offset: 0x0000C828
		private static bool TryReadMaterialsFromCode(string path, out SplitMaterialInfo materials)
		{
			materials = default(SplitMaterialInfo);
			IEnumerable<string> enumerable = path.Split('-', 3);
			Func<string, bool> func;
			if ((func = SplitMaterialWeaponUtil.<>O.<0>__IsKnownMaterial) == null)
			{
				func = (SplitMaterialWeaponUtil.<>O.<0>__IsKnownMaterial = new Func<string, bool>(SplitMaterialWeaponUtil.IsKnownMaterial));
			}
			IEnumerable<string> enumerable2 = Enumerable.Where<string>(enumerable, func);
			Func<string, string> func2;
			if ((func2 = SplitMaterialWeaponUtil.<>O.<1>__NormalizeMaterial) == null)
			{
				func2 = (SplitMaterialWeaponUtil.<>O.<1>__NormalizeMaterial = new Func<string, string>(SplitMaterialWeaponUtil.NormalizeMaterial));
			}
			string[] materialTokens = Enumerable.ToArray<string>(Enumerable.Select<string, string>(enumerable2, func2));
			if (materialTokens.Length < 2)
			{
				return false;
			}
			string bodyMaterial = materialTokens[0];
			string[] array = materialTokens;
			materials = new SplitMaterialInfo(bodyMaterial, array[array.Length - 1]);
			return true;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000E6AC File Offset: 0x0000C8AC
		[return: Nullable(new byte[]
		{
			2,
			1
		})]
		private static Dictionary<string, int> ReadDurabilityByType(CollectibleObject collectible)
		{
			JsonObject attributes = collectible.Attributes;
			JsonObject durabilityByType = (attributes != null) ? attributes["durabilitybytype"] : null;
			if (durabilityByType == null || !durabilityByType.Exists)
			{
				JsonObject attributes2 = collectible.Attributes;
				durabilityByType = ((attributes2 != null) ? attributes2["durabilityByType"] : null);
			}
			if (durabilityByType == null || !durabilityByType.Exists)
			{
				return null;
			}
			return durabilityByType.AsObject<Dictionary<string, int>>(null);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000E710 File Offset: 0x0000C910
		private static string BuildBodyMaterialPath(string path, SplitMaterialInfo materials)
		{
			string[] parts = path.Split('-', 0);
			bool replaced = false;
			for (int index = parts.Length - 1; index >= 0; index--)
			{
				if (parts[index].Equals(materials.BladeMaterial, 5))
				{
					parts[index] = materials.BodyMaterial;
					replaced = true;
					break;
				}
			}
			if (!replaced)
			{
				for (int index2 = parts.Length - 1; index2 >= 0; index2--)
				{
					if (SplitMaterialWeaponUtil.IsKnownMaterial(parts[index2]))
					{
						parts[index2] = materials.BodyMaterial;
						break;
					}
				}
			}
			return string.Join("-", parts);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000E790 File Offset: 0x0000C990
		[return: Nullable(2)]
		private static string GetPrimaryMaterial(ItemStack stack)
		{
			CollectibleObject collectible = stack.Collectible;
			if (collectible == null)
			{
				return null;
			}
			string variantMaterial = SplitMaterialWeaponUtil.ReadVariant(collectible, new string[]
			{
				"edge",
				"edge1",
				"blade",
				"material",
				"head",
				"body",
				"metal"
			});
			if (variantMaterial != null)
			{
				return SplitMaterialWeaponUtil.NormalizeMaterial(variantMaterial);
			}
			IEnumerable<string> enumerable = collectible.Code.Path.Split('-', 3);
			Func<string, bool> func;
			if ((func = SplitMaterialWeaponUtil.<>O.<0>__IsKnownMaterial) == null)
			{
				func = (SplitMaterialWeaponUtil.<>O.<0>__IsKnownMaterial = new Func<string, bool>(SplitMaterialWeaponUtil.IsKnownMaterial));
			}
			IEnumerable<string> enumerable2 = Enumerable.Where<string>(enumerable, func);
			Func<string, string> func2;
			if ((func2 = SplitMaterialWeaponUtil.<>O.<1>__NormalizeMaterial) == null)
			{
				func2 = (SplitMaterialWeaponUtil.<>O.<1>__NormalizeMaterial = new Func<string, string>(SplitMaterialWeaponUtil.NormalizeMaterial));
			}
			string[] materialTokens = Enumerable.ToArray<string>(Enumerable.Select<string, string>(enumerable2, func2));
			if (materialTokens.Length == 0)
			{
				return null;
			}
			string[] array = materialTokens;
			return array[array.Length - 1];
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000E860 File Offset: 0x0000CA60
		[return: Nullable(2)]
		private static string ReadVariant(CollectibleObject collectible, params string[] keys)
		{
			foreach (string key in keys)
			{
				string value;
				if (collectible.Variant.TryGetValue(key, ref value) && SplitMaterialWeaponUtil.IsKnownMaterial(value))
				{
					return value;
				}
			}
			return null;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000E89C File Offset: 0x0000CA9C
		[return: Nullable(2)]
		private static string ReadString(ITreeAttribute attributes, params string[] keys)
		{
			foreach (string key in keys)
			{
				string value = attributes.GetString(key, "");
				if (SplitMaterialWeaponUtil.IsKnownMaterial(value))
				{
					return value;
				}
			}
			return null;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000E8D5 File Offset: 0x0000CAD5
		private static bool SetString(ITreeAttribute attributes, string key, string value)
		{
			if (attributes.GetString(key, "") == value)
			{
				return false;
			}
			attributes.SetString(key, value);
			return true;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000E8F6 File Offset: 0x0000CAF6
		private static bool SetInt(ITreeAttribute attributes, string key, int value)
		{
			if (attributes.GetInt(key, -2147483648) == value)
			{
				return false;
			}
			attributes.SetInt(key, value);
			return true;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000E912 File Offset: 0x0000CB12
		private static bool SetFloat(ITreeAttribute attributes, string key, float value)
		{
			if (Math.Abs(attributes.GetFloat(key, float.NaN) - value) < 0.0001f)
			{
				return false;
			}
			attributes.SetFloat(key, value);
			return true;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000E939 File Offset: 0x0000CB39
		private static bool TryGetMaterialTier(string material, out int tier)
		{
			return SplitMaterialWeaponUtil.MaterialTiers.TryGetValue(material, ref tier);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000E947 File Offset: 0x0000CB47
		private static float GetDamageMultiplier(string material)
		{
			return CollectionExtensions.GetValueOrDefault<string, float>(SplitMaterialWeaponUtil.DamageMultipliers, material, 1f);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000E959 File Offset: 0x0000CB59
		[NullableContext(2)]
		private static bool IsKnownMaterial(string material)
		{
			return !string.IsNullOrWhiteSpace(material) && SplitMaterialWeaponUtil.MaterialTiers.ContainsKey(material);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000E970 File Offset: 0x0000CB70
		private static string NormalizeMaterial(string material)
		{
			return material.ToLowerInvariant();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000E978 File Offset: 0x0000CB78
		private static string MaterialTexture(string material)
		{
			return "game:block/metal/ingot/" + material;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000E988 File Offset: 0x0000CB88
		private static string MaterialName(string material)
		{
			string key = "material-" + material;
			string translated = Lang.Get(key, Array.Empty<object>());
			if (!translated.Equals(key, 4))
			{
				return translated;
			}
			string domainKey = "game:material-" + material;
			translated = Lang.Get(domainKey, Array.Empty<object>());
			if (!translated.Equals(domainKey, 4))
			{
				return translated;
			}
			string fallback;
			if (SplitMaterialWeaponUtil.MaterialFallbackNames.TryGetValue(material, ref fallback))
			{
				return fallback;
			}
			return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(material.Replace("-", " "));
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000EA10 File Offset: 0x0000CC10
		private static string StripTrailingMaterialSuffix(string name)
		{
			int suffixStart = name.LastIndexOf(" (", 4);
			if (suffixStart <= 0 || !name.EndsWith(")", 4))
			{
				return name;
			}
			return name.Substring(0, suffixStart);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000EA46 File Offset: 0x0000CC46
		private static void AppendLineOnce(StringBuilder dsc, string line)
		{
			if (string.IsNullOrWhiteSpace(line) || dsc.ToString().Contains(line, 4))
			{
				return;
			}
			dsc.AppendLine(line);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000EA68 File Offset: 0x0000CC68
		// Note: this type is marked as 'beforefieldinit'.
		static SplitMaterialWeaponUtil()
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			dictionary["copper"] = 1;
			dictionary["tinbronze"] = 2;
			dictionary["bismuthbronze"] = 2;
			dictionary["blackbronze"] = 2;
			dictionary["iron"] = 4;
			dictionary["meteoriciron"] = 5;
			dictionary["steel"] = 6;
			dictionary["stahl"] = 6;
			dictionary["meteoricsteel"] = 7;
			dictionary["wootz"] = 7;
			dictionary["springsteel"] = 7;
			SplitMaterialWeaponUtil.MaterialTiers = dictionary;
			Dictionary<string, float> dictionary2 = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);
			dictionary2["meteoriciron"] = 0.98f;
			dictionary2["springsteel"] = 1.05f;
			dictionary2["wootz"] = 1.05f;
			SplitMaterialWeaponUtil.DamageMultipliers = dictionary2;
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			dictionary3["copper"] = "Copper";
			dictionary3["tinbronze"] = "Tin bronze";
			dictionary3["bismuthbronze"] = "Bismuth bronze";
			dictionary3["blackbronze"] = "Black bronze";
			dictionary3["iron"] = "Iron";
			dictionary3["meteoriciron"] = "Meteoric iron";
			dictionary3["steel"] = "Steel";
			dictionary3["stahl"] = "Stahl";
			dictionary3["meteoricsteel"] = "Meteoric steel";
			dictionary3["wootz"] = "Wootz";
			dictionary3["springsteel"] = "Spring steel";
			SplitMaterialWeaponUtil.MaterialFallbackNames = dictionary3;
		}

		// Token: 0x0400012C RID: 300
		private const string ToolsmithToolHeadAttribute = "tinkeredToolHead";

		// Token: 0x0400012D RID: 301
		private const float SpringSteelBladeDurabilityMultiplier = 1.2f;

		// Token: 0x0400012E RID: 302
		[Nullable(2)]
		private static IWorldAccessor _world;

		// Token: 0x0400012F RID: 303
		private static readonly Dictionary<string, int> MaterialTiers;

		// Token: 0x04000130 RID: 304
		private static readonly Dictionary<string, float> DamageMultipliers;

		// Token: 0x04000131 RID: 305
		private static readonly Dictionary<string, string> MaterialFallbackNames;

		// Token: 0x020001C9 RID: 457
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000AC6 RID: 2758
			[Nullable(0)]
			public static Func<string, bool> <0>__IsKnownMaterial;

			// Token: 0x04000AC7 RID: 2759
			[Nullable(0)]
			public static Func<string, string> <1>__NormalizeMaterial;
		}
	}
}
