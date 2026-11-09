using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x0200001B RID: 27
	[NullableContext(1)]
	[Nullable(0)]
	public class InvalidNodeReturnTypeException : Exception
	{
		// Token: 0x06000188 RID: 392 RVA: 0x0000C8B0 File Offset: 0x0000AAB0
		public InvalidNodeReturnTypeException(string message) : base(message)
		{
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000C8B9 File Offset: 0x0000AAB9
		public InvalidNodeReturnTypeException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
