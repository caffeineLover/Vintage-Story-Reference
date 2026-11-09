using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000051 RID: 81
	[NullableContext(1)]
	[Nullable(0)]
	internal static class WeaponQuenchTooltipPatch
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x0001B2C8 File Offset: 0x000194C8
		internal static void NormalizeTooltip(ItemSlot inSlot, StringBuilder dsc)
		{
			ItemStack stack = inSlot.Itemstack;
			if (inSlot.Empty || ((stack != null) ? stack.Collectible : null) == null || QuenchableStateUtil.GetKind(stack) != "weapon")
			{
				return;
			}
			float previousStoredDamageMultiplier = 1f + Math.Max(0f, stack.Attributes.GetFloat("powervalue", 0f));
			if (CraftedArmorQuenchStatePatch.NormalizeWeaponQuenchDamageBuff(stack))
			{
				inSlot.MarkDirty();
			}
			float damageMultiplier = QuenchableStatUtil.GetAttackPowerMultiplier(stack);
			List<string> list = new List<string>();
			list.Add(Lang.Get("Attack power: {0} damage", new object[]
			{
				stack.Collectible.GetAttackPower(stack).ToString("0.#", CultureInfo.CurrentCulture)
			}));
			list.Add(Lang.Get("Attack tier: {0}", new object[]
			{
				stack.Collectible.GetToolTier(inSlot)
			}));
			list.Add(Lang.Get("mulbuff-hardened-attackpower", new object[]
			{
				damageMultiplier - 1f,
				0
			}));
			list.Add(Lang.Get("mulbuff-hardened-attackpower", new object[]
			{
				previousStoredDamageMultiplier - 1f,
				0
			}));
			List<string> linesToRemove = list;
			WeaponQuenchTooltipPatch.RemoveTooltipLines(dsc, linesToRemove);
			if (damageMultiplier <= 1.0001f)
			{
				return;
			}
			string bonusText = ((damageMultiplier - 1f) * 100f).ToString("0.#", CultureInfo.CurrentCulture);
			WeaponQuenchTooltipPatch.AppendLineOnce(dsc, "<font color=\"#00bb00\">" + Lang.Get("combatoverhaul:quenchable-weapon-damage-multiplier", new object[]
			{
				bonusText
			}) + "</font>");
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001B468 File Offset: 0x00019668
		private static void RemoveTooltipLines(StringBuilder dsc, IReadOnlyCollection<string> linesToRemove)
		{
			IEnumerable<string> enumerable = Enumerable.Where<string>(linesToRemove, (string line) => !string.IsNullOrWhiteSpace(line));
			Func<string, string> func;
			if ((func = WeaponQuenchTooltipPatch.<>O.<0>__NormalizeLine) == null)
			{
				func = (WeaponQuenchTooltipPatch.<>O.<0>__NormalizeLine = new Func<string, string>(WeaponQuenchTooltipPatch.NormalizeLine));
			}
			HashSet<string> normalizedTargets = Enumerable.ToHashSet<string>(Enumerable.Select<string, string>(enumerable, func), StringComparer.Ordinal);
			if (normalizedTargets.Count == 0)
			{
				return;
			}
			string[] array = dsc.ToString().Replace("\r\n", "\n", 4).Split('\n', 0);
			dsc.Clear();
			foreach (string line2 in array)
			{
				if (!normalizedTargets.Contains(WeaponQuenchTooltipPatch.NormalizeLine(line2)))
				{
					dsc.AppendLine(line2);
				}
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001B51C File Offset: 0x0001971C
		private static string NormalizeLine(string line)
		{
			return line.Replace("<font color=\"#00bb00\">", "", 4).Replace("<font color=\"#bb0000\">", "", 4).Replace("</font>", "", 4).Trim();
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001B554 File Offset: 0x00019754
		private static void AppendLineOnce(StringBuilder dsc, string line)
		{
			if (string.IsNullOrWhiteSpace(line) || dsc.ToString().Contains(line, 4))
			{
				return;
			}
			dsc.AppendLine(line);
		}

		// Token: 0x020001F5 RID: 501
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000B5A RID: 2906
			[Nullable(0)]
			public static Func<string, string> <0>__NormalizeLine;
		}
	}
}
