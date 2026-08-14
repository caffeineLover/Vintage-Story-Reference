using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine
{
	// Token: 0x0200005F RID: 95
	public class SyntaxException : Exception
	{
		// Token: 0x060002B8 RID: 696 RVA: 0x000024AE File Offset: 0x000006AE
		[NullableContext(1)]
		public SyntaxException(string message) : base(message)
		{
		}
	}
}
