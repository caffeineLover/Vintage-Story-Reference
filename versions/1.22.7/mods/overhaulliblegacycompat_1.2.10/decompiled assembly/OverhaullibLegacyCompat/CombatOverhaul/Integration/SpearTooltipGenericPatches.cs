using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using HarmonyLib;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000054 RID: 84
	[NullableContext(1)]
	[Nullable(0)]
	[HarmonyPatch(typeof(CollectibleObject), "GetHeldItemInfo")]
	public static class SpearTooltipGenericPatches
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x0001BC4C File Offset: 0x00019E4C
		[HarmonyPostfix]
		public static void CollectibleObject_GetHeldItemInfo_Postfix(CollectibleObject __instance, ItemSlot inSlot, StringBuilder dsc)
		{
			string text2;
			if (__instance == null)
			{
				text2 = null;
			}
			else
			{
				AssetLocation code = __instance.Code;
				text2 = ((code != null) ? code.Path : null);
			}
			string path = text2 ?? "";
			if (!path.Contains("spear", 5))
			{
				return;
			}
			if (path.Contains("spearhead", 5))
			{
				return;
			}
			if (path.Contains("head", 5))
			{
				return;
			}
			string text = dsc.ToString();
			text = SpearTooltipGenericPatches.ReplaceRangeWithExplicitValue(text, "One-handed");
			text = SpearTooltipGenericPatches.ReplaceRangeWithExplicitValue(text, "Two-handed");
			text = SpearTooltipProficiencies.InsertIfMissing(text);
			dsc.Clear();
			dsc.Append(text);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001BCE0 File Offset: 0x00019EE0
		private static string ReplaceRangeWithExplicitValue(string text, string stanceLabel)
		{
			string pattern = "(?m)^(" + Regex.Escape(stanceLabel) + ":\\s*)(\\d+)\\s*-\\s*(\\d+)(\\s*\\((\\d+)(?:\\s*-\\s*(\\d+))?\\s+tier\\).*)$";
			return Regex.Replace(text, pattern, delegate(Match match)
			{
				string value = match.Groups[1].Value;
				string max = match.Groups[3].Value;
				string minTier = match.Groups[5].Value;
				string maxTier = match.Groups[6].Success ? match.Groups[6].Value : minTier;
				string suffix = match.Groups[4].Value;
				suffix = Regex.Replace(suffix, "\\(\\d+(?:\\s*-\\s*\\d+)?\\s+tier\\)", "(" + maxTier + " tier)");
				return value + max + suffix;
			});
		}
	}
}
