using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x020001A2 RID: 418
	internal static class ThrowHelper
	{
		// Token: 0x060008D5 RID: 2261 RVA: 0x0001C7E7 File Offset: 0x0001A9E7
		[NullableContext(1)]
		[MethodImpl(8)]
		public static void ThrowArgumentOutOfRangeException(string paramName, string message)
		{
			throw new ArgumentOutOfRangeException(paramName, message);
		}
	}
}
