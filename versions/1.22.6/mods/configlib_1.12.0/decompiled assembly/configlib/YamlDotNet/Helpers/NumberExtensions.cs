using System;

namespace YamlDotNet.Helpers
{
	// Token: 0x0200019B RID: 411
	internal static class NumberExtensions
	{
		// Token: 0x060008D2 RID: 2258 RVA: 0x0001CBB0 File Offset: 0x0001ADB0
		public static bool IsPowerOfTwo(this int value)
		{
			return (value & value - 1) == 0;
		}
	}
}
