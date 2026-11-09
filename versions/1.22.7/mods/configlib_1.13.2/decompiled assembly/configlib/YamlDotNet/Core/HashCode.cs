using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001B1 RID: 433
	[NullableContext(2)]
	[Nullable(0)]
	internal static class HashCode
	{
		// Token: 0x06000967 RID: 2407 RVA: 0x0001F2FF File Offset: 0x0001D4FF
		public static int CombineHashCodes(int h1, int h2)
		{
			return (h1 << 5) + h1 ^ h2;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0001F308 File Offset: 0x0001D508
		public static int CombineHashCodes(int h1, object o2)
		{
			return HashCode.CombineHashCodes(h1, HashCode.GetHashCode(o2));
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0001F316 File Offset: 0x0001D516
		private static int GetHashCode(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}
	}
}
