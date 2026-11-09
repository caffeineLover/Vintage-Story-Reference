using System;
using System.Runtime.CompilerServices;

namespace SimpleExpressionEngine
{
	// Token: 0x0200005B RID: 91
	public class SyntaxException : Exception
	{
		// Token: 0x0600027C RID: 636 RVA: 0x000025CE File Offset: 0x000007CE
		[NullableContext(1)]
		public SyntaxException(string message) : base(message)
		{
		}
	}
}
