using System;

namespace YamlDotNet.Helpers
{
	// Token: 0x02000197 RID: 407
	internal static class NumberExtensions
	{
		// Token: 0x06000896 RID: 2198 RVA: 0x0001C268 File Offset: 0x0001A468
		public static bool IsPowerOfTwo(this int value)
		{
			return (value & value - 1) == 0;
		}
	}
}
