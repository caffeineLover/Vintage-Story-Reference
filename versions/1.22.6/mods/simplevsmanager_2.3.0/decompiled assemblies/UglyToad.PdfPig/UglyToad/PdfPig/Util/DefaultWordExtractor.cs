using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Content;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x0200002A RID: 42
	public class DefaultWordExtractor : IWordExtractor
	{
		// Token: 0x0600011B RID: 283 RVA: 0x0000B7B4 File Offset: 0x000099B4
		public IEnumerable<Word> GetWords(IReadOnlyList<Letter> letters)
		{
			IOrderedEnumerable<Letter> orderedEnumerable = Enumerable.ThenBy<Letter, double>(Enumerable.OrderByDescending<Letter, double>(letters, (Letter x) => x.Location.Y), (Letter x) => x.Location.X);
			List<Letter> lettersSoFar = new List<Letter>(10);
			Dictionary<double, Dictionary<double, int>> gapCountsSoFarByFontSize = new Dictionary<double, Dictionary<double, int>>();
			double? y = default(double?);
			double? num = default(double?);
			Letter letter2 = null;
			foreach (Letter letter in orderedEnumerable)
			{
				if (y == null)
				{
					y = new double?(letter.Location.Y);
				}
				if (num == null)
				{
					num = new double?(letter.Location.X);
				}
				if (letter2 == null)
				{
					if (!string.IsNullOrWhiteSpace(letter.Value))
					{
						lettersSoFar.Add(letter);
						letter2 = letter;
						y = new double?(letter.Location.Y);
						num = new double?(letter.Location.X);
					}
				}
				else if (letter.Location.Y < y.Value - 0.5)
				{
					if (lettersSoFar.Count > 0)
					{
						yield return DefaultWordExtractor.GenerateWord(lettersSoFar);
						lettersSoFar.Clear();
					}
					if (!string.IsNullOrWhiteSpace(letter.Value))
					{
						lettersSoFar.Add(letter);
					}
					y = new double?(letter.Location.Y);
					num = new double?(letter.Location.X);
					letter2 = letter;
				}
				else
				{
					double num2 = Math.Max(letter2.GlyphRectangle.Height, letter.GlyphRectangle.Height);
					double num3 = letter.Location.X - (letter2.Location.X + letter2.Width);
					bool flag = letter.Location.X < num.Value - 1.0;
					bool flag2 = num3 > num2 * 0.39;
					bool flag3 = string.IsNullOrWhiteSpace(letter.Value);
					bool flag4 = !string.Equals(letter.FontName, letter2.FontName, 5) && num3 > letter.Width * 0.1;
					bool flag5 = Math.Abs(letter.FontSize - letter2.FontSize) > 0.1;
					bool flag6 = letter.TextOrientation != letter2.TextOrientation;
					bool flag7 = false;
					if (!flag5 && letter.FontSize > 0.0 && num3 >= 0.0)
					{
						double num4 = Math.Round(letter.FontSize);
						Dictionary<double, int> dictionary;
						if (!gapCountsSoFarByFontSize.TryGetValue(num4, ref dictionary))
						{
							dictionary = new Dictionary<double, int>();
							gapCountsSoFarByFontSize[num4] = dictionary;
						}
						double num5 = Math.Round(num3, 2);
						if (!dictionary.ContainsKey(num5))
						{
							dictionary[num5] = 0;
						}
						Dictionary<double, int> dictionary2 = dictionary;
						double num6 = num5;
						int num7 = dictionary2[num6];
						dictionary2[num6] = num7 + 1;
						if (dictionary.Count > 1 && num3 > num2 * 0.16)
						{
							KeyValuePair<double, int> keyValuePair = Enumerable.First<KeyValuePair<double, int>>(Enumerable.OrderByDescending<KeyValuePair<double, int>, int>(dictionary, (KeyValuePair<double, int> x) => x.Value));
							if (num3 > keyValuePair.Key * 5.0 && keyValuePair.Value > 1)
							{
								flag7 = true;
							}
						}
					}
					if ((flag || flag2 || flag3 || flag4 || flag5 || flag6 || flag7) && lettersSoFar.Count > 0)
					{
						yield return DefaultWordExtractor.GenerateWord(lettersSoFar);
						lettersSoFar.Clear();
					}
					if (!string.IsNullOrWhiteSpace(letter.Value))
					{
						lettersSoFar.Add(letter);
					}
					letter2 = letter;
					num = new double?(letter.Location.X);
					letter = null;
				}
			}
			IEnumerator<Letter> enumerator = null;
			if (lettersSoFar.Count > 0)
			{
				yield return DefaultWordExtractor.GenerateWord(lettersSoFar);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000B7C4 File Offset: 0x000099C4
		private static Word GenerateWord(List<Letter> letters)
		{
			return new Word(Enumerable.ToList<Letter>(letters));
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000B7D1 File Offset: 0x000099D1
		public static IWordExtractor Instance { get; } = new DefaultWordExtractor();

		// Token: 0x0600011E RID: 286 RVA: 0x0000B7D8 File Offset: 0x000099D8
		private DefaultWordExtractor()
		{
		}
	}
}
