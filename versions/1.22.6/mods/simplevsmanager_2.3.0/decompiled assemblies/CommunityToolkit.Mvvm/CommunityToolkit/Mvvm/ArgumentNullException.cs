using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm
{
	// Token: 0x0200000D RID: 13
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class ArgumentNullException
	{
		// Token: 0x06000033 RID: 51 RVA: 0x00002AC1 File Offset: 0x00000CC1
		[MethodImpl(256)]
		public static void ThrowIfNull([NotNull] object argument, [CallerArgumentExpression("argument")] string paramName = null)
		{
			if (argument == null)
			{
				ArgumentNullException.Throw(paramName);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002ACC File Offset: 0x00000CCC
		[DoesNotReturn]
		private static void Throw(string paramName)
		{
			throw new ArgumentNullException(paramName);
		}

		// Token: 0x02000049 RID: 73
		[Nullable(0)]
		public static class For<T>
		{
			// Token: 0x060001C8 RID: 456 RVA: 0x0000811A File Offset: 0x0000631A
			[MethodImpl(256)]
			public static void ThrowIfNull([NotNull] T argument, [CallerArgumentExpression("argument")] string paramName = null)
			{
				if (argument == null)
				{
					ArgumentNullException.Throw(paramName);
				}
			}
		}
	}
}
