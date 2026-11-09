using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NamingConventions
{
	// Token: 0x0200015E RID: 350
	public sealed class LowerCaseNamingConvention : INamingConvention
	{
		// Token: 0x06000711 RID: 1809 RVA: 0x0000412E File Offset: 0x0000232E
		private LowerCaseNamingConvention()
		{
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00018770 File Offset: 0x00016970
		[NullableContext(1)]
		public string Apply(string value)
		{
			return value.ToCamelCase().ToLower();
		}

		// Token: 0x04000307 RID: 775
		[Nullable(1)]
		public static readonly INamingConvention Instance = new LowerCaseNamingConvention();
	}
}
