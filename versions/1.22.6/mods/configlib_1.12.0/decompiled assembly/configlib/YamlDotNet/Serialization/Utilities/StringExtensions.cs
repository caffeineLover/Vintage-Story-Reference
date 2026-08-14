using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x02000105 RID: 261
	[NullableContext(1)]
	[Nullable(0)]
	internal static class StringExtensions
	{
		// Token: 0x060005D9 RID: 1497 RVA: 0x00014EE4 File Offset: 0x000130E4
		private static string ToCamelOrPascalCase(string str, Func<char, char> firstLetterTransform)
		{
			string text = Regex.Replace(str, "([_\\-])(?<char>[a-z])", (Match match) => match.Groups["char"].Value.ToUpperInvariant(), 1);
			return firstLetterTransform.Invoke(text.get_Chars(0)).ToString() + text.Substring(1);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00014F3E File Offset: 0x0001313E
		public static string ToCamelCase(this string str)
		{
			return StringExtensions.ToCamelOrPascalCase(str, new Func<char, char>(char.ToLowerInvariant));
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00014F52 File Offset: 0x00013152
		public static string ToPascalCase(this string str)
		{
			return StringExtensions.ToCamelOrPascalCase(str, new Func<char, char>(char.ToUpperInvariant));
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00014F68 File Offset: 0x00013168
		public static string FromCamelCase(this string str, string separator)
		{
			str = char.ToLower(str.get_Chars(0)).ToString() + str.Substring(1);
			str = Regex.Replace(str.ToCamelCase(), "(?<char>[A-Z])", (Match match) => separator + match.Groups["char"].Value.ToLowerInvariant());
			return str;
		}
	}
}
