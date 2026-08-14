using System;

namespace YamlDotNet.Helpers
{
	// Token: 0x020000BC RID: 188
	internal static class NumberExtensions
	{
		// Token: 0x06000460 RID: 1120 RVA: 0x0000E7C5 File Offset: 0x0000C9C5
		public static bool IsPowerOfTwo(this int value)
		{
			return (value & value - 1) == 0;
		}
	}
}
