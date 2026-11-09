using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x0200001A RID: 26
	[NullableContext(1)]
	[Nullable(0)]
	public class MissingXPathException : Exception
	{
		// Token: 0x06000185 RID: 389 RVA: 0x0000C895 File Offset: 0x0000AA95
		public MissingXPathException()
		{
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000C89D File Offset: 0x0000AA9D
		public MissingXPathException(string message) : base(message)
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000C8A6 File Offset: 0x0000AAA6
		public MissingXPathException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
