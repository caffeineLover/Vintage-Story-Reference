using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x020000BF RID: 191
	internal static class ThrowHelper
	{
		// Token: 0x0600047E RID: 1150 RVA: 0x0000EB62 File Offset: 0x0000CD62
		[NullableContext(1)]
		[MethodImpl(8)]
		public static void ThrowArgumentOutOfRangeException(string paramName, string message)
		{
			throw new ArgumentOutOfRangeException(paramName, message);
		}
	}
}
