using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000C9 RID: 201
	[NullableContext(2)]
	[Nullable(0)]
	internal static class HashCode
	{
		// Token: 0x0600050A RID: 1290 RVA: 0x000117D4 File Offset: 0x0000F9D4
		public static int CombineHashCodes(int h1, int h2)
		{
			return (h1 << 5) + h1 ^ h2;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000117DD File Offset: 0x0000F9DD
		public static int CombineHashCodes(int h1, object o2)
		{
			return HashCode.CombineHashCodes(h1, HashCode.GetHashCode(o2));
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x000117EB File Offset: 0x0000F9EB
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
