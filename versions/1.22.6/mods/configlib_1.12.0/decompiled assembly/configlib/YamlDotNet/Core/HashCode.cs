using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001B5 RID: 437
	[NullableContext(2)]
	[Nullable(0)]
	internal static class HashCode
	{
		// Token: 0x060009A3 RID: 2467 RVA: 0x0001FC47 File Offset: 0x0001DE47
		public static int CombineHashCodes(int h1, int h2)
		{
			return (h1 << 5) + h1 ^ h2;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0001FC50 File Offset: 0x0001DE50
		public static int CombineHashCodes(int h1, object o2)
		{
			return HashCode.CombineHashCodes(h1, HashCode.GetHashCode(o2));
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0001FC5E File Offset: 0x0001DE5E
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
