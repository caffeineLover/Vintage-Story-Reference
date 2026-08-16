using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000053 RID: 83
	[NullableContext(1)]
	[Nullable(0)]
	[HarmonyPatch(typeof(ItemSpear), "GetHeldItemInfo")]
	public static class SpearTooltipPatches
	{
		// Token: 0x060004AE RID: 1198 RVA: 0x0001BB84 File Offset: 0x00019D84
		[HarmonyPostfix]
		public static void ItemSpear_GetHeldItemInfo_Postfix(ItemSlot inSlot, StringBuilder dsc)
		{
			string text = dsc.ToString();
			text = SpearTooltipPatches.ReplaceRangeWithExplicitValue(text, "One-handed");
			text = SpearTooltipPatches.ReplaceRangeWithExplicitValue(text, "Two-handed");
			text = SpearTooltipProficiencies.InsertIfMissing(text);
			dsc.Clear();
			dsc.Append(text);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001BBC8 File Offset: 0x00019DC8
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
