using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x02000101 RID: 257
	[NullableContext(1)]
	[Nullable(0)]
	internal static class StringExtensions
	{
		// Token: 0x0600059D RID: 1437 RVA: 0x0001459C File Offset: 0x0001279C
		private static string ToCamelOrPascalCase(string str, Func<char, char> firstLetterTransform)
		{
			string text = Regex.Replace(str, "([_\\-])(?<char>[a-z])", (Match match) => match.Groups["char"].Value.ToUpperInvariant(), 1);
			return firstLetterTransform.Invoke(text.get_Chars(0)).ToString() + text.Substring(1);
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x000145F6 File Offset: 0x000127F6
		public static string ToCamelCase(this string str)
		{
			return StringExtensions.ToCamelOrPascalCase(str, new Func<char, char>(char.ToLowerInvariant));
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001460A File Offset: 0x0001280A
		public static string ToPascalCase(this string str)
		{
			return StringExtensions.ToCamelOrPascalCase(str, new Func<char, char>(char.ToUpperInvariant));
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00014620 File Offset: 0x00012820
		public static string FromCamelCase(this string str, string separator)
		{
			str = char.ToLower(str.get_Chars(0)).ToString() + str.Substring(1);
			str = Regex.Replace(str.ToCamelCase(), "(?<char>[A-Z])", (Match match) => separator + match.Groups["char"].Value.ToLowerInvariant());
			return str;
		}
	}
}
