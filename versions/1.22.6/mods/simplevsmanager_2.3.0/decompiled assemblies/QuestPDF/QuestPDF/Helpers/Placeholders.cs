using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Helpers
{
	// Token: 0x0200007A RID: 122
	[NullableContext(1)]
	[Nullable(0)]
	public static class Placeholders
	{
		// Token: 0x0600032D RID: 813 RVA: 0x000084E0 File Offset: 0x000066E0
		static Placeholders()
		{
			SkNativeDependencyCompatibilityChecker.Test();
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00008624 File Offset: 0x00006824
		private static string[] LoadLatinWords()
		{
			string[] result;
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("QuestPDF.Resources.LatinWords.txt"))
			{
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					result = Enumerable.ToArray<string>(streamReader.ReadToEnd().Split('\n', 0));
				}
			}
			return result;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00008690 File Offset: 0x00006890
		private static string RandomWord()
		{
			int num = Placeholders.Random.Next(0, Placeholders.LatinWords.Length);
			return Placeholders.LatinWords[num];
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000086B8 File Offset: 0x000068B8
		private static string LongRandomWord()
		{
			int num = Placeholders.Random.Next(0, Placeholders.LongLatinWords.Length);
			return Placeholders.LongLatinWords[num];
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000086E0 File Offset: 0x000068E0
		private static string RandomWords(int min, int max)
		{
			int num = Placeholders.Random.Next(min, max + 1);
			IEnumerable<string> enumerable = Enumerable.Select<int, string>(Enumerable.Range(0, num), (int x) => Placeholders.RandomWord());
			return string.Join(" ", enumerable);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00008733 File Offset: 0x00006933
		public static string LoremIpsum()
		{
			return "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000873A File Offset: 0x0000693A
		public static string Label()
		{
			return Placeholders.RandomWords(2, 3).FirstCharToUpper();
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00008748 File Offset: 0x00006948
		public static string Sentence()
		{
			return Placeholders.RandomWords(6, 12).FirstCharToUpper() + ".";
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00008761 File Offset: 0x00006961
		public static string Question()
		{
			return Placeholders.RandomWords(4, 8).FirstCharToUpper() + "?";
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000877C File Offset: 0x0000697C
		public static string Paragraph()
		{
			int num = Placeholders.Random.Next(3, 6);
			IEnumerable<string> enumerable = Enumerable.Select<int, string>(Enumerable.Range(0, num), (int x) => Placeholders.Sentence());
			return string.Join(" ", enumerable);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x000087D0 File Offset: 0x000069D0
		public static string Paragraphs()
		{
			int num = Placeholders.Random.Next(2, 5);
			IEnumerable<string> enumerable = Enumerable.Select<int, string>(Enumerable.Range(0, num), (int x) => Placeholders.Paragraph());
			return string.Join("\n", enumerable);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00008824 File Offset: 0x00006A24
		public static string Email()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(5, 3);
			defaultInterpolatedStringHandler.AppendFormatted(Placeholders.LongRandomWord());
			defaultInterpolatedStringHandler.AppendFormatted<int>(Placeholders.Random.Next(10, 99));
			defaultInterpolatedStringHandler.AppendLiteral("@");
			defaultInterpolatedStringHandler.AppendFormatted(Placeholders.LongRandomWord());
			defaultInterpolatedStringHandler.AppendLiteral(".com");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00008886 File Offset: 0x00006A86
		public static string Name()
		{
			return Placeholders.LongRandomWord().FirstCharToUpper() + " " + Placeholders.LongRandomWord().FirstCharToUpper();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000088A8 File Offset: 0x00006AA8
		public static string PhoneNumber()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted<int>(Placeholders.Random.Next(100, 999));
			defaultInterpolatedStringHandler.AppendLiteral("-");
			defaultInterpolatedStringHandler.AppendFormatted<int>(Placeholders.Random.Next(100, 999));
			defaultInterpolatedStringHandler.AppendLiteral("-");
			defaultInterpolatedStringHandler.AppendFormatted<int>(Placeholders.Random.Next(1000, 9999));
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00008928 File Offset: 0x00006B28
		public static string WebpageUrl()
		{
			return "www." + Placeholders.LongRandomWord() + ".com";
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00008940 File Offset: 0x00006B40
		public static string Price()
		{
			decimal num = (decimal)Math.Round(Placeholders.Random.NextDouble() * 100.0) + 0.99m;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(num, "C");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000899C File Offset: 0x00006B9C
		private static string FirstCharToUpper(this string text)
		{
			return Enumerable.First<char>(text).ToString().ToUpper() + text.Substring(1);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000089C8 File Offset: 0x00006BC8
		private static DateTime RandomDate()
		{
			double num = (Placeholders.Random.NextDouble() - 0.5) * 1000.0;
			return System.DateTime.Now - TimeSpan.FromDays(num);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00008A04 File Offset: 0x00006C04
		public static string Time()
		{
			return Placeholders.RandomDate().ToString("T");
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00008A24 File Offset: 0x00006C24
		public static string ShortDate()
		{
			return Placeholders.RandomDate().ToString("d");
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00008A44 File Offset: 0x00006C44
		public static string LongDate()
		{
			return Placeholders.RandomDate().ToString("D");
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00008A64 File Offset: 0x00006C64
		public static string DateTime()
		{
			return Placeholders.RandomDate().ToString("G");
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00008A84 File Offset: 0x00006C84
		public static string Integer()
		{
			return Placeholders.Random.Next(0, 10000).ToString();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00008AAC File Offset: 0x00006CAC
		public static string Decimal()
		{
			return (Placeholders.Random.NextDouble() * (double)Placeholders.Random.Next(0, 100)).ToString("N2");
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00008AE0 File Offset: 0x00006CE0
		public static string Percent()
		{
			return (Placeholders.Random.NextDouble() * 100.0).ToString("N0") + "%";
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00008B18 File Offset: 0x00006D18
		public static Color BackgroundColor()
		{
			int num = Placeholders.Random.Next(0, Placeholders.BackgroundColors.Length);
			return Placeholders.BackgroundColors[num];
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00008B44 File Offset: 0x00006D44
		public static Color Color()
		{
			Color[] array = new Color[]
			{
				Colors.Red.Medium,
				Colors.Pink.Medium,
				Colors.Purple.Medium,
				Colors.DeepPurple.Medium,
				Colors.Indigo.Medium,
				Colors.Blue.Medium,
				Colors.LightBlue.Medium,
				Colors.Cyan.Medium,
				Colors.Teal.Medium,
				Colors.Green.Medium,
				Colors.LightGreen.Medium,
				Colors.Lime.Medium,
				Colors.Yellow.Medium,
				Colors.Amber.Medium,
				Colors.Orange.Medium,
				Colors.DeepOrange.Medium,
				Colors.Brown.Medium,
				Colors.Grey.Medium,
				Colors.BlueGrey.Medium
			};
			int num = Placeholders.Random.Next(0, array.Length);
			return array[num];
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00008C5D File Offset: 0x00006E5D
		public static byte[] Image(int width, int height)
		{
			return Placeholders.Image(new ImageSize(width, height));
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00008C6C File Offset: 0x00006E6C
		public static byte[] Image(ImageSize size)
		{
			size = Placeholders.<Image>g__LimitSize|33_1(size, 64);
			Color[] array = Enumerable.ToArray<Color>(Enumerable.Take<Color>(Enumerable.OrderBy<Color, int>(Placeholders.BackgroundColors, (Color _) => Placeholders.Random.Next()), 2));
			byte[] result;
			using (SkImage skImage = SkImage.GeneratePlaceholder(size.Width, size.Height, array[0], array[1]))
			{
				using (SkData encodedData = skImage.GetEncodedData())
				{
					result = encodedData.ToBytes();
				}
			}
			return result;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00008D24 File Offset: 0x00006F24
		[CompilerGenerated]
		internal static ImageSize <Image>g__LimitSize|33_1(ImageSize size, int maxSize = 64)
		{
			if (size.Width < maxSize && size.Height < maxSize)
			{
				return size;
			}
			if (size.Width <= size.Height)
			{
				return new ImageSize(maxSize * size.Width / size.Height, maxSize);
			}
			return new ImageSize(maxSize, maxSize * size.Height / size.Width);
		}

		// Token: 0x04000269 RID: 617
		public static readonly Random Random = new Random();

		// Token: 0x0400026A RID: 618
		private const string CommonParagraph = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

		// Token: 0x0400026B RID: 619
		private static readonly string[] LatinWords = Placeholders.LoadLatinWords();

		// Token: 0x0400026C RID: 620
		private static readonly string[] LongLatinWords = Enumerable.ToArray<string>(Enumerable.Where<string>(Placeholders.LatinWords, (string x) => x.Length > 8));

		// Token: 0x0400026D RID: 621
		private static readonly Color[] BackgroundColors = new Color[]
		{
			Colors.Red.Lighten3,
			Colors.Pink.Lighten3,
			Colors.Purple.Lighten3,
			Colors.DeepPurple.Lighten3,
			Colors.Indigo.Lighten3,
			Colors.Blue.Lighten3,
			Colors.LightBlue.Lighten3,
			Colors.Cyan.Lighten3,
			Colors.Teal.Lighten3,
			Colors.Green.Lighten3,
			Colors.LightGreen.Lighten3,
			Colors.Lime.Lighten3,
			Colors.Yellow.Lighten3,
			Colors.Amber.Lighten3,
			Colors.Orange.Lighten3,
			Colors.DeepOrange.Lighten3,
			Colors.Brown.Lighten3,
			Colors.Grey.Lighten3,
			Colors.BlueGrey.Lighten3
		};
	}
}
