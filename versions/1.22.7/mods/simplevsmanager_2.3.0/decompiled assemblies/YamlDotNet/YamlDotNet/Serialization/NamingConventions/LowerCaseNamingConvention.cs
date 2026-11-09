using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x0200008A RID: 138
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LowerCaseNamingConvention : INamingConvention
	{
		// Token: 0x0600030A RID: 778 RVA: 0x0000B325 File Offset: 0x00009525
		private LowerCaseNamingConvention()
		{
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000B32D File Offset: 0x0000952D
		public string Apply(string value)
		{
			return value.ToCamelCase().ToLower(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000B340 File Offset: 0x00009540
		public string Reverse(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			char c = char.ToUpperInvariant(value.get_Chars(0));
			return new ReadOnlySpan<char>(ref c) + value.Substring(1);
		}

		// Token: 0x040000D2 RID: 210
		public static readonly INamingConvention Instance = new LowerCaseNamingConvention();
	}
}
