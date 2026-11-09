using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x02000050 RID: 80
	[NullableContext(1)]
	[Nullable(0)]
	internal static class StringExtensions
	{
		// Token: 0x06000221 RID: 545 RVA: 0x0000716C File Offset: 0x0000536C
		private static string ToCamelOrPascalCase(string str, Func<char, char> firstLetterTransform)
		{
			string text = Regex.Replace(str, "([_\\-])(?<char>[a-z])", (Match match) => match.Groups["char"].Value.ToUpperInvariant(), 1);
			char c = firstLetterTransform.Invoke(text.get_Chars(0));
			return new ReadOnlySpan<char>(ref c) + text.Substring(1);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000071CB File Offset: 0x000053CB
		public static string ToCamelCase(this string str)
		{
			Func<char, char> firstLetterTransform;
			if ((firstLetterTransform = StringExtensions.<>O.<0>__ToLowerInvariant) == null)
			{
				firstLetterTransform = (StringExtensions.<>O.<0>__ToLowerInvariant = new Func<char, char>(char.ToLowerInvariant));
			}
			return StringExtensions.ToCamelOrPascalCase(str, firstLetterTransform);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000071EE File Offset: 0x000053EE
		public static string ToPascalCase(this string str)
		{
			Func<char, char> firstLetterTransform;
			if ((firstLetterTransform = StringExtensions.<>O.<1>__ToUpperInvariant) == null)
			{
				firstLetterTransform = (StringExtensions.<>O.<1>__ToUpperInvariant = new Func<char, char>(char.ToUpperInvariant));
			}
			return StringExtensions.ToCamelOrPascalCase(str, firstLetterTransform);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00007214 File Offset: 0x00005414
		public static string FromCamelCase(this string str, string separator)
		{
			char c = char.ToLower(str.get_Chars(0), CultureInfo.InvariantCulture);
			str = new ReadOnlySpan<char>(ref c) + str.Substring(1);
			str = Regex.Replace(str.ToCamelCase(), "(?<char>[A-Z])", (Match match) => separator + match.Groups["char"].Value.ToLowerInvariant());
			return str;
		}

		// Token: 0x02000163 RID: 355
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040002C0 RID: 704
			[Nullable(0)]
			public static Func<char, char> <0>__ToLowerInvariant;

			// Token: 0x040002C1 RID: 705
			[Nullable(0)]
			public static Func<char, char> <1>__ToUpperInvariant;
		}
	}
}
