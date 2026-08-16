using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Vintagestory.API.Config;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000055 RID: 85
	public static class SpearTooltipProficiencies
	{
		// Token: 0x060004B2 RID: 1202 RVA: 0x0001BCF4 File Offset: 0x00019EF4
		[NullableContext(1)]
		public unsafe static string InsertIfMissing(string text)
		{
			string proficiencyLabel = Lang.Get("combatoverhaul:iteminfo-proficiency", new object[]
			{
				""
			});
			int placeholderIndex = proficiencyLabel.IndexOf("{0}", 4);
			if (placeholderIndex >= 0)
			{
				proficiencyLabel = proficiencyLabel.Substring(0, placeholderIndex);
			}
			if (text.Contains(proficiencyLabel, 5))
			{
				return text;
			}
			string text2 = ", ";
			InlineArray2<string> inlineArray = default(InlineArray2<string>);
			*<PrivateImplementationDetails>.InlineArrayElementRef<InlineArray2<string>, string>(ref inlineArray, 0) = Lang.Get("combatoverhaul:proficiency-meleeProficiency", Array.Empty<object>());
			*<PrivateImplementationDetails>.InlineArrayElementRef<InlineArray2<string>, string>(ref inlineArray, 1) = Lang.Get("combatoverhaul:proficiency-spearsProficiency", Array.Empty<object>());
			string statsList = string.Join(text2, <PrivateImplementationDetails>.InlineArrayAsReadOnlySpan<InlineArray2<string>, string>(inlineArray, 2));
			string proficiencyLine = Lang.Get("combatoverhaul:iteminfo-proficiency", new object[]
			{
				statsList
			});
			Match stanceMatch = Regex.Match(text, "(?m)^One-handed:.*(?:\\r?\\n|$)");
			if (!stanceMatch.Success)
			{
				stanceMatch = Regex.Match(text, "(?m)^Two-handed:.*(?:\\r?\\n|$)");
			}
			if (stanceMatch.Success)
			{
				return text.Insert(stanceMatch.Index, proficiencyLine + Environment.NewLine);
			}
			if (!text.EndsWith('\n'))
			{
				text += Environment.NewLine;
			}
			return text + proficiencyLine + Environment.NewLine;
		}
	}
}
