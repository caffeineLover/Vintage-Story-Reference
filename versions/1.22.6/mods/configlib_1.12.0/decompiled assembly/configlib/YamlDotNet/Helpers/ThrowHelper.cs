using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x020001A6 RID: 422
	internal static class ThrowHelper
	{
		// Token: 0x06000911 RID: 2321 RVA: 0x0001D12F File Offset: 0x0001B32F
		[NullableContext(1)]
		[MethodImpl(8)]
		public static void ThrowArgumentOutOfRangeException(string paramName, string message)
		{
			throw new ArgumentOutOfRangeException(paramName, message);
		}
	}
}
