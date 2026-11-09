using System;
using System.Collections.Generic;
using System.Globalization;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x0200002B RID: 43
	internal static class Diacritics
	{
		// Token: 0x06000120 RID: 288 RVA: 0x0000B7EC File Offset: 0x000099EC
		public static bool IsPotentialStandaloneDiacritic(string value)
		{
			return Diacritics.NonCombiningDiacritics.Contains(value);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000B7FC File Offset: 0x000099FC
		public static bool IsInCombiningDiacriticRange(string value)
		{
			if (value.Length != 1)
			{
				return false;
			}
			int num = (int)value.get_Chars(0);
			return num >= 768 && num <= 879;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000B830 File Offset: 0x00009A30
		public static bool TryCombineDiacriticWithPreviousLetter(string diacritic, string previous, out string result)
		{
			result = null;
			if (previous == null)
			{
				return false;
			}
			result = previous + diacritic;
			int num = Diacritics.MeasureDiacriticAwareLength(previous);
			int num2 = Diacritics.MeasureDiacriticAwareLength(result);
			return num == num2;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000B860 File Offset: 0x00009A60
		private static int MeasureDiacriticAwareLength(string input)
		{
			int num = 0;
			TextElementEnumerator textElementEnumerator = StringInfo.GetTextElementEnumerator(input);
			while (textElementEnumerator.MoveNext())
			{
				textElementEnumerator.GetTextElement();
				num++;
			}
			return num;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000B88C File Offset: 0x00009A8C
		// Note: this type is marked as 'beforefieldinit'.
		static Diacritics()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("´");
			hashSet.Add("^");
			hashSet.Add("ˆ");
			hashSet.Add("¨");
			hashSet.Add("©");
			hashSet.Add("™");
			hashSet.Add("®");
			hashSet.Add("`");
			hashSet.Add("˜");
			hashSet.Add("∼");
			hashSet.Add("¸");
			Diacritics.NonCombiningDiacritics = hashSet;
		}

		// Token: 0x040000A9 RID: 169
		private static readonly HashSet<string> NonCombiningDiacritics;
	}
}
