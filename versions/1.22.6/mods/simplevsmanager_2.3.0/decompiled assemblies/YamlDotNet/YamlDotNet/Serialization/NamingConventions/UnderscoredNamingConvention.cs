using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x0200008D RID: 141
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class UnderscoredNamingConvention : INamingConvention
	{
		// Token: 0x06000316 RID: 790 RVA: 0x0000B3D5 File Offset: 0x000095D5
		[Obsolete("Use the Instance static field instead of creating new instances")]
		public UnderscoredNamingConvention()
		{
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000B3DD File Offset: 0x000095DD
		public string Apply(string value)
		{
			return value.FromCamelCase("_");
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000B3EC File Offset: 0x000095EC
		public string Reverse(string value)
		{
			return value.ToPascalCase();
		}

		// Token: 0x040000D5 RID: 213
		public static readonly INamingConvention Instance = new UnderscoredNamingConvention();
	}
}
